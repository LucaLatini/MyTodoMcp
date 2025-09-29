# MyTodoMcp - Server To-Do List con MCP

Un semplice server per la gestione di una lista di task (To-Do list), costruito in C# e .NET. Il server espone i comandi tramite il **Model Context Protocol (MCP)** per essere utilizzato da client compatibili, come l'Agent Mode di Visual Studio Code.

Questo progetto utilizza **Entity Framework Core** con un database **SQLite** per salvare i task in un file locale (`todos.db`).

---

## 🚀 Funzionalità

- **Aggiungere** un nuovo task con descrizione e priorità.
- **Elencare** tutti i task, con filtri per stato (completati o in sospeso).
- **Completare** un task esistente.
- **Modificare** la descrizione di un task.
- **Eliminare** un task dalla lista.
- **Visualizzare statistiche** complete (numero di task totali, completati, in sospeso, ecc.).

---

## 🛠️ Prerequisiti

Per eseguire questo progetto, avrai bisogno di:
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) o superiore.
- Un client compatibile con MCP, come [Visual Studio Code](https://code.visualstudio.com/) con l'estensione appropriata per l'Agent Mode.

---

## ⚙️ Installazione e Avvio

1.  **Clona il repository:**
    ```bash
    git clone [https://github.com/tuo-nome-utente/MyTodoMcp.git](https://github.com/tuo-nome-utente/MyTodoMcp.git)
    cd MyTodoMcp
    ```

2.  **Ripristina le dipendenze:**
    ```bash
    dotnet restore
    ```

3.  **Avvia il server:**
    ```bash
    dotnet run
    ```
    Il server si avvierà e creerà automaticamente il file di database `todos.db` se non esiste.

---

## 🔌 Utilizzo con VS Code (Agent Mode)

Per interagire con il server tramite VS Code, è necessario configurare l'Agent Mode.

1.  Crea una cartella `.vscode` nella root del progetto.
2.  All'interno di `.vscode`, crea un file `mcp.json` con questo contenuto:

    ```json
    {
        "servers": {
            "MyTodoMcp": {
                "type": "stdio",
                "command": "dotnet",
                "args": [
                    "run",
                    "--project",
                    "${workspaceFolder}/MyTodoMcp.csproj"
                ]
            }
        }
    }
    ```

3.  Apri la Command Palette di VS Code (`Ctrl+Shift+P`), cerca e seleziona **"MCP: Connect to Server"** e scegli `MyTodoMcp`. Ora puoi inviare comandi al server.

---

## 📖 API / Comandi MCP Disponibili

Ecco la lista dei comandi che il server accetta.

| Comando         | Descrizione                               | Parametri                                                                 |
|-----------------|-------------------------------------------|---------------------------------------------------------------------------|
| `add_todo`      | Aggiunge un nuovo task alla lista.        | `description` (string), `priority` (int, opzionale, default: 2)           |
| `list_todos`    | Mostra tutti i task.                      | `filter` (string, opzionale, valori: "all", "completed", "pending")       |
| `complete_todo` | Marca un task come completato.            | `id` (int)                                                                |
| `delete_todo`   | Rimuove un task dalla lista.              | `id` (int)                                                                |
| `update_todo`   | Aggiorna la descrizione di un task.       | `id` (int), `newDescription` (string)                                     |
| `todo_stats`    | Mostra statistiche sulla To-Do list.      | Nessuno                                                                   |