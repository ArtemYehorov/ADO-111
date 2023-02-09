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

namespace Sales.CRUD
{
    /// <summary>
    /// Логика взаимодействия для CrudDepartmentWindow.xaml
    /// </summary>
    public partial class CrudDepartmentWindow : Window
    {

        public Enities.Department Department { get; set; } = null;
        public CrudDepartmentWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (Department is null) //режим "C"- добавление нового отдела.
            {
                Department = new Enities.Department()
                {
                    Id=Guid.NewGuid()
                };
                ButtonDelete.IsEnabled= false;
            }
            else// Режимы "UD" - есть переданный отдел для изменения и удаления.
            {
                ButtonDelete.IsEnabled= true;
            }
            DepartmentId.Text = Department.Id.ToString();
            DepartmentName.Text = Department.Name;
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            if (Department is null) return;
            Department.Name = DepartmentName.Text;
            if (DepartmentName.Text == String.Empty)
            {
                MessageBox.Show("Введите название отдела!");
                DepartmentName.Focus();
            }
            else
            {
                this.DialogResult = true;
                this.Close();
            }
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBoxResult.Yes ==
            MessageBox.Show("Вы подтверждаете удаление отдела из БД?",
                            "Удаление данных из БД",
                            MessageBoxButton.YesNo,
                            MessageBoxImage.Question)) 
            {
                Department = null!;
                this.DialogResult = true;
                this.Close();
            }
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult= false;
            this.Close();
        }
    }
}
