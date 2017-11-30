using Microsoft.EntityFrameworkCore;

namespace EFCoreTools.Conventions
{
    public interface IConvention
    {
        // void Apply(ModelBuilder model);
        ModelBuilder Apply(ModelBuilder model);
    }
}