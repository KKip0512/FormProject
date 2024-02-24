using System.Text;

namespace FormProject.Classes.TextToExpression
{
    internal class MathPolynomial(string polynomial)
    {
        public string Polynomial { get; } = polynomial ?? throw new NullReferenceException("Polynomial was null");

        private StringBuilder _polynomialBuilder = new(polynomial.Replace(" ", string.Empty).ToLower());

        private const string nums = "0123456789";
        public static readonly Dictionary<char, Func<double, double, double>> possibleOperations = new()
        {
            { '+',   (a1, a2) => a1 + a2  },
            { '-',   (a1, a2) => a1 - a2  },
            { '*',   (a1, a2) => a1 * a2  },
            { '/',   (a1, a2) => a1 / a2  },
            { '^',               Math.Pow }
        };

        public static double Calculate(string polynomial) => new MathPolynomial(polynomial).Calculate();

        public double Calculate()
        {
            CalculatePolynomialsInParentheses();
            CalculatePow();
            CalculateMultAndDiv();
            CalculateSumAndSub();

            return double.Parse(_polynomialBuilder.ToString(), MyForm.numberFormatInfo);
        }

        private void CalculatePow()
        {
            if (_polynomialBuilder.ToString().Contains('^'))
                CalculateOperationsRelatedToOperators('^');
        }
        private void CalculateMultAndDiv()
        {
            if (_polynomialBuilder.ToString().Contains('/') || _polynomialBuilder.ToString().Contains('*'))
                CalculateOperationsRelatedToOperators('*', '/');
        }
        private void CalculateSumAndSub()
        {
            double result = 0;
            _polynomialBuilder.Replace("-", "+-");
            foreach (var num in _polynomialBuilder.ToString().Split('+'))
                result += num.Length > 0 ? double.Parse(num, MyForm.numberFormatInfo) : 0;

            _polynomialBuilder = new(result.ToString(MyForm.numberFormatInfo));
        }
        private void CalculateOperationsRelatedToOperators(params char[] operators)
        {
            int startOfOperation, endOfOperation;

            for (int i = 0; i < _polynomialBuilder.Length; i++)
            {
                if (!IsRightOperator(i, operators)) continue;

                char @operator = _polynomialBuilder[i];
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
                if (_polynomialBuilder[index] == op)
                { isRightOperator = true; break; }
            return isRightOperator;
        }
        private int GetStartOfOperation(int operatorIndex)
        {
            int startOfOperation = operatorIndex;

            do { startOfOperation--; }
            while (startOfOperation > 0 &&
                (nums.Contains(_polynomialBuilder[startOfOperation]) || _polynomialBuilder[startOfOperation] == '.'));

            if (_polynomialBuilder[startOfOperation] != '-' && startOfOperation > 0) startOfOperation++;

            return startOfOperation;
        }
        private int GetEndOfOperation(int operatorIndex)
        {
            int endOfOperation = operatorIndex + 1;

            if (_polynomialBuilder[endOfOperation] == '-') endOfOperation++;

            while (endOfOperation < _polynomialBuilder.Length &&
                (nums.Contains(_polynomialBuilder[endOfOperation]) || _polynomialBuilder[endOfOperation] == '.'))
            { endOfOperation++; }

            return --endOfOperation;
        }
        private void ReplaceOperationWithResult(int startOfOperation, int endOfOperation, char @operator)
        {
            string operation = _polynomialBuilder.ToString()[startOfOperation..(endOfOperation + 1)];
            _polynomialBuilder.Remove(startOfOperation, endOfOperation - startOfOperation + 1);
            _polynomialBuilder.Insert(startOfOperation,
                CalculateOperation(operation, @operator).ToString("0.###############", MyForm.numberFormatInfo));
        }

        private void CalculatePolynomialsInParentheses()
        {
            if (!_polynomialBuilder.ToString().Contains('(')) return;

            Stack<int> indicesOfParentheses = [];
            for (int i = 0; i < _polynomialBuilder.Length; i++)
            {
                if (_polynomialBuilder[i] == '(') indicesOfParentheses.Push(i);
                else if (_polynomialBuilder[i] == ')')
                {
                    int startIndex = indicesOfParentheses.Pop();
                    string polynomial = _polynomialBuilder.ToString()[(startIndex + 1)..i];
                    _polynomialBuilder.Remove(startIndex, i - startIndex + 1);
                    _polynomialBuilder.Insert(startIndex,
                        Calculate(polynomial).ToString(MyForm.numberFormatInfo));
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