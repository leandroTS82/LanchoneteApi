using Microsoft.AspNetCore.Mvc;

public static class DepartamentosContext
{
    public static void MapDepartamentosEndpoints(WebApplication app)
    {
        app.MapGet("/departamentos", async ([FromServices] IDepartamentosServices departamentosServices ) =>
        {
            var departamentos = await departamentosServices.ObterDepartamentosAsync();
            return Results.Ok(departamentos);
        });

        app.MapPost("/departamentos", async ([FromServices] IDepartamentosServices departamentosServices, Departamento departamento) =>
        {
            var result = await departamentosServices.RegistrarDepartamento(departamento);
            return result > 0 ? Results.Ok("Departamento criado com sucesso.") : Results.Problem("Erro ao criar departamento.");
        });
    }
}