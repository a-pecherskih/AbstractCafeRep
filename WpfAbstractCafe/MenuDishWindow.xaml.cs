using AbstractCafeService.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;

namespace WpfAbstractCafe
{
    /// <summary>
    /// Логика взаимодействия для MenuDishWindow.xaml
    /// </summary>
    public partial class MenuDishWindow : Window
    {
        public MenuDishViewModel Model { set { model = value; } get { return model; } }

        private MenuDishViewModel model;

        public MenuDishWindow()
        {
            InitializeComponent();
            Loaded += MenuDishWindow_Load;
        }

        private void MenuDishWindow_Load(object sender, RoutedEventArgs e)
        {
            try
            {
                comboBoxDish.DisplayMemberPath = "DishName";
                comboBoxDish.SelectedValuePath = "Id";
                comboBoxDish.ItemsSource = Task.Run(() => APIClient.GetRequestData<List<DishViewModel>>("api/Dish/GetList")).Result;
                comboBoxDish.SelectedItem = null;
            }
            catch (Exception ex)
            {
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            if (model != null)
            {
                comboBoxDish.IsEnabled = false;
                comboBoxDish.SelectedValue = model.DishId;
                textBoxCount.Text = model.Count.ToString();
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
                MessageBox.Show("Выберите блюдо", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                if (model == null)
                {
                    model = new MenuDishViewModel
                    {
                        DishId = Convert.ToInt32(comboBoxDish.SelectedValue),
                        DishName = comboBoxDish.Text,
                        Count = Convert.ToInt32(textBoxCount.Text)
                    };
                }
                else
                {
                    model.Count = Convert.ToInt32(textBoxCount.Text);
                }
                MessageBox.Show("Сохранение прошло успешно", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                DialogResult = true;
                Close();
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
