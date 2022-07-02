namespace IT.Security.Cryptography.Models;

public enum SignaturesStatus
{
    /// <summary>
    /// недостаточно информации для определения статуса ни одной из имеющихся подписей, 
    /// возможно в случае, если не найдены сертификаты авторов 
    /// либо для них недоступны полные и актуальные СОС.
    /// </summary>
    Unknown,

    /// <summary>
    /// Все имеющиеся подписи недействительны
    /// </summary>
    Invalid,

    /// <summary>
    /// Все подписи проверены успешно
    /// </summary>
    Valid,

    /// <summary>
    /// Часть подписей действительна, часть – нет.
    /// </summary>
    PartiallyValid
}