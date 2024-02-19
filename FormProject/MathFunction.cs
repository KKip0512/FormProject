using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormProject
{
    internal class MathFunction
    {
        public static readonly (string, Func<double, double>)[] possibleFunctions1Arg =
        {
            ("",            num => num),
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
            int indexOfParenthesis = function.IndexOf('(');
            if (indexOfParenthesis == -1)
            {
                _functionName = possibleFunctions1Arg[0].Item1;
                _arguments = [MathOperation.CalculateExpression(function)];
                return;
            }

            _functionName = function[..indexOfParenthesis];

            string[] args = function.Substring(indexOfParenthesis + 1, function.Length - indexOfParenthesis - 2).Split(',');
            _arguments = new double[args.Length];
            for (int i = 0; i < args.Length; i++)
                _arguments[i] = MathOperation.CalculateExpression(args[i]);
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
            return NumberOfArguments switch
            {
                1 => GetFunction1Arg(_functionName)(_arguments[0]),
                2 => GetFunction2Args(_functionName)(_arguments[0], _arguments[1]),
                3 => GetFunction3Args(_functionName)(_arguments[0], _arguments[1], _arguments[2]),
                _ => throw new Exception("СЛИШКОМ МНОГО АРГУМЕНТОВ!!!"),
            };
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
