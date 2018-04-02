## Vasily

------

- ### 项目背景
  ​

  ​       由于本人工作原因，经常被小工具、小任务、小接口骚扰，因此想封装一个类库方便数据库方面的操作。在经过Mellivora项目过后，对Dapper项目有了一个大致的权衡，Dapper实体类映射的缓存方法的性能已经接近极限，有些地方考虑到不同数据库的实现以及兼容性，Dapper做出了平衡。因此采用Dapper作为底层操作库。

  ​


- ### 项目介绍

  ### 

  ​	该项目主要是针对实体类进行解析，动态生成静态的SQL缓存，方便对Dapper的封装操作。

  ​



- ### 使用简介


  1. #### 实体类标签

     - 表名标签 [Table("tablename")]：对实体类进行定义，标识出实体类所属的表。

     - 主键标签 [PrimaryKey] / [PrimaryKey(true)]：标识实体类的主键，若参数为true,则主键参与增删改查的各个操作，否则主键只作为WHERE条件出现。

     - 列名标签 [Column("M K")]：数据库列名到实体类列名的映射，M K显然不能作为一个变量存在实体类中，但是它可以在数据库中使用。

     - 忽略标签 [Ignore]：该属性或者共有字段将不参Vasily的初始化解析过程，因此也不会生成到SQL字符串中。

     - 查重标签 [Repeate]：Vasily会将其解析成查重操作的SQL字符串，方便VasilyDapper的查重操作。

       ```c#
       namespace VasilyWebDemo.Entity
       {
           [Table("ad_type")]
           public class AdType : IVasily
           {
               [PrimaryKey]
               public int tid { get; set; }

               [Required(ErrorMessage = "名字不能为空！")]
               public string name { get; set; }
               public string description { get; set; }
           }
       }
       ```

       ​


  2. #### 初始化配置

     ```C#
      VasilyService service = new VasilyService();
      service.AddVasily((option) =>
      {
          option.SqlSplite = "  ";

      }).AddVasilyConnectionCache((option) =>
      {
          option.Key("Mysql").AddConnection<MySqlConnection>("Database=xxx;Data Source=192.168.1.225;User Id=sa;Password=sa;CharSet=utf8;port=3306;SslMode=None;");
          option.Key("Mssql").AddConnection<SqlConnection>("Data Source=192.168.1.125;Initial Catalog=xxx; uid = sa; pwd = sa; Max Pool Size = 512; ");

      }).AddVasilySqlCache((o) =>
      {
          /*o.Key("GetData").SelectConditionCache<Entity>(
              SqlCondition.EMPTY.EQU("year"),
              SqlCondition.AND.EQU("is_delete")
          );*/
          //select * from table where year=@year and is_delete=@is_delete

          o.Key("GetData").SelectConditionCache<Entity>(
              SqlCondition.EMPTY.EQU("year"),
              SqlCondition.AND.EQU("is_delete")
          );
      });
     ```

     ​

  3. #### Dapper封装-VasilyDapper<<EntityType>>

     - Add、Modify、Get、Delete、IsRepeat五中操作,并支持批量操作。

     - Execute、ExecuteScalar常规操作。

     - ExecuteCache、GetCache执行/查询缓存字符串操作。

       ```C#
       VasilyDapper<Entity> sqlHandler = new VasilyDapper<Entity>("KEY");

       Entity entity = new Entity();
       entity.Name="test";
       entity.Description = "just for fun";

       sqlHandler.Add(entity);
       sqlHandler.Delete(entity);
       ```


4. #### Http

- 提供基础Controller，封装了VasilyDapper

- 增加了ReturnResult返回结果，方便快速搭建WebApi.

- 支持IServiceCollection扩展方法。

  ```c#
  return Result(SqlHandler.Get());

  Result:
  {
    "msg": null,
    "data": [
      {
        "tid": 4,
        "name": "首页",
        "description": "aaaaaaaaaaaa"
      },
      {
        "tid": 5,
        "name": "首页5",
        "description": "哈哈哈哈"
      }
    ],
    "status": 0
  }
  ```

  ```c#
  if (ModelState.IsValid)
  {
      return Result(SqlHandler.Modify(value));
  }
  else
  {
       return Result();
  }

  Result:
  {
    "msg": "名字不能为空！",
    "data": null,
    "status": 2
  }
  ```

  ​



- ### 项目计划

   - [x] 将支持并发解析操作

   - [ ] 将跟随.NET Core2.1特性进行性能修改

   - [ ] 进一步封装Sql相关的操作

   - [ ] 支持增加之后自动获取主键ID操作

     ​

- ### 更新日志

   - 2018-02-26：正式发布1.0.0版本.
   - 2018-02-26：发布1.0.1版本，修改部分备注信息，增加单元测试，优化部分逻辑.
   - 2018-02-27：发布1.0.2版本，修改部分命名空间，修改Nuget标签信息，增加HttpDemo, 完善Github ReadMe文档.
   - 2018-03-24：支持并发操作，改EString为StringBuilder操作，从而支持Core2.1的性能提升.
