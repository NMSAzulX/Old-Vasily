using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;

namespace Vasily
{
    public class SqlModel
    {
        internal HashSet<string> IgnoreMembers;
        internal HashSet<string> RepeateMembers;
        public ConcurrentDictionary<string, string> ColumnToRealMap;
        public ConcurrentDictionary<string, string> RealToColumnMap;
        public ConcurrentDictionary<string, string> ALMap;
        public ModelStruction Struction;
        internal Type TypeHandler;

        public SqlModel(Type type)
        {
            TypeHandler = type;
            Struction = new ModelStruction(type);
            VasilyCache.StructionCache[type] = Struction;
            IgnoreMembers = new HashSet<string>();
            RepeateMembers = new HashSet<string>();
            ColumnToRealMap = new ConcurrentDictionary<string, string>();
            RealToColumnMap = new ConcurrentDictionary<string, string>();
            ALMap = new ConcurrentDictionary<string, string>();
        }

        public string GetColumnName(string key)
        {
            if (RealToColumnMap.ContainsKey(key))
            {
                return RealToColumnMap[key];
            }
            return key;
        }
        public string GetRealName(string key)
        {
            if (ColumnToRealMap.ContainsKey(key))
            {
                return ColumnToRealMap[key];
            }
            return key;
        }
        public bool IsMaunally;
        public string Table;
        public string PrimaryKey;


        public string Insert;
        public string Update;
        public string Select;
        public string Delete;

        public string ConditionUpdate;
        public string ConditionDelete;
        public string ConditionSelect;

        public string SelectAll;
        public string CheckRepeate;
        public string GetPrimaryKey;
    }
}
