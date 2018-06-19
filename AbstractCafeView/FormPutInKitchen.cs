using AbstractCafeService.BindingModel;
using AbstractCafeService.Interfaces;
using AbstractCafeService.ViewModels;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Unity;
using Unity.Attributes;

namespace AbstractCafeView
{
    public partial class FormPutInKitchen : Form
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        private readonly IKitchenService serviceS;

        private readonly IDishService serviceC;

        private readonly IMainService serviceM;

        public FormPutInKitchen(IKitchenService serviceS, IDishService serviceC, IMainService serviceM)
        {
            InitializeComponent();
            this.serviceS = serviceS;
            this.serviceC = serviceC;
            this.serviceM = serviceM;
        }

        private void FormPutInKitchen_Load(object sender, EventArgs e)
        {
            try
            {
                List<DishViewModel> listC = serviceC.GetList();
                if (listC != null)
                {
                    comboBoxDish.DisplayMember = "DishName";
                    comboBoxDish.ValueMember = "Id";
                    comboBoxDish.DataSource = listC;
                    comboBoxDish.SelectedItem = null;
                }
                List<KitchenViewModel> listS = serviceS.GetList();
                if (listS != null)
                {
                    comboBoxKitchen.DisplayMember = "KitchenName";
                    comboBoxKitchen.ValueMember = "Id";
                    comboBoxKitchen.DataSource = listS;
                    comboBoxKitchen.SelectedItem = null;
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
                serviceM.PutDishOnKitchen(new KitchenDishBindingModel
                {
                    DishId = Convert.ToInt32(comboBoxDish.SelectedValue),
                    KitchenId = Convert.ToInt32(comboBoxKitchen.SelectedValue),
                    Count = Convert.ToInt32(textBoxCount.Text)
                });
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
