using AbstractCafeService.BindingModel;
using AbstractCafeService.ViewModels;
using System;
using System.Collections.Generic;
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
                    var responseP = APIClient.GetRequest("api/Menu/Get/" + id);
                    if (responseP.Result.IsSuccessStatusCode)
                    {
                        MenuViewModel menu = APIClient.GetElement<MenuViewModel>(responseP);
                        int count = Convert.ToInt32(textBoxCount.Text);
                        textBoxSum.Text = (count * (int)menu.Price).ToString();
                    }
                    else
                    {
                        throw new Exception(APIClient.GetError(responseP));
                    }
                }
                catch (Exception ex)
                {
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
                var responseC = APIClient.GetRequest("api/Customer/GetList");
                if (responseC.Result.IsSuccessStatusCode)
                {
                    List<CustomerViewModel> list = APIClient.GetElement<List<CustomerViewModel>>(responseC);
                    if (list != null)
                    {
                        comboBoxCustomer.DisplayMemberPath = "CustomerFIO";
                        comboBoxCustomer.SelectedValuePath = "Id";
                        comboBoxCustomer.ItemsSource = list;
                        comboBoxCustomer.SelectedItem = null;
                    }
                }
                else
                {
                    throw new Exception(APIClient.GetError(responseC));
                }
                var responseP = APIClient.GetRequest("api/Menu/GetList");
                if (responseP.Result.IsSuccessStatusCode)
                {
                    List<MenuViewModel> list = APIClient.GetElement<List<MenuViewModel>>(responseP);
                    if (list != null)
                    {
                        comboBoxMenu.DisplayMemberPath = "MenuName";
                        comboBoxMenu.SelectedValuePath = "Id";
                        comboBoxMenu.ItemsSource = list;
                        comboBoxMenu.SelectedItem = null;
                    }
                }
                else
                {
                    throw new Exception(APIClient.GetError(responseP));
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
            try
            {
                var response = APIClient.PostRequest("api/Main/CreateChoice", new ChoiceBindingModel
                {
                    CustomerId = Convert.ToInt32(comboBoxCustomer.SelectedValue),
                    MenuId = Convert.ToInt32(comboBoxMenu.SelectedValue),
                    Count = Convert.ToInt32(textBoxCount.Text),
                    Sum = Convert.ToInt32(textBoxSum.Text)
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
