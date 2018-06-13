using AlexeysShopService.BindingModels;
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
    public partial class FormContract : System.Web.UI.Page
    {
        private readonly ICustomerService serviceC=new CustomerServiceList();

        private readonly IArticleService serviceS = new ArticleServiceList();

        private readonly IGeneralService serviceM = new GeneralServiceList();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                List<CustomerViewModel> listC = serviceC.GetList();
                if (listC != null)
                {
                    DropDownListCustomer.DataSource = listC;
                    DropDownListCustomer.DataBind();
                    DropDownListCustomer.DataTextField = "CustomerFIO";
                    DropDownListCustomer.DataValueField = "Id";
                }
                List<ArticleViewModel> listP = serviceS.GetList();
                if (listP != null)
                {
                    DropDownListArticle.DataSource = listP;
                    DropDownListArticle.DataBind();
                    DropDownListArticle.DataTextField = "ArticleName";
                    DropDownListArticle.DataValueField = "Id";
                }
                Page.DataBind();
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('" + ex.Message + "');</script>");
            }
        }

        private void CalcSum()
        {
            
            if (DropDownListArticle.SelectedValue != null && !string.IsNullOrEmpty(TextBoxCount.Text))
            {
                try
                {
                    int id = Convert.ToInt32(DropDownListArticle.SelectedValue);
                    ArticleViewModel product = serviceS.GetElement(id);
                    int count = Convert.ToInt32(TextBoxCount.Text);
                    TextBoxSum.Text = (count * product.Cost).ToString();
                }
                catch (Exception ex)
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('" + ex.Message + "');</script>");
                }
            }
        }

        protected void DropDownListArticle_SelectedIndexChanged(object sender, EventArgs e)
        {
            CalcSum();
        }

        protected void TextBoxCount_TextChanged(object sender, EventArgs e)
        {
            CalcSum();
        }

        protected void ButtonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TextBoxCount.Text))
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('Заполните поле Количество');</script>");
                return;
            }
            if (DropDownListCustomer.SelectedValue == null)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('Выберите клиента');</script>");
                return;
            }
            if (DropDownListArticle.SelectedValue == null)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('Выберите изделие');</script>");
                return;
            }
            try
            {
                serviceM.CreateContract(new ContractBindingModel
                {
                    CustomerId = Convert.ToInt32(DropDownListCustomer.SelectedValue),
                    ArticleId = Convert.ToInt32(DropDownListArticle.SelectedValue),
                    Count = Convert.ToInt32(TextBoxCount.Text),
                    Cost = Convert.ToInt32(TextBoxSum.Text)
                });
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('Сохранение прошло успешно');</script>");
                Server.Transfer("FormGeneral.aspx");
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('" + ex.Message + "');</script>");
            }
        }

        protected void ButtonCancel_Click(object sender, EventArgs e)
        {
            Server.Transfer("FormGeneral.aspx");
        }
    }
}