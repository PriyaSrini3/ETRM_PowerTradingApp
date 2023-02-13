using Axpo;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using PowerTradingApplication;
using PowerTradingApplication.Helper;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<IPowerService, PowerService>();
builder.Services.AddHostedService<SchedulerService>();

// Add our Config object so it can be injected
ConfigurationManager configuration = builder.Configuration;
configuration.GetSection(ConfigurationSetup.Config).Bind(ConfigurationSetup.myConfig);

var app = builder.Build();
app.Run();
