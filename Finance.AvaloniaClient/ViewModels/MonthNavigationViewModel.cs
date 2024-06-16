using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance.AvaloniaClient.ViewModels
{
    public partial class MonthNavigationViewModel : ObservableObject
    {
        [ObservableProperty]
        private int _currentMonth;
        [ObservableProperty]
        private int _currentYear;

        public MonthNavigationViewModel()
        {
            CurrentMonth = DateTime.Now.Month;
            CurrentYear = DateTime.Now.Year;
        }

        public string DisplayMonth => new DateTime(CurrentYear, CurrentMonth, 1).ToString("MMMM yyyy");

        [RelayCommand]
        public void PreviousMonth()
        {
            if (CurrentMonth == 1)
            {
                CurrentMonth = 12;
                CurrentYear--;
            }
            else
            {
                CurrentMonth--;
            }
        }

        [RelayCommand]
        public void NextMonth()
        {
            if (CurrentMonth == 12)
            {
                CurrentMonth = 1;
                CurrentYear++;
            }
            else
            {
                CurrentMonth++;
            }
        }
    }
}
