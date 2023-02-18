using Sales.EFContext;
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
            ShowDailyStatistics();

        }

        private void ShowDailyStatistics()
        {
            StatTotalSales.Content = dataContext.Sales.Where(sale => sale.Moment.Date == DateTime.Now.Date).Count();
            StatTotalProducts.Content = dataContext.Sales.Where(sale => sale.Moment.Date == DateTime.Now.Date).Sum(sale => sale.Count);

            StatTotalMoney.Content = dataContext.Sales
                .Where(sale => sale.Moment.Date == DateTime.Now.Date)
                .Join(dataContext.Products,
                sale => sale.IdProduct,
                prod => prod.Id,
                (Sale, prod) => Sale.Count * prod.Price).Sum().ToString("0.00");

            //лучший сотрудник
            StatTopManager.Content = dataContext.Managers.GroupJoin(
                dataContext.Sales.Where(sale => sale.Moment.Date == DateTime.Now.Date),
                man => man.Id,
                sale => sale.IdManager,
                (man, sales) => new { Manager = man, Total = sales.Sum(s => s.Count) })
                .OrderByDescending(mix => mix.Total)
                .Take(1)
                .Select(mix => mix.Manager.ToShortString() + $" ({mix.Total})")
                .First();

            // лучший департамент

            StatTopDepart.Content = dataContext.Departments.GroupJoin(
               dataContext.Sales.Where(sale => sale.Moment.Date == DateTime.Now.Date).Join(dataContext.Managers,
               s => s.IdManager,
               m => m.Id,
               (s,m) => new {Sale = s, Manager = m}),
               d => d.Id,
               m => m.Manager.IdMainDep,
               (d, mix) => new { Department = d, Total = mix.FirstOrDefault().Sale})
               .OrderByDescending(mix => mix.Total)
               .Take(1)
               .Select(mix => mix.Department.Name)
               .First();


            //дучший товар
            StatTopProduct.Content = dataContext.Products.GroupJoin(
               dataContext.Sales.Where(sale => sale.Moment.Date == DateTime.Now.Date),
               p => p.Id,
               sale => sale.IdProduct,
               (p, sales) => new { Product = p, Total = p.Price * sales.Sum(s => s.Count) })
               .OrderByDescending(mix => mix.Total)
               .Take(1)
               .Select(mix => mix.Product.ToShortString() + $" ({mix.Total})")
               .First();

        }

        private void ButtonSalesAdd_Click(object sender, RoutedEventArgs e)
        {
            int managersCount = dataContext.Managers.Count();
            int productsCount = dataContext.Products.Count();
            for (int i = 0; i < 10; i++)
            {
                dataContext.Sales.Add(new EFContext.Sale { Id = Guid.NewGuid(),
                    IdManager = dataContext.Managers.Skip(App.rand.Next(managersCount)).First().Id,
                    IdProduct = dataContext.Products.Skip(App.rand.Next(productsCount)).First().Id,
                    Count = App.rand.Next(10 + 1),
                    Moment = DateTime.Now });
            }
            dataContext.SaveChanges();
            MonitorSales.Content = dataContext.Sales.Count().ToString();
            ShowDailyStatistics();
        }
    }
}
