using Flurl;
using Flurl.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace MultPlatProject
{
    public class NewAuthorViewModel : INotifyPropertyChanged
    {
        string _name;
        public event EventHandler AuthorAdded;
        public ICommand PostCommand { get; private set; }
        public event EventHandler<ErrorResponse> RequestFailed;
        public event PropertyChangedEventHandler PropertyChanged;
        
        public string Name
        {
            get => _name; 
            set
            {
                if (!Equals(_name, value))
                {
                    _name = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Name"));
                }
            }
        }

        private bool _isLoading = false;
        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                if (_isLoading != value)
                {
                    _isLoading = value;
                    PropertyChanged?.Invoke(this,
                        new PropertyChangedEventArgs(nameof(IsLoading)));
                }
            }
        }

        public NewAuthorViewModel()
        {
            async void execute()
            {
                try
                {
                    var author = new Author() { Name = _name };
                    var httpResponse = await
                        Constants.BaseServiceUrl
                                 .AppendPathSegment(typeof(Author).Name)
                                 .PostJsonAsync(author);

                    if (httpResponse.IsSuccessStatusCode)
                    {
                        AuthorAdded?.Invoke(this, new EventArgs());
                    }
                }
                catch (FlurlHttpException ex)
                {
                    var error = await ex.GetResponseJsonAsync<ErrorResponse>();
                    RequestFailed?.Invoke(this, error);
                }
            }

            PostCommand = new Command(execute);
        }
    }
}
