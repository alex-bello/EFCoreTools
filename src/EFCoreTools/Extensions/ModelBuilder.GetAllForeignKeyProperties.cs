using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace EFCoreTools.Extensions
{
    public static partial class AttributeExtensionMethods
    {
        public static IEnumerable<PropertyInfo> GetAllForeignKeyProperties(this ModelBuilder modelBuilder) 
        {
            return modelBuilder.Model.GetEntityTypes()
            .SelectMany(x => x.ClrType.GetProperties().Where(t => t.IsDefined(typeof(ForeignKeyAttribute), false)));
        }
    }
}