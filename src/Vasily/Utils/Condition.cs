using System;
using System.Collections.Generic;
using System.Text;

namespace Vasily.Utils
{
    public class SqlCondition
    {
        private bool? _ao;
        private string _operation;
        private static SqlCondition _sqlCondition;
        static SqlCondition()
        {
            _sqlCondition = new SqlCondition();
        }

        public static SqlCondition AND
        {
            get {
                _sqlCondition._ao = true;
                return _sqlCondition;
            }
        }
        public static SqlCondition EMPTY
        {
            get
            {
                _sqlCondition._ao = null;
                return _sqlCondition;
            }
        }

        public static SqlCondition OR
        {
            get
            {
                _sqlCondition._ao = false;
                return _sqlCondition;
            }
        }
        
        public Condition EQU(string property)
        {
            _sqlCondition._operation = "=";
            return _sqlCondition.Return(property);
        }
        public Condition NEQ(string property)
        {
            _sqlCondition._operation = "<>";
            return _sqlCondition.Return(property);
        }
        public Condition LEQ(string property)
        {
            _sqlCondition._operation = "<=";
            return _sqlCondition.Return(property);
        }
        public Condition GEQ(string property)
        {
            _sqlCondition._operation = ">=";
            return _sqlCondition.Return(property);
        }
        public Condition LSS(string property)
        {
            _sqlCondition._operation = "<";
            return _sqlCondition.Return(property);
        }
        public Condition GTR(string property)
        {
            _sqlCondition._operation = "<";
            return _sqlCondition.Return(property);
        }
        private Condition Return(string property)
        {
            string result = string.Empty;
            if (_ao!=null)
            {
                if ((bool)_ao)
                {
                    result = string.Format(" AND {1}{0}@{1}", _operation,property);
                }
                else
                {
                    result = string.Format(" AND {1}{0}@{1}", _operation, property);
                }
                _ao = null;
            }
            else
            {
                result = string.Format("{1}{0}@{1}", _operation, property);
            }
            return new Condition(result);
        }
    }

    public struct Condition
    {
        public string Property;

        public Condition(string property)
        {
            Property = property;
        }

        public static implicit operator Condition(string property)
        {
            return new Condition(property + "=@" + property);
        }
    }
}
