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
    public partial class FormPutOnStorage : System.Web.UI.Page
    {
        private readonly IStorageService serviceS = new StorageServiceList();

        private readonly IPartService serviceE = new PartServiceList();

        private readonly IGeneralService serviceM = new GeneralServiceList();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                List<PartViewModel> listE = serviceE.GetList();
                if (listE != null)
                {
                    DropDownListStorage.DataSource = listE;
                    DropDownListStorage.DataBind();
                    DropDownListStorage.DataTextField = "PartName";
                    DropDownListStorage.DataValueField = "Id";
                }
                List<StorageViewModel> listS = serviceS.GetList();
                if (listS != null)
                {
                    DropDownListPart.DataSource = listS;
                    DropDownListPart.DataBind();
                    DropDownListPart.DataTextField = "StorageName";
                    DropDownListPart.DataValueField = "Id";
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
            if (DropDownListStorage.SelectedValue == null)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('Выберите склад');</script>");
                return;
            }
            try
            {
                serviceM.PutPartOnStorage(new StoragePartBindingModel
                {
                    PartId = Convert.ToInt32(DropDownListPart.SelectedValue),
                    StorageId = Convert.ToInt32(DropDownListStorage.SelectedValue),
                    Count = Convert.ToInt32(TextBoxCount.Text)
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