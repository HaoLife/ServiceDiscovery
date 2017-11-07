using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Rainbow.ServiceDiscovery
{
    public static class AttributeExtension
    {
        public static IEnumerable<T> GetCustomAttributes<T>(this ICustomAttributeProvider type, bool inherit)
        {
            var attrs = type.GetCustomAttributes(typeof(T), inherit);
            if (!attrs.Any()) return new List<T>();

            return attrs.Select(a => (T)a).ToList();
        }

        public static IEnumerable<T> GetCustomAttributes<T>(this ICustomAttributeProvider type)
        {
            return type.GetCustomAttributes<T>(false);
        }
    }
}
