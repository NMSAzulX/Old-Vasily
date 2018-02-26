using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Vasily.Model;

namespace Vasily.Driver
{
    public class VasilyDapper<T> where T : class
    {
        public IDbConnection Connection;
        public int? Timeout;
        public VasilyDapper(string key,int? timeout=null)
        {
            Timeout = timeout;
            ConnectionMapper mapper =  VasilyCache.ConnectionCache[key];
            Connection = (IDbConnection)Activator.CreateInstance(mapper.ConnectionType, mapper.ConnectionString);
        }

        public bool Add(params object[] instance)
        {
            int result = instance.Length;
            for (int i = 0; i < instance.Length; i += 1)
            {
                result -= Connection.Execute(Sql<T>.Insert, instance[i], commandTimeout: Timeout);
            }
            return result == 0;
        }

        public bool Modify(params object[] instance)
        {
            int result = instance.Length;
            for (int i = 0; i < instance.Length; i+=1)
            {
                result -= Connection.Execute(Sql<T>.Update, instance[i], commandTimeout: Timeout);
            }
            return result==0;
        }
        
        public T GetFirst(string key,object instance,int? commandTimeout = null)
        {
            string sql = VasilyCache.GetFromCache(key);
            return Connection.QueryFirst<T>(sql, instance, commandTimeout: commandTimeout);
        }
        public IEnumerable<T> Get(int? commandTimeout=null)
        {
            return Connection.Query<T>(Sql<T>.SelectAll,commandTimeout: commandTimeout);
        }

        public T GetByPrimaryKey(object id,int? commandTimeout= null)
        {
            IEnumerable<T> results = Connection.Query<T>(Sql<T>.Select, id, commandTimeout: commandTimeout);
            var result = results.AsList();
            if (result.Count>0)
            {
                return result[0];
            }
            return null;
        }
        
        public IEnumerable<T> Get(string sql, int? commandTimeout = null)
        {
            return Connection.Query<T>(sql, commandTimeout: commandTimeout);
        }

        public bool Delete(params object[] instance)
        {
            return Connection.Execute(Sql<T>.Delete, instance,commandTimeout: Timeout) == instance.Length;
        }

        public bool IsRepeat(object instance)
        {
            return Connection.ExecuteScalar<int>(Sql<T>.CheckRepeate, instance, commandTimeout: Timeout) >0;
        }

        public int Execute(string sql, int? commandTimeout = null, params object[] instance)
        {
            int result = 0;
            for (int i = 0; i < instance.Length; i+=1)
            {
                result+= Connection.Execute(sql, instance, commandTimeout: commandTimeout);
            }
            return result;
        }
        public S ExecuteScalar<S>(string sql,object instance=null, int? commandTimeout = null)
        {
            return Connection.ExecuteScalar<S>(sql, instance, commandTimeout: commandTimeout);
        }

        public int ExecuteCache(string key, int? commandTimeout = null, params object[] instance)
        {
            int result = 0;
            string sql = VasilyCache.GetFromCache(key);
            if (sql != null)
            {
                for (int i = 0; i < instance.Length; i += 1)
                {
                    result += Connection.Execute(sql, instance, commandTimeout: commandTimeout);
                }
            }
            return result;
        }
        public IEnumerable<T> GetCache(string key, object parameter, int? commandTimeout = null)
        {
            string sql = VasilyCache.GetFromCache(key);
            if (sql != null)
            {
                return Connection.Query<T>(sql, parameter, commandTimeout: commandTimeout);
            }
            return null;
        }
    }
}
