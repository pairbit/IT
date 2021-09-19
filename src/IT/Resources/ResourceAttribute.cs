using IT.Ext;
using IT.Resources;
using System;

[assembly: Resource(typeof(Res))]

namespace IT.Resources
{
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false)]
    public class ResourceAttribute : Attribute
    {
        internal String Prefix { get; set; }

        public Type ResourceType { get; }

        public ResourceAttribute(Type resourceType)
        {
            ResourceType = resourceType;
        }

        internal String GetResourceName(Type type)
        {
            return GetResourceName(type.GetNameWithoutPostfix());
        }

        internal String GetResourceName(String name)
        {
            return Prefix.IsEmpty() ? name : Prefix + "_" + name;
        }
    }
}