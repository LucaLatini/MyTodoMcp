using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Protocol;
using ModelContextProtocol.Server;
using MyTodoMcp.Data;
using MyTodoMcp.Tools;
using System.Reflection;

// NESSUN Console.WriteLine all'inizio

using var db = new TodoDbContext();

// La creazione del database deve avvenire in silenzio
await db.Database.EnsureCreatedAsync();

var builder = Host.CreateApplicationBuilder(args);

// Disattiva il logging di default sulla console per non "sporcare" l'output
builder.Logging.ClearProviders();

builder.Services.AddSingleton<TodoDbContext>(db);
builder.Services.AddScoped<TodoTool>();

// Configura il server MCP
builder.Services.AddMcpServer()
.WithToolsFromAssembly(typeof(Program).Assembly)
.WithStdioServerTransport();

var host = builder.Build();

// NESSUN Console.WriteLine prima di RunAsync

await host.RunAsync();