using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Vasily
{
    public class ModelStruction
    {
        public HashSet<MemberInfo> Members;
        public ConcurrentDictionary<string, Action<object, object>> SetMethodCache;
        public ConcurrentDictionary<string, Func<object, object>> GetMethodCache;
        public ConcurrentDictionary<string, Type> ModelTypeCache;
        public ConcurrentDictionary<string, MethodInfo> GetMethodInfoCache;
        public ConcurrentDictionary<string, MethodInfo> SetMethodInfoCache;
        public ConcurrentDictionary<string, FieldInfo> FieldInfoCache;
        public ConcurrentDictionary<string, PropertyInfo> PropertyInfoCache;
        public PropertyInfo[] Properties;
        public FieldInfo[] Fields;
        public Type _type;
        public ModelStruction(Type type)
        {
            _type = type;
            Initialize();
            DealType();
        }
        public void Initialize()
        {
            SetMethodCache = new ConcurrentDictionary<string, Action<object, object>>();
            GetMethodCache = new ConcurrentDictionary<string, Func<object, object>>();
            ModelTypeCache = new ConcurrentDictionary<string, Type>();
            FieldInfoCache = new ConcurrentDictionary<string, FieldInfo>();
            GetMethodInfoCache = new ConcurrentDictionary<string, MethodInfo>();
            SetMethodInfoCache = new ConcurrentDictionary<string, MethodInfo>();
            PropertyInfoCache = new ConcurrentDictionary<string, PropertyInfo>();
            Members = new HashSet<MemberInfo>();
        }
        public void DealType() {

            Properties = _type.GetProperties(BindingFlags.Instance | BindingFlags.Public).OrderBy(p => p.Name).ToArray();
            Fields = _type.GetFields(BindingFlags.Instance | BindingFlags.Public).OrderBy(p => p.Name).ToArray();

            Members.UnionWith(Properties);
            Members.UnionWith(Fields);

            for (int i = 0; i < Properties.Length; i += 1)
            {
                PropertyInfoCache[Properties[i].Name] = Properties[i];
                GetMethodInfoCache[Properties[i].Name] = Properties[i].GetGetMethod(true);
                SetMethodInfoCache[Properties[i].Name] = Properties[i].GetSetMethod(true);
                ModelTypeCache[Properties[i].Name] = Properties[i].PropertyType;
            }
            for (int i = 0; i < Fields.Length; i += 1)
            {
                FieldInfoCache[Fields[i].Name] = Fields[i];
                ModelTypeCache[Fields[i].Name] = Fields[i].FieldType;
            }
        }
    }
}
