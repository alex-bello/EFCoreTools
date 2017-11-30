using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using EFCoreTools.Relational;
using Microsoft.EntityFrameworkCore;

namespace EFCoreTools.Extensions
{
    public static partial class ModelBuilderExtensionMethods
    {
        public static ModelBuilder ApplyTableSettings(this ModelBuilder modelBuilder, RelationalTableSettingsBuilder tableSettingsBuilder) 
        {
            return tableSettingsBuilder.Apply(modelBuilder);
        }
    }
}