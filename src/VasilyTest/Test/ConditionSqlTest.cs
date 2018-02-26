using System;
using System.Collections.Generic;
using System.Text;
using Vasily;
using Xunit;

namespace VasilyTest
{
    [Trait("SQL语句", "条件")]
    [VasilyInitialize]
    public class CondionSqlTest
    {

        [Fact(DisplayName = "条件查询语句")]
        public void Test_SelectAll()
        {
            Assert.Equal(Sql<NormalTest>.ConditionSelect, "SELECT * FROM [tablename] WHERE ");
        }


        [Fact(DisplayName = "条件更新语句")]
        public void Test_Update()
        {
            Assert.Equal(Sql<NormalTest>.ConditionUpdate, "UPDATE [tablename] SET [Age]=@Age,[Html Title]=@Title,[UpdateTime]=@UpdateTime,[CreateTime]=@CreateTime,[School Description]=@Desc,[StudentId]=@StudentId WHERE ");
        }

        [Fact(DisplayName = "条件删除语句")]
        public void Test_Delete()
        {
            Assert.Equal(Sql<NormalTest>.ConditionDelete, "DELETE FROM [tablename] WHERE ");
        }

    }
}
