using System.Collections.Generic;
using System.Linq;

namespace GrainImplement.CMS
{
    public static class ListExtend
    {
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> list)
        {
            return list != null && list.Any();
        }
    }
}
