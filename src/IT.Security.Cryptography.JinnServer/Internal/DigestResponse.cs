using System;

namespace IT.Security.Cryptography.JinnServer.Internal;

internal class DigestResponse
{
    /// <summary>
    /// Расчитанный хеш
    /// </summary>
    /// <example>xal6cN208mLz4a6qPLKGX72EA2OvgIVhzUZl3dC0HJE=</example>
    public String Digest { get; set; }

    /// <summary>
    /// Параметр для вычисления хеша больших файлов (значение из результата прошлого вычисления)
    /// </summary>
    /// <example>IYAAAAFLPYNWLexhNqrhmZmLPFy44pclNGriCYvaqqpeGWUe6roBIqS3eMzaQe49d8j59dR0wbssUiTD+FMguDriekPAALoNAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAJpXpk3AD+5kRoRzU0kmWy5SPLFsVUfYKGvK1LJ6XYXkvh/Ong/E/WJ3W7QSckLZYDCeBx8I+pXnu5uY0frgQxp1c3RvbVhtbC9pdGVtNC54bWxQSwUGAAAAACYAJgDkCQAAbq0BAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAKAA=</example>
    public String State { get; set; }
}