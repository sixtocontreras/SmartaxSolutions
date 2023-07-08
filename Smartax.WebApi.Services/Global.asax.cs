using System;
using System.Collections.Generic;
using System.Configuration;
using Hangfire;
using Hangfire.SqlServer;
using Smartax.WebApi.Services.Clases.Seguridad;

namespace Smartax.WebApi.Services
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        private IEnumerable<IDisposable> GetHangfireServers()
        {
            string _connString = ConfigurationManager.ConnectionStrings["SQLServer"].ConnectionString;
            //--
            //--.UseSqlServerStorage("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=HangfireTest;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False", new SqlServerStorageOptions
            GlobalConfiguration.Configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(_connString, new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.Zero,
                    UseRecommendedIsolationLevel = true,
                    DisableGlobalLocks = true
                });

            yield return new BackgroundJobServer();
        }

        protected void Application_Start()
        {
            System.Web.Http.GlobalConfiguration.Configure(WebApiConfig.Register);

            //--Aqui inicializamos el Logs de auditoria del Api.
            log4net.Config.XmlConfigurator.Configure();

            //AreaRegistration.RegisterAllAreas();
            //RouteConfig.RegisterRoutes(RouteTable.Routes);
            //.UseActivator(...)
            //.UseLogProvider(...)

            HangfireAspNet.Use(GetHangfireServers);
            // Let's also create a sample background job
            //BackgroundJob.Enqueue(() => Debug.WriteLine("Hello world from Hangfire!"));            
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            //Tiempo de la sesión 20 minutos
            Session.Timeout = 240;
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError();
            if (ex != null)
            {

            }
        }
    }
}
