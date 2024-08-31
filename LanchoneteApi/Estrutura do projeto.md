Para separar os endpoints da sua Minimal API em .NET 6 por contexto, você pode organizar seu código usando classes estáticas para cada contexto. 
Cada classe será responsável por registrar os endpoints relacionados ao seu domínio específico, como Departamentos, Pedidos, Produtos, etc. 
Isso ajuda a manter o código limpo e organizado.

Estrutura do Projeto
Primeiro, vamos criar uma estrutura de pastas para os diferentes contextos:

/SeuProjeto
  /Contexts
    /DepartamentosContext.cs
    /PedidosContext.cs
    /ProdutosContext.cs
  Program.cs

## Atualização da estrutura do projeto

Desenho de exemplo considerando contexto de pedidos

/MyMinimalApiProject
│
├── /Controllers
│   └── PedidosContext.cs            # Mapeia os endpoints para pedidos.
│
├── /Services
│   ├── IPedidoService.cs            # Interface para o serviço de pedidos.
│   └── PedidoService.cs             # Implementação do serviço de pedidos.
│
├── /Models
│   ├── Pedido.cs                    # Modelo de dados para Pedido.
│   ├── Produto.cs                   # Modelo de dados para Produto.
│   └── Departamento.cs              # Modelo de dados para Departamento.
│
├── /Data
│   └── IDbConnectionFactory.cs      # Interface para criar conexões com o banco de dados (opcional).
│
├── /Configurations
│   └── DatabaseConfiguration.cs     # Configuração para strings de conexão e outros parâmetros de banco de dados (opcional).
│
├── appsettings.json                 # Arquivo de configuração para strings de conexão e outras configurações.
├── Program.cs                       # Ponto de entrada da aplicação que configura a Minimal API e injeta serviços.
├── MyMinimalApiProject.csproj       # Arquivo de projeto do .NET.
└── README.md                        # Documentação do projeto.
