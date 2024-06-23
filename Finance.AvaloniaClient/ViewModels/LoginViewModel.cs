using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Finance.Application.DTOs.UserDto;
using Finance.Application.Interface;
using Finance.AvaloniaClient.Service.Store;
using Finance.Domain.Exceptions;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Finance.AvaloniaClient.ViewModels;

public partial class LoginViewModel : ObservableValidator
{
    private readonly IAuthenticator _authenticator;
    private readonly NavigationService<FinanceViewModel> _navigationServiceFinance;

    [ObservableProperty]
    private UserDto _user;
    
    [ObservableProperty]
    private string _message;

    [ObservableProperty]
    [Required(ErrorMessage ="Введите почту!")]
    [CustomValidation(typeof(LoginViewModel), nameof(ValidateEmail))]
    private string _email;

    [ObservableProperty]
    [Required(ErrorMessage = "Введите пароль!")]
    [StringLength(25, MinimumLength = 6, ErrorMessage ="Длина пароля должна быть не меньше 6 и не больше 25")]
    private string _passwordHash;

    public LoginViewModel(IAuthenticator authenticator, NavigationService<FinanceViewModel> navigationServiceFinance)
    {
        User = new UserDto();
        _authenticator = authenticator;
        _navigationServiceFinance = navigationServiceFinance;
    }

    [RelayCommand]
    public async Task LoginCommand()
    {
        ValidateAllProperties();
        if (!HasErrors)
        {
            Message = string.Empty;

            try
            {
                User.Email = Email;
                User.PasswordHash = PasswordHash;

                var loginedUser = await _authenticator.Login(User);

                if (loginedUser != null)
                {
                    _navigationServiceFinance.Navigate();
                }
                else
                {
                    Message = "Ошибка входа";
                }
            }
            catch (UserNotFoundException)
            {
                Message = "Пользователь не найден";
            }
            catch (InvalidPasswordException)
            {
                Message = "Не правильный пароль";
            }
            catch (Exception)
            {
                Message = "Ошибка входа";
            }
        }
        else
        {
            Message = "Ошибка входа";
        }
    }
    public static ValidationResult ValidateEmail(string email, ValidationContext validationContext)
    {
        bool isVaslid = !Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$") && email != "Admin";
        if (!isVaslid)
        {
            return ValidationResult.Success;
        }

        return new("Не корректный email.");
    }
}

