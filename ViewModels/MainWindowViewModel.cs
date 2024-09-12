using AvaloniaApplication2.Views;
using System;

namespace AvaloniaApplication2.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public GoodsPage GoodsPage { get; set; }

        public EmployeesPage EmployeesPage { get; set; }
        public MainWindowViewModel() 
        {
            GoodsPage = new GoodsPage();
            GoodsPage.DataContext = new GoodsPageViewModel();

            EmployeesPage = new EmployeesPage();
            EmployeesPage.DataContext = new EmployeesPageViewModel();
        }
    }
}