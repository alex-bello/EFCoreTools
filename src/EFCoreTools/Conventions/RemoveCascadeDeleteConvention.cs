using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Metadata;
using EFCoreTools.Extensions;
using EFCoreTools.Attributes;

namespace EFCoreTools.Conventions
{
    /// <summary>
    /// Convention that sets the cascade delete behavior to `DeleteBehavior.Restricted` for for all entities in the Model. This overrides
    /// the default behavior, which deletes all related entities if a parent is removed.
    /// </summary>
    public class RemoveCascadeDeleteConvention : IConvention
    {
        //TODO: 
        // - Implement Ordering of Properties in Index
        
        /// <summary>
        /// Applies the convention to the provided ModelBuilder object and returns the object once the convention is applied.
        /// </summary>
        /// <returns>ModelBuilder</returns>
        public ModelBuilder Apply(ModelBuilder modelBuilder)
        {   
            var entities = modelBuilder.Model
                .GetEntityTypes()
                .ToList();

            foreach (var x in entities)
            {
                modelBuilder.Entity(x.ClrType)
                    .Metadata
                    .GetForeignKeys()
                    .ToList()
                    .ForEach( y => y.DeleteBehavior = DeleteBehavior.Restrict);

            }

            return modelBuilder;
        }
    }
}