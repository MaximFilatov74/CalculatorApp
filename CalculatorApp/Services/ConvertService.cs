using CalculatorApp.Services.Interfaces;

namespace CalculatorApp.Services;

public class ConvertService: IConvertService
{
    private readonly Dictionary<string, int> _operationPriority = new() {
        {"(", 0},
        {"+", 1},
        {"-", 1},
        {"*", 2},
        {"/", 2},
        {"~", 3}
    };
    
    public List<string> ConvertToPostFix(string expression)
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
            else if (ContainsOperation(tokenString[position].ToString())) 
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
    
    private static bool IsOpeningBracket(char symbol) => symbol.Equals('(');
    
    private static bool IsClosingBracket(char symbol) => symbol.Equals(')');

    private static bool IsPointOrComa(char symbol) => symbol.Equals('.') || symbol.Equals(',');
    
    private bool OperationPriorityCheck(char topOfTheStack, char token) => 
        _operationPriority[topOfTheStack.ToString()] >= _operationPriority[token.ToString()];

    private bool ContainsOperation(string token) => _operationPriority.ContainsKey(token);
}