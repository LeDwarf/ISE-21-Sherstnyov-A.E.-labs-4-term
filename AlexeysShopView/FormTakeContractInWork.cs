using AlexeysShopService.BindingModels;
using AlexeysShopService.ViewModels;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AlexeysShopView
{
	public partial class FormTakeContractInWork : Form
	{
		public int Id { set { id = value; } }

		private int? id;

		public FormTakeContractInWork()
		{
			InitializeComponent();
		}

		private void FormTakeContractInWork_Load(object sender, EventArgs e)
		{
			try
			{
				if (!id.HasValue)
				{
					MessageBox.Show("Не указан заказ", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
					Close();
				}
				var response = APIClient.GetRequest("api/Builder/GetList");
				if (response.Result.IsSuccessStatusCode)
				{
					List<BuilderViewModel> list = APIClient.GetElement<List<BuilderViewModel>>(response);
					if (list != null)
					{
						comboBoxBuilder.DisplayMember = "BuilderFIO";
						comboBoxBuilder.ValueMember = "Id";
						comboBoxBuilder.DataSource = list;
						comboBoxBuilder.SelectedItem = null;
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

		private void buttonSave_Click(object sender, EventArgs e)
		{
			if (comboBoxBuilder.SelectedValue == null)
			{
				MessageBox.Show("Выберите исполнителя", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			try
			{
				var response = APIClient.PostRequest("api/General/TakeContractInWork", new ContractBindingModel
				{
					Id = id.Value,
					BuilderId = Convert.ToInt32(comboBoxBuilder.SelectedValue)
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
