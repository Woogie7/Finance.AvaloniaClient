using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Finance.Application.DTOs;
using Finance.Application.DTOs.DtoCategory;
using Finance.Application.DTOs.DtoCurrency;
using Finance.Application.DTOs.Income;
using Finance.Application.DTOs.UserDto;
using Finance.Application.Interface;
using Finance.AvaloniaClient.Service.Store;
using Finance.Domain.Entities;
using Finance.Domain.Entities.Users;
using Finance.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance.AvaloniaClient.ViewModels
{
    public partial class AddIncomeViewModel : ObservableValidator
    {
        private readonly IIncomeApiService _incomeApiService;
        private readonly ICategoryIncomeApiInterface _categoryApiInterface;
        private readonly ICurrencyApiInterface _currencyApiInterface;
        private readonly NavigationService<FinanceViewModel> __navigationFinance;

        public ObservableCollection<CurrencyDto> Currencies { get; set; }
        public ObservableCollection<CategoryIncomeDto> Categories { get; set; }

        [ObservableProperty]
        private CreateIncomeDto _income;

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
        private CategoryIncomeDto _selectedCategory;
        private readonly NavigationService<FinanceViewModel> _navigationServiceFinance;

        public static ValidationResult ValidateDate(DateTime date, ValidationContext context)
        {
            if (date < new DateTime(2000, 1, 1) || date > DateTime.Now)
            {
                return new ValidationResult("Дата должна быть не раньше 2000 года и не позже сегодняшнего дня");
            }
            return ValidationResult.Success;
        }

        public AddIncomeViewModel(NavigationService<FinanceViewModel> navigationServiceFinance, IIncomeApiService incomeApiService, ICategoryIncomeApiInterface categoryApiInterface, ICurrencyApiInterface currencyApiInterface, NavigationService<FinanceViewModel> navigationFinance)
        {
            Income = new CreateIncomeDto();
            Currencies = new ObservableCollection<CurrencyDto>();
            Categories = new ObservableCollection<CategoryIncomeDto>();

            _navigationServiceFinance = navigationServiceFinance;
            _incomeApiService = incomeApiService;
            _categoryApiInterface = categoryApiInterface;
            _currencyApiInterface = currencyApiInterface;

            LoadCategoryIncome();
            LoadCurrency();
            __navigationFinance = navigationFinance;
        }

        [RelayCommand]
        private void NavigateFinance()
        {
            __navigationFinance.Navigate();
        }

        public async Task LoadCategoryIncome()
        {
            Categories.Clear();
            var allCategories = await _categoryApiInterface.GetCategoriesAsync();

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
        public async Task AddNewIncome()
        {
            ValidateAllProperties();
            if (!HasErrors)
            {
                Message = string.Empty;

                try
                {
                    Income.Amount = Convert.ToDecimal(Amount);
                    Income.Date = DateOnly.FromDateTime(SelectedDate);
                    Income.CurrencyId = SelectedCurrency.Id;
                    Income.CategoryIncomeId = SelectedCategory.Id;

                    var incomeDto = await _incomeApiService.AddIncomeAsync(Income);

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
