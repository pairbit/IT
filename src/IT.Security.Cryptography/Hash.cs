﻿using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace IT.Security.Cryptography;

public static class Hash
{
    public static class Name
    {
        public static String[] All { get; } = new[] { XXH32, XXH64, MD5, SHA1, SHA256, SHA384, SHA512 };

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


    /*
1.3.6.1.4.1.1722.12.2.1.5  |       BLAKE2B-160 | DE975D77EFA358554F3285658A037177F578D4CD
1.3.6.1.4.1.1722.12.2.1.8  |       BLAKE2B-256 | F436A0ECB418901CAE550A0FC4334F5220AFF7BCB8D60E75DA954CDE35D4BC7D
1.3.6.1.4.1.1722.12.2.1.12 |       BLAKE2B-384 | 2A584070B377A4E401F84D9FD04E6124C07FF43096B81278261E78C0AC22E1FBE7F9008F92D09DDCE5A776753BD4E636
1.3.6.1.4.1.1722.12.2.1.16 |       BLAKE2B-512 | FDD62AF7E4242FF0FADD2310C924DC3C687EB1E0D2047299D2C52E7C7C2E5C28F6B65C7945A45E8FBE9AA7C4432685AFB522AD141FA3CAFD0F5BD0C297608E61
1.3.6.1.4.1.1722.12.2.2.4  |       BLAKE2S-128 | 3D29F3D7BBCE5064465D038939340E4D
1.3.6.1.4.1.1722.12.2.2.5  |       BLAKE2S-160 | DE6D703D28A378087B8CE0DC395B8079DCA81185
1.3.6.1.4.1.1722.12.2.2.7  |       BLAKE2S-224 | 888D880F7E569B745DA9ED186DD868A40BC43E95F802ECA54DBA1B94
1.3.6.1.4.1.1722.12.2.2.8  |       BLAKE2S-256 | 1692A53D8E8EF5F28D77C3ADB0EDC2B37AF61BE2A6D1243917426F912EC50F62
1.2.804.2.1.1.1.1.2.2.1    |      DSTU7564-256 | D151C41F439D12BF233FED968A42434C5D2BB499F422F6841F960CD593734F49
1.2.804.2.1.1.1.1.2.2.2    |      DSTU7564-384 | AFE547622B01C9742590B70C426FCF582FE2FDFE3F255998AF2BBD81FCEA6AF19525E3C91A285F52EC4E2EA610C16D31
1.2.804.2.1.1.1.1.2.2.3    |      DSTU7564-512 | 84B3AE4947F94AEB12E0DB1AC63E55BAAFE547622B01C9742590B70C426FCF582FE2FDFE3F255998AF2BBD81FCEA6AF19525E3C91A285F52EC4E2EA610C16D31
1.2.643.2.2.9              |          GOST3411 | 53D6ACC8C1832D065EF1C5537D4A7D5DF340CB7D9B11E68BC8534266635AB1B4
1.2.643.7.1.1.2.2          | GOST3411-2012-256 | CFF3F5D505BF35F32B9F8D4844BB7ED4FB30F92684C90311768519E7DCBDFB24
1.2.643.7.1.1.2.3          | GOST3411-2012-512 | 5CFDB5B69968D5CD9A064EC8414737ECCBB5FB9D11819E44CCE85A5159B158F7EC683B16DD2B3EC26A793942E4BE8ACE98D6A3D58108C5A5AD6009AFEA4C7A4B
1.2.840.113549.2.2         |               MD2 | 03DEBEADC622365496477604BEE3EC59
1.2.840.113549.2.4         |               MD4 | 228244C4E74C7C4662F683BDECBE7807
1.2.840.113549.2.5         |               MD5 | CE2C8AED9C2FA0CFBED56CBDA4D8BF07
1.3.36.3.2.2               |         RIPEMD128 | E4D852166B0512115649046AAE6A250B
1.3.36.3.2.1               |         RIPEMD160 | 7E92E8AA27CED913BA732409070644DFBCEC35DF
1.3.36.3.2.3               |         RIPEMD256 | 7B9AEA3DF58624CFD209F6D863E005004D81BBCBE97333D42D43458D6FD5593F
1.3.14.3.2.26              |             SHA-1 | 3159FE421B3221381B3C778DC1C3C26E4540BE37
2.16.840.1.101.3.4.2.4     |           SHA-224 | 92D2D0E4079627C2B3ED27BC4459937CCB9F095D170B2BDE781E9E7E
2.16.840.1.101.3.4.2.1     |           SHA-256 | C6C094BC0054F9CBE34102FF49F86B3928B5AC09F3D2AC87E170D0500675921F
2.16.840.1.101.3.4.2.2     |           SHA-384 | A3878693F6BD6CACB03E271B31A38050D7227904919801E69FA3496BB2A3E1CB163856B4C9BC5B74C3D988E7389EF906
2.16.840.1.101.3.4.2.3     |           SHA-512 | F918671FEBC52CE97D7233B92C256292D74450FB6D922CBDF001AAFFC7A56ADE4582CABD81F3B8F6F19E7F732865E43A4D60FC8F49521F3F35DF700C31490AE3
2.16.840.1.101.3.4.2.5     |       SHA-512/224 | 9289D7D85525C3CF45AC8FF77D697A4841072461C82991130AB1F1C1
2.16.840.1.101.3.4.2.6     |       SHA-512/256 | 1E1ADE9BC035951E0317F5E4FB6CB65E8D3388CA6DFBD1E5A8380D75DC1C695D
2.16.840.1.101.3.4.2.7     |          SHA3-224 | 18FA71F22B163F5268FFECF8410ACBB1D85399AD8295BC2E944364AF
2.16.840.1.101.3.4.2.8     |          SHA3-256 | F26E40C9ABF4E9F4EF45AA69B30D795BF12C4586468C02801EB05EFE45A05354
2.16.840.1.101.3.4.2.9     |          SHA3-384 | 5E9EFA63524D65568F86DD092E9434B05B3E5365B7AA74B84BBECA913776ACF997B985B92A3C8B76EDFE7393CCDA6299
2.16.840.1.101.3.4.2.10    |          SHA3-512 | 424FE8585FF33CE8933DB45D39F0B388DB7C0D7F8C3438F747BD11D84C15C489C2D9F6C52E9D7FF25682484CFCC8941264CCA38C7742114EA69624B530C154AF
2.16.840.1.101.3.4.2.11    |      SHAKE128-256 | 85FCF53B271F74D313917968FD6DA6AEE757121F03EE5571D0FF4CDA2E5D61C1
2.16.840.1.101.3.4.2.12    |      SHAKE256-512 | CD49A5C52B15590DC783EA034B7AECDBAC1B57EE6C28D7B230FD77681F6C03CC993114B7564CE67922C3122640DAD69FF16A9E7E9208D83151CEB5926D37BEDA
1.2.156.10197.1.401        |               SM3 | EE660FA36C288745A600CD9C0449E097553CDFCFD6FA6EB033692774559F2F4E
     
     */

    private static readonly Dictionary<String, String> _algs = new(StringComparer.OrdinalIgnoreCase)
    {
        { "XXH32", "XXH32" },
        { "XXH-32", "XXH32" },
        { "XXH64", "XXH64" },
        { "XXH-64", "XXH64" },

        { "BLAKE2B-160", "1.3.6.1.4.1.1722.12.2.1.5" },
        { "BLAKE2B-256", "1.3.6.1.4.1.1722.12.2.1.8" },
        { "BLAKE2B-384", "1.3.6.1.4.1.1722.12.2.1.12" },
        { "BLAKE2B-512", "1.3.6.1.4.1.1722.12.2.1.16" },

        { "BLAKE2S-128", "1.3.6.1.4.1.1722.12.2.2.4" },
        { "BLAKE2S-160", "1.3.6.1.4.1.1722.12.2.2.5" },
        { "BLAKE2S-224", "1.3.6.1.4.1.1722.12.2.2.7" },
        { "BLAKE2S-256", "1.3.6.1.4.1.1722.12.2.2.8" },

        { "DSTU7564-256", "1.2.804.2.1.1.1.1.2.2.1" },
        { "DSTU7564-384", "1.2.804.2.1.1.1.1.2.2.2" },
        { "DSTU7564-512", "1.2.804.2.1.1.1.1.2.2.3" },

        { "GOST3411", "1.2.643.2.2.9" },
        { "GOST_R3411_94", "1.2.643.2.2.9" },
        { "urn:ietf:params:xml:ns:cpxmlsec:algorithms:gostr3411", "1.2.643.2.2.9" },
        { "urn:ietf:params:xml:ns:cpxmlsec:algorithms:gostr34102001-gostr3411", "1.2.643.2.2.9" },

        { "GOST3411-2012-256", "1.2.643.7.1.1.2.2" },
        { "GOST_R3411_2012_256", "1.2.643.7.1.1.2.2" },
        { "urn:ietf:params:xml:ns:cpxmlsec:algorithms:gostr34112012-256", "1.2.643.7.1.1.2.2" },
        { "urn:ietf:params:xml:ns:cpxmlsec:algorithms:gostr34102012-gostr34112012-256", "1.2.643.7.1.1.2.2" },

        { "GOST3411-2012-512", "1.2.643.7.1.1.2.3" },
        { "GOST_R3411_2012_512", "1.2.643.7.1.1.2.3" },
        { "urn:ietf:params:xml:ns:cpxmlsec:algorithms:gostr34112012-512", "1.2.643.7.1.1.2.3" },
        { "urn:ietf:params:xml:ns:cpxmlsec:algorithms:gostr34102012-gostr34112012-512", "1.2.643.7.1.1.2.3" },

        { "MD2", "1.2.840.113549.2.2" },
        { "MD-2", "1.2.840.113549.2.2" },

        { "MD4", "1.2.840.113549.2.4" },
        { "MD-4", "1.2.840.113549.2.4" },

        { "MD5", "1.2.840.113549.2.5" },
        { "MD-5", "1.2.840.113549.2.5" },

        { "RIPEMD128", "1.3.36.3.2.2" },
        { "RIPEMD-128", "1.3.36.3.2.2" },

        { "RIPEMD160", "1.3.36.3.2.1" },
        { "RIPEMD-160", "1.3.36.3.2.1" },

        { "RIPEMD256", "1.3.36.3.2.3" },
        { "RIPEMD-256", "1.3.36.3.2.3" },

        { "SHA1", "1.3.14.3.2.26" },
        { "SHA-1", "1.3.14.3.2.26" },

        { "SHA224", "2.16.840.1.101.3.4.2.4" },
        { "SHA-224", "2.16.840.1.101.3.4.2.4" },

        { "SHA256", "2.16.840.1.101.3.4.2.1" },
        { "SHA-256", "2.16.840.1.101.3.4.2.1" },

        { "SHA384", "2.16.840.1.101.3.4.2.2" },
        { "SHA-384", "2.16.840.1.101.3.4.2.2" },

        { "SHA512", "2.16.840.1.101.3.4.2.3" },
        { "SHA-512", "2.16.840.1.101.3.4.2.3" },

        { "SHA512/224", "2.16.840.1.101.3.4.2.5" },
        { "SHA-512/224", "2.16.840.1.101.3.4.2.5" },

        { "SHA512/256", "2.16.840.1.101.3.4.2.6" },
        { "SHA-512/256", "2.16.840.1.101.3.4.2.6" },

        { "SHA3-224", "2.16.840.1.101.3.4.2.7" },

        { "SHA3-256", "2.16.840.1.101.3.4.2.8" },

        { "SHA3-384", "2.16.840.1.101.3.4.2.9" },

        { "SHA3-512", "2.16.840.1.101.3.4.2.10" },
        
        { "SHAKE128-256", "2.16.840.1.101.3.4.2.11" },
        { "SHAKE-128-256", "2.16.840.1.101.3.4.2.11" },
        
        { "SHAKE256-512", "2.16.840.1.101.3.4.2.12" },
        { "SHAKE-256-512", "2.16.840.1.101.3.4.2.12" },

        { "SM3", "1.2.156.10197.1.401" },
        { "SM-3", "1.2.156.10197.1.401" },
    };

    private static readonly Dictionary<String, String> _empty = new(StringComparer.OrdinalIgnoreCase)
    {
        //XXH-32
        { Name.XXH32, "2E1723D3" },
        //XXH-64
        { Name.XXH64, "FC468381D06BCAEA" },

        //BLAKE2B-160
        { "1.3.6.1.4.1.1722.12.2.1.5", "DE975D77EFA358554F3285658A037177F578D4CD" },
        //BLAKE2B-256
        { "1.3.6.1.4.1.1722.12.2.1.8", "F436A0ECB418901CAE550A0FC4334F5220AFF7BCB8D60E75DA954CDE35D4BC7D" },
        //BLAKE2B-384
        { "1.3.6.1.4.1.1722.12.2.1.12", "2A584070B377A4E401F84D9FD04E6124C07FF43096B81278261E78C0AC22E1FBE7F9008F92D09DDCE5A776753BD4E636" },
        //BLAKE2B-512
        { "1.3.6.1.4.1.1722.12.2.1.16", "FDD62AF7E4242FF0FADD2310C924DC3C687EB1E0D2047299D2C52E7C7C2E5C28F6B65C7945A45E8FBE9AA7C4432685AFB522AD141FA3CAFD0F5BD0C297608E61" },
        
        //BLAKE2S-128
        { "1.3.6.1.4.1.1722.12.2.2.4", "3D29F3D7BBCE5064465D038939340E4D" },
        //BLAKE2S-160
        { "1.3.6.1.4.1.1722.12.2.2.5", "DE6D703D28A378087B8CE0DC395B8079DCA81185" },
        //BLAKE2S-224
        { "1.3.6.1.4.1.1722.12.2.2.7", "888D880F7E569B745DA9ED186DD868A40BC43E95F802ECA54DBA1B94" },
        //BLAKE2S-256
        { "1.3.6.1.4.1.1722.12.2.2.8", "1692A53D8E8EF5F28D77C3ADB0EDC2B37AF61BE2A6D1243917426F912EC50F62" },
        
        //DSTU7564-256
        { "1.2.804.2.1.1.1.1.2.2.1", "D151C41F439D12BF233FED968A42434C5D2BB499F422F6841F960CD593734F49" },
        //DSTU7564-384
        { "1.2.804.2.1.1.1.1.2.2.2", "AFE547622B01C9742590B70C426FCF582FE2FDFE3F255998AF2BBD81FCEA6AF19525E3C91A285F52EC4E2EA610C16D31" },
        //DSTU7564-512
        { "1.2.804.2.1.1.1.1.2.2.3", "84B3AE4947F94AEB12E0DB1AC63E55BAAFE547622B01C9742590B70C426FCF582FE2FDFE3F255998AF2BBD81FCEA6AF19525E3C91A285F52EC4E2EA610C16D31" },

        //GOST_R3411_94
        { "1.2.643.2.2.9", "53D6ACC8C1832D065EF1C5537D4A7D5DF340CB7D9B11E68BC8534266635AB1B4" },
        //GOST_R3411_2012_256
        { "1.2.643.7.1.1.2.2", "CFF3F5D505BF35F32B9F8D4844BB7ED4FB30F92684C90311768519E7DCBDFB24" },
        //GOST_R3411_2012_512
        { "1.2.643.7.1.1.2.3", "5CFDB5B69968D5CD9A064EC8414737ECCBB5FB9D11819E44CCE85A5159B158F7EC683B16DD2B3EC26A793942E4BE8ACE98D6A3D58108C5A5AD6009AFEA4C7A4B" },

        //MD-2
        { "1.2.840.113549.2.2", "03DEBEADC622365496477604BEE3EC59" },
        //MD-4
        { "1.2.840.113549.2.4", "228244C4E74C7C4662F683BDECBE7807" },
        //MD-5
        { "1.2.840.113549.2.5", "CE2C8AED9C2FA0CFBED56CBDA4D8BF07" },

        //RIPEMD-128
        { "1.3.36.3.2.2", "E4D852166B0512115649046AAE6A250B" },
        //RIPEMD-160
        { "1.3.36.3.2.1", "7E92E8AA27CED913BA732409070644DFBCEC35DF" },
        //RIPEMD-256
        { "1.3.36.3.2.3", "7B9AEA3DF58624CFD209F6D863E005004D81BBCBE97333D42D43458D6FD5593F" },

        //SHA-1
        { "1.3.14.3.2.26", "3159FE421B3221381B3C778DC1C3C26E4540BE37" },
        //SHA-224
        { "2.16.840.1.101.3.4.2.4", "92D2D0E4079627C2B3ED27BC4459937CCB9F095D170B2BDE781E9E7E" },
        //SHA-256
        { "2.16.840.1.101.3.4.2.1", "C6C094BC0054F9CBE34102FF49F86B3928B5AC09F3D2AC87E170D0500675921F" },
        //SHA-384
        { "2.16.840.1.101.3.4.2.2", "A3878693F6BD6CACB03E271B31A38050D7227904919801E69FA3496BB2A3E1CB163856B4C9BC5B74C3D988E7389EF906" },
        //SHA-512
        { "2.16.840.1.101.3.4.2.3", "F918671FEBC52CE97D7233B92C256292D74450FB6D922CBDF001AAFFC7A56ADE4582CABD81F3B8F6F19E7F732865E43A4D60FC8F49521F3F35DF700C31490AE3" },
        //SHA-512/224
        { "2.16.840.1.101.3.4.2.5", "9289D7D85525C3CF45AC8FF77D697A4841072461C82991130AB1F1C1" },
        //SHA-512/256
        { "2.16.840.1.101.3.4.2.6", "1E1ADE9BC035951E0317F5E4FB6CB65E8D3388CA6DFBD1E5A8380D75DC1C695D" },
        //SHA3-224
        { "2.16.840.1.101.3.4.2.7", "18FA71F22B163F5268FFECF8410ACBB1D85399AD8295BC2E944364AF" },
        //SHA3-256
        { "2.16.840.1.101.3.4.2.8", "F26E40C9ABF4E9F4EF45AA69B30D795BF12C4586468C02801EB05EFE45A05354" },
        //SHA3-384
        { "2.16.840.1.101.3.4.2.9", "5E9EFA63524D65568F86DD092E9434B05B3E5365B7AA74B84BBECA913776ACF997B985B92A3C8B76EDFE7393CCDA6299" },
        //SHA3-512
        { "2.16.840.1.101.3.4.2.10", "424FE8585FF33CE8933DB45D39F0B388DB7C0D7F8C3438F747BD11D84C15C489C2D9F6C52E9D7FF25682484CFCC8941264CCA38C7742114EA69624B530C154AF" },
        //SHAKE-128-256
        { "2.16.840.1.101.3.4.2.11", "85FCF53B271F74D313917968FD6DA6AEE757121F03EE5571D0FF4CDA2E5D61C1" },
        //SHAKE-256-512
        { "2.16.840.1.101.3.4.2.12", "CD49A5C52B15590DC783EA034B7AECDBAC1B57EE6C28D7B230FD77681F6C03CC993114B7564CE67922C3122640DAD69FF16A9E7E9208D83151CEB5926D37BEDA" },

        //SM3
        { "1.2.156.10197.1.401", "EE660FA36C288745A600CD9C0449E097553CDFCFD6FA6EB033692774559F2F4E" }
    };

    static Hash()
    {
        Add<XXH32>(Name.XXH32);
        Add<XXH64>(Name.XXH64);

        foreach (var alg in _algs)
        {
            var name = alg.Key;
            var oid = alg.Value;
            CryptoConfig.AddOID(oid, name);
        }

        Crypto.Init();
    }

    public static void Add<T>(params String[] names) where T : HashAlgorithm => CryptoConfig.AddAlgorithm(typeof(T), names);

    /// <summary>
    /// Получить результат вычислениях хеша от строки 'Empty'
    /// </summary>
    public static String GetEmptyByOid(String oid) => _empty.TryGetValue(oid, out var value) ? value : throw new ArgumentException($"Oid '{oid}' not found", nameof(oid));

    /// <summary>
    /// CryptoConfig.MapNameToOID(alg)
    /// </summary>
    public static String? TryGetOid(String alg) => CryptoConfig.MapNameToOID(alg);

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