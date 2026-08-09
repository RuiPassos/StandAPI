using Microsoft.AspNetCore.Mvc;
using StandAPI.Models;
using StandAPI.Repositories;

namespace StandAPI.Controllers;

[ApiController]
[Route("api/veiculos")] 
public class VeiculosController : ControllerBase
{
    private readonly IVeiculoRepository _repository;

    public VeiculosController(IVeiculoRepository repository)
    {
        _repository = repository;
    }
    
    // Listar todos
    [HttpGet]
    public IActionResult ListarTodos() 
    {
        var veiculos = _repository.ObterTodos();
        return Ok(veiculos);
    }
    
    // Listar por matricula
    [HttpGet("{matricula}")]
    public IActionResult ObterPorMatricula(string matricula) 
    {
        var veiculo = _repository.ObterPorMatricula(matricula);

        if (veiculo == null)
        {
            return NotFound("Veiculo not found");
        }
        else
        {
            return Ok(veiculo);
        }
    }

    // Adicionar Veiculo
    [HttpPost]
    public IActionResult AdicionarNovo(Veiculo novoVeiculo)
    {
        var veiculo = _repository.Adicionar(novoVeiculo);

        if (!veiculo)
        {
            return Conflict("Veiculo with the same matricula already exists");
        }
        
        return CreatedAtAction(nameof(ObterPorMatricula), new { matricula = novoVeiculo.Matricula }, novoVeiculo);
    }

    // Atualizar Veiculo
    [HttpPut("{matricula}")]
    public IActionResult Atualizar(Veiculo veiculo)
    {
        var veiculoAtualizado = _repository.Atualizar(veiculo);

        if (!veiculoAtualizado)
        {
            return NotFound("Veiculo not found");
        }
        
        return Ok(veiculo);
    }

    [HttpDelete("{matricula}")]
    public IActionResult Elimianr(string matricula)
    {
        var veiculoExcluido = _repository.Excluir(matricula);
        
        if (!veiculoExcluido)
        {
            return NotFound("Veiculo not found");
        }
        
        return NoContent();
    }
}
