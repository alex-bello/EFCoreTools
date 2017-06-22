using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EFCoreTools.Core
{
    public class TypePropertiesWithAttributeList
    {
        public IMutableEntityType Entity { get; private set; }
        public IEnumerable<PropertyInfo> Properties { get; set; }

        public TypePropertiesWithAttributeList(IMutableEntityType entity, IEnumerable<PropertyInfo> properties) 
        {
            Entity = entity;
            Properties = properties;
        }
    }
}