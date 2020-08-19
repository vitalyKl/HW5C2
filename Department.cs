using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;

namespace HW5C2
{
    public class Department: INotifyPropertyChanged, INotifyCollectionChanged
    {   
        /// <summary>
        /// Название отдела
        /// </summary>
        private string departmentName;
        /// <summary>
        /// Список сотрудников в отделе
        /// </summary>
        private ObservableCollection<Employee> employees = new ObservableCollection<Employee>();
        /// <summary>
        /// Можно ли удалять отдел
        /// </summary>
        private bool isDeleteAvailable;

        /// <summary>
        /// Название отдела
        /// </summary>
        public string DepartmentName 
        { 
            get => departmentName;
            private set
            {
                departmentName = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.DepartmentName)));
            }
        }
        /// <summary>
        /// Список сотрудников в отделе
        /// </summary>
        public ObservableCollection<Employee> Employees 
        { 
            get => employees;
            private set
            {
                employees = value;
                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove));
            }
        }
        /// <summary>
        /// Можно ли удалять отдел
        /// </summary>
        public bool IsDeleteAvailable { get => isDeleteAvailable; set => isDeleteAvailable = value; }
        /// <summary>
        /// Шаблон для распределения значений по переменным
        /// </summary>
        private readonly static Regex template = new Regex(@"(?<name>\w+\s*\w*)\s(?<delitable>\w+)", RegexOptions.Compiled);

        public event PropertyChangedEventHandler PropertyChanged;
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public Department(string input)
        {
            var match = template.Match(input);
            DepartmentName = match.Groups["name"].Value;
            IsDeleteAvailable = Convert.ToBoolean(match.Groups["delitable"].Value);
        }

        /// <summary>
        /// Добавляет в отдел сотрудника
        /// </summary>
        /// <param name="employee">Сотрудник добавляющийся в отдел</param>
        public void AddEmployee(Employee employee)
        {
            employee.ChangeDepartment(DepartmentName);
            Employees.Add(employee);            
        }
        /// <summary>
        /// Удалить сотрудника из отдела
        /// </summary>
        /// <param name="employee">Сотрудник подлежащий удалению</param>
        public void RemoveEmployee(Employee employee)
        {
            Employees.Remove(employee);
        }
        /// <summary>
        /// Применение изменений для отдела
        /// </summary>
        /// <param name="newDep">Новый экземлпяр отдела</param>
        public void ChangeDepartment(Department newDep)
        {
            DepartmentName = newDep.DepartmentName;
            IsDeleteAvailable = newDep.IsDeleteAvailable;
        }

        public void NotifyPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        public override string ToString()
        {            
            return DepartmentName;
        }
        /// <summary>
        /// Вывод Department в строковый формат для записи в файл.
        /// </summary>
        /// <param name="isDeleteAvailableNeeded">Нужно ли передавать IsDeleteAvailable для отдела, по умолчанию true</param>
        /// <returns>Строковое представление класса с полем isDeleteAvailable</returns>
        public string ToString(bool isDeleteAvailableNeeded = true)
        {
            return $"{DepartmentName} {IsDeleteAvailable}";
        }
    }
}
