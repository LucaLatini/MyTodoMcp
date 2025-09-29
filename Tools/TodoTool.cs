using System.ComponentModel;
using Microsoft.EntityFrameworkCore;
using ModelContextProtocol.Server;
using MyTodoMcp.Data;
using MyTodoMcp.Models;

namespace MyTodoMcp.Tools;

/// <summary>
/// Strumenti MCP per gestire la todo list
/// </summary>
[McpServerToolType]
public class TodoTool
{
    private readonly TodoDbContext _db;

    public TodoTool(TodoDbContext db)
    {
        _db = db;
    }

    /// <summary>
    /// Aggiunge un nuovo todo
    /// </summary>
    [McpServerTool(Name = "add_todo", Title = "Aggiungi un task")]
    [Description("Aggiunge un nuovo task alla lista")]
    public async Task<string> AddTodoAsync(
        [Description("Descrizione del task")] string description,
        [Description("Priorità: 1=Bassa, 2=Media, 3=Alta (default: 2)")] int priority = 2)
    {
        // Validazione input
        if (string.IsNullOrWhiteSpace(description))
        {
            return " Errore: La descrizione non può essere vuota!";
        }

        if (priority < 1 || priority > 3)
        {
            return " Errore: La priorità deve essere 1, 2 o 3!";
        }

        // Crea nuovo todo
        var todo = new TodoItem
        {
            Description = description.Trim(),
            Priority = priority,
            CreatedAt = DateTime.UtcNow
        };

        // Salva nel database
        _db.TodoItems.Add(todo);
        await _db.SaveChangesAsync();

        var priorityText = priority switch
        {
            1 => " Bassa",
            2 => " Media",
            3 => " Alta",
            _ => "Sconosciuta"
        };

        return $"✅ Task aggiunto con successo!\n" +
               $"ID: {todo.Id}\n" +
               $"Descrizione: {todo.Description}\n" +
               $"Priorità: {priorityText}";
    }

    /// <summary>
    /// Mostra tutti i todo
    /// </summary>
    [McpServerTool(Name = "list_todos", Title = "Lista tutti i task")]
    [Description("Mostra tutti i task, opzionalmente filtrati per stato")]
    public async Task<string> ListTodosAsync(
        [Description("Filtra per stato: all, completed, pending (default: all)")] string filter = "all")
    {
        IQueryable<TodoItem> query = _db.TodoItems;

        // Applica filtro
        query = filter.ToLower() switch
        {
            "completed" => query.Where(t => t.IsCompleted),
            "pending" => query.Where(t => !t.IsCompleted),
            _ => query
        };

        // Ordina per priorità (alta -> bassa) e data
        var todos = await query
            .OrderByDescending(t => t.Priority)
            .ThenBy(t => t.CreatedAt)
            .ToListAsync();

        if (!todos.Any())
        {
            return " Nessun task trovato!";
        }

        var result = $" **Lista Task** ({todos.Count} totali)\n\n";

        foreach (var todo in todos)
        {
            var status = todo.IsCompleted ? "✓" : "○";
            var priority = todo.Priority switch
            {
                1 => "bassa",
                2 => "media",
                3 => "alta",
                _ => "sconosciuta"
            };

            result += $"{status} [{todo.Id}] {priority} {todo.Description}\n";
            result += $"   Creato: {todo.CreatedAt:dd/MM/yyyy HH:mm}\n\n";
        }

        return result;
    }

    /// <summary>
    /// Completa un todo
    /// </summary>
    [McpServerTool(Name = "complete_todo", Title = "Completa un task")]
    [Description("Marca un task come completato")]
    public async Task<string> CompleteTodoAsync(
        [Description("ID del task da completare")] int id)
    {
        var todo = await _db.TodoItems.FindAsync(id);

        if (todo == null)
        {
            return $" Task con ID {id} non trovato!";
        }

        if (todo.IsCompleted)
        {
            return $"Il task '{todo.Description}' è già completato!";
        }

        todo.IsCompleted = true;
        await _db.SaveChangesAsync();

        return $" Task completato!\n" +
               $"ID: {todo.Id}\n" +
               $"Descrizione: {todo.Description}";
    }

    /// <summary>
    /// Elimina un todo
    /// </summary>
    [McpServerTool(Name = "delete_todo", Title = "Elimina un task")]
    [Description("Rimuove un task dalla lista")]
    public async Task<string> DeleteTodoAsync(
        [Description("ID del task da eliminare")] int id)
    {
        var todo = await _db.TodoItems.FindAsync(id);

        if (todo == null)
        {
            return $" Task con ID {id} non trovato!";
        }

        var description = todo.Description;
        _db.TodoItems.Remove(todo);
        await _db.SaveChangesAsync();

        return $" Task eliminato!\n" +
               $"ID: {id}\n" +
               $"Descrizione: {description}";
    }

    /// <summary>
    /// Modifica la descrizione di un todo
    /// </summary>
    [McpServerTool(Name = "update_todo", Title = "Modifica un task")]
    [Description("Aggiorna la descrizione di un task esistente")]
    public async Task<string> UpdateTodoAsync(
        [Description("ID del task da modificare")] int id,
        [Description("Nuova descrizione")] string newDescription)
    {
        if (string.IsNullOrWhiteSpace(newDescription))
        {
            return " Errore: La nuova descrizione non può essere vuota!";
        }

        var todo = await _db.TodoItems.FindAsync(id);

        if (todo == null)
        {
            return $" Task con ID {id} non trovato!";
        }

        var oldDescription = todo.Description;
        todo.Description = newDescription.Trim();
        await _db.SaveChangesAsync();

        return $" Task aggiornato!\n" +
               $"ID: {id}\n" +
               $"Vecchia: {oldDescription}\n" +
               $"Nuova: {todo.Description}";
    }

    /// <summary>
    /// Statistiche della todo list
    /// </summary>
    [McpServerTool(Name = "todo_stats", Title = "Statistiche")]
    [Description("Mostra statistiche sulla todo list")]
    public async Task<string> GetStatsAsync()
    {
        var total = await _db.TodoItems.CountAsync();
        var completed = await _db.TodoItems.CountAsync(t => t.IsCompleted);
        var pending = total - completed;
        var highPriority = await _db.TodoItems.CountAsync(t => !t.IsCompleted && t.Priority == 3);

        var completionRate = total > 0 ? (completed * 100.0 / total) : 0;

        return $" **Statistiche Todo List**\n\n" +
               $" Totale task: {total}\n" +
               $" Completati: {completed}\n" +
               $" In sospeso: {pending}\n" +
               $" Alta priorità (non completati): {highPriority}\n" +
               $" Tasso completamento: {completionRate:F1}%";
    }
}