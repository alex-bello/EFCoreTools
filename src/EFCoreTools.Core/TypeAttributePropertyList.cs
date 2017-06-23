using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EFCoreTools.Core
{
    public class TypePropertiesWithAttributeList
    {
        public Type Type { get; private set; }
        public PropertyInfo[] Properties { get; set; }

        public TypePropertiesWithAttributeList(Type type, PropertyInfo[] properties) 
        {
            Type = type;
            Properties = properties;
        }
    }
}