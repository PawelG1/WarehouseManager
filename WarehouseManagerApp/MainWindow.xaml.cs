using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WarehouseManagerApp.Data;
using WarehouseManagerApp.Models;

namespace WarehouseManagerApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            initializeDb();
        }

        private void initializeDb()
        {
            using (var context = new WarehouseContext())
            {
                //ensure db exists
                context.Database.EnsureCreated();

                //if db is empty adds example data
                if (!context.Warehouses.Any())
                {
                    var warehouse1 = new Warehouse
                    {
                        Name = "Main Warehouse",
                        Location = "Niepolomice, ul. Przemyslowa 1",
                        CapacityM3 = 2000,
                    };

                    var warehouse2 = new Warehouse
                    {
                        Name = "Secondary Warehouse",
                        Location = "Myslenice, ul. Przemyslowa 2",
                        CapacityM3 = 500,
                    };

                    context.Warehouses.AddRange(warehouse1, warehouse2);
                    context.SaveChanges();

                    //add example products 
                    var products = new[]
                    {
                        new Product{
                            Name = "Laptop Dell",
                            SKU = "DELL-XPS15",
                            WarehouseId = warehouse1.Id,
                            Quantity = 15,
                            minimumQuantity = 5,
                        },
                        new Product{
                            Name = "Mysz Logitech",
                            SKU = "LOG-MX3",
                            WarehouseId = warehouse1.Id,
                            Quantity = 10,
                            minimumQuantity = 15,
                        },
                        new Product{
                            Name = "Klawiatura Dell",
                            SKU = "KB212-B",
                            WarehouseId = warehouse2.Id,
                            Quantity = 7,
                            minimumQuantity = 5,
                        }
                    };

                    context.Products.AddRange(products);
                    context.SaveChanges();

                    MessageBox.Show("DB has been initialized with example data",
                        "Info",
                        MessageBoxButton.OK, 
                        MessageBoxImage.Information);

                    
                }
            }

        }
        private void ShowProducts()//shows products in dataGird
        {
            using (var context = new WarehouseContext())
            {
                var products = context.Products
                    .Include(p => p.Warehouse)
                    .OrderByDescending(p => p.Quantity)
                    .ToList();

                dgDane.ItemsSource = products;
                txtNaglowek.Text = "Products list";

                RefreshStatistics();
            }
        }

        private void ShowWarehouses() //shows warehouses in dataGird
        {
            using (var context = new WarehouseContext())
            {
                var warehouses = context.Warehouses
                    .Include(w => w.Products)
                    .ToList();

                dgDane.ItemsSource = warehouses;
                txtNaglowek.Text = "Warehouses List";

                RefreshStatistics();
            }
        }

        private void RefreshStatistics()
        {
            using (var context = new WarehouseContext())
            {
                var ProductsCount = context.Products.Count();
                var WarehousesCount = context.Warehouses.Count();
                
                txtLiczbaProdukty.Text = ProductsCount.ToString();
                txtLiczbaMagazyny.Text = WarehousesCount.ToString();
                txtWartosc.Text = "999";
            }
        }

        //buttons onClick event Handling
        private void BtnProducts_Click(object sender, RoutedEventArgs e)
        {
            ShowProducts();
        }

        private void BtnWarehouses_Click(object sender, RoutedEventArgs e)
        {
            ShowWarehouses();
        }

        private void BtnAddWarehouse_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("To be implemented");
        }

        private void BtnAddProduct_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("To be implemented");
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            ShowProducts();
            MessageBox.Show("Data refreshed", "info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BtnRemove_Click(object sender, RoutedEventArgs e)
        {
            if(dgDane.SelectedItem == null)
            {
                MessageBox.Show(
                    "Selected an item, to remove it",
                    "Warning",
                    MessageBoxButton.OK,
                    MessageBoxImage.Hand
                    );
                return;
            }

            var result = MessageBox.Show("Are You sure, You want to remove selected item?",
                "Confirmation",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
                );

            if (result == MessageBoxResult.Yes)
            {
                using (var context = new WarehouseContext())
                {
                    if (dgDane.SelectedItem is Product product)
                    {
                        context.Products.Attach(product);
                        context.Products.Remove(product);
                        context.SaveChanges();
                        ShowProducts();
                        MessageBox.Show("Product removed!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else if (dgDane.SelectedItem is Warehouse warehouse)
                    {
                        if (warehouse.Products?.Count > 0)
                        {
                            MessageBox.Show("Cannot remove warehouse that contains products",
                                "Error",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);

                        }
                        else
                        {
                            context.Warehouses.Attach(warehouse);
                            context.Warehouses.Remove(warehouse);
                            context.SaveChanges();
                            ShowWarehouses();
                            MessageBox.Show(
                                "Warehouse deleted",
                                "Success",
                                MessageBoxButton.OK,
                                MessageBoxImage.Information
                                );
                        }
                    }
                }

            }   
        }

        private void btnDodajMagazyn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnUsun_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}