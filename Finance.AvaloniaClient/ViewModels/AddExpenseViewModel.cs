using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Finance.Application.DTOs.DtoCategory;
using Finance.Application.DTOs.DtoCurrency;
using Finance.Application.DTOs.DtoExpense;
using Finance.Application.Interface;
using Finance.AvaloniaClient.Service.Store;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance.AvaloniaClient.ViewModels
{
    public partial class AddExpenseViewModel : ObservableValidator
    {
        private readonly IExpenseApiService _expenseApiService;
        private readonly ICategoryExpenseApiInterface _categoryApiInterface;
        private readonly ICurrencyApiInterface _currencyApiInterface;
        private readonly NavigationService<FinanceViewModel> __navigationFinance;

        public ObservableCollection<CurrencyDto> Currencies { get; set; }
        public ObservableCollection<CategoryExpenseDto> Categories { get; set; }

        [ObservableProperty]
        private CreateExpenseDto _expense;

        [ObservableProperty]
        private string _message;

        [ObservableProperty]
        [Required(ErrorMessage = "Введите сумму")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Сумма должна быть числом")]
        private string _amount;

        [ObservableProperty]
        [Required(ErrorMessage = "Введите дату")]
        [CustomValidation(typeof(AddIncomeViewModel), nameof(ValidateDate))]
        private DateTime _selectedDate = DateTime.Now;

        [ObservableProperty]
        [Required(ErrorMessage = "Выберите валюту")]
        private CurrencyDto _selectedCurrency;

        [ObservableProperty]
        [Required(ErrorMessage = "Выберите категорию")]
        private CategoryExpenseDto _selectedCategory;
        private readonly NavigationService<FinanceViewModel> _navigationServiceFinance;

        public static ValidationResult ValidateDate(DateTime date, ValidationContext context)
        {
            if (date < new DateTime(2000, 1, 1) || date > DateTime.Now)
            {
                return new ValidationResult("Дата должна быть не раньше 2000 года и не позже сегодняшнего дня");
            }
            return ValidationResult.Success;
        }

        public AddExpenseViewModel(NavigationService<FinanceViewModel> navigationServiceFinance, IExpenseApiService expenseApiService, ICategoryExpenseApiInterface categoryApiInterface, ICurrencyApiInterface currencyApiInterface, NavigationService<FinanceViewModel> navigationFinance)
        {
            Expense = new CreateExpenseDto();
            Currencies = new ObservableCollection<CurrencyDto>();
            Categories = new ObservableCollection<CategoryExpenseDto>();

            _navigationServiceFinance = navigationServiceFinance;
            _expenseApiService = expenseApiService;
            _categoryApiInterface = categoryApiInterface;
            _currencyApiInterface = currencyApiInterface;

            LoadCategoryIncome();
            LoadCurrency();
            __navigationFinance = navigationFinance;
        }

        public async Task LoadCategoryIncome()
        {
            Categories.Clear();
            var allCategories = await _categoryApiInterface.GetCategoriesExpenseAsync();

            foreach (var category in allCategories)
            {
                Categories.Add(category);
            }
        }

        public async Task LoadCurrency()
        {
            Currencies.Clear();
            var allCurrency = await _currencyApiInterface.GetCurenciesAsync();

            foreach (var currency in allCurrency)
            {
                Currencies.Add(currency);
            }
        }
        [RelayCommand]
        private void NavigateFinance()
        {
            __navigationFinance.Navigate();
        }

        [RelayCommand]
        public async Task AddNewExpense()
        {
            ValidateAllProperties();
            if (!HasErrors)
            {
                Message = string.Empty;

                try
                {
                    Expense.Amount = Convert.ToDecimal(Amount);
                    Expense.Date = DateOnly.FromDateTime(SelectedDate);
                    Expense.CurrencyId = SelectedCurrency.Id;
                    Expense.CategoryExpenseId = SelectedCategory.Id;

                    var incomeDto = await _expenseApiService.AddExpenseAsync(Expense);

                    if (incomeDto != null)
                    {
                        _navigationServiceFinance.Navigate();
                    }
                    else
                    {
                        Message = "Ошибка добавления";
                    }
                }
                catch (Exception)
                {
                    Message = "Ошибка добавления";
                }
            }
            else
            {
                Message = "Ошибка добавления";
            }
        }
    }
}
