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
using Unity;
using Unity.Attributes;

namespace AlexeysShopWebView
{
    public partial class FormCustomer : System.Web.UI.Page
    {
        public int Id { set { id = value; } }

        private readonly ICustomerService service = new CustomerServiceList();

        private int id;

        private string name;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Int32.TryParse((string)Session["id"], out id))
            {
                try
                {
                    CustomerViewModel view = service.GetElement(id);
                    if (view != null)
                    {
                        name = view.CustomerFIO;
                        service.UpdElement(new CustomerBindingModel
                        {
                            Id = id,
                            CustomerFIO = ""
                        });
                        if (!string.IsNullOrEmpty(name) && string.IsNullOrEmpty(textBoxFIO.Text))
                        {
                            textBoxFIO.Text = name;
                        }
                        service.UpdElement(new CustomerBindingModel
                        {
                            Id = id,
                            CustomerFIO = name
                        });
                    }
                }
                catch (Exception ex)
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('" + ex.Message + "');</script>");
                }
            }
        }

        protected void ButtonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxFIO.Text))
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('Заполните ФИО');</script>");
                return;
            }
            try
            {
                if (Int32.TryParse((string)Session["id"], out id))
                {
                    service.UpdElement(new CustomerBindingModel
                    {
                        Id = id,
                        CustomerFIO = textBoxFIO.Text
                    });
                }
                else
                {
                    service.AddElement(new CustomerBindingModel
                    {
                        CustomerFIO = textBoxFIO.Text
                    });
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('" + ex.Message + "');</script>");
                Server.Transfer("FormCustomers.aspx");
            }
            Session["id"] = null;
            Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('Сохранение прошло успешно');</script>");
            Server.Transfer("FormCustomers.aspx");
        }

        protected void ButtonCancel_Click(object sender, EventArgs e)
        {
            Session["id"] = null;
            Server.Transfer("FormCustomers.aspx");
        }
    }
}