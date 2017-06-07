using System;

namespace EFCoreTools.Core.Attributes
{
    public class IndexAttribute : Attribute
    {
        public string Name { get; private set; }
        public bool IsUnique { get; set; }
        public string PropertyName { get; set; }

        public IndexAttribute(string name, bool isUnique = false) 
        {
            Name = name;
            IsUnique = isUnique;
        }
    }
}