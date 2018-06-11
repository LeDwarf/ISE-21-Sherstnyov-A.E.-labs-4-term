using AlexeysShopService.BindingModels;
using AlexeysShopService.ViewModels;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AlexeysShopView
{
	public partial class FormArticle : Form
	{
		public int Id { set { id = value; } }

		private int? id;

		private List<ArticlePartViewModel> ArticleParts;

		public FormArticle()
		{
			InitializeComponent();
		}

		private void FormArticle_Load(object sender, EventArgs e)
		{
			if (id.HasValue)
			{
				try
				{
					var response = APIClient.GetRequest("api/Article/Get/" + id.Value);
					if (response.Result.IsSuccessStatusCode)
					{
						var Article = APIClient.GetElement<ArticleViewModel>(response);
						textBoxName.Text = Article.ArticleName;
						textBoxPrice.Text = Article.Cost.ToString();
						ArticleParts = Article.ArticleParts;
						LoadData();
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
			else
			{
				ArticleParts = new List<ArticlePartViewModel>();
			}
		}

		private void LoadData()
		{
			try
			{
				if (ArticleParts != null)
				{
					dataGridView.DataSource = null;
					dataGridView.DataSource = ArticleParts;
					dataGridView.Columns[0].Visible = false;
					dataGridView.Columns[1].Visible = false;
					dataGridView.Columns[2].Visible = false;
					dataGridView.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void buttonAdd_Click(object sender, EventArgs e)
		{
			var form = new FormArticlePart();
			if (form.ShowDialog() == DialogResult.OK)
			{
				if (form.Model != null)
				{
					if (id.HasValue)
					{
						form.Model.ArticleId = id.Value;
					}
					ArticleParts.Add(form.Model);
				}
				LoadData();
			}
		}

		private void buttonUpd_Click(object sender, EventArgs e)
		{
			if (dataGridView.SelectedRows.Count == 1)
			{
				var form = new FormArticlePart();
				form.Model = ArticleParts[dataGridView.SelectedRows[0].Cells[0].RowIndex];
				if (form.ShowDialog() == DialogResult.OK)
				{
					ArticleParts[dataGridView.SelectedRows[0].Cells[0].RowIndex] = form.Model;
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
					try
					{
						ArticleParts.RemoveAt(dataGridView.SelectedRows[0].Cells[0].RowIndex);
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

		private void buttonSave_Click(object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(textBoxName.Text))
			{
				MessageBox.Show("Заполните название", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			if (string.IsNullOrEmpty(textBoxPrice.Text))
			{
				MessageBox.Show("Заполните цену", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			if (ArticleParts == null || ArticleParts.Count == 0)
			{
				MessageBox.Show("Заполните компоненты", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			try
			{
				List<ArticlePartBindingModel> ArticlePartBM = new List<ArticlePartBindingModel>();
				for (int i = 0; i < ArticleParts.Count; ++i)
				{
					ArticlePartBM.Add(new ArticlePartBindingModel
					{
						Id = ArticleParts[i].Id,
						ArticleId = ArticleParts[i].ArticleId,
						PartId = ArticleParts[i].PartId,
						Count = ArticleParts[i].Count
					});
				}
				Task<HttpResponseMessage> response;
				if (id.HasValue)
				{
					response = APIClient.PostRequest("api/Article/UpdElement", new ArticleBindingModel
					{
						Id = id.Value,
						ArticleName = textBoxName.Text,
						Cost = Convert.ToInt32(textBoxPrice.Text),
						ArticleParts = ArticlePartBM
					});
				}
				else
				{
					response = APIClient.PostRequest("api/Article/AddElement", new ArticleBindingModel
					{
						ArticleName = textBoxName.Text,
						Cost = Convert.ToInt32(textBoxPrice.Text),
						ArticleParts = ArticlePartBM
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
