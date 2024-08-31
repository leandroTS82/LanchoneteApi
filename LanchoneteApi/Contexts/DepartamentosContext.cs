using System.Data;
using Dapper;

public static class DepartamentosContext
{
    public static void MapDepartamentosEndpoints(WebApplication app)
    {
        app.MapGet("/departamentos", async (IDbConnection db) =>
        {
            var sql = "SELECT * FROM Departamentos";
            var departamentos = await db.QueryAsync(sql);
            return Results.Ok(departamentos);
        });

        app.MapPost("/departamentos", async (IDbConnection db, Departamento departamento) =>
        {
            var sql = "INSERT INTO Departamentos (Nome, PixResponsavel, Ativo) VALUES (@Nome, @PixResponsavel, @Ativo)";
            var result = await db.ExecuteAsync(sql, departamento);
            return result > 0 ? Results.Ok("Departamento criado com sucesso.") : Results.Problem("Erro ao criar departamento.");
        });

        // Outros endpoints de departamentos...
    }
}