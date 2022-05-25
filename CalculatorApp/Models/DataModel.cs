using System.ComponentModel.DataAnnotations;

namespace CalculatorApp.Models;

public class DataModel
{
    public DataModel(string value)
    {
        Value = value;
    }

    [Required] 
    public string Value { get; }
}