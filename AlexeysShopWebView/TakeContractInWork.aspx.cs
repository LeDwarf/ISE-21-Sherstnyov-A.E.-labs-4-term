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
    public partial class TakeContractInWork : System.Web.UI.Page
    {
        private readonly IBuilderService serviceP = new BuilderServiceList();

        private readonly IGeneralService serviceM = new GeneralServiceList();

        private int id;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Int32.TryParse((string)Session["id"],out id))
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('Не указан заказ');</script>");
                    Server.Transfer("FormGeneral.aspx");
                }
                List<BuilderViewModel> listI = serviceP.GetList();
                if (listI != null)
                {
                    DropDownListBuilder.DataSource = listI;
                    DropDownListBuilder.DataBind();
                    DropDownListBuilder.DataTextField = "BuilderFIO";
                    DropDownListBuilder.DataValueField = "Id";
                    DropDownListBuilder.SelectedIndex = -1;
                }
                Page.DataBind();
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('" + ex.Message + "');</script>");
            }
        }

        protected void ButtonSave_Click(object sender, EventArgs e)
        {
            if (DropDownListBuilder.SelectedValue == null)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('Выберите исполнителя');</script>");
                return;
            }
            try
            {
                serviceM.TakeContractInWork(new ContractBindingModel
                {
                    Id = id,
                    BuilderId = Convert.ToInt32(DropDownListBuilder.SelectedValue)
                });
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('Сохранение прошло успешно');</script>");
                Session["id"] = null;
                Server.Transfer("FormGeneral.aspx");
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('" + ex.Message + "');</script>");
            }
        }

        protected void ButtonCancel_Click(object sender, EventArgs e)
        {
            Session["id"] = null;
            Server.Transfer("FormGeneral.aspx");
        }
    }
}