using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Логика взаимодействия для WindowAddEmployee.xaml
    /// </summary>
    public partial class WindowAddEmployee : Window
    {
        Data data;
        /// <summary>
        /// Окно добавления сотрудника
        /// </summary>
        /// <param name="data">Ссылка на экземпляр Data</param>
        public WindowAddEmployee(Data data)
        {
            InitializeComponent();
            this.data = data;
            TxtBoxBirthDate.Text = $"{DateTime.Now.AddYears(-30):dd.MM.yyyy}";
            ComBoxDepartment.ItemsSource = data.Departments;
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (ComBoxDepartment.SelectedItem == null)
            {
                MessageBox.Show("Не выбран отдел для сотрудника!");
                return;
            }
            if (string.IsNullOrWhiteSpace(TxtBoxName.Text))
            {
                MessageBox.Show("Поле \"Имя\" не должно быть пустым!");
                return;
            }
            if (string.IsNullOrWhiteSpace(TxtBoxSurname.Text))
            {
                MessageBox.Show("Поле \"Фамилия\" не должно быть пустым!");
                return;
            }
            if (string.IsNullOrWhiteSpace(TxtBoxPatronymic.Text))
            {
                MessageBox.Show("Поле \"Отчество\" не должно быть пустым!");
                return;
            }

            string output = $"{TxtBoxSurname.Text}\t{TxtBoxName.Text}\t{TxtBoxPatronymic.Text}\t{TxtBoxBirthDate.Text}\t{ComBoxDepartment.SelectedItem}";
            data.AddEmployee(output);
            MessageBox.Show("Успешно добавлено!");
            Owner.Focus();
            Close();
        }

        private void BtnDiscard_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void TxtBoxBirthDate_TextChanged(object sender, TextChangedEventArgs e)
        {
            Regex dataTemplate = new Regex(@"(?<day>\d{2})\.(?<month>\d{2})\.(?<year>\d{4})");
            if (TxtBoxBirthDate.Text.Length == 2 && Keyboard.IsKeyDown(Key.Back) !=true)
            {
                TxtBoxBirthDate.Text += ".";
                TxtBoxBirthDate.CaretIndex = TxtBoxBirthDate.Text.Length;
            }
            if (TxtBoxBirthDate.Text.Length == 5 && Keyboard.IsKeyDown(Key.Back) != true)
            {
                TxtBoxBirthDate.Text += ".";
                TxtBoxBirthDate.CaretIndex = TxtBoxBirthDate.Text.Length;
            }
            Match mathes = dataTemplate.Match(TxtBoxBirthDate.Text);
            if (TxtBoxBirthDate.Text.Length == 10)
            {
                if (!dataTemplate.IsMatch(TxtBoxBirthDate.Text))
                {
                    MessageBox.Show("Дата указана неверно! Повторите ввод!");
                }
                if (Convert.ToInt32(mathes.Groups["day"].Value) > 31 || Convert.ToInt32(mathes.Groups["day"].Value) < 1)
                {
                    MessageBox.Show("День месяца может принимать значения от 01 до 31");
                }
                if (Convert.ToInt32(mathes.Groups["month"].Value) > 12 || Convert.ToInt32(mathes.Groups["month"].Value) < 1)
                {
                    MessageBox.Show("Месяц может принимать значения от 01 до 12");
                }
                if (Convert.ToInt32(mathes.Groups["year"].Value) > DateTime.Now.Year-18 || Convert.ToInt32(mathes.Groups["year"].Value) < DateTime.Now.Year-70)
                {
                    MessageBox.Show($"Год может принимать значения от {DateTime.Now.Year - 70} до {DateTime.Now.Year - 18}");
                }
            }
            if (TxtBoxBirthDate.Text.Length > 10)
            {
                TxtBoxBirthDate.Text = TxtBoxBirthDate.Text.Substring(0, 10);
                TxtBoxBirthDate.CaretIndex = TxtBoxBirthDate.Text.Length;
            }
        }
    }
}
