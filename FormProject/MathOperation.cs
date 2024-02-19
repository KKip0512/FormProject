using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
            bool flag = true;
            double result = 0;
            StringBuilder sb = new(expression);

            for (int i = 0; i < sb.Length; i++)
            {
                if (sb[0] == '-' && flag) { i++; flag = false; }
                if (i >= sb.Length) break;
                if (sb[i] == possibleOperations[0].Item1 || sb[i] == possibleOperations[1].Item1)
                {
                    result += double.Parse(sb.ToString()[..i], MyForm.numberFormatInfo);
                    sb.Remove(0, i);
                    i = 0;
                    flag = true;
                }
            }
            result += double.Parse(sb.ToString(), MyForm.numberFormatInfo);
            return result.ToString(MyForm.numberFormatInfo);
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

                    if (expressionB[i + 1] == possibleOperations[1].Item1) exprEndIndex++;

                    while (exprStartIndex >= 0 &&
                        (nums.Contains(expressionB[exprStartIndex]) || expressionB[exprStartIndex] == '.'))
                    { exprStartIndex--; }

                    while (exprEndIndex <= expressionB.Length - 1 &&
                        (nums.Contains(expressionB[exprEndIndex]) || expressionB[exprEndIndex] == '.'))
                    { exprEndIndex++; }

                    string operation = expressionB.ToString()[(exprStartIndex + 1)..exprEndIndex];
                    expressionB = expressionB.Remove(exprStartIndex + 1, exprEndIndex - exprStartIndex - 1);
                    expressionB = expressionB.Insert(exprStartIndex + 1,
                        CalculateOperation(operation, @operator).ToString(MyForm.numberFormatInfo));

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
            return mathOperation.Item2(double.Parse(operands[0], MyForm.numberFormatInfo),
                double.Parse(operands[1], MyForm.numberFormatInfo));
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
