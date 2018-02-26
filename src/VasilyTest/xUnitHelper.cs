using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Vasily.Utils;
using Xunit.Abstractions;
using Xunit.Sdk;
using Vasily;

namespace VasilyTest
{
    public class VasilyInitializeAttribute : BeforeAfterTestAttribute {

        public override void Before(MethodInfo methodUnderTest)
        {
            VasilyService service = new VasilyService();
            service.AddVasily(o =>
            {
                o.SqlSplite = "[]";

            }).AddVasilyConnectionCache(o =>
            {
               // o.Key("house").AddConnection<MySqlConnection>(ConfigCache.Info.Connection);

            }).AddVasilySqlCache(o =>
            {

            });
            SqlAnalyser.Initialization(typeof(NormalTest));
            SqlAnalyser.Initialization(typeof(AlTest));
        }
    }

}
