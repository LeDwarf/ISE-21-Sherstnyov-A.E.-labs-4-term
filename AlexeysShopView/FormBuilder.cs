using AlexeysShopService.BindingModels;
using AlexeysShopService.ViewModels;
using System;
using System.Net.Http;
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
					var response = APIClient.GetRequest("api/Builder/Get/" + id.Value);
					if (response.Result.IsSuccessStatusCode)
					{
						var Builder = APIClient.GetElement<BuilderViewModel>(response);
						textBoxFIO.Text = Builder.BuilderFIO;
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
			if (string.IsNullOrEmpty(textBoxFIO.Text))
			{
				MessageBox.Show("Заполните ФИО", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			try
			{
				Task<HttpResponseMessage> response;
				if (id.HasValue)
				{
					response = APIClient.PostRequest("api/Builder/UpdElement", new BuilderBindingModel
					{
						Id = id.Value,
						BuilderFIO = textBoxFIO.Text
					});
				}
				else
				{
					response = APIClient.PostRequest("api/Builder/AddElement", new BuilderBindingModel
					{
						BuilderFIO = textBoxFIO.Text
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
