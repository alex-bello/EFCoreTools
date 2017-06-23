using System;

namespace EFCoreTools.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class IndexAttribute : Attribute
    {
        /// <summary>
        /// Name of the Index. Value must be unique in the model 
        /// </summary>
        /// <returns></returns>
        public string Name { get; private set; }
        public bool IsUnique { get; private set; }

        public IndexAttribute(string name, bool isUnique = false) 
        {
            Name = name;
            IsUnique = isUnique;
        }
    }
}