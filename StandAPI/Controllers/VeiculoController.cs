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
    
    // Listar por id
    [HttpGet("{id}")]
    public IActionResult ObterPorId(int id) 
    {
        var veiculo = _repository.ObterPorId(id);

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
        var novoId = _repository.Adicionar(novoVeiculo);

        if (novoId == -1)
        {
            return Conflict("Veiculo with the same matricula already exists");
        }

        var veiculoCriado = _repository.ObterPorId(novoId);
        return CreatedAtAction(nameof(ObterPorId), new { id = novoId }, veiculoCriado);
    }

    // Atualizar Veiculo
    [HttpPut("{id}")]
    public IActionResult Atualizar(int id, Veiculo veiculo)
    {
        veiculo.Id = id;
        var veiculoAtualizado = _repository.Atualizar(veiculo);

        if (!veiculoAtualizado)
        {
            return NotFound("Veiculo not found");
        }

        return Ok(veiculo);
    }

    // Eliminar Veiculo
    [HttpDelete("{id}")]
    public IActionResult Eliminar(int id)
    {
        var veiculoExcluido = _repository.Excluir(id);
        
        if (!veiculoExcluido)
        {
            return NotFound("Veiculo not found");
        }
        
        return NoContent();
    }
}
