using System;

namespace EFCoreTools.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class UniqueIndexAttribute : Attribute
    {
        /// <summary>
        /// Name of the Index. Value must be unique in the model 
        /// </summary>
        /// <returns></returns>
        public string Name { get; private set; }

        public UniqueIndexAttribute(string name) 
        {
            Name = name;
        }
    }
}