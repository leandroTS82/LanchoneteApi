using System.Data;
using Dapper;
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
    }
}