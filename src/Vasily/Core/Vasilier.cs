using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Threading;
using Vasily.Model;
using Vasily.Utils;

namespace Vasily
{
    public static class Vasilier
    {
        
        /// <summary>
        /// 开局必须调用的函数
        /// </summary>
        /// <param name="interfaceName">如果自己有特殊接口，那么可以写自己的接口名</param>
        public static void Initialize(string interfaceName = "IVasily")
        {
            Assembly assmbly = Assembly.GetEntryAssembly();
            if (assmbly == null) { return; }
            IEnumerator<Type> typeCollection = assmbly.ExportedTypes.GetEnumerator();
            Type temp_Type = null;
            while (typeCollection.MoveNext())
            {
                temp_Type = typeCollection.Current;
                if (temp_Type.IsClass && !temp_Type.IsAbstract)
                {
                    if (temp_Type.GetInterface(interfaceName) != null)
                    {
                        SqlAnalyser.Initialization(temp_Type);
                    }
                }
            }
        }


        /// <summary>
        /// 注册SqlConnection
        /// </summary>
        /// <typeparam name="T">SqlConnection类型</typeparam>
        /// <param name="key">获取实例的hashKey</param>
        /// <param name="connectionString">链接字符串</param>
        public static void AddConnection<T>(string key, string connectionString)
        {
            ConnectionMapper mapper = new ConnectionMapper();
            mapper.ConnectionType = typeof(T);
            mapper.ConnectionString = connectionString;
            VasilyCache.ConnectionCache[key] = mapper;
        }

        public static void SpilteKeyWords(string key)
        {
            SqlAnalyser._open = new string(new char[] { key[0] }).Trim();
            SqlAnalyser._close = new string(new char[] { key[1] }).Trim();
        }

        public static IDbConnection GetConnection(string key)
        {
            if (VasilyCache.ConnectionCache.ContainsKey(key))
            {
                ConnectionMapper mapper = VasilyCache.ConnectionCache[key];
                return (IDbConnection)Activator.CreateInstance(mapper.ConnectionType, mapper.ConnectionString);
            }
            else
            {
                return null;
            }
        }
    }
}
