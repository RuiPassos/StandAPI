# StandAPI

API REST em C# / ASP.NET Core para gestão dos veículos de um stand automóvel. Permite listar, consultar, criar, atualizar e eliminar veículos, com persistência em SQLite.

## Tecnologias

- [.NET 10](https://dotnet.microsoft.com/) — ASP.NET Core Web API
- [SQLite](https://www.sqlite.org/) via `Microsoft.Data.Sqlite`
- [Swagger / OpenAPI](https://swagger.io/) via `Swashbuckle.AspNetCore`

## Arquitetura

O projeto está organizado em camadas, cada uma com uma responsabilidade única:

```
Controller  →  Service  →  Repository  →  SQLite
(HTTP)         (regras de negócio)  (acesso a dados)
```

```
StandAPI/
├── Controllers/       # Endpoints da API — só lida com HTTP (rotas, status codes)
├── Services/           # Regras de negócio, orquestra chamadas ao Repository
├── Models/             # Entidade Veiculo (com validação nos setters)
├── Repositories/       # Acesso a dados (SQLite)
├── Program.cs           # Configuração da app e criação do schema da BD
├── StandAPI.http         # Pedidos de exemplo para testar a API
└── StandAPI.csproj
```

## Modelo `Veiculo`

| Campo       | Tipo          | Regras                                                                 |
|-------------|---------------|-------------------------------------------------------------------------|
| `id`        | `int`         | Atribuído automaticamente pela base de dados                            |
| `matricula` | `string`      | Formato português (ex.: `AA-11-BB`, `11-AA-11`), única                  |
| `marca`     | `string`      | Não pode ser nula ou vazia                                              |
| `modelo`    | `string`      | Não pode ser nulo ou vazio                                              |
| `peso`      | `double`      | Não pode ser negativo                                                   |
| `comb`      | `Combustivel` | `0` = Gasolina, `1` = Gasóleo, `2` = Elétrico                           |

## Endpoints

| Método   | Rota                   | Descrição                              |
|----------|------------------------|-----------------------------------------|
| `GET`    | `/api/veiculos`        | Lista todos os veículos                 |
| `GET`    | `/api/veiculos/{id}`   | Obtém um veículo pelo id                |
| `POST`   | `/api/veiculos`        | Cria um novo veículo                    |
| `PUT`    | `/api/veiculos/{id}`   | Atualiza um veículo existente           |
| `DELETE` | `/api/veiculos/{id}`   | Elimina um veículo                      |

## Como clonar e testar

### 1. Pré-requisitos

- [.NET 10 SDK](https://dotnet.microsoft.com/download) instalado (`dotnet --version` deve reportar 10.x)

### 2. Clonar o repositório

```bash
git clone https://github.com/<utilizador>/StandAPI.git
cd StandAPI
```

### 3. Correr a API

```bash
cd StandAPI
dotnet restore
dotnet run
```

A base de dados (`stand.db`) e a respetiva tabela são criadas automaticamente na primeira execução — não é preciso nenhum passo manual de setup.

Por omissão a API fica disponível em:

- `http://localhost:5163`
- Swagger UI: `http://localhost:5163/swagger`

### 4. Testar os endpoints

**Opção A — Swagger UI**

Abrir `http://localhost:5163/swagger` no browser e experimentar os endpoints diretamente.

**Opção B — ficheiro `StandAPI.http`**

O ficheiro `StandAPI/StandAPI.http` tem pedidos de exemplo prontos a usar (compatível com o plugin *REST Client* do VS Code ou o cliente HTTP nativo do Rider/Visual Studio). Basta abrir o ficheiro e clicar em "Send Request" em cada pedido.

**Opção C — curl**

```bash
# Listar todos os veículos
curl http://localhost:5163/api/veiculos

# Criar um veículo
curl -X POST http://localhost:5163/api/veiculos \
  -H "Content-Type: application/json" \
  -d '{"matricula":"AA-11-BB","marca":"Toyota","modelo":"Corolla","peso":1200,"comb":0}'

# Obter um veículo por id
curl http://localhost:5163/api/veiculos/1

# Atualizar um veículo
curl -X PUT http://localhost:5163/api/veiculos/1 \
  -H "Content-Type: application/json" \
  -d '{"matricula":"AA-11-BB","marca":"Toyota","modelo":"Corolla","peso":1250,"comb":0}'

# Eliminar um veículo
curl -X DELETE http://localhost:5163/api/veiculos/1
```
