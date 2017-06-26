using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace EFCoreTools.Core.Extensions
{
    public static class AttributeExtensionMethods
    {
        public static IEnumerable<TypePropertyAttributes<T>> GetAllTypePropertyAttributes<T>(this ModelBuilder modelBuilder) 
            where T : Attribute
        {
            return modelBuilder.Model.GetEntityTypes()
            .SelectMany(x => x.ClrType.GetProperties().Where(t => t.IsDefined(typeof(T), false)))
            .GroupBy(a => a.DeclaringType, b => new PropertyAttribute<T>(b.Name, b.GetCustomAttribute<T>()), (c, d) => new TypePropertyAttributes<T>(c, d));
        }
    }
}