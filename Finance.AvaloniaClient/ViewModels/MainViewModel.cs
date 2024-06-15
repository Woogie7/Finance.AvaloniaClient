using Avalonia;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Finance.Application.DTOs;
using Finance.Application.DTOs.DtoExpense;
using Finance.Application.DTOs.Income;
using Finance.Application.Interface;
using Finance.AvaloniaClient.Service;
using Finance.Domain.Entities;
using LiveChartsCore;
using LiveChartsCore.Drawing;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
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
    private readonly IExpenseApiService _expenseApiService;

    [ObservableProperty]
    private string _buldTitle = "Привет";
    [ObservableProperty]
    private decimal _income;
    [ObservableProperty]
    private decimal _expense;
    [ObservableProperty]
    private int _currentMonth = DateTime.Now.Month;

    private ObservableCollection<IncomeDTO> Incomes { get; set; }
    private ObservableCollection<ExpenseDto> Expenses { get; set; }


    public ISeries[] Series { get; set; }
    public Axis[] XAxes { get; set; }

    public MainViewModel(IIncomeApiService incomeApiService, IExpenseApiService expenseApiService)
    {
        _incomeApiService = incomeApiService;
        _expenseApiService = expenseApiService;
        Incomes = new ObservableCollection<IncomeDTO>();
        Expenses = new ObservableCollection<ExpenseDto>();
        Series = new ISeries[] { };
        XAxes = new Axis[] { };
    }
    public static async Task<MainViewModel> CreateAsync(IIncomeApiService apiService, IExpenseApiService expenseApiService)
    {
        var instance = new MainViewModel(apiService, expenseApiService);
        await instance.InitializeAsync();
        return instance;
    }
    public async Task InitializeAsync()
    {
        await LoadIncome();
        await LoadExpenses();
        GetTotalIncomeForCurrentMonth();
        UpdateSeries();
    }
    public async Task LoadExpenses()
    {
        Expenses.Clear();
        var allExpense = await _expenseApiService.GetExpenseAsync();

        allExpense = allExpense.Where(i => i.Date.Month == CurrentMonth);

        foreach (var expense in allExpense)
        {
            Expenses.Add(expense);
        }
    }
    public async Task LoadIncome()
    {
        Incomes.Clear();
        var allIncome = await _incomeApiService.GetIncomesAsync();

        allIncome = allIncome.Where(i => i.Date.Month == CurrentMonth);

        foreach (var income in allIncome)
        {
            Incomes.Add(income);
        }
    }

    public void GetTotalIncomeForCurrentMonth()
    {
        Income =  Incomes.Sum(i => i.Amount);
        Expense = Expenses.Sum(e => e.Amount);
    }
    private void UpdateSeries()
    {
        var groupedIncomes = Incomes
            .GroupBy(i => i.Date)
            .OrderBy(g => g.Key)
            .Select(g => new { Date = g.Key, TotalAmount = g.Sum(i => i.Amount) })
            .ToList();

        var values = groupedIncomes.Select(g => (double)g.TotalAmount).ToArray();   

        var dateLabels = groupedIncomes.Select(g => g.Date.ToString("dd-MM-yy")).ToArray();

        Series = new ISeries[]
        {
            new ColumnSeries<double>
            {
                Values = values
            }
        };
        XAxes = new Axis[]
        {
            new Axis
            {
                Labels = dateLabels,
                TextSize = 14,
                LabelsPaint = new SolidColorPaint(SKColors.Black),
                NamePadding = new Padding(5)
            }
        };
    }
}
