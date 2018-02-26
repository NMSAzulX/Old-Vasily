using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Vasily.Model;
using Vasily.Utils;

namespace Vasily.Utils
{
    public class SqlAnalyser
    {
        internal static string _open;
        internal static string _close;
        public static void Initialization(Type type)
        {

            //创建数据库查询缓存实例
            SqlModel model = new SqlModel(type);

            //获取SQL主键
            GetPrimaryKey(model);
            //获取表名
            GetTableName(model, type);

            if (model.Table != null)
            {
                GetCIN(model);
                GetAL(model);
                GetSqlString(model);
                VasilyCache.SqlModelCache[type] = model;
            }
            SqlCopyer.Initialize(model);
        }
        private static void GetPrimaryKey(SqlModel model)
        {
            int i = 0;
            int length = model.Struction.Members.Count;
            MemberInfo[] members = model.Struction.Members.ToArray();
            while (i < length)
            {
                if (model.PrimaryKey == null || model.PrimaryKey == string.Empty)
                {
                    PrimaryKeyAttribute temp_PrimaryKeyAttributer = members[i].GetCustomAttribute<PrimaryKeyAttribute>(true);
                    if (temp_PrimaryKeyAttributer != null)
                    {
                        model.PrimaryKey = members[i].Name;
                        model.IsMaunally = temp_PrimaryKeyAttributer.IsManually;
                        break;
                    }
                }
                i += 1;
            }
            if (model.PrimaryKey!=null && !model.IsMaunally)
            {
                model.IgnoreMembers.Add(model.PrimaryKey);
            }
        }
        private static void GetTableName(SqlModel model, Type type)
        {
            TableAttribute temp_TableAttribute = type.GetCustomAttribute<TableAttribute>(true);
            if (temp_TableAttribute != null)
            {
                model.Table = temp_TableAttribute.Name;
            }
        }
        private static void GetCIN(SqlModel model)
        {
            int i = 0;
            int length = model.Struction.Members.Count;
            MemberInfo[] members = model.Struction.Members.ToArray();
            while (i < length)
            {
                string temp_Name = members[i].Name;
                ColumnAttribute temp_ColumnAttribute = members[i].GetCustomAttribute<ColumnAttribute>();
                IgnoreAttribute temp_IgnoreAttribute = members[i].GetCustomAttribute<IgnoreAttribute>();
                RepeateAttribute temp_RepeateAttribute = members[i].GetCustomAttribute<RepeateAttribute>();
                //字段映射缓存
                if (temp_ColumnAttribute != null)
                {
                    model.ColumnToRealMap[temp_ColumnAttribute.Name] = members[i].Name;
                    model.RealToColumnMap[members[i].Name] = temp_ColumnAttribute.Name;
                }
                //字段忽略缓存
                if (temp_IgnoreAttribute != null)
                {
                    model.IgnoreMembers.Add(temp_Name);
                }
                //字段查重缓存
                if (temp_RepeateAttribute != null)
                {
                    model.RepeateMembers.Add(temp_Name);
                }
                i += 1;
            }
        }

        private static void GetAL(SqlModel model) {
            Dictionary<string, List<string>> UpdateDict = new Dictionary<string, List<string>>();
            Dictionary<string, List<string>> SelectDict = new Dictionary<string, List<string>>();
            Dictionary<string, List<string>> RepeateDict = new Dictionary<string, List<string>>();
            Dictionary<string, List<ALStruct>> Where = new Dictionary<string, List<ALStruct>>();
            MemberInfo[] infos = model.Struction.Members.ToArray();
            for (int i = 0; i < infos.Length; i+=1)
            {
                MemberInfo info = infos[i];
                GetLogicalList<UpdateAttribute>(info, UpdateDict);
                GetLogicalList<SelectAttribute>(info, SelectDict);
                GetLogicalList<RepeateAttribute>(info, RepeateDict);
                GetLogicalList<AndGtrAttribute>(info, Where);
                GetLogicalList<OrGtrAttribute>(info, Where);
                GetLogicalList<AndGeqAttribute>(info, Where);
                GetLogicalList<OrGeqAttribute>(info, Where);
                GetLogicalList<AndEquAttribute>(info, Where);
                GetLogicalList<OrEquAttribute>(info, Where);
                GetLogicalList<AndNeqAttribute>(info, Where);
                GetLogicalList<OrNeqAttribute>(info, Where);
                GetLogicalList<AndLssAttribute>(info, Where);
                GetLogicalList<OrLssAttribute>(info, Where);
                GetLogicalList<AndLeqAttribute>(info, Where);
                GetLogicalList<OrLeqAttribute>(info, Where);

            }
            SqlMaker maker = new SqlMaker(model);


            foreach (var item in UpdateDict)
            {
                EString result = maker.GetUpdate(item.Value.ToArray());
                EString condition = string.Empty;
                if (Where.ContainsKey(item.Key))
                {
                    List<ALStruct> models = Where[item.Key];
                    foreach (var alItem in models)
                    {
                        condition +=  maker.GetOperator(alItem.OChar, alItem.CChar, alItem.Data);
                    }
                    switch (models[0].OChar)
                    {
                        case OperatorChar.AND:
                            condition = condition.Remove(0, 4);
                            break;
                        case OperatorChar.OR:
                            condition = condition.Remove(0, 3);
                            break;
                        default:
                            break;
                    }
                }
                result += condition;
                model.ALMap[item.Key] = result.ToString();
            }
            foreach (var item in SelectDict)
            {
                string result = maker.GetSelect(item.Value.ToArray());
                string condition = string.Empty;
                if (Where.ContainsKey(item.Key))
                {
                    List<ALStruct> models = Where[item.Key];
                    foreach (var alItem in models)
                    {
                        condition += maker.GetOperator(alItem.OChar, alItem.CChar, alItem.Data);
                    }
                    switch (models[0].OChar)
                    {
                        case OperatorChar.AND:
                            condition = condition.Remove(0, 4);
                            break;
                        case OperatorChar.OR:
                            condition = condition.Remove(0, 3);
                            break;
                        default:
                            break;
                    }
                }
                result += condition;
                model.ALMap[item.Key] = result;
            }
            foreach (var item in RepeateDict)
            {
                model.ALMap[item.Key] = maker.GetRepeate(item.Value.ToArray());
            }
        }
        private static void GetSqlString(SqlModel model)
        {
            EString insertFields = string.Empty;
            EString insertValues = string.Empty;
            EString updateFieldAndValues = string.Empty;
            EString checkRepeate = string.Empty;
            EString getModelId = string.Empty;
            string table = EString.Contact(_open, model.Table, _close," ");
            int i = 0;
            int length = model.Struction.Members.Count;
            MemberInfo[] members = model.Struction.Members.ToArray();
            while (i < length)
            {
                string memberName = members[i].Name;
                string mapMemberName = model.GetColumnName(memberName);
                if (!model.IgnoreMembers.Contains(memberName))
                {
                    insertFields = insertFields.Append(",", _open, mapMemberName, _close);
                    insertValues = insertValues.Append(",@", memberName);

                    updateFieldAndValues = updateFieldAndValues.Append(",",_open, mapMemberName,_close, "=@", memberName);
                    getModelId = getModelId.Append(" AND ", _open, mapMemberName, _close,"=@", memberName);

                    if (model.RepeateMembers.Contains(memberName))
                    {
                        checkRepeate = checkRepeate.Append(" AND ", _open, mapMemberName, _close,"=@", memberName);
                    }
                }
                i += 1;
            }

            insertFields = insertFields.Remove(0, 1);
            insertValues = insertValues.Remove(0, 1);
            updateFieldAndValues = updateFieldAndValues.ToString().Remove(0, 1);
            if (checkRepeate.ToString().Length > 5)
            {
                checkRepeate = checkRepeate.Remove(0, 5);
            }
            getModelId = getModelId.Remove(0, 4);

            model.Update = EString.Contact("UPDATE ", table, "SET ", updateFieldAndValues.ToString());
            model.Select = EString.Contact("SELECT * FROM ", table.Remove(table.Length-1,1));
            model.Delete = EString.Contact("DELETE FROM ", table);
            model.Insert = EString.Contact("INSERT INTO ", table, "(", insertFields.ToString(), ") VALUES (", insertValues.ToString(), ")");
            System.Diagnostics.Debug.WriteLine(model.Update);
            model.ConditionDelete = EString.Contact(model.Delete,"WHERE ");
            model.ConditionSelect = EString.Contact(model.Select," WHERE ");
            model.ConditionUpdate = EString.Contact(model.Update," WHERE ");

            model.SelectAll = model.Select;

            if (model.PrimaryKey != null)
            {
                EString byPrimaryKey = string.Empty;
                string realPrimaryKey = model.GetColumnName(model.PrimaryKey);
                string conditionByPrimaryKey = byPrimaryKey.Append(_open, realPrimaryKey, _close, "=@", model.PrimaryKey);
                model.GetPrimaryKey = EString.Contact("SELECT ", _open, realPrimaryKey, _close, " FROM ",table, "WHERE", getModelId.ToString());
                model.Update = EString.Contact(model.ConditionUpdate, conditionByPrimaryKey);
                model.Select = EString.Contact(model.ConditionSelect, conditionByPrimaryKey);
                model.Delete = EString.Contact(model.ConditionDelete, conditionByPrimaryKey);
            }

            model.CheckRepeate = EString.Contact("SELECT COUNT(*) FROM ", table, "WHERE ", checkRepeate.ToString());
        }

        private static void GetLogicalList<T>(MemberInfo info, Dictionary<string, List<string>> dict) where T:Attribute, IAttributesLogicalData
        {
            IAttributesLogicalData instance = info.GetCustomAttribute<T>(true);
            if (instance==null)
            {
                return;
            }
            string[] keys = instance.Keys;
            for (int i = 0; i < keys.Length; i += 1)
            {
                if (!dict.ContainsKey(keys[i]))
                {
                    dict[keys[i]] = new List<string>();
                }
                dict[keys[i]].Add(info.Name);
            }
        }
        private static void GetLogicalList<T>(MemberInfo info, Dictionary<string, List<ALStruct>> dict) where T : Attribute, IAttributesLogicalOperator
        {
            IAttributesLogicalOperator instance = info.GetCustomAttribute<T>(true);
            if (instance == null)
            {
                return;
            }
            string[] keys = instance.Keys;
            for (int i = 0; i < keys.Length; i += 1)
            {
                if (!dict.ContainsKey(keys[i]))
                {
                    dict[keys[i]] = new List<ALStruct>();
                }
                dict[keys[i]].Add(new ALStruct() { CChar = instance.CChar, OChar = instance.OChar, Data = info.Name });
            }
        }
    }
}
