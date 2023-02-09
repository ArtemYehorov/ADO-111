using Sales.Enities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
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
            DataContext = this; //Представление получает доступ к всему объекту окна
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
                using SqlCommand cmd3 = new("SELECT Id, Name, Surname, Secname, Id_main_dep, Id_sec_dep, Id_chief FROM Managers", connection);
                {
                    using var reader = cmd3.ExecuteReader();
                    while (reader.Read())
                    {
                        Managers.Add(new() //изменения коллекции автоматически изменяет и изменения на ListView
                        {
                            Id = reader.GetGuid(0),
                            Surname = reader.GetString(1),
                            Name = reader.GetString(2),
                            Secname = reader.GetString(3),
                            IdMainDep= reader.GetGuid(4),
                            IdSecDep= reader[5] == DBNull.Value  //  в БД любой элемер  может быть NULL
                            ? null                               //  но в C# значимые типы не могут быть NULL
                            : reader.GetGuid(5),                 //  Для передачи значимых типов, но опциональных сначала запрашивают object
                            IdChief= reader[6] == DBNull.Value   //  Его проверяют на DBNull.Value и если это не так, то
                            ? null                               //  проверяют запрос со значимым Get-тером (GetGuid) - структура
                            : reader.GetGuid(6),                 
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
                if (item.Content is Department department)
                {
                    CRUD.CrudDepartmentWindow window = new()
                    {
                        Department = department
                    };
                    int index = Departments.IndexOf(department);
                    Departments.Remove(department); // удалеям из колекции и передаём на редактирование
                    if (window.ShowDialog().GetValueOrDefault())
                    {
                        using SqlConnection connection = new(App.ConectionString);
                        try
                        {
                            connection.Open();
                            using SqlCommand cmd = new() { Connection = connection };
                            if (window.Department is null) //удаление
                            {
                                 cmd.CommandText = $"DELETE FROM Departments WHERE Id = '{department.Id}' ";
                                cmd.ExecuteNonQuery();
                                MessageBox.Show("Данные удалены!");
                            }
                            else //возвращаем но изменённый.
                            {
                                cmd.CommandText = $"UPDATE Departments SET Name = @name WHERE Id = '{department.Id}'";
                                cmd.Parameters.AddWithValue("@name", department.Name);
                                Departments.Insert(index, department);
                            }
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Задание выполнено успешно!");
                        }
                        catch(Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }

                    }
                    else//отмена - возвращение в окно
                    {
                        Departments.Insert(index,department);
                    }
                    //MessageBox.Show(department.ToShortString());
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

        private void AddDepartment_Click(object sender, RoutedEventArgs e)
        {
            var window = new CRUD.CrudDepartmentWindow();

            if (window.ShowDialog().GetValueOrDefault())
            {
                MessageBox.Show(window.Department.ToShortString());
                using SqlConnection connection = new(App.ConectionString);
                try
                {
                    connection.Open();
                    using SqlCommand cmd = new(
                        $"INSERT INTO Departments ( Id, Name ) VALUES (@id, @name)", 
                        connection);
                    cmd.Parameters.AddWithValue("@id", window.Department.Id);
                    cmd.Parameters.AddWithValue("@name", window.Department.Name);
                    cmd.ExecuteNonQuery();

                    Departments.Add(window.Department);
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("NoOK");
            }
            
        }

        private void AddManager_Click(object sender, RoutedEventArgs e)
        {
            CRUD.CrudManagerWindow managerWindow = new() { Owner = this };
            if (managerWindow.ShowDialog().GetValueOrDefault())
            {
                MessageBox.Show(managerWindow.manager.ToShortString());
                using SqlConnection connection = new(App.ConectionString);
                try
                {
                    connection.Open();
                    using SqlCommand cmd = new(
                        $"INSERT INTO Managers ( Id, Surname, Name, Secname, IdMainDep, IdSecDep, IdChief ) VALUES (@id, @surname, @name, @secname, @Id_main_dep, @Id_sec_dep, @Id_chief)",
                        connection);
                    cmd.Parameters.AddWithValue("@id", managerWindow.manager.Id);
                    cmd.Parameters.AddWithValue("@surname", managerWindow.manager.Surname);
                    cmd.Parameters.AddWithValue("@name", managerWindow.manager.Name);
                    cmd.Parameters.AddWithValue("@secname", managerWindow.manager.Secname);
                    cmd.Parameters.AddWithValue("@Id_main_dep", managerWindow.manager.IdMainDep);
                    cmd.Parameters.AddWithValue("@Id_sec_dep", managerWindow.manager.IdSecDep);
                    cmd.Parameters.AddWithValue("@Id_chief", managerWindow.manager.IdChief);
                    cmd.ExecuteNonQuery();

                    Managers.Add(managerWindow.manager);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("NoOK");
            }
        }

        private void ListViewItem_MouseDoubleClick_3(object sender, MouseButtonEventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var man = Managers[3];
            var dep1 = Departments.FirstOrDefault(d => d.Id == man.IdMainDep);
            var dep2 = Departments.FirstOrDefault(d => d.Id == man.IdSecDep);

            TextBlock1.Text += man.Name + " " + dep1.Name + " " + (dep2?.Name ?? "__") + "\n";
        }
    }
}
