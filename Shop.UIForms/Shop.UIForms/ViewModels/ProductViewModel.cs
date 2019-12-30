using Shop.common.Models;
using Shop.common.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;

namespace Shop.UIForms.ViewModels
{
    public class ProductViewModel : BaseViewModel
    {
        private ApiService apiService;
        private ObservableCollection<Product> products;

        public ObservableCollection<Product> Products
        {
            get => products;
            set 
            {
                if (products != value)
                {
                    products = value;
                    NotifyPropertyChanged();
                }
            }

        }

        public ProductViewModel()
        {
            apiService = new ApiService();
            LoadProducts();

            
        }

        private async void LoadProducts()
        {
            var response = await apiService.GetListAsync<Product>("http://192.168.0.10:555/", "/api", "/Products");

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "Accept");

                return;
            }

            var myProducts = (List<Product>)response.Result;
          
             Products =  new ObservableCollection<Product>(myProducts.OrderBy(p => p.Name));
           

              
            }
            
        }
    }

