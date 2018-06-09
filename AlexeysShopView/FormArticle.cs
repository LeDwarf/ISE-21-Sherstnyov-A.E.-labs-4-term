using AlexeysShopService.BindingModels;
using AlexeysShopService.Interfaces;
using AlexeysShopService.ViewModels;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Unity;
using Unity.Attributes;

namespace AlexeysShopView
{
    public partial class FormArticle : Form
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        public int Id { set { id = value; } }

        private readonly IArticleService service;

        private int? id;

        private List<ArticlePartViewModel> productParts;

        public FormArticle(IArticleService service)
        {
            InitializeComponent();
            this.service = service;
        }

        private void FormArticle_Load(object sender, EventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    ArticleViewModel view = service.GetElement(id.Value);
                    if (view != null)
                    {
                        textBoxName.Text = view.ArticleName;
                        textBoxPrice.Text = view.Cost.ToString();
                        productParts = view.ArticleParts;
                        LoadData();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                productParts = new List<ArticlePartViewModel>();
            }
        }

        private void LoadData()
        {
            try
            {
                if (productParts != null)
                {
                    dataGridView.DataSource = null;
                    dataGridView.DataSource = productParts;
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
            var form = Container.Resolve<FormArticlePart>();
            if (form.ShowDialog() == DialogResult.OK)
            {
                if (form.Model != null)
                {
                    if (id.HasValue)
                    {
                        form.Model.ArticleId = id.Value;
                    }
                    productParts.Add(form.Model);
                }
                LoadData();
            }
        }

        private void buttonUpd_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count == 1)
            {
                var form = Container.Resolve<FormArticlePart>();
                form.Model = productParts[dataGridView.SelectedRows[0].Cells[0].RowIndex];
                if (form.ShowDialog() == DialogResult.OK)
                {
                    productParts[dataGridView.SelectedRows[0].Cells[0].RowIndex] = form.Model;
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
                        productParts.RemoveAt(dataGridView.SelectedRows[0].Cells[0].RowIndex);
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
            if (productParts == null || productParts.Count == 0)
            {
                MessageBox.Show("Заполните компоненты", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                List<ArticlePartBindingModel> productPartBM = new List<ArticlePartBindingModel>();
                for (int i = 0; i < productParts.Count; ++i)
                {
                    productPartBM.Add(new ArticlePartBindingModel
                    {
                        Id = productParts[i].Id,
                        ArticleId = productParts[i].ArticleId,
                        PartId = productParts[i].PartId,
                        Count = productParts[i].Count
                    });
                }
                if (id.HasValue)
                {
                    service.UpdElement(new ArticleBindingModel
                    {
                        Id = id.Value,
                        ArticleName = textBoxName.Text,
                        Cost = Convert.ToInt32(textBoxPrice.Text),
                        ArticleParts = productPartBM
                    });
                }
                else
                {
                    service.AddElement(new ArticleBindingModel
                    {
                        ArticleName = textBoxName.Text,
                        Cost = Convert.ToInt32(textBoxPrice.Text),
                        ArticleParts = productPartBM
                    });
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
