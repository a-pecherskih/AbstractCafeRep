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
    /// Логика взаимодействия для CreateChoiceWindow.xaml
    /// </summary>
    public partial class CreateChoiceWindow : Window
    {

        public CreateChoiceWindow()
        {
            InitializeComponent();
            Loaded += CreateChoiceWindow_Load;
        }

        private void CalcSum()
        {
            if (comboBoxMenu.SelectedValue != null && !string.IsNullOrEmpty(textBoxCount.Text))
            {
                try
                {
                    int id = Convert.ToInt32(comboBoxMenu.SelectedValue);
                    MenuViewModel menu = Task.Run(() => APIClient.GetRequestData<MenuViewModel>("api/Menu/Get/" + id)).Result;
                    int count = Convert.ToInt32(textBoxCount.Text);
                    textBoxSum.Text = (count * (int)menu.Price).ToString();
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

        private void comboBoxMenu_SelectedIndexChanged(object sender, SelectionChangedEventArgs e)
        {
            CalcSum();
        }

        private void CreateChoiceWindow_Load(object sender, RoutedEventArgs e)
        {
            try
            {
                List<CustomerViewModel> listC = Task.Run(() => APIClient.GetRequestData<List<CustomerViewModel>>("api/Customer/GetList")).Result;
                if (listC != null)
                {
                    comboBoxCustomer.DisplayMemberPath = "CustomerFIO";
                        comboBoxCustomer.SelectedValuePath = "Id";
                        comboBoxCustomer.ItemsSource = listC;
                        comboBoxCustomer.SelectedItem = null;
                }

                List<MenuViewModel> listM = Task.Run(() => APIClient.GetRequestData<List<MenuViewModel>>("api/Menu/GetList")).Result;
                if (listM != null)
                {
                    comboBoxMenu.DisplayMemberPath = "MenuName";
                        comboBoxMenu.SelectedValuePath = "Id";
                        comboBoxMenu.ItemsSource = listM;
                        comboBoxMenu.SelectedItem = null;
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
            if (comboBoxCustomer.SelectedValue == null)
            {
                MessageBox.Show("Выберите клиента", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (comboBoxMenu.SelectedValue == null)
            {
                MessageBox.Show("Выберите меню", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            int customerId = Convert.ToInt32(comboBoxCustomer.SelectedValue);
            int menuId = Convert.ToInt32(comboBoxMenu.SelectedValue);
            int count = Convert.ToInt32(textBoxCount.Text);
            int sum = Convert.ToInt32(textBoxSum.Text);
            Task task = Task.Run(() => APIClient.PostRequestData("api/Main/CreateChoice", new ChoiceBindingModel
            {
                CustomerId = customerId,
                MenuId = menuId,
                Count = count,
                Sum = sum
            }));

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

        private void textBoxSum_TextChanged(object sender, TextChangedEventArgs e)
        {
            CalcSum();
        }

        private void textBoxCount_TextChanged(object sender, TextChangedEventArgs e)
        {
            CalcSum();
        }
    }
}
