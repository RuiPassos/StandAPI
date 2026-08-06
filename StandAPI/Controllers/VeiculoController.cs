using Microsoft.AspNetCore.Mvc;
using StandAPI.Models;

namespace StandAPI.Controllers;

[ApiController]
[Route("api/veiculos")] // O endereço base vai ser /api/veiculos
public class VeiculosController : ControllerBase
{
    // A nossa "Base de Dados" falsa por agora (estática para não se perder a cada request)
    private static List<Veiculo> baseDeDadosDeVeiculos = new List<Veiculo>
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
        baseDeDadosDeVeiculos.Add(novoVeiculo);
        
        return Ok("Veículo inserido com sucesso!");
    }
}