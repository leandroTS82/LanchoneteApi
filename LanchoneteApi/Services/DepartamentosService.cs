
using System.Data;
using Dapper;

public class DepartamentosService : IDepartamentosServices
{
    private readonly IDbConnection _db;

    public DepartamentosService(IDbConnection db)
    {
        _db = db;
    }
    public async Task<List<Departamento>> ObterDepartamentosAsync()
    {
        var sql = "SELECT * FROM Departamentos";
        var departamentos = await _db.QueryAsync<Departamento>(sql);
        return departamentos.ToList();
    }

    public async Task<int> RegistrarDepartamento(Departamento departamento)
    {
        var sql = "INSERT INTO Departamentos (Nome, PixResponsavel, Ativo) VALUES (@Nome, @PixResponsavel, @Ativo)";
        var result = await _db.ExecuteAsync(sql, departamento);
        return result;
    }
}