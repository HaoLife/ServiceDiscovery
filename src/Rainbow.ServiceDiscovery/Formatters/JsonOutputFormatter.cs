﻿using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;

namespace Rainbow.ServiceDiscovery.Formatters
{
    public class JsonOutputFormatter : IOutputFormatter
    {
        public bool CanRead(IOutputFormatterContext context)
        {
            if (context.ContentType == null) return false;

            return context.ContentType.StartsWith("application/json", StringComparison.InvariantCultureIgnoreCase);
        }

        public void Read(IOutputFormatterContext context)
        {
            using (var responseStream = context.GetStream())
            {
                StreamReader reader = new StreamReader(responseStream);
                var responseFromServer = reader.ReadToEnd();

                context.Result = Newtonsoft.Json.JsonConvert.DeserializeObject(responseFromServer, context.OutType);
            }
        }


        public bool CanWrite(IInputFormatterContext context)
        {
            if (context.ContentType == null || context.Paramters.Length != 1) return false;

            return context.ContentType.StartsWith("application/json", StringComparison.InvariantCultureIgnoreCase);
        }


        public void Write(IInputFormatterContext context)
        {
            context.Result = Newtonsoft.Json.JsonConvert.SerializeObject(context.Args.First());

        }
    }
}