using StandAPI.Models;

namespace StandAPI.Repositories;

public interface IVeiculoRepository
{
    List<Veiculo> ObterTodos();
    Veiculo? ObterPorMatricula(string matricula); //null se não existir
    bool Adicionar(Veiculo veiculo); // false se a matricula já existir
    bool Atualizar(Veiculo veiculo); // false se não existir
    bool Excluir(string matricula); // false se não existir
}