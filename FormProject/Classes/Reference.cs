namespace FormProject.Classes
{
    internal class Reference<T>(T value) where T : struct
    {
        public T Value { get; set; } = value;
    }
}