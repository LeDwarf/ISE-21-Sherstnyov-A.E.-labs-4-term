using AlexeysShopService.ImplementationsList;
using AlexeysShopService.Interfaces;
using AlexeysShopService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AlexeysShopWebView
{
    public partial class FormArticlePart : System.Web.UI.Page
    {
        private readonly IPartService service = new PartServiceList();

        private ArticlePartViewModel model;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                List<PartViewModel> list = service.GetList();
                if (list != null)
                {
                    DropDownListPart.DataSource = list;
                    DropDownListPart.DataValueField = "Id";
                    DropDownListPart.DataTextField = "PartName";
                    DropDownListPart.SelectedIndex = -1;
                    Page.DataBind();
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('" + ex.Message + "');</script>");
            }
            if (Session["SEId"] != null)
            {
                DropDownListPart.Enabled = false;
                DropDownListPart.SelectedValue = (string)Session["SEPartId"];
                TextBoxCount.Text = (string)Session["SECount"];
            }
        }

        protected void ButtonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TextBoxCount.Text))
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('Заполните поле Количество');</script>");
                return;
            }
            if (DropDownListPart.SelectedValue == null)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('Выберите компонент');</script>");
                return;
            }
            try
            {
                if (Session["SEId"] == null)
                {
                    model = new ArticlePartViewModel
                    {
                        PartId = Convert.ToInt32(DropDownListPart.SelectedValue),
                        PartName = DropDownListPart.SelectedItem.Text,
                        Count = Convert.ToInt32(TextBoxCount.Text)
                    };
                    Session["SEId"] = model.Id;
                    Session["SEArticleId"] = model.ArticleId;
                    Session["SEPartId"] = model.PartId;
                    Session["SEPartName"] = model.PartName;
                    Session["SECount"] = model.Count;
                }
                else
                {
                    model.Count = Convert.ToInt32(TextBoxCount.Text);
                    Session["SEId"] = model.Id;
                    Session["SEArticleId"] = model.ArticleId;
                    Session["SEPartId"] = model.PartId;
                    Session["SEPartName"] = model.PartName;
                    Session["SECount"] = model.Count;
                }
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('Сохранение прошло успешно');</script>");
                Server.Transfer("FormArticle.aspx");
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('" + ex.Message + "');</script>");
            }
        }

        protected void ButtonCancel_Click(object sender, EventArgs e)
        {
            Server.Transfer("FormArticle.aspx");
        }
    }
}