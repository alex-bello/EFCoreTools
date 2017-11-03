using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EFCoreTools
{
    public class PropertyAttribute<T>
        where T : Attribute
    {
        public string PropertyName { get; private set; }
        public T Attribute { get; private set; }

        public PropertyAttribute(string propertyName, T attribute) 
        {
            PropertyName = propertyName;
            Attribute = attribute;
        }
    }
}