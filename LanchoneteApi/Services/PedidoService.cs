// Services/PedidoService.cs
using System.Data;
using Dapper;

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
            ORDER BY p.DataCriacao DESC;
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
}
