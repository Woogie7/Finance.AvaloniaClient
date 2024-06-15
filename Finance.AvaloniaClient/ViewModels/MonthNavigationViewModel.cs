using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance.AvaloniaClient.ViewModels
{
    //public partial class MonthNavigationViewModel : ObservableObject
    //{
    //    [ObservableProperty]
    //    private int _currentMonth;
    //    [ObservableProperty]
    //    private int _currentYear;

    //    public MonthNavigationViewModel()
    //    {
    //        CurrentMonth = DateTime.Now.Month;
    //        CurrentYear = DateTime.Now.Year;
    //    }

    //    public int CurrentMonth
    //    {
    //        get => _currentMonth;
    //        set
    //        {
    //            SetProperty(ref _currentMonth, value);
    //            OnPropertyChanged(nameof(DisplayMonth));
    //        }
    //    }

    //    public int CurrentYear
    //    {
    //        get => _currentYear;
    //        set => SetProperty(ref _currentYear, value);
    //    }

    //    public string DisplayMonth => new DateTime(CurrentYear, CurrentMonth, 1).ToString("MMMM yyyy");

    //    [RelayCommand]
    //    private void PreviousMonth()
    //    {
    //        if (CurrentMonth == 1)
    //        {
    //            CurrentMonth = 12;
    //            CurrentYear--;
    //        }
    //        else
    //        {
    //            CurrentMonth--;
    //        }
    //    }

    //    [RelayCommand]
    //    private void NextMonth()
    //    {
    //        if (CurrentMonth == 12)
    //        {
    //            CurrentMonth = 1;
    //            CurrentYear++;
    //        }
    //        else
    //        {
    //            CurrentMonth++;
    //        }
    //    }
    //}
}
