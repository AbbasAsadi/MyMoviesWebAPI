using System.Text.Json.Serialization;
using Asp.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using MyMovies.Data;
using MyMovies.Data.Service;
using MyMovies.Exception;

namespace MyMovies;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
        ConnectionString = Configuration.GetConnectionString("DefaultConnection");
    }

    public string ConnectionString { get; set; }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers().AddJsonOptions(options =>
            options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

        //Configure DBContext with SQL
        services.AddDbContext<AppDbContext>(options => options.UseSqlServer(ConnectionString));

        // Configure the Services
        services.AddTransient<MoviesService>();
        services.AddTransient<DirectorService>();
        services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
            }).AddMvc()
            .AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "MyMovies", Version = "v1" });
            c.SwaggerDoc("v2", new OpenApiInfo { Title = "MyMovies", Version = "v2" });
            c.OperationFilter<SwaggerDefaultValues>();
        });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseExceptionHandler();

        if (env.IsDevelopment())
        {
            // app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "MyMovies v1");
                c.SwaggerEndpoint("/swagger/v2/swagger.json", "MyMovies v2");
            });
        }

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

        AppDbInitializer.Seed(app);
    }
}