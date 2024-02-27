using System.Globalization;

namespace FormProject.Classes
{
    internal static class StringParser
    {
        public static NumberFormatInfo NumberFormatInfo { get; } = new()
        {
            NumberDecimalSeparator = ".",
        };

        public static double ToDouble(string num) => double.Parse(num, NumberFormatInfo);
        public static float ToFloat(string num) => float.Parse(num, NumberFormatInfo);

        public static string FromDouble(double num) => num.ToString("0.#########", NumberFormatInfo);
        public static string FromFloat(float num) => num.ToString("0.#########", NumberFormatInfo);
    }
}