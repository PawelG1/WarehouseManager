# Warehouse Manager App

A modern desktop application for managing warehouse inventory built with WPF and .NET 8.

## Overview

Warehouse Manager App is a Windows desktop application that helps you track products across multiple warehouses. It provides an intuitive interface for adding, editing, viewing, and managing inventory with real-time filtering and search capabilities.

## Key Features

- **Product Management**: Add, edit, delete, and view products
- **Multi-Warehouse Support**: Manage inventory across multiple warehouse locations
- **Advanced Filtering**: Filter products by warehouse and search by name, SKU, or location
- **Real-Time Updates**: Automatic refresh of product lists after changes
- **Data Validation**: Built-in validation for product data integrity
- **SQLite Database**: Lightweight local database storage
- **MVVM Architecture**: Clean separation of concerns using Model-View-ViewModel pattern

## Technologies

- **.NET 8** - Latest .NET framework
- **WPF** - Windows Presentation Foundation for UI
- **Entity Framework Core** - Database access and ORM
- **SQLite** - Embedded database
- **CommunityToolkit.Mvvm** - MVVM helpers and source generators
- **Dependency Injection** - Microsoft.Extensions.DependencyInjection

## Quick Start

1. Clone the repository
2. Open the solution in Visual Studio 2022
3. Build and run the application
4. The database will be created automatically with sample data

## Documentation

Detailed documentation is available in the `docs` folder:

### Getting Started
- [Installation Guide](docs/01-installation.md) - How to set up and run the application
- [Quick Start](docs/02-quick-start.md) - Get started in 5 minutes
- [User Guide](docs/03-user-guide.md) - Complete guide to using the application

### Architecture
- [Architecture Overview](docs/04-architecture.md) - High-level system architecture
- [Project Structure](docs/05-project-structure.md) - Directory and file organization
- [MVVM Pattern](docs/06-mvvm-pattern.md) - How MVVM is implemented
- [Dependency Injection](docs/07-dependency-injection.md) - DI container configuration

### Data Layer
- [Database Schema](docs/08-database-schema.md) - Database tables and relationships
- [Data Models](docs/09-data-models.md) - Entity classes and validation rules
- [Entity Framework](docs/10-entity-framework.md) - EF Core configuration and usage

### Business Layer
- [Services](docs/11-services.md) - Business logic and data access
- [Caching Strategy](docs/12-caching-strategy.md) - Performance optimization

### Presentation Layer
- [ViewModels](docs/13-viewmodels.md) - ViewModel classes and commands
- [Views and Controls](docs/14-views-controls.md) - WPF windows and user controls
- [Data Binding](docs/15-data-binding.md) - XAML binding patterns

### Development
- [Development Setup](docs/16-development-setup.md) - Setting up development environment
- [Adding Features](docs/17-adding-features.md) - How to extend the application
- [Testing](docs/18-testing.md) - Testing strategies
- [Troubleshooting](docs/19-troubleshooting.md) - Common issues and solutions

## Requirements

- Windows 10 or later
- .NET 8 SDK
- Visual Studio 2022 (recommended) or Visual Studio Code

## License

This project is available for educational and personal use.

## Contributing

Contributions are welcome! Please read the development documentation before submitting pull requests.
