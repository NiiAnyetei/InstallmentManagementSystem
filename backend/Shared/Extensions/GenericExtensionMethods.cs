using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Extensions
{
    public static class GenericExtensionMethods
    {
        public static void Locking<T>(this T source, Action<T> action)
        {
            if (source is null) return;

            lock (source)
            {
                action(source);
            }
        }
    }
}
