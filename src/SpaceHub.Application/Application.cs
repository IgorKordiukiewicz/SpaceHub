using Microsoft.Extensions.DependencyInjection;
using SpaceHub.Application.Behaviors;
using System.Reflection;

namespace SpaceHub.Application;

public static class Application
{
    public static IServiceCollection ConfigureApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        ValidatorOptions.Global.LanguageManager.Enabled = false;
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        // Order matters, result logging has to be registered before validation,
        // otherwise result logging will not get called if validation returns failure
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ResultLoggingBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        return services;
    }
}
