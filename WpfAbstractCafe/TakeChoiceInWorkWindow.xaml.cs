using AbstractCafeService.BindingModel;
using AbstractCafeService.ViewModels;
using System;
using System.Collections.Generic;
using System.Windows;

namespace WpfAbstractCafe
{
    /// <summary>
    /// Логика взаимодействия для TakeChoiceInWorkWindow.xaml
    /// </summary>
    public partial class TakeChoiceInWorkWindow : Window
    {
        public int Id { set { id = value; } }

        private int? id;

        public TakeChoiceInWorkWindow()
        {
            InitializeComponent();
            Loaded += TakeChoiceInWorkWindow_Load;
        }

        private void TakeChoiceInWorkWindow_Load(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!id.HasValue)
                {
                    MessageBox.Show("Не указан заказ", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    Close();
                }
                var response = APIClient.GetRequest("api/Chef/GetList");
                if (response.Result.IsSuccessStatusCode)
                {
                    List<ChefViewModel> list = APIClient.GetElement<List<ChefViewModel>>(response);
                    if (list != null)
                    {
                        comboBoxChef.DisplayMemberPath = "ChefFIO";
                        comboBoxChef.SelectedValuePath = "Id";
                        comboBoxChef.ItemsSource = list;
                        comboBoxChef.SelectedItem = null;
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

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            if (comboBoxChef.SelectedValue == null)
            {
                MessageBox.Show("Выберите исполнителя", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                var response = APIClient.PostRequest("api/Main/TakeChoiceInWork", new ChoiceBindingModel
                {
                    Id = id.Value,
                    ChefId = Convert.ToInt32(comboBoxChef.SelectedValue)
                });
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
