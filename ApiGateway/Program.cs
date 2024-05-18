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

builder.Services.AddCors();

var app = builder.Build();

//app.UseHttpsRedirection();

app.UseCors(builder =>
    builder.AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod()
);

app.MapReverseProxy();

app.Run();

