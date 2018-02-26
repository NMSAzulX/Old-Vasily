using System;
using System.Collections.Generic;
using System.Text;
using Vasily;
using Xunit;

namespace VasilyTest
{
    [Trait("SQL语句", "AL")]
    [VasilyInitialize]
    public class ALSqlTest
    {
        [Fact(DisplayName = "根据名字和年龄查备注")]
        public void Test_Al1()
        {
            Assert.Equal(Sql<AlTest>.AL("根据名字和年龄查备注"), "SELECT [Description] FROM [tablename] WHERE [Age]=@Age AND [Name]=@Name");
        }

        [Fact(DisplayName = "查询大于当前StudentId的集合")]
        public void Test_Al2()
        {
            Assert.Equal(Sql<AlTest>.AL("查询大于当前StudentId的集合"), "SELECT [Count] FROM [tablename] WHERE [StudentId]>@StudentId");
        }

        [Fact(DisplayName = "根据学生类型或者分数查询状态集合")]
        public void Test_Al3()
        {
            Assert.Equal(Sql<AlTest>.AL("根据学生类型或者分数查询状态集合"), "SELECT [Status] FROM [tablename] WHERE [Score]=@Score OR [StudentType]=@StudentType");
        }
    }
}
