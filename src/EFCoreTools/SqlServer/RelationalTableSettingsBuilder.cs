using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft​.EntityFrameworkCore​;
using Microsoft​.EntityFrameworkCore​.Metadata;

namespace EFCoreTools.SqlServer
{
    /// <summary>
    /// Collection of Relational Table Settings that will be applied to Entity Types that inherit from specified types during OnModelCreating
    /// </summary>
    public class RelationalTableSettingsBuilder
    {
        protected ICollection<RelationalTableSettings> TableSettings { get; set; } = new List<RelationalTableSettings>();
        public Func<string, string> DefaultTableNameFormat { get; set; }
        public Func<string, string> DefaultPropertyNameFormat { get; set; }
        public Func<IMutableEntityType, string> DefaultPrimaryKeyNameFormat { get; set; }
        public bool UseClrTypeNamesForTables { get; set; }

        /// <summary>
        /// Default Constructor.
        /// </summary>
        public RelationalTableSettingsBuilder()
        {
            
        }

        /// <summary>
        /// Method that adds a new convention to the collection for later application.
        /// </summary>
        /// <param name="tableSettings">The settings that should be applied to a relational table.</param>
        public void Add(RelationalTableSettings tableSettings)
        {
            // Remove existing settings for the type, if any
            TableSettings.Remove(TableSettings.SingleOrDefault(x => x.EntityType == tableSettings.EntityType));

            // Add new setting
            TableSettings.Add(tableSettings);
        }

        /// <summary>
        /// Method that removes a convention from the collection.
        /// </summary>
        /// <param name="tableSettings">The settings that should be applied to a relational table.</param>
        public void Remove(RelationalTableSettings tableSettings)
        {
            // Remove existing settings for the type, if any
            TableSettings.Remove(TableSettings.SingleOrDefault(x => x.EntityType == tableSettings.EntityType));
        }

        /// <summary>
        /// Method that will iterates through all convention objects in the collection, applying changes to the model.
        /// </summary>
        public void Apply(ModelBuilder modelBuilder)
        {
            // Update all table names to ClrType name if needed
            if (UseClrTypeNamesForTables) modelBuilder.Model.GetEntityTypes()?.ToList().ForEach(x => { x.Relational().TableName = x.ClrType.Name; });
            
            foreach (var table in TableSettings)
            {
                // Apply Formats first
                modelBuilder.Model.GetEntityTypes()?.ToList()?.ForEach(x => {

                    // Check to see if the current EntityType matches the EntityType the TableSettings apply to, if not exit
                    if (!table.EntityType.IsAssignableFrom(x.ClrType)) return;

                    // Check if Schema is not null and if so, set Schema value
                    if (!string.IsNullOrWhiteSpace(table.Schema)) x.Relational().Schema = table.Schema;

                    // Check if any FindReplaceValues were added and if so, apply them
                    foreach (var item in table.FindReplaceValues)
                    {
                        var oldString = item.Key;
                        var newString = item.Value ?? "";
                        
                        x.Relational().TableName = x.Relational().TableName.Replace(oldString, newString);

                        // Apply FindReplaceValues to Id Properties if enabled
                        if (table.ApplyFindReplaceToIdProperty) 
                        {
                            var property = x.GetProperties().SingleOrDefault(p => p.Name == "Id"); 
                            property.Relational().ColumnName = property.Relational().ColumnName.Replace(oldString, newString);
                        }
                    }
                });
            }

            ApplyPrimaryKeyNameFormats(modelBuilder);
            ApplyTableNameFormats(modelBuilder);
            ApplyPropertyNameFormats(modelBuilder); 
        }

        public void ApplyPrimaryKeyNameFormats(ModelBuilder modelBuilder)
        {

            if (DefaultPrimaryKeyNameFormat == null) return;

            // Loop through the entities in the model
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                // Set the relational column name to the formatted value returned by the annonymous method.
                entity.GetProperties().SingleOrDefault(x => x.Name == "Id").Relational().ColumnName = DefaultPrimaryKeyNameFormat(entity);
            }
        }

        public void ApplyTableNameFormats(ModelBuilder modelBuilder)
        {
            if (DefaultTableNameFormat == null) return;

            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                entity.Relational().TableName = DefaultTableNameFormat(entity.Relational().TableName);
            }
        }

        public void ApplyPropertyNameFormats(ModelBuilder modelBuilder)
        {
            if (DefaultPropertyNameFormat == null) return;
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var prop in entity.GetProperties())
                {
                    prop.Relational().ColumnName = DefaultPropertyNameFormat(prop.Relational().ColumnName);
                }
            }
        }
    }
}