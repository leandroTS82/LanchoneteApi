// Services/PedidoService.cs
using System.Data;
using Dapper;
using Microsoft.AspNetCore.Mvc;

public class PedidoService : IPedidoService
{
    private readonly IDbConnection _db;

    public PedidoService(IDbConnection db)
    {
        _db = db;
    }

    public async Task<IEnumerable<Pedido>> ObterPedidosAsync(int departmentId, DateTime? data, string? periodo)
    {
        string sqlQuery = @"
            SELECT p.Id AS Id, p.DepartamentoId, p.DataCriacao, p.Periodo, p.NomeCliente, p.Status AS StatusPedido, p.Pago, p.ValorTotal, p.TipoPagamento,
                pr.Id AS ProdutoId, pr.Descricao, pr.ValorUnitario, pr.Quantidade, pr.ValorTotal AS ValorTotalProduto
            FROM Pedidos p
            LEFT JOIN Produtos pr ON p.Id = pr.PedidoId
            WHERE p.DepartamentoId = @DepartmentId
            AND (@Data IS NULL OR DATE(p.DataCriacao) = DATE(@Data))
            AND (@Periodo IS NULL OR p.Periodo = @Periodo)
            ORDER BY p.ID DESC;
        ";

        var pedidosDictionary = new Dictionary<int, Pedido>();

        var pedidos = await _db.QueryAsync<Pedido, Produto, Pedido>(
            sqlQuery,
            (pedido, produto) =>
            {
                if (!pedidosDictionary.TryGetValue(pedido.Id, out var pedidoExistente))
                {
                    pedidoExistente = pedido;
                    pedidoExistente.Produtos = new List<Produto>();
                    pedidosDictionary[pedido.Id] = pedidoExistente;
                }

                if (produto != null && produto.ProdutoId != 0)
                {
                    pedidoExistente.Produtos.Add(produto);
                }

                return pedidoExistente;
            },
            splitOn: "ProdutoId",
            param: new { DepartmentId = departmentId, Data = data, Periodo = periodo }
        );

        return pedidosDictionary.Values.ToList();
    }

    public async Task<int> RegistrarPedido(Pedido pedido)
    {

        var dataCriacao = FormatDate.BrasiliaNow();

        var sqlPedido = "INSERT INTO Pedidos (DepartamentoId, DataCriacao, Periodo, NomeCliente, Status, Pago, ValorTotal, TipoPagamento) " +
                        "VALUES (@DepartamentoId, @DataCriacao, @Periodo, @NomeCliente, @StatusPedido, @Pago, @ValorTotal, @TipoPagamento); " +
                        "SELECT LAST_INSERT_ID();";
        var pedidoId = await _db.ExecuteScalarAsync<int>(sqlPedido, new
        {
            pedido.DepartamentoId,
            dataCriacao,
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
            await _db.ExecuteAsync(sqlProduto, new
            {
                produto.Descricao,
                produto.ValorUnitario,
                produto.Quantidade,
                produto.ValorTotal,
                PedidoId = pedido.Id // Use o ID do pedido inserido
            });
        }

        return pedidoId;
    }
}
