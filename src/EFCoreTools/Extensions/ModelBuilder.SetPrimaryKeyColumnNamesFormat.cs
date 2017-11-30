using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EFCoreTools.Extensions
{
    public static partial class ModelBuilderExtensionMethods
    {
        public static ModelBuilder SetPrimaryKeyColumnNamesFormat(this ModelBuilder modelBuilder, Func<IMutableEntityType, string> primaryKeyColumnNameFormat)
        {

            if (primaryKeyColumnNameFormat == null) return modelBuilder;

            // Loop through the entities in the model
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                // Set the relational column name to the formatted value returned by the annonymous method.
                entity.GetProperties().SingleOrDefault(x => x.Name == "Id").Relational().ColumnName = primaryKeyColumnNameFormat(entity);
            }

            return modelBuilder;
        }
    }
}