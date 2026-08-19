using Microsoft.Data.Sqlite;
using StandAPI.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Caminho absoluto para o ficheiro da BD, independente do diretório a partir do qual a app é lançada.
// stand.db não é versionado (ver .gitignore), por isso o schema é garantido em runtime mais abaixo.
var dbPath = Path.Combine(builder.Environment.ContentRootPath, "stand.db");
builder.Configuration["ConnectionStrings:DefaultConnection"] = $"Data Source={dbPath}";

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddScoped<IVeiculoRepository, VeiculoRepository>();

var app = builder.Build();

// Cria a tabela caso ainda não exista (primeira execução após um clone, onde stand.db não existe).
using (var connection = new SqliteConnection(app.Configuration.GetConnectionString("DefaultConnection")))
{
    connection.Open();
    var command = connection.CreateCommand();
    command.CommandText = """
        CREATE TABLE IF NOT EXISTS veiculos (
            id          INTEGER PRIMARY KEY AUTOINCREMENT,
            matricula   VARCHAR(8) UNIQUE,
            marca       VARCHAR(200),
            modelo      VARCHAR(200),
            peso        REAL,
            combustivel INTEGER,
            created_at  REAL DEFAULT (strftime('%s', 'now'))
        );
        """;
    command.ExecuteNonQuery();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();

