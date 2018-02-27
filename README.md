## 要你命三千系列之Vasily

------

- ### 项目背景

  ​       由于本人工作原因，经常被小工具、小任务、小接口骚扰，因此想封装一个类库方便数据库方面的操作。在经过Mellivora项目过后，对Dapper项目有了一个大致的权衡，Dapper实体类映射的缓存方法的性能已经接近极限，有些地方考虑到不同数据库的实现以及兼容性，Dapper做出了平衡。因此采用Dapper作为底层操作库。

  ​

- ### 项目介绍

  ​	该项目主要是针对实体类进行解析，动态生成静态的SQL缓存，方便对Dapper的封装操作。

  ​

- ### 使用简介

  1. #### 实体类标签

    - 表名标签 [Table("tablename")]：对实体类进行定义，标识出实体类所属的表。

    - 主键标签 [PrimaryKey] / [PrimaryKey(true)]：标识实体类的主键，若参数为true,则主键参与增删改查的各个操作，否则主键只作为WHERE条件出现。

    - 列名标签 [Column("M K")]：数据库列名到实体类列名的映射，M K显然不能作为一个变量存在实体类中，但是它可以在数据库中使用。

    - 忽略标签 [Ignore]：该属性或者共有字段将不参Vasily的初始化解析过程，因此也不会生成到SQL字符串中。

    - 查重标签 [Repeate]：Vasily会将其解析成查重操作的SQL字符串，方便VasilyDapper的查重操作。


  2. #### Dapper封装-VasilyDapper<<EntityType>>

     - Add、Modify、Get、Delete、IsRepeat五中操作,并支持批量操作。
     
     - Execute、ExecuteScalar常规操作。
     
     - ExecuteCache、GetCache执行/查询缓存字符串操作。

  3. #### Http
  
     - 提供基础Controller，封装了VasilyDapper
     
     - 增加了ReturnResult返回结果，方便快速搭建WebApi.
     
     - 支持IServiceCollection扩展方法。

       ​

- ### 项目计划

   - 将支持并发解析操作。

   - 将跟随.NET Core2.1特性进行性能修改。

   - 进一步封装Sql相关的操作。

     ​

- ### 更新日志

   - 2018-02-26：正式发布1.0.0版本。
   - 2018-02-26：发布1.0.1版本，修改部分备注信息，增加单元测试，优化部分逻辑。
   - 2018-02-27：发布1.0.2版本，修改部分命名空间，修改Nuget标签信息，增加HttpDemo, 完善Github ReadMe文档。
