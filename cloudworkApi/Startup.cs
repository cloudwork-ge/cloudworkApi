using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using cloudworkApi.DataManagers;
using Newtonsoft.Json;
using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using cloudworkApi.Common;
using cloudworkApi.StoredProcedures;
using cloudworkApi.Models;
using cloudworkApi.SqlDataBaseEntity;
using System.Net.WebSockets;
using System.Threading;
using System.Text;
using cloudworkApi.Services;

namespace cloudworkApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            DataManager.configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(o => o.AddPolicy("AllowAll", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));
            services.AddRouting(r => r.SuppressCheckForUnhandledSecurityMetadata = true);
            //services.AddSession();

            services.AddDistributedMemoryCache();


            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services.AddSession(options =>
            {
                //options.IdleTimeout = TimeSpan.FromSeconds(60);
                options.Cookie.Name = ".my.Session";
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            services.AddControllers().AddMvcOptions(options =>
            {
                options.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor(_ => "ეს ველი აუცილებლად შესავსებია");

            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors("AllowAll");
            //app.UseSession();


          //  if (!env.IsDevelopment() || ResponseBuilder.isUserException)
          //  {
                //app.UseDeveloperExceptionPage();
                ExceptionHandlerOptions options = new ExceptionHandlerOptions();
                //options.
                app.UseExceptionHandler(options =>
                {
                    options.Run(async context =>
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        context.Response.ContentType = "application/json";
                        var ex = context.Features.Get<IExceptionHandlerFeature>();
                        if (ex != null)
                        {
                            if (!ResponseBuilder.isUserException)
                            {
                                CurrentInstance.IP = context.Connection.RemoteIpAddress.ToString();
                                new PKG_ERROR_LOGS().LogException(CurrentInstance.userID, ex.Error.Message.ToString(), ex.Error.StackTrace.ToString(), context.Request.Path.ToString(), CurrentInstance.IP, context.Request.QueryString.ToString());
                            }
                            var err = "";
                            err = ResponseBuilder.Error(ex.Error.Message, Configuration["error_text"].ToString(), env.IsDevelopment());
                            ResponseBuilder.isUserException = false;
                            //err = new ResponseBuilder().Error(Configuration["error_text"].ToString());

                            await context.Response.WriteAsync(err).ConfigureAwait(false);
                        }
                    });
                });
    //        }


            app.UseHttpsRedirection();

            app.UseRouting();

            var webSocketOptions = new WebSocketOptions()
            {
                KeepAliveInterval = TimeSpan.FromSeconds(120),
                ReceiveBufferSize = 4 * 1024
            };
            //webSocketOptions.AllowedOrigins.Add("https://client.com");

            app.UseWebSockets(webSocketOptions);


            app.UseAuthorization();
            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.Use(async (context, next) =>
            {
                if (context.Request.Path == "/Chat")
                {
                    if (context.WebSockets.IsWebSocketRequest)
                    {
                        await new ChatService().GetAndSendMessage(context);
                    }
                    else
                    {
                        context.Response.StatusCode = 400;
                    }
                }
                else
                {
                    await next();
                }
            });

        }
        
    }
}
