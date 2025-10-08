using System.Collections;

namespace Plugin.TabbedPage.Maui.Extensions
{
    internal static class EnumerableExtensions
    {
        public static T FirstOrDefault<T>(this IEnumerable items)
        {
            foreach (var item in items.OfType<T>())
            {
                return item;
            }

            return default;
        }

    }
}