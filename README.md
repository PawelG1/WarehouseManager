# Warehouse Manager App

A modern desktop application for managing warehouse inventory built with WPF and .NET 8.

## Overview

Warehouse Manager App is a Windows desktop application that helps you track products across multiple warehouses. It provides an intuitive interface for adding, editing, viewing, and managing inventory with real-time filtering and search capabilities.

## Key Features

### Product Management
- **Add Products**: Create new products with name, SKU, quantity, and minimum stock levels
- **Edit Products**: Modify existing product details
- **Delete Products**: Remove products from inventory
- **View Products**: Browse all products in a searchable, filterable list
- **Low Stock Alerts**: Visual indicators for products below minimum quantity

### Warehouse Management
- **Add Warehouses**: Create new warehouse locations with name, location, and capacity
- **Edit Warehouses**: Update warehouse details including capacity
- **Delete Warehouses**: Remove warehouses (with confirmation and cascade delete of products)
- **View Warehouses**: Browse all warehouses in an organized ListView
- **Warehouse Details**: View capacity and number of products per warehouse

### Advanced Features
- **Multi-Warehouse Support**: Manage inventory across multiple warehouse locations
- **Advanced Filtering**: Filter products by warehouse and search by name, SKU, or location
- **Real-Time Updates**: Automatic refresh of lists after changes
- **Data Validation**: Built-in validation for data integrity
- **Overlay Forms**: Modal dialogs for adding and editing with modern UX
- **Confirmation Dialogs**: Safe deletion with confirmation prompts

## Technologies

- **.NET 8** - Latest .NET framework
- **WPF** - Windows Presentation Foundation for UI
- **Entity Framework Core** - Database access and ORM
- **SQLite** - Embedded database
- **CommunityToolkit.Mvvm** - MVVM helpers and source generators
- **Dependency Injection** - Microsoft.Extensions.DependencyInjection
- **Markdig.Wpf** - Markdown rendering for documentation

## Application Structure

### Main Features

#### 1. Dashboard (Coming Soon)
- Overview of inventory statistics
- Quick access to key metrics

#### 2. Inventory Management
- Complete product CRUD operations
- Search and filter by warehouse
- Visual stock level indicators
- Add/Edit product forms with validation

#### 3. Warehouse Management
- Complete warehouse CRUD operations
- ListView with sortable columns
- Capacity and location tracking
- Cascade delete handling

#### 4. About
- Application information
- README.md viewer with Markdown support
- Version information

## Quick Start

1. Clone the repository
   ```bash
   git clone https://github.com/PawelG1/WarehouseManager.git
   cd WarehouseManagerApp
   ```

2. Open the solution in Visual Studio 2022
   ```
   WarehouseManagerApp.sln
   ```

3. Build and run the application (F5)

4. The database will be created automatically with sample data:
   - 2 sample warehouses (Main Warehouse, Secondary Warehouse)
   - 3 sample products

## User Interface

### Navigation
- **Dashboard** - Overview and statistics (Coming Soon)
- **Inventory** - Manage products
- **Warehouses** - Manage warehouse locations
- **About** - Application information

### Product Management Flow
1. Navigate to **Inventory**
2. Filter by warehouse or search by text
3. Click **Add Product** to create new product
4. Click **Edit** on any product to modify
5. Click **Delete** to remove product (with confirmation)

### Warehouse Management Flow
1. Navigate to **Warehouses**
2. View all warehouses in organized list
3. Click **? Add New Warehouse** to create
4. Click **Edit** (green button) to modify warehouse
5. Click **Delete** (red button) to remove (with confirmation)

## Architecture Highlights

### MVVM Pattern
The application follows the Model-View-ViewModel pattern for clean separation of concerns:

**Models**: Data entities
- `Product` - Product entity with validation
- `Warehouse` - Warehouse entity with capacity tracking

**Views**: XAML UI components
- `MainWindow` - Main application window
- `ProductsList` - Product list view
- `WarehousesList` - Warehouse list view
- `AddProductControl` - Add product form
- `EditProductControl` - Edit product form
- `AddWarehouseControl` - Add warehouse form
- `EditWarehouseControl` - Edit warehouse form
- `AboutControl` - About page with README viewer

**ViewModels**: Business logic and commands
- `MainWindowViewModel` - Main window coordination and navigation
- `ProductListViewModel` - Product list management
- `AddProductViewModel` - Add product logic
- `EditProductViewModel` - Edit product logic
- `WarehouseListViewModel` - Warehouse list management
- `AddWarehouseViewModel` - Add warehouse logic
- `EditWarehouseViewModel` - Edit warehouse logic
- `AboutViewModel` - About page logic

### Services Layer
The service layer handles all business logic and database operations:

**IWarehousesService / WarehousesService**
- Product CRUD operations (Add, Update, Delete, Get)
- Warehouse CRUD operations (Add, Update, Delete, Get)
- Caching for performance optimization
- Async/await for non-blocking database operations
- Automatic cache invalidation on data changes

### Data Layer
- **Entity Framework Core** with SQLite for data persistence
- **Code-First** approach with migrations
- **Automatic database initialization** with sample data
- **Cascade delete** handling for warehouse-product relationships
- **Data validation** at the model level

### Dependency Injection
All services and ViewModels are registered in the DI container for:
- Loose coupling between components
- Easy testing and mocking
- Centralized configuration
- Lifetime management (Singleton for services, Transient for ViewModels)

## Database Schema

### Tables

**Warehouses**
- Id (Primary Key)
- Name (Required, 3-200 characters)
- Location (Required, 5-500 characters)
- CapacityM3 (Required, > 0)

**Products**
- Id (Primary Key)
- Name (Required, 3-200 characters)
- SKU (Required, unique, 3-50 characters)
- Quantity (Required, ? 0)
- MinimumQuantity (Required, ? 0)
- WarehouseId (Foreign Key ? Warehouses.Id)

### Relationships
- One Warehouse can have many Products (1:N)
- Cascade delete: Deleting a Warehouse deletes all its Products

## Requirements

- **Operating System**: Windows 10 or later
- **.NET SDK**: .NET 8.0 or later
- **IDE**: Visual Studio 2022 (recommended) or Visual Studio Code
- **Disk Space**: 100 MB minimum
- **RAM**: 2 GB minimum (4 GB recommended)

## Project Status

### Completed Features ?
- ? Product Management (Add, Edit, Delete, View)
- ? Warehouse Management (Add, Edit, Delete, View)
- ? Multi-warehouse support
- ? Search and filtering
- ? Data validation
- ? SQLite database with EF Core
- ? MVVM architecture
- ? Dependency Injection
- ? About page with README viewer
- ? Overlay forms for Add/Edit operations
- ? Confirmation dialogs for destructive operations

### In Progress ??
- ?? Dashboard with statistics
- ?? Configuration page
- ?? Export/Import functionality

### Planned Features ??
- ?? Reporting system
- ?? User authentication
- ?? Audit logging
- ?? Barcode scanning
- ?? Print labels
- ?? Advanced search with filters
- ?? Bulk operations
- ?? Product categories

## Development

### Building from Source

```bash
# Clone the repository
git clone https://github.com/PawelG1/WarehouseManager.git
cd WarehouseManagerApp

# Restore dependencies
dotnet restore

# Build the solution
dotnet build

# Run the application
dotnet run --project WarehouseManagerApp
```

### Running Tests

```bash
# Run all tests
dotnet test

# Run with coverage
dotnet test --collect:"XPlat Code Coverage"
```

### Project Structure

```
WarehouseManagerApp/
??? WarehouseManagerApp/          # Main application project
?   ??? Data/                     # Database context
?   ??? Models/                   # Data models
?   ??? Services/                 # Business logic layer
?   ??? ViewModels/               # ViewModels for MVVM
?   ??? Views/                    # XAML views and controls
?   ?   ??? Controls/            # Reusable user controls
?   ??? Resources/               # Images, styles, etc.
?   ??? App.xaml[.cs]           # Application startup
?   ??? MainWindow.xaml[.cs]    # Main window
??? WarehouseManagerApp.Tests/   # Unit tests project
??? README.md                     # This file
??? WarehouseManagerApp.sln      # Solution file
```

## Troubleshooting

### Common Issues

**Database not created**
- Make sure the application has write permissions in its directory
- The database file will be created in the application's base directory

**Application crashes on startup**
- Ensure .NET 8 SDK is installed
- Check if any antivirus is blocking the application
- Try running as administrator

**Changes not saving**
- Check if the database file is not read-only
- Verify that the application has write permissions

**UI not responsive**
- All database operations are async, but heavy operations might take time
- Check the loading indicators

## License

This project is available for educational and personal use.

## Contributing

Contributions are welcome! To contribute:

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

### Contribution Guidelines
- Follow MVVM pattern
- Write unit tests for new features
- Update README.md if needed
- Use meaningful commit messages
- Keep code clean and well-documented

## Support

For issues and questions:
- **GitHub Issues**: [Create an issue](https://github.com/PawelG1/WarehouseManager/issues)
- **Email**: Contact the development team
- **Documentation**: Check this README for common solutions

## Acknowledgments

Built with modern .NET technologies and following best practices for WPF application development.

### Technologies & Libraries
- Microsoft .NET Team - for .NET 8 and WPF
- Entity Framework Team - for EF Core
- CommunityToolkit.Mvvm - for MVVM helpers
- Markdig - for Markdown rendering

---

**Made with ?? using .NET 8 and WPF**
