using CalculatorApp.Services.Interfaces;

namespace CalculatorApp.Services;

public class ConvertService: IConvertService
{
    private readonly IVerificationService _verificationService;
    private readonly IPriorityService _priorityService;
    
    public ConvertService(IVerificationService verificationService, IPriorityService priorityService)
    {
        _verificationService = verificationService;
        _priorityService = priorityService;
    }

    public List<string> ConvertToPostFix(string expression)
    {
        var operationsStack = new Stack<char>();
        var postfixExpressionList = new List<string>();
        var tokenString = UnaryOperationReplace(expression.Trim());

        for (var position = 0; position < tokenString.Length; position++)
        {
            if (_verificationService.IsOpeningBracket(tokenString[position]))
            {
                operationsStack.Push(tokenString[position]);
            }
            else if (char.IsDigit(tokenString[position]))
            {
                postfixExpressionList.Add(GetNumber(tokenString, ref position));
            }
            else if (_verificationService.IsClosingBracket(tokenString[position]))
            {
                var topOperation = operationsStack.Pop();

                while (!_verificationService.IsOpeningBracket(topOperation) && operationsStack.Count > 0)
                {
                    postfixExpressionList.Add(topOperation.ToString());
                    topOperation = operationsStack.Pop();
                }
            }
            else if (_priorityService.ContainsOperation(tokenString[position].ToString())) 
            {
                while (operationsStack.Count > 0 
                       && _priorityService.OperationPriorityCheck(operationsStack.Peek(), tokenString[position]))
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
    
    private string GetNumber(string analyzingString, ref int position)
    {
        var number = "";
        
        for (; position < analyzingString.Length; position++)
        {
            var symbol = analyzingString[position];

            if(char.IsDigit(symbol) || _verificationService.IsPointOrComa(symbol))
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
}