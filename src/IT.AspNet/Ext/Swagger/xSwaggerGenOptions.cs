using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace IT.Ext
{
    /// <summary>
    /// https://codeburst.io/get-started-with-swashbuckle-and-asp-net-core-fd3a75350aac
    /// </summary>
    public static class xSwaggerGenOptions
    {
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void IncludeXmlComments(this SwaggerGenOptions swaggerGenOptions, Boolean includeControllerXmlComments = true)
        {
            var xmlFolder = Path.GetDirectoryName(This.EntryAssembly.Location);
            foreach (var xml in Directory.GetFiles(xmlFolder, "*.xml"))
            {
                //Log.Debug("Load comments xml '{0}'", xml);
                swaggerGenOptions.IncludeXmlComments(xml, includeControllerXmlComments);
            }
        }
    }
}