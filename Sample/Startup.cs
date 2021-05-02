using DynamicControllersFactory;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Sample
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            //.AddNewtonsoftJson(
            //options =>
            //{
            //    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
            //    //设置时间格式
            //    options.SerializerSettings.DateFormatString = "yyyy-MM-dd";
            //}
            //);

            // services.AddWebApiAssembly(null);//添加程序集
            services.AddWebApiDirectory();
            services.AddDynamicWebApi(new DynamicWebApiOptions() {  ControllerFeature=(P)=> {
                if (P.GetInterface(typeof(ICall).Name) == null)
                {
                    return false;
                }
                return true;
            } });
            //services.AddSwaggerGen(c => c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo()
            //{
            //    Title = "MyAPI",
            //    Version = "v1"
            //}));
            services.AddOpenApiDocument();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            //   app.UseSwagger();
            //  app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MyAPI"));
          app.UseOpenApi().UseSwaggerUi3().UseReDoc();
            
        }
    }
}
