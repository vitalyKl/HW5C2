using System.Windows;

namespace HW5C2
{
    /// <summary>
    /// Логика взаимодействия для WindowChangeDepartment.xaml
    /// </summary>
    public partial class WindowChangeDepartment : Window
    {
        private Data data;
        private Department currentDep;
        private Department newDep;
        public WindowChangeDepartment(Department currentDep, Data data)
        {
            InitializeComponent();
            this.currentDep = currentDep;
            this.data = data;
            TxtBoxDepartmentName.Text = currentDep.DepartmentName;
            ChckIsDelitable.IsChecked = currentDep.IsDeleteAvailable;
        }

        private void BtnApply_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Применить изменения?", "Уведомление", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                if (string.IsNullOrEmpty(TxtBoxDepartmentName.Text))
                {
                    MessageBox.Show("Поле с имененем отдела не должно быть пустым. Повторите ввод.");
                    return;
                }
                newDep = new Department($"{TxtBoxDepartmentName.Text}\t{ChckIsDelitable.IsChecked}");
                bool isEdited = data.ChangeDepartment(currentDep, newDep);
                if (isEdited)
                {
                    Owner.Focus();
                    this.Close();                    
                }
            }

        }
    }
}
