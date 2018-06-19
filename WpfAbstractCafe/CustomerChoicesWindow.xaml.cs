using AbstractCafeService.BindingModel;
using AbstractCafeService.Interfaces;
using Microsoft.Reporting.WinForms;
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Windows;
using Unity;
using Unity.Attributes;

namespace WpfAbstractCafe
{
    /// <summary>
    /// Логика взаимодействия для CustomerChoicesWindow.xaml
    /// </summary>
    public partial class CustomerChoicesWindow : Window
    {
        [Dependency]
        public IUnityContainer Container { get; set; }

        private readonly IReportService service;

        private bool _isReportViewerLoaded;

        public CustomerChoicesWindow(IReportService service)
        {
            InitializeComponent();
            reportViewer.Load += FormCustomerChoices_Load;
            this.service = service;
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

        private void buttonMake_Click(object sender, RoutedEventArgs e)
        {
            if (dateTimePickerFrom.SelectedDate.Value.Date >= dateTimePickerTo.SelectedDate.Value.Date)
            {
                System.Windows.MessageBox.Show("Дата начала должна быть меньше даты окончания", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                ReportParameter parameter = new ReportParameter("ReportParameterPeriod",
                                            "c " + dateTimePickerFrom.SelectedDate.Value.Date.ToShortDateString() +
                                            " по " + dateTimePickerTo.SelectedDate.Value.Date.ToShortDateString());
                reportViewer.LocalReport.SetParameters(parameter);

                var dataSource = service.GetCustomerChoices(new ReportBindingModel
                {
                    DateFrom = dateTimePickerFrom.SelectedDate.Value,
                    DateTo = dateTimePickerTo.SelectedDate.Value
                });
                ReportDataSource source = new ReportDataSource("DataSetChoices", dataSource);
                reportViewer.LocalReport.DataSources.Add(source);

                reportViewer.RefreshReport();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void buttonToPdf_Click(object sender, RoutedEventArgs e)
        {
            if (dateTimePickerFrom.SelectedDate.Value.Date >= dateTimePickerTo.SelectedDate.Value.Date)
            {
                System.Windows.MessageBox.Show("Дата начала должна быть меньше даты окончания", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "pdf|*.pdf"
            };
            if (sfd.ShowDialog() == true)
            {
                try
                {
                    service.SaveCustomerChoices(new ReportBindingModel
                    {
                        FileName = sfd.FileName,
                        DateFrom = dateTimePickerFrom.SelectedDate.Value,
                        DateTo = dateTimePickerTo.SelectedDate.Value
                    });
                    System.Windows.MessageBox.Show("Выполнено", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
