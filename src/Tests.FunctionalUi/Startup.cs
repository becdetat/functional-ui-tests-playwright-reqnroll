using System.Reflection;
using Autofac;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Playwright;
using Reqnroll.Autofac;

namespace Tests.FunctionalUi;

public static class Startup
{
    [ScenarioDependencies]
    public static void CreateServices(ContainerBuilder builder)
    {
        // Register app settings
        builder
            .RegisterInstance(new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile("appsettings.development.json", true, true)
                .Build())
            .As<IConfiguration>()
            .SingleInstance();
        builder
            .Register(context =>
            {
                var configuration = context.Resolve<IConfiguration>();
                var appSettings = new AppSettings();
                
                configuration.Bind(appSettings);
                
                return Options.Create(appSettings);
            })
            .As<IOptions<AppSettings>>();
        
        // Register Playwright
        builder
            .Register(async context =>
            {
                var appSettings = context.Resolve<IOptions<AppSettings>>().Value;
                var playwright = await Playwright.CreateAsync().ConfigureAwait(false);
                var browser = await playwright.Chromium
                    .LaunchAsync(new BrowserTypeLaunchOptions
                    {
                        Headless = appSettings.IsHeadless,
                        SlowMo = appSettings.ActionDelayMs
                    })
                    .ConfigureAwait(false);
                var page = await browser.NewPageAsync().ConfigureAwait(false);

                return page;
            })
            .As<Task<IPage>>()
            .InstancePerDependency();
        
        // Register step definition types
        var stepDefinitionTypes = Assembly.GetExecutingAssembly().GetTypes()
            .Where(x => x.IsClass && !x.IsAbstract && x.IsSubclassOf(typeof(BaseStepDefinitions)));
        foreach (var stepDefinitionType in stepDefinitionTypes)
        {
            builder.RegisterType(stepDefinitionType).AsSelf().InstancePerDependency();
        }
    }
}