using AlexeysShopService.BindingModels;
using AlexeysShopService.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AlexeysShopView
{
    public partial class FormGeneral : Form
    {
        public FormGeneral()
        {
            InitializeComponent();
        }

        private void LoadData()
        {
            try
            {
                List<ContractViewModel> list = Task.Run(() => APIClient.GetRequestData<List<ContractViewModel>>("api/General/GetList")).Result;
                if (list != null)
                {
                    dataGridView.DataSource = list;
                    dataGridView.Columns[0].Visible = false;
                    dataGridView.Columns[1].Visible = false;
                    dataGridView.Columns[3].Visible = false;
                    dataGridView.Columns[5].Visible = false;
                    dataGridView.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
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

        private void клиентыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new FormCustomers();
            form.ShowDialog();
        }

        private void компонентыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new FormParts();
            form.ShowDialog();
        }

        private void изделияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new FormArticles();
            form.ShowDialog();
        }

        private void складыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new FormStorages();
            form.ShowDialog();
        }

        private void сотрудникиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new FormBuilders();
            form.ShowDialog();
        }

        private void пополнитьСкладToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new FormPutOnStorage();
            form.ShowDialog();
        }

        private void buttonCreateContract_Click(object sender, EventArgs e)
        {
            var form = new FormCreateContract();
            form.ShowDialog();
        }

        private void buttonTakeContractInWork_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count == 1)
            {
                var form = new FormTakeContractInWork
                {
                    Id = Convert.ToInt32(dataGridView.SelectedRows[0].Cells[0].Value)
                };
                form.ShowDialog();
            }
        }

        private void buttonContractReady_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count == 1)
            {
                int id = Convert.ToInt32(dataGridView.SelectedRows[0].Cells[0].Value);

                Task task = Task.Run(() => APIClient.PostRequestData("api/General/FinishContract", new ContractBindingModel
                {
                    Id = id
                }));

                task.ContinueWith((prevTask) => MessageBox.Show("Статус заказа изменен. Обновите список", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information),
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
            }
        }

        private void buttonPayContract_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count == 1)
            {
                int id = Convert.ToInt32(dataGridView.SelectedRows[0].Cells[0].Value);

                Task task = Task.Run(() => APIClient.PostRequestData("api/General/PayContract", new ContractBindingModel
                {
                    Id = id
                }));

                task.ContinueWith((prevTask) => MessageBox.Show("Статус заказа изменен. Обновите список", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information),
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
            }
        }

        private void buttonRef_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void прайсСудовToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "doc|*.doc|docx|*.docx"
            };
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                string fileName = sfd.FileName;
                Task task = Task.Run(() => APIClient.PostRequestData("api/Report/SaveArticlePrice", new ReportBindingModel
                {
                    FileName = fileName
                }));

                task.ContinueWith((prevTask) => MessageBox.Show("Выполнено", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information),
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
            }
        }

        private void загруженностьСкладовToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            var form = new FormStoragesLoad();
            form.ShowDialog();
        }

        private void контрактыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new FormCustomerContracts();
            form.ShowDialog();
        }

        private void письмаToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            var form = new FormMails();
            form.ShowDialog();
        }
    }
}
