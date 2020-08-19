using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Windows;
using System.Windows.Controls;

namespace HW5C2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Data data;        

        public MainWindow()
        {
            InitializeComponent();
            data = new Data();
            LstViewDepartments.ItemsSource = data.Departments;
        }

        
        /// <summary>
        /// Заполнение LstBoxEmployees данными из выделенного отдела в LstBoxDepartments
        /// </summary>        
        private void LstViewDepartments_SelectionChanged(object sender, EventArgs e)
        {
            LstViewEmployees.ItemsSource = data.Departments.Where(x => x == LstViewDepartments.SelectedItem).FirstOrDefault().Employees;
        }
        
        private void BtnAddDepartment_Click(object sender, RoutedEventArgs e)
        {
            var w = new WindowAddDepartment(data);
            w.Owner = this;            
            w.Show();
        }

        private void BtnDeleteDepartment_Click(object sender, RoutedEventArgs e)
        {
            if (LstViewDepartments.SelectedItem == null)
            {
                MessageBox.Show("Не выделен отдел для удаления!");
                return;
            }
            data.DeleteDepartment(LstViewDepartments.SelectedItem as Department); 
        }

        private void BtnChangeDepartment_Click(object sender, RoutedEventArgs e)
        {
            if (LstViewDepartments.SelectedValue.ToString() == "Без отдела")
            {
                MessageBox.Show("Данный отдел редактировать нельзя!");
                return;
            }
            if (LstViewDepartments.SelectedItem == null)
            {
                MessageBox.Show("Не выделен отдел для удаления!");
                return;
            }
            var w = new WindowChangeDepartment(LstViewDepartments.SelectedItem as Department, data);
            w.Owner = this;
            w.Show();
        }

        private void LstViewDepartments_GotFocus(object sender, RoutedEventArgs e)
        {
            BtnChangeDepartment.Visibility = Visibility.Visible;
            BtnDeleteDepartment.Visibility = Visibility.Visible;
            BtnChangeEmployee.Visibility = Visibility.Hidden;
            BtnDeleteEmployee.Visibility = Visibility.Hidden;

        }

        private void LstViewEmployees_GotFocus(object sender, RoutedEventArgs e)
        {
            BtnChangeDepartment.Visibility = Visibility.Hidden;
            BtnDeleteDepartment.Visibility = Visibility.Hidden;
            BtnChangeEmployee.Visibility = Visibility.Visible;
            BtnDeleteEmployee.Visibility = Visibility.Visible;
        }

        private void BtnAddEmployee_Click(object sender, RoutedEventArgs e)
        {
            WindowAddEmployee w = new WindowAddEmployee(data);
            w.Owner = this;
            w.Show();
        }

        private void BtnChangeEmployee_Click(object sender, RoutedEventArgs e)
        {
            WindowEditEmployee w = new WindowEditEmployee((Employee)LstViewEmployees.SelectedItem, data);
            w.Owner = this;
            w.Show();
        }

        private void BtnDeleteEmployee_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите удалить сотрудника?", "Удаление сотрудника", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                data.RemoveEmployee((Employee)LstViewEmployees.SelectedItem);
                MessageBox.Show("Сотрудник удалён!");
            }
        }
    }
}
