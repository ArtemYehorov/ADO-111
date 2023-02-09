using Sales.Enities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Логика взаимодействия для CrudManagerWindow.xaml
    /// </summary>
    public partial class CrudManagerWindow : Window
    {
        public Enities.Managers manager { get; set; } = null;
        public CrudManagerWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = (Owner as DisconnectWindow);
            if (manager is null) //режим "C"- добавление нового отдела.
            {
                manager = new Enities.Managers()
                {
                    Id = Guid.NewGuid()
                };
                ButtonDelete.IsEnabled = false;
            }
            else// Режимы "UD" - есть переданный отдел для изменения и удаления.
            {
                ButtonDelete.IsEnabled = true;
            }
            ManagerId.Text = manager.Id.ToString();
            ManagerSurname.Text = manager.Surname;
            ManagerName.Text = manager.Name;
            ManagerSecname.Text = manager.Secname;
            DepartmentsCombo.SelectedItem= manager.IdMainDep.ToString();
            SecondDepartmentsCombo.SelectedItem = manager.IdSecDep.ToString();
            ManagersCombo.SelectedItem = manager.IdChief.ToString();
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            if (DepartmentsCombo.SelectedValue is Enities.Department deparment)
            {

                if (manager is null) return;
                manager.Name = ManagerName.Text;
                manager.Surname = ManagerSecname.Text;
                manager.Secname = ManagerSecname.Text;
                manager.IdMainDep = (DepartmentsCombo.SelectedItem as Department).Id;
                if (ManagerSurname.Text == String.Empty)
                {
                    MessageBox.Show("Введите фамилию работника!");
                    ManagerSurname.Focus();
                }
                else if (ManagerName.Text == String.Empty)
                {
                    MessageBox.Show("Введите имя работника!");
                    ManagerName.Focus();
                }
                else if (ManagerSecname.Text == String.Empty)
                {
                    MessageBox.Show("Введите отчество работника!");
                    ManagerName.Focus();
                }
                else if (DepartmentsCombo.SelectedItem.ToString() == String.Empty)
                {
                    MessageBox.Show("Введите депертамент работника!");
                    ManagerName.Focus();
                }
                else
                {
                    this.DialogResult = true;
                    this.Close();
                }
                Guid id = deparment.Id;
                MessageBox.Show(
                    $"{ManagerSurname.Text} - {ManagerName.Text} - {ManagerSecname.Text} - {id}"
                    );
            }
            else
            {
                MessageBox.Show("Выберете отдел!");
            }
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
