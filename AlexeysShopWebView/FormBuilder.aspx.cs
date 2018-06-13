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
    public partial class FormBuilder : System.Web.UI.Page
    {
        private readonly IBuilderService service = new BuilderServiceList();

        private int id;

        private string name;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Int32.TryParse((string)Session["id"], out id))
            {
                try
                {
                    BuilderViewModel view = service.GetElement(id);
                    if (view != null)
                    {
                        name = view.BuilderFIO;
                        service.UpdElement(new BuilderBindingModel
                        {
                            Id = id,
                            BuilderFIO = ""
                        });
                        if (!string.IsNullOrEmpty(name) && string.IsNullOrEmpty(TextBoxFIO.Text))
                        {
                            TextBoxFIO.Text = name;
                        }
                        service.UpdElement(new BuilderBindingModel
                        {
                            Id = id,
                            BuilderFIO = name
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
            if (string.IsNullOrEmpty(TextBoxFIO.Text))
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('Заполните ФИО');</script>");
                return;
            }
            try
            {
                if (Int32.TryParse((string)Session["id"], out id))
                {
                    service.UpdElement(new BuilderBindingModel
                    {
                        Id = id,
                        BuilderFIO = TextBoxFIO.Text
                    });
                }
                else
                {
                    service.AddElement(new BuilderBindingModel
                    {
                        BuilderFIO = TextBoxFIO.Text
                    });
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('" + ex.Message + "');</script>");
                Server.Transfer("FormBuilders.aspx");
            }
            Session["id"] = null;
            Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('Сохранение прошло успешно');</script>");
            Server.Transfer("FormBuilders.aspx");
        }

        protected void ButtonCancel_Click(object sender, EventArgs e)
        {
            Session["id"] = null;
            Server.Transfer("FormBuilders.aspx");
        }
    }
}