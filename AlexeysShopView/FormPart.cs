using AlexeysShopService.BindingModels;
using AlexeysShopService.ViewModels;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AlexeysShopView
{
	public partial class FormPart : Form
	{
		public int Id { set { id = value; } }

		private int? id;

		public FormPart()
		{
			InitializeComponent();
		}

		private void FormPart_Load(object sender, EventArgs e)
		{
			if (id.HasValue)
			{
				try
				{
					var response = APIClient.GetRequest("api/Part/Get/" + id.Value);
					if (response.Result.IsSuccessStatusCode)
					{
						var Part = APIClient.GetElement<PartViewModel>(response);
						textBoxName.Text = Part.PartName;
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
					response = APIClient.PostRequest("api/Part/UpdElement", new PartBindingModel
					{
						Id = id.Value,
						PartName = textBoxName.Text
					});
				}
				else
				{
					response = APIClient.PostRequest("api/Part/AddElement", new PartBindingModel
					{
						PartName = textBoxName.Text
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
