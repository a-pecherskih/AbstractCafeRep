using AbstractCafeService.BindingModel;
using AbstractCafeService.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
                List<DishViewModel> listD = Task.Run(() => APIClient.GetRequestData<List<DishViewModel>>("api/Dish/GetList")).Result;
                if (listD != null)
                {
                    comboBoxDish.DisplayMemberPath = "DishName";
                    comboBoxDish.SelectedValuePath = "Id";
                    comboBoxDish.ItemsSource = listD;
                    comboBoxDish.SelectedItem = null;
                }
                List<KitchenViewModel> listK = Task.Run(() => APIClient.GetRequestData<List<KitchenViewModel>>("api/Kitchen/GetList")).Result;
                if (listK != null)
                {
                    comboBoxKitchen.DisplayMemberPath = "KitchenName";
                    comboBoxKitchen.SelectedValuePath = "Id";
                    comboBoxKitchen.ItemsSource = listK;
                    comboBoxKitchen.SelectedItem = null;
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
                int dishId = Convert.ToInt32(comboBoxDish.SelectedValue);
                int kitchenId = Convert.ToInt32(comboBoxKitchen.SelectedValue);
                int count = Convert.ToInt32(textBoxCount.Text);
                Task task = Task.Run(() => APIClient.PostRequestData("api/Main/PutDishOnKitchen", new KitchenDishBindingModel
                {
                    DishId = dishId,
                    KitchenId = kitchenId,
                    Count = count
                }));

                task.ContinueWith((prevTask) => MessageBox.Show("Кухня пополнена", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information),
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
            catch (Exception ex)
            {
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }
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
