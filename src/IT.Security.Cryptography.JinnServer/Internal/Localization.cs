using System;

namespace IT.Security.Cryptography.JinnServer.Internal;

internal static class Localization
{
    public static String Localize(this GlobalStatus status) => Res.ResourceManager.GetString($"{nameof(GlobalStatus)}_{status}");

    public static String Localize(this SignatureStatus status) => Res.ResourceManager.GetString($"{nameof(SignatureStatus)}_{status}");
}