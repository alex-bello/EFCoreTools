// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See https://github.com/aspnet/EntityFramework6/blob/master/License.txt for license information.
// Modified by Alex Bello <alex.bello@uberops.com> to remove dependencies on EF6 files and to simplify for use in EFCoreTools project.

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text;

namespace System.ComponentModel.DataAnnotations.Schema
{
    /// <summary>
    /// When this attribute is placed on a property it indicates that the database column to which the
    /// property is mapped has an index.
    /// </summary>
    /// <remarks>
    /// This attribute is used by Entity Framework Migrations to create indexes on mapped database columns.
    /// Multi-column indexes are created by using the same index name in multiple attributes. The information
    /// in these attributes is then merged together to specify the actual database index.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class IndexAttribute : Attribute
    {
        /// <summary>
        /// The index name.
        /// </summary>
        /// <remarks>
        /// Multi-column indexes are created by using the same index name in multiple attributes. The information
        /// in these attributes is then merged together to specify the actual database index.
        /// </remarks>
        public string Name { get; internal set; }

        /// <summary>
        /// A number which will be used to determine column ordering for multi-column indexes. This will be -1 if no
        /// column order has been specified.
        /// </summary>
        /// <remarks>
        /// Multi-column indexes are created by using the same index name in multiple attributes. The information
        /// in these attributes is then merged together to specify the actual database index.
        /// </remarks>
        public int Order { get; internal set; } = -1;
        
        /// <summary>
        /// Set this property to true to define a clustered index. Set this property to false to define a 
        /// non-clustered index.
        /// </summary>
        public bool? IsClustered { get; internal set; }

        /// <summary>
        /// Set this property to true to define a unique index. Set this property to false to define a 
        /// non-unique index.
        /// </summary>
        public bool? IsUnique { get; internal set; }

        /// <summary>
        /// Creates a <see cref="IndexAttribute" /> instance for an index that will be named by convention and
        /// has no column order, clustering, or uniqueness specified.
        /// </summary>
        public IndexAttribute()
        {
        }

        /// <summary>
        /// Creates a <see cref="IndexAttribute" /> instance for an index with the given name and
        /// has no column order, clustering, or uniqueness specified.
        /// </summary>
        /// <param name="name">The index name.</param>
        public IndexAttribute(string name) : this(name, null, null, null) {}

        /// <summary>
        /// Creates a <see cref="IndexAttribute" /> instance for an index with the given name and column order, 
        /// but with no clustering or uniqueness specified.
        /// </summary>
        /// <remarks>
        /// Multi-column indexes are created by using the same index name in multiple attributes. The information
        /// in these attributes is then merged together to specify the actual database index.
        /// </remarks>
        /// <param name="name">The index name.</param>
        /// <param name="order">A number which will be used to determine column ordering for multi-column indexes.</param>
        public IndexAttribute(string name, int order) : this(name, order, null, null) {}

        public IndexAttribute(string name, bool isClustered, bool isUnique) : this(name, null, isClustered, isUnique) {}

        public IndexAttribute(string name, int? order = null, bool? isClustered = null, bool? isUnique = null)
        {
            if (name == null || name.Trim().Length ==0) throw new ArgumentException("name");
            if (order != null && order < 0) throw new ArgumentOutOfRangeException("value");

            Name = name;
            if (order.HasValue) Order = order.Value;
            IsClustered = isClustered;
            IsUnique = isUnique;
        }

        /// <summary>
        /// Returns true if this attribute specifies the same name and configuration as the given attribute.
        /// </summary>
        /// <param name="other">The attribute to compare.</param>
        /// <returns>True if the other object is equal to this object; otherwise false.</returns>
        protected virtual bool Equals(IndexAttribute other)
        {
            return Name == other.Name
                && Order == other.Order
                && IsClustered.Equals(other.IsClustered)
                && IsUnique.Equals(other.IsUnique);
        }

        /// <summary>
        /// Returns true if this attribute specifies the same name and configuration as the given attribute.
        /// </summary>
        /// <param name="obj">The attribute to compare.</param>
        /// <returns>True if the other object is equal to this object; otherwise false.</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != GetType())
            {
                return false;
            }

            return Equals((IndexAttribute)obj);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = base.GetHashCode();
                hashCode = (hashCode * 397) ^ (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Order;
                hashCode = (hashCode * 397) ^ IsClustered.GetHashCode();
                hashCode = (hashCode * 397) ^ IsUnique.GetHashCode();
                return hashCode;
            }
        }
    }
}