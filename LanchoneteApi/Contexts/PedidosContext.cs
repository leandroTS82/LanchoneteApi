using Microsoft.AspNetCore.Mvc;

public static class PedidosContext
{
    public static void MapPedidosEndpoints(WebApplication app)
    {
        app.MapGet("/pedidos", async ([FromServices] IPedidoService pedidoService, int departmentId, DateTime? data, string? periodo) =>
        {
            var pedidos = await pedidoService.ObterPedidosAsync(departmentId, data, periodo);
            return Results.Ok(pedidos);
        });

        app.MapPost("/pedidos", async ([FromServices] IPedidoService pedidoService, Pedido pedido) =>
        {
            var idPedido = await pedidoService.RegistrarPedido(pedido);
            return Results.Ok(new {PedidoId = idPedido});
        });
    }

}