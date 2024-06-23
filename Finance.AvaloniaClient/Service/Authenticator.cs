using CommunityToolkit.Mvvm.ComponentModel;
using Finance.Application.DTOs.UserDto;
using Finance.Application.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance.AvaloniaClient.Service
{
    internal class Authenticator : ObservableObject, IAuthenticator
    {
        private readonly IAuthenticationService _authenticationService;

        public Authenticator(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        private UserDto _currentUser;

        public UserDto CurrentUser
        {
            get => _currentUser;
            private set
            {
                SetProperty(ref _currentUser, value);
                OnPropertyChanged(nameof(IsLoggedIn));
            }
        }

        public bool IsLoggedIn => CurrentUser != null;

        public async Task<UserDto> Login(UserDto userDto)
        {
            CurrentUser = await _authenticationService.Login(userDto);

            return CurrentUser;
        }

        public void Logout()
        {
            CurrentUser = null;
        }

        public async Task Register(CreateUserDto userDto)
        {
            await _authenticationService.Register(userDto);
        }
    }
}
