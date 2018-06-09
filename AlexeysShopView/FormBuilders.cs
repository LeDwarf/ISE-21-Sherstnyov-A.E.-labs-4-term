using AlexeysShopService.BindingModels;
using AlexeysShopService.ViewModels;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AlexeysShopView
{
	public partial class FormBuilders : Form
	{
		public FormBuilders()
		{
			InitializeComponent();
		}

		private void FormBuilders_Load(object sender, EventArgs e)
		{
			LoadData();
		}

		private void LoadData()
		{
			try
			{
				var response = APIClient.GetRequest("api/Builder/GetList");
				if (response.Result.IsSuccessStatusCode)
				{
					List<BuilderViewModel> list = APIClient.GetElement<List<BuilderViewModel>>(response);
					if (list != null)
					{
						dataGridView.DataSource = list;
						dataGridView.Columns[0].Visible = false;
						dataGridView.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
					}
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

		private void buttonAdd_Click(object sender, EventArgs e)
		{
			var form = new FormBuilder();
			if (form.ShowDialog() == DialogResult.OK)
			{
				LoadData();
			}
		}

		private void buttonUpd_Click(object sender, EventArgs e)
		{
			if (dataGridView.SelectedRows.Count == 1)
			{
				var form = new FormBuilder();
				form.Id = Convert.ToInt32(dataGridView.SelectedRows[0].Cells[0].Value);
				if (form.ShowDialog() == DialogResult.OK)
				{
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
					int id = Convert.ToInt32(dataGridView.SelectedRows[0].Cells[0].Value);
					try
					{
						var response = APIClient.PostRequest("api/Builder/DelElement", new CustomerBindingModel { Id = id });
						if (!response.Result.IsSuccessStatusCode)
						{
							throw new Exception(APIClient.GetError(response));
						}
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
	}
}
