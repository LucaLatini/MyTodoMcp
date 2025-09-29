using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyTodoMcp.Models;

namespace MyTodoMcp.Data;

/// <summary>
/// Context per gestire la connessione al database SQLite
/// </summary>
public class TodoDbContext : DbContext
{
    // Percorso del file database
    private const string DbPath = "todos.db";

    /// <summary>
    /// Tabella dei todo items
    /// </summary>
    public DbSet<TodoItem> TodoItems { get; set; } = null!;

    /// <summary>
    /// Configurazione della connessione
    /// </summary>
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        // Usa un file SQLite locale
        options.UseSqlite($"Data Source={DbPath}");
        
        // Abilita logging delle query SQL (utile per debug)
        options.LogTo(Console.WriteLine, LogLevel.Information);
    }

    /// <summary>
    /// Configurazione dello schema del database
    /// </summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TodoItem>(entity =>
        {
            // Nome tabella
            entity.ToTable("TodoItems");

            // Chiave primaria
            entity.HasKey(e => e.Id);

            // Configurazione colonne
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd(); // Auto-incremento

            entity.Property(e => e.Description)
                .IsRequired()
                .HasMaxLength(500);

            entity.Property(e => e.IsCompleted)
                .IsRequired()
                .HasDefaultValue(false);

            entity.Property(e => e.CreatedAt)
                .IsRequired();

            entity.Property(e => e.Priority)
                .IsRequired()
                .HasDefaultValue(2);

            // Indice per ricerche più veloci
            entity.HasIndex(e => e.IsCompleted);
        });
    }
}