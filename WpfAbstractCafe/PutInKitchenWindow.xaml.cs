using AbstractCafeService.BindingModel;
using AbstractCafeService.Interfaces;
using AbstractCafeService.ViewModels;
using System;
using System.Collections.Generic;
using System.Windows;
using Unity;
using Unity.Attributes;

namespace WpfAbstractCafe
{
    /// <summary>
    /// Логика взаимодействия для PutInKitchenWindow.xaml
    /// </summary>
    public partial class PutInKitchenWindow : Window
    {
        [Dependency]
        public IUnityContainer Container { get; set; }

        private readonly IKitchenService serviceS;

        private readonly IDishService serviceC;

        private readonly IMainService serviceM;

        public PutInKitchenWindow(IKitchenService serviceS, IDishService serviceC, IMainService serviceM)
        {
            InitializeComponent();
            this.serviceS = serviceS;
            this.serviceC = serviceC;
            this.serviceM = serviceM;
            Loaded += PutInKitchenWindow_Load;
        }

        private void PutInKitchenWindow_Load(object sender, RoutedEventArgs e)
        {
            try
            {
                List<DishViewModel> listC = serviceC.GetList();
                if (listC != null)
                {
                    comboBoxDish.DisplayMemberPath = "DishName";
                    comboBoxDish.SelectedValuePath = "Id";
                    comboBoxDish.ItemsSource = listC;
                    comboBoxDish.SelectedItem = null;
                }
                List<KitchenViewModel> listS = serviceS.GetList();
                if (listS != null)
                {
                    comboBoxKitchen.DisplayMemberPath = "KitchenName";
                    comboBoxKitchen.SelectedValuePath = "Id";
                    comboBoxKitchen.ItemsSource = listS;
                    comboBoxKitchen.SelectedItem = null;
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
                serviceM.PutDishOnKitchen(new KitchenDishBindingModel
                {
                    DishId = Convert.ToInt32(comboBoxDish.SelectedValue),
                    KitchenId = Convert.ToInt32(comboBoxKitchen.SelectedValue),
                    Count = Convert.ToInt32(textBoxCount.Text)
                });
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
