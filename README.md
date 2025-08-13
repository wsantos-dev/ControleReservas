# Controle de Reservas

## Visão Geral

Este projeto é uma aplicação Fullstack desenvolvida com uma WebAPI **ASP.NET Core 9.0** no backend, e **ASP.NET Core MVC** no frontend. 

**Domínio**:

Um empresa de coworking deseja um sistema para gerenciar as reservas de suas salas de reunião.  

Deve ser possível, criar, editar e cancelar reservas, conforme regras de negócio que serão exibidas abaixo.

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
- XUnit e Moq para Testes Unitários.
- Docker
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


### Pré-requisitos

Ter o git instaldo em sua máquina. (Para realizar o download acesse https://git-scm.com/downloads, e nele você encontrará informações sobre a instalação)

Após ter o git instalado, instale o Docker Desktop conforme sua plataforma. Abaixo temos o link com o passo a passo da instalação. (Caso não tenha instalado em sua máquina):

- **Windows:** https://docs.docker.com/desktop/setup/install/windows-install/
- **Linux:** https://docs.docker.com/desktop/setup/install/linux/
- **MacOS:** https://docs.docker.com/desktop/setup/install/mac-install/

---

## ⚙️ Configuração Inicial

- Após o docker estar instalado e funcional em sua máquina, acesse um terminal de sua preferência e execute os comando abaixos na sequência:

Crie uma rede para o docker:

```bash
docker network create dev_network
```

Crie o volume para o SQL Server 2022:
```bash
docker volume create controle-reservas-db
```

Crie o container do SQL Server 2022:
> ⚠️ Para Linux/MacOS substitua os acentos graves (\`) por barras invertidas (`\`)

```powershell
docker run `
  -e "ACCEPT_EULA=Y" `
  -e "SA_PASSWORD=DotNet@2025" `
  -e "MSSQL_PID=Express" `
  -p 1433:1433 `
  --name container-sqlserver `
  --network dev_network `
  -v controle-reservas-db:/var/opt/mssql `
  -d mcr.microsoft.com/mssql/server:2022-latest
```

Clone o repositório num diretório raiz de sua preferência:

```bash
git clone https://github.com/wsantos-dev/Desafio-FSBR.git
```

Se usar SSH:
```bash
git clone git@github.com:wsantos-dev/Desafio-FSBR.git
```

Navege até o diretório:
```bash
cd Desafio-FSBR
```

Crie a pasta de backup no container (se ainda não tiver)
```bash
docker exec -it container-sqlserver mkdir /var/opt/mssql/backup
```

Copie o arquivo .bak (Que está na raiz do projeto) do host para dentro do container:
```bash
docker cp ControleReservas.bak container-sqlserver:/var/opt/mssql/backup/ControleReservas.bak    
```

## Próximos passos


1. Execute o comando abaixo (Aguarde a construção da imagem e do contâiner):
```bash
 docker-compose up --build
```
2. Após o comando acima executar com sucesso, você pode acessar a Web API com o Swagger, digite em seu navegador:
```bash
   http://localhost:5089/swagger/index.html
```
3. Para acessar a aplicação MVC digite em seu navegador:
```bash
  http://localhost:7149
```
4. Para rodar os testes unitários, siga essas instruções:

    No Visual Studio:
    Vá em Test > Test Explorer.

    O Test Explorer irá exibir todos os testes do seu projeto.

   Você pode clicar em Run All para rodar todos os testes, ou selecionar testes específicos e clicar em Run. A interface é      bem intuitiva e mostra os resultados de forma visual.
