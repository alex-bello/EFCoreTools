using System;
using Microsoft.EntityFrameworkCore;

namespace EFCoreTools.Extensions
{
    public static partial class ModelBuilderExtensionMethods
    {
        public static ModelBuilder SetDbTableNamesFormat(this ModelBuilder modelBuilder, Func<string, string> tableNamesFormat)
        {
            if (tableNamesFormat == null) return modelBuilder;

            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                entity.Relational().TableName = tableNamesFormat(entity.Relational().TableName);
            }

            return modelBuilder;
        }
    }
}

