using System.ComponentModel.DataAnnotations;

namespace IT.Security.Cryptography
{
    /// <summary>
    /// Формат подписи
    /// </summary>
    public enum SignFormat
    {
        /// <summary>
        /// Cades-BES - подпись текста или бинарных данных
        /// </summary>
        [Display(Name = "cades-bes")]
        CadesBES,

        /// <summary>
        /// Cades-C - подпись текста после усиления
        /// </summary>
        [Display(Name = "cades-c")]
        CadesC,

        /// <summary>
        /// Xades-BES - подпись xml и xslt
        /// </summary>
        [Display(Name = "xades-bes")]
        XadesBES,

        /// <summary>
        /// Xades-C - подпись xml и xslt после усиления
        /// </summary>
        [Display(Name = "xades-c")]
        XadesC
    }
}