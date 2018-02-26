using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vasily;

namespace VasilyTest
{
    [Table("tablename")]
    public class NormalTest : IVasily
    {
        [PrimaryKey]
        public int ID;
        public int? Age { get; set; }

        [Repeate]
        public int StudentId;

        [Column("School Description")]
        public string Desc;
        [Column("Html Title")]
        public string Title { get; set; }

        public DateTime? UpdateTime {get;set;}
        public DateTime CreateTime;

        [Ignore]
        public string JsonTime
        {
            get { return CreateTime.ToString("yyyy-MM-dd"); }
        }
    }
}
