using AbstractCafeService.BindingModel;
using AbstractCafeService.ViewModels;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
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
                var response = APIClient.GetRequest("api/Report/GetKitchensLoad");
                if (response.Result.IsSuccessStatusCode)
                {
                    dataGridView.Items.Clear();
                    foreach (var elem in APIClient.GetElement<List<KitchensLoadViewModel>>(response))
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

        private void buttonSaveToExcel_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "xls|*.xls|xlsx|*.xlsx"
            };
            if (sfd.ShowDialog() == true)
            {
                try
                {
                    var response = APIClient.PostRequest("api/Report/SaveKitchensLoad", new ReportBindingModel
                    {
                        FileName = sfd.FileName
                    });
                    if (response.Result.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Выполнено", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
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
        }
    }
}
