# Ez SQL

**Ez SQL** is a lightweight, feature-rich SQL Server query editor and database management tool built with Windows Forms (.NET Framework 4.8).

## Features

- **Multi-tab Query Editor** — Open and manage multiple SQL queries simultaneously, each in its own tab with full syntax highlighting powered by ICSharpCode.TextEditor.
- **Connection Management** — Create, organize, and switch between multiple SQL Server connections grouped into named categories.
- **Database Object Browser** — Browse database schemas, tables, views, stored procedures, scalar functions, and table-valued functions in a sidebar.
- **Intelligent Auto-complete** — Context-aware SQL completion for table names, column names, procedures, and keywords.
- **Stored Procedure Generator** — Automatically generate ADD, UPDATE, DELETE, and GET stored procedures from a selected table.
- **C# Code Generator** — Generate C# model classes and data-access stored procedure wrappers from query results or database objects.
- **Data Export** — Export query results to Excel (.xlsx), CSV, or pipe-delimited formats using ClosedXML.
- **Query History** — Automatically logs executed queries for later review.
- **Search & Replace** — Find and replace text within the active query editor.
- **Code Snippets** — Save, manage, and insert reusable SQL snippets.
- **Database Comparison (DbComparer)** — Compare the schema of two databases side by side.
- **Side-by-Side Text Comparison** — Compare query output or any text blocks line by line.
- **Syntax Highlighting Themes** — Customize SQL keyword, string, comment, and operator colors to your preference.
- **Object Searcher** — Full-text search across database object names.

## Requirements

- Windows OS
- .NET Framework 4.8
- Microsoft SQL Server (any version supporting `System.Data.SqlClient`)

## Project Structure

```
Ez SQL/
├── AdditionalForms/          # Auxiliary dialogs: ObjectSearcher, SP generators
├── Common Code/              # Shared utilities: DataExporter, Extensions, XmlSerializer
├── ConnectionBarNodes/       # TreeView node types for the connection sidebar
├── ConnectionManagement/     # Connection info model, ConxAdmin form, SQLConnectForm
├── CSharpForm/               # C# code generation dialog
├── Custom Controls/          # Reusable UI controls: AddressBar, AnimatedWaitTextBox,
│                             #   SideToSide comparers, DifferenceEngine, DataGridView extras
├── DataBaseObjects/          # Domain model: Table, View, Procedure, Field, SQLConnector, etc.
├── DbComparer/               # Database schema comparison form and model
├── EzConfig/                 # App configuration, color/syntax theme settings
├── Models/                   # Lightweight data models (Session)
├── MultiQueryForm/           # Core query editor: QueryForm, QueryExecutor, Autocomplete,
│                             #   Dialogs for SP/class generation, Search & Replace
├── QueryLog/                 # Query history form and log record classes
├── Snippets/                 # Snippet model and editor form
└── TextEditorClasses/        # SQL folding and formatting strategies for ICSharpCode.TextEditor
```

## Key Dependencies

| Package | Purpose |
|---|---|
| `ICSharpCode.TextEditor` | Syntax-highlighted SQL editor control |
| `WeifenLuo.WinFormsUI.Docking` | Dockable panel/window framework |
| `ClosedXML` | Excel export |
| `FastMember.Signed` | High-performance object member access for data binding |
| `DocumentFormat.OpenXml` | OpenXML document support |
| `ColorPickerLib.dll` | Color picker control for theme configuration |

## Getting Started

1. Open `Ez SQL.sln` in Visual Studio 2019 or later.
2. Restore NuGet packages.
3. Build and run the `Ez SQL` project (x86, .NET 4.8).
4. On first launch, add a connection via the **Connection Manager** (sidebar or menu).
5. Select a connection, open a new query tab, and start writing SQL.
