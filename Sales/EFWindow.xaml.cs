using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Sales
{
    /// <summary>
    /// Логика взаимодействия для EFWindow.xaml
    /// </summary>
    public partial class EFWindow : Window
    {
        public EfContext.DataContext dataContext;

        public EFWindow()
        {
            InitializeComponent();
            dataContext = new();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            MonitorDepartments.Content = dataContext.Departments.Count().ToString();
            MonitorProducts.Content= dataContext.Products.Count().ToString();
            MonitorManagers.Content= dataContext.Managers.Count().ToString();
            MonitorSales.Content = dataContext.Sales.Count().ToString();

        }

        private void ButtonSalesAdd_Click(object sender, RoutedEventArgs e)
        {
            dataContext.Sales.Add(new EFContext.Sale { Id = Guid.NewGuid(), IdManager = Guid.NewGuid(), IdProduct = Guid.NewGuid(), Count = new Random().Next(10), Moment = DateTime.Now});
            dataContext.Sales.Add(new EFContext.Sale { Id = Guid.NewGuid(), IdManager = Guid.NewGuid(), IdProduct = Guid.NewGuid(), Count = new Random().Next(10), Moment = DateTime.Now });
            dataContext.Sales.Add(new EFContext.Sale { Id = Guid.NewGuid(), IdManager = Guid.NewGuid(), IdProduct = Guid.NewGuid(), Count = new Random().Next(10), Moment = DateTime.Now });
            dataContext.Sales.Add(new EFContext.Sale { Id = Guid.NewGuid(), IdManager = Guid.NewGuid(), IdProduct = Guid.NewGuid(), Count = new Random().Next(10), Moment = DateTime.Now });
            dataContext.Sales.Add(new EFContext.Sale { Id = Guid.NewGuid(), IdManager = Guid.NewGuid(), IdProduct = Guid.NewGuid(), Count = new Random().Next(10), Moment = DateTime.Now });
            dataContext.Sales.Add(new EFContext.Sale { Id = Guid.NewGuid(), IdManager = Guid.NewGuid(), IdProduct = Guid.NewGuid(), Count = new Random().Next(10), Moment = DateTime.Now });
            dataContext.Sales.Add(new EFContext.Sale { Id = Guid.NewGuid(), IdManager = Guid.NewGuid(), IdProduct = Guid.NewGuid(), Count = new Random().Next(10), Moment = DateTime.Now });
            dataContext.Sales.Add(new EFContext.Sale { Id = Guid.NewGuid(), IdManager = Guid.NewGuid(), IdProduct = Guid.NewGuid(), Count = new Random().Next(10), Moment = DateTime.Now });
            dataContext.Sales.Add(new EFContext.Sale { Id = Guid.NewGuid(), IdManager = Guid.NewGuid(), IdProduct = Guid.NewGuid(), Count = new Random().Next(10), Moment = DateTime.Now });
            dataContext.Sales.Add(new EFContext.Sale { Id = Guid.NewGuid(), IdManager = Guid.NewGuid(), IdProduct = Guid.NewGuid(), Count = new Random().Next(10), Moment = DateTime.Now });
            dataContext.SaveChanges();
            MonitorSales.Content = dataContext.Sales.Count().ToString();
        }
    }
}
