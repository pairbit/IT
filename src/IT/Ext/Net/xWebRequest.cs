using System.Net;
using System.Threading.Tasks;

namespace IT.Ext
{
    public static class xWebRequest
    {
        public static WebResponse TryGetResponse(this WebRequest request)
        {
            try
            {
                return request.GetResponse();
            }
            catch (WebException ex)
            {
                if (ex.Response == null) throw;
                return ex.Response;
            }
        }

        public static WebResponse TryGetResponse(this WebRequest request, out WebExceptionStatus? status)
        {
            try
            {
                status = null;
                return request.GetResponse();
            }
            catch (WebException ex)
            {
                if (ex.Response == null) throw;
                status = ex.Status;
                return ex.Response;
            }
        }

        public static async Task<WebResponse> TryGetResponseAsync(this WebRequest request)
        {
            try
            {
                return await request.GetResponseAsync().CA();
            }
            catch (WebException ex)
            {
                if (ex.Response == null) throw;
                return ex.Response;
            }
        }
    }
}