using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
    public static class UriBuilderExtensions
    {
        public static UriBuilder PathCombine(this UriBuilder builder, params string[] paths)
        {
            paths = (new List<string> { builder.Path })
                .Union(paths)
                .Select(a => a.EndsWith("/") ? a.Substring(0, a.Length - 1) : a)
                .Select(a => a.StartsWith("/") ? a.Substring(1) : a)
                .Where(a => !string.IsNullOrEmpty(a))
                .ToArray();


            builder.Path = $"/{string.Join("/", paths)}";

            return builder;
        }
    }
}
