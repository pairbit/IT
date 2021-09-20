using IT.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace IT.AspNet.Mvc
{
    public class ProducesJsonAttribute : ProducesAttribute
    {
        public ProducesJsonAttribute() : base(Json.MIME)
        {

        }
    }
}