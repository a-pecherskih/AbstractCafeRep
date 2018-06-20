using AbstractCafeService.BindingModel;
using AbstractCafeService.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;

namespace WpfAbstractCafe
{
    /// <summary>
    /// Логика взаимодействия для TakeChoiceInWorkWindow.xaml
    /// </summary>
    public partial class TakeChoiceInWorkWindow : Window
    {
        public int Id { set { id = value; } }

        private int? id;

        public TakeChoiceInWorkWindow()
        {
            InitializeComponent();
            Loaded += TakeChoiceInWorkWindow_Load;
        }

        private void TakeChoiceInWorkWindow_Load(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!id.HasValue)
                {
                    MessageBox.Show("Не указан заказ", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    Close();
                }
                List<ChefViewModel> list = Task.Run(() => APIClient.GetRequestData<List<ChefViewModel>>("api/Chef/GetList")).Result;
                if (list != null)
                {
                    comboBoxChef.DisplayMemberPath = "ChefFIO";
                    comboBoxChef.SelectedValuePath = "Id";
                    comboBoxChef.ItemsSource = list;
                    comboBoxChef.SelectedItem = null;
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

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            if (comboBoxChef.SelectedValue == null)
            {
                MessageBox.Show("Выберите исполнителя", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                int chefId = Convert.ToInt32(comboBoxChef.SelectedValue);
                Task task = Task.Run(() => APIClient.PostRequestData("api/Main/TakeChoiceInWork", new ChoiceBindingModel
                {
                    Id = id.Value,
                    ChefId = chefId
                }));

                task.ContinueWith((prevTask) => MessageBox.Show("Заказ передан в работу. Обновите список", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information),
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

                Close();
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

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
