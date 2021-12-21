using System.Linq;

namespace KittyHelper
{
    public static class ExtensionMethods
    {
        public static string ToUnderscoreCase(this string str)
        {
            return string.Concat(str.Select((x, i) => i > 0 && char.IsUpper(x) ? "-" + x : x.ToString()))
                .ToLower();
        }
    }
}