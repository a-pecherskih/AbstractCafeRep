using AbstractCafeService.BindingModel;
using AbstractCafeService.ViewModels;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AbstractCafeView
{
    public partial class FormCreateChoice : Form
    {
        public FormCreateChoice()
        {
            InitializeComponent();
        }

        private void FormCreateChoice_Load(object sender, EventArgs e)
        {
            try
            {
                var responseC = APIClient.GetRequest("api/Customer/GetList");
                if (responseC.Result.IsSuccessStatusCode)
                {
                    List<CustomerViewModel> list = APIClient.GetElement<List<CustomerViewModel>>(responseC);
                    if (list != null)
                    {
                        comboBoxCustomer.DisplayMember = "CustomerFIO";
                        comboBoxCustomer.ValueMember = "Id";
                        comboBoxCustomer.DataSource = list;
                        comboBoxCustomer.SelectedItem = null;
                    }
                }
                else
                {
                    throw new Exception(APIClient.GetError(responseC));
                }
                var responseP = APIClient.GetRequest("api/Menu/GetList");
                if (responseP.Result.IsSuccessStatusCode)
                {
                    List<MenuViewModel> list = APIClient.GetElement<List<MenuViewModel>>(responseP);
                    if (list != null)
                    {
                        comboBoxMenu.DisplayMember = "MenuName";
                        comboBoxMenu.ValueMember = "Id";
                        comboBoxMenu.DataSource = list;
                        comboBoxMenu.SelectedItem = null;
                    }
                }
                else
                {
                    throw new Exception(APIClient.GetError(responseP));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CalcSum()
        {
            if (comboBoxMenu.SelectedValue != null && !string.IsNullOrEmpty(textBoxCount.Text))
            {
                try
                {
                    int id = Convert.ToInt32(comboBoxMenu.SelectedValue);
                    var responseP = APIClient.GetRequest("api/Menu/Get/" + id);
                    if (responseP.Result.IsSuccessStatusCode)
                    {
                        MenuViewModel menu = APIClient.GetElement<MenuViewModel>(responseP);
                        int count = Convert.ToInt32(textBoxCount.Text);
                        textBoxSum.Text = (count * (int)menu.Price).ToString();
                    }
                    else
                    {
                        throw new Exception(APIClient.GetError(responseP));
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void textBoxCount_TextChanged(object sender, EventArgs e)
        {
            CalcSum();
        }

        private void comboBoxMenu_SelectedIndexChanged(object sender, EventArgs e)
        {
            CalcSum();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxCount.Text))
            {
                MessageBox.Show("Заполните поле Количество", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (comboBoxCustomer.SelectedValue == null)
            {
                MessageBox.Show("Выберите клиента", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (comboBoxMenu.SelectedValue == null)
            {
                MessageBox.Show("Выберите меню", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                var response = APIClient.PostRequest("api/Main/CreateChoice", new ChoiceBindingModel
                {
                    CustomerId = Convert.ToInt32(comboBoxCustomer.SelectedValue),
                    MenuId = Convert.ToInt32(comboBoxMenu.SelectedValue),
                    Count = Convert.ToInt32(textBoxCount.Text),
                    Sum = Convert.ToInt32(textBoxSum.Text)
                });
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
