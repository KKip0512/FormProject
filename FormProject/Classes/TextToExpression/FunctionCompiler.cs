using System.Text;

namespace FormProject.Classes.TextToExpression
{
    internal class FunctionCompiler
    {
        private const string _letters = "abcdefghijklmnopqrstuvwxyz";

        private readonly StringBuilder _expressionBuilder;

        public FunctionCompiler(string expression, double x)
        {
            _expressionBuilder = new(expression.Replace(" ", string.Empty).ToLower());
            ReplaceVariableWithValue("x", x);
            ReplaceVariableWithValue("pi", Math.PI);
            ReplaceVariableWithValue("e", Math.E);
        }

        public static double GetY(string expression, double x) => new FunctionCompiler(expression, x).GetY();

        public double GetY()
        {
            Stack<int> indicesOfParentheses = [];
            for (int i = 0; i < _expressionBuilder.Length; i++)
            {
                if (_expressionBuilder[i] == '(') indicesOfParentheses.Push(i);
                else if (_expressionBuilder[i] == ')')
                {
                    int parenthesisIndex = indicesOfParentheses.Pop();
                    int funcStartIndex = GetFunctionStartIndex(parenthesisIndex);

                    string func = GetFunction(parenthesisIndex, funcStartIndex, i);
                    var a = MathFunction.Calculate(func);
                    _expressionBuilder.Replace(func, a.ToString("0.###############", MyForm.numberFormatInfo));

                    i = funcStartIndex;
                }
            }

            return MathFunction.Calculate(_expressionBuilder.ToString());
        }

        private int GetFunctionStartIndex(int argsStartIndex)
        {
            int funcStartIndex = argsStartIndex - 1;
            while (funcStartIndex >= 0 && _letters.Contains(_expressionBuilder[funcStartIndex]))
                funcStartIndex--;
            return ++funcStartIndex;  
        }
        private string GetFunction(int parenthesisIndex, int funcStartIndex, int funcEndIndex)
        {
            string func;
            if (parenthesisIndex != 0 && _letters.Contains(_expressionBuilder[parenthesisIndex - 1]))
                func = _expressionBuilder.ToString()[funcStartIndex..(funcEndIndex + 1)];
            else
                func = _expressionBuilder.ToString()[(parenthesisIndex + 1)..funcEndIndex];
            return func;
        }

        private StringBuilder ReplaceVariableWithValue(string variable, double value)
        {
            string valueAsString = value.ToString(MyForm.numberFormatInfo);

            if (IsStartOfVariable(variable, 0)) _expressionBuilder.Remove(0, variable.Length).Insert(0, valueAsString);
            for (int i = 1; i < _expressionBuilder.Length; i++)
                if (IsStartOfVariable(variable, i) && !_letters.Contains(_expressionBuilder[i - 1]))
                    _expressionBuilder.Remove(i, variable.Length).Insert(i, valueAsString);

            return _expressionBuilder;
        }
        private bool IsStartOfVariable(string variable, int index)
        {
            for (int i = 0; i < variable.Length; i++)
                if (_expressionBuilder[i + index] != variable[i])
                    return false;

            return true;
        }
    }
}