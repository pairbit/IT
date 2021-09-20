using IT.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace IT.AspNet.Mvc
{
    public class ConsumesJsonAttribute : ConsumesAttribute
    {
        public ConsumesJsonAttribute() : base(Json.MIME)
        {

        }
    }
}