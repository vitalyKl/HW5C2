using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Data;
using static HW5C2.DocumentWorker;

namespace HW5C2
{
    public class Data: INotifyCollectionChanged
    {
        private ObservableCollection<Department> departments = new ObservableCollection<Department>();
        private ObservableCollection<Employee> employees = new ObservableCollection<Employee>();
        

        internal ObservableCollection<Department> Departments
        { 
            get => departments;
            private set
            {
                departments = value;
                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset, nameof(Departments)));
            }
        }
        internal ObservableCollection<Employee> Employees
        {
            get => employees;
            private set
            {
                employees = value;     
            }
        }
        private DocumentWorker dw = new DocumentWorker();

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public Data()
        {
            FillDepartmentsList();            
            FillEmployeesList();
        }

        /// <summary>
        /// Заполнение коллекции departments именами отделов
        /// </summary>
        /// <param name="path">Путь к файлу со списком отделов</param>
        public void FillDepartmentsList()
        {
            Departments.Clear();
            List<string> input = dw.TakeInputData(InputType.department).Split($"{Environment.NewLine}").ToList();
            foreach (string x in input)
            {
                Department dep = new Department(x);
                Departments.Add(dep);
            }
        }
        /// <summary>
        /// Удаление отдела
        /// </summary>
        /// <param name="departmentToDelete">Название отдела для удаления</param>
        public void DeleteDepartment(Department departmentToDelete)
        {
            MessageBoxResult answer = MessageBox.Show($"Вы уверены, что хотите удалить отдел \"{departmentToDelete}\"?", "Удаление отдела", MessageBoxButton.YesNo);
            if (answer == MessageBoxResult.Yes)
            {
                var dep = Departments.Where(x => x == departmentToDelete).First();
                if (dep.IsDeleteAvailable == false)
                {
                    MessageBox.Show("Данный отдел запрещён к удалению!");
                    return;
                }
                var emp = Employees.Where(x => x.Department == dep.DepartmentName).Take(dep.Employees.Count);
                var depEmpty = Departments.Where(x => x.DepartmentName == "Без отдела").First();
                foreach (Employee employee in emp)
                {
                    employee.ChangeDepartment("Без отдела");
                    depEmpty.AddEmployee(employee);
                }
                Departments.Remove(dep);
                UpdateDocuments();
            }
        }
       
        /// <summary>
        /// Добавление нового отдела
        /// </summary>
        /// <param name="departmentData">Имя нового отдела</param>
        /// <returns>bool удалось ли добавить отдел</returns>
        public bool AddDeparetment(string departmentData)
        {
            bool isAded = false;
            try
            {
                if (string.IsNullOrWhiteSpace(departmentData))
                {
                    throw new ArgumentNullException();
                }
                var sameName = Departments.Where(x => x.DepartmentName == departmentData).FirstOrDefault();
                if (sameName != null)
                {
                    throw new InvalidDataException();
                }
                else
                {
                    string output = $"{departmentData}\ttrue";
                    Department x = new Department(output);
                    Departments.Add(x);
                    UpdateDocuments();
                    isAded = true;
                }

            }
            catch (InvalidDataException)
            {
                MessageBox.Show("Такое название отдела уже использовано! Введите другое!");
            }
            catch (ArgumentNullException)
            {
                MessageBox.Show("Поле с именем отдела пустое! Введите значение!");
            }
            return isAded;
        }
        /// <summary>
        /// Изменение отдела
        /// </summary>
        /// <param name="currentDep">Ссылка на текущий экземпляр отдела</param>
        /// <param name="newDep">Экземпляр отдела, с которого берутся новые данные</param>
        /// <returns></returns>
        public bool ChangeDepartment(Department currentDep, Department newDep)
        {                       
            var dep = Departments.Where(x => x == currentDep).First();
            foreach (Employee emp in dep.Employees)
            {
                emp.ChangeDepartment(newDep.DepartmentName);
            }
            dep.ChangeDepartment(newDep);
            UpdateDocuments();            
            return true;
        }

        /// <summary>
        /// Заполнение списка employees данными из файла
        /// </summary>
        /// <param name="path">Путь к файлу со списком сотрудников</param>
        public void FillEmployeesList()
        {
            Employees.Clear();
            List<string> input = dw.TakeInputData(InputType.employee).Split($"{Environment.NewLine}").ToList();
            foreach (string x in input)
            {
                Employee emp = new Employee(x);
                var temp = Departments.Where(x => x.DepartmentName == emp.Department).FirstOrDefault();
                if (temp == null)
                {
                    emp.ChangeDepartment("Без отдела");
                    temp = Departments.Where(x => x.DepartmentName == "Без отдела").First();
                }
                temp.AddEmployee(emp);
                Employees.Add(emp);
            }
        }
        /// <summary>
        /// Добавление сотрудника
        /// </summary>
        /// <param name="employeeData">Строка, содержащаяя всю информацию по сотруднику</param>
        public void AddEmployee(string employeeData)
        {
            Employee newEmp = new Employee(employeeData);
            Employees.Add(newEmp);
            Departments.Where(x => x.DepartmentName == newEmp.Department).First().AddEmployee(newEmp);
            UpdateDocuments();
        }
        /// <summary>
        /// Редактирование сотрудника
        /// </summary>
        /// <param name="output">Строка, содержащая всю информацию по сотруднику</param>
        /// <param name="currentEmp">Ссылка на сотрудника, которого нужно изменить</param>
        public void EditEmployee(string output, Employee currentEmp)
        {
            Employee newEmp = new Employee(output);
            Departments.Where(x => x.DepartmentName == currentEmp.Department).First().RemoveEmployee(currentEmp);
            currentEmp.EditEmployee(newEmp);
            Departments.Where(x => x.DepartmentName == currentEmp.Department).First().AddEmployee(currentEmp);
            UpdateDocuments();
        }
        /// <summary>
        /// Удаление сотрудника
        /// </summary>
        /// <param name="currentEmp">Ссылка, на выбранного сотрудника</param>
        public void RemoveEmployee(Employee currentEmp)
        {
            Departments.Where(x => x.DepartmentName == currentEmp.Department).First().RemoveEmployee(currentEmp);
            Employees.Remove(currentEmp);
            UpdateDocuments();
        }

        /// <summary>
        /// Запуск обновления документов
        /// </summary>
        private void UpdateDocuments()
        {
            dw.RewritingDocuments(Departments, Employees);
        }
    }
}
