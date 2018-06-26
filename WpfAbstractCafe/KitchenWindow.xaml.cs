using AbstractCafeService.BindingModel;
using AbstractCafeService.ViewModels;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WpfAbstractCafe
{
    /// <summary>
    /// Логика взаимодействия для KitchenWindow.xaml
    /// </summary>
    public partial class KitchenWindow : Window
    {
        public int Id { set { id = value; } }

        private int? id;

        public KitchenWindow()
        {
            InitializeComponent();
            Loaded += KitchenWindow_Load;
        }

        private void KitchenWindow_Load(object sender, RoutedEventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    var kitchen = Task.Run(() => APIClient.GetRequestData<KitchenViewModel>("api/Kitchen/Get/" + id.Value)).Result;
                    TextBoxName.Text = kitchen.KitchenName;
                    dataGridView.ItemsSource = kitchen.KitchenDishs;
                    dataGridView.Columns[0].Visibility = Visibility.Hidden;
                    dataGridView.Columns[1].Visibility = Visibility.Hidden;
                    dataGridView.Columns[2].Visibility = Visibility.Hidden;
                    dataGridView.Columns[3].Width = DataGridLength.Auto;
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
            if (string.IsNullOrEmpty(TextBoxName.Text))
            {
                MessageBox.Show("Заполните название", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            string name = TextBoxName.Text;
            Task task;
            if (id.HasValue)
            {
                task = Task.Run(() => APIClient.PostRequestData("api/Kitchen/UpdElement", new KitchenBindingModel
                {
                    Id = id.Value,
                    KitchenName = name
                }));
            }
            else
            {
                task = Task.Run(() => APIClient.PostRequestData("api/Kitchen/AddElement", new KitchenBindingModel
                {
                    KitchenName = name
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

        private void buttonСancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
