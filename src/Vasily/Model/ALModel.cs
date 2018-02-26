using System;
using Vasily.Model;

namespace Vasily
{
    public class TableAttribute : Attribute
    {
        public TableAttribute(string tableName)
        {
            Name = tableName;
        }

        public string Name;
    }
    public class PrimaryKeyAttribute : Attribute
    {
        public bool IsManually { get; set; }
        public PrimaryKeyAttribute()
        {

        }
        public PrimaryKeyAttribute(bool shut = false)
        {
            IsManually = shut;
        }
    }
    public class IgnoreAttribute : Attribute { }
    public class NoRepeateAttribute : Attribute { }
    public class OuterMapAttribute : Attribute
    {
        public string[] OuterKeys;
        public OuterMapAttribute(params string[] keys)
        {
            OuterKeys = keys;
        }
    }




    public class AndGtrAttribute : Attribute, IAttributesLogicalOperator
    {
        public AndGtrAttribute(params string[] keys)
        {
            Keys = keys;
            CChar = OperatorChar.GTR;
            OChar = OperatorChar.AND;
        }

        public OperatorChar CChar
        {
            get;
            set;
        }
        public OperatorChar OChar
        {
            get;
            set;
        }

        public string[] Keys
        {
            get;
            set;
        }
    }
    public class AndGeqAttribute : Attribute, IAttributesLogicalOperator
    {
        public AndGeqAttribute(params string[] keys)
        {
            Keys = keys;
            CChar = OperatorChar.GEQ;
            OChar = OperatorChar.AND;
        }

        public OperatorChar CChar
        {
            get;
            set;
        }
        public OperatorChar OChar
        {
            get;
            set;
        }

        public string[] Keys
        {
            get;
            set;
        }
    }
    public class AndLssAttribute : Attribute, IAttributesLogicalOperator
    {
        public AndLssAttribute(params string[] keys)
        {
            Keys = keys;
            CChar = OperatorChar.LSS;
            OChar = OperatorChar.AND;
        }

        public OperatorChar CChar
        {
            get;
            set;
        }
        public OperatorChar OChar
        {
            get;
            set;
        }

        public string[] Keys
        {
            get;
            set;
        }
    }
    public class AndLeqAttribute : Attribute, IAttributesLogicalOperator
    {
        public AndLeqAttribute(params string[] keys)
        {
            Keys = keys;
            CChar = OperatorChar.LEQ;
            OChar = OperatorChar.AND;
        }

        public OperatorChar CChar
        {
            get;
            set;
        }
        public OperatorChar OChar
        {
            get;
            set;
        }

        public string[] Keys
        {
            get;
            set;
        }
    }
    public class AndEquAttribute : Attribute, IAttributesLogicalOperator
    {
        public AndEquAttribute(params string[] keys)
        {
            Keys = keys;
            CChar = OperatorChar.EQU;
            OChar = OperatorChar.AND;
        }

        public OperatorChar CChar
        {
            get;
            set;
        }
        public OperatorChar OChar
        {
            get;
            set;
        }

        public string[] Keys
        {
            get;
            set;
        }
    }
    public class AndNeqAttribute : Attribute, IAttributesLogicalOperator
    {
        public AndNeqAttribute(params string[] keys)
        {
            Keys = keys;
            CChar = OperatorChar.NEQ;
            OChar = OperatorChar.AND;
        }

        public OperatorChar CChar
        {
            get;
            set;
        }
        public OperatorChar OChar
        {
            get;
            set;
        }

        public string[] Keys
        {
            get;
            set;
        }
    }


    public class OrGtrAttribute : Attribute, IAttributesLogicalOperator
    {
        public OrGtrAttribute(params string[] keys)
        {
            Keys = keys;
            CChar = OperatorChar.GTR;
            OChar = OperatorChar.OR;
        }

        public OperatorChar CChar
        {
            get;
            set;
        }
        public OperatorChar OChar
        {
            get;
            set;
        }

        public string[] Keys
        {
            get;
            set;
        }
    }
    public class OrGeqAttribute : Attribute, IAttributesLogicalOperator
    {
        public OrGeqAttribute(params string[] keys)
        {
            Keys = keys;
            CChar = OperatorChar.GEQ;
            OChar = OperatorChar.OR;
        }

        public OperatorChar CChar
        {
            get;
            set;
        }
        public OperatorChar OChar
        {
            get;
            set;
        }

        public string[] Keys
        {
            get;
            set;
        }
    }
    public class OrLssAttribute : Attribute, IAttributesLogicalOperator
    {
        public OrLssAttribute(params string[] keys)
        {
            Keys = keys;
            CChar = OperatorChar.LSS;
            OChar = OperatorChar.OR;
        }

        public OperatorChar CChar
        {
            get;
            set;
        }
        public OperatorChar OChar
        {
            get;
            set;
        }

        public string[] Keys
        {
            get;
            set;
        }
    }
    public class OrLeqAttribute : Attribute, IAttributesLogicalOperator
    {
        public OrLeqAttribute(params string[] keys)
        {
            Keys = keys;
            CChar = OperatorChar.LEQ;
            OChar = OperatorChar.OR;
        }

        public OperatorChar CChar
        {
            get;
            set;
        }
        public OperatorChar OChar
        {
            get;
            set;
        }

        public string[] Keys
        {
            get;
            set;
        }
    }
    public class OrEquAttribute : Attribute, IAttributesLogicalOperator
    {
        public OrEquAttribute(params string[] keys)
        {
            Keys = keys;
            CChar = OperatorChar.EQU;
            OChar = OperatorChar.OR;
        }

        public OperatorChar CChar
        {
            get;
            set;
        }
        public OperatorChar OChar
        {
            get;
            set;
        }

        public string[] Keys
        {
            get;
            set;
        }
    }
    public class OrNeqAttribute : Attribute, IAttributesLogicalOperator
    {
        public OrNeqAttribute(params string[] keys)
        {
            Keys = keys;
            CChar = OperatorChar.NEQ;
            OChar = OperatorChar.OR;
        }

        public OperatorChar CChar
        {
            get;
            set;
        }
        public OperatorChar OChar
        {
            get;
            set;
        }

        public string[] Keys
        {
            get;
            set;
        }
    }

    public class UpdateAttribute : Attribute, IAttributesLogicalData
    {
        public UpdateAttribute(params string[] keys)
        {
            Keys = keys;
        }
        public string[] Keys
        {
            get;
            set;
        }
    }
    public class SelectAttribute : Attribute, IAttributesLogicalData
    {
        public string[] Keys
        {
            get;
            set;
        }
        public SelectAttribute(params string[] keys)
        {
            Keys = keys;
        }
    }
    public class RepeateAttribute : Attribute, IAttributesLogicalData
    {
        public string[] Keys
        {
            get;
            set;
        }
        public RepeateAttribute(params string[] keys)
        {
            Keys = keys;
        }
    }
    public class ColumnAttribute : Attribute
    {
        public string Name;

        public ColumnAttribute(string mapName)
        {
            Name = mapName;
        }
    }
}
