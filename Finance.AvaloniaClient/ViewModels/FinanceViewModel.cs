using CommunityToolkit.Mvvm.ComponentModel;
using Finance.Application.DTOs.DtoExpense;
using Finance.Application.DTOs.Income;
using Finance.Application.Interface;
using LiveChartsCore.Drawing;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance.AvaloniaClient.ViewModels;

public partial class FinanceViewModel : ObservableObject
{
    private readonly IIncomeApiService _incomeApiService;
    private readonly IExpenseApiService _expenseApiService;

    [ObservableProperty]
    private decimal _earnings;
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

    public FinanceViewModel(IIncomeApiService incomeApiService, IExpenseApiService expenseApiService)
    {
        _incomeApiService = incomeApiService;
        _expenseApiService = expenseApiService;
        Incomes = new ObservableCollection<IncomeDTO>();
        Expenses = new ObservableCollection<ExpenseDto>();
        Series = new ISeries[] { };
        XAxes = new Axis[] { };
    }
    public static async Task<FinanceViewModel> CreateAsync(IIncomeApiService apiService, IExpenseApiService expenseApiService)
    {
        var instance = new FinanceViewModel(apiService, expenseApiService);
        await instance.InitializeAsync();
        return instance;
    }
    public async Task InitializeAsync()
    {
        // await LoadIncome();
        // await LoadExpenses();
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
        Income = Incomes.Sum(i => i.Amount);
        Expense = Expenses.Sum(e => e.Amount);
        Earnings = Income - Expense;
    }
    private void UpdateSeries()
    {
        var allDates = Incomes.Select(i => i.Date)
                              .Concat(Expenses.Select(e => e.Date))
                              .Distinct()
                              .OrderBy(d => d)
                              .ToList();

        var incomeByDate = Incomes.GroupBy(i => i.Date)
                                  .ToDictionary(g => g.Key, g => g.Sum(i => i.Amount));
        var expenseByDate = Expenses.GroupBy(e => e.Date)
                                    .ToDictionary(g => g.Key, g => g.Sum(e => e.Amount));

        var valuesIncome = new List<double>();
        var valuesExpense = new List<double>();
        var earnings = new List<double>();

        foreach (var date in allDates)
        {
            var income = incomeByDate.ContainsKey(date) ? (double)incomeByDate[date] : 0;
            var expense = expenseByDate.ContainsKey(date) ? (double)expenseByDate[date] : 0;
            valuesIncome.Add(income);
            valuesExpense.Add(expense);
            earnings.Add(income - expense);
        }


        var dateLabels = allDates.Select(d => d.ToString("dd")).ToArray();

        Series = new ISeries[]
        {
        new ColumnSeries<double>
        {
            Values = valuesIncome.ToArray(),
            Name = "Доходы"
        },
        new ColumnSeries<double>
        {
            Values = valuesExpense.ToArray(),
            Name = "Расходы"
        },
        new LineSeries<double>
        {
            Values = earnings.ToArray(),
            Name = "Выручка"
        }
        };

        XAxes = new Axis[]
        {
        new Axis
        {
            Labels = dateLabels,
            TextSize = 12,
            LabelsPaint = new SolidColorPaint(SKColors.Black),
            NamePadding = new Padding(5),
        }
        };
    }
}
