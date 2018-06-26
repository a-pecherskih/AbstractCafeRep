using AbstractCafeService.BindingModel;
using AbstractCafeService.ViewModels;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WpfAbstractCafe
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoadData()
        {
            try
            {
                List<ChoiceViewModel> list = Task.Run(() => APIClient.GetRequestData<List<ChoiceViewModel>>("api/Main/GetList")).Result;
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
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void buttonCreateChoice_Click(object sender, RoutedEventArgs e)
        {
            var form = new CreateChoiceWindow();
            form.ShowDialog();
            LoadData();
        }

        private void buttonTakeChoiceInWork_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridView.SelectedItem != null)
            {
                var form = new TakeChoiceInWorkWindow();
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
                Task task = Task.Run(() => APIClient.PostRequestData("api/Main/FinishChoice", new ChoiceBindingModel
                {
                    Id = id
                }));

                task.ContinueWith((prevTask) => MessageBox.Show("Статус заказа изменен. Обновите список", "Успех", MessageBoxButton.OK, MessageBoxImage.Information),
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
            }
        }

        private void buttonPayChoice_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridView.SelectedItem != null)
            {
                int id = ((ChoiceViewModel)dataGridView.SelectedItem).Id;
                Task task = Task.Run(() => APIClient.PostRequestData("api/Main/PayChoice", new ChoiceBindingModel
                {
                    Id = id
                }));

                task.ContinueWith((prevTask) => MessageBox.Show("Статус заказа изменен. Обновите список", "Успех", MessageBoxButton.OK, MessageBoxImage.Information),
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
            }
        }

        private void buttonRef_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void пополнитьСкладToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var form = new PutInKitchenWindow();
            form.ShowDialog();
        }

        private void заказчикиToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var form = new CustomersWindow();
            form.ShowDialog();
        }

        private void блюдаToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var form = new DishsWindow();
            form.ShowDialog();
        }

        private void менюToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var form = new MenusWindow();
            form.ShowDialog();
        }

        private void кухниToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var form = new KitchensWindow();
            form.ShowDialog();
        }

        private void шефыToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var form = new ChefsWindow();
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
                string fileName = sfd.FileName;
                Task task = Task.Run(() => APIClient.PostRequestData("api/Report/SaveMenuPrice", new ReportBindingModel
                {
                    FileName = fileName
                }));

                task.ContinueWith((prevTask) => MessageBox.Show("Выполнено", "Успех", MessageBoxButton.OK, MessageBoxImage.Information),
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
            }
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            var form = new KitchenLoadWindow();
            form.ShowDialog();
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            var form = new CustomerChoicesWindow();
            form.ShowDialog();
        }
    }
}
