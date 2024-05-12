using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var cert = new X509Certificate2("certificates/aspnetapp.pfx", "1234");

builder.Services.AddReverseProxy()
    .ConfigureHttpClient((context, handler) =>
        {
            handler.SslOptions = new SslClientAuthenticationOptions
            {
                ClientCertificates = new X509CertificateCollection
                {
                    cert
                }
            };
        

        })
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();

//app.UseHttpsRedirection();

app.MapReverseProxy();

app.Run();

