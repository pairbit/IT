using IT.Ext;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace IT.AspNet.Swagger
{
    public class ValidationFilter : IDocumentFilter, ISchemaFilter, IRequestBodyFilter, IParameterFilter, IOperationFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            //foreach (var path in swaggerDoc.Paths)
            //{
            //    foreach (var operation in path.Value.Operations)
            //    {
            //        foreach (var parameter in operation.Value.Parameters)
            //        {
            //            parameter.Required = true;
            //            foreach (var prop in parameter.Schema.Properties)
            //            {
            //                parameter.Schema.Properties.Remove(prop);
            //                break;
            //            }

            //        }

            //        foreach (var response in operation.Value.Responses)
            //        {

            //        }
            //    }
            //}

            //foreach (var apiDescription in context.ApiDescriptions)
            //{
            //    if (!apiDescription.TryGetMethodInfo(out MethodInfo method)) continue;

            //    var methodCache = method.GetCache();

            //    for (int i = 0; i < methodCache.Parameters.Length; i++)
            //    {
            //        var parameterCache = methodCache.Parameters[i];
            //        var apiParameterDescription = apiDescription.ParameterDescriptions[i];
            //        if (parameterCache.Attributes.IsRequired) apiParameterDescription.IsRequired = true;
            //    }
            //}
        }

        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            //context.me
            foreach (var property in schema.Properties)
            {
                //prop.Value.Required
            }

            if (context.ParameterInfo != null)
            {
                if (context.ParameterInfo.IsRequired())
                {
                    //schema.Nullable = false;
                    //schema.Required.Add(cache.Info.Name);
                    //Log.Debug("Parameter {0} is required", cache);
                }
            }
        }

        public void Apply(OpenApiRequestBody requestBody, RequestBodyFilterContext context)
        {
            foreach (var item in requestBody.Content)
            {
                foreach (var prop in item.Value.Schema.Properties)
                {
                    //prop.Value.Required.Add(prop.Key);
                }
                //item.Value.Schema.Required.Add();
            }

            if (context.BodyParameterDescription != null)
            {
                if (context.BodyParameterDescription.ParameterInfo().IsRequired())
                {

                }
            }

            foreach (var apiParameterDescription in context.FormParameterDescriptions.Empty())
            {
                if (apiParameterDescription.ParameterInfo().IsRequired())
                {

                }
            }
        }

        public void Apply(OpenApiParameter parameter, ParameterFilterContext context)
        {
            if (context.ParameterInfo.IsRequired())
            {
                parameter.Required = true;
                //Log.Debug("Parameter {0} is required", cache);
            }
        }

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var m = context.MethodInfo;
        }
    }
}