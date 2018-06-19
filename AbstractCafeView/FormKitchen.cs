﻿using AbstractCafeService.BindingModel;
using AbstractCafeService.ViewModels;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AbstractCafeView
{
    public partial class FormKitchen : Form
    {
        public int Id { set { id = value; } }

        private int? id;

        public FormKitchen()
        {
            InitializeComponent();
        }

        private void FormKitchen_Load(object sender, EventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    var response = APIClient.GetRequest("api/Kitchen/Get/" + id.Value);
                    if (response.Result.IsSuccessStatusCode)
                    {
                        var kitchen = APIClient.GetElement<KitchenViewModel>(response);
                        textBoxName.Text = kitchen.KitchenName;
                        dataGridView.DataSource = kitchen.KitchenDishs;
                        dataGridView.Columns[0].Visible = false;
                        dataGridView.Columns[1].Visible = false;
                        dataGridView.Columns[2].Visible = false;
                        dataGridView.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    }
                    else
                    {
                        throw new Exception(APIClient.GetError(response));
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxName.Text))
            {
                MessageBox.Show("Заполните название", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                Task<HttpResponseMessage> response;
                if (id.HasValue)
                {
                    response = APIClient.PostRequest("api/Kitchen/UpdElement", new KitchenBindingModel
                    {
                        Id = id.Value,
                        KitchenName = textBoxName.Text
                    });
                }
                else
                {
                    response = APIClient.PostRequest("api/Kitchen/AddElement", new KitchenBindingModel
                    {
                        KitchenName = textBoxName.Text
                    });
                }
                if (response.Result.IsSuccessStatusCode)
                {
                    MessageBox.Show("Сохранение прошло успешно", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    throw new Exception(APIClient.GetError(response));
                }
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
