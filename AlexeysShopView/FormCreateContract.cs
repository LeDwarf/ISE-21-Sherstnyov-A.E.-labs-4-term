using AlexeysShopService.BindingModels;
using AlexeysShopService.ViewModels;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AlexeysShopView
{
	public partial class FormCreateContract : Form
	{
		public FormCreateContract()
		{
			InitializeComponent();
		}

		private void FormCreateContract_Load(object sender, EventArgs e)
		{
			try
			{
				var responseC = APIClient.GetRequest("api/Customer/GetList");
				if (responseC.Result.IsSuccessStatusCode)
				{
					List<CustomerViewModel> list = APIClient.GetElement<List<CustomerViewModel>>(responseC);
					if (list != null)
					{
						comboBoxCustomer.DisplayMember = "CustomerFIO";
						comboBoxCustomer.ValueMember = "Id";
						comboBoxCustomer.DataSource = list;
						comboBoxCustomer.SelectedItem = null;
					}
				}
				else
				{
					throw new Exception(APIClient.GetError(responseC));
				}
				var responseP = APIClient.GetRequest("api/Article/GetList");
				if (responseP.Result.IsSuccessStatusCode)
				{
					List<ArticleViewModel> list = APIClient.GetElement<List<ArticleViewModel>>(responseP);
					if (list != null)
					{
						comboBoxArticle.DisplayMember = "ArticleName";
						comboBoxArticle.ValueMember = "Id";
						comboBoxArticle.DataSource = list;
						comboBoxArticle.SelectedItem = null;
					}
				}
				else
				{
					throw new Exception(APIClient.GetError(responseP));
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void CalcSum()
		{
			if (comboBoxArticle.SelectedValue != null && !string.IsNullOrEmpty(textBoxCount.Text))
			{
				try
				{
					int id = Convert.ToInt32(comboBoxArticle.SelectedValue);
					var responseP = APIClient.GetRequest("api/Article/Get/" + id);
					if (responseP.Result.IsSuccessStatusCode)
					{
						ArticleViewModel Article = APIClient.GetElement<ArticleViewModel>(responseP);
						int count = Convert.ToInt32(textBoxCount.Text);
						textBoxSum.Text = (count * (int)Article.Cost).ToString();
					}
					else
					{
						throw new Exception(APIClient.GetError(responseP));
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
		}

		private void textBoxCount_TextChanged(object sender, EventArgs e)
		{
			CalcSum();
		}

		private void comboBoxArticle_SelectedIndexChanged(object sender, EventArgs e)
		{
			CalcSum();
		}

		private void buttonSave_Click(object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(textBoxCount.Text))
			{
				MessageBox.Show("Заполните поле Количество", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			if (comboBoxCustomer.SelectedValue == null)
			{
				MessageBox.Show("Выберите клиента", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			if (comboBoxArticle.SelectedValue == null)
			{
				MessageBox.Show("Выберите изделие", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			try
			{
				var response = APIClient.PostRequest("api/General/CreateContract", new ContractBindingModel
				{
					CustomerId = Convert.ToInt32(comboBoxCustomer.SelectedValue),
					ArticleId = Convert.ToInt32(comboBoxArticle.SelectedValue),
					Count = Convert.ToInt32(textBoxCount.Text),
					Cost = Convert.ToInt32(textBoxSum.Text)
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
