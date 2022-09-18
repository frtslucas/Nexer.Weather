namespace Nexer.Weather.Application
{
    internal static class Extensions
    {
        internal static IEnumerable<string> ReadLines(this string s)
        {
            string? line;
            
            using var sr = new StringReader(s);
            while ((line = sr.ReadLine()) is not null)
            {
                yield return line;
            }
        }
    }
}
