using IT.Ext;
using IT.Validation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace IT.Exceptions
{
    /// <summary>
    /// Ошибка
    /// </summary>
    public class Error
    {
        /// <summary>
        /// Сообщение об ошибке
        /// </summary>
        public String[] Message { get; set; }

        /// <summary>
        /// Тип исключения
        /// </summary>
        public String Type { get; set; }

        /// <summary>
        /// Источник ошибки
        /// </summary>
        public String Source { get; set; }

        /// <summary>
        /// Дополнительная информация
        /// </summary>
        public IDictionary Data { get; set; }

        /// <summary>
        /// Стек ошибки
        /// </summary>
        public String[] StackTrace { get; set; }

        /// <summary>
        /// Вложеные ошибки
        /// </summary>
        public Error[] Inners { get; set; }

        /// <summary>
        /// Подробности
        /// </summary>
        public Boolean HasDetail { get; set; }

        public Error()
        {

        }

        public Error(Exception exception, Boolean detail = true, Boolean inner = true)
        {
            Arg.NotNull(exception, nameof(exception));

            if (exception is TargetInvocationException iException && iException.InnerException != null) exception = iException.InnerException;

            Arg.NotNull(exception.Message, nameof(exception.Message));
            HasDetail = detail;
            Message = exception.Message.Split(Environment.NewLine);
            if (inner)
            {
                var inners = exception.GetInnerExceptions().Select(x => new Error(x, detail, inner)).ToArray();
                if (inners.Length > 0) Inners = inners;
            }
            if (!exception.Data.IsEmpty()) Data = exception.Data;
            if (!detail) return;
            if (exception.StackTrace != null) StackTrace = exception.StackTrace.Split(Environment.NewLine);
            Source = exception.Source;
            Type = exception.GetType().FullName;
        }

        public Error[] All()
        {
            var errors = new List<Error>();
            AddInner(this, errors);
            return errors.ToArray();
        }

        private static void AddInner(Error error, List<Error> errors)
        {
            errors.Add(error);
            if (error.Inners == null) return;
            foreach (var inner in error.Inners) AddInner(inner, errors);
        }
    }
}