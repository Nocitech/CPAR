using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPAR.Core
{
    public interface IArrayIndex
    {
        int Index { get; set; }
    }

    public static class IArrayIndexExtensions
    {
        public static void IndexArray(this IArrayIndex[] array)
        {
            if (array != null)
            {
                for (int i = 0; i < array.Length; ++i)
                {
                    array[i].Index = i;
                }
            }
        }

        public static void SetIndex(this IArrayIndex[] array, int index)
        {
            array.Foreach<IArrayIndex>((e) => e.Index = index);
        }

        public static void Foreach<T>(this T[] array, Action<T> action)
        {
            foreach (var element in array)
            {
                action(element);
            }
        }
    }
}
