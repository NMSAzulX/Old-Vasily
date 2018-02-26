using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vasily.Utils;

namespace Vasily.Core
{
    public class VasilyLink
    {
        private SqlModel _model;
        private SqlMaker _maker;
        private string _baseQuery;
        private string _conditionQuery;
        public string SqlString {
            get {
                _conditionQuery = _conditionQuery.Replace("WHERE OR", "WHERE").Replace("WHERE AND", "WHERE");
                return EString.Contact(_baseQuery,_conditionQuery);
            }
        }

        #region Create
        private VasilyLink(Type type)
        {
            if (!VasilyCache.SqlModelCache.ContainsKey(type))
            {
                SqlAnalyser.Initialization(type);
            }
            _model = VasilyCache.SqlModelCache[type];
            _maker = new SqlMaker(_model);
        }
        #endregion

        #region Base
        public static VasilyLink Select<T>(params string[] data)
        {
            VasilyLink instance = new VasilyLink(typeof(T));
            instance._baseQuery = instance._maker.GetSelect(data);
            return instance;
        }
        public static VasilyLink Update<T>(params string[] data)
        {
            VasilyLink instance = new VasilyLink(typeof(T));
            instance._baseQuery = instance._maker.GetUpdate(data);
            return instance;
        }
        public static VasilyLink Insert<T>(params string[] data)
        {
            VasilyLink instance = new VasilyLink(typeof(T));
            instance._baseQuery = instance._maker.GetInsert(data);
            return instance;
        }
        public static VasilyLink Delete<T>()
        {
            VasilyLink instance = new VasilyLink(typeof(T));
            instance._baseQuery = instance._maker.GetDelete();
            return instance;
        }
        public static VasilyLink Repeate<T>(params string[] data)
        {
            VasilyLink instance = new VasilyLink(typeof(T));
            instance._baseQuery = instance._maker.GetRepeate(data);
            return instance;
        }
        #endregion

        #region And
        public VasilyLink AndGtr(params string[] data)
        {
            foreach (var item in data)
            {
                _conditionQuery = EString.Contact(_conditionQuery,_maker.GetOperator(Model.OperatorChar.AND, Model.OperatorChar.GTR, item));
            }
            return this;
        }
        public VasilyLink AndGeq(params string[] data)
        {
            foreach (var item in data)
            {
                _conditionQuery = EString.Contact(_conditionQuery,_maker.GetOperator(Model.OperatorChar.AND, Model.OperatorChar.GEQ, item));
            }
            return this;
        }
        public VasilyLink AndEqu(params string[] data)
        {
            foreach (var item in data)
            {
                _conditionQuery = EString.Contact(_conditionQuery,_maker.GetOperator(Model.OperatorChar.AND, Model.OperatorChar.EQU, item));
            }
            return this;
        }
        public VasilyLink AndNeq(params string[] data)
        {
            foreach (var item in data)
            {
                _conditionQuery = EString.Contact(_conditionQuery,_maker.GetOperator(Model.OperatorChar.AND, Model.OperatorChar.NEQ, item));
            }
            return this;
        }
        public VasilyLink AndLss(params string[] data)
        {
            foreach (var item in data)
            {
                _conditionQuery = EString.Contact(_conditionQuery,_maker.GetOperator(Model.OperatorChar.AND, Model.OperatorChar.LSS, item));
            }
            return this;
        }
        public VasilyLink AndLeq(params string[] data)
        {
            foreach (var item in data)
            {
                _conditionQuery = EString.Contact(_conditionQuery,_maker.GetOperator(Model.OperatorChar.AND, Model.OperatorChar.LEQ, item));
            }
            return this;
        }
        #endregion

        #region Or
        public VasilyLink OrGtr(params string[] data)
        {
            foreach (var item in data)
            {
                _conditionQuery = EString.Contact(_conditionQuery,_maker.GetOperator(Model.OperatorChar.OR, Model.OperatorChar.GTR, item));
            }
            return this;
        }
        public VasilyLink OrGeq(params string[] data)
        {
            foreach (var item in data)
            {
                _conditionQuery = EString.Contact(_conditionQuery,_maker.GetOperator(Model.OperatorChar.OR, Model.OperatorChar.GEQ, item));
            }
            return this;
        }
        public VasilyLink OrEqu(params string[] data)
        {
            foreach (var item in data)
            {
                _conditionQuery = EString.Contact(_conditionQuery,_maker.GetOperator(Model.OperatorChar.OR, Model.OperatorChar.EQU, item));
            }
            return this;
        }
        public VasilyLink OrNeq(params string[] data)
        {
            foreach (var item in data)
            {
                _conditionQuery = EString.Contact(_conditionQuery,_maker.GetOperator(Model.OperatorChar.OR, Model.OperatorChar.NEQ, item));
            }
            return this;
        }
        public VasilyLink OrLss(params string[] data)
        {
            foreach (var item in data)
            {
                _conditionQuery = EString.Contact(_conditionQuery,_maker.GetOperator(Model.OperatorChar.OR, Model.OperatorChar.LSS, item));
            }
            return this;
        }
        public VasilyLink OrLeq(params string[] data)
        {
            foreach (var item in data)
            {
                _conditionQuery = EString.Contact(_conditionQuery,_maker.GetOperator(Model.OperatorChar.OR, Model.OperatorChar.LEQ, item));
            }
            return this;
        }
        #endregion

        #region GroupBy
        /*
        public VasilyLink GroupBy(params string[] data)
        {
            _conditionQuery.Append("GROUP BY");
            for (int i = 0; i < data.Length-1; i+=1)
            {
                _conditionQuery = EString.Contact(_conditionQuery,data[i],",");
            }
            _conditionQuery.Append(data[data.Length - 1]);
            return this;
        }
        public VasilyLink OrderBy(params string[] data)
        {
            _conditionQuery.Append("ORDER BY");
            for (int i = 0; i < data.Length - 1; i += 1)
            {
                _conditionQuery = _conditionQuery.Append(data[i]).Append(",");
            }
            _conditionQuery.Append(data[data.Length - 1]);
            return this;
        }*/
        #endregion

    }
}
