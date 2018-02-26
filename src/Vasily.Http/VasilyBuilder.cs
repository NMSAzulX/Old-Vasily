using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Vasily;
using Vasily.Utils;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class VasilyBuilder
    {
        public static IServiceCollection AddVasily(this IServiceCollection service,Action<VasilyOptions> action=null)
        {
            VasilyOptions options = new VasilyOptions();
            action?.Invoke(options);
            Vasilier.SpilteKeyWords(options.SqlSplite);
            Vasilier.Initialize(options.InterfaceName);
            return service;
        }

        public static IServiceCollection AddVasilySqlCache(this IServiceCollection service, Action<SqlOptions> action)
        {
            SqlOptions options = new SqlOptions();
            action(options);
            return service;
        }
        public static IServiceCollection AddVasilyConnectionCache(this IServiceCollection service, Action<ConnectionOptions> action)
        {
            ConnectionOptions options = new ConnectionOptions();
            action(options);
            return service;
        }
    }
}
