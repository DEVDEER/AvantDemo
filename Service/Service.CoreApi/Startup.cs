namespace Service.CoreApi
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.OpenApi.Models;

    public class Startup
    {
        #region constructors and destructors

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        #endregion

        #region methods

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DEVDEER Test API v1"));
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors(_corsPolicyName);
			app.UseAuthorization();
            app.UseEndpoints(
                endpoints =>
                {
                    endpoints.MapControllers();
                });
            app.UseStaticFiles();
        }

        /// <summary>
        /// Identifier for the default CORS policy.
        /// </summary>
        private readonly string _corsPolicyName = "DefaultPolicy";

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddApplicationInsightsTelemetry(
				opt =>
				{
					opt.EnableDebugLogger = false;
				});
			services.AddControllers();
            services.AddCors(cors =>
	            { 
					cors.AddPolicy(_corsPolicyName,
						builder =>
						{
							builder.AllowAnyHeader();
							builder.AllowAnyMethod();
							builder.AllowAnyOrigin();
						});
	            });
            services.AddSwaggerGen(
                c =>
                {
                    c.SwaggerDoc(
                        "v1",
                        new OpenApiInfo
                        {
                            Title = "DEVDEER Test API",
                            Version = "v1"
                        });
                    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "Service.CoreApi.xml"));
                });
        }

        #endregion

        #region properties

        public IConfiguration Configuration { get; }

        #endregion
    }
}
