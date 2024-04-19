using System.Linq;

public static class StringExtensions
{

    public static string NormalizeName(this string name)
    {
        return new string(name.Where(c => !char.IsWhiteSpace(c)).ToArray());
    }
}