using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace StandAPI.Models;

public enum Combustivel
{
    Gasolina = 0,
    Gasoleo = 1, 
    Eletrico = 2,
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
                "^(([A-Z]{2}-\\d{2}-\\d{2})|(\\d{2}-\\d{2}-[A-Z]{2})|(\\d{2}-[A-Z]{2}-\\d{2})|([A-Z]{2}-\\d{2}-[A-Z]{2}))$";
            if (Regex.IsMatch(value.ToUpper(), regexMatricula))
            {
                _matricula = value.ToUpper();
            }
            else
            {
                throw new ArgumentException("Matricula Inválida!");
            }
        }
    }
    
    private string _marca = "";
    public string Marca
    {
        get { return _marca; }
        set
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("Marca não pode ser nula ou vazia");
            }
            _marca = value;
        }
    }
    
    private string _modelo = "";
    public string Modelo
    {
        get { return _modelo; }
        set
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("Modelo não pode ser nulo ou vazio");
            }
            _modelo = value;
        }
    }
    
    private double _peso;
    public double Peso
    {
        get { return _peso; }
        set
        {
            if (value < 0)
            {
                throw new ArgumentException("Peso não pode ser negativo");
            }
            _peso = value;
        }
    }
    
    private Combustivel _comb;
    public Combustivel Comb
    {
        get { return _comb; }
        set { _comb = value; }
    }
    
    private int _id;
    public int Id
    {
        get { return _id; }
        set
        {
            if (value < 0)
            {
                throw new ArgumentException("Id não pode ser negativo");
            }
            _id = value;
        }
    }

    public Veiculo(int id, string matricula, string marca, string modelo, double peso, Combustivel comb)
    {
        this.Id = id;
        this.Matricula = matricula;
        this.Marca = marca;
        this.Modelo = modelo;
        this.Peso = peso;
        this.Comb = comb;
    }

    override public string ToString()
    {
        return $"{Id} | {Matricula} {Marca} {Modelo} {Peso}";
    }
}