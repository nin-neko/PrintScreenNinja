using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Media;


namespace PrintScreenNinja
{
    static class WpfHelper
    {
        public static bool TryFindParent<T>(this DependencyObject dependencyObject, [MaybeNullWhen(false)] out T parent)
            where T : DependencyObject
        {
            var p = VisualTreeHelper.GetParent(dependencyObject);
            while (true)
            {
                if (p is null) break;
                if (p is not DependencyObject obj) break;

                if (p is T found)
                {
                    parent = found;
                    return true;
                }

                p = VisualTreeHelper.GetParent(obj);
            }

            parent = default;
            return false;
        }

        public static IEnumerable<DependencyObject> EnumerateVisuals(this DependencyObject dependencyObject)
        {
            var count = VisualTreeHelper.GetChildrenCount(dependencyObject);
            for (var i = 0; i < count; i++)
            {
                var childVisual = VisualTreeHelper.GetChild(dependencyObject, i);

                yield return childVisual;

                foreach (var v in EnumerateVisuals(childVisual))
                {
                    yield return v;
                }
            }
        }
    }
}
