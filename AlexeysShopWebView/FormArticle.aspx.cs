using AlexeysShopService.BindingModels;
using AlexeysShopService.ImplementationsList;
using AlexeysShopService.Interfaces;
using AlexeysShopService.ViewModels;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AlexeysShopWebView
{
    public partial class FormArticle : System.Web.UI.Page
    {
        private readonly IArticleService service = new ArticleServiceList();

        private int id;

        private List<ArticlePartViewModel> productComponents;

        private ArticlePartViewModel model;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Int32.TryParse((string)Session["id"], out id))
            {
                try
                {
                    ArticleViewModel view = service.GetElement(id);
                    if (view != null)
                    {
                        textBoxName.Text = view.ArticleName;
                        textBoxCost.Text = view.Cost.ToString();
                        productComponents = view.ArticleParts;
                        LoadData();
                    }
                }
                catch (Exception ex)
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('" + ex.Message + "');</script>");
                }
            }
            else
            {
                if (service.GetList().Count == 0 || service.GetList().Last().ArticleName != null)
                {
                    productComponents = new List<ArticlePartViewModel>();
                    LoadData();
                }
                else
                {
                    productComponents = service.GetList().Last().ArticleParts;
                    LoadData();
                }
            }
            if (Session["SEId"] != null)
            {
                model = new ArticlePartViewModel
                {
                    Id = (int)Session["SEId"],
                    ArticleId = (int)Session["SEArticleId"],
                    PartId = (int)Session["SEPartId"],
                    PartName = (string)Session["SEPartName"],
                    Count = (int)Session["SECount"]
                };
                if (Session["SEIs"] != null)
                {
                    productComponents[(int)Session["SEIs"]] = model;
                }
                else
                {
                    productComponents.Add(model);
                }
            }
            List<ArticlePartBindingModel> productComponentBM = new List<ArticlePartBindingModel>();
            for (int i = 0; i < productComponents.Count; ++i)
            {
                productComponentBM.Add(new ArticlePartBindingModel
                {
                    Id = productComponents[i].Id,
                    ArticleId = productComponents[i].ArticleId,
                    PartId = productComponents[i].PartId,
                    Count = productComponents[i].Count
                });
            }
            if (productComponentBM.Count != 0)
            {
                if (service.GetList().Count == 0 || service.GetList().Last().ArticleName != null)
                {
                    service.AddElement(new ArticleBindingModel
                    {
                        ArticleName = null,
                        Cost = -1,
                        ArticleParts = productComponentBM
                    });
                }
                else
                {
                    service.UpdElement(new ArticleBindingModel
                    {
                        Id = service.GetList().Last().Id,
                        ArticleName = null,
                        Cost = -1,
                        ArticleParts = productComponentBM
                    });
                }

            }
            try
            {
                if (productComponents != null)
                {
                    dataGridView.DataBind();
                    dataGridView.DataSource = productComponents;
                    dataGridView.DataBind();
                    dataGridView.ShowHeaderWhenEmpty = true;
                    dataGridView.SelectedRowStyle.BackColor = Color.Silver;
                    dataGridView.Columns[1].Visible = false;
                    dataGridView.Columns[2].Visible = false;
                    dataGridView.Columns[3].Visible = false;
                    dataGridView.DataBind();
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('" + ex.Message + "');</script>");
            }
            Session["SEId"] = null;
            Session["SEArticleId"] = null;
            Session["SEPartId"] = null;
            Session["SEPartName"] = null;
            Session["SECount"] = null;
            Session["SEIs"] = null;
        }

        private void LoadData()
        {
            try
            {
                if (productComponents != null)
                {
                    dataGridView.DataBind();
                    dataGridView.DataSource = productComponents;
                    dataGridView.DataBind();
                    dataGridView.ShowHeaderWhenEmpty = true;
                    dataGridView.SelectedRowStyle.BackColor = Color.Silver;
                    dataGridView.Columns[1].Visible = false;
                    dataGridView.Columns[2].Visible = false;
                    dataGridView.Columns[3].Visible = false;
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('" + ex.Message + "');</script>");
            }
        }

        protected void ButtonAdd_Click(object sender, EventArgs e)
        {
            Server.Transfer("FormArticlePart.aspx");
        }

        protected void ButtonChange_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedIndex >= 0)
            {
                Session["SEId"] = model.Id;
                Session["SEArticleId"] = model.ArticleId;
                Session["SEPartId"] = model.PartId;
                Session["SEPartName"] = model.PartName;
                Session["SECount"] = model.Count;
                Session["SEIs"] = dataGridView.SelectedIndex;
                Server.Transfer("FormArticlePart.aspx");
            }
        }

        protected void ButtonDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedIndex >= 0)
            {
                try
                {
                    productComponents.RemoveAt(dataGridView.SelectedIndex);
                }
                catch (Exception ex)
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('" + ex.Message + "');</script>");
                }
                LoadData();
            }
        }

        protected void ButtonUpd_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        protected void ButtonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxName.Text))
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('Заполните название');</script>");
                return;
            }
            if (string.IsNullOrEmpty(textBoxCost.Text))
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('Заполните цену');</script>");
                return;
            }
            if (productComponents == null || productComponents.Count == 0)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('Заполните компоненты');</script>");
                return;
            }
            try
            {
                List<ArticlePartBindingModel> productComponentBM = new List<ArticlePartBindingModel>();
                for (int i = 0; i < productComponents.Count; ++i)
                {
                    productComponentBM.Add(new ArticlePartBindingModel
                    {
                        Id = productComponents[i].Id,
                        ArticleId = productComponents[i].ArticleId,
                        PartId = productComponents[i].PartId,
                        Count = productComponents[i].Count
                    });
                }
                service.DelElement(service.GetList().Last().Id);
                if (Int32.TryParse((string)Session["id"], out id))
                {
                    service.UpdElement(new ArticleBindingModel
                    {
                        Id = id,
                        ArticleName = textBoxName.Text,
                        Cost = Convert.ToInt32(textBoxCost.Text),
                        ArticleParts = productComponentBM
                    });
                }
                else
                {
                    service.AddElement(new ArticleBindingModel
                    {
                        ArticleName = textBoxName.Text,
                        Cost = Convert.ToInt32(textBoxCost.Text),
                        ArticleParts = productComponentBM
                    });
                }
                Session["id"] = null;
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('Сохранение прошло успешно');</script>");
                Server.Transfer("FormArticles.aspx");
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('" + ex.Message + "');</script>");
            }
        }

        protected void ButtonCancel_Click(object sender, EventArgs e)
        {
            if (service.GetList().Count != 0 && service.GetList().Last().ArticleName == null)
            {
                service.DelElement(service.GetList().Last().Id);
            }
            Session["id"] = null;
            Server.Transfer("FormArticles.aspx");
        }

        protected void dataGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            e.Row.Cells[1].Visible = false;
            e.Row.Cells[2].Visible = false;
            e.Row.Cells[3].Visible = false;
        }
    }
}