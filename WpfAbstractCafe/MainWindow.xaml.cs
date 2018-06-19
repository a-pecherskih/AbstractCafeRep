using AbstractCafeService.BindingModel;
using AbstractCafeService.Interfaces;
using AbstractCafeService.ViewModels;
using Microsoft.Win32;
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
        private readonly IReportService reportService;

        public MainWindow(IMainService service, IReportService reportService)
        {
            InitializeComponent();
            this.service = service;
            this.reportService = reportService;
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

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "doc|*.doc|docx|*.docx"
            };
            if (sfd.ShowDialog() == true)
            {
                try
                {
                    reportService.SaveMenuPrice(new ReportBindingModel
                    {
                        FileName = sfd.FileName
                    });
                    MessageBox.Show("Выполнено", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            var form = Container.Resolve<KitchenLoadWindow>();
            form.ShowDialog();
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            var form = Container.Resolve<CustomerChoicesWindow>();
            form.ShowDialog();
        }
    }
}
