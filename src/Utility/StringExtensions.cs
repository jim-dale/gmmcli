
namespace gmmcli
{
    internal static class StringExtensions
    {
        internal static string FromCString(this string value)
        {
            return value?.Trim('\0');
        }

        internal static string ToCString(this string value)
        {
            return value == null ? null : value + '\0';
        }
    }
}
