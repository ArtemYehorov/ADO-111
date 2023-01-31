using Sales.Enities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
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
    /// Логика взаимодействия для DisconnectWindow.xaml
    /// </summary>
    public partial class DisconnectWindow : Window
    {
        public ObservableCollection<Enities.Department> Departments { get; set; }
        public ObservableCollection<Enities.Product> Products { get; set; }

        public ObservableCollection<Enities.Managers> Managers { get; set; }
        public DisconnectWindow()
        {
            InitializeComponent();
            //Связывание. Часть 1.  Контекст
            DataContext= this; //Представление получает доступ к всему объекту окна
            using SqlConnection connection = new(App.ConectionString);
            try
            {
                connection.Open();

                #region departments

                Departments = new();
                using SqlCommand cmd = new("SELECT Id, Name FROM Departments", connection);
                {
                    using var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Departments.Add(new() //изменения коллекции автоматически изменяет и изменения на ListView
                        {
                            Id = reader.GetGuid(0),
                            Name = reader.GetString(1),

                        });
                    }
                }
                #endregion

                #region Products
                Products = new();
                using SqlCommand cmd2 = new("SELECT Id, Name, Price FROM Products", connection);
                {
                    using var reader = cmd2.ExecuteReader();
                    while (reader.Read())
                    {
                        Products.Add(new() //изменения коллекции автоматически изменяет и изменения на ListView
                        {
                            Id = reader.GetGuid(0),
                            Name = reader.GetString(1),
                            Price = reader.GetDouble(2)
                        });
                    }
                }
                #endregion

                #region Managers
                Managers = new();
                using SqlCommand cmd3 = new("SELECT Id, Name, Surname, Secname FROM Managers", connection);
                {
                    using var reader = cmd3.ExecuteReader();
                    while (reader.Read())
                    {
                        Managers.Add(new() //изменения коллекции автоматически изменяет и изменения на ListView
                        {
                            Id = reader.GetGuid(0),
                            Name = reader.GetString(2),
                            Surname = reader.GetString(1),
                            Secname = reader.GetString(3)
                        });
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Close();
            }
            
        }

        private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            if (sender is ListViewItem item) // item = sender as ListViewItem
            {
                // Обратная связь (view->model) через item.Content 
                if (item.Content is Enities.Department department)
                {
                    MessageBox.Show(department.ToShortString());
                }
               
            }
           
        }

        private void ListViewItem_MouseDoubleClick_1(object sender, MouseButtonEventArgs e)
        {

            if (sender is ListViewItem item) // item = sender as ListViewItem
            {
                // Обратная связь (view->model) через item.Content 
                if (item.Content is Enities.Product product)
                {
                    MessageBox.Show(product.ToShortString());
                }

            }
        }

        private void ListViewItem_MouseDoubleClick_2(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListViewItem item) // item = sender as ListViewItem
            {
                // Обратная связь (view->model) через item.Content 
                if (item.Content is Enities.Managers Managers)
                {
                    MessageBox.Show(Managers.ToShortString());
                }

            }
        }
    }
}
