using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input.Manipulations;

namespace HW5C2
{
    public class Employee:INotifyPropertyChanged
    {
        private string name;
        private string surname;
        private string patronymic;
        private string birthDate;
        private string department;

        /// <summary>
        /// Имя сотрудника
        /// </summary>
        public string Name 
        { 
            get => name;
            private set
            {
                name = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
            }
        }
        /// <summary>
        /// Фамилия сотрудника
        /// </summary>
        public string Surname 
        { 
            get => surname;
            private set
            {
                surname = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Surname)));
            }
        }
        /// <summary>
        /// Отчество сотрудника
        /// </summary>
        public string Patronymic 
        { 
            get => patronymic;
            private set
            {
                patronymic = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Patronymic)));
            }
        }
        /// <summary>
        /// Дата рождения сотрудника
        /// </summary>
        public string BirthDate 
        { 
            get => birthDate;
            private set
            {
                birthDate = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BirthDate)));
            }
        }
        /// <summary>
        /// За каким отделом закреплён сотрудник
        /// </summary>
        public string Department 
        { 
            get => department;
            private set
            {
                department = value;
            }
        }
        /// <summary>
        /// Макет, по которому заполняется Employee из документа
        /// </summary>
        private static readonly Regex template = new Regex(@"(?<surname>\w+)\s(?<name>\w+)\s(?<patronymic>\w+)\s(?<birthDate>\d{2}\.\d{2}\.\d{4})\s(?<department>\w+\s?\w*)", RegexOptions.Compiled);

        public event PropertyChangedEventHandler PropertyChanged;

        public Employee(string data)
        {
            try
            {
                var match = template.Match(data);

                Name = match.Groups["name"].Value;
                Surname = match.Groups["surname"].Value;
                Patronymic = match.Groups["patronymic"].Value;
                BirthDate = match.Groups["birthDate"].Value;
                Department = match.Groups["department"].Value;
            }
            catch
            {
                MessageBox.Show($"Исходная строка \"{data}\" имела недопустимый формат!");
            }            
        }
        public override string ToString()
        {
            return $"{Surname} {Name} {Patronymic} {BirthDate}"; ;
        }
        /// <summary>
        /// Вывод текстового значения Employee
        /// </summary>
        /// <param name="IsDepartmentNeeded">Определяет нужно ли выводить название отдела (по умолчанию true)</param>
        /// <returns></returns>
        public string ToString(bool IsDepartmentNeeded = true)
        {
            return $"{Surname} {Name} {Patronymic} {BirthDate} {Department}";
        }
        public void ChangeDepartment(string newDeparmentName)
        {
            Department = newDeparmentName;
        }
        public void EditEmployee(Employee newEmp)
        {
            Surname = newEmp.Surname;
            Name = newEmp.Name;
            Patronymic = newEmp.Patronymic;
            BirthDate = newEmp.BirthDate;
            Department = newEmp.Department;
        }
    }
}
