using GalaSoft.MvvmLight.Command;
using Shop.UIForms.Views;
using System.Windows.Input;
using Xamarin.Forms;

namespace Shop.UIForms.ViewModels
{
    public class LoginViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public ICommand LoginCommand => new RelayCommand(Login);

        public LoginViewModel()
        {
            Email = "emilio@gmail.com";
            Password = "Eabs123.";
        }

        private async void Login()
        {
            if (string.IsNullOrEmpty(Email))
            {
                await App.Current.MainPage.DisplayAlert("Error", "Please, You must enter an Email.", "Accept");

                return;
            }

            if (string.IsNullOrEmpty(Password))
            {
                await App.Current.MainPage.DisplayAlert("Error", "Plese, You must enter a Password.", "Accept");

                return;
            }

            if (!Email.Equals("emilio@gmail.com") || !Password.Equals("Eabs123."))
            {
                await App.Current.MainPage.DisplayAlert("Ok", "User or Password wrong....", "Accept");
                return;

            }

            //await App.Current.MainPage.DisplayAlert("Ok", "Fuck yeah!!!!", "Accept");

            MainViewModel.GetInstance().Products = new ProductViewModel();
            await Application.Current.MainPage.Navigation.PushAsync(new ProductsPage());


        }
    }
}
