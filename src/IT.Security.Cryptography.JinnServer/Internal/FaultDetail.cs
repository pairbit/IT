using System;

namespace IT.Security.Cryptography.JinnServer.Internal;

internal class FaultDetail
{
    /// <summary>
    /// Текст исключения
    /// </summary>
    /// <example>Operation Not Found, Endpoint referance address is http://195.230.101.103:8080/tccs/SigningService/?Q and wsa actions is (null)</example>
    public String? Exception { get; set; }

    /// <summary>
    /// Бизнес ошибка
    /// </summary>
    public FaultInfo? ServiceFaultInfo { get; set; }

    public override String ToString() => Exception ?? ServiceFaultInfo?.ToString();
}