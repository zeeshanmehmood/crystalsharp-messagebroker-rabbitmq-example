using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CrystalSharp;
using CrystalSharp.Messaging.Distributed;
using CrystalSharp.Messaging.Distributed.Models;
using CrystalSharp.Messaging.RabbitMq.Configuration;
using CrystalSharp.Messaging.RabbitMq.Extensions;

IResolver resolver = ConfigureApp();
IMessageBroker messageBroker = resolver.Resolve<IMessageBroker>();
IList<string> queues = new List<string>() { "customer-order-queue" };
GeneralConsumer consumer = new()
{
    Queues = queues,
    Action = ProcessMessage
};
bool keepRunning = true;
Console.CancelKeyPress += CancelKeyPressHandler;

try
{
    Console.WriteLine("Listener is started:");

    await messageBroker.StartConsuming(consumer).ConfigureAwait(false);

    while (keepRunning)
    {
        //
    }
}
catch
{
    keepRunning = false;
}

if (!keepRunning)
{
    await messageBroker.StopConsuming().ConfigureAwait(false);
    messageBroker.Disconnect();

    Console.WriteLine("Listener stopped.");
}

void CancelKeyPressHandler(object sender, ConsoleCancelEventArgs e)
{
    e.Cancel = true;
    keepRunning = false;
}

void ProcessMessage(string message)
{
    Console.WriteLine("============>>> NEW MESSAGE");
    Console.WriteLine(message);
    Console.WriteLine("===========================");
}

IResolver ConfigureApp()
{
    IConfigurationBuilder builder = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
    IConfigurationRoot _configuration = builder.Build();
    IServiceCollection services = new ServiceCollection();
    string host = _configuration.GetSection("AppSettings:RabbitMqHost").Value;
    int port = int.Parse(_configuration.GetSection("AppSettings:RabbitMqPort").Value);
    string username = _configuration.GetSection("AppSettings:RabbitMqUsername").Value;
    string password = _configuration.GetSection("AppSettings:RabbitMqPassword").Value;
    string clientProvidedName = _configuration.GetSection("AppSettings:RabbitMqClientProvidedName").Value;
    string virtualHost = _configuration.GetSection("AppSettings:RabbitMqVirtualHost").Value;
    RabbitMqSettings messageBrokerSettings = new(host, port, username, password, clientProvidedName, virtualHost);

    IResolver resolver = CrystalSharpAdapter.New(services)
        .AddRabbitMq(messageBrokerSettings)
        .CreateResolver();

    return resolver;
}
