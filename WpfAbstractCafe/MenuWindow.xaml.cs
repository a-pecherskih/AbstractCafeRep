using AbstractCafeService.BindingModel;
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
    /// Логика взаимодействия для MenuWindow.xaml
    /// </summary>
    public partial class MenuWindow : Window
    {
        [Dependency]
        public IUnityContainer Container { get; set; }

        public int Id { set { id = value; } }

        private readonly IMenuService service;

        private int? id;

        private List<MenuDishViewModel> MenuDishs;

        public MenuWindow(IMenuService service)
        {
            InitializeComponent();
            this.service = service;
            Loaded += MenuWindow_Load;
        }

        private void MenuWindow_Load(object sender, RoutedEventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    MenuViewModel view = service.GetElement(id.Value);
                    if (view != null)
                    {
                        textBoxName.Text = view.MenuName;
                        textBoxPrice.Text = view.Price.ToString();
                        MenuDishs = view.MenuDishs;
                        LoadData();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MenuDishs = new List<MenuDishViewModel>();
            }
        }

        private void LoadData()
        {
            try
            {
                if (MenuDishs != null)
                {
                    dataGridView.ItemsSource = null;
                    dataGridView.ItemsSource = MenuDishs;
                    dataGridView.Columns[0].Visibility = Visibility.Hidden;
                    dataGridView.Columns[1].Visibility = Visibility.Hidden;
                    dataGridView.Columns[2].Visibility = Visibility.Hidden;
                    dataGridView.Columns[3].Width = DataGridLength.Auto;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void buttonAdd_Click(object sender, RoutedEventArgs e)
        {
            var form = Container.Resolve<MenuDishWindow>();
            if (form.ShowDialog() == true)
            {
                if (form.Model != null)
                {
                    if (id.HasValue)
                    {
                        form.Model.MenuId = id.Value;
                    }
                    MenuDishs.Add(form.Model);
                }
                LoadData();
            }
        }

        private void buttonUpd_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridView.SelectedItem != null)
            {
                var form = Container.Resolve<MenuDishWindow>();
                form.Model = MenuDishs[dataGridView.SelectedIndex];
                if (form.ShowDialog() == true)
                {
                    MenuDishs[dataGridView.SelectedIndex] = form.Model;
                    LoadData();
                }
            }
        }

        private void buttonDel_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridView.SelectedItem != null)
            {
                if (MessageBox.Show("Удалить запись", "Вопрос", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        MenuDishs.RemoveAt(dataGridView.SelectedIndex);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    LoadData();
                }
            }
        }

        private void buttonRef_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxName.Text))
            {
                MessageBox.Show("Заполните название", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrEmpty(textBoxPrice.Text))
            {
                MessageBox.Show("Заполните цену", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (MenuDishs == null || MenuDishs.Count == 0)
            {
                MessageBox.Show("Заполните компоненты", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                List<MenuDishBindingModel> MenuDishBM = new List<MenuDishBindingModel>();
                for (int i = 0; i < MenuDishs.Count; ++i)
                {
                    MenuDishBM.Add(new MenuDishBindingModel
                    {
                        Id = MenuDishs[i].Id,
                        MenuId = MenuDishs[i].MenuId,
                        DishId = MenuDishs[i].DishId,
                        Count = MenuDishs[i].Count
                    });
                }
                if (id.HasValue)
                {
                    service.UpdElement(new MenuBindingModel
                    {
                        Id = id.Value,
                        MenuName = textBoxName.Text,
                        Price = Convert.ToInt32(textBoxPrice.Text),
                        MenuDishs = MenuDishBM
                    });
                }
                else
                {
                    service.AddElement(new MenuBindingModel
                    {
                        MenuName = textBoxName.Text,
                        Price = Convert.ToInt32(textBoxPrice.Text),
                        MenuDishs = MenuDishBM
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
