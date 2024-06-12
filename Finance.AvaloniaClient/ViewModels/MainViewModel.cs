using Avalonia;
using CommunityToolkit.Mvvm.ComponentModel;
using Finance.Application.DTOs;
using Finance.Application.Interface;
using Finance.AvaloniaClient.Service;
using Finance.Domain.Entities;
using LiveChartsCore;
using LiveChartsCore.Drawing;
using LiveChartsCore.SkiaSharpView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Finance.AvaloniaClient.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly IIncomeApiService _incomeApiService;

    [ObservableProperty]
    private string _buldTitle = "Привет";
    [ObservableProperty]
    private decimal _income;
    [ObservableProperty]
    private decimal _expenses = 25000.00m;

    private ObservableCollection<IncomeDTO> Incomes { get; set; }


    public ISeries[] Series { get; set; }
    public Axis[] XAxes { get; set; }

    public MainViewModel(IIncomeApiService incomeApiService)
    {
        _incomeApiService = incomeApiService;
        Incomes = new ObservableCollection<IncomeDTO>();
        Series = new ISeries[] { };
        XAxes = new Axis[] { }; // Initialize XAxes with empty array
    }
    public static async Task<MainViewModel> CreateAsync(IIncomeApiService apiService)
    {
        var instance = new MainViewModel(apiService);
        await instance.InitializeAsync();
        return instance;
    }
    public async Task InitializeAsync()
    {
        await LoadIncome();
        Income = GetTotalIncomeForCurrentMonth();
        UpdateSeries();
    }
    public async Task LoadIncome()
    {
        Incomes.Clear();
        var allIncome = await _incomeApiService.GetIncomesAsync();

        foreach (var income in allIncome)
        {
            Incomes.Add(income);
        }
    }

    public decimal GetTotalIncomeForCurrentMonth()
    {
        DateOnly firstDayOfMonth = new DateOnly(DateTime.Now.Year, DateTime.Now.Month, 1);
        DateOnly lastDayOfMonth = new DateOnly(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));

        var i = Incomes
            .Sum(i => i.Amount);

        return i;
    }
    private void UpdateSeries()
    {
        // Group incomes by date and order by date
        var groupedIncomes = Incomes
            .GroupBy(i => i.Date)
            .OrderBy(g => g.Key)
            .Select(g => new { Date = g.Key, TotalAmount = g.Sum(i => i.Amount) })
            .ToList();

        // Extract values for the ColumnSeries
        var values = groupedIncomes.Select(g => (double)g.TotalAmount).ToArray();

        // Create labels for X axis
        var dateLabels = groupedIncomes.Select(g => g.Date.ToString("yyyy-MM-dd")).ToArray();

        // Update the Series property
        Series = new ISeries[]
        {
            new ColumnSeries<double>
            {
                Values = values
            }
        };

        // Update the XAxes property
        XAxes = new Axis[]
        {
            new Axis
            {
                Labels = dateLabels,
                LabelsRotation = 45, // Optional: Rotate labels for better readability
                TextSize = 12,
                Name = "Dates",
                NamePadding = new Padding(10),
                UnitWidth = 1 // Ensures each label corresponds to one data point
            }
        };
    }
}
