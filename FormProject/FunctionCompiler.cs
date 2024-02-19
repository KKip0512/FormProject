using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace FormProject
{
    internal static class FunctionCompiler
    {
        public static double GetY(string expression, double x)
        {
            if (expression.StartsWith('x')) expression = expression.Replace("x", x.ToString(MyForm.numberFormatInfo));
            else
            {
                for (int i = 0; i < expression.Length; i++)
                {
                    if (expression[i] == 'x')
                    {
                        bool isX = false;
                        foreach (var op in MathFunction.possibleOperations)
                        {
                            if (expression[i - 1].ToString() == op.Item1) isX = true;
                        }
                        if (expression[i - 1] == '(' || isX)
                        {
                            StringBuilder sb = new(expression);
                            sb[i] = '~';
                            sb.Replace("~", x.ToString(MyForm.numberFormatInfo));
                            expression = sb.ToString();
                        }
                    }
                }
            }

            Stack<int> parenthesisIndices = [];
            int expressionLength = expression.Length;

            for (int i = 0; i < expressionLength; i++)
            {
                if (expression[i] == '(')
                {
                    parenthesisIndices.Push(i);
                }
                else if (expression[i] == ')')
                {
                    int index = parenthesisIndices.Pop();
                    int funcStartIndex = parenthesisIndices.Count > 0 ? parenthesisIndices.Peek() + 1 : 0;

                    string func = expression.Substring(funcStartIndex, i - funcStartIndex + 1);
                    expression = expression.Replace(func, new MathFunction(func).Calculate().ToString(MyForm.numberFormatInfo));

                    expressionLength = expression.Length;
                    i = index;
                }
            }

            return double.Parse(expression, MyForm.numberFormatInfo);
        }
    }
    internal class MathFunction
    {
        public static readonly (string, Func<double, double, double>)[] possibleOperations =
        {
            ("+", (a1, a2) => a1 + a2),
            ("-", (a1, a2) => a1 - a2),
            ("*", (a1, a2) => a1 * a2),
            ("/", (a1, a2) => a1 / a2),
            ("^", Math.Pow),
        };

        public static readonly (string, Func<double, double>)[] possibleFunctions1Arg =
        {
            ("abs",         Math.Abs),
            ("acos",        Math.Acos),
            ("acosh",       Math.Acosh),
            ("asin",        Math.Asin),
            ("asinh",       Math.Asinh),
            ("atan",        Math.Atan),
            ("atanh",       Math.Atanh),
            ("bitDecrement",Math.BitDecrement),
            ("bitIncrement",Math.BitIncrement),
            ("cbrt",        Math.Cbrt),
            ("ceiling",     Math.Ceiling),
            ("cos",         Math.Cos),
            ("cosh",        Math.Cosh),
            ("exp",         Math.Exp),
            ("floor",       Math.Floor),
            ("log",         Math.Log),
            ("round",       Math.Round),
            ("sign", (num) => Math.Sign(num)),
            ("sin",         Math.Sin),
            ("sinh",        Math.Sinh),
            ("sqrt",        Math.Sqrt),
            ("tan",         Math.Tan),
            ("tanh",        Math.Tanh),
            ("truncate",    Math.Truncate)
            };
        public static readonly (string, Func<double, double, double>)[] possibleFunctions2Args =
        {
            ("atan2",       Math.Atan2),
            ("copySign",    Math.CopySign),
            ("log",         Math.Log),
            ("max",         Math.Max),
            ("min",         Math.Min),
            ("pow",         Math.Pow),
            ("round", (num, digits) => Math.Round(num, (int)digits)),
        };
        public static readonly (string, Func<double, double, double, double>)[] possibleFunctions3Args =
        {
            ("fusedMultiplyAdd",Math.FusedMultiplyAdd)
        };



        private readonly string _functionName;
        private readonly double[] _arguments;

        public int NumberOfArguments { get => _arguments.Length; }
        public string Function
        {
            get
            {
                if (NumberOfArguments == 1)
                    return $"{_functionName}({_arguments[0]})";
                else if (NumberOfArguments == 2)
                    return $"{_functionName}({_arguments[0]}, {_arguments[1]})";
                else
                    return $"{_functionName}({_arguments[0]}, {_arguments[1]}, {_arguments[2]})";
            }
        }

        public MathFunction(string function)
        {

            const string nums = "0123456789";
            string[] args;

            function = function.ToLower().Replace(" ", string.Empty);

            if (nums.Contains(function.First()))
            {
                int indexOfOperator = GetIndexOfOperator(function);
                _functionName = function[indexOfOperator].ToString(MyForm.numberFormatInfo);
                args = function.Split(_functionName);

                _arguments = [double.Parse(args[0]), double.Parse(args[1])];
            }
            else
            {
                int indexOfParenthesis = function.IndexOf('(');
                _functionName = function[..indexOfParenthesis].ToLower();
                args = function.Substring(indexOfParenthesis + 1, function.Length - indexOfParenthesis - 2).Split(',');

                _arguments = new double[args.Length];
                for (int i = 0; i < args.Length; i++)
                {
                    _arguments[i] = double.Parse(args[i], MyForm.numberFormatInfo);
                }
            }

        }

        public MathFunction(string functionName, double argument1)
        {
            _functionName = functionName;
            _arguments = [argument1];
        }
        public MathFunction(string functionName, double argument1, double argument2)
        {
            _functionName = functionName;
            _arguments = [argument1, argument2];
        }
        public MathFunction(string functionName, double argument1, double argument2, double argument3)
        {
            _functionName = functionName;
            _arguments = [argument1, argument2, argument3];
        }

        public double Calculate()
        {
            if (_functionName.Length == 1) return GetOperation(_functionName)(_arguments[0], _arguments[1]);

            return NumberOfArguments switch
            {
                1 => GetFunction1Arg(_functionName)(_arguments[0]),
                2 => GetFunction2Args(_functionName)(_arguments[0], _arguments[1]),
                3 => GetFunction3Args(_functionName)(_arguments[0], _arguments[1], _arguments[2]),
                _ => throw new Exception("СЛИШКОМ МНОГО АРГУМЕНТОВ!!!"),
            };
        }

        private static int GetIndexOfOperator(string function)
        {
            for (int i = 1; i < function.Length - 1; i++)
                foreach (string @operator in possibleOperations.Select(op => op.Item1))
                    if (function.Contains(@operator)) return function.IndexOf(@operator);

            throw new NotImplementedException("Оператор не найден");
        }

        private static Func<double, double, double> GetOperation(string @operator)
        {
            var result = possibleOperations.Where(op => op.Item1 == @operator).Select(func => func.Item2);
            if (!result.Any()) throw new Exception("Такая операция не найдена");
            if (result.Count() > 1) throw new Exception("НАЙДЕНО БОЛЬШЕ 1 ОПЕРАЦИИ!!!");
            return result.First();
        }
        private static Func<double, double> GetFunction1Arg(string functionName)
        {
            var result = possibleFunctions1Arg.Where(func => func.Item1 == functionName).Select(func => func.Item2);
            if (!result.Any()) throw new Exception("Такая функция не найдена");
            if (result.Count() > 1) throw new Exception("НАЙДЕНО БОЛЬШЕ 1 ФУНКЦИИ!!!");
            return result.First();
        }
        private static Func<double, double, double> GetFunction2Args(string functionName)
        {
            var result = possibleFunctions2Args.Where(func => func.Item1 == functionName).Select(func => func.Item2);
            if (!result.Any()) throw new Exception("Такая функция не найдена");
            if (result.Count() > 1) throw new Exception("НАЙДЕНО БОЛЬШЕ 1 ФУНКЦИИ!!!");
            return result.First();
        }
        private static Func<double, double, double, double> GetFunction3Args(string functionName)
        {
            var result = possibleFunctions3Args.Where(func => func.Item1 == functionName).Select(func => func.Item2);
            if (!result.Any()) throw new Exception("Такая функция не найдена");
            if (result.Count() > 1) throw new Exception("НАЙДЕНО БОЛЬШЕ 1 ФУНКЦИИ!!!");
            return result.First();
        }
    }
}