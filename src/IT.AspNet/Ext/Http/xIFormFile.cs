using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace IT.Ext
{
    public static class xIFormFile
    {
        public static async Task<Byte[]> GetBytes(this IFormFile file, CancellationToken cancellationToken = default)
        {
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream, cancellationToken).CA();
                return memoryStream.ToArray();
            }
        }
    }
}