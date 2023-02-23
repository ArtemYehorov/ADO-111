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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlTypes;

using System.Data.SqlClient;// подключаем ADO для MS SQL Server (не забыть NuGet)
using System.Text.RegularExpressions;
using System.Data;
using Sales.Enities;

namespace Sales
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SqlConnection _connection;
        private List<Enities.Department>? _departments; //ORM: колекция обьектов - сущностей == таблица
        private List<Enities.Product>? _products;
        private List<Enities.Managers>? _managers;
        private List<Enities.TodaySales>? _todaysales;
        public MainWindow()
        {
            InitializeComponent();
            // Строчка подключение берёться из свойст бд(Server Explorer)
            // создание обьекта-подключения !! не открывает подключение 
            _connection = new(App.ConectionString);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                _connection.Open(); // открытие подключения 
                MonitorConnection.Content = "Установлено";
                MonitorConnection.Foreground = Brushes.Green;
            }
            catch (SqlException ex)
            {
                MonitorConnection.Content = "Закрыто";
                MonitorConnection.Foreground = Brushes.Red;
                MessageBox.Show(ex.Message);
                this.Close();
            }
            ShowDepartmentsCount();
            ShowProductsCount();
            ShowManagersCount();
            ShowSalesCount();

            ShowDailyStatistics();

            ShowDepartmentsORM();

            ShowProductsORM();

            ShowManagersORM();

            ShowTodaySalesORM();
        }

        #region ShowMonitor
        /// <summary>
        /// Выводит в таблицу-монитор количество отделов (департаментов) из БД
        /// </summary>
        private void ShowDepartmentsCount()
        {
            String sql = "SELECT COUNT(*) FROM Departments";
            // SqlCommand обьект для передачи команд (запросов) к БД.
            // Требует закрытия, по этому using.
            using var cmd = new SqlCommand(sql, _connection);
            //создание обьекта не исполняет команду, для этого есть методы ExecuteXxxx.
            MonitorDepartments.Content = cmd.ExecuteScalar().ToString();// выполняет комануд и возвращает "верхний-левый" результат
        }

        /// <summary>
        /// Выводит в таблицу-монитор количество продуктов (департаментов) из БД
        /// </summary>
        private void ShowProductsCount()
        {
            String sql = "SELECT COUNT(*) FROM Products";
            // SqlCommand обьект для передачи команд (запросов) к БД.
            // Требует закрытия, по этому using.
            using var cmd = new SqlCommand(sql, _connection);
            //создание обьекта не исполняет команду, для этого есть методы ExecuteXxxx.
            MonitorProducts.Content = cmd.ExecuteScalar().ToString();// выполняет комануд и возвращает "верхний-левый" результат
        }

        /// <summary>
        /// Выводит в таблицу-монитор количество менеджеров (департаментов) из БД
        /// </summary>
        private void ShowManagersCount()
        {
            String sql = "SELECT COUNT(*) FROM Managers";
            // SqlCommand обьект для передачи команд (запросов) к БД.
            // Требует закрытия, по этому using.
            using var cmd = new SqlCommand(sql, _connection);
            //создание обьекта не исполняет команду, для этого есть методы ExecuteXxxx.
            MonitorManagers.Content = cmd.ExecuteScalar().ToString();// выполняет комануд и возвращает "верхний-левый" результат
        }

        /// <summary>
        /// Выводит в таблицу-монитор количество продаж (департаментов) из БД
        /// </summary>
        private void ShowSalesCount()
        {
            String sql = "SELECT COUNT(*) FROM Sales";
            // SqlCommand обьект для передачи команд (запросов) к БД.
            // Требует закрытия, по этому using.
            using var cmd = new SqlCommand(sql, _connection);
            //создание обьекта не исполняет команду, для этого есть методы ExecuteXxxx.
            MonitorSales.Content = cmd.ExecuteScalar().ToString();// выполняет комануд и возвращает "верхний-левый" результат
        }

        #endregion

        /// <summary>
        /// Заполняет блок "Статистика за день"
        /// </summary>
        private void ShowDailyStatistics()
        {
            SqlCommand cmd = new()
            {
                Connection = _connection
            };
            try
            {

           
                 // в БД информация за 2022 год, по этому формируем дату с тикущим днём и месяцем, но за 2022 год
                 String date = $"2022-{DateTime.Now.Month}-{DateTime.Now.Day}";
                 
                 //всегт продаж(чеков)
                 cmd.CommandText = $"SELECT COUNT(*) FROM Sales s WHERE CAST(s.Moment AS DATE) = '{date}'";
                 StatTotalSales.Content = cmd.ExecuteScalar().ToString();
                 
                 //всегт продаж(товаров, штук)
                 cmd.CommandText = $"SELECT SUM(s.Cnt) FROM Sales s WHERE CAST(s.Moment AS DATE) = '{date}'";
                 StatTotalProducts.Content = cmd.ExecuteScalar().ToString();
                 
                 //всегт продаж(деньги, грн)
                 cmd.CommandText = $"SELECT ROUND ( SUM(s.Cnt * p.Price ),3) FROM Sales s JOIN Products p ON s.ID_product = p.Id WHERE CAST(s.Moment AS DATE) = '{date}'";
                 StatTotalMoney.Content = cmd.ExecuteScalar().ToString();
                 
                 cmd.CommandText = $"SELECT TOP 1 m.Surname + ' ' + m.Name " +
                     $"FROM Sales s JOIN Managers m  ON s.ID_manager = m.Id " +
                     $"JOIN Products p " +
                     $"ON s.ID_product = p.Id WHERE CAST(s.Moment AS DATE) = '{date}' " +
                     $"GROUP BY  m.Surname + ' ' + m.Name\r\nORDER BY SUM(s.Cnt * p.Price) DESC\r\n ";
                 StatTopManager.Content = cmd.ExecuteScalar().ToString();
                 
                 cmd.CommandText = $"SELECT TOP 1 d.Name FROM Sales s JOIN Managers m ON s.ID_manager = m.Id JOIN Products p ON s.ID_product = p.Id JOIN Departments d ON m.Id_main_dep = d.Id WHERE CAST(s.Moment AS DATE) = '{date}' GROUP BY d.Name ORDER BY SUM(s.Cnt) DESC ";
                 StatTopDepart.Content = cmd.ExecuteScalar().ToString();
                 
                 cmd.CommandText = $"SELECT TOP 1 p.Name FROMe Sales s JOIN  Products p ON s.ID_product = p.Id WHERE CAST(s.Moment AS DATE) = '{date}' GROUP BY p.Name ORDER BY SUM(s.Cnt) DESC";
                 StatTopProduct.Content = cmd.ExecuteScalar().ToString();
                 
                 
            }
            catch (Exception ex)
            {

                App.Logger.Log(ex.Message, Logging.LogLevel.Error, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod()?.Name ?? "", cmd.CommandText);
                StatTopDepart.Content = "--";
                StatTopProduct.Content = "--";
                StatTopManager.Content = "--";
                StatTotalMoney.Content = "--";
                StatTotalProducts.Content = "--";
                StatTotalSales.Content = "--";
            }
            cmd.Dispose();
        }
        /// <summary>
        /// Заполняет блок "Отделы" - выборка всех данных из таблицы Deparments
        /// </summary>
       private void ShowDepartments()
       {
            using SqlCommand cmd = new("SELECT * FROM Departments", _connection);   //Табличный запрос - возвращает SqlDataReader
            SqlDataReader reader = cmd.ExecuteReader();                             //Передача данных происходит по строчно - по одной строке в выборке(рещультата)
            DepartmentCell.Text = "";                                               // Вызов ExecuteReader не читает данные, только создаёт reader
            while (reader.Read())                                                   // Команда Reader.Read() заполняет reader данными ( строкой - выборкой) - "Самозаполняется"
            {                                                                       //!! Возврат Read() - статус (успех\конец)
                Guid id = reader.GetGuid("id");                                     // После чтения данные доступны 
                String name = (String)reader[1];                                    // а) через Get-теры (GetGuid\GetString...)
                                                                                    // б) через Get-теры с именем поля (using System.Data)
                DepartmentCell.Text += $"{id} {name} \n";                           // в) через индексатор [] с числом - индексом поля
                                                                                    // г) через индексатор [] c строкой - название поля
                // Значение индекса начинаеться с 0 и соответствует порядку данных в строке-результате (в таблице)                                                 
                //Поскольку обращение к данным идет по индексам, крайне НЕ рекомендуется
                //Оформлять запрос как SELECT * -  в нем не видно порядок полей
                //Лучше перечислять поля SELECT id, name FROM Departments
                // readre обязательно нужно закрывать, если останеться открытым, то не будут выполняться другие команды
            }
            reader.Dispose();
       }

       
        private void ShowDepartmentsORM()
        {
            if (_departments is null) // 1) обращение - заполняем коллекцию 
            {
                using SqlCommand cmd = new("SELECT D.Id, D.Name FROM Departments D", _connection);
                try
                {
                    using SqlDataReader reader = cmd.ExecuteReader();
                    _departments = new();
                    while (reader.Read())
                    {
                        _departments.Add(new()
                        {
                            Id = reader.GetGuid("id"),
                            Name = reader.GetString(1)
                        });
                    }
                }
                catch (SqlException ex)
                {
                    App.Logger.Log(ex.Message, Logging.LogLevel.Error, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod()?.Name ?? "", cmd.CommandText);
                }
             
            }
            DepartmentCell.Text = "";
            foreach (var department in _departments)
            {
                DepartmentCell.Text += department.ToShortString() + "\n";
            }
        }

        private void ShowProductsORM()
        {
            if (_products is null) // 1) обращение - заполняем коллекцию 
            {
                using SqlCommand cmd = new("SELECT P.Id, P.Name, P.Price FROM Products P", _connection);
                try
                {
                    using SqlDataReader reader = cmd.ExecuteReader();
                    _products = new();
                    while (reader.Read())
                    {
                        _products.Add(new()
                        {
                            Id = reader.GetGuid("id"),
                            Name = reader.GetString(1),
                            Price = reader.GetDouble(2)
                        }) ;
                    }
                }
                catch (SqlException ex)
                {
                    App.Logger.Log(ex.Message, Logging.LogLevel.Error, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod()?.Name ?? "", cmd.CommandText);
                }

            }
            ProductsCell.Text = "";
            foreach (var product in _products)
            {
                ProductsCell.Text += product.ToShortString() + "\n";
            }
        }

        private void ShowManagersORM()
        {
            if (_managers is null) // 1) обращение - заполняем коллекцию 
            {
                using SqlCommand cmd = new("SELECT M.Id, M.Surname, M.Name, M.Secname FROM Managers M", _connection);
                try
                {
                    using SqlDataReader reader = cmd.ExecuteReader();
                    _managers = new();
                    while (reader.Read())
                    {
                        _managers.Add(new()
                        {
                            Id = reader.GetGuid("id"),
                            Name = reader.GetString("Name"),
                            Surname = reader.GetString("Surname"),
                            Secname = reader.GetString("Secname")
                        });
                    }
                }
                catch (SqlException ex)
                {
                    App.Logger.Log(ex.Message, Logging.LogLevel.Error, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod()?.Name ?? "", cmd.CommandText);
                }

            }
            ManagersCell.Text = "";
            foreach (var managers in _managers)
            {
                ManagersCell.Text += managers.ToShortString() + "\n";
            }
        }

        private void ShowTodaySalesORM()
        {
            String date = $"2022-{DateTime.Now.Month}-{DateTime.Now.Day}";
            if (_todaysales is null) // 1) обращение - заполняем коллекцию 
            {
                using SqlCommand cmd = new($"SELECT s.ID, p.Name , s.Cnt, p.price FROM Sales s JOIN Products p ON s.ID_product = p.id WHERE CAST(s.Moment AS DATE) = '{date}'", _connection);
                try
                {
                    using SqlDataReader reader = cmd.ExecuteReader();
                    _todaysales = new();
                    while (reader.Read())
                    {
                        _todaysales.Add(new()
                        {
                            Id = reader.GetGuid("ID"),
                            Name = reader.GetString("Name"),
                            Count = reader.GetInt32("Cnt"),
                            Sum = reader.GetDouble("Price") * reader.GetInt32("Cnt")
                        });
                    }
                }
                catch (SqlException ex)
                {
                    App.Logger.Log(ex.Message, Logging.LogLevel.Error, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod()?.Name ?? "", cmd.CommandText);
                }

            }
            TodaySalesCell.Text = "";
            foreach (var todaySales in _todaysales)
            {
                TodaySalesCell.Text += todaySales.ToShortString() + "\n";
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (_connection?.State == ConnectionState.Open)
            {
                _connection.Close();
            }
        }
    }
}
