<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FormGeneral.aspx.cs" Inherits="AlexeysShopWebView.FormGeneral" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <style type="text/css">
        #form1 {
            height: 666px;
            width: 1067px;
        }
    </style>
    <script src="https://unpkg.com/sweetalert/dist/sweetalert.min.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:Menu ID="Menu" runat="server" BackColor="White" ForeColor="Black" Height="150px">
            <Items>
                <asp:MenuItem Text="Справочники" Value="Справочники">
                    <asp:MenuItem Text="Клиенты" Value="Клиенты" NavigateUrl="~/FormCustomers.aspx"></asp:MenuItem>
                    <asp:MenuItem Text="Материалы" Value="Материалы" NavigateUrl="~/FormParts.aspx"></asp:MenuItem>
                    <asp:MenuItem Text="Изделия" Value="Изделия" NavigateUrl="~/FormArticles.aspx"></asp:MenuItem>
                    <asp:MenuItem Text="Склады" Value="Склады" NavigateUrl="~/FormStorages.aspx"></asp:MenuItem>
                    <asp:MenuItem Text="Сотрудники" Value="Сотрудники" NavigateUrl="~/FormBuilders.aspx"></asp:MenuItem>
                </asp:MenuItem>
                <asp:MenuItem Text="Пополнить склад" Value="Пополнить склад" NavigateUrl="~/FormPutOnStorage.aspx"></asp:MenuItem>
            </Items>
        </asp:Menu>
        <asp:Button ID="ButtonCreateContract" runat="server" Text="Создать заказ" OnClick="ButtonCreateContract_Click" />
        <asp:Button ID="ButtonTakeContractInWork" runat="server" Text="Отдать на выполнение" OnClick="ButtonTakeContractInWork_Click" />
        <asp:Button ID="ButtonContractReady" runat="server" Text="Заказ готов" OnClick="ButtonContractReady_Click" />
        <asp:Button ID="ButtonContractPayed" runat="server" Text="Заказ оплачен" OnClick="ButtonContractPayed_Click" />
        <asp:Button ID="ButtonUpd" runat="server" Text="Обновить список" OnClick="ButtonUpd_Click" />
       <asp:GridView ID="dataGridView1" runat="server"  ShowHeaderWhenEmpty="True" BackColor="White" BContractColor="#0477db" BContractWidth="3px" CellPadding="4" GridLines="Horizontal" BContractStyle="Double">
            <FooterStyle BackColor="White" ForeColor="#333333" />
            <HeaderStyle BackColor="#0477db" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#0477db" ForeColor="White" HorizontalAlign="Center" />
            <RowStyle BackColor="White" ForeColor="#333333" />
            <SelectedRowStyle BackColor="#339966" ForeColor="White" Font-Bold="True" />
            <SortedAscendingCellStyle BackColor="#F7F7F7" />
            <SortedAscendingHeaderStyle BackColor="#487575" />
            <SortedDescendingCellStyle BackColor="#E5E5E5" />
            <SortedDescendingHeaderStyle BackColor="#275353" />
        </asp:GridView>
    </form>
</body>
</html>
