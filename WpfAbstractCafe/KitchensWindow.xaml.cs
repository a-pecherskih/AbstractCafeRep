using AbstractCafeService.Interfaces;
using System.Windows;
using System;
using AbstractCafeService.ViewModels;
using System.Windows.Controls;
using System.Collections.Generic;
using AbstractCafeService.BindingModel;

namespace WpfAbstractCafe
{
    /// <summary>
    /// Логика взаимодействия для KitchensWindow.xaml
    /// </summary>
    public partial class KitchensWindow : Window
    {
        public KitchensWindow()
        {
            InitializeComponent();
            Loaded += KitchensWindow_Load;
        }

        private void KitchensWindow_Load(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                var response = APIClient.GetRequest("api/Kitchen/GetList");
                if (response.Result.IsSuccessStatusCode)
                {
                    List<KitchenViewModel> list = APIClient.GetElement<List<KitchenViewModel>>(response);
                    if (list != null)
                    {
                        dataGridView.ItemsSource = list;
                        dataGridView.Columns[0].Visibility = Visibility.Hidden;
                        dataGridView.Columns[1].Width = DataGridLength.Auto;
                    }
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

        private void buttonAdd_Click(object sender, RoutedEventArgs e)
        {
            var form = new KitchenWindow();
            if (form.ShowDialog() == true)
            {
                LoadData();
            }
        }

        private void buttonUpd_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridView.SelectedItem != null)
            {
                var form = new KitchenWindow();
                form.Id = ((KitchenViewModel)dataGridView.SelectedItem).Id;
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
                    int id = ((KitchenViewModel)dataGridView.SelectedItem).Id;
                    try
                    {
                        var response = APIClient.PostRequest("api/Kitchen/DelElement", new CustomerBindingModel { Id = id });
                        if (!response.Result.IsSuccessStatusCode)
                        {
                            throw new Exception(APIClient.GetError(response));
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    LoadData();
                }
            }
        }

        private void buttonRef_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
        }
    }
}
