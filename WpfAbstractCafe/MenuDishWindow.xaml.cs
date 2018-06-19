using AbstractCafeService.Interfaces;
using AbstractCafeService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Unity;
using Unity.Attributes;

namespace WpfAbstractCafe
{
    /// <summary>
    /// Логика взаимодействия для MenuDishWindow.xaml
    /// </summary>
    public partial class MenuDishWindow : Window
    {
        [Dependency]
        public IUnityContainer Container { get; set; }

        public MenuDishViewModel Model { set { model = value; } get { return model; } }

        private readonly IDishService service;

        private MenuDishViewModel model;

        public MenuDishWindow(IDishService service)
        {
            InitializeComponent();
            this.service = service;
            Loaded += MenuDishWindow_Load;
        }

        private void MenuDishWindow_Load(object sender, RoutedEventArgs e)
        {
            try
            {
                List<DishViewModel> list = service.GetList();
                if (list != null)
                {
                    comboBoxDish.DisplayMemberPath = "DishName";
                    comboBoxDish.SelectedValuePath = "Id";
                    comboBoxDish.ItemsSource = list;
                    comboBoxDish.SelectedItem = null;
                }
            }
            catch (Exception ex)
            {
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
