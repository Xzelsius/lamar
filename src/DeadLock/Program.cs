using DeadLock.Services;
using Lamar;
using Lamar.Microsoft.DependencyInjection;
using Marten;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.FeatureManagement;

namespace DeadLock
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Host.UseLamar();

            // Register them services
            builder.Services.AddSingleton<WolverineOptions>();

            builder.Services.AddMarten(opts => 
            { 
                opts.Connection("host=localhost;database=postgres;password=postgres;username=postgres");
                opts.UseSystemTextJsonForSerialization();
            });

            builder.Services.AddFeatureManagement();

            builder.Services.AddHttpContextAccessor();

            builder.Services.AddScoped<DefaultTenantIdProvider>();
            builder.Services.AddScoped<ITenantIdProvider>(sp => sp.GetRequiredService<DefaultTenantIdProvider>());
            builder.Services.AddSingleton<ITenantIdResolver, TenantIdResolver1>();
            builder.Services.AddSingleton<ITenantIdResolver, TenantIdResolver2>();

            builder.Services.AddScoped<ITenantContext, WebTenantContext>();

            builder.Services.AddScoped<IUserContext, WebUserContext>();
            builder.Services.AddScoped<IUserContextInfoResolver, UserContextInfoResolver>();

            builder.Services.AddOptions<ProAuthClientOptions>().BindConfiguration(ProAuthClientOptions.OptionsKey);
            builder.Services.AddSingleton<IServiceEndpointSettings>(sp => 
            {
                _ = sp.GetRequiredService<IHostEnvironment>();
                _ = sp.GetRequiredService<IOptions<ProAuthClientOptions>>().Value;

                return new ServiceEndpointSettingsBase();
            });
            builder.Services.AddSingleton<ITokenHandler, ProAuthTokenHandler>();
            builder.Services.AddSingleton<IServiceClientFactory, ServiceClientFactory>();
            builder.Services.AddSingleton<IProAuthManagementService, ProAuthManagementService>();

            builder.Services.Configure("Marten", (TenancyOptions opts) => { });

            builder.Services.AddDbContextFactory<DeadLockDbContext>(
                (sp, opts) =>
                {
                    _ = sp.GetRequiredService<IOptionsSnapshot<TenancyOptions>>().Get("Marten");
                    _ = sp.GetRequiredService<ITenantIdProvider>();

                    opts.UseNpgsql("host=localhost;database=postgres;password=postgres;username=postgres");
                },
                ServiceLifetime.Scoped);

            builder.Services.AddSingleton<IConditionFactory, ConditionFactory>();
            builder.Services.AddSingleton<IExpressionCacheFactory, ExpressionCacheFactory>();

            builder.Services.AddScoped<ISecurityFilterContextFactory, SecurityFilterContextFactory>();
            builder.Services.AddScoped<IResourceSecurityContextService, ResourceSecurityContextService>();
            builder.Services.AddScoped<IRoleAssignmentRepository, RoleAssignmentRepository>();
            builder.Services.AddScoped<IPermissionService, PermissionService>();

            var app = builder.Build();

            app.MapGet("/r1", async (WolverineOptions o, IContainer c, IProAuthManagementService p) =>
            {
                await using var scope = c.GetNestedContainer();
                _ = scope.GetInstance<IUserContext>();

                return Results.Ok();
            });

            app.MapGet("/r2", async (WolverineOptions o, IContainer c) =>
            {
                await using var scope = c.GetNestedContainer();
                _ = scope.GetInstance<ISecurityFilterContextFactory>();
                _ = scope.GetInstance<IDbContextFactory<DeadLockDbContext>>();

                return Results.Ok();
            });

            app.MapGet("/r3", async (WolverineOptions o, IContainer c) =>
            {
                await using var scope = c.GetNestedContainer();
                _ = scope.GetInstance<ISecurityFilterContextFactory>();
                _ = scope.GetInstance<IDbContextFactory<DeadLockDbContext>>();

                return Results.Ok();
            });

            app.MapGet("/r4", async (WolverineOptions o, IContainer c) =>
            {
                await using var scope = c.GetNestedContainer();
                _ = scope.GetInstance<IPermissionService>();
                _ = scope.GetInstance<IUserContext>();

                return Results.Ok();
            });

            app.MapGet("/r5", async (WolverineOptions o, IContainer c) =>
            {
                await using var scope = c.GetNestedContainer();
                _ = scope.GetInstance<ISecurityFilterContextFactory>();
                _ = scope.GetInstance<IDbContextFactory<DeadLockDbContext>>();

                return Results.Ok();
            });

            app.MapGet("/r6", async (WolverineOptions o, IContainer c) =>
            {
                await using var scope = c.GetNestedContainer();
                _ = scope.GetInstance<IDbContextFactory<DeadLockDbContext>>();
                _ = scope.GetInstance<IUserContext>();

                return Results.Ok();
            });

            app.Run();
        }
    }
}
