using EFCoreTools.Conventions;
using Microsoft.EntityFrameworkCore;

namespace EFCoreTools.Extensions
{
    public static partial class AttributeExtensionMethods
    {
        public static ModelBuilder UseIndexConvention(this ModelBuilder builder)
        {
            return new IndexConvention().Apply(builder);
        }

    }
}