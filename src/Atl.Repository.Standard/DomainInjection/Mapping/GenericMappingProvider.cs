using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Atl.Repository.Standard.DomainInjection.Mapping
{
	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <seealso cref="Microsoft.EntityFrameworkCore.IEntityTypeConfiguration{T}" />
	public class GenericConfiguration<T> : IEntityTypeConfiguration<T> where T:class 
    {
	    public void Configure(EntityTypeBuilder<T> builder)
	    {
		    throw new System.NotImplementedException();
	    }
    }

    /// <summary>
    /// 
    /// </summary>
    public static class GenericMappingProvider
    {
        /// <summary>
        /// Gets the maping.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEntityTypeConfiguration<T> GetMaping<T>() where T: class
        {
            return new GenericConfiguration<T>();
        }
    }
}
