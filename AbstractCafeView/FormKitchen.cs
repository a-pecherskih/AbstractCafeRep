using AbstractCafeService.BindingModel;
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
                    var kitchen = Task.Run(() => APIClient.GetRequestData<KitchenViewModel>("api/Kitchen/Get/" + id.Value)).Result;
                    textBoxName.Text = kitchen.KitchenName;
                    dataGridView.DataSource = kitchen.KitchenDishs;
                    dataGridView.Columns[0].Visible = false;
                    dataGridView.Columns[1].Visible = false;
                    dataGridView.Columns[2].Visible = false;
                    dataGridView.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
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
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxName.Text))
            {
                MessageBox.Show("Заполните название", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string name = textBoxName.Text;
            Task task;
            if (id.HasValue)
            {
                task = Task.Run(() => APIClient.PostRequestData("api/Kitchen/UpdElement", new KitchenBindingModel
                {
                    Id = id.Value,
                    KitchenName = name
                }));
            }
            else
            {
                task = Task.Run(() => APIClient.PostRequestData("api/Kitchen/AddElement", new KitchenBindingModel
                {
                    KitchenName = name
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
