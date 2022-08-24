using System.Text;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using E_Wallet.Application;
using E_Wallet.EntityFramework;
using E_Wallet.WebApi.Filters;

namespace E_Wallet.WebApi;

internal static class HostingExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        //AddSecretsJson(builder.Configuration);
        builder.Services.AddApplicationLayer();
        builder.Services.AddInfrastructureLayer(builder.Configuration);
        builder.Services.AddHttpContextAccessor();

        //builder.Services.AddControllers(configure =>
        //{
        //    configure.Filters.Add(HMACAuthorizationAttribute);
        //})
        builder.Services.AddControllers()
        .ConfigureApiBehaviorOptions(options =>
        {
            options.InvalidModelStateResponseFactory = context =>
                new BadRequestObjectResult(DataResult.CreateError(GetModelStateErrors(context.ModelState)));
        });

        //builder.Services.AddDbContext<AppDbContext>(options =>
        //{
        //    options.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection")).UseLazyLoadingProxies();
        //});
        builder.Services.AddScoped<HMACAuthorizationAttribute>();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        return builder.Build();
    }

    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseCors(builder =>
        {
            builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
        });

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();
        return app;
    }

    //private static void AddSecretsJson(IConfigurationBuilder configuration)
    //{
    //    var path = Path.Combine(AppContext.BaseDirectory, "secrets.json");
    //    configuration.AddJsonFile(path, true);
    //}

    private static string GetModelStateErrors(ModelStateDictionary modelState)
    {
        var errors = new StringBuilder();
        foreach (var error in modelState.Values.SelectMany(modelStateValue => modelStateValue.Errors))
        {
            errors.Append($"{error.ErrorMessage} ");
        }

        return errors.ToString();
    }
}
