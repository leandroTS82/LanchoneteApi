using MySql.Data.MySqlClient;
using System.Data;

var builder = WebApplication.CreateBuilder(args);

// Registre o serviço
builder.Services.AddScoped<IPedidoService, PedidoService>();
builder.Services.AddScoped<IDepartamentosServices, DepartamentosService>();

// Adicione a string de conexão do banco de dados
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Registre o IDbConnection no contêiner de serviços para injeção de dependência
builder.Services.AddTransient<IDbConnection>((sp) => new MySqlConnection(connectionString));

var app = builder.Build();
// Registra os endpoints dos diferentes contextos
DepartamentosContext.MapDepartamentosEndpoints(app);
PedidosContext.MapPedidosEndpoints(app);

app.Run();