using AbstractCafeService.Interfaces;
using AbstractCafeService.ViewModels;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Unity;
using Unity.Attributes;

namespace WpfAbstractCafe
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        [Dependency]
        public IUnityContainer Container { get; set; }

        private readonly IMainService service;

        public MainWindow(IMainService service)
        {
            InitializeComponent();
            this.service = service;
        }

        private void LoadData()
        {
            try
            {
                List<ChoiceViewModel> list = service.GetList();
                if (list != null)
                {
                    dataGridView.ItemsSource = list;
                    dataGridView.Columns[0].Visibility = Visibility.Hidden;
                    dataGridView.Columns[1].Visibility = Visibility.Hidden;
                    dataGridView.Columns[3].Visibility = Visibility.Hidden;
                    dataGridView.Columns[5].Visibility = Visibility.Hidden;
                    dataGridView.Columns[1].Width = DataGridLength.Auto;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void buttonCreateChoice_Click(object sender, RoutedEventArgs e)
        {
            var form = Container.Resolve<CreateChoiceWindow>();
            form.ShowDialog();
            LoadData();
        }

        private void buttonTakeChoiceInWork_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridView.SelectedItem != null)
            {
                var form = Container.Resolve<TakeChoiceInWorkWindow>();
                form.Id = ((ChoiceViewModel)dataGridView.SelectedItem).Id;
                form.ShowDialog();
                LoadData();
            }
        }

        private void buttonChoiceReady_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridView.SelectedItem != null)
            {
                int id = ((ChoiceViewModel)dataGridView.SelectedItem).Id;
                try
                {
                    service.FinishChoice(id);
                    LoadData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void buttonPayChoice_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridView.SelectedItem != null)
            {
                int id = ((ChoiceViewModel)dataGridView.SelectedItem).Id;
                try
                {
                    service.PayChoice(id);
                    LoadData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void buttonRef_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void пополнитьСкладToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var form = Container.Resolve<PutInKitchenWindow>();
            form.ShowDialog();
        }

        private void заказчикиToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var form = Container.Resolve<CustomersWindow>();
            form.ShowDialog();
        }

        private void блюдаToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var form = Container.Resolve<DishsWindow>();
            form.ShowDialog();
        }

        private void менюToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var form = Container.Resolve<MenusWindow>();
            form.ShowDialog();
        }

        private void кухниToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var form = Container.Resolve<KitchensWindow>();
            form.ShowDialog();
        }

        private void шефыToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var form = Container.Resolve<ChefsWindow>();
            form.ShowDialog();
        }
    }
}
