using IT.Ext;
using System;

namespace IT.Security.Cryptography.JinnServer.Internal.Services
{
    internal class Fault
    {
        /// <summary>
        /// Код ошибки
        /// </summary>
        /// <example>soapenv:Server</example>
        public String FaultCode { get; set; }

        /// <summary>
        /// Текст ошибки
        /// </summary>
        /// <example>Operation Not Found</example>
        public String FaultString { get; set; }

        /// <summary>
        /// Детали ошибки
        /// </summary>
        public FaultDetail Detail { get; set; }

        public override String ToString() => "[{0}][{1}]: {2}".Format(FaultCode, FaultString, Detail);
    }
}