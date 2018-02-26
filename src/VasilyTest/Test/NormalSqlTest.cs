using System;
using System.Collections.Generic;
using System.Text;
using Vasily;
using Xunit;

namespace VasilyTest
{
    [Trait("SQL语句", "普通")]
    [VasilyInitialize]
    public class NormalSqlTest
    {
        [Fact(DisplayName = "查重语句")]

        public void Test_Repeate()
        {
            Assert.Equal(Sql<NormalTest>.CheckRepeate, "SELECT COUNT(*) FROM [tablename] WHERE [StudentId]=@StudentId");
        }

        [Fact(DisplayName = "全部查询语句")]
        public void Test_SelectAll()
        {
            Assert.Equal(Sql<NormalTest>.SelectAll, "SELECT * FROM [tablename]");
        }

        [Fact(DisplayName = "主键查询语句")]
        public void Test_Select()
        {
            Assert.Equal(Sql<NormalTest>.Select, "SELECT * FROM [tablename] WHERE [ID]=@ID");
        }

        [Fact(DisplayName = "更新语句")]
        public void Test_Update()
        {
            Assert.Equal(Sql<NormalTest>.Update, "UPDATE [tablename] SET [Age]=@Age,[Html Title]=@Title,[UpdateTime]=@UpdateTime,[CreateTime]=@CreateTime,[School Description]=@Desc,[StudentId]=@StudentId WHERE [ID]=@ID");
        }

        [Fact(DisplayName = "插入语句")]
        public void Test_Insert()
        {
            Assert.Equal(Sql<NormalTest>.Insert, "INSERT INTO [tablename] ([Age],[Html Title],[UpdateTime],[CreateTime],[School Description],[StudentId]) VALUES (@Age,@Title,@UpdateTime,@CreateTime,@Desc,@StudentId)");
        }

        [Fact(DisplayName = "删除语句")]
        public void Test_Delete()
        {
            Assert.Equal(Sql<NormalTest>.Delete, "DELETE FROM [tablename] WHERE [ID]=@ID");
        }
    }
}
