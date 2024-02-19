using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormProject
{
    internal class MathOperation
    {
        public static readonly (char, Func<double, double, double>)[] possibleOperations =
        {
            ('+', (a1, a2) => a1 + a2),
            ('-', (a1, a2) => a1 - a2),
            ('*', (a1, a2) => a1 * a2),
            ('/', (a1, a2) => a1 / a2),
            ('^', Math.Pow),
        };

        private const string nums = "0123456789";

        public string Expression { get; private set; }
        public double Result { get; private set; }

        public MathOperation(string expression)
        {
            expression = expression.Replace(" ", string.Empty).ToLowerInvariant();
            Expression = expression;

            if (expression.Contains('('))
                expression = CalculateExpressionInParentheses(expression);

            expression = CalculatePow(expression);
            expression = CalculateMultAndDiv(expression);
            expression = CalculateSumAndSub(expression);

            Result = double.Parse(expression, MyForm.numberFormatInfo);
        }

        public static double CalculateExpression(string expression)
        {
            return new MathOperation(expression).Result;
        }

        private static string CalculatePow(string expression)
        {
            if (expression.Contains(possibleOperations[4].Item1))
                return CalculateOperationsRelatedToOperator(expression, possibleOperations[4].Item1);
            return expression;
        }
        private static string CalculateMultAndDiv(string expression)
        {
            if (expression.Contains(possibleOperations[2].Item1))
                expression = CalculateOperationsRelatedToOperator(expression, possibleOperations[2].Item1);
            if (expression.Contains(possibleOperations[3].Item1))
                expression = CalculateOperationsRelatedToOperator(expression, possibleOperations[3].Item1);
            return expression;
        }
        private static string CalculateSumAndSub(string expression)
        {
            StringBuilder sb = new(expression);
            List<int> minusIndices = [];
            for (int i = 0; i < expression.Length; i++)
                if (expression[i] == '-')
                    minusIndices.Add(i);
            foreach (int i in minusIndices)
            {
                if (i == 0) sb.Insert(i, "0");
                else if (!nums.Contains(sb[i-1])) sb.Insert(i, "0");
            }
            expression = sb.ToString();

            if (expression.Contains(possibleOperations[0].Item1))
                expression = CalculateOperationsRelatedToOperator(expression, possibleOperations[0].Item1);
            if (expression.Contains(possibleOperations[1].Item1))
                expression = CalculateOperationsRelatedToOperator(expression, possibleOperations[1].Item1);
            return expression;
        }
        private static string CalculateOperationsRelatedToOperator(string expression, char @operator)
        {
            StringBuilder expressionB = new(expression);
            for (int i = 0; i < expressionB.Length; i++)
            {
                if (expressionB[i] == @operator)
                {
                    int exprStartIndex = i - 1;
                    int exprEndIndex = i + 1;

                    while (exprStartIndex >= 0 &&
                        (nums.Contains(expressionB[exprStartIndex]) || expressionB[exprStartIndex] == '.'
                        || expressionB[exprStartIndex] == '-'))
                    { exprStartIndex--; }

                    while (exprEndIndex <= expressionB.Length - 1 &&
                        (nums.Contains(expressionB[exprEndIndex]) || expressionB[exprEndIndex] == '.'))
                    { exprEndIndex++; }

                    string operation = expressionB.ToString()[(exprStartIndex + 1)..exprEndIndex];
                    expressionB = expressionB.Remove(exprStartIndex + 1, exprEndIndex - exprStartIndex - 1);
                    expressionB = expressionB.Insert(exprStartIndex + 1, CalculateOperation(operation, @operator));

                    i = exprStartIndex + 1;
                }
            }
            return expressionB.ToString();
            //throw new Exception("Некорректное выражение");
        }
        private static string CalculateExpressionInParentheses(string expression)
        {
            StringBuilder sb = new(expression);
            Stack<int> indicesOfParentheses = [];
            for (int i = 0; i < sb.Length; i++)
            {
                if (sb[i] == '(') indicesOfParentheses.Push(i);
                else if (sb[i] == ')')
                {
                    int startIndex = indicesOfParentheses.Pop();
                    sb = sb.Remove(startIndex, i - startIndex + 1);
                    sb = sb.Insert(startIndex, CalculateExpression(sb.ToString()[(startIndex + 1)..i]));    
                    i = startIndex + 1;
                }
            }
            return sb.ToString();
        }

        private static double CalculateOperation(string operation, char @operator)
        {
            string[] operands = operation.Split(@operator);
            return GetOperation(@operator)(double.Parse(operands[0], MyForm.numberFormatInfo),
                double.Parse(operands[1], MyForm.numberFormatInfo));
        }
        private static double CalculateOperation(string operation)
        {
            var mathOperation = possibleOperations[GetIndexOfOperator(operation)];
            string[] operands = operation.Split(mathOperation.Item1);
            return mathOperation.Item2(double.Parse(operands[0]), double.Parse(operands[1]));
        }

        private static Func<double, double, double> GetOperation(char @operator)
        {
            var result = possibleOperations.Where(op => op.Item1 == @operator).Select(func => func.Item2);
            if (!result.Any()) throw new Exception("Такая операция не найдена");
            if (result.Count() > 1) throw new Exception("НАЙДЕНО БОЛЬШЕ 1 ОПЕРАЦИИ!!!");
            return result.First();
        }

        private static int GetIndexOfOperator(string operation)
        {
            for (int i = 1; i < operation.Length - 1; i++)
                foreach (char @operator in possibleOperations.Select(op => op.Item1))
                    if (operation.Contains(@operator)) return operation.IndexOf(@operator);

            throw new NotImplementedException("Оператор не найден");
        }
    }
}
