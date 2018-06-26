using AbstractCafeService.BindingModel;
using Microsoft.Reporting.WinForms;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;

namespace WpfAbstractCafe
{
    /// <summary>
    /// Логика взаимодействия для CustomerChoicesWindow.xaml
    /// </summary>
    public partial class CustomerChoicesWindow : Window
    {
        private bool _isReportViewerLoaded;

        public CustomerChoicesWindow()
        {
            InitializeComponent();
            reportViewer.Load += FormCustomerChoices_Load;
        }

        private void FormCustomerChoices_Load(object sender, EventArgs e)
        {
            if (!_isReportViewerLoaded)
            {
                string exeFolder = System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
                reportViewer.LocalReport.ReportPath = exeFolder + @"..\..\..\ReportCustomerChoices.rdlc";
                reportViewer.RefreshReport();
                _isReportViewerLoaded = true;
            }
        }

        private void buttonMake_Click(object sender, EventArgs e)
        {
            if (dateTimePickerFrom.SelectedDate.Value.Date >= dateTimePickerTo.SelectedDate.Value.Date)
            {
                MessageBox.Show("Дата начала должна быть меньше даты окончания", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                ReportParameter parameter = new ReportParameter("ReportParameterPeriod",
                                            "c " + dateTimePickerFrom.SelectedDate.Value.ToShortDateString() +
                                            " по " + dateTimePickerTo.SelectedDate.Value.ToShortDateString());
                reportViewer.LocalReport.SetParameters(parameter);

                var dataSource = Task.Run(() => APIClient.PostRequestData<ReportBindingModel, List<CustomerChoicesModel>>("api/Report/GetCustomerChoices",
                   new ReportBindingModel
                   {
                       DateFrom = dateTimePickerFrom.SelectedDate.Value,
                       DateTo = dateTimePickerTo.SelectedDate.Value
                   })).Result;
                ReportDataSource source = new ReportDataSource("DataSetChoices", dataSource);
                reportViewer.LocalReport.DataSources.Add(source);

                reportViewer.RefreshReport();
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

        private void buttonToPdf_Click(object sender, EventArgs e)
        {
            if (dateTimePickerFrom.SelectedDate.Value.Date >= dateTimePickerTo.SelectedDate.Value.Date)
            {
                MessageBox.Show("Дата начала должна быть меньше даты окончания", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "pdf|*.pdf"
            };
            if (sfd.ShowDialog() == true)
            {
                string fileName = sfd.FileName;
                Task task = Task.Run(() => APIClient.PostRequestData("api/Report/SaveCustomerChoices", new ReportBindingModel
                {
                    FileName = fileName,
                    DateFrom = dateTimePickerFrom.SelectedDate.Value,
                    DateTo = dateTimePickerTo.SelectedDate.Value
                }));

                task.ContinueWith((prevTask) => MessageBox.Show("Список заказов сохранен", "Успех", MessageBoxButton.OK, MessageBoxImage.Information),
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
