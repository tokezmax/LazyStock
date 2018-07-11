using System.Collections.Generic;

namespace Common.Extensions
{
    public static class EnumerableExtension
    {
        public static string Join<T>(this IEnumerable<T> source, string separator = ",")
        {
            return string.Join(separator, source);
        }
    }
}
