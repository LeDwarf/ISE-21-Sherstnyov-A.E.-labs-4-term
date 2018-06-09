using AlexeysShopService.BindingModels;
using AlexeysShopService.ViewModels;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AlexeysShopView
{
    public partial class FormBuilder : Form
    {
        public int Id { set { id = value; } }

        private int? id;

        public FormBuilder()
        {
            InitializeComponent();
        }

        private void FormBuilder_Load(object sender, EventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    var Builder = Task.Run(() => APIClient.GetRequestData<BuilderViewModel>("api/Builder/Get/" + id.Value)).Result;
                    textBoxFIO.Text = Builder.BuilderFIO;
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
            if (string.IsNullOrEmpty(textBoxFIO.Text))
            {
                MessageBox.Show("Заполните ФИО", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string fio = textBoxFIO.Text;
            Task task;
            if (id.HasValue)
            {
                task = Task.Run(() => APIClient.PostRequestData("api/Builder/UpdElement", new BuilderBindingModel
                {
                    Id = id.Value,
                    BuilderFIO = fio
                }));
            }
            else
            {
                task = Task.Run(() => APIClient.PostRequestData("api/Builder/AddElement", new BuilderBindingModel
                {
                    BuilderFIO = fio
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
