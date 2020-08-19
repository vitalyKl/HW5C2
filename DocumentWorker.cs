using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace HW5C2
{
    class DocumentWorker
    {
        public enum InputType
        {
            employee,
            department
        }
        private readonly string departmentsListPath = @"data\departments.txt";
        private readonly string employeesListPath = @"data\employees.txt";

        /// <summary>
        /// Перезаписывание файлов с данными
        /// </summary>
        public void RewritingDocuments(ObservableCollection<Department> departments, ObservableCollection<Employee> employees)
        {
            using (StreamWriter swr = new StreamWriter(departmentsListPath))
            {
                foreach (Department dep in departments)
                {
                    swr.WriteLine(dep.ToString(true));
                }
                swr.Dispose();
            }
            using (StreamWriter swr = new StreamWriter(employeesListPath))
            {
                foreach (Employee emp in employees)
                {
                    swr.WriteLine(emp.ToString(true));
                }
                swr.Dispose();
            }
        }

        public string TakeInputData(InputType type)
        {
            string input = null;
            string path = null;
            if (type == InputType.department)
            {
                path = departmentsListPath;
            }
            else if (type == InputType.employee)
            {
                path = employeesListPath;
            }
            try
            {
                bool isFileExists = File.Exists(path);
                if (isFileExists == false)
                {
                    throw new FileNotFoundException();
                }
                using (StreamReader str = new StreamReader(path))
                {
                    input = str.ReadToEnd();
                    if (string.IsNullOrWhiteSpace(input))
                    {
                        throw new InvalidDataException();
                    }
                    input = input.Remove(input.Length - 2);
                    str.Dispose();
                }
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show($"По пути \"...{path}\" не найден искомый список!");
            }
            catch (InvalidDataException)
            {
                MessageBox.Show($"По пути \"...{path}\" файл не содержит данных!");
            }
            return input;
        }
    }
}
