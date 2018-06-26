﻿using AbstractCafeService.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AbstractCafeView
{
    public partial class FormMenuDish : Form
    {
        public MenuDishViewModel Model { set { model = value; } get { return model; } }

        private MenuDishViewModel model;

        public FormMenuDish()
        {
            InitializeComponent();
        }

        private void FormMenuDish_Load(object sender, EventArgs e)
        {
            try
            {
                comboBoxDish.DisplayMember = "DishName";
                comboBoxDish.ValueMember = "Id";
                comboBoxDish.DataSource = Task.Run(() => APIClient.GetRequestData<List<DishViewModel>>("api/Dish/GetList")).Result;
                comboBoxDish.SelectedItem = null;

            }
            catch (Exception ex)
            {
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (model != null)
            {
                comboBoxDish.Enabled = false;
                comboBoxDish.SelectedValue = model.DishId;
                textBoxCount.Text = model.Count.ToString();
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxCount.Text))
            {
                MessageBox.Show("Заполните поле Количество", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (comboBoxDish.SelectedValue == null)
            {
                MessageBox.Show("Выберите блюдо", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                if (model == null)
                {
                    model = new MenuDishViewModel
                    {
                        DishId = Convert.ToInt32(comboBoxDish.SelectedValue),
                        DishName = comboBoxDish.Text,
                        Count = Convert.ToInt32(textBoxCount.Text)
                    };
                }
                else
                {
                    model.Count = Convert.ToInt32(textBoxCount.Text);
                }
                MessageBox.Show("Сохранение прошло успешно", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
