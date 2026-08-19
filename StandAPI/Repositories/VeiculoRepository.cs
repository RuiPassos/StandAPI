using Microsoft.Data.Sqlite;
using StandAPI.Models;

namespace StandAPI.Repositories;

public class VeiculoRepository : IVeiculoRepository
{
    private readonly string _connectionString;

    public VeiculoRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
    }

    public List<Veiculo> ObterTodos()
    {
        var veiculos = new List<Veiculo>();
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        
        using var command = connection.CreateCommand();
        command.CommandText = "SELECT id, matricula, marca, modelo, peso, combustivel FROM veiculos";
        
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            veiculos.Add(MapearVeiculo(reader));
        }
        
        return veiculos;
    }

    public Veiculo? ObterPorId(int id)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        
        using var command = connection.CreateCommand();
        command.CommandText = "SELECT id, matricula, marca, modelo, peso, combustivel FROM veiculos WHERE id = @id";
        command.Parameters.AddWithValue("@id", id);
        
        using var reader = command.ExecuteReader();
        if (reader.Read())
        {
            return MapearVeiculo(reader);
        }
        else
        {
            return null;
        }
    }

    public int Adicionar(Veiculo veiculo)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        
        using var command = connection.CreateCommand();
        command.CommandText = "INSERT INTO veiculos (Matricula, Marca, Modelo, Peso, Combustivel) values (@matricula, @marca, @modelo, @peso, @combustivel);" +
                              "SELECT last_insert_rowid();";   
        PreencherParametros(command, veiculo);

        try
        {
            var novoId = (long)command.ExecuteScalar();
            return novoId > 0 ? (int)novoId : -1; // retorna o id do novo veiculo ou -1 se falhar
        }
        catch (SqliteException e) when (e.SqliteErrorCode == 19) // 19 = UNIQUE constraint
        {
            return -1; // matricula já existe
        }
    }

    public bool Atualizar(Veiculo veiculo)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        
        using var command = connection.CreateCommand();
        command.CommandText = "UPDATE veiculos " +
                              "SET Matricula = @matricula, Marca = @marca, Modelo = @modelo, Peso = @peso, Combustivel = @combustivel " +
                              "WHERE id = @id";
        
        PreencherParametros(command, veiculo);
        
        return command.ExecuteNonQuery() > 0; // linhas afetadas
    }

    public bool Excluir(int id)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        
        using var command = connection.CreateCommand();
        command.CommandText = "DELETE FROM veiculos WHERE id = @id";
        command.Parameters.AddWithValue("@id", id);
        
        return command.ExecuteNonQuery() > 0; // linhas afetadas
    }

    // helpers privados: mapeamento e parâmetros num só sitio
    private static Veiculo MapearVeiculo(SqliteDataReader reader) => new Veiculo(
        reader.GetInt32(0),
        reader.GetString(1),
        reader.GetString(2),
        reader.GetString(3),
        reader.GetDouble(4),
        (Combustivel)reader.GetInt32(5)
    );
    
    private static void PreencherParametros(SqliteCommand command, Veiculo veiculo)
    {
        command.Parameters.AddWithValue("@id", veiculo.Id);
        command.Parameters.AddWithValue("@matricula", veiculo.Matricula);
        command.Parameters.AddWithValue("@marca", veiculo.Marca);
        command.Parameters.AddWithValue("@modelo", veiculo.Modelo);
        command.Parameters.AddWithValue("@peso", veiculo.Peso);
        command.Parameters.AddWithValue("@combustivel", (int)veiculo.Comb);
    }
}