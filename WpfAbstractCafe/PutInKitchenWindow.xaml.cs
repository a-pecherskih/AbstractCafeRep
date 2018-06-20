using AbstractCafeService.BindingModel;
using AbstractCafeService.ViewModels;
using System;
using System.Collections.Generic;
using System.Windows;

namespace WpfAbstractCafe
{
    /// <summary>
    /// Логика взаимодействия для PutInKitchenWindow.xaml
    /// </summary>
    public partial class PutInKitchenWindow : Window
    {
        public PutInKitchenWindow()
        {
            InitializeComponent();
            Loaded += PutInKitchenWindow_Load;
        }

        private void PutInKitchenWindow_Load(object sender, RoutedEventArgs e)
        {
            try
            {
                var responseC = APIClient.GetRequest("api/Dish/GetList");
                if (responseC.Result.IsSuccessStatusCode)
                {
                    List<DishViewModel> list = APIClient.GetElement<List<DishViewModel>>(responseC);
                    if (list != null)
                    {
                        comboBoxDish.DisplayMemberPath = "DishName";
                        comboBoxDish.SelectedValuePath = "Id";
                        comboBoxDish.ItemsSource = list;
                        comboBoxDish.SelectedItem = null;
                    }
                }
                else
                {
                    throw new Exception(APIClient.GetError(responseC));
                }
                var responseS = APIClient.GetRequest("api/Kitchen/GetList");
                if (responseS.Result.IsSuccessStatusCode)
                {
                    List<KitchenViewModel> list = APIClient.GetElement<List<KitchenViewModel>>(responseS);
                    if (list != null)
                    {
                        comboBoxKitchen.DisplayMemberPath = "KitchenName";
                        comboBoxKitchen.SelectedValuePath = "Id";
                        comboBoxKitchen.ItemsSource = list;
                        comboBoxKitchen.SelectedItem = null;
                    }
                }
                else
                {
                    throw new Exception(APIClient.GetError(responseC));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxCount.Text))
            {
                MessageBox.Show("Заполните поле Количество", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (comboBoxDish.SelectedValue == null)
            {
                MessageBox.Show("Выберите компонент", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (comboBoxKitchen.SelectedValue == null)
            {
                MessageBox.Show("Выберите склад", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                var response = APIClient.PostRequest("api/Main/PutDishOnKitchen", new KitchenDishBindingModel
                {
                    DishId = Convert.ToInt32(comboBoxDish.SelectedValue),
                    KitchenId = Convert.ToInt32(comboBoxKitchen.SelectedValue),
                    Count = Convert.ToInt32(textBoxCount.Text)
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
