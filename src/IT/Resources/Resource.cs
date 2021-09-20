using System;
using System.Resources;

namespace IT.Resources
{
    public class Resource
    {
        public Type Type { get; set; }

        public String Name { get; set; }

        public static ResourceManager GetManager(Type resourceType)
        {
            return new ResourceManager(resourceType);
        }

        /// <summary>
        /// <see cref="ResourceManager.GetString(string)"/>
        /// </summary>
        public static String Get(String key)
        {
            var manager = GetManager(null);
            return manager.GetString(key);
        }

        public static String Get(Enum key) => Get(key.GetType().Name + "_" + key.ToString());
    }
}