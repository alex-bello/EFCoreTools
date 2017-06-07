using Hiv.HivDis.EntityFramework;
using Microsoft.Extensions.Configuration;
using System.IO;
using System;
using System.Reflection;
using System.Linq;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Hiv.HivDis.Tools
{
    public class Seeding
    {
        public HivDisDbContext Context { get; }

        public Seeding(HivDisDbContext context)
        {
            Context = context;
        }

        public void GenerateSeedFiles(string fileType) 
        {
            if (fileType != "json") return;

            foreach (var x in Context.Model.GetEntityTypes())
            {
                var fileName = x.ClrType.Name.ToLower() + ".json";
                var location = Path.Combine(System.AppContext.BaseDirectory, fileName);

                var y = Context.GetType().GetMethod("Set").MakeGenericMethod(x.ClrType);
                var listValues = y.Invoke(Context, null);
                
                JObject o = JObject.FromObject(listValues);
                File.WriteAllText(location, o.ToString());
            }
        }
    }
}
