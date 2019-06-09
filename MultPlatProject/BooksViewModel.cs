using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Flurl;
using Flurl.Http;
using Xamarin.Forms;

namespace MultPlatProject
{
    class BooksViewModel : BaseViewModel, INotifyPropertyChanged
    {
        public event EventHandler<ErrorResponse> RequestFailed;
        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand GetCommand { get; private set; }

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

        IEnumerable<Book> mBooks;
        public IEnumerable<Book> Books
        {
            get => mBooks;
            set
            { 
                if (!Equals(mBooks, value))
                {
                    mBooks = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Books"));
                }
            }
        }

        public BooksViewModel()
        {
            async void execute(object parameter)
            {
                try
                {
                    IsLoading = true;
                    Books = await
                        Constants.BaseServiceUrl
                                 .AppendPathSegment(typeof(Book).Name)
                                 .GetJsonAsync<List<Book>>();

                    if (parameter != null)
                    {
                        Entry txt = parameter as Entry;
                        Books = Books.Where(x => x.Author.Name.ToLower().Contains(txt.Text.ToLower()));
                    }

                    IsLoading = false;
                }
                
                catch (FlurlHttpException ex)
                {
                    IsLoading = false;
                    var error = await ex.GetResponseJsonAsync<ErrorResponse>();  
                    RequestFailed?.Invoke(this, error);
                }

               
            }
 
            GetCommand = new Command(execute);
        }
    }
}
