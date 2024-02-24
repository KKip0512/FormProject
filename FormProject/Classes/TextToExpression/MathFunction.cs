namespace FormProject.Classes.TextToExpression
{
    internal class MathFunction
    {
        public static readonly Dictionary<string, Func<double, double>> possibleFunctions1Arg = new()
        {
            { "",            num => num },
            { "abs",         Math.Abs },
            { "acos",        Math.Acos },
            { "acosh",       Math.Acosh },
            { "asin",        Math.Asin },
            { "asinh",       Math.Asinh },
            { "atan",        Math.Atan },
            { "atanh",       Math.Atanh },
            { "bitDecrement",Math.BitDecrement },
            { "bitIncrement",Math.BitIncrement },
            { "cbrt",        Math.Cbrt },
            { "ceiling",     Math.Ceiling },
            { "cos",         Math.Cos },
            { "cosh",        Math.Cosh },
            { "exp",         Math.Exp },
            { "floor",       Math.Floor },
            { "log",         Math.Log },
            { "round",       Math.Round },
            { "sign", (num) => Math.Sign(num) },
            { "sin",         Math.Sin },
            { "sinh",        Math.Sinh },
            { "sqrt",        Math.Sqrt },
            { "tan",         Math.Tan },
            { "tanh",        Math.Tanh },
            { "truncate",    Math.Truncate }
        };
        public static readonly Dictionary<string, Func<double, double, double>> possibleFunctions2Args = new()
        {
            { "atan2",       Math.Atan2 },
            { "copySign",    Math.CopySign },
            { "log",         Math.Log },
            { "max",         Math.Max },
            { "min",         Math.Min },
            { "pow",         Math.Pow },
            { "round", (num, digits) => Math.Round(num, (int)digits) },
        };
        public static readonly Dictionary<string, Func<double, double, double, double>> possibleFunctions3Args = new()
        {
            { "fusedMultiplyAdd", Math.FusedMultiplyAdd }
        };



        private readonly string _functionName;
        private readonly string[] _arguments;

        public int NumberOfArguments => _arguments.Length;
        public string Function => NumberOfArguments switch
        {
            1 => $"{_functionName}({_arguments[0]})",
            2 => $"{_functionName}({_arguments[0]}, {_arguments[1]})",
            3 => $"{_functionName}({_arguments[0]}, {_arguments[1]}, {_arguments[2]})",
            _ => throw new NotImplementedException()
        };

        public MathFunction(string function)
        {
            int indexOfParenthesis = function.IndexOf('(');
            if (indexOfParenthesis == -1)
            {
                _functionName = string.Empty;
                _arguments = [function];
            }
            else
            {
                _functionName = function[..indexOfParenthesis];
                _arguments = function[(indexOfParenthesis + 1)..(function.Length - 1)].Split(',');
            }
        }

        public MathFunction(string functionName, string argument1)
        {
            _functionName = functionName;
            _arguments = [argument1];
        }
        public MathFunction(string functionName, string argument1, string argument2)
        {
            _functionName = functionName;
            _arguments = [argument1, argument2];
        }
        public MathFunction(string functionName, string argument1, string argument2, string argument3)
        {
            _functionName = functionName;
            _arguments = [argument1, argument2, argument3];
        }

        public static double Calculate(string function) => new MathFunction(function).Calculate();

        public double Calculate()
        {
            double[] calculatedArgs = new double[NumberOfArguments];
            for (int i = 0; i < NumberOfArguments; i++)
                calculatedArgs[i] = MathPolynomial.Calculate(_arguments[i]);

            return NumberOfArguments switch
            {
                1 => possibleFunctions1Arg[_functionName](calculatedArgs[0]),
                2 => possibleFunctions2Args[_functionName](calculatedArgs[0], calculatedArgs[1]),
                3 => possibleFunctions3Args[_functionName](calculatedArgs[0], calculatedArgs[1], calculatedArgs[2]),
                _ => throw new Exception("СЛИШКОМ МНОГО АРГУМЕНТОВ!!!, что, собственно говоря, по идее невозможно"),
            };
        }
    }
}
