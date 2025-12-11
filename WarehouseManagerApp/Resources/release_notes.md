# Warehouse Manager App

A modern desktop application for managing warehouse inventory built with WPF and .NET 8.

![Main Dashboard](docs.screenshots/dashboard.png)

## Overview

Warehouse Manager App is a Windows desktop application designed to track products across multiple warehouses. It features an intuitive interface for complete inventory management with real-time updates and comprehensive data validation.

---

## Application Requirements Compliance

This application fulfills the following technical requirements:

### Multiple Views/Screens
The application includes **5 distinct views**:
- **Dashboard** - Statistics overview with charts and key metrics
- **Inventory** - Product management with search and filtering
- **Warehouses** - Warehouse location management
- **About** - Application information
- **Edit Product Window** - Separate modal window for editing

Navigation is handled through a side navigation bar with instant view switching.


### Form Validation
All input forms include comprehensive validation:
- **Required fields** - Name, SKU, quantity fields cannot be empty
- **Data type validation** - Numeric fields accept only valid numbers
- **Range validation** - Quantity and capacity must be positive values
- **Unique constraints** - SKU must be unique across all products

Validation is implemented using:
- `INotifyDataErrorInfo` interface
- Data annotations (`[Required]`, `[Range]`, etc.)
- Custom validation logic in ViewModels

![Form Validation](docs.screenshots/validation.png)

### CRUD Operations
Complete CRUD functionality for both Products and Warehouses:

**Products:**
- **Create** - Add new products with validation
- **Read** - View all products with search and filters
- **Update** - Edit product details in modal window
- **Delete** - Remove products with confirmation dialog

**Warehouses:**
- **Create** - Add new warehouse locations
- **Read** - Browse all warehouses
- **Update** - Modify warehouse information
- **Delete** - Remove warehouses (with cascade delete warning)

All operations update the UI immediately upon completion.

### Data Persistence
Data is stored in a **local SQLite database** using Entity Framework Core:
- Database is automatically created on first run
- Database file: `warehouse.db` in application directory
- Migrations support for schema updates
- Asynchronous database operations for better performance

**Database Schema:**
- `Products` table - Product inventory
- `Warehouses` table - Warehouse locations
- Foreign key relationships with cascade rules

### Error Handling & User Feedback
Comprehensive error handling throughout the application:
- **Try-catch blocks** around all database operations
- **User-friendly messages** via MessageBox dialogs
- **Validation errors** displayed inline in forms
- **Confirmation dialogs** for destructive operations (delete)
- **Loading indicators** during async operations
- **Success notifications** after completed actions

Example error scenarios handled:
- Database connection failures
- Invalid input data
- Duplicate SKU entries
- Deletion of warehouses with products

![Destructive operation confirmation](docs.screenshots/error_handling.png)

### Dependency Injection
The application uses **Microsoft.Extensions.DependencyInjection**:
- Services registered in `App.xaml.cs` during startup
- ViewModels injected into Views
- Service layer injected into ViewModels
- DbContext managed by DI container

**Registered Services:**
```csharp
services.AddDbContext<WarehouseContext>();
services.AddSingleton<IWarehousesService, WarehousesService>();
services.AddTransient<ProductListViewModel>();
services.AddTransient<DashboardViewModel>();
// ... and more
```

---

## Key Features

### Dashboard
- **Statistics Cards** - Total warehouses, products, low stock items, critical capacity
- **Warehouse Utilization Chart** - Visual bars showing capacity usage
- **Product Distribution** - Pie chart of products per warehouse
- **Low Stock Alerts** - List of products below minimum quantity
- **Real-time Data** - Refresh button to update all statistics

![Dashboard](docs.screenshots/dashboard.png)

### Product Management
- Add products with name, SKU, quantity, price, dimensions, and minimum stock level
- Edit existing products in dedicated modal window
- Delete products with confirmation
- Search by name, SKU, or warehouse location
- Filter by warehouse
- View low stock indicators (red text for items below minimum)
- Automatic warehouse association

![Product Management](docs.screenshots/inventory.png)

### Warehouse Management
- Create warehouses with name, location, and capacity (m³)
- Edit warehouse details and capacity
- Delete warehouses (with warning if products exist)
- View current products count per warehouse
- See capacity utilization

![Warehouse Management](docs.screenshots/Warehouses.png)

---

## Technologies

- **.NET 8** - Latest .NET framework
- **WPF** - Windows Presentation Foundation for desktop UI
- **Entity Framework Core** - ORM for database access
- **SQLite** - Embedded lightweight database
- **CommunityToolkit.Mvvm** - MVVM helpers and source generators
- **Microsoft.Extensions.DependencyInjection** - Dependency injection container
- **LiveChartsCore** - Charts for dashboard visualization
- **Markdig.Wpf** - Markdown rendering for About page

---

## Getting Started

### Prerequisites

- **Operating System**: Windows 10/11
- **.NET SDK**: [Download .NET 8.0](https://dotnet.microsoft.com/download/dotnet/8.0) or later
- **IDE** (optional but recommended):
  - Visual Studio 2022 (Community, Professional, or Enterprise)

### Installation & Running

#### Option 1: Download Pre-built Release (Recommended for Users)

1. **Download the latest release**
   - Go to [Releases page](https://github.com/PawelG1/WarehouseManager/releases)
   - Download the latest release package (`.zip` file)

2. **Extract the archive**
   - Extract the downloaded `.zip` file to your desired location
   - Example: `C:\Program Files\WarehouseManager\`

3. **Run the application**
   - Navigate to the extracted folder
   - Double-click `WarehouseManagerApp.exe` to launch the application
   - The database will be created automatically on first run

> **Note**: No .NET SDK installation required for pre-built releases - the application is self-contained.

#### Option 2: Using Visual Studio

1. **Clone the repository**
   ```bash
   git clone https://github.com/PawelG1/WarehouseManager.git
   ```

2. **Open the solution**
   - Launch Visual Studio 2022
   - Open `WarehouseManagerApp.sln`

3. **Restore NuGet packages**
   - Visual Studio will automatically restore packages
   - Or manually: Right-click solution → Restore NuGet Packages

4. **Run the application**
   - Press `F5` or click the "Start" button
   - The database will be created automatically on first run

#### Option 3: Using Command Line

1. **Clone the repository**
   ```bash
   git clone https://github.com/PawelG1/WarehouseManager.git
   cd WarehouseManagerApp
   ```

2. **Restore dependencies**
   ```bash
   dotnet restore
   ```

3. **Build the application**
   ```bash
   dotnet build
   ```

4. **Run the application**
   ```bash
   dotnet run --project WarehouseManagerApp
   ```

The application will start, create the database file (`localDb.db`) in the application directory, and display the main window.

---

## Project Structure

```
WarehouseManagerApp/
├── WarehouseManagerApp/          # Main application project
│   ├── Data/                     # Database context and migrations
│   │   └── WarehouseContext.cs
│   ├── Models/                   # Data models (Product, Warehouse, etc.)
│   ├── Services/                 # Business logic layer
│   │   ├── IWarehousesService.cs
│   │   └── WarehousesService.cs
│   ├── ViewModels/               # ViewModels (MVVM pattern)
│   │   ├── DashboardViewModel.cs
│   │   ├── ProductListViewModel.cs
│   │   ├── AddProductViewModel.cs
│   │   └── ...
│   ├── Views/                    # XAML views
│   │   ├── Controls/             # User controls (Dashboard, ProductsList, etc.)
│   │   └── Screens/              # Windows (EditProductWindow)
│   ├── Converters/               # Value converters for XAML binding
│   ├── Resources/                # Images, styles, themes
│   ├── App.xaml[.cs]             # Application startup and DI setup
│   └── MainWindow.xaml[.cs]      # Main application window
├── WarehouseManagerApp.Tests/    # Unit tests project (xUnit)
├── README.md                      # This file
└── WarehouseManagerApp.sln       # Visual Studio solution
```

---

## Architecture

The application follows the **MVVM (Model-View-ViewModel)** pattern:

- **Models** - Data entities (Product, Warehouse)
- **Views** - XAML UI components
- **ViewModels** - Business logic, data binding, commands
- **Services** - Data access layer (IWarehousesService)

**Key Design Patterns:**
- Dependency Injection
- Repository Pattern (via Service layer)
- Command Pattern (RelayCommand from CommunityToolkit.Mvvm)
- Observer Pattern (INotifyPropertyChanged)

---

## Development

### Running Tests

```bash
# Run all tests
dotnet test

# Run with detailed output
dotnet test --logger "console;verbosity=detailed"

```

### Database Migrations

To create a new migration after model changes:

```bash
cd WarehouseManagerApp
dotnet ef migrations add MigrationName
```

To update the database:

```bash
dotnet ef database update
```

---

## Troubleshooting

### Database Issues

**Problem**: Database file not created  
**Solution**: Ensure the application has write permissions in its directory

**Problem**: "Database is locked" error  
**Solution**: Close all instances of the application and try again

### Application Crashes

**Problem**: Application crashes on startup  
**Solution**: 
- Verify .NET 8 SDK is installed: `dotnet --version`
- Run as administrator
- Check antivirus is not blocking the app

---

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

---

## Acknowledgments

Built with modern .NET technologies and best practices for WPF application development.

**Technologies & Libraries:**
- Microsoft .NET Team - .NET 8 and WPF framework
- Entity Framework Core Team - Database ORM
- CommunityToolkit.Mvvm - MVVM source generators
- LiveChartsCore - Data visualization
- Markdig - Markdown rendering
- SQLite - Embedded database

---

