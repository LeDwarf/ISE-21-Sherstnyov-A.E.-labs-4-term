using AlexeysShopService.BindingModels;
using AlexeysShopService.ViewModels;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AlexeysShopView
{
	public partial class FormPutOnStorage : Form
	{
		public FormPutOnStorage()
		{
			InitializeComponent();
		}

		private void FormPutOnStorage_Load(object sender, EventArgs e)
		{
			try
			{
				var responseC = APIClient.GetRequest("api/Part/GetList");
				if (responseC.Result.IsSuccessStatusCode)
				{
					List<PartViewModel> list = APIClient.GetElement<List<PartViewModel>>(responseC);
					if (list != null)
					{
						comboBoxPart.DisplayMember = "PartName";
						comboBoxPart.ValueMember = "Id";
						comboBoxPart.DataSource = list;
						comboBoxPart.SelectedItem = null;
					}
				}
				else
				{
					throw new Exception(APIClient.GetError(responseC));
				}
				var responseS = APIClient.GetRequest("api/Storage/GetList");
				if (responseS.Result.IsSuccessStatusCode)
				{
					List<StorageViewModel> list = APIClient.GetElement<List<StorageViewModel>>(responseS);
					if (list != null)
					{
						comboBoxStorage.DisplayMember = "StorageName";
						comboBoxStorage.ValueMember = "Id";
						comboBoxStorage.DataSource = list;
						comboBoxStorage.SelectedItem = null;
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
			if (comboBoxPart.SelectedValue == null)
			{
				MessageBox.Show("Выберите компонент", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			if (comboBoxStorage.SelectedValue == null)
			{
				MessageBox.Show("Выберите склад", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			try
			{
				var response = APIClient.PostRequest("api/General/PutPartOnStorage", new StoragePartBindingModel
				{
					PartId = Convert.ToInt32(comboBoxPart.SelectedValue),
					StorageId = Convert.ToInt32(comboBoxStorage.SelectedValue),
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
