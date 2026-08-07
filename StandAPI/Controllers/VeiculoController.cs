using Microsoft.AspNetCore.Mvc;
using StandAPI.Models;

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
        // Num projeto real, aqui abrias a SqlConnection e fazias "SELECT * FROM Veiculos"
        return Ok(baseDeDadosDeVeiculos); // O Ok() transforma logo a lista em JSON!
    }

    // O equivalente ao botão "Adicionar"
    [HttpPost]
    public IActionResult AdicionarNovo(Veiculo novoVeiculo)
    {
        // Num projeto real, aqui fazias o "INSERT INTO Veiculos..."
        if (baseDeDadosDeVeiculos.Any(v => v.Matricula == novoVeiculo.Matricula))
        {
            return BadRequest("Veículo já existe na base de dados!");
        }
        baseDeDadosDeVeiculos.Add(novoVeiculo);
        
        return CreatedAtAction(nameof(ListarTodos), new { matricula = novoVeiculo.Matricula }, novoVeiculo);
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
            var veiculoExistente = VeiculosController.baseDeDadosDeVeiculos.FirstOrDefault(v => v.Matricula == veiculo.Matricula);
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
        var veiculoExistente = VeiculosController.baseDeDadosDeVeiculos.FirstOrDefault(v => v.Matricula == matricula);
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