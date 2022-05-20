using System.Diagnostics;
using System.Globalization;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using CalculatorApp.Models;

namespace CalculatorApp.Controllers;

public class CalculationController : Controller
{
    private readonly Dictionary<string, int> _operationPriority = new() {
        {"(", 0},
        {"+", 1},
        {"-", 1},
        {"*", 2},
        {"/", 2},
        {"~", 3}
    };
    
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    
    [HttpPost]
    public JsonResult GetExpression([FromBody]DataModel expression)
    {
        return ModelState.IsValid
            ? Json(Calculate(ConvertToPostFix(expression.Value)))
            : Json("ModelError");
    }

    private List<string> ConvertToPostFix(string? expression)
    {
        var operationsStack = new Stack<char>();
        var postfixExpressionList = new List<string>();
        var tokenString = UnaryOperationReplace(expression.Trim());

        for (var position = 0; position < tokenString.Length; position++)
        {
            if (IsOpeningBracket(tokenString[position]))
            {
                operationsStack.Push(tokenString[position]);
            }
            else if (char.IsDigit(tokenString[position]))
            {
               postfixExpressionList.Add(GetNumber(tokenString, ref position));
            }
            else if (IsClosingBracket(tokenString[position]))
            {
                var topOperation = operationsStack.Pop();

                while (!IsOpeningBracket(topOperation) && operationsStack.Count > 0)
                {
                    postfixExpressionList.Add(topOperation.ToString());
                    topOperation = operationsStack.Pop();
                }
            }
            else if (_operationPriority.ContainsKey(tokenString[position].ToString())) 
            {
                while (operationsStack.Count > 0 
                       && OperationPriorityCheck(operationsStack.Peek(), tokenString[position]))
                {
                    postfixExpressionList.Add(operationsStack.Pop().ToString());
                }
                
                operationsStack.Push(tokenString[position]);
            }
        }

        while (operationsStack.Count > 0)
        {
            postfixExpressionList.Add(operationsStack.Pop().ToString());
        }

        return postfixExpressionList;
    }

    private static bool IsOpeningBracket(char symbol) => symbol.Equals('(');

    private static bool IsClosingBracket(char symbol) => symbol.Equals(')');

    private static bool IsPointOrComa(char symbol) => symbol.Equals('.') || symbol.Equals(',');

    private static bool IsUnaryOperation(string symbol) => symbol.Equals("~");

    private bool OperationPriorityCheck(char topOfTheStack, char token) => 
        _operationPriority[topOfTheStack.ToString()] >= _operationPriority[token.ToString()];

    private static string GetNumber(string analyzingString, ref int position)
    {
        var number = "";
        
        for (; position < analyzingString.Length; position++)
        {
            var symbol = analyzingString[position];

            if(char.IsDigit(symbol) || IsPointOrComa(symbol))
            {
                number += symbol;
            }
            else
            {
                position--;
                break;
            }
        }

        return number;
    }

    private static string UnaryOperationReplace(string stringToChange)
    {
        if (stringToChange.StartsWith('-'))
        {
            stringToChange = stringToChange.Remove(0,1).Insert(0, "~");
        }
        
        stringToChange = stringToChange
            .Replace("(-", "(~")
            .Replace("(+", "(")
            .Replace("*-", "*~")
            .Replace("/-", "/~")
            .Replace("+-", "+~")
            .Replace("--", "-~");

        return stringToChange;
    }

    private List<string> Calculate(List<string> postfixList)
    {

        var calculations = new Stack<double>();
        var results = new List<string>();

        foreach (var token in postfixList)
        {
            if (double.TryParse(token, out var number))
            {
                calculations.Push(number);
            } 
            else if (_operationPriority.ContainsKey(token))
            {
                if (IsUnaryOperation(token))
                {
                    var numberWithUnaryOp = calculations.Count > 0 ? calculations.Pop() : 0;
                    calculations.Push(ExecuteOperation(token, 0, numberWithUnaryOp));
                    results.Add($"{token}{numberWithUnaryOp} = {calculations.Peek()}");
                }
                else
                {
                    var secondNumber = calculations.Count > 0 ? calculations.Pop() : 0;
                    var firstNumber = calculations.Count > 0 ? calculations.Pop() : 0;
                    
                    calculations.Push(ExecuteOperation(token, firstNumber, secondNumber));
                    results.Add($"{firstNumber} {token} {secondNumber} = {calculations.Peek()}");
                }
            }
        }
        
        results.Add(calculations.Count > 0 ? $"final Result: {calculations.Pop()}" : string.Empty);
        return results;
    }

    private double ExecuteOperation(string operation, double firstNumber, double secondNumber)
    {
        return operation switch
        {
            "+" => firstNumber + secondNumber,
            "-" => firstNumber - secondNumber,
            "~" => firstNumber - secondNumber,
            "*" => firstNumber * secondNumber,
            "/" => firstNumber / secondNumber,
            _ => 0
        };
    }
}