using AbstractCafeService.BindingModel;
using AbstractCafeService.Interfaces;
using AbstractCafeService.ViewModels;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;

namespace WpfAbstractCafe
{
    /// <summary>
    /// Логика взаимодействия для DishWindow.xaml
    /// </summary>
    public partial class DishWindow : Window
    {
        public int Id { set { id = value; } }

        private int? id;

        public DishWindow()
        {
            InitializeComponent();
            Loaded += DishWindow_Load;
        }

        private void DishWindow_Load(object sender, RoutedEventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    var dish = Task.Run(() => APIClient.GetRequestData<DishViewModel>("api/Dish/Get/" + id.Value)).Result;
                    TextBoxName.Text = dish.DishName;
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
                MessageBox.Show("Введите название", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            string name = TextBoxName.Text;
            Task task;
            if (id.HasValue)
            {
                task = Task.Run(() => APIClient.PostRequestData("api/Dish/UpdElement", new DishBindingModel
                {
                    Id = id.Value,
                    DishName = name
                }));
            }
            else
            {
                task = Task.Run(() => APIClient.PostRequestData("api/Dish/AddElement", new DishBindingModel
                {
                    DishName = name
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
