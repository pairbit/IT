﻿using IT.Security.Cryptography;
using IT.Security.Cryptography.JinnServer;
using IT.Security.Cryptography.Models;
using System.Reflection;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

string? url = null;

if (args.Length > 0)
{
    var argUrl = args[0];
    if (!string.IsNullOrWhiteSpace(argUrl))
    {
        url = argUrl;
        Console.WriteLine($"Url address JinnServer '{url}'");
    }
}

if (url == null)
{
    Console.WriteLine($"Enter url address JinnServer (example: http://0.0.0.0:8080/tccs/SignatureValidationService)");
    Console.Write("Url JinnServer: ");
    url = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(url))
    {
        Console.WriteLine($"Url address JinnServer '{url}' is empty or whitespace");
        return;
    } 
}

ISignatureVerifier signVerifier = new ValidationService(() => new ValidationOptions { ValidationUrl = url });

if (Check(signVerifier))
{
    var location = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

    var rootPath = Path.GetFullPath(Path.Combine(location!, "sign"));

    if (!Directory.Exists(rootPath))
    {
        Console.WriteLine($"Directory sign not found '{rootPath}'");
        return;
    }

    Console.WriteLine($"Directory sign found '{rootPath}'");

    var files = Directory.GetFiles(rootPath, "*.xml", SearchOption.TopDirectoryOnly);

    var count = files.Length;

    var width = count.ToString().Length;

    Console.WriteLine($"Found {count} signs by mask '*.xml'");

    var options = new JsonSerializerOptions
    {
        WriteIndented = true,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DictionaryKeyPolicy = JsonNamingPolicy.CamelCase
    };
    options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));

    for (int i = 0; i < files.Length; i++)
    {
        var file = files[i];
        var dirname = Path.GetDirectoryName(file)!;
        var filename = Path.GetFileName(file);

        var no = (i + 1).ToString().PadRight(width);

        Console.WriteLine();
        Console.Write($"[{no}] {filename}: ");

        try
        {
            var detailPath = Path.Combine(dirname, $"{filename}.verifed.json");

            if (File.Exists(detailPath))
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.Write("Detail verifed exist");
                Console.ResetColor();
                continue;
            }

            var signature = File.ReadAllText(file);

            var detail = signVerifier.VerifyDetail(signature);

            var contents = JsonSerializer.Serialize(detail, options);

            File.WriteAllText(detailPath, contents);

            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write("Detail verifed save");
            Console.ResetColor();
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;

            Console.Write(ex.Message);

            var inner = ex.InnerException;

            if (inner != null)
                Console.Write($" -> {inner.Message}");
        }
        finally
        {
            Console.ResetColor();
            Console.WriteLine();
        }
    }
}

const string SignatureValid = "PD94bWwgdmVyc2lvbj0iMS4wIiBlbmNvZGluZz0idXRmLTgiPz4KPERvYz48RmllbGQ+0JfQvdCw0YfQtdC90LjQtSDQv9C+0LvRjyDQtNC+0LrRg9C80LXQvdGC0LA8L0ZpZWxkPjxkczpTaWduYXR1cmUgeG1sbnM6ZHM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvMDkveG1sZHNpZyMiIElkPSJzaWctWXVrMnhJbFYzbENPNGJVSSI+PGRzOlNpZ25lZEluZm8geG1sbnM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvMDkveG1sZHNpZyMiPjxkczpDYW5vbmljYWxpemF0aW9uTWV0aG9kIEFsZ29yaXRobT0iaHR0cDovL3d3dy53My5vcmcvMjAwMS8xMC94bWwtZXhjLWMxNG4jIi8+PGRzOlNpZ25hdHVyZU1ldGhvZCBBbGdvcml0aG09InVybjppZXRmOnBhcmFtczp4bWw6bnM6Y3B4bWxzZWM6YWxnb3JpdGhtczpnb3N0cjM0MTAyMDEyLWdvc3RyMzQxMTIwMTItMjU2Ii8+PGRzOlJlZmVyZW5jZSBJZD0icmVmLVl1azJ4SWxWM2xDTzRiVUkiPjxkczpUcmFuc2Zvcm1zPjxkczpUcmFuc2Zvcm0gQWxnb3JpdGhtPSJodHRwOi8vd3d3LnczLm9yZy8yMDAwLzA5L3htbGRzaWcjZW52ZWxvcGVkLXNpZ25hdHVyZSIvPjxkczpUcmFuc2Zvcm0gQWxnb3JpdGhtPSJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzEwL3htbC1leGMtYzE0biMiLz48L2RzOlRyYW5zZm9ybXM+PGRzOkRpZ2VzdE1ldGhvZCBBbGdvcml0aG09InVybjppZXRmOnBhcmFtczp4bWw6bnM6Y3B4bWxzZWM6YWxnb3JpdGhtczpnb3N0cjM0MTEyMDEyLTI1NiIvPjxkczpEaWdlc3RWYWx1ZT5QbTdvMmtady81K3NlZUR0ZWdnWW9GWnVLUVNlVlpDYWJvaEp6U0thSUpJPTwvZHM6RGlnZXN0VmFsdWU+PC9kczpSZWZlcmVuY2U+PGRzOlJlZmVyZW5jZSBJZD0icmVmLXNpZ25lZHByb3BzLVl1azJ4SWxWM2xDTzRiVUkiIFR5cGU9Imh0dHA6Ly91cmkuZXRzaS5vcmcvMDE5MDMvI1NpZ25lZFByb3BlcnRpZXMiIFVSST0iI3NpZ25lZHByb3BzLVl1azJ4SWxWM2xDTzRiVUkiPjxkczpUcmFuc2Zvcm1zPjxkczpUcmFuc2Zvcm0gQWxnb3JpdGhtPSJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzEwL3htbC1leGMtYzE0biMiLz48L2RzOlRyYW5zZm9ybXM+PGRzOkRpZ2VzdE1ldGhvZCBBbGdvcml0aG09InVybjppZXRmOnBhcmFtczp4bWw6bnM6Y3B4bWxzZWM6YWxnb3JpdGhtczpnb3N0cjM0MTEyMDEyLTI1NiIvPjxkczpEaWdlc3RWYWx1ZT4zMmV3RWMxRTNRSFo5UDY3N0NCZWNxbUxVQnhtTVM0UzFvdWRCZzM4V1V3PTwvZHM6RGlnZXN0VmFsdWU+PC9kczpSZWZlcmVuY2U+PGRzOlJlZmVyZW5jZSBJZD0icmVmLWtleS1ZdWsyeElsVjNsQ080YlVJIiBUeXBlPSJodHRwOi8vd3d3LnczLm9yZy9UUi8yMDAyL1JFQy14bWxkc2lnLWNvcmUtMjAwMjAyMTIveG1sZHNpZy1jb3JlLXNjaGVtYS54c2QjWDUwOURhdGEiIFVSST0iI2tleS1ZdWsyeElsVjNsQ080YlVJIj48ZHM6VHJhbnNmb3Jtcz48ZHM6VHJhbnNmb3JtIEFsZ29yaXRobT0iaHR0cDovL3d3dy53My5vcmcvMjAwMS8xMC94bWwtZXhjLWMxNG4jIi8+PC9kczpUcmFuc2Zvcm1zPjxkczpEaWdlc3RNZXRob2QgQWxnb3JpdGhtPSJ1cm46aWV0ZjpwYXJhbXM6eG1sOm5zOmNweG1sc2VjOmFsZ29yaXRobXM6Z29zdHIzNDExMjAxMi0yNTYiLz48ZHM6RGlnZXN0VmFsdWU+WVhjM21UOFU0aC90aS9tYlljM2p5M3RXQS9iMzJYM0N0YndaRjNRbHBSOD08L2RzOkRpZ2VzdFZhbHVlPjwvZHM6UmVmZXJlbmNlPjwvZHM6U2lnbmVkSW5mbz48ZHM6U2lnbmF0dXJlVmFsdWU+aXkvcGQ0d2NEOGpjUmdKUWtXOUFIbmJsQUN6ZWZmUmNFTGQzTko4WjJsZElHZnNGOWxnUGdabVd0Q2V2QlVRaQo0dUpCUFhpZ0lndjFCVVp5ZUZaNXNRPT08L2RzOlNpZ25hdHVyZVZhbHVlPjxkczpLZXlJbmZvIElkPSJrZXktWXVrMnhJbFYzbENPNGJVSSI+PGRzOlg1MDlEYXRhPjxkczpYNTA5Q2VydGlmaWNhdGU+TUlJSWpqQ0NDRHVnQXdJQkFnSUNHTG93Q2dZSUtvVURCd0VCQXdJd2dnRlpNU0F3SGdZSktvWklodmNOQVFrQgpGaEYxWTE5bWEwQnliM05yWVhwdVlTNXlkVEVaTUJjR0ExVUVDQXdRMExNdUlOQ2MwTDdSZ2RDNjBMTFFzREVhCk1CZ0dDQ3FGQXdPQkF3RUJFZ3d3TURjM01UQTFOamczTmpBeEdEQVdCZ1VxaFFOa0FSSU5NVEEwTnpjNU56QXgKT1Rnek1ERXNNQ29HQTFVRUNRd2owWVBRdTlDNDBZYlFzQ0RRbU5DNzBZelF1TkM5MExyUXNDd2cwTFRRdnRDOApJRGN4RlRBVEJnTlZCQWNNRE5DYzBMN1JnZEM2MExMUXNERUxNQWtHQTFVRUJoTUNVbFV4U0RCR0JnTlZCQW9NClA5Q2YwTDdRdE5HSDBMalF2ZEdSMEwzUXZkR0wwTGtnMFlMUXRkR0IwWUxRdnRDeTBZdlF1U0RRbzlDbUlOQ2sKMEpvZzBKUFFudENoMEtJdE1qQXhNakZJTUVZR0ExVUVBd3cvMEovUXZ0QzAwWWZRdU5DOTBaSFF2ZEM5MFl2UQp1U0RSZ3RDMTBZSFJndEMrMExMUmk5QzVJTkNqMEtZZzBLVFFtaURRazlDZTBLSFFvaTB5TURFeU1CNFhEVEl4Ck1Ea3hNekE0TlRBeE9Wb1hEVEl5TVRJeE16QTROVEF4T1Zvd2dnSFBNUlV3RXdZRktvVURaQVFUQ2pjM01UQTEKTmpnM05qQXhGakFVQmdVcWhRTmtBeElMTWpFeU5ETTFORFkxTnpneEdEQVdCZ1VxaFFOa0FSSU5NVEEwTnpjNQpOekF4T1Rnek1ERVlNQllHQTFVRUNRd1AwSnZRdGRDOTBMalF2ZEN3TERrd01TSXdJQVlKS29aSWh2Y05BUWtCCkZoTmliM0p2ZG01NWIzWmhRR3hoYm1sMExuSjFNUXN3Q1FZRFZRUUdFd0pTVlRFWk1CY0dBMVVFQ0F3UTBMTXUKSU5DYzBMN1JnZEM2MExMUXNERVpNQmNHQTFVRUJ3d1EwTE11SU5DYzBMN1JnZEM2MExMUXNERTRNRFlHQTFVRQpDZ3d2MEtUUWxkQ1UwSlhRb05DUTBKdlFyTkNkMEo3UWxTRFFtdENRMEpmUW5kQ1EwS2ZRbGRDWjBLSFFvdENTCjBKNHhOakEwQmdOVkJDb01MZENWMExyUXNOR0MwTFhSZ05DNDBMM1FzQ0RRb2RHQzBMRFF2ZEM0MFlIUXU5Q3cKMExMUXZ0Q3kwTDNRc0RFYk1Ca0dBMVVFQkF3UzBKSFF2dEdBMEw3UXN0QzkwTFhRc3RDd01Ub3dPQVlEVlFRTQpEREhRb05HRDBMclF2dEN5MEw3UXROQzQwWUxRdGRDNzBZd2cwWUxRdGRHQjBZTFF1TkdBMEw3UXN0Q3cwTDNRCnVOR1BNVGd3TmdZRFZRUUREQy9RcE5DVjBKVFFsZENnMEpEUW05Q3MwSjNRbnRDVklOQ2EwSkRRbDlDZDBKRFEKcDlDVjBKblFvZENpMEpMUW5qQm1NQjhHQ0NxRkF3Y0JBUUVCTUJNR0J5cUZBd0lDSkFBR0NDcUZBd2NCQVFJQwpBME1BQkVEZ3JxdTdINmgvaDlOWEd0YlpjZlhFcm1xRGxad0xoT2tXWjBqRVJTem8zMFhlS2ZTczFaclkyVTRoCjNSL1hVb3hJeXpuRlpwMXBWOGVPalovcmRkWnVvNElFYWpDQ0JHWXdEQVlEVlIwVEFRSC9CQUl3QURCRUJnZ3IKQmdFRkJRY0JBUVE0TURZd05BWUlLd1lCQlFVSE1BS0dLR2gwZEhBNkx5OWpjbXd1Y205emEyRjZibUV1Y25VdgpZM0pzTDNWalptdGZNakF5TVM1amNuUXdIUVlEVlIwZ0JCWXdGREFJQmdZcWhRTmtjUUV3Q0FZR0tvVURaSEVDCk1EWUdCU3FGQTJSdkJDME1LeUxRbXRHQTBMalF2OUdDMEw3UW45R0EwTDRnUTFOUUlpQW8wTExRdGRHQTBZSFEKdU5HUElEUXVNQ2t3Z2dGa0JnVXFoUU5rY0FTQ0FWa3dnZ0ZWREVjaTBKclJnTkM0MEwvUmd0QyswSi9SZ05DKwpJRU5UVUNJZzBMTFF0ZEdBMFlIUXVOR1BJRFF1TUNBbzBMalJnZEMvMEw3UXU5QzkwTFhRdmRDNDBMVWdNaTFDCllYTmxLUXhvMEovUmdOQyswTFBSZ05DdzBMelF2TkM5MEw0dDBMRFF2OUMvMExEUmdOQ3cwWUxRdmRHTDBMa2cKMExyUXZ0QzgwTC9RdTlDMTBMclJnU0RDcTlDdTBMM1F1TkdCMExYUmdOR0NMZENUMEo3UW9kQ2l3cnN1SU5DUwowTFhSZ05HQjBMalJqeUF6TGpBTVQ5Q2gwTFhSZ05HQzBMalJoTkM0MExyUXNOR0NJTkdCMEw3UXZ0R0MwTExRCnRkR0MwWUhSZ3RDeTBMalJqeURpaEpZZzBLSFFwQzh4TWpRdE16azJOaURRdnRHQ0lERTFMakF4TGpJd01qRU0KVDlDaDBMWFJnTkdDMExqUmhOQzQwTHJRc05HQ0lOR0IwTDdRdnRHQzBMTFF0ZEdDMFlIUmd0Q3kwTGpSanlEaQpoSllnMEtIUXBDOHhNamd0TXpVNE1TRFF2dEdDSURJd0xqRXlMakl3TVRnd0RBWUZLb1VEWkhJRUF3SUJBREFPCkJnTlZIUThCQWY4RUJBTUNBL2d3RXdZRFZSMGxCQXd3Q2dZSUt3WUJCUVVIQXdJd0t3WURWUjBRQkNRd0lvQVAKTWpBeU1UQTVNVE13T0RRNU1qZGFnUTh5TURJeU1USXhNekE0TkRreU4xb3dnZ0dPQmdOVkhTTUVnZ0dGTUlJQgpnWUFVWjdRWk51YlJVRVRobXVITEN1eHZlNTZtWEJLaGdnRlZwSUlCVVRDQ0FVMHhJREFlQmdrcWhraUc5dzBCCkNRRVdFV2x6WDNWalFISnZjMnRoZW01aExuSjFNUmd3RmdZRktvVURaQUVTRFRFd05EYzNPVGN3TVRrNE16QXgKR2pBWUJnZ3FoUU1EZ1FNQkFSSU1NREEzTnpFd05UWTROell3TVFzd0NRWURWUVFHRXdKU1ZURVpNQmNHQTFVRQpDQXdRMExNdUlOQ2MwTDdSZ2RDNjBMTFFzREVWTUJNR0ExVUVCd3dNMEp6UXZ0R0IwTHJRc3RDd01Td3dLZ1lEClZRUUpEQ1BSZzlDNzBMalJodEN3SU5DWTBMdlJqTkM0MEwzUXV0Q3dMQ0RRdE5DKzBMd2dOekZDTUVBR0ExVUUKQ2d3NTBKclF2dEdBMEwzUXRkQ3kwTDdRdVNEUmd0QzEwWUhSZ3RDKzBMTFJpOUM1SU5DajBLWWcwS1RRbWlEUQprOUNlMEtIUW9pMHlNREV5TVVJd1FBWURWUVFERERuUW10QyswWURRdmRDMTBMTFF2dEM1SU5HQzBMWFJnZEdDCjBMN1FzdEdMMExrZzBLUFFwaURRcE5DYUlOQ1QwSjdRb2RDaUxUSXdNVEtDRURiM1N3V3BHRFM3NkJHVTR2OVAKcTNrd1FBWURWUjBmQkRrd056QTFvRE9nTVlZdmFIUjBjRG92TDJOeWJDNXliM05yWVhwdVlTNXlkUzlqY213dgpkR1Z6ZEM5emRXSm5iM04wTWpBeE1pNWpjbXd3SFFZRFZSME9CQllFRkZLWkR4UTBUMkswYWFqSDdmUXAvQS94CkhHd0pNQW9HQ0NxRkF3Y0JBUU1DQTBFQWNLV0FJR3hoVHNBcC85S2pOd0xoQzZDMFRaQ1Y1ZVZNZFYwd2lJUngKZHN5cExndHZpZWpIY1ZwdksrMExQc1ZocjZzWEJPTk4wdmdSdDRtOWpMVmo3dz09PC9kczpYNTA5Q2VydGlmaWNhdGU+PC9kczpYNTA5RGF0YT48L2RzOktleUluZm8+PGRzOk9iamVjdCBJZD0ib2JqLVl1azJ4SWxWM2xDTzRiVUkiPjx4YWRlczpRdWFsaWZ5aW5nUHJvcGVydGllcyB4bWxuczp4YWRlcz0iaHR0cDovL3VyaS5ldHNpLm9yZy8wMTkwMy92MS40LjEjIiBUYXJnZXQ9InNpZy1ZdWsyeElsVjNsQ080YlVJIj48eGFkZXM6U2lnbmVkUHJvcGVydGllcyBJZD0ic2lnbmVkcHJvcHMtWXVrMnhJbFYzbENPNGJVSSI+PHhhZGVzOlNpZ25lZFNpZ25hdHVyZVByb3BlcnRpZXMvPjwveGFkZXM6U2lnbmVkUHJvcGVydGllcz48eGFkZXM6VW5zaWduZWRQcm9wZXJ0aWVzPjx4YWRlczpVbnNpZ25lZFNpZ25hdHVyZVByb3BlcnRpZXM+PHhhZGVzOlNpZ25hdHVyZVRpbWVTdGFtcD48eGFkZXM6RW5jYXBzdWxhdGVkVGltZVN0YW1wPk1JSVhQQVlKS29aSWh2Y05BUWNDb0lJWExUQ0NGeWtDQVFFeERqQU1CZ2dxaFFNSEFRRUNBZ1VBTUlJQ1pBWUxLb1pJaHZjTkFRa1FBUVNnZ2dKVEJJSUNUekNDQWtzQ0FRRUdDQ3FGQXdJVkFXY0NNQzR3Q2dZR0tvVURBZ0lKQlFBRUlNd0FFZHpwQ1lPQ01vaERlYUVNWmFJOFVodHliNFd0cEtnWDMzb0t6WDBVQWd3VTAzc0RtWStnSEUxRVdKQVlFekl3TWpJd09EQXlNVFF6TnpNNExqazFPRnFnZ2dIbnBJSUI0ekNDQWQ4eEdqQVlCZ2dxaFFNRGdRTUJBUklNTURBM056RXdOVFk0TnpZd01SZ3dGZ1lGS29VRFpBRVNEVEV3TkRjM09UY3dNVGs0TXpBeFhqQmNCZ05WQkFrTVZkQ1IwTDdRdTlHTTBZalF2dEM1SU5DWDBMdlFzTkdDMEw3Umc5R0IwWUxRdU5DOTBZSFF1dEM0MExrZzBML1F0ZEdBMExYUmc5QzcwTDdRdWlEUXRDNGdOaURSZ2RHQzBZRFF2dEMxMEwzUXVOQzFJREV4SHpBZEJna3Foa2lHOXcwQkNRRVdFR2x6Wm10QWNtOXphMkY2Ym1FdWNuVXhDekFKQmdOVkJBWVRBbEpWTVJrd0Z3WURWUVFJREJEUXN5NGcwSnpRdnRHQjBMclFzdEN3TVJVd0V3WURWUVFIREF6UW5OQyswWUhRdXRDeTBMQXhPREEyQmdOVkJBb01MOUNrMExYUXROQzEwWURRc05DNzBZelF2ZEMrMExVZzBMclFzTkMzMEwzUXNOR0gwTFhRdWRHQjBZTFFzdEMrTVZrd1Z3WURWUVFMREZEUW85Qy8wWURRc05DeTBMdlF0ZEM5MExqUXRTRFF1TkM5MFlUUXZ0R0EwTHpRc05HRzBMalF2dEM5MEwzUXZ0QzVJTkM0MEwzUmhOR0EwTERSZ2RHQzBZRFJnOUM2MFlMUmc5R0EwTDdRdVRFWU1CWUdDU3FHU0liM0RRRUpBaE1KZEdsdFpYTjBZVzF3TVRnd05nWURWUVFEREMvUXBOQzEwTFRRdGRHQTBMRFF1OUdNMEwzUXZ0QzFJTkM2MExEUXQ5QzkwTERSaDlDMTBMblJnZEdDMExMUXZxQ0NFR1V3Z2dlWk1JSUhScUFEQWdFQ0Fnc0F5OGFZTXdBQUFBQUZiakFLQmdncWhRTUhBUUVEQWpDQ0FTUXhIakFjQmdrcWhraUc5dzBCQ1FFV0QyUnBkRUJ0YVc1emRubGhlaTV5ZFRFTE1Ba0dBMVVFQmhNQ1VsVXhHREFXQmdOVkJBZ01EemMzSU5DYzBMN1JnZEM2MExMUXNERVpNQmNHQTFVRUJ3d1EwTE11SU5DYzBMN1JnZEM2MExMUXNERXVNQ3dHQTFVRUNRd2wwWVBRdTlDNDBZYlFzQ0RRb3RDeTBMWFJnTkdCMExyUXNOR1BMQ0RRdE5DKzBMd2dOekVzTUNvR0ExVUVDZ3dqMEp6UXVOQzkwTHJRdnRDODBZSFFzdEdQMExmUmpDRFFvTkMrMFlIUmdkQzQwTGd4R0RBV0JnVXFoUU5rQVJJTk1UQTBOemN3TWpBeU5qY3dNVEVhTUJnR0NDcUZBd09CQXdFQkVnd3dNRGMzTVRBME56UXpOelV4TERBcUJnTlZCQU1NSTlDYzBMalF2ZEM2MEw3UXZOR0IwTExSajlDMzBZd2cwS0RRdnRHQjBZSFF1TkM0TUI0WERUSXhNRFF4TXpFek1qYzFOMW9YRFRNMk1EUXhNekV6TWpjMU4xb3dnZ0Z0TVNBd0hnWUpLb1pJaHZjTkFRa0JGaEYxWTE5bWEwQnliM05yWVhwdVlTNXlkVEVaTUJjR0ExVUVDQXdRMExNdUlOQ2MwTDdSZ2RDNjBMTFFzREVhTUJnR0NDcUZBd09CQXdFQkVnd3dNRGMzTVRBMU5qZzNOakF4R0RBV0JnVXFoUU5rQVJJTk1UQTBOemM1TnpBeE9UZ3pNREZnTUY0R0ExVUVDUXhYMEpIUXZ0QzcwWXpSaU5DKzBMa2cwSmZRdTlDdzBZTFF2dEdEMFlIUmd0QzQwTDNSZ2RDNjBMalF1U0RRdjlDMTBZRFF0ZEdEMEx2UXZ0QzZMQ0RRdEM0Z05pd2cwWUhSZ3RHQTBMN1F0ZEM5MExqUXRTQXhNUlV3RXdZRFZRUUhEQXpRbk5DKzBZSFF1dEN5MExBeEN6QUpCZ05WQkFZVEFsSlZNVGd3TmdZRFZRUUtEQy9RcE5DMTBMVFF0ZEdBMExEUXU5R00wTDNRdnRDMUlOQzYwTERRdDlDOTBMRFJoOUMxMExuUmdkR0MwTExRdmpFNE1EWUdBMVVFQXd3djBLVFF0ZEMwMExYUmdOQ3cwTHZSak5DOTBMN1F0U0RRdXRDdzBMZlF2ZEN3MFlmUXRkQzUwWUhSZ3RDeTBMNHdaakFmQmdncWhRTUhBUUVCQVRBVEJnY3FoUU1DQWlNQkJnZ3FoUU1IQVFFQ0FnTkRBQVJBMU5nR0x2S2hCOWpSZUplT3haeVIwTnFOQ1dEcml6UWtiWit3eTlwQUg2ZjRadTFRVXVEd0RsUUcwOUNCS1dMUnVkeUdjdVBHSGNhdVpTdUU5VmQxaGFPQ0JBTXdnZ1AvTUJJR0ExVWRFd0VCL3dRSU1BWUJBZjhDQVFBd1VnWUZLb1VEWkc4RVNReEhJdENhMFlEUXVOQy8wWUxRdnRDZjBZRFF2aUJEVTFBaUlOQ3kwTFhSZ05HQjBMalJqeUEwTGpBZ0tOQzQwWUhRdjlDKzBMdlF2ZEMxMEwzUXVOQzFJREl0UW1GelpTa3dKUVlEVlIwZ0JCNHdIREFJQmdZcWhRTmtjUUV3Q0FZR0tvVURaSEVDTUFZR0JGVWRJQUF3RGdZRFZSMFBBUUgvQkFRREFnSEdNSUlCWlFZRFZSMGpCSUlCWERDQ0FWaUFGTUpVOGJScjFFeTM0RzAydENPUThmN0RQSnNHb1lJQkxLU0NBU2d3Z2dFa01SNHdIQVlKS29aSWh2Y05BUWtCRmc5a2FYUkFiV2x1YzNaNVlYb3VjblV4Q3pBSkJnTlZCQVlUQWxKVk1SZ3dGZ1lEVlFRSURBODNOeURRbk5DKzBZSFF1dEN5MExBeEdUQVhCZ05WQkFjTUVOQ3pMaURRbk5DKzBZSFF1dEN5MExBeExqQXNCZ05WQkFrTUpkR0QwTHZRdU5HRzBMQWcwS0xRc3RDMTBZRFJnZEM2MExEUmp5d2cwTFRRdnRDOElEY3hMREFxQmdOVkJBb01JOUNjMExqUXZkQzYwTDdRdk5HQjBMTFJqOUMzMFl3ZzBLRFF2dEdCMFlIUXVOQzRNUmd3RmdZRktvVURaQUVTRFRFd05EYzNNREl3TWpZM01ERXhHakFZQmdncWhRTURnUU1CQVJJTU1EQTNOekV3TkRjME16YzFNU3d3S2dZRFZRUUREQ1BRbk5DNDBMM1F1dEMrMEx6UmdkQ3kwWS9RdDlHTUlOQ2cwTDdSZ2RHQjBMalF1SUlRVG0xSGl5YnlmV1YvZG80Q1hPUFRrekFkQmdOVkhRNEVGZ1FVVlREeERKeDNRN0lrM0FaWkxWd0J0bkhVWkRZd2daZ0dBMVVkSHdTQmtEQ0JqVEF0b0N1Z0tZWW5hSFIwY0RvdkwzSmxaWE4wY2kxd2Eya3VjblV2WTJSd0wyZDFZMTluYjNOME1USXVZM0pzTUMyZ0s2QXBoaWRvZEhSd09pOHZZMjl0Y0dGdWVTNXlkQzV5ZFM5alpIQXZaM1ZqWDJkdmMzUXhNaTVqY213d0xhQXJvQ21HSjJoMGRIQTZMeTl5YjNOMFpXeGxZMjl0TG5KMUwyTmtjQzluZFdOZloyOXpkREV5TG1OeWJEQkRCZ2dyQmdFRkJRY0JBUVEzTURVd013WUlLd1lCQlFVSE1BS0dKMmgwZEhBNkx5OXlaV1Z6ZEhJdGNHdHBMbkoxTDJOa2NDOW5kV05mWjI5emRERXlMbU55ZERDQjlRWUZLb1VEWkhBRWdlc3dnZWdNTk5DZjBKRFFtdENjSU1LcjBKclJnTkM0MEwvUmd0QyswSi9SZ05DK0lFaFRUY0s3SU5DeTBMWFJnTkdCMExqUXVDQXlMakFNUTlDZjBKRFFtaURDcTlDVDBMN1F1OUMrMExMUXZkQyswTGtnMFlQUXROQyswWUhSZ3RDKzBMTFF0ZEdBMFkvUmp0R0owTGpRdVNEUmh0QzEwTDNSZ3RHQXdyc01OZENYMExEUXV0QzcwWTdSaDlDMTBMM1F1TkMxSU9LRWxpQXhORGt2TXk4eUx6SXZNak1nMEw3UmdpQXdNaTR3TXk0eU1ERTRERFRRbDlDdzBMclF1OUdPMFlmUXRkQzkwTGpRdFNEaWhKWWdNVFE1THpjdk5pOHhNRFVnMEw3UmdpQXlOeTR3Tmk0eU1ERTRNQW9HQ0NxRkF3Y0JBUU1DQTBFQWNKbGtxY2cvUnh0MDFjOEl4OFZyUm1iYjFXYUFMWnNMSWh0SUlxYlhQYU1ORUNSeTc0UTQ2aWlIejBYMXRPOSs4SGViL214eUJ5TFUyZHl0a2RlWWVqQ0NDTVF3Z2doeG9BTUNBUUlDRkRiRkdyTWt0ZVN3NGJ4ZHI4dE0vZ0oxUmxzTU1Bb0dDQ3FGQXdjQkFRTUNNSUlCYlRFZ01CNEdDU3FHU0liM0RRRUpBUllSZFdOZlptdEFjbTl6YTJGNmJtRXVjblV4R1RBWEJnTlZCQWdNRU5DekxpRFFuTkMrMFlIUXV0Q3kwTEF4R2pBWUJnZ3FoUU1EZ1FNQkFSSU1NREEzTnpFd05UWTROell3TVJnd0ZnWUZLb1VEWkFFU0RURXdORGMzT1Rjd01UazRNekF4WURCZUJnTlZCQWtNVjlDUjBMN1F1OUdNMFlqUXZ0QzVJTkNYMEx2UXNOR0MwTDdSZzlHQjBZTFF1TkM5MFlIUXV0QzQwTGtnMEwvUXRkR0EwTFhSZzlDNzBMN1F1aXdnMExRdUlEWXNJTkdCMFlMUmdOQyswTFhRdmRDNDBMVWdNVEVWTUJNR0ExVUVCd3dNMEp6UXZ0R0IwTHJRc3RDd01Rc3dDUVlEVlFRR0V3SlNWVEU0TURZR0ExVUVDZ3d2MEtUUXRkQzAwTFhSZ05DdzBMdlJqTkM5MEw3UXRTRFF1dEN3MExmUXZkQ3cwWWZRdGRDNTBZSFJndEN5MEw0eE9EQTJCZ05WQkFNTUw5Q2swTFhRdE5DMTBZRFFzTkM3MFl6UXZkQyswTFVnMExyUXNOQzMwTDNRc05HSDBMWFF1ZEdCMFlMUXN0QytNQjRYRFRJeE1EY3dPREE0TWpnd01sb1hEVEl5TVRBd09EQTRNamd3TWxvd2dnSGZNUm93R0FZSUtvVURBNEVEQVFFU0REQXdOemN4TURVMk9EYzJNREVZTUJZR0JTcUZBMlFCRWcweE1EUTNOemszTURFNU9ETXdNVjR3WEFZRFZRUUpERlhRa2RDKzBMdlJqTkdJMEw3UXVTRFFsOUM3MExEUmd0QyswWVBSZ2RHQzBMalF2ZEdCMExyUXVOQzVJTkMvMExYUmdOQzEwWVBRdTlDKzBMb2cwTFF1SURZZzBZSFJndEdBMEw3UXRkQzkwTGpRdFNBeE1SOHdIUVlKS29aSWh2Y05BUWtCRmhCcGMyWnJRSEp2YzJ0aGVtNWhMbkoxTVFzd0NRWURWUVFHRXdKU1ZURVpNQmNHQTFVRUNBd1EwTE11SU5DYzBMN1JnZEM2MExMUXNERVZNQk1HQTFVRUJ3d00wSnpRdnRHQjBMclFzdEN3TVRnd05nWURWUVFLREMvUXBOQzEwTFRRdGRHQTBMRFF1OUdNMEwzUXZ0QzFJTkM2MExEUXQ5QzkwTERSaDlDMTBMblJnZEdDMExMUXZqRlpNRmNHQTFVRUN3eFEwS1BRdjlHQTBMRFFzdEM3MExYUXZkQzQwTFVnMExqUXZkR0UwTDdSZ05DODBMRFJodEM0MEw3UXZkQzkwTDdRdVNEUXVOQzkwWVRSZ05DdzBZSFJndEdBMFlQUXV0R0MwWVBSZ05DKzBMa3hHREFXQmdrcWhraUc5dzBCQ1FJVENYUnBiV1Z6ZEdGdGNERTRNRFlHQTFVRUF3d3YwS1RRdGRDMDBMWFJnTkN3MEx2UmpOQzkwTDdRdFNEUXV0Q3cwTGZRdmRDdzBZZlF0ZEM1MFlIUmd0Q3kwTDR3YURBaEJnZ3FoUU1IQVFFQkFUQVZCZ2txaFFNSEFRSUJBUUVHQ0NxRkF3Y0JBUUlDQTBNQUJFQ2R3YkFzcGFwaTA5d1lpS0x1VXE3WGc2MHpoN1RzU2IyS3JNTGNhVVdNYVcwNjVZN0ZGZzNjTUFJaFFnc3NuKythaUoyRTcrR0ZyYVViSW1LR1RKdXFvNElFYURDQ0JHUXdEQVlEVlIwVEFRSC9CQUl3QURCRUJnZ3JCZ0VGQlFjQkFRUTRNRFl3TkFZSUt3WUJCUVVITUFLR0tHaDBkSEE2THk5amNtd3VjbTl6YTJGNmJtRXVjblV2WTNKc0wzVmpabXRmTWpBeU1TNWpjblF3SFFZRFZSMGdCQll3RkRBSUJnWXFoUU5rY1FFd0NBWUdLb1VEWkhFQ01FZ0dCU3FGQTJSdkJEOE1QU0xRbXRHQTBMalF2OUdDMEw0dDBKL1JnTkMrSUVOVFVDSWdkaTQwTGpBZ0tOQzQwWUhRdjlDKzBMdlF2ZEMxMEwzUXVOQzFJREl0UW1GelpTa3dnZ0ZrQmdVcWhRTmtjQVNDQVZrd2dnRlZERWNpMEpyUmdOQzQwTC9SZ3RDKzBKL1JnTkMrSUVOVFVDSWcwTExRdGRHQTBZSFF1TkdQSURRdU1DQW8wTGpSZ2RDLzBMN1F1OUM5MExYUXZkQzQwTFVnTWkxQ1lYTmxLUXhvMEovUmdOQyswTFBSZ05DdzBMelF2TkM5MEw0dDBMRFF2OUMvMExEUmdOQ3cwWUxRdmRHTDBMa2cwTHJRdnRDODBML1F1OUMxMExyUmdTRENxOUN1MEwzUXVOR0IwTFhSZ05HQ0xkQ1QwSjdRb2RDaXdyc3VJTkNTMExYUmdOR0IwTGpSanlBekxqQU1UOUNoMExYUmdOR0MwTGpSaE5DNDBMclFzTkdDSU5HQjBMN1F2dEdDMExMUXRkR0MwWUhSZ3RDeTBMalJqeURpaEpZZzBLSFFwQzh4TWpRdE16azJOaURRdnRHQ0lERTFMakF4TGpJd01qRU1UOUNoMExYUmdOR0MwTGpSaE5DNDBMclFzTkdDSU5HQjBMN1F2dEdDMExMUXRkR0MwWUhSZ3RDeTBMalJqeURpaEpZZzBLSFFwQzh4TWpndE16VTRNU0RRdnRHQ0lESXdMakV5TGpJd01UZ3dEZ1lEVlIwUEFRSC9CQVFEQWdQNE1CTUdBMVVkSlFRTU1Bb0dDQ3NHQVFVRkJ3TUlNQ3NHQTFVZEVBUWtNQ0tBRHpJd01qRXdOekE0TURneU56VTVXb0VQTWpBeU1qRXdNRGd3T0RJM05UbGFNSUlCWUFZRFZSMGpCSUlCVnpDQ0FWT0FGRlV3OFF5Y2QwT3lKTndHV1MxY0FiWngxR1Eyb1lJQkxLU0NBU2d3Z2dFa01SNHdIQVlKS29aSWh2Y05BUWtCRmc5a2FYUkFiV2x1YzNaNVlYb3VjblV4Q3pBSkJnTlZCQVlUQWxKVk1SZ3dGZ1lEVlFRSURBODNOeURRbk5DKzBZSFF1dEN5MExBeEdUQVhCZ05WQkFjTUVOQ3pMaURRbk5DKzBZSFF1dEN5MExBeExqQXNCZ05WQkFrTUpkR0QwTHZRdU5HRzBMQWcwS0xRc3RDMTBZRFJnZEM2MExEUmp5d2cwTFRRdnRDOElEY3hMREFxQmdOVkJBb01JOUNjMExqUXZkQzYwTDdRdk5HQjBMTFJqOUMzMFl3ZzBLRFF2dEdCMFlIUXVOQzRNUmd3RmdZRktvVURaQUVTRFRFd05EYzNNREl3TWpZM01ERXhHakFZQmdncWhRTURnUU1CQVJJTU1EQTNOekV3TkRjME16YzFNU3d3S2dZRFZRUUREQ1BRbk5DNDBMM1F1dEMrMEx6UmdkQ3kwWS9RdDlHTUlOQ2cwTDdSZ2RHQjBMalF1SUlMQU12R21ETUFBQUFBQlc0d2FBWURWUjBmQkdFd1h6QXVvQ3lnS29Zb2FIUjBjRG92TDJOeWJDNXliM05yWVhwdVlTNXlkUzlqY213dmRXTm1hMTh5TURJeExtTnliREF0b0N1Z0tZWW5hSFIwY0RvdkwyTnliQzVtYzJackxteHZZMkZzTDJOeWJDOTFZMlpyWHpJd01qRXVZM0pzTUIwR0ExVWREZ1FXQkJSaGZuc0U4Vk5YYjdDVXAySE5aZlVoMkt2ZG1EQUtCZ2dxaFFNSEFRRURBZ05CQUhmYzdCQ3RoM2ppVUl4MFlzelhCeHhyQ1hHL1BXdzcrZGNUaXFDckVmbTYwa3lQK1NMKytnUjNjMU9INHN1SlRZcnVBU0xTMEt4L3FPVzMxS1JKV2pjeGdnUkJNSUlFUFFJQkFUQ0NBWWN3Z2dGdE1TQXdIZ1lKS29aSWh2Y05BUWtCRmhGMVkxOW1hMEJ5YjNOcllYcHVZUzV5ZFRFWk1CY0dBMVVFQ0F3UTBMTXVJTkNjMEw3UmdkQzYwTExRc0RFYU1CZ0dDQ3FGQXdPQkF3RUJFZ3d3TURjM01UQTFOamczTmpBeEdEQVdCZ1VxaFFOa0FSSU5NVEEwTnpjNU56QXhPVGd6TURGZ01GNEdBMVVFQ1F4WDBKSFF2dEM3MFl6UmlOQyswTGtnMEpmUXU5Q3cwWUxRdnRHRDBZSFJndEM0MEwzUmdkQzYwTGpRdVNEUXY5QzEwWURRdGRHRDBMdlF2dEM2TENEUXRDNGdOaXdnMFlIUmd0R0EwTDdRdGRDOTBMalF0U0F4TVJVd0V3WURWUVFIREF6UW5OQyswWUhRdXRDeTBMQXhDekFKQmdOVkJBWVRBbEpWTVRnd05nWURWUVFLREMvUXBOQzEwTFRRdGRHQTBMRFF1OUdNMEwzUXZ0QzFJTkM2MExEUXQ5QzkwTERSaDlDMTBMblJnZEdDMExMUXZqRTRNRFlHQTFVRUF3d3YwS1RRdGRDMDBMWFJnTkN3MEx2UmpOQzkwTDdRdFNEUXV0Q3cwTGZRdmRDdzBZZlF0ZEM1MFlIUmd0Q3kwTDRDRkRiRkdyTWt0ZVN3NGJ4ZHI4dE0vZ0oxUmxzTU1Bd0dDQ3FGQXdjQkFRSUNCUUNnZ2dKTk1Cb0dDU3FHU0liM0RRRUpBekVOQmdzcWhraUc5dzBCQ1JBQkJEQWNCZ2txaGtpRzl3MEJDUVV4RHhjTk1qSXdPREF5TVRRek56TTRXakF2QmdrcWhraUc5dzBCQ1FReElnUWcxdTBDR000U1ByUW5Oa0UzNDVLK3dWR2hITmJpVXhSMnlPdTFKeVhhamc4d2dnSGVCZ3NxaGtpRzl3MEJDUkFDTHpHQ0FjMHdnZ0hKTUlJQnhUQ0NBY0V3Q2dZSUtvVURCd0VCQWdJRUlJcjRWRkVWdHREaHhmYXFkdjM2dnlNZnp3TkpDaXFVM2hKaXdoNG9TdG5zTUlJQmp6Q0NBWFdrZ2dGeE1JSUJiVEVnTUI0R0NTcUdTSWIzRFFFSkFSWVJkV05mWm10QWNtOXphMkY2Ym1FdWNuVXhHVEFYQmdOVkJBZ01FTkN6TGlEUW5OQyswWUhRdXRDeTBMQXhHakFZQmdncWhRTURnUU1CQVJJTU1EQTNOekV3TlRZNE56WXdNUmd3RmdZRktvVURaQUVTRFRFd05EYzNPVGN3TVRrNE16QXhZREJlQmdOVkJBa01WOUNSMEw3UXU5R00wWWpRdnRDNUlOQ1gwTHZRc05HQzBMN1JnOUdCMFlMUXVOQzkwWUhRdXRDNDBMa2cwTC9RdGRHQTBMWFJnOUM3MEw3UXVpd2cwTFF1SURZc0lOR0IwWUxSZ05DKzBMWFF2ZEM0MExVZ01URVZNQk1HQTFVRUJ3d00wSnpRdnRHQjBMclFzdEN3TVFzd0NRWURWUVFHRXdKU1ZURTRNRFlHQTFVRUNnd3YwS1RRdGRDMDBMWFJnTkN3MEx2UmpOQzkwTDdRdFNEUXV0Q3cwTGZRdmRDdzBZZlF0ZEM1MFlIUmd0Q3kwTDR4T0RBMkJnTlZCQU1NTDlDazBMWFF0TkMxMFlEUXNOQzcwWXpRdmRDKzBMVWcwTHJRc05DMzBMM1FzTkdIMExYUXVkR0IwWUxRc3RDK0FoUTJ4UnF6SkxYa3NPRzhYYS9MVFA0Q2RVWmJEREFNQmdncWhRTUhBUUVCQVFVQUJFQUxPb0F0NnFlemFRQkxiaElCRnIzVnNjeWxMVXVZeUZ1bi9QMm5JajB1bGl3QmlyaUVjYUV5Q0RCUVVjbkk0S2RvV201clhsZnFHK0hsS2FtUStLc2Y8L3hhZGVzOkVuY2Fwc3VsYXRlZFRpbWVTdGFtcD48L3hhZGVzOlNpZ25hdHVyZVRpbWVTdGFtcD48ZHM6Q29tcGxldGVSZXZvY2F0aW9uUmVmcz48ZHM6Q1JMUmVmcz48ZHM6Q1JMUmVmPjxkczpEaWdlc3RBbGdBbmRWYWx1ZT48ZHM6RGlnZXN0TWV0aG9kIEFsZ29yaXRobT0iaHR0cDovL3d3dy53My5vcmcvMjAwMS8wNC94bWxkc2lnLW1vcmUjZ29zdHIzNDExIi8+PGRzOkRpZ2VzdFZhbHVlPm9wMFZzMExTZEZLajV4c0t1ckYrQTlxU0RnaS9NTGZyNExWeXRVRlY5YW89PC9kczpEaWdlc3RWYWx1ZT48L2RzOkRpZ2VzdEFsZ0FuZFZhbHVlPjxkczpDUkxJZGVudGlmaWVyPjxkczpJc3N1ZXI+Q0490J/QvtC00YfQuNC90ZHQvdC90YvQuSDRgtC10YHRgtC+0LLRi9C5INCj0KYg0KTQmiDQk9Ce0KHQoi0yMDEyLE890J/QvtC00YfQuNC90ZHQvdC90YvQuSDRgtC10YHRgtC+0LLRi9C5INCj0KYg0KTQmiDQk9Ce0KHQoi0yMDEyLEM9UlUsTD3QnNC+0YHQutCy0LAsU1RSRUVUPdGD0LvQuNGG0LAg0JjQu9GM0LjQvdC60LAsINC00L7QvCA3LE9HUk49MTA0Nzc5NzAxOTgzMCxJTk49MDA3NzEwNTY4NzYwLFNUPdCzLiDQnNC+0YHQutCy0LAsTUFJTD11Y19ma0Byb3NrYXpuYS5ydTwvZHM6SXNzdWVyPjxkczpJc3N1ZVRpbWU+MjAyMi0wNy0zMFQxNDoxMToxNFo8L2RzOklzc3VlVGltZT48ZHM6TnVtYmVyPjU1NTwvZHM6TnVtYmVyPjwvZHM6Q1JMSWRlbnRpZmllcj48L2RzOkNSTFJlZj48L2RzOkNSTFJlZnM+PC9kczpDb21wbGV0ZVJldm9jYXRpb25SZWZzPjxkczpDb21wbGV0ZUNlcnRpZmljYXRlUmVmcz48ZHM6Q2VydFJlZnM+PGRzOkNlcnQ+PGRzOkNlcnREaWdlc3Q+PGRzOkRpZ2VzdE1ldGhvZCBBbGdvcml0aG09Imh0dHA6Ly93d3cudzMub3JnLzIwMDEvMDQveG1sZHNpZy1tb3JlI2dvc3RyMzQxMSIvPjxkczpEaWdlc3RWYWx1ZT52bDM2MloyTGJkTitNL1hWQjFDUHJ2aHJWZG15NnI0NTVlN2ZGaTQ4WTBjPTwvZHM6RGlnZXN0VmFsdWU+PC9kczpDZXJ0RGlnZXN0PjxkczpJc3N1ZXJTZXJpYWw+PHhhZGVzOlg1MDlJc3N1ZXJOYW1lPkNOPdCf0L7QtNGH0LjQvdGR0L3QvdGL0Lkg0YLQtdGB0YLQvtCy0YvQuSDQo9CmINCk0Jog0JPQntCh0KItMjAxMixPPdCf0L7QtNGH0LjQvdGR0L3QvdGL0Lkg0YLQtdGB0YLQvtCy0YvQuSDQo9CmINCk0Jog0JPQntCh0KItMjAxMixDPVJVLEw90JzQvtGB0LrQstCwLFNUUkVFVD3Rg9C70LjRhtCwINCY0LvRjNC40L3QutCwLCDQtNC+0LwgNyxPR1JOPTEwNDc3OTcwMTk4MzAsSU5OPTAwNzcxMDU2ODc2MCxTVD3Qsy4g0JzQvtGB0LrQstCwLE1BSUw9dWNfZmtAcm9za2F6bmEucnU8L3hhZGVzOlg1MDlJc3N1ZXJOYW1lPjx4YWRlczpYNTA5SXNzdWVyU2VyaWFsPjczMDYyMzMwNzI1NjM2ODc1OTE2NDU0NzI3MTc0MDEwNjc4MTM3PC94YWRlczpYNTA5SXNzdWVyU2VyaWFsPjwvZHM6SXNzdWVyU2VyaWFsPjwvZHM6Q2VydD48L2RzOkNlcnRSZWZzPjwvZHM6Q29tcGxldGVDZXJ0aWZpY2F0ZVJlZnM+PC94YWRlczpVbnNpZ25lZFNpZ25hdHVyZVByb3BlcnRpZXM+PC94YWRlczpVbnNpZ25lZFByb3BlcnRpZXM+PC94YWRlczpRdWFsaWZ5aW5nUHJvcGVydGllcz48L2RzOk9iamVjdD48L2RzOlNpZ25hdHVyZT48L0RvYz4K";

static bool Check(ISignatureVerifier signVerifier)
{
    Console.Write("Check connection: ");

    try
    {
        var success = signVerifier.Verify(SignatureValid);

        if (success)
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write("Ok");
        }

        return success;
    }
    catch (Exception ex)
    {
        Console.ForegroundColor = ConsoleColor.DarkRed;

        Console.Write(ex.Message);

        var inner = ex.InnerException;

        if (inner != null)
            Console.Write($" -> {inner.Message}");

        return false;
    }
    finally
    {
        Console.ResetColor();
        Console.WriteLine();
    }
}

Console.Write("Press any key to exit...");
Console.ReadKey(true);