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
        
        SqliteCommand command = connection.CreateCommand();
        command.CommandText = "SELECT * FROM veiculos";
        
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            veiculos.Add(MapearVeiculo(reader));
        }
        
        return veiculos;
    }

    public Veiculo? ObterPorMatricula(string matricula)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        
        SqliteCommand command = connection.CreateCommand();
        command.CommandText = "Select * from veiculos where matricula = @matricula";
        command.Parameters.AddWithValue("@matricula", matricula);
        
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

    public bool Adicionar(Veiculo veiculo)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        
        SqliteCommand command = connection.CreateCommand();
        command.CommandText = "INSERT INTO veiculos (Matricula, Marca, Modelo, Peso, Combustivel) values (@matricula, @marca, @modelo, @peso, @combustivel)";   
        PreencherParametros(command, veiculo);

        try
        {
            command.ExecuteNonQuery();
            return true;
        }
        catch (SqliteException e) when (e.SqliteErrorCode == 19) // 19 = UNIQUE constraint
        {
            return false; // matrícula já existe
        }
    }

    public bool Atualizar(Veiculo veiculo)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        
        SqliteCommand command = connection.CreateCommand();
        command.CommandText = "UPDATE veiculos " +
                              "SET Marca = @marca, Modelo = @modelo, Peso = @peso, Combustivel = @combustivel " +
                              "WHERE Matricula = @matricula";
        PreencherParametros(command, veiculo);
        
        return command.ExecuteNonQuery() > 0; // linhas afetadas
    }

    public bool Excluir(string matricula)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        
        SqliteCommand command = connection.CreateCommand();
        command.CommandText = "DELETE FROM veiculos WHERE Matricula = @matricula";
        command.Parameters.AddWithValue("@matricula", matricula);
        
        return command.ExecuteNonQuery() > 0; // linhas afetadas
    }

    // helpers privados: mapeamento e parâmetros num só sitio
    private static Veiculo MapearVeiculo(SqliteDataReader reader) => new Veiculo(
        reader.GetString(0),
        reader.GetString(1),
        reader.GetString(2),
        reader.GetDouble(3),
        (Combustivel)reader.GetInt32(4)
    );
    
    private static void PreencherParametros(SqliteCommand command, Veiculo veiculo)
    {
        command.Parameters.AddWithValue("@matricula", veiculo.Matricula);
        command.Parameters.AddWithValue("@marca", veiculo.Marca);
        command.Parameters.AddWithValue("@modelo", veiculo.Modelo);
        command.Parameters.AddWithValue("@peso", veiculo.Peso);
        command.Parameters.AddWithValue("@combustivel", (int)veiculo.Comb);
    }
}