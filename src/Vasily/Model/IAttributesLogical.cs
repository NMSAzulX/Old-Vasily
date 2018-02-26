using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vasily.Model;

namespace Vasily
{
    public interface IAttributesLogicalData
    {
        string[] Keys { get; set; }
    }
    public interface IAttributesLogicalOperator
    {
        string[] Keys { get; set; }
        OperatorChar OChar { get; set; }
        OperatorChar CChar { get; set; }

    }
}
