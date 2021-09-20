using Newtonsoft.Json;
using System;

namespace IT.Security.Cryptography.JinnServer.Internal.Services
{
    internal class Body
    {
        /// <summary>
        /// Системная ошибка
        /// </summary>
        public Fault Fault { get; set; }

        /// <summary>
        /// Бизнес ошибка
        /// </summary>
        public FaultInfo ServiceFaultInfo { get; set; }

        public DigestResponse DigestResponseType { get; set; }

        public SimpleValidationResponse ValidationResponseType { get; set; }

        public String SigningResponseType { get; set; }

        [JsonIgnore]
        public Object[] Responses => new Object[] { DigestResponseType, SigningResponseType, ValidationResponseType };
    }
}