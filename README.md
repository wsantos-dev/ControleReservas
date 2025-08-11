# Controle de Reservas

## Visão Geral

Este projeto é uma aplicação Fullstack desenvolvida com uma WebAPI **ASP.NET Core 9.0** no backend, e **ASP.NET Core MVC** no frontend. 
A aplicação implementa operações para criar, editar e cancelar uma Reserva conforme regras de negócio que serão exibidas abaixo.

---

## Tecnologias Utilizadas

- ASP.NET Core 9.0
- Entity Framework Core (EF Core 9.0)
- Padrões de Projetos: Repository, Unit of Work
- Arquitetura Limpa, seguindo princípios DDD (Domínio, Aplicação, Infraestrutura)
- SendGrid - API para envio de e-mails
- Injeção de Dependência nativa do ASP.NET Core
- Banco de Dados: SQL Server
- Swagger / OpenAPI para documentação
- XUnit para Testes Unitários.
---

## Regras de negócio


- Uma sala só pode ser reservada se não houver conflitos de horário com outras reservas.
- Uma reserva só pode ser cancelada com no mínimo 24 horas de antecedência.

## Estrutura do projeto

```
.
ControleReservas.sln
├──src
    ├── ControleReservas.API
    │   ├── Connected Services
    │   ├── Dependencies
    │   ├── Properties
    │   ├── Controllers
    │   ├── appsettings.json
    │   ├── ControleReservas.API.http
    │   ├── Program.cs
    ├── ControleReservas.Application
    │   ├── Dependencies
    │   ├── DTOs
    ├   ├    ├── ReservaCreateDto.cs
    ├   ├    ├── ReservaDto.cs
    ├   ├    ├── SalaDto.cs
    ├   ├    ├── UsuarioDto.cs
    ├   ├── Interfaces
    ├   ├    ├── IReservaService.cs
    ├   ├    ├── ISalaService.cs
    ├   ├    ├── IUsuarioService.cs
    ├   ├── Services
    ├   ├    ├── EmailService.cs
    ├   ├    ├── ReservaService.cs
    ├   ├    ├── SalaService.cs
    ├   ├    ├── UsuarioService.cs
    ├      
    └── ControleReservas.Domain
    ├   ├── Dependencies
    ├   ├── Entities
    ├       ├── DTOs
    ├       ├── ConfiguracoesEmail.cs
    ├       ├── Reserva.cs
    ├       ├── Sala.cs
    ├       ├── Usuario.cs
    |    ├── Enum
    ├       ├── ReservaStatus.cs
    |    ├── Exceptions
    ├       ├── CancelamentoExpiradoException.cs
    ├       ├── CapacidadeDaSalaExcedidaException.cs
    ├       ├── ReservaCancelamentoInvalidoException.cs
    ├       ├── ReservaConflitoHorarioException.cs
    ├       ├── ReservaDataInvalidaException.cs
    ├       ├── ReservaInexistenteException.cs
    |       ├── SalaComReservasExistenteException
    ├       ├── SalaNaoEncontradaException.cs
    ├       ├── SalaNomeDuplicadoException.cs
    |       ├── UsuarioComReservasExistenteException.cs
    ├       ├── UsuarioEmailDuplicadoException.cs
    ├       ├── UsuarioNaoEncontradoException.cs
    └── Interfaces
    ├   ├── IConfiguracoesEmailRepository.cs
    ├   ├── IEmailService.cs
    ├   ├── IReservaRepository.cs
    ├   ├── IRespository.cs
    ├   ├── ISalaRepository.cs
    ├   ├── IUnitOfWork.cs
    ├   ├── IUsuarioRepository.cs
└── ControleReservas.Infrastructure
    ├── Dependencies
    ├── Migrations
    ├   ├── 20250806045437_CriarTabelas.cs
    ├   ├── 20250807170104_AlteracaoDeProriedadesEntidadeReserva.cs
    ├   ├── 20250807181605_IncluiIndicesTabelaReservas.cs
    ├   ├── 20250807182051_PopularBancoDados.cs
    ├   ├── 20250808104507_ConfiguracoesEmail_Nova.cs
    ├   ├── ControleReservasDbContextModelSnapshot.cs
    ├── Persistence
    ├    ├── ControleReservasDbContext.cs
    ├    ├── ControleReservasDbContextFactory.cs
    ├── Repositories
    ├    ├── ConfiguracoesEmailRepository.cs
    ├    ├── Repository.cs
    ├    ├── ReservaRepository.cs
    ├    ├── SalaRepository.cs
    ├    ├── UnitOfWork.cs
    ├    ├── UsuarioRepository.cs

└── ControleReservas.MVC
├   ├── Connected Services
├   ├── Dependences
├   ├── Properties
├   ├── wwwroot
├   └── Controllers
|     ├── HomeController.cs
|     ├── ReservasController.cs
|     ├── SalaController.cs
|     ├── UsuarioController.cs
|   └── Controllers
|        ├── ErrorViewModel.cs
|        ├── ReservaCreateViewModel.cs
|        ├── ReservaViewModel.cs
|        ├── SalaViewModel.cs
|        ├── UsuarioViewModel.cs
|   └── Services
|        ├── IReservaApiService.cs
|        ├── ISalaApiService.cs
|        ├── IUsuarioApiService.cs
|        ├── ReservaApiService.cs
|        ├── SalaApiService.cs
|        |__ UsuarioApiService.cs
|   └── Views
|        ├── Home
|        ├── Reservas
|        ├── Shared
|            ├── _ViewImports
|            ├── _ViewStart
|        appsettings.json
|        Program.cs
├──tests
|  ├── ControleReservas.Tests
|      ├── ReservaServiceTests.cs




```

## Como Executar

### Pré-requisitos

- .NET 9 SDK instalado
  
- Banco de dados SQL Server: Segue abaixo o link para download:

```bash
https://www.microsoft.com/pt-br/sql-server/sql-server-downloads
```

- Siga o guia de instalação do SQL Server 2022:

```bash
https://learn.microsoft.com/pt-br/sql/database-engine/install-windows/install-sql-server?view=sql-server-ver17   
```

- Instale um aplicativo para gerenciar o banco de dados ControleReservas. Como sugestão, indico o SQL Server Management Studio. Segue abaixo os links para download e configurações.

```bash
https://learn.microsoft.com/pt-br/ssms/install/install
```

- Em relação a dúvidas sobre como conectar e realizar consultas, siga o link abaixo:

```bash
https://learn.microsoft.com/pt-br/ssms/quickstarts/ssms-connect-query-sql-server?view=sql-server-ver16&tabs=modern
```


- Realize a restauração do banco de dados **ControleReservas.bak**, incluído na raiz do projeto.
- Você pode encontrar instruções para restauração de um banco de dados SQL Server no link abaixo:
  ```html
      https://learn.microsoft.com/pt-br/sql/relational-databases/backup-restore/quickstart-backup-restore-database?view=sql-server-ver17&tabs=ssms
  ```
Obs: A necessidade de usar o backup do banco de dados, é que não ele possui uma tabela com informações como credenciais para acesso a API de envio de e-mail. 
Sem isso, o serviço de e-mail não funcionará. Também não foi possível armazenar essa informação no código para ser executadas as migrations, porquê a chave seria excluída do meu perfil do SendGrid por motivos de segurança.

- Uma vez, que o SQL Server estiver instalado e configurado conforme instruções acima na sua máquina local, abra um documento .sql no caminho (Arquivo -> Nova Consulta) ou (File -> New Query) e execute o script T-SQL abaixo para criar o usuário do banco de dados. 

- Usuário: desenvolvedor | Senha: DotNet@2025 conforme abaixo:
- 
  
     ```tsql
       USE ControleReservas
       GO
        -- Cria o login no servidor
      CREATE LOGIN [desenvolvedor] WITH PASSWORD = N'DotNet@2025';
      GO
      
      -- Adiciona o login à role 'sysadmin' para conceder permissões totais
      ALTER SERVER ROLE [sysadmin] ADD MEMBER [desenvolvedor];
      GO
     ```
    
### Próximos passos

1. Clone o repositório:
   ```bash
   git clone https://github.com/seu-usuario/seu-repositorio.git

2. Verifique o arquivo appsettings.json do do projeto ControleReservas.API e modifique a string de conexão conforme
   o seu ambiente:

```bash
    "ControleReservasConnection": "Server=localhost\\SQLEXPRESS;Database=ControleReservas;User Id=desenvolvedor;Password=DotNet@2025;TrustServerCertificate=True;"
```


3. Navege até o diretório:
```bash
cd Desafio-FSBR
   ```
3. Execute o comando abaixo:
```bash
 dotnet build
```
4. Uma vez compilado com sucesso, navege até o diretório ControleReservas.API e execute o comando:
```bash
  dotnet run
```
5. Para acessar a API com o Swagger digite em seu navegador:
```bash
   http://localhost:5089/swagger/index.html
```
6. Abra outro terminal e navegue até o diretório ControleReservas.MVC e execute o comando:
```bash
  dotnet run
```
7. Para acessar a aplicação digite em seu navegador:
```bash
  http://localhost:7149
```
8. Para rodar os testes unitários, siga essas instruções:

    No Visual Studio:
    Vá em Test > Test Explorer.

    O Test Explorer irá exibir todos os testes do seu projeto.

   Você pode clicar em Run All para rodar todos os testes, ou selecionar testes específicos e clicar em Run. A interface é      bem intuitiva e mostra os resultados de forma visual.
