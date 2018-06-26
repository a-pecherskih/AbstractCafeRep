using AbstractCafeService.BindingModel;
using AbstractCafeService.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WpfAbstractCafe
{
    /// <summary>
    /// Логика взаимодействия для ChefsWindow.xaml
    /// </summary>
    public partial class ChefsWindow : Window
    {
        public ChefsWindow()
        {
            InitializeComponent();
            this.Loaded += ChefsWindow_Load;
        }

        private void ChefsWindow_Load(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                List<ChefViewModel> list = Task.Run(() => APIClient.GetRequestData<List<ChefViewModel>>("api/Chef/GetList")).Result;
                if (list != null)
                {
                    dataGridView.ItemsSource = list;
                    dataGridView.Columns[0].Visibility = Visibility.Hidden;
                    dataGridView.Columns[1].Width = DataGridLength.Auto;
                }
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

        private void buttonAdd_Click(object sender, RoutedEventArgs e)
        {
            var form = new ChefWindow();
            if (form.ShowDialog() == true)
            {
                LoadData();
            }
        }

        private void buttonUpd_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridView.SelectedItem != null)
            {
                var form = new ChefWindow();
                form.Id = ((ChefViewModel)dataGridView.SelectedItem).Id;
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
                    int id = ((ChefViewModel)dataGridView.SelectedItem).Id;
                    Task task = Task.Run(() => APIClient.PostRequestData("api/Chef/DelElement", new CustomerBindingModel { Id = id }));

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
