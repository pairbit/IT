using System;
using System.Collections.Generic;

namespace IT.Ext
{
    public static class xException
    {
        public static IEnumerable<Exception> GetInnerExceptions(this Exception exception)
        {
            if (exception is AggregateException aggr) return aggr.InnerExceptions;
            return exception.InnerException != null ? exception.InnerException.InArray() : Array.Empty<Exception>();
        }

        public static IEnumerable<Exception> GetAllInnerExceptions(this Exception exception)
        {
            var list = new List<Exception>();
            AddInnerExceptions(exception, list);
            return list;
        }

        private static void AddInnerExceptions(Exception exception, List<Exception> list)
        {
            foreach (var innerException in exception.GetInnerExceptions())
            {
                list.Add(innerException);
                if (innerException.InnerException != null) AddInnerExceptions(innerException.InnerException, list);
            }
        }

        public static IEnumerable<Exception> All(this Exception exception)
        {
            if (exception is AggregateException aggregateException) return aggregateException.InnerExceptions;
            return exception.InArray();
        }
    }
}