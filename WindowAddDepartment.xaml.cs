using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace HW5C2
{
    /// <summary>
    /// Логика взаимодействия для WindowAddDepartment.xaml
    /// </summary>
    public partial class WindowAddDepartment : Window
    {
        private Data _data;

        public WindowAddDepartment(Data data)
        {
            InitializeComponent();
            _data = data;
        }

        /// <summary>
        /// Добавление нового отдела в файл со списком отделов
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnApply_Click(object sender, RoutedEventArgs e)
        {
            bool isAdded = _data.AddDeparetment(TxtBoxDepartmentName.Text);
            if (isAdded)
            {
                MessageBoxResult result = MessageBox.Show("Успешно добавлено!");
                if (result == MessageBoxResult.OK)
                {
                    Owner.Focus();
                    Close(); 
                }
            }
        }
        /// <summary>
        /// Закрытие окна без изменений
        /// </summary>        
        private void BtnDiscard_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }  
}
