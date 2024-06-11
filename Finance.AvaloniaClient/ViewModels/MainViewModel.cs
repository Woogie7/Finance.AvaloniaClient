using CommunityToolkit.Mvvm.ComponentModel;
using Finance.Application.Interface;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using System.Threading.Tasks;

namespace Finance.AvaloniaClient.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly IIncomeApiService _incomeApiService;

    [ObservableProperty]
    private string _buldTitle = "Привет";
    [ObservableProperty]
    private decimal _income = 30000.00m;
    [ObservableProperty]
    private decimal _expenses = 25000.00m;


    public ISeries[] Series { get; set; }
            = new ISeries[]
            {
                new ColumnSeries<double>
                {
                    Values = new double[] { 2, 5, 4, -2, 4, -3, 5 }
                },
                new ColumnSeries<double>
                {
                    Values = new double[] { 3, 2, 5, -1, -4, -5, 3 }
                },
                new LineSeries<int>
                {
                    Values = new int[] { 4, 6, 5, 3, -3, -1, 2 }
                }
            };

    public MainViewModel(IIncomeApiService incomeApiService)
    {
        _incomeApiService = incomeApiService;

        LoadIncome();
    }

    private async Task LoadIncome()
    {
        var incomes = await _incomeApiService.GetIncomesAsync();
    }


}
