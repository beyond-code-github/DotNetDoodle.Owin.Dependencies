using DotNetDoodle.Owin.Dependencies;
using DotNetDoodle.Owin.Dependencies.Adapters.WebApi;
using DotNetDoodle.Owin.Dependencies.Adapters.WebApi.Infrastructure;
using Owin;
using System;
using System.ComponentModel;
using System.Net.Http;
using System.Web.Http;

namespace DotNetDoodle.Owin
{
    using global::Owin.Context;

    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class OwinExtensions
    {
        public static IAppBuilder UseWebApiWithContainer(this IAppBuilder app, HttpConfiguration configuration)
        {
            IServiceProvider appContainer = app.GetApplicationContainer();
            configuration.DependencyResolver = new OwinDependencyResolverWebApiAdapter(appContainer);
            HttpServer httpServer = new OwinDependencyScopeHttpServerAdapter(configuration);
            return app.UseWebApi(httpServer);
        }

        public static IAppBuilder UseWebApiWithContainer(this IAppBuilder app, HttpConfiguration configuration, HttpMessageHandler dispatcher)
        {
            IServiceProvider appContainer = app.GetApplicationContainer();
            configuration.DependencyResolver = new OwinDependencyResolverWebApiAdapter(appContainer);
            HttpServer httpServer = new OwinDependencyScopeHttpServerAdapter(configuration, dispatcher);
            return app.UseWebApi(httpServer);
        }

        public static IContextBuilder<HttpServer> WithContainer(this IContextBuilder<HttpServer> builder, HttpConfiguration configuration)
        {
            IServiceProvider appContainer = builder.GetApplicationContainer();
            configuration.DependencyResolver = new OwinDependencyResolverWebApiAdapter(appContainer);

            if (builder.Context == null)
            {
                builder.Context = new OwinDependencyScopeHttpServerAdapter(configuration);
                return builder;
            }

            var oldContext = builder.Context;
            builder.Context = new OwinDependencyScopeHttpServerAdapter(configuration) { InnerHandler = oldContext };
            
            return builder;
        }
    }
}