using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection; // Aggiunto per chiarezza
using Microsoft.Extensions.Hosting;
using ModelContextProtocol;
using ModelContextProtocol.Protocol;
using ModelContextProtocol.Server;
using MyTodoMcp.Data;
using MyTodoMcp.Tools;
using System.Reflection; // Aggiunto per chiarezza

Console.WriteLine(" Avvio MCP Todo List Server...\n");

using var db = new TodoDbContext();

Console.WriteLine(" Creazione database...");
await db.Database.EnsureCreatedAsync();
Console.WriteLine("Database pronto!\n");

var existingCount = await db.TodoItems.CountAsync();
Console.WriteLine($"Trovati {existingCount} task esistenti\n");

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSingleton<TodoDbContext>(db);
builder.Services.AddScoped<TodoTool>();

// Configura il server MCP con la sintassi corretta
builder.Services.AddMcpServer()
.WithToolsFromAssembly(typeof(Program).Assembly); 

var host = builder.Build();

Console.WriteLine("Server MCP attivo e pronto!");
Console.WriteLine("Strumenti disponibili:");
Console.WriteLine("   - add_todo: Aggiungi un task");
Console.WriteLine("   - list_todos: Lista tutti i task");
Console.WriteLine("   - complete_todo: Completa un task");
Console.WriteLine("   - delete_todo: Elimina un task");
Console.WriteLine("   - update_todo: Modifica un task");
Console.WriteLine("   - todo_stats: Mostra statistiche");
Console.WriteLine("\n In attesa di comandi...\n");

await host.RunAsync();