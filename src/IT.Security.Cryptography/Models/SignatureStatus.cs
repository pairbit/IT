namespace IT.Security.Cryptography.Models;

public enum SignatureStatus
{
    /// <summary>
    /// недостаточно информации для определения статуса подписи, 
    /// возможно в случае, если не найден сертификат автора 
    /// либо для него недо-ступны полные и актуальные СОС.
    /// </summary>
    Unknown,

    /// <summary>
    /// Подпись недействительна
    /// </summary>
    Invalid,

    /// <summary>
    /// Подпись проверена успешно
    /// </summary>
    Valid,
}