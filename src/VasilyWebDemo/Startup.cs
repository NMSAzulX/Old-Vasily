using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VasilyWebDemo.Entity;

namespace VasilyWebDemo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            services.AddVasily(o =>
            {
                //这里指定了数据库独特的分隔符
                //例如SqlServer是这样 [tablename]
                //MySql是这样 `tablename`
                //以下是用MySql例子
                o.SqlSplite = "``";

            }).AddVasilyConnectionCache(o => {

                //key：每个key对应一种IDbConnection,同时也对应一种连接字符串
                //因此支持多种，多个数据库操作
                //o.Key("AdSql").AddConnection<"这里填DbConnection类型，例如MySqlConnection">("这里填连接字符串");

            }).AddVasilySqlCache(o => {

                //o.Key("fuzzyQuery").SelectSqlCache<Advertisement>("subject like concat(concat('%',@subject),'%')");
                //o.Key("GetActiveByGid").SelectSqlCache<Advertisement>("gid=@gid AND isEnabled=1");
                //o.Key("GetByGid").SelectConditionCache<Advertisement>("gid");
                //o.Key("GetByTid").SelectConditionCache<AdGroup>("tid");
                /* 相当于 SELECT * FROM TABLE WHERE Sid=@Sid AND Tid=@Tid And Age>@Age
                o.Key("GetBy =Sid and =Tid >Age").SelectConditionCache<AdType>(
                    "Sid", 
                    SqlCondition.AND.EQU("Tid"), 
                    SqlCondition.AND.GTR("Age")
                );*/
                /* UPDATE XXXXXX WHERE Tid=@Tid AND Age>@Age
                o.Key("GetBy =Sid and =Tid >Age").UpdateConditionCache<AdType>(
                   SqlCondition.EMPTY.EQU("Tid"),
                   SqlCondition.AND.GTR("Age")
               );*/

            });
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
