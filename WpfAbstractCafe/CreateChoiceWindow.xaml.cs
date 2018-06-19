using AbstractCafeService.BindingModel;
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
    /// Логика взаимодействия для CreateChoiceWindow.xaml
    /// </summary>
    public partial class CreateChoiceWindow : Window
    {
        [Dependency]
        public IUnityContainer Container { get; set; }

        private readonly ICustomerService serviceC;

        private readonly IMenuService serviceP;

        private readonly IMainService serviceM;

        public CreateChoiceWindow(ICustomerService serviceC, IMenuService serviceP, IMainService serviceM)
        {
            InitializeComponent();
            this.serviceC = serviceC;
            this.serviceP = serviceP;
            this.serviceM = serviceM;
            Loaded += CreateChoiceWindow_Load;
            comboBoxMenu.SelectionChanged += comboBoxMenu_SelectedIndexChanged;
            comboBoxMenu.SelectionChanged += new SelectionChangedEventHandler(comboBoxMenu_SelectedIndexChanged);
        }

        private void CalcSum()
        {
            if (comboBoxMenu.SelectedValue != null && !string.IsNullOrEmpty(textBoxCount.Text))
            {
                try
                {
                    int id = Convert.ToInt32(comboBoxMenu.SelectedValue);
                    MenuViewModel menu = serviceP.GetElement(id);
                    int count = Convert.ToInt32(textBoxCount.Text);
                    textBoxSum.Text = Convert.ToInt32(count * menu.Price).ToString();
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
                List<CustomerViewModel> listC = serviceC.GetList();
                if (listC != null)
                {
                    comboBoxCustomer.DisplayMemberPath = "CustomerFIO";
                    comboBoxCustomer.SelectedValuePath = "Id";
                    comboBoxCustomer.ItemsSource = listC;
                    comboBoxCustomer.SelectedItem = null;
                }
                List<MenuViewModel> listP = serviceP.GetList();
                if (listP != null)
                {
                    comboBoxMenu.DisplayMemberPath = "MenuName";
                    comboBoxMenu.SelectedValuePath = "Id";
                    comboBoxMenu.ItemsSource = listP;
                    comboBoxMenu.SelectedItem = null;
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
                serviceM.CreateChoice(new ChoiceBindingModel
                {
                    CustomerId = Convert.ToInt32(comboBoxCustomer.SelectedValue),
                    MenuId = Convert.ToInt32(comboBoxMenu.SelectedValue),
                    Count = Convert.ToInt32(textBoxCount.Text),
                    Sum = Convert.ToInt32(textBoxSum.Text)
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
