﻿using AbstractCafeService.BindingModel;
using AbstractCafeService.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AbstractCafeView
{
    public partial class FormMenu : Form
    {
        public int Id { set { id = value; } }

        private int? id;

        private List<MenuDishViewModel> menuDishs;

        public FormMenu()
        {
            InitializeComponent();
        }

        private void LoadData()
        {
            try
            {
                if (menuDishs != null)
                {
                    dataGridView.DataSource = null;
                    dataGridView.DataSource = menuDishs;
                    dataGridView.Columns[0].Visible = false;
                    dataGridView.Columns[1].Visible = false;
                    dataGridView.Columns[2].Visible = false;
                    dataGridView.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormMenu_Load(object sender, EventArgs e)
        {
            if (id.HasValue)
            {
                try
                { 
                    var menu = Task.Run(() => APIClient.GetRequestData<MenuViewModel>("api/Menu/Get/" + id.Value)).Result;
                    textBoxName.Text = menu.MenuName;
                    textBoxPrice.Text = menu.Price.ToString();
                    menuDishs = menu.MenuDishs;
                    LoadData();
                }
                catch (Exception ex)
                {
                    while (ex.InnerException != null)
                    {
                        ex = ex.InnerException;
                    }
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                menuDishs = new List<MenuDishViewModel>();
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            var form = new FormMenuDish();
            if (form.ShowDialog() == DialogResult.OK)
            {
                if (form.Model != null)
                {
                    if (id.HasValue)
                    {
                        form.Model.MenuId = id.Value;
                    }
                    menuDishs.Add(form.Model);
                }
                LoadData();
            }
        }

        private void buttonUpd_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count == 1)
            {
                var form = new FormMenuDish();
                form.Model = menuDishs[dataGridView.SelectedRows[0].Cells[0].RowIndex];
                if (form.ShowDialog() == DialogResult.OK)
                {
                    menuDishs[dataGridView.SelectedRows[0].Cells[0].RowIndex] = form.Model;
                    LoadData();
                }
            }
        }

        private void buttonDel_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count == 1)
            {
                if (MessageBox.Show("Удалить запись", "Вопрос", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        menuDishs.RemoveAt(dataGridView.SelectedRows[0].Cells[0].RowIndex);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    LoadData();
                }
            }
        }

        private void buttonRef_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxName.Text))
            {
                MessageBox.Show("Заполните название", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (string.IsNullOrEmpty(textBoxPrice.Text))
            {
                MessageBox.Show("Заполните цену", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (menuDishs == null || menuDishs.Count == 0)
            {
                MessageBox.Show("Заполните компоненты", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            List<MenuDishBindingModel> menuDishBM = new List<MenuDishBindingModel>();
            for (int i = 0; i < menuDishs.Count; ++i)
            {
                menuDishBM.Add(new MenuDishBindingModel
                {
                    Id = menuDishs[i].Id,
                    MenuId = menuDishs[i].MenuId,
                    DishId = menuDishs[i].DishId,
                    Count = menuDishs[i].Count
                });
            }
            string name = textBoxName.Text;
            int price = Convert.ToInt32(textBoxPrice.Text);
            Task task;
            if (id.HasValue)
            {
                task = Task.Run(() => APIClient.PostRequestData("api/Menu/UpdElement", new MenuBindingModel
                {
                    Id = id.Value,
                    MenuName = name,
                    Price = price,
                    MenuDishs = menuDishBM
                }));
            }
            else
            {
                task = Task.Run(() => APIClient.PostRequestData("api/Menu/AddElement", new MenuBindingModel
                {
                    MenuName = name,
                    Price = price,
                    MenuDishs = menuDishBM
                }));
            }

            task.ContinueWith((prevTask) => MessageBox.Show("Сохранение прошло успешно. Обновите список", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information),
                TaskContinuationOptions.OnlyOnRanToCompletion);
            task.ContinueWith((prevTask) =>
            {
                var ex = (Exception)prevTask.Exception;
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }, TaskContinuationOptions.OnlyOnFaulted);

            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
