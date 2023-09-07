using Org.BouncyCastle.Utilities.Encoders;

namespace ErabliereApi.Extensions;

/// <summary>
/// String Extensions
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Try parse a base64 string, it return true if its valide
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool IsValidBase64(this string value)
    {
        try
        {
            Base64.Decode(value);

            return true;
        }
        catch
        {
            return false;
        }
    }
}
