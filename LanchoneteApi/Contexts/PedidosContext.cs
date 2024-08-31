using System.Data;
using Dapper;

public static class PedidosContext
{
    public static void MapPedidosEndpoints(WebApplication app)
    {
        app.MapGet("/pedidos", async (IDbConnection db, int departmentId, DateTime? data, string? periodo) =>
{
    string sqlQuery = @"
        SELECT p.Id AS PedidoId, p.DepartamentoId, p.DataCriacao, p.Periodo, p.NomeCliente, p.Status, p.Pago, p.ValorTotal, p.TipoPagamento,
       pr.Id AS ProdutoId, pr.Descricao, pr.ValorUnitario, pr.Quantidade, pr.ValorTotal AS ValorTotalProduto
FROM Pedidos p
LEFT JOIN Produtos pr ON p.Id = pr.PedidoId
WHERE p.DepartamentoId = @DepartmentId
AND (@Data IS NULL OR DATE(p.DataCriacao) = DATE(@Data))
AND (@Periodo IS NULL OR p.Periodo = @Periodo)
ORDER BY p.DataCriacao DESC;
";

    var pedidos = new Dictionary<int, Pedido>();

    await db.QueryAsync<Pedido, Produto, Pedido>(
        sqlQuery,
        (pedido, produto) =>
        {
            if (!pedidos.TryGetValue(pedido.Id, out var pedidoExistente))
            {
                pedidoExistente = pedido;
                pedidoExistente.Produtos = new List<Produto>();
                pedidos[pedido.Id] = pedidoExistente;
            }

            if (produto != null)
            {
                pedidoExistente.Produtos.Add(produto);
            }

            return pedidoExistente;
        },
        splitOn: "ProdutoId",
        param: new { DepartmentId = departmentId, Data = data, Periodo = periodo }
    );

    return Results.Ok(pedidos.Values);
});






        app.MapPost("/pedidos", async (Pedido pedido, IDbConnection db) =>
        {
            // SQL para inserir o pedido e retornar o ID gerado
            var sqlPedido = "INSERT INTO Pedidos (DepartamentoId, DataCriacao, Periodo, NomeCliente, Status, Pago, ValorTotal, TipoPagamento) " +
                            "VALUES (@DepartamentoId, @DataCriacao, @Periodo, @NomeCliente, @StatusPedido, @Pago, @ValorTotal, @TipoPagamento); " +
                            "SELECT LAST_INSERT_ID();"; // Obtenha o ID gerado

            // Executa o comando e captura o ID do pedido inserido
            var pedidoId = await db.ExecuteScalarAsync<int>(sqlPedido, new
            {
                pedido.DepartamentoId,
                pedido.DataCriacao,
                pedido.Periodo,
                pedido.NomeCliente,
                pedido.StatusPedido,
                pedido.Pago,
                pedido.ValorTotal,
                pedido.TipoPagamento
            });

            // Atribua o ID ao pedido
            pedido.Id = pedidoId;

            // Inserir produtos associados ao pedido
            foreach (var produto in pedido.Produtos)
            {
                var sqlProduto = "INSERT INTO Produtos (Descricao, ValorUnitario, Quantidade, ValorTotal, PedidoId) " +
                                 "VALUES (@Descricao, @ValorUnitario, @Quantidade, @ValorTotal, @PedidoId)";
                await db.ExecuteAsync(sqlProduto, new
                {
                    produto.Descricao,
                    produto.ValorUnitario,
                    produto.Quantidade,
                    produto.ValorTotal,
                    PedidoId = pedido.Id // Use o ID do pedido inserido
                });
            }

            return Results.Ok(new { PedidoId = pedidoId });
        });
    }
}