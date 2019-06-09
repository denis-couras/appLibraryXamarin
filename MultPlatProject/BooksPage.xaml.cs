using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MultPlatProject
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BooksPage : ContentPage
    {
        public BooksPage()
        {
            InitializeComponent();
        }

        // Executado toda vez que a tela for exibida.
        protected override void OnAppearing()
        {
            base.OnAppearing();
            (BindingContext as BooksViewModel).GetCommand.Execute(null);
        }

        void Handle_RequestFailed(object sender, MultPlatProject.ErrorResponse e)
        {
            DisplayAlert("Erro", e.Message, "ok");
        }
    }
}