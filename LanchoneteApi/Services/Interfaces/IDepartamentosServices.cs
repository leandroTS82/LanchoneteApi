public interface IDepartamentosServices
{
    Task<List<Departamento>> ObterDepartamentosAsync();
    Task<int> RegistrarDepartamento(Departamento departamento);
}