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
using CommunityToolkit.Mvvm.Input;
using System.Runtime.CompilerServices;
using LiveChartsCore.Defaults;
using DynamicData;
using Finance.AvaloniaClient.Service.Store;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System.IO;
using LiveChartsCore.SkiaSharpView.Avalonia;
using System.Drawing;
using Avalonia.Controls;

namespace Finance.AvaloniaClient.ViewModels;

public partial class FinanceViewModel : ObservableObject
{
    private readonly IIncomeApiService _incomeApiService;
    private readonly IExpenseApiService _expenseApiService;
    private readonly NavigationService<AddIncomeViewModel> _navigationAddIncome;
    private readonly NavigationService<AddExpenseViewModel> _navigationAddExpense;
    [ObservableProperty]
    private decimal _earnings;
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(Series))]
    private decimal _income;
    [ObservableProperty]
    private decimal _expense;
    [ObservableProperty]
    private int _currentMonth = DateTime.Now.Month;

    private ObservableCollection<IncomeDTO> Incomes { get; set; }
    private ObservableCollection<ExpenseDto> Expenses { get; set; }

    [ObservableProperty]
    private ISeries[] _series;

    [ObservableProperty]
    private Axis[] _xAxes;

    private FinanceViewModel(IIncomeApiService incomeApiService, IExpenseApiService expenseApiService, NavigationService<AddIncomeViewModel> navigationAddIncome, NavigationService<AddExpenseViewModel> navigationAddExpense)
    {
        _incomeApiService = incomeApiService;
        _expenseApiService = expenseApiService;
        _navigationAddIncome = navigationAddIncome;
        Incomes = new ObservableCollection<IncomeDTO>();
        Expenses = new ObservableCollection<ExpenseDto>();
        Series = new ISeries[] { };
        XAxes = new Axis[] { };


        InitializeCommand.Execute(null);
        _navigationAddExpense = navigationAddExpense;
    }

    public static FinanceViewModel CreateAsync(IIncomeApiService apiService, IExpenseApiService expenseApiService, 
        NavigationService<AddIncomeViewModel> navigationAddIncome, NavigationService<AddExpenseViewModel> navigationAddExpense)
    {
        var instance = new FinanceViewModel(apiService, expenseApiService, navigationAddIncome, navigationAddExpense);

        instance.InitializeCommand.Execute(null);

        instance.UpdateSeriesCommand.Execute(null);

        return instance;
    }

    [RelayCommand]
    public async Task InitializeAsync()
    {
        await LoadIncome();
        await LoadExpenses();
        GetTotalIncomeForCurrentMonth();
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
    
    [RelayCommand]
    private void NavigateAddIncome()
    {
        _navigationAddIncome.Navigate();
    }

    [RelayCommand]
    private void NavigateAddExpense()
    {
        _navigationAddExpense.Navigate();
    }

    public void GetTotalIncomeForCurrentMonth()
    {
        Income = Incomes.Sum(i => i.Amount);
        Expense = Expenses.Sum(e => e.Amount);
        Earnings = Income - Expense;
    }

    partial void OnExpenseChanged(decimal value)
    {
        UpdateSeriesCommand.Execute(null);
    }

    [RelayCommand]
    private void CreatePdf()
    {
        PdfDocument document = new PdfDocument();
        document.Info.Title = "Финансы";

        PdfPage page = document.AddPage();
        XGraphics gfx = XGraphics.FromPdfPage(page);

        XFont font = new XFont("Verdana", 20);
        gfx.DrawString("Финансы", font, XBrushes.Black, new XRect(0, 0, page.Width, 40), XStringFormats.TopCenter);

        // Рисование текста
        font = new XFont("Verdana", 12);
        gfx.DrawString($"Доходы: {Income:C}", font, XBrushes.Black, new XRect(20, 60, page.Width, 20), XStringFormats.TopLeft);
        gfx.DrawString($"Расходы: {Expense:C}", font, XBrushes.Black, new XRect(20, 80, page.Width, 20), XStringFormats.TopLeft);
        gfx.DrawString($"Сбережения: {Earnings:C}", font, XBrushes.Black, new XRect(20, 100, page.Width, 20), XStringFormats.TopLeft);

        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string filename = Path.Combine(desktopPath, "123.pdf");
        document.Save(filename);
    }

    [RelayCommand]
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
