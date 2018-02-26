using System;
using System.Collections.Concurrent;
using System.Text;
using Vasily.Utils;

namespace Vasily
{
    public static class Sql<T>
    {
        public static ConcurrentDictionary<string, string> ColumnToRealMap;
        public static ConcurrentDictionary<string, string> RealToColumnMap;
        public static ConcurrentDictionary<string, string> ALMap;
        public static ModelStruction Struction;
        static Sql()
        {
            ColumnToRealMap = new ConcurrentDictionary<string, string>();
            RealToColumnMap = new ConcurrentDictionary<string, string>();
            ALMap = new ConcurrentDictionary<string, string>();
        }
        
        public static string GetColumnName(string key)
        {
            if (RealToColumnMap.ContainsKey(key))
            {
                return RealToColumnMap[key];
            }
            return key;
        }
        public static string GetRealName(string key)
        {
            if (ColumnToRealMap.ContainsKey(key))
            {
                return ColumnToRealMap[key];
            }
            return key;
        }
        public static void SelectSqlCache(string key, string sql)
        {
            VasilyCache.TempCache[key] = ConditionSelect+sql;
        }
        public static void DeleteSqlCache(string key, string sql)
        {
            VasilyCache.TempCache[key] = ConditionDelete + sql;
        }
        public static void UpdateSqlCache(string key, string sql)
        {
            VasilyCache.TempCache[key] = ConditionUpdate + sql;
        }
        public static void SelectCache(string key, params Condition[] conditions)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(ConditionSelect);
            builder.Append(GetFromCondition(ref conditions));
            VasilyCache.TempCache[key] = builder.ToString();
        }
        public static void DelectCache(string key, params Condition[] conditions)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(ConditionDelete);
            builder.Append(GetFromCondition(ref conditions));
            VasilyCache.TempCache[key] = builder.ToString();
        }
        public static void UpdateCache(string key, params Condition[] conditions)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(ConditionUpdate);
            builder.Append(GetFromCondition(ref conditions));
            VasilyCache.TempCache[key] = builder.ToString();
        }
        private static string GetFromCondition(ref Condition[] conditions)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < conditions.Length; i += 1)
            {
                builder.Append(conditions[i].Property);
            }
            return builder.ToString();
        }
        public static string AL(string key)
        {
            if (ALMap.ContainsKey(key))
            {
                return ALMap[key];
            }
            return null;
        }

        public static bool IsMaunally;
        public static string Table;
        public static string PrimaryKey;


        public static string Insert;
        public static string Update;
        public static string Select;
        public static string Delete;

        public static string ConditionUpdate;
        public static string ConditionDelete;
        public static string ConditionSelect;

        public static string SelectAll;
        public static string CheckRepeate;
        public static string GetPrimaryKey;
    }
}