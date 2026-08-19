namespace StandAPI.Services;

public class ServiceResult<T>
{
    public bool Sucesso { get; }
    public T? Dados { get; }
    public string? Erro { get; }

    private ServiceResult(bool sucesso, T? dados, string? erro)
    {
        Sucesso = sucesso;
        Dados = dados;
        Erro = erro;
    }

    public static ServiceResult<T> Ok(T dados) => new(true, dados, null);
    public static ServiceResult<T> Falha(string erro) => new(false, default, erro);
}
