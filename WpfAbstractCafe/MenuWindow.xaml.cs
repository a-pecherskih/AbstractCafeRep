﻿using AbstractCafeService.BindingModel;
using AbstractCafeService.ViewModels;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WpfAbstractCafe
{
    /// <summary>
    /// Логика взаимодействия для MenuWindow.xaml
    /// </summary>
    public partial class MenuWindow : Window
    {
        public int Id { set { id = value; } }

        private int? id;

        private List<MenuDishViewModel> MenuDishs;

        public MenuWindow()
        {
            InitializeComponent();
            Loaded += MenuWindow_Load;
        }

        private void MenuWindow_Load(object sender, RoutedEventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    var response = APIClient.GetRequest("api/Menu/Get/" + id.Value);
                    if (response.Result.IsSuccessStatusCode)
                    {
                        var menu = APIClient.GetElement<MenuViewModel>(response);
                        textBoxPrice.Text = menu.Price.ToString();
                        MenuDishs = menu.MenuDishs;
                        LoadData();
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
            var form = new MenuDishWindow();
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
                var form = new MenuDishWindow();
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
                Task<HttpResponseMessage> response;
                if (id.HasValue)
                {
                    response = APIClient.PostRequest("api/Menu/UpdElement", new MenuBindingModel
                    {
                        Id = id.Value,
                        MenuName = textBoxName.Text,
                        Price = Convert.ToInt32(textBoxPrice.Text),
                        MenuDishs = MenuDishBM
                    });
                }
                else
                {
                    response = APIClient.PostRequest("api/Menu/AddElement", new MenuBindingModel
                    {
                        MenuName = textBoxName.Text,
                        Price = Convert.ToInt32(textBoxPrice.Text),
                        MenuDishs = MenuDishBM
                    });
                }
                if (response.Result.IsSuccessStatusCode)
                {
                    MessageBox.Show("Сохранение прошло успешно", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                    DialogResult = true;
                    Close();
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

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
