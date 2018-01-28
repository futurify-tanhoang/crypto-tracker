using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace Website
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
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }

            app.UseDefaultFiles();

            app.UseStaticFiles();

            var trackPackageRouteHandler = new RouteHandler(context =>
            {
                var file = env.WebRootFileProvider.GetFileInfo("index.html");
                context.Response.ContentType = "text/html";

                context.Response.Headers.Add("Cache-Control", "no-cache, no-store");
                context.Response.Headers.Add("Expires", "-1");

                return context.Response.WriteAsync(File.ReadAllText(file.PhysicalPath));
            });

            var routeBuilder = new RouteBuilder(app, trackPackageRouteHandler);
            routeBuilder.MapRoute(
             "SPA",
             @"{*url:regex(^(?!api/).*(?<!\..*)$)}");
            var routes = routeBuilder.Build();
            app.UseRouter(routes);
        }
    }
}
