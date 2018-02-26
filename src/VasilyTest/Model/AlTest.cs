using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vasily;

namespace VasilyTest
{
    [Table("tablename")]
    public class AlTest :IVasily
    {
        [AndEqu("根据名字和年龄查备注")]
        public string Name;
        [AndEqu("根据名字和年龄查备注")]
        public int Age;
        [Select("根据名字和年龄查备注")]
        public string Description;

        [AndGtr("查询大于当前StudentId的集合")]
        public string StudentId;
        [Select("查询大于当前StudentId的集合")]
        public int Count;


        [OrEqu("根据学生类型或者分数查询状态集合")]
        public string StudentType;
        [OrEqu("根据学生类型或者分数查询状态集合")]
        public int Score;
        [Select("根据学生类型或者分数查询状态集合")]
        public int Status;


    }
}
