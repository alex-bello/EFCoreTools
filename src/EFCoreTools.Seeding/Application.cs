using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Hiv.HivDis.EntityFramework;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace Hiv.HivDis.Tools
{
    public class Application
    {
        public static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            var configuration = builder.Build();

            //setup our DI
            var serviceProvider = new ServiceCollection()
                .AddLogging();
                
            serviceProvider.AddDbContext<HivDisDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("HivDisDevDbConnection")));

            var services = serviceProvider.BuildServiceProvider();

            var app = new Microsoft.Extensions.CommandLineUtils.CommandLineApplication();
            var seeding = app.Command("seeding", config => {});
            var context = services.GetService<HivDisDbContext>();

            seeding.Command("create", config => {
                config.OnExecute(()=>{ 
                    config.Description = "Create seeding files for all DbSets in the DbContext";

                    Console.WriteLine("Starting Process...");
                    
                    new Seeding(context).GenerateSeedFiles("json");

                    Console.WriteLine("Process Completed...");
                    return 0;
                    });   
            });

            app.Command("snowball", config => {});
            //give people help with --help
            app.HelpOption("-? | -h | --help");
            app.Execute(args);
        }
    }
}
