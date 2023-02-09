using Sales.Enities;
using System;
using System.Collections.Generic;
using System.Data;
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
    /// Логика взаимодействия для LINQWindow.xaml
    /// </summary>
    public partial class LINQWindow : Window
    {
        private LinqContext.DataContext context;

        public LINQWindow()
        {
            InitializeComponent();
            try
            {
                context = new(App.ConectionString);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Simple_Click(object sender, RoutedEventArgs e)
        {
            var query = context.Products.OrderBy(p => p.Price);
            TextBlock1.Text = "";
            foreach (var item in query)
            {
                TextBlock1.Text += item.Price + " " + item.Name + "\n";
            }
            TextBlock1.Text += "\n" + query.Count() + " Total";
        }

        private void Simple2_Click(object sender, RoutedEventArgs e)
        {
            var query = context.Products.OrderBy(p => p.Name);
            TextBlock1.Text = "";
            foreach (var item in query)
            {
                TextBlock1.Text += item.Price + " " + item.Name + "\n";
            }
            TextBlock1.Text += "\n" + query.Count() + " Total";
        }

        private void Simple3_Click(object sender, RoutedEventArgs e)
        {
            var query = context.Products.OrderByDescending(p => p.Price);
            TextBlock1.Text = "";
            foreach (var item in query)
            {
                TextBlock1.Text += item.Price + " " + item.Name + "\n";
            }
            TextBlock1.Text += "\n" + query.Count() + " Total";
        }

        private void Simple4_Click(object sender, RoutedEventArgs e)
        {
            var query = context.Products.Where(p => p.Price < 200).OrderBy(p => p.Price);
            TextBlock1.Text = "";
            foreach (var item in query)
            {
                TextBlock1.Text += item.Price + " " + item.Name + "\n";
            }
            TextBlock1.Text += "\n" + query.Count() + " Total";

        }

        private void Simple5_Click(object sender, RoutedEventArgs e)
        {

            var query = context.Products
            .Where(p => p.Name.Contains("ов"));
            //.OrderBy(p => p.Price);
            TextBlock1.Text = "";
            foreach (var item in query)
            {
                TextBlock1.Text += item.Price + " " + item.Name + "\n";
            }
            TextBlock1.Text += "\n" + query.Count() + " Total";

        }

        private void Simple6_Click(object sender, RoutedEventArgs e)
        {
            var query = context.Products
            .Where(p => p.Name.Contains("Г"));
            //.OrderBy(p => p.Price);
            TextBlock1.Text = "";
            foreach (var item in query)
            {
                TextBlock1.Text += item.Price + " " + item.Name + "\n";
            }
            TextBlock1.Text += "\n" + query.Count() + " Total";

        }

        private void Simple7_Click(object sender, RoutedEventArgs e)
        {
            var query2 = from m in context.Managers
                        join d in context.Departments on m.IdMainDep equals d.Id
                        select new
                        {
                            Managers = m.Surname + " " + m.Name,
                            Department = "| Отдел - " + d.Name
                        };
            var query = context.Managers.Join(   // метод обьеденения.
                context.Departments,             // коллекция с которой обьеденяеться 
                m => m.IdMainDep,                // функция извлечения внешнего ключа
                d => d.Id,                       // -- первичного ключа
                (m, d) => new { Managers = m.Surname + " " + m.Name, Department = d.Name }); // функция композиции из двуз обьектов с совпавшим ключами

            TextBlock1.Text = "";
            foreach (var item in query)
            {
                TextBlock1.Text += item.Managers + " " + item.Department + "\n";
            }
            TextBlock1.Text += "\n" + query.Count() + " Total";
        }

        private void Simple8_Click(object sender, RoutedEventArgs e)
        {
            var query2 = from m in context.Managers
                         join c in context.Managers on m.IdChief equals c.Id
                         select new
                         {
                             Managers = m.Surname + " " + m.Name,
                             Managers2 = "| Начальник - " + c.Surname + " " + c.Name
                         };
            var query = context.Managers.Join(   // метод обьеденения.
                context.Managers,             // коллекция с которой обьеденяеться 
                m => m.IdChief,                // функция извлечения внешнего ключа
                c => c.Id,                       // -- первичного ключа
                (m, c) => new { Managers = m.Surname + " " + m.Name, Managers2 = " Начальник - " + c.Surname + " " + c.Name }); // функция композиции из двуз обьектов с совпавшим ключами

            TextBlock1.Text = "";
            foreach (var item in query)
            {
                TextBlock1.Text += item.Managers + " " + item.Managers2 + "\n";
            }
            TextBlock1.Text += "\n" + query.Count() + " Total";
        }

        private void Simple9_Click(object sender, RoutedEventArgs e)
        {
            var query2 = from m in context.Managers
                         join d in context.Departments on m.IdMainDep equals d.Id
                         join c in context.Managers on m.IdChief equals c.Id
                         select new
                         {
                             Managers = m.Surname + " " + m.Name,
                             Managers2 = "| Начальник - " + c.Surname + " " + c.Name,
                             Department = "| Отдел - " + d.Name
                         };
       
            TextBlock1.Text = "";
            foreach (var item in query2)
            {
                TextBlock1.Text += item.Managers + " " + item.Department + " " + item.Managers2 + "\n";
            }
            TextBlock1.Text += "\n" + query2.Count() + " Total";
        }
    }
}
