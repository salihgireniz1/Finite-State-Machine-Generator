using System.Linq;

public static class StringExtensions
{
    /// <summary>
    /// Normalizes the input string by removing all white spaces.
    /// </summary>
    /// <param name="name">The input string to be normalized.</param>
    /// <returns>A new string without any white spaces.</returns>
    public static string NormalizeName(this string name)
    {
        return new string(name.Where(c => !char.IsWhiteSpace(c)).ToArray());
    }
}