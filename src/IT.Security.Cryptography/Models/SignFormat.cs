namespace IT.Security.Cryptography.Models;

/// <summary>
/// Формат подписи
/// </summary>
public enum SignFormat
{
    /// <summary>
    /// Cades-BES - подпись текста или бинарных данных
    /// </summary>
    CadesBES,

    /// <summary>
    /// Cades-C - подпись текста после усиления
    /// </summary>
    CadesC,

    /// <summary>
    /// Xades-BES - подпись xml и xslt
    /// </summary>
    XadesBES,

    /// <summary>
    /// Xades-C - подпись xml и xslt после усиления
    /// </summary>
    XadesC
}