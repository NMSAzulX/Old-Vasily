using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Vasily.Utils
{
    public delegate void Setter(SqlModel value);
    public static class SqlCopyer
    {
        public static void Initialize(SqlModel model) {
            Type typeHandler = typeof(Sql<>);
            Type _type = typeHandler.MakeGenericType(model.TypeHandler);
            if (model.TypeHandler.GetInterface("IVasily")!=null)
            {
                CopyModel(_type, model);
            }
        }
        public static void CopyModel(Type type,SqlModel value)
        {
            FieldInfo[] infos = typeof(SqlModel).GetFields(BindingFlags.Public | BindingFlags.Instance);
            for (int i = 0; i < infos.Length; i+=1)
            {
                FieldInfo info = type.GetField(infos[i].Name);
                DynamicMethod method = new DynamicMethod(type.Name + infos[i].Name, null, new Type[] { typeof(SqlModel) });
                ILGenerator il = method.GetILGenerator();
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldfld, infos[i]);
                il.Emit(OpCodes.Stsfld, info);
                il.Emit(OpCodes.Ret);
                Setter action = (Setter)(method.CreateDelegate(typeof(Setter)));
                action(value);
            }
        }
    }
}
