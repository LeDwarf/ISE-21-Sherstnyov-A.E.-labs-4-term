using AlexeysShopService.ImplementationsList;
using AlexeysShopService.Interfaces;
using AlexeysShopService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AlexeysShopWebView
{
    public partial class FormGeneral : System.Web.UI.Page
    {
        private readonly IGeneralService service = new GeneralServiceList();

        List<ContractViewModel> list;

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                list = service.GetList();
                dataGridView1.DataSource = list;
                dataGridView1.AutoGenerateSelectButton = true;
                dataGridView1.DataBind();
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('" + ex.Message + "');</script>");
            }
        }

        protected void ButtonCreateContract_Click(object sender, EventArgs e)
        {
            Server.Transfer("FormContract.aspx");
        }

        protected void ButtonTakeContractInWork_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedIndex >= 0)
            {
                string index = list[dataGridView1.SelectedIndex].Id.ToString();
                Session["id"] = index;
                Server.Transfer("TakeContractInWork.aspx");
            }
        }

        protected void ButtonContractReady_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedIndex >= 0)
            {
                int id = list[dataGridView1.SelectedIndex].Id;
                try
                {
                    service.FinishContract(id);
                    LoadData();
                    Server.Transfer("FormGeneral.aspx");
                }
                catch (Exception ex)
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('" + ex.Message + "');</script>");
                }
            }
        }

        protected void ButtonContractPayed_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedIndex >= 0)
            {
                int id = list[dataGridView1.SelectedIndex].Id;
                try
                {
                    service.PayContract(id);
                    LoadData();
                    Server.Transfer("FormGeneral.aspx");
                }
                catch (Exception ex)
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('" + ex.Message + "');</script>");
                }
            }
        }

        protected void ButtonUpd_Click(object sender, EventArgs e)
        {
            LoadData();
            Server.Transfer("FormGeneral.aspx");
        }
    }
}