using AbstractCafeService.BindingModel;
using AbstractCafeService.ViewModels;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AbstractCafeView
{
    public partial class FormPutInKitchen : Form
    {
        public FormPutInKitchen()
        {
            InitializeComponent();
        }

        private void FormPutInKitchen_Load(object sender, EventArgs e)
        {
            try
            {
                var responseC = APIClient.GetRequest("api/Dish/GetList");
                if (responseC.Result.IsSuccessStatusCode)
                {
                    List<DishViewModel> list = APIClient.GetElement<List<DishViewModel>>(responseC);
                    if (list != null)
                    {
                        comboBoxDish.DisplayMember = "DishName";
                        comboBoxDish.ValueMember = "Id";
                        comboBoxDish.DataSource = list;
                        comboBoxDish.SelectedItem = null;
                    }
                }
                else
                {
                    throw new Exception(APIClient.GetError(responseC));
                }
                var responseS = APIClient.GetRequest("api/Kitchen/GetList");
                if (responseS.Result.IsSuccessStatusCode)
                {
                    List<KitchenViewModel> list = APIClient.GetElement<List<KitchenViewModel>>(responseS);
                    if (list != null)
                    {
                        comboBoxKitchen.DisplayMember = "KitchenName";
                        comboBoxKitchen.ValueMember = "Id";
                        comboBoxKitchen.DataSource = list;
                        comboBoxKitchen.SelectedItem = null;
                    }
                }
                else
                {
                    throw new Exception(APIClient.GetError(responseC));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                MessageBox.Show("Выберите компонент", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (comboBoxKitchen.SelectedValue == null)
            {
                MessageBox.Show("Выберите склад", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                var response = APIClient.PostRequest("api/Main/PutDishOnKitchen", new KitchenDishBindingModel
                {
                    DishId = Convert.ToInt32(comboBoxDish.SelectedValue),
                    KitchenId = Convert.ToInt32(comboBoxKitchen.SelectedValue),
                    Count = Convert.ToInt32(textBoxCount.Text)
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
