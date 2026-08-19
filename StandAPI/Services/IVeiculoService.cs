using StandAPI.Models;

namespace StandAPI.Services;

public interface IVeiculoService
{
    List<Veiculo> ObterTodos();
    Veiculo? ObterPorId(int id);
    ServiceResult<Veiculo> Adicionar(Veiculo veiculo);
    ServiceResult<Veiculo> Atualizar(int id, Veiculo veiculo);
    bool Excluir(int id);
}