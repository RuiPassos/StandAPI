using StandAPI.Models;
using StandAPI.Repositories;

namespace StandAPI.Services;

public class VeiculoService : IVeiculoService
{
    private readonly IVeiculoRepository _repository;

    public VeiculoService(IVeiculoRepository repository)
    {
        _repository = repository;
    }

    public List<Veiculo> ObterTodos()
    {
        return _repository.ObterTodos();
    }

    public Veiculo? ObterPorId(int id)
    {
        return _repository.ObterPorId(id);
    }

    public ServiceResult<Veiculo> Adicionar(Veiculo veiculo)
    {
        var novoId = _repository.Adicionar(veiculo);
        if (novoId == -1)
        {
            return ServiceResult<Veiculo>.Falha("Já existe um veiculo com essa matricula");
        }

        var veiculoCriado = _repository.ObterPorId(novoId)!;
        return ServiceResult<Veiculo>.Ok(veiculoCriado);
    }

    public ServiceResult<Veiculo> Atualizar(int id, Veiculo veiculo)
    {
        veiculo.Id = id;

        if (!_repository.Atualizar(veiculo))
        {
            return ServiceResult<Veiculo>.Falha("Veiculo not found");
        }

        return ServiceResult<Veiculo>.Ok(veiculo);
    }

    public bool Excluir(int id)
    {
        return _repository.Excluir(id);
    }
}