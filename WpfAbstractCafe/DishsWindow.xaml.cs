using AbstractCafeService.BindingModel;
using AbstractCafeService.Interfaces;
using AbstractCafeService.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WpfAbstractCafe
{
    /// <summary>
    /// Логика взаимодействия для DishsWindow.xaml
    /// </summary>
    public partial class DishsWindow : Window
    {
        public DishsWindow()
        {
            InitializeComponent();
            Loaded += DishsWindow_Load;
        }

        private void DishsWindow_Load(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                List<DishViewModel> list = Task.Run(() => APIClient.GetRequestData<List<DishViewModel>>("api/Dish/GetList")).Result;
                if (list != null)
                {
                    dataGridView.ItemsSource = list;
                        dataGridView.Columns[0].Visibility = Visibility.Hidden;
                        dataGridView.Columns[1].Width = DataGridLength.Auto;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void buttonAdd_Click(object sender, RoutedEventArgs e)
        {
            var form = new DishWindow();
            if (form.ShowDialog() == true)
            {
                LoadData();
            }
        }

        private void buttonUpd_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridView.SelectedItem != null)
            {
                var form = new DishWindow();
                form.Id = ((DishViewModel)dataGridView.SelectedItem).Id;
                if (form.ShowDialog() == true)
                {
                    LoadData();
                }
            }
        }

        private void buttonDel_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridView.SelectedItem != null)
            {
                if (MessageBox.Show("Удалить запись", "Вопрос", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    int id = ((DishViewModel)dataGridView.SelectedItem).Id;
                    Task task = Task.Run(() => APIClient.PostRequestData("api/Dish/DelElement", new CustomerBindingModel { Id = id }));

                    task.ContinueWith((prevTask) => MessageBox.Show("Запись удалена. Обновите список", "Успех", MessageBoxButton.OK, MessageBoxImage.Information),
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
                }
            }
        }

        private void buttonRef_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
        }
    }
}
