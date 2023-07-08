using System;
using Microsoft.Owin;
using Owin;
using Hangfire;
using Hangfire.SqlServer;
using System.Collections.Generic;

[assembly: OwinStartup(typeof(Smartax.WebApi.Services.Startup))]

namespace Smartax.WebApi.Services
{
    public class Startup
    {
        private IEnumerable<IDisposable> GetHangfireServers()
        {
            GlobalConfiguration.Configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage("Server=.\\SQLEXPRESS; Database=HangfireTest;user id=sa;password=123456;Integrated Security=True;", new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.Zero,
                    UseRecommendedIsolationLevel = true,
                    DisableGlobalLocks = true
                });

            yield return new BackgroundJobServer();
        }

        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            GlobalConfiguration.Configuration.UseSqlServerStorage("DefaultConnection");

            BackgroundJob.Enqueue(() => Console.WriteLine("Getting Started with HangFire!"));
            app.UseHangfireDashboard();
            app.UseHangfireServer();
            // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=316888
        }

        private void ConfigureAuth(IAppBuilder app)
        {
            throw new NotImplementedException();
        }
    }
}
