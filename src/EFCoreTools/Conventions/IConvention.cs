using Microsoft.EntityFrameworkCore;

namespace EFCoreTools.Core.Conventions
{
    public interface IConvention
    {
        void Apply(ModelBuilder model);
    }
}