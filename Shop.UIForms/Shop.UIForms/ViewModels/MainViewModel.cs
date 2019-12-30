using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.UIForms.ViewModels
{
    public class MainViewModel
    {
        private static MainViewModel instance;
        public LoginViewModel Login { get; set; }
        public ProductViewModel Products { get; set; }

        public MainViewModel()
        {
            //this.Login = new LoginViewModel();

            instance = this;
            
        }

        public static MainViewModel GetInstance()
        {
            if (instance == null)
            {
                return new  MainViewModel();
            }

            return instance;
        }


    }
}
