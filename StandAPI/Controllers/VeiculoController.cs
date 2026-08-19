using Microsoft.AspNetCore.Mvc;
using StandAPI.Models;
using StandAPI.Repositories;
using StandAPI.Services;

namespace StandAPI.Controllers;

[ApiController]
[Route("api/veiculos")] 
public class VeiculosController : ControllerBase
{
    private readonly IVeiculoService _service;

    public VeiculosController(IVeiculoService service)
    {
        _service = service;
    }
    
    // Listar todos
    [HttpGet]
    public IActionResult ListarTodos() 
    {
        var veiculos = _service.ObterTodos();
        return Ok(veiculos);
    }
    
    // Listar por id
    [HttpGet("{id}")]
    public IActionResult ObterPorId(int id) 
    {
        var veiculo = _service.ObterPorId(id);

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
        var resultado = _service.Adicionar(novoVeiculo);

        if (!resultado.Sucesso)
        {
            return Conflict(resultado.Erro);
        }

        var veiculoCriado = resultado.Dados;
        return CreatedAtAction(nameof(ObterPorId), new { id = veiculoCriado.Id }, veiculoCriado);
    }

    // Atualizar Veiculo
    [HttpPut("{id}")]
    public IActionResult Atualizar(int id, Veiculo veiculo)
    {
        var resultado = _service.Atualizar(id, veiculo);

        if (!resultado.Sucesso)
        {
            return NotFound(resultado.Erro);
        }

        return Ok(resultado.Dados);
    }

    // Eliminar Veiculo
    [HttpDelete("{id}")]
    public IActionResult Eliminar(int id)
    {
        var resultado = _service.Excluir(id);

        if (!resultado)
        {
            return NotFound("Veiculo not found");
        }
        
        return NoContent();
    }
}
