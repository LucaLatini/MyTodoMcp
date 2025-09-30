using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Protocol;
using ModelContextProtocol.Server;
using MyTodoMcp.Data;
using MyTodoMcp.Tools;
using System.Reflection;

var builder = Host.CreateApplicationBuilder(args);

// Disattiva il logging di default sulla console per non "sporcare" l'output
builder.Logging.ClearProviders();

// Registra il DbContext nel servizio di dependency injection
builder.Services.AddDbContext<TodoDbContext>();

builder.Services.AddScoped<TodoTool>();

// Configura il server MCP
builder.Services.AddMcpServer()
    .WithToolsFromAssembly(typeof(Program).Assembly)
    .WithStdioServerTransport();

var host = builder.Build();

// Blocco per creare il database all'avvio (opzionale)
// Questo assicura che il database sia creato al primo avvio dell'applicazione
//using (var scope = host.Services.CreateScope())
//{
  //  var services = scope.ServiceProvider;
    //var dbContext = services.GetRequiredService<TodoDbContext>();
    //await dbContext.Database.EnsureCreatedAsync();
//}

await host.RunAsync();