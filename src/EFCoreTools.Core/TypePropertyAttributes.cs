using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EFCoreTools.Core
{
    public class TypePropertyAttributes<T>
        where T : Attribute
    {
        public Type Type { get; private set; }
        public IEnumerable<PropertyAttribute<T>> PropertyAttributes { get; private set; }

        public TypePropertyAttributes(Type type, IEnumerable<PropertyAttribute<T>> propertyAttributes) 
        {
            Type = type;
            PropertyAttributes = propertyAttributes;
        }
    }
}