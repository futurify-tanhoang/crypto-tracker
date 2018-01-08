using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using CryptoTracker.Authentication;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection.Extensions;
using CryptoTracker.Services;
using CryptoTracker.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using CryptoTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace CryptoTracker
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
            #region authentication
            // Add JWT Authentication Token options
            services.Configure<JwtTokenOptions>(options =>
            {
                options.SecretKey = Configuration["JwtTokenOptions:Secret"];
                options.Audience = Configuration["JwtTokenOptions:Audience"];
                options.Issuer = Configuration["JwtTokenOptions:Issuer"];
                options.Expiration = TimeSpan.FromMinutes(double.Parse(Configuration["JwtTokenOptions:ExpireMins"]));

                var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(options.SecretKey));
                options.SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            });

            // Add Authentication handler using jwt
            services.AddJwt(Configuration["JwtTokenOptions:Secret"], Configuration["JwtTokenOptions:Issuer"], Configuration["JwtTokenOptions:Audience"]);

            // Add Db context
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            #endregion

            // Try Add IHttpContextAccessor for get http context in db context
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IWalletService, WalletService>();
            services.AddScoped<ICryptoCurrencyService, CryptoCurrencyService>();
            services.AddScoped<ICryptoWalletService, CryptoWalletService>();


            #region app lifecycle
            // add cors
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                    builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().AllowCredentials());
            });

            services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add(new CorsAuthorizationFilterFactory("AllowAllOrigins"));
            });


            // Add mvc fw
            services.AddMvc().AddJsonOptions(options =>
            {
                // always treat unspecified kind datetime as local kind
                options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Local;

                // always serialize enum value as string
                options.SerializerSettings.Converters.Add(new StringEnumConverter());

                // keep case of name prop in de/serializing object
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
            });
            #endregion

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
