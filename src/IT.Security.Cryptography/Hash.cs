using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace IT.Security.Cryptography;

public static class Hash
{
    public static class Name
    {
        public static String[] All { get; } = new[] { XXH32, XXH64, MD5, SHA1, SHA256, SHA384, SHA512, GOST_R3411_94, GOST_R3411_2012_256, GOST_R3411_2012_512 };

        public const String XXH32 = nameof(XXH32);
        public const String XXH64 = nameof(XXH64);
        public const String MD5 = nameof(MD5);
        public const String SHA1 = nameof(SHA1);
        public const String SHA256 = nameof(SHA256);
        public const String SHA384 = nameof(SHA384);
        public const String SHA512 = nameof(SHA512);

        /*
                                 | 2001/94        | 2012-256          | 2012-512
         Подпись ГОСТ Р 34.10    | 1.2.643.2.2.19 | 1.2.643.7.1.1.1.1 | 1.2.643.7.1.1.1.2
         Хэш     ГОСТ Р 34.11    | 1.2.643.2.2.9  | 1.2.643.7.1.1.2.2 | 1.2.643.7.1.1.2.3
         Хэш+подпись 34.11+34.10 | 1.2.643.2.2.3  | 1.2.643.7.1.1.3.2 | 1.2.643.7.1.1.3.3
        */

        //ГОСТ Р 34.11-94 старые
        //1.2.643.2.2.30.0
        //1.2.643.2.2.30.1

        /// <summary>
        /// ГОСТ Р 34.11–94
        /// </summary>
        public const String GOST_R3411_94 = nameof(GOST_R3411_94);

        /// <summary>
        /// ГОСТ Р 34.11–2012–256
        /// </summary>
        public const String GOST_R3411_2012_256 = nameof(GOST_R3411_2012_256);

        /// <summary>
        /// ГОСТ Р 34.11–2012–512
        /// </summary>
        public const String GOST_R3411_2012_512 = nameof(GOST_R3411_2012_512);

        //public const String RIPEMD160 = nameof(RIPEMD160);
        //public const String TripleDESKeyWrap = nameof(TripleDESKeyWrap);
        //public const String RC2 = nameof(RC2);
        //public const String DES = nameof(DES);
        //public const String TripleDES = nameof(TripleDES);
    }

    private static Dictionary<String, String> Empty { get; } = new(StringComparer.OrdinalIgnoreCase)
    {
        //xxh-32
        { Name.XXH32, "2E1723D3" },
        //xxh-64
        { Name.XXH64, "FC468381D06BCAEA" },
        //MD-5
        { "1.2.840.113549.2.5", "CE2C8AED9C2FA0CFBED56CBDA4D8BF07" },
        //SHA-1
        { "1.3.14.3.2.26", "3159FE421B3221381B3C778DC1C3C26E4540BE37" },
        //SHA-256
        { "2.16.840.1.101.3.4.2.1", "C6C094BC0054F9CBE34102FF49F86B3928B5AC09F3D2AC87E170D0500675921F" },
        //SHA-384
        { "2.16.840.1.101.3.4.2.2", "A3878693F6BD6CACB03E271B31A38050D7227904919801E69FA3496BB2A3E1CB163856B4C9BC5B74C3D988E7389EF906" },
        //SHA-512
        { "2.16.840.1.101.3.4.2.3", "F918671FEBC52CE97D7233B92C256292D74450FB6D922CBDF001AAFFC7A56ADE4582CABD81F3B8F6F19E7F732865E43A4D60FC8F49521F3F35DF700C31490AE3" },
        //GOST_R3411_94
        { "1.2.643.2.2.9", "53D6ACC8C1832D065EF1C5537D4A7D5DF340CB7D9B11E68BC8534266635AB1B4" },
        //GOST_R3411_2012_256
        { "1.2.643.7.1.1.2.2", "CFF3F5D505BF35F32B9F8D4844BB7ED4FB30F92684C90311768519E7DCBDFB24" },
        //GOST_R3411_2012_512
        { "1.2.643.7.1.1.2.3", "5CFDB5B69968D5CD9A064EC8414737ECCBB5FB9D11819E44CCE85A5159B158F7EC683B16DD2B3EC26A793942E4BE8ACE98D6A3D58108C5A5AD6009AFEA4C7A4B" },
    };

    static Hash()
    {
        Crypto.Init();
    }

    public static void Add<T>(params String[] names) where T : HashAlgorithm => CryptoConfig.AddAlgorithm(typeof(T), names);

    public static void Add<T>(String oid, params String[] names) where T : HashAlgorithm
    {
        Add<T>(names);
        CryptoConfig.AddOID(oid, names);
    }

    /// <summary>
    /// Получить результат вычислениях хеша от пустой строки
    /// </summary>
    public static String? GetEmpty(String alg) => Empty.TryGetValue(alg, out var value) ? value : default;

    /// <summary>
    /// CryptoConfig.MapNameToOID(alg)
    /// </summary>
    public static String TryGetOid(String alg) => CryptoConfig.MapNameToOID(alg);

    /// <inheritdoc cref="TryGetOid"/>
    /// <exception cref="ArgumentNullException"/>
    public static String GetOid(String alg)
    {
        var oid = TryGetOid(alg);

        if (oid is null) throw new ArgumentNullException(nameof(alg), $"Oid for hash algorithm '{alg}' not found.");

        return oid;
    }

    /// <summary>
    /// HashAlgorithm.Create
    /// </summary>
    public static HashAlgorithm? TryCreate(String alg)
    {
        if (alg.Equals("XXH32", StringComparison.OrdinalIgnoreCase)) return new XXH32().AsHashAlgorithm();
        if (alg.Equals("XXH64", StringComparison.OrdinalIgnoreCase)) return new XXH64().AsHashAlgorithm();

        var crypto = CryptoConfig.CreateFromName(alg);

        if (crypto == null) return null;

        if (crypto is HashAlgorithm hashAlgorithm) return hashAlgorithm;

        if (crypto is SignatureDescription signatureDescription) return signatureDescription.CreateDigest();

        throw new InvalidOperationException($"Hash algorithm '{alg}' not supported '{crypto.GetType().FullName}'");
    }

    /// <inheritdoc cref="TryCreate"/>
    /// <exception cref="ArgumentNullException"/>
    public static HashAlgorithm Create(String alg)
    {
        var hasher = TryCreate(alg);

        if (hasher is null) throw new ArgumentException($"Hash algorithm '{alg}' not found.", nameof(alg));

        return hasher!;
    }
}