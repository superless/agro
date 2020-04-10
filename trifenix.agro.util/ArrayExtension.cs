using System.Collections.Generic;
using System.Linq;

namespace trifenix.agro.util {
    public static class ArrayExtension {

        public static T[] Add<T>(this T[] array, T element) {
            var list = array.ToList();
            list.Add(element);
            return list.ToArray();
        }

        public static T[] Add<T>(this T[] array, IEnumerable<T> elements) {
            var list = array.ToList();
            list.AddRange(elements);
            return list.ToArray();
        }

        public static T[] Remove<T>(this T[] array, int indexToRemove) {
            var list = array.ToList();
            list.RemoveAt(indexToRemove);
            return list.ToArray();
        }

    }

}