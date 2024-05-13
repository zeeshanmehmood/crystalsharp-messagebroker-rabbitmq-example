using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using CrystalSharp;
using CrystalSharp.Messaging.RabbitMq.Configuration;
using CrystalSharp.Messaging.RabbitMq.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string host = builder.Configuration.GetSection("AppSettings:RabbitMqHost").Value;
int port = int.Parse(builder.Configuration.GetSection("AppSettings:RabbitMqPort").Value);
string username = builder.Configuration.GetSection("AppSettings:RabbitMqUsername").Value;
string password = builder.Configuration.GetSection("AppSettings:RabbitMqPassword").Value;
string clientProvidedName = builder.Configuration.GetSection("AppSettings:RabbitMqClientProvidedName").Value;
string virtualHost = builder.Configuration.GetSection("AppSettings:RabbitMqVirtualHost").Value;

RabbitMqSettings messageBrokerSettings = new(host, port, username, password, clientProvidedName, virtualHost);

CrystalSharpAdapter.New(builder.Services)
    .AddRabbitMq(messageBrokerSettings)
    .CreateResolver();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
