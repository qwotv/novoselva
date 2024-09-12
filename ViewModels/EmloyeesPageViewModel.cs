using AvaloniaApplication2.Models;
using Microsoft.EntityFrameworkCore;
using ReactiveUI;
using Spire.Doc;
using Spire.Xls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

namespace AvaloniaApplication2.ViewModels
{
    public class EmployeesPageViewModel : ViewModelBase
    {
        private IEnumerable<Employee> employees = new List<Employee>();
        public IEnumerable<Employee> Employees
        {
            get => employees;
            set => this.RaiseAndSetIfChanged(ref employees, value);
        }
        private Employee selectedEmployee = new Employee();
        public Employee SelectedEmployee
        {
            get => selectedEmployee;
            set
            {
                this.RaiseAndSetIfChanged(ref selectedEmployee, value);
            }
        }
        public EmployeesPageViewModel()
        {
            Update();
        }
        public void Update()
        {
            db = new DbIs615Context();
            db.Employees.Load();
            Employees = db.Employees.Local.ToObservableCollection();
        }
        private DbIs615Context db = new DbIs615Context();
        public void Add()
        {
            var _employee = new Employee();
            db.Employees.Local.Add(_employee);
            Employees = db.Employees.Local.ToObservableCollection();
            SelectedEmployee = _employee;
        }
        public void Remove()
        {
            db.Employees.Local.Remove(SelectedEmployee);
            Employees = db.Employees.Local.ToObservableCollection();
        }
        public void Save()
        {
            db.SaveChanges();
            Update();
        }
        public void Export()
        {
            Workbook workbook = new Workbook();
            workbook.Worksheets.Clear();
            Worksheet sheet = workbook.CreateEmptySheet();
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("ИД");
            dataTable.Columns.Add("ФИО");
            dataTable.Columns.Add("ДОЛЖНОСТЬ");
            foreach (var item in Employees)
            {

                if (SelectedEmployee!.Id != null)
                    if (SelectedEmployee!.Id == item.Id) { continue; }
                dataTable.Rows.Add(item.Id, item.Name, item.Post);
            }
            sheet.InsertDataTable(dataTable, true, 1, 1);
            workbook.SaveToFile("export.xlsx", ExcelVersion.Version2007);
            string full = Environment.CurrentDirectory + "\\export.xlsx";
            Process.Start(new ProcessStartInfo
            {
                FileName = full,
                UseShellExecute = true,
            });
        }
        
    }
}