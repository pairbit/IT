using Microsoft.AspNetCore.Mvc;

namespace IT.AspNet.Mvc
{
    public sealed class IgnoreApiAttribute : ApiExplorerSettingsAttribute
    {
        public IgnoreApiAttribute()
        {
            IgnoreApi = true;
        }
    }
}