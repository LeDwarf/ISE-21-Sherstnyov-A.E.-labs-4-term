using AlexeysShopService.ViewModels;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AlexeysShopView
{
	public partial class FormArticlePart : Form
	{
		public ArticlePartViewModel Model { set { model = value; } get { return model; } }

		private ArticlePartViewModel model;

		public FormArticlePart()
		{
			InitializeComponent();
		}

		private void FormArticlePart_Load(object sender, EventArgs e)
		{
			try
			{
				var response = APIClient.GetRequest("api/Part/GetList");
				if (response.Result.IsSuccessStatusCode)
				{
					comboBoxPart.DisplayMember = "PartName";
					comboBoxPart.ValueMember = "Id";
					comboBoxPart.DataSource = APIClient.GetElement<List<PartViewModel>>(response);
					comboBoxPart.SelectedItem = null;
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
			if (model != null)
			{
				comboBoxPart.Enabled = false;
				comboBoxPart.SelectedValue = model.PartId;
				textBoxCount.Text = model.Count.ToString();
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
			try
			{
				if (model == null)
				{
					model = new ArticlePartViewModel
					{
						PartId = Convert.ToInt32(comboBoxPart.SelectedValue),
						PartName = comboBoxPart.Text,
						Count = Convert.ToInt32(textBoxCount.Text)
					};
				}
				else
				{
					model.Count = Convert.ToInt32(textBoxCount.Text);
				}
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
