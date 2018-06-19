using AbstractCafeService.BindingModel;
using AbstractCafeService.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

                List<DishViewModel> listD = Task.Run(() => APIClient.GetRequestData<List<DishViewModel>>("api/Dish/GetList")).Result;
                if (listD != null)
                {
                    comboBoxDish.DisplayMember = "DishName";
                    comboBoxDish.ValueMember = "Id";
                    comboBoxDish.DataSource = listD;
                    comboBoxDish.SelectedItem = null;
                }

                List<KitchenViewModel> listK = Task.Run(() => APIClient.GetRequestData<List<KitchenViewModel>>("api/Kitchen/GetList")).Result;
                if (listK != null)
                {
                    comboBoxKitchen.DisplayMember = "KitchenName";
                    comboBoxKitchen.ValueMember = "Id";
                    comboBoxKitchen.DataSource = listK;
                    comboBoxKitchen.SelectedItem = null;
                }
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
                int dishId = Convert.ToInt32(comboBoxDish.SelectedValue);
                int kitchenId = Convert.ToInt32(comboBoxKitchen.SelectedValue);
                int count = Convert.ToInt32(textBoxCount.Text);
                Task task = Task.Run(() => APIClient.PostRequestData("api/Main/PutDishOnKitchen", new KitchenDishBindingModel
                {
                    DishId = dishId,
                    KitchenId = kitchenId,
                    Count = count
                }));

                task.ContinueWith((prevTask) => MessageBox.Show("Кухня пополнена", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information),
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
            catch (Exception ex)
            {
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
