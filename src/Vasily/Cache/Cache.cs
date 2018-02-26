using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Vasily.Model;
using Vasily.Utils;

namespace Vasily
{
    public static class VasilyCache
    {
        public static ConcurrentDictionary<Type, SqlModel> SqlModelCache;
        public static ConcurrentDictionary<Type, ModelStruction> StructionCache;
        public static ConcurrentDictionary<Type, List<Type>> OuterTypeCache;
        public static ConcurrentDictionary<Type, string> OuterSelectCache;
        public static ConcurrentDictionary<Type, ConcurrentDictionary<string, string>> ColumnMapCache;
        public static ConcurrentDictionary<string, ConnectionMapper> ConnectionCache;
        public static ConcurrentDictionary<string, string> TempCache;
        static VasilyCache()
        {
            SqlModelCache = new ConcurrentDictionary<Type, SqlModel>();
            OuterTypeCache = new ConcurrentDictionary<Type, List<Type>>();
            OuterSelectCache = new ConcurrentDictionary<Type, string>();
            ColumnMapCache = new ConcurrentDictionary<Type, ConcurrentDictionary<string, string>>();
            StructionCache = new ConcurrentDictionary<Type, ModelStruction>();
            ConnectionCache = new ConcurrentDictionary<string, ConnectionMapper>();
            TempCache = new ConcurrentDictionary<string, string>();
        }

        public static string GetFromCache(string key)
        {
            if (TempCache.ContainsKey(key))
            {
                return TempCache[key];
            }
            return null;
        }

        public static void SqlCache(string key, string sql)
        {
            TempCache[key] = sql;
        }
        
    }
}
