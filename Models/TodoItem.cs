namespace MyTodoMcp.Models;

/// <summary>
/// Rappresenta un singolo item della todo list
/// </summary>
public class TodoItem
{
    /// <summary>
    /// ID univoco (auto-incrementale)
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Descrizione del task
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Se il task è completato
    /// </summary>
    public bool IsCompleted { get; set; } = false;

    /// <summary>
    /// Data di creazione
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Priorità: 1=Bassa, 2=Media, 3=Alta
    /// </summary>
    public int Priority { get; set; } = 2;
}