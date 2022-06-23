using System.Text;

namespace System.Security.Cryptography.Xml;

public static class xTransformChain
{
    public static String Transform(this TransformChain chain, String data, Encoding? encoding = null)
    {
        for (int i = 0; i < chain.Count; i++)
        {
            var transform = chain[i];
            transform.LoadInputDocument(data);
            data = transform.Transform(encoding);
        }
        return data;
    }
}