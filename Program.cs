using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using ModelContextProtocol.Server;
using MyTodoMcp.Data;
using MyTodoMcp.Tools;

Console.WriteLine(" Avvio MCP Todo List Server...\n");

// Crea e configura il database
using var db = new TodoDbContext();

Console.WriteLine(" Creazione database...");
await db.Database.EnsureCreatedAsync();
Console.WriteLine("Database pronto!\n");

// Mostra statistiche iniziali
var existingCount = await db.TodoItems.CountAsync();
Console.WriteLine($"Trovati {existingCount} task esistenti\n");

// Configura il server MCP
var builder = IHost.CreateApplicationBuilder(args);

// Registra i servizi
builder.Services.AddSingleton<TodoDbContext>(db);
builder.Services.AddScoped<TodoTool>();

// Configura il server MCP
builder.Services.AddMcpServer(options =>
{
    options.ServerInfo = new ServerInfo
    {
        Name = "MyTodoMcp",
        Version = "1.0.0"
    };
});

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