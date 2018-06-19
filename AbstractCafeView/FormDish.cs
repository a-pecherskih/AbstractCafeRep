using AbstractCafeService.BindingModel;
using AbstractCafeService.ViewModels;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AbstractCafeView
{
    public partial class FormDish : Form
    {
        public int Id { set { id = value; } }

        private int? id;

        public FormDish()
        {
            InitializeComponent();
        }

        private void FormDish_Load(object sender, EventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    var response = APIClient.GetRequest("api/Dish/Get/" + id.Value);
                    if (response.Result.IsSuccessStatusCode)
                    {
                        var dish = APIClient.GetElement<DishViewModel>(response);
                        textBoxName.Text = dish.DishName;
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
                    response = APIClient.PostRequest("api/Dish/UpdElement", new DishBindingModel
                    {
                        Id = id.Value,
                        DishName = textBoxName.Text
                    });
                }
                else
                {
                    response = APIClient.PostRequest("api/Dish/AddElement", new DishBindingModel
                    {
                        DishName = textBoxName.Text
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
