using System;
using Vasily.Model;

namespace Vasily.Utils
{
    public class SqlMaker
    {
        public SqlModel Model;
        public SqlMaker(SqlModel model)
        {
            Model = model;
        }
        /// <summary>
        /// 获取按条件更新的SQL
        /// </summary>
        /// <param name="data">成员名数组</param>
        /// <returns>更新字符串</returns>
        public string GetUpdate(string[] data)
        {
            string update = string.Empty;
            for (int i = 0; i < data.Length; i+=1)
            {
                update = EString.Contact(update, ",",SqlAnalyser._open, Model.GetColumnName(data[i]), SqlAnalyser._close,"=@", data[i]);
            }
            update = update.Remove(0, 1);
            return EString.Contact("UPDATE ",SqlAnalyser._open, Model.Table, SqlAnalyser._close," SET ", update," WHERE"); ;
        }
        /// <summary>
        /// 获取按条件删除的SQL
        /// </summary>
        /// <returns>删除字符串</returns>
        public string GetDelete()
        {
            return EString.Contact("DELETE ",SqlAnalyser._open, Model.Table, SqlAnalyser._close," WHERE"); ;
        }
        /// <summary>
        /// 获取插入的SQL
        /// </summary>
        /// <param name="data">成员名数组</param>
        /// <returns>插入字符串</returns>
        public string GetInsert(string[] data)
        {
            string insertFields = string.Empty;
            string insertValues = string.Empty;

            for (int i = 0; i < data.Length; i += 1)
            {
                insertFields = EString.Contact(insertFields, ",",SqlAnalyser._open, Model.GetColumnName(data[i]), SqlAnalyser._close,"");
                insertValues = EString.Contact(insertValues, ",@", data[i]);
            }
            insertFields = insertFields.Remove(0, 1);
            insertValues = insertValues.Remove(0, 1);
            return EString.Contact(SqlAnalyser._open, Model.Table, SqlAnalyser._close," (", insertFields, ") VALUES (", insertValues, ")");
        }

        /// <summary>
        /// 获取查重的SQL
        /// </summary>
        /// <param name="data">成员名数组</param>
        /// <returns>查重字符串</returns>
        public string GetRepeate(string[] data)
        {
            string where = string.Empty;
            for (int i = 0; i < data.Length; i += 1)
            {
                where = EString.Contact(" AND ",SqlAnalyser._open, Model.GetColumnName(data[i]), SqlAnalyser._close,"=@", data[i]);
            }
            where = where.Remove(0, 4);
            return EString.Contact("SELECT COUNT(*) FROM ",SqlAnalyser._open, Model.Table, SqlAnalyser._close," WHERE", where);
        }

        /// <summary>
        /// 获取查询的SQL
        /// </summary>
        /// <param name="data">成员名数组</param>
        /// <returns>查询字符串</returns>
        public string GetSelect(string[] data)
        {
            string select = string.Empty;
            for (int i = 0; i < data.Length; i += 1)
            {
                select = EString.Contact(select,",",SqlAnalyser._open, Model.GetColumnName(data[i]), SqlAnalyser._close,"");
            }
            select = select.Remove(0, 1);
            return EString.Contact("SELECT ", select, " FROM ",SqlAnalyser._open, Model.Table, SqlAnalyser._close," WHERE");
        }

        /// <summary>
        /// 获取AL操作字符串
        /// </summary>
        /// <param name="ochar">优先级运算</param>
        /// <param name="cchar">比较运算</param>
        /// <param name="data">成员名</param>
        /// <returns>按条件查询字符串</returns>
        public string GetOperator(OperatorChar ochar,OperatorChar cchar,string data)
        {
            string o = string.Empty;
            switch (ochar)
            {
                case OperatorChar.AND:
                    o = "AND";
                    break;
                case OperatorChar.OR:
                    o = "OR";
                    break;
                default:
                    break;
            }
            string c = string.Empty;
            switch (cchar)
            {
                case OperatorChar.EQU:
                    c = "=";
                    break;
                case OperatorChar.NEQ:
                    c = "<>";
                    break;
                case OperatorChar.LSS:
                    c = "<";
                    break;
                case OperatorChar.LEQ:
                    c = "<=";
                    break;
                case OperatorChar.GTR:
                    c = ">";
                    break;
                case OperatorChar.GEQ:
                    c = ">=";
                    break;
                default:
                    break;
            }
            string where = string.Empty;
            where = EString.Contact(" ", o, " ",SqlAnalyser._open, Model.GetColumnName(data), SqlAnalyser._close, c, "@", data);
            return where;
        }
    }
}
