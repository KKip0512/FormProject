using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace FormProject
{
    internal static class FunctionCompiler
    {
        private const string _letters = "abcdefghijklmnopqrstuvwxyz";

        public static double GetY(string expression, double x)
        {
            StringBuilder sb = new(ReplaceXWithValue(expression.Replace(" ", string.Empty).ToLower(), x));

            Stack<int> indicesOfParentheses = [];
            for (int i = 0; i < sb.Length; i++)
            {
                if (sb[i] == '(')
                    indicesOfParentheses.Push(i);
                else if (sb[i] == ')')
                {
                    int argsStartIndex = indicesOfParentheses.Pop();

                    int funcStartIndex = argsStartIndex - 1;
                    while (funcStartIndex >= 0 && _letters.Contains(sb[funcStartIndex]))
                        funcStartIndex--;
                    funcStartIndex++;

                    string func;

                    if (argsStartIndex != 0 && _letters.Contains(sb[argsStartIndex - 1]))
                        func = sb.ToString()[funcStartIndex..(i + 1)];
                    else
                        func = sb.ToString()[(argsStartIndex + 1)..i];

                    sb = sb.Remove(funcStartIndex, i - funcStartIndex + 1);
                    sb = sb.Insert(funcStartIndex, new MathFunction(func).Calculate().ToString(MyForm.numberFormatInfo));

                    i = funcStartIndex;
                }
            }

            //return double.Parse(sb.ToString(), MyForm.numberFormatInfo);
            return new MathFunction(sb.ToString()).Calculate();
        }
        private static string ReplaceXWithValue(string expression, double x)
        {
            StringBuilder sb = new(expression);
            if (sb[0] == 'x') sb = sb.Replace("x", x.ToString(MyForm.numberFormatInfo));
            else
            {
                for (int i = 1; i < sb.Length; i++)
                {
                    if (sb[i] == 'x' && !_letters.Contains(sb[i - 1]))
                    {
                        sb = sb.Remove(i, 1).Insert(i, x.ToString(MyForm.numberFormatInfo));
                    }

                }
            }
            return sb.ToString();
        }
    }
}