using System;
using Microsoft.EntityFrameworkCore;

namespace EFCoreTools.Extensions
{
    public static partial class ModelBuilderExtensionMethods
    {
        public static ModelBuilder SetDbColumnNamesFormat(this ModelBuilder modelBuilder, Func<string, string> columnNamesFormat)
        {
            if (columnNamesFormat == null) return modelBuilder;
            
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var prop in entity.GetProperties())
                {
                    prop.Relational().ColumnName = columnNamesFormat(prop.Relational().ColumnName);
                }
            }

            return modelBuilder;
        }
    }
}

