using Microsoft.AspNetCore.Http;
using System;

namespace IT.Ext
{
    public static class xHttpRequest
    {
        public static String HostUrl(this HttpRequest request) => string.Concat(request.Scheme, Uri.SchemeDelimiter, request.Host.Value);
    }
}