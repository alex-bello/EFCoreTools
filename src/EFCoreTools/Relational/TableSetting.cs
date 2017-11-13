using System;
using System.Collections.Generic;

namespace EFCoreTools.Relational
{
    public class RelationalTableSettings
    {
        public Type EntityType { get; private set; }
        public string Schema { get; private set; }
        public Dictionary<string, string> FindReplaceValues { get; set; }
        public bool ApplyFindReplaceToIdProperty { get; set; }
        
        public RelationalTableSettings(Type type, string schema = null, string findString = null, string replaceString = null, bool applyFindReplaceToIdProperty = true)
        {
            EntityType = type;
            Schema = schema;
            FindReplaceValues = new Dictionary<string, string>();
            ApplyFindReplaceToIdProperty = applyFindReplaceToIdProperty;

            if (!string.IsNullOrWhiteSpace(findString)) FindReplaceValues.Add(findString, replaceString ?? "");         
        }
    }
}