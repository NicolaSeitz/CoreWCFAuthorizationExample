using System.Runtime.Versioning;
using CoreWCF;
using CoreWCF.Channels;
using CoreWCF.Configuration;
using CoreWCFServer;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.HttpSys;
using Microsoft.Extensions.DependencyInjection;

namespace CoreWcfServer
{
    [SupportedOSPlatform("windows")]
    class Program
    {
        private static void Main()
        {
            var host = new WebHostBuilder()
                .ConfigureServices(services =>
                {
                    services.AddServiceModelServices();

                    services.AddAuthentication(HttpSysDefaults.AuthenticationScheme);

                    services.AddAuthorization(options =>
                    {
                        options.FallbackPolicy = options.DefaultPolicy;
                        options.AddPolicy("TestPolicy",
                            policy => policy.Requirements.Add(new AssertionRequirement(_ => false)));
                    });
                })
                .UseHttpSys(options =>
                {
                    options.Authentication.Schemes = AuthenticationSchemes.Negotiate |
                                                     AuthenticationSchemes.NTLM;
                    options.Authentication.AllowAnonymous = false;
                })
                .Configure(app =>
                {
                    app.UseAuthentication();
                    app.UseServiceModel(builder =>
                    {
                        builder.AddService<CalculatorService>();

                        var netHttpBinding = new BasicHttpBinding()
                        {
                            Security = new BasicHttpSecurity()
                            {
                                Mode = BasicHttpSecurityMode.TransportCredentialOnly,
                                Transport = new HttpTransportSecurity()
                                {
                                    ClientCredentialType = HttpClientCredentialType.InheritedFromHost
                                }
                            }
                        };

                        builder.AddServiceEndpoint<CalculatorService, ICalculatorService>(netHttpBinding,
                            "/CalculatorService");
                    });
                })
                .Build();

            host.Run();
        }
    }
}