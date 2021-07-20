using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;


namespace PrintScreenNinja
{
    static class ApplicationExtensions
    {
        public static bool TryGetProperty<T>(this Application self, object key, [MaybeNullWhen(false)] out T value)
        {
            var properties = self.Properties;
            if (properties.Contains(key))
            {
                var v = properties[key];
                if (v is T)
                {
                    value = (T)v;
                    return true;
                }
            }

            value = default;
            return false;
        }

        public static T GetProperty<T>(this Application self, object key)
            => TryGetProperty<T>(self, key, out var value) ? value : throw new InvalidOperationException();

        public static bool TryGetActiveWindow(this Application self, [MaybeNullWhen(false)] out Window window)
        {
            foreach (var w in self.Windows)
            {
                if (w is Window found)
                {
                    window = found;
                    return true;
                }
            }

            window = default;
            return false;
        }
    }
}
