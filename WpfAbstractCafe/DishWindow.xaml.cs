using AbstractCafeService.BindingModel;
using AbstractCafeService.Interfaces;
using AbstractCafeService.ViewModels;
using System;
using System.Windows;
using Unity;
using Unity.Attributes;

namespace WpfAbstractCafe
{
    /// <summary>
    /// Логика взаимодействия для DishWindow.xaml
    /// </summary>
    public partial class DishWindow : Window
    {
        [Dependency]
        public IUnityContainer Container { get; set; }

        public int Id { set { id = value; } }

        private readonly IDishService service;

        private int? id;

        public DishWindow(IDishService service)
        {
            InitializeComponent();
            this.service = service;
            Loaded += DishWindow_Load;
        }

        private void DishWindow_Load(object sender, RoutedEventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    DishViewModel view = service.GetElement(id.Value);
                    if (view != null)
                    {
                        TextBoxName.Text = view.DishName;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(TextBoxName.Text))
            {
                MessageBox.Show("Введите название", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                if (id.HasValue)
                {
                    service.UpdElement(new DishBindingModel
                    {
                        Id = id.Value,
                        DishName = TextBoxName.Text
                    });
                }
                else
                {
                    service.AddElement(new DishBindingModel
                    {
                        DishName = TextBoxName.Text
                    });
                }
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
