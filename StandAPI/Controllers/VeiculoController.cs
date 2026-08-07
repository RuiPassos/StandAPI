using Microsoft.AspNetCore.Mvc;
using StandAPI.Models;
using Microsoft.Data.Sqlite;

namespace StandAPI.Controllers;

[ApiController]
[Route("api/veiculos")] // O endereço base vai ser /api/veiculos
public class VeiculosController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public VeiculosController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    // O equivalente ao botão "Listar"
    [HttpGet]
    public IActionResult ListarTodos()
    {
        var listaDeVeiculos = new List<Veiculo>();

        string connectionString = _configuration.GetConnectionString("DefaultConnection");

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            var createCommand = connection.CreateCommand();
            createCommand.CommandText = "Select * From veiculos";

            using (var reader = createCommand.ExecuteReader())
            {
                while (reader.Read())
                {
                    var veiculo = new Veiculo(
                        reader.GetString(0),
                        reader.GetString(1),
                        reader.GetString(2),
                        reader.GetDouble(3),
                        (Combustivel)reader.GetInt32(4)
                    );
                    listaDeVeiculos.Add(veiculo);
                }
            }
        }

        return Ok(listaDeVeiculos); // O Ok() transforma logo a lista em JSON!
    }

    // O equivalente ao botão "Adicionar"
    [HttpPost]
    public IActionResult AdicionarNovo(Veiculo novoVeiculo)
    {
        string connectionString = _configuration.GetConnectionString("DefaultConnection");

        using (var connection = new SqliteConnection(connectionString))
        {
            try
            {
                connection.Open();

                var createCommand = connection.CreateCommand();
                createCommand.CommandText =
                    "INSERT INTO veiculos (matricula, marca, modelo, peso, combustivel) VALUES (@matricula, @marca, @modelo, @peso, @combustivel)";
                createCommand.Parameters.AddWithValue("@matricula", novoVeiculo.Matricula);
                createCommand.Parameters.AddWithValue("@marca", novoVeiculo.Marca);
                createCommand.Parameters.AddWithValue("@modelo", novoVeiculo.Modelo);
                createCommand.Parameters.AddWithValue("@peso", novoVeiculo.Peso);
                createCommand.Parameters.AddWithValue("@combustivel", (int)novoVeiculo.Comb);
                createCommand.ExecuteNonQuery();

                return CreatedAtAction(nameof(ListarTodos), new { matricula = novoVeiculo.Matricula }, novoVeiculo);
            }
            catch (SqliteException e)
            {
                if (e.SqliteErrorCode == 19)
                {
                    return Conflict(
                        "Erro ao adicionar o veículo: Já existe um veículo com a mesma matrícula na base de dados!");
                }

                return StatusCode(500, "Erro ao adicionar o veículo: " + e.Message);
            }
        }
    }

    [HttpPut("{matricula}")]
    public IActionResult Atualizar(string matricula, string marca, string modelo, double peso, int combustivel)
        {
            try
            {
                string connectionString = _configuration.GetConnectionString("DefaultConnection");

                using (var connection = new SqliteConnection(connectionString))
                {
                    connection.Open();

                    var updateCommand = connection.CreateCommand();
                    updateCommand.CommandText =
                        "UPDATE veiculos SET marca = @marca, modelo = @modelo, peso = @peso, combustivel = @combustivel WHERE matricula = @matricula";
                    if (!string.IsNullOrEmpty(matricula))
                    {
                        updateCommand.Parameters.AddWithValue("@matricula", matricula);
                    }

                    if (!string.IsNullOrEmpty(marca))
                    {
                        updateCommand.Parameters.AddWithValue("@marca", marca);
                    }

                    if (!string.IsNullOrEmpty(modelo))
                    {
                        updateCommand.Parameters.AddWithValue("@modelo", modelo);
                    }

                    if (peso > 0)
                    {
                        updateCommand.Parameters.AddWithValue("@peso", peso);
                    }

                    if (combustivel >= 0 && combustivel < 3)
                    {
                        updateCommand.Parameters.AddWithValue("@combustivel", combustivel);
                    }

                    updateCommand.ExecuteNonQuery();
                    return Ok();
                }
            }
            catch (SqliteException e)
            {
                if (e.SqliteErrorCode == 19)
                {
                    return Conflict();
                }
                return StatusCode(500, e.Message);
            }
        }

        [HttpDelete("{matricula}")]
        public IActionResult Eliminar(string matricula)
        {
            try
            {
                string connectionString = _configuration.GetConnectionString("DefaultConnection");

                using (var connection = new SqliteConnection(connectionString))
                {
                    connection.Open();

                    var updateCommand = connection.CreateCommand();
                    updateCommand.CommandText = "DELETE FROM veiculos WHERE matricula = @matricula";
                        
                    if (!string.IsNullOrEmpty(matricula))
                    {
                        updateCommand.Parameters.AddWithValue("@matricula", matricula);
                    }

                    updateCommand.ExecuteNonQuery();
                    return NoContent();
                }
            }
            catch (SqliteException e)
            {
                if (e.SqliteErrorCode == 19)
                {
                    return Conflict();
                }
                return StatusCode(500, e.Message);
            }
        }
}
