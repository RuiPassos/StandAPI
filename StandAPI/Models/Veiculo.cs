using System.Text.RegularExpressions;

namespace StandAPI.Models;

public enum Combustivel
{
    Gasolina,
    Gasoleo
}

public class Veiculo
{
    private string _matricula = "";

    public string Matricula
    {
        get { return _matricula; }
        set
        {
            string regexMatricula =
                "^(([A-Z]{2}-\\d{2}-\\d{2})|(\\d{2}-\\d{2}-[A-Z]{2})|(\\d{2}-[A-Z]{2}-\\d{2})|([A-Z]{2}-\\d{2}-[A-Z]{2}))$\n";
            if (value.Length == 8 || Regex.IsMatch(value.ToUpper(), regexMatricula))
            {
                _matricula = value;
            }
            else
            {
                throw new ArgumentException("Matricula Inválida!");
            }
        }
    }
    
    public string Marca { get; set; }
    public string Modelo { get; set; }
    public double Peso { get; set; }
    public Combustivel Comb { get; set; }

    public Veiculo(string matricula, string marca, string modelo, double peso, Combustivel comb)
    {
        this.Matricula = matricula;
        this.Marca = marca;
        this.Modelo = modelo;
        this.Peso = peso;
        this.Comb = comb;
    }

    override public string ToString()
    {
        return $"{Matricula} {Marca} {Modelo} {Peso}";
    }
}