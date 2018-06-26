using AbstractCafeService.BindingModel;
using AbstractCafeService.ViewModels;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;

namespace WpfAbstractCafe
{
    /// <summary>
    /// Логика взаимодействия для KitchenLoadWindow.xaml
    /// </summary>
    public partial class KitchenLoadWindow : Window
    {
        public KitchenLoadWindow()
        {
            InitializeComponent();
            Loaded += KitchenLoadWindow_Load;
        }

        private void KitchenLoadWindow_Load(object sender, EventArgs e)
        {
            try
            {
                dataGridView.Items.Clear();
                foreach (var elem in Task.Run(() => APIClient.GetRequestData<List<KitchensLoadViewModel>>("api/Report/GetKitchensLoad")).Result)
                {
                    dataGridView.Items.Add(new object[] { elem.KitchenName, "", "" });
                    foreach (var listElem in elem.Dishs)
                    {
                        dataGridView.Items.Add(new object[] { "", listElem.DishName, listElem.Count });
                    }
                    dataGridView.Items.Add(new object[] { "Итого", "", elem.TotalCount });
                    dataGridView.Items.Add(new object[] { });
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

        private void buttonSaveToExcel_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "xls|*.xls|xlsx|*.xlsx"
            };
            if (sfd.ShowDialog() == true)
            {
                string fileName = sfd.FileName;
                Task task = Task.Run(() => APIClient.PostRequestData("api/Report/SaveKitchensLoad", new ReportBindingModel
                {
                    FileName = fileName
                }));

                task.ContinueWith((prevTask) => MessageBox.Show("Выполнено", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information),
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
    }
}
