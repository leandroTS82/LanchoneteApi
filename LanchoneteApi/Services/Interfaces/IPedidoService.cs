// Services/IPedidoService.cs
public interface IPedidoService
{
    Task<IEnumerable<Pedido>> ObterPedidosAsync(int departmentId, DateTime? data, string? periodo);
}
