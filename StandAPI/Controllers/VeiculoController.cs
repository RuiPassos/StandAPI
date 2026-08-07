using Microsoft.AspNetCore.Mvc;
using StandAPI.Models;
using Microsoft.Data.Sqlite;

namespace StandAPI.Controllers;

[ApiController]
[Route("api/veiculos")] // O endereço base vai ser /api/veiculos
public class VeiculosController : ControllerBase
{
    // A nossa "Base de Dados" falsa por agora (estática para não se perder a cada request)
    public static List<Veiculo> baseDeDadosDeVeiculos = new List<Veiculo>
    {
        new Veiculo("HH-43-KM", "BMW", "320", 1500, Combustivel.Gasolina),
        new Veiculo("MH-KJ-85", "BMW", "550", 2100, Combustivel.Gasoleo)

    };

    // O equivalente ao botão "Listar"
    [HttpGet]
    public IActionResult ListarTodos()
    {
        var listaDeVeiculos = new List<Veiculo>();

        string connectionString = "Data source=/home/sword/Workspace/StandAPI/StandAPI/stand.db";

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
                        reader.GetInt32(3),
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
        string connectionString = "Data source=/home/sword/Workspace/StandAPI/StandAPI/stand.db";

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

    [ApiController]
    [Route("api/veiculos/{matricula}")] // O endereço base vai ser /api/veiculos
    public class VeiculoController : ControllerBase
    {
        [HttpPut]
        public IActionResult Atualizar(Veiculo veiculo)
        {
            if (!string.IsNullOrEmpty(veiculo.Matricula))
            {
                var veiculoExistente =
                    VeiculosController.baseDeDadosDeVeiculos.FirstOrDefault(v => v.Matricula == veiculo.Matricula);
                if (veiculoExistente != null)
                {
                    veiculoExistente.Marca = veiculo.Marca;
                    veiculoExistente.Modelo = veiculo.Modelo;
                    veiculoExistente.Peso = veiculo.Peso;
                    veiculoExistente.Comb = veiculo.Comb;

                    return Ok(veiculoExistente);
                }
                else
                {
                    return NotFound("Veículo não encontrado na base de dados!");
                }
            }
            else
            {
                return BadRequest("Matrícula inválida!");
            }
        }

        [HttpDelete]
        public IActionResult Eliminar(string matricula)
        {
            var veiculoExistente =
                VeiculosController.baseDeDadosDeVeiculos.FirstOrDefault(v => v.Matricula == matricula);
            if (veiculoExistente != null)
            {
                VeiculosController.baseDeDadosDeVeiculos.Remove(veiculoExistente);
                return NoContent();
            }
            else
            {
                return NotFound("Veículo não encontrado na base de dados!");
            }
        }
    }
}