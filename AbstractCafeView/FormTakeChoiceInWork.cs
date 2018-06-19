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
    public partial class FormTakeChoiceInWork : Form
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        public int Id { set { id = value; } }

        private readonly IChefService serviceI;

        private readonly IMainService serviceM;

        private int? id;

        public FormTakeChoiceInWork(IChefService serviceI, IMainService serviceM)
        {
            InitializeComponent();
            this.serviceI = serviceI;
            this.serviceM = serviceM;
        }

        private void FormTakeChoiceInWork_Load(object sender, EventArgs e)
        {
            try
            {
                if (!id.HasValue)
                {
                    MessageBox.Show("Не указан заказ", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Close();
                }
                List<ChefViewModel> listI = serviceI.GetList();
                if (listI != null)
                {
                    comboBoxChef.DisplayMember = "ChefFIO";
                    comboBoxChef.ValueMember = "Id";
                    comboBoxChef.DataSource = listI;
                    comboBoxChef.SelectedItem = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (comboBoxChef.SelectedValue == null)
            {
                MessageBox.Show("Выберите исполнителя", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                serviceM.TakeChoiceInWork(new ChoiceBindingModel
                {
                    Id = id.Value,
                    ChefId = Convert.ToInt32(comboBoxChef.SelectedValue)
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
