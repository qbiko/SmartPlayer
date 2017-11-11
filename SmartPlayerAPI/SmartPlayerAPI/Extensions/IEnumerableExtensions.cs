using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartPlayerAPI.Extensions
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<T> LastN<T>(this IList<T> list, int n)
        {
            if(list == null)
            {
                throw new ArgumentException("list");
            }

            if(list.Count - n < 0)
            {
                n = list.Count;
            }

            for(var i = list.Count - n; i < list.Count; i++)
            {
                yield return list[i];
            }
        }
    }
}
