using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Finance.Application.DTOs.UserDto;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Windows.Input;

namespace Finance.AvaloniaClient.ViewModels;

public partial class LoginViewModel : ObservableValidator
{
    public ICommand RegisterNavigationCommand { get; }
    public ICommand PizzaNavigationCommand { get; }

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

    public LoginViewModel()
    {
        User = new UserDto();

        //LoginCommand = new LoginCommand(_authenticator, this);
        //PizzaNavigationCommand = new NavigateCommand<PizzaViewModel>(navigationServicePizza);
        //RegisterNavigationCommand = new NavigateCommand<RegisterViewModel>(navigationServiceReg);
    }

    [RelayCommand]
    public void LoginCommand()
    {
        ValidateAllProperties();
        if (!HasErrors)
        {
            Message = string.Empty;

            try
            {
                var succes = await _authenticator.Login(_loginViewModel.User);

                _loginViewModel.PizzaNavigationCommand.Execute(succes);
            }
            catch (UserNotFoundException)
            {
                _loginViewModel.ErrorMessage = "Пользователь не найден";
            }
            catch (InvalidPasswordException)
            {
                _loginViewModel.ErrorMessage = "Не правильный пароль";
            }
            catch (Exception)
            {
                _loginViewModel.ErrorMessage = "Ошибка входа";
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

