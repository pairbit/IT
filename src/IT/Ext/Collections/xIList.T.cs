using System.Collections.Generic;

namespace IT.Ext
{
    public static class xIListT
    {
        public static T GetRandom<T>(this IList<T> list) => list == null || list.Count == 0 ? default : list[list.Count > 1 ? xRandom.Instance.Next(list.Count) : 0];
    }
}