using System.Text;

namespace FormProject
{
    internal class MathExpression(string expression)
    {
        public string Expression { get; } = expression ?? throw new NullReferenceException("Expression was null");

        private StringBuilder _expressionBuilder = new(expression.Replace(" ", string.Empty).ToLower());

        private const string nums = "0123456789";
        public static readonly Dictionary<char, Func<double, double, double>> possibleOperations = new()
        {
            { '+',   (a1, a2) => a1 + a2  },
            { '-',   (a1, a2) => a1 - a2  },
            { '*',   (a1, a2) => a1 * a2  },
            { '/',   (a1, a2) => a1 / a2  },
            { '^',               Math.Pow }
        };

        public static double Calculate(string expression) => new MathExpression(expression).Calculate();

        public double Calculate()
        {
            CalculateExpressionInParentheses();
            CalculatePow();
            CalculateMultAndDiv();
            CalculateSumAndSub();

            return double.Parse(_expressionBuilder.ToString(), MyForm.numberFormatInfo);
        }

        private void CalculatePow()
        {
            if (_expressionBuilder.ToString().Contains('^'))
                CalculateOperationsRelatedToOperators('^');
        }
        private void CalculateMultAndDiv()
        {
            if (_expressionBuilder.ToString().Contains('/') || _expressionBuilder.ToString().Contains('*'))
                CalculateOperationsRelatedToOperators('*', '/');
        }
        private void CalculateSumAndSub()
        {
            double result = 0;
            _expressionBuilder.Replace("-", "+-");
            foreach (var num in _expressionBuilder.ToString().Split('+'))
                result += num.Length > 0 ? double.Parse(num, MyForm.numberFormatInfo) : 0;

            _expressionBuilder = new(result.ToString(MyForm.numberFormatInfo));
        }
        private void CalculateOperationsRelatedToOperators(params char[] operators)
        {
            int startOfOperation, endOfOperation;

            for (int i = 0; i < _expressionBuilder.Length; i++)
            {
                if (!IsRightOperator(i, operators)) continue;

                char @operator = _expressionBuilder[i];
                startOfOperation = GetStartOfOperation(i);
                endOfOperation = GetEndOfOperation(i);
                ReplaceOperationWithResult(startOfOperation, endOfOperation, @operator);

                i = startOfOperation;
            }
        }
        private bool IsRightOperator(int index, char[] operators)
        {
            bool isRightOperator = false;
            foreach (char op in operators)
                if (_expressionBuilder[index] == op)
                { isRightOperator = true; break; }
            return isRightOperator;
        }
        private int GetStartOfOperation(int operatorIndex)
        {
            int startOfOperation = operatorIndex;

            do { startOfOperation--; }
            while (startOfOperation > 0 &&
                (nums.Contains(_expressionBuilder[startOfOperation]) || _expressionBuilder[startOfOperation] == '.'));

            if (_expressionBuilder[startOfOperation] != '-' && startOfOperation > 0) startOfOperation++;

            return startOfOperation;
        }
        private int GetEndOfOperation(int operatorIndex)
        {
            int endOfOperation = operatorIndex + 1;

            if (_expressionBuilder[endOfOperation] == '-') endOfOperation++;

            while (endOfOperation < _expressionBuilder.Length &&
                (nums.Contains(_expressionBuilder[endOfOperation]) || _expressionBuilder[endOfOperation] == '.'))
            { endOfOperation++; }

            return --endOfOperation;
        }
        private void ReplaceOperationWithResult(int startOfOperation, int endOfOperation, char @operator)
        {
            string operation = _expressionBuilder.ToString()[startOfOperation..(endOfOperation + 1)];
            _expressionBuilder.Remove(startOfOperation, endOfOperation - startOfOperation + 1);
            _expressionBuilder.Insert(startOfOperation,
                CalculateOperation(operation, @operator).ToString("0.###############", MyForm.numberFormatInfo));
        }

        private void CalculateExpressionInParentheses()
        {
            if (!_expressionBuilder.ToString().Contains('(')) return;

            Stack<int> indicesOfParentheses = [];
            for (int i = 0; i < _expressionBuilder.Length; i++)
            {
                if (_expressionBuilder[i] == '(') indicesOfParentheses.Push(i);
                else if (_expressionBuilder[i] == ')')
                {
                    int startIndex = indicesOfParentheses.Pop();
                    string expression = _expressionBuilder.ToString()[(startIndex+1)..i];
                    _expressionBuilder.Remove(startIndex, i - startIndex + 1);
                    _expressionBuilder.Insert(startIndex,
                        Calculate(expression).ToString(MyForm.numberFormatInfo));
                    i = startIndex;
                }
            }
        }

        private static double CalculateOperation(string operation, char @operator)
        {
            string[] operands = operation.Split(@operator);
            return possibleOperations[@operator]
                (double.Parse(operands[0], MyForm.numberFormatInfo),
                 double.Parse(operands[1], MyForm.numberFormatInfo));
        }
    }
}
