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
                    var chef = Task.Run(() => APIClient.GetRequestData<ChefViewModel>("api/Chef/Get/" + id.Value)).Result;
                    TextBoxFIO.Text = chef.ChefFIO;
                }
                catch (Exception ex)
                {
                    while (ex.InnerException != null)
                    {
                        ex = ex.InnerException;
                    }
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
            string fio = TextBoxFIO.Text;
            Task task;
            if (id.HasValue)
            {
                task = Task.Run(() => APIClient.PostRequestData("api/Chef/UpdElement", new ChefBindingModel
                {
                    Id = id.Value,
                    ChefFIO = fio
                }));
            }
            else
            {
                task = Task.Run(() => APIClient.PostRequestData("api/Chef/AddElement", new ChefBindingModel
                {
                    ChefFIO = fio
                }));
            }
            task.ContinueWith((prevTask) => MessageBox.Show("Сохранение прошло успешно. Обновите список", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information),
                TaskContinuationOptions.OnlyOnRanToCompletion);
            task.ContinueWith((prevTask) =>
            {
                var ex = (Exception)prevTask.Exception;
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }, TaskContinuationOptions.OnlyOnFaulted);

            Close();
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
