using StandAPI.Models;

namespace StandAPI.Repositories;

public interface IVeiculoRepository
{
    List<Veiculo> ObterTodos();
    Veiculo? ObterPorId(int id); //null se não existir
    int Adicionar(Veiculo veiculo); // -1 se a matricula já existir
    bool Atualizar(Veiculo veiculo); // false se não existir
    bool Excluir(int id); // false se não existir
}