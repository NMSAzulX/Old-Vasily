using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Vasily.Utils;

namespace Vasily
{
    public class VasilyService
    {
        public VasilyService AddVasily(Action<VasilyOptions> action = null)
        {
            VasilyOptions options = new VasilyOptions();
            action?.Invoke(options);
            Vasilier.SpilteKeyWords(options.SqlSplite);
            Vasilier.Initialize(options.InterfaceName);
            return this;
        }

        public VasilyService AddVasilySqlCache(Action<SqlOptions> action)
        {
            SqlOptions options = new SqlOptions();
            action(options);
            return this;
        }
        public VasilyService AddVasilyConnectionCache(Action<ConnectionOptions> action)
        {
            ConnectionOptions options = new ConnectionOptions();
            action(options);
            return this;
        }
    }

    public class VasilyOptions
    {
        public string InterfaceName;
        public string SqlSplite;

        public VasilyOptions()
        {
            SqlSplite = "  ";
            InterfaceName = "IVasily";
        }
    }

    public class SqlOptions
    {
        private string _key;
        /// <summary>
        /// 添加一个缓存的key
        /// </summary>
        /// <param name="key">缓存的key</param>
        /// <returns>链式调用</returns>
        public SqlOptions Key(string key)
        {
            _key = key;
            return this;
        }

        /// <summary>
        /// Select * from [tabel] where + sql, 支持参数化
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="sql">拼接的条件查询语句</param>
        public void SelectSqlCache<T>(string sql)
        {
            Sql<T>.SelectSqlCache(_key, sql);
        }
        /// <summary>
        /// Select * from [table] where + conditions
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="conditions">创建条件查询语句 SqlCondition.And.Equ(property)</param>
        public void SelectConditionCache<T>(params Condition[] conditions)
        {
            Sql<T>.SelectCache(_key, conditions);
        }
        /// <summary>
        /// Update -- where + sql
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="sql">拼接的条件查询语句</param>
        public void UpdateSqlCache<T>(string sql)
        {
            Sql<T>.UpdateSqlCache(_key, sql);
        }
        /// <summary>
        /// Update -- where + Condition
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="conditions">创建条件查询语句 SqlCondition.And.Equ(property)</param>
        public void UpdateConditionCache<T>(params Condition[] conditions)
        {
            Sql<T>.UpdateCache(_key, conditions);
        }
        /// <summary>
        /// Delete -- where + sql
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="sql">拼接的条件查询语句</param>
        public void DeleteSqlCache<T>(string sql)
        {
            Sql<T>.DeleteSqlCache(_key, sql);
        }
        /// <summary>
        /// Delete -- where + Condition
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="conditions">创建条件查询语句 SqlCondition.And.Equ(property)</param>
        public void DeleteConditionCache<T>(params Condition[] conditions)
        {
            Sql<T>.DelectCache(_key, conditions);
        }
    }

    public class ConnectionOptions
    {

        private string _key;
        /// <summary>
        /// 添加一个缓存的key
        /// </summary>
        /// <param name="key">缓存的key</param>
        /// <returns>链式调用</returns>
        public ConnectionOptions Key(string key)
        {
            _key = key;
            return this;
        }
        /// <summary>
        /// 添加一个数据库的连接类型以及对应的链接字符串
        /// </summary>
        /// <typeparam name="T">数据库连接类型</typeparam>
        /// <param name="connection_string">连接字符串</param>
        public void AddConnection<T>(string connection_string)
        {
            Vasilier.AddConnection<T>(_key, connection_string);
        }

    }
}
