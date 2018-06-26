using AbstractCafeService.BindingModel;
using AbstractCafeService.Interfaces;
using AbstractCafeService.ViewModels;
using System;
using System.Collections.Generic;
using System.Net.Http;
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
                    var response = APIClient.GetRequest("api/Menu/Get/" + id.Value);
                    if (response.Result.IsSuccessStatusCode)
                    {
                        var menu = APIClient.GetElement<MenuViewModel>(response);
                        textBoxName.Text = menu.MenuName;
                        textBoxPrice.Text = menu.Price.ToString();
                        menuDishs = menu.MenuDishs;
                        LoadData();
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
            try
            {
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
                Task<HttpResponseMessage> response;
                if (id.HasValue)
                {
                    response = APIClient.PostRequest("api/Menu/UpdElement", new MenuBindingModel
                    {
                        Id = id.Value,
                        MenuName = textBoxName.Text,
                        Price = Convert.ToInt32(textBoxPrice.Text),
                        MenuDishs = menuDishBM
                    });
                }
                else
                {
                    response = APIClient.PostRequest("api/Menu/AddElement", new MenuBindingModel
                    {
                        MenuName = textBoxName.Text,
                        Price = Convert.ToInt32(textBoxPrice.Text),
                        MenuDishs = menuDishBM
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
