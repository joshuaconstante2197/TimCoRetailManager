﻿using Caliburn.Micro;
using JRMDesktopUI.Library;
using System;
using System.Threading.Tasks;
using JRMDesktopUI.Library.Api;
using JRMDesktopUI.EventModels;

namespace JRMDesktopUI.ViewModels
{
    public class LoginViewModel : Screen
    {
        private string _userName;
        private string _password;
        private IAPIHelper _apiHelper;
        private IEventAggregator _events;
        private string _errorMessage;
        public LoginViewModel(IAPIHelper apiHelper, IEventAggregator events)
        {
            _apiHelper = apiHelper;
            _events = events;
        }

        public string UserName
        {
            get { return _userName; }
            set 
            { 
                _userName = value;
                NotifyOfPropertyChange(() => UserName);

            }
        }

        public string Password
        {
            get { return _password; }
            set 
            { 
                _password = value;
                NotifyOfPropertyChange(() => Password);
                NotifyOfPropertyChange(() => CanLogIn);
            }
        }

        

        public bool IsErrorVisible
        {
            get 
            {
                bool output = false;
                if(ErrorMessage?.Length > 0)
                {
                    output = true;
                }
                return output; 
            }
            
        }


        public string ErrorMessage
        {
            get { return _errorMessage; }
            set 
            {
                _errorMessage = value;
                NotifyOfPropertyChange(() => IsErrorVisible);
                NotifyOfPropertyChange(() => ErrorMessage);
            }
        }


        public bool CanLogIn
        {
            get
            {
                bool output = false;

                if (UserName?.Length > 0 && Password?.Length > 0)
                {
                    output = true;
                }

                return output;
            }

        }

        public async Task LogIn()
        {
            try
            {
                ErrorMessage = "";
                var result = await _apiHelper.Authenticate(UserName, Password);

                //Capture more information about hte user
                await _apiHelper.GetLoggedUnserInfo(result.Access_Token);

                _events.PublishOnUIThread(new LogOnEvent());

            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }



    }
}
