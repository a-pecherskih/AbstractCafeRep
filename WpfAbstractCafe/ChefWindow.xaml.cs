using AbstractCafeService.BindingModel;
using AbstractCafeService.ViewModels;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;

namespace WpfAbstractCafe
{
    /// <summary>
    /// Логика взаимодействия для ChefWindow.xaml
    /// </summary>
    public partial class ChefWindow : Window
    {
        public int Id { set { id = value; } }

        private int? id;

        public ChefWindow()
        {
            InitializeComponent();
            Loaded += ChefWindow_Load;
        }

        private void ChefWindow_Load(object sender, RoutedEventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    var response = APIClient.GetRequest("api/Chef/Get/" + id.Value);
                    if (response.Result.IsSuccessStatusCode)
                    {
                        var implementer = APIClient.GetElement<ChefViewModel>(response);
                        TextBoxFIO.Text = implementer.ChefFIO;
                    }
                    else
                    {
                        throw new Exception(APIClient.GetError(response));
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(TextBoxFIO.Text))
            {
                MessageBox.Show("Заполнить ФИО", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                Task<HttpResponseMessage> response;
                if (id.HasValue)
                {
                    response = APIClient.PostRequest("api/Chef/UpdElement", new ChefBindingModel
                    {
                        Id = id.Value,
                        ChefFIO = TextBoxFIO.Text
                    });
                }
                else
                {
                    response = APIClient.PostRequest("api/Chef/AddElement", new ChefBindingModel
                    {
                        ChefFIO = TextBoxFIO.Text
                    });
                }
                if (response.Result.IsSuccessStatusCode)
                {
                    MessageBox.Show("Сохранение прошло успешно", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                    DialogResult = true;
                    Close();
                }
                else
                {
                    throw new Exception(APIClient.GetError(response));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
