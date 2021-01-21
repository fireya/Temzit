using System.Linq;

namespace Temzit.MQTT
{
    /// <summary>
    ///     Copied from https://gist.github.com/vkobel/d7302c0076c64c95ef4b
    /// </summary>
    public static class ExtensionMethods
    {
        public static string ToSnakeCase(this string str)
        {
            return string.Concat(
                str.Select(
                    (x, i) => i > 0 && char.IsUpper(x)
                        ? "_" + x
                        : x.ToString()
                )
            ).ToLower();
        }
    }
}