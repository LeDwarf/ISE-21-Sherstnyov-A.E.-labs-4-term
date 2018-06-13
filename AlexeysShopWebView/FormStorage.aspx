<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FormStorage.aspx.cs" Inherits="AlexeysShopWebView.FormStorage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        Название&nbsp;
        <asp:TextBox ID="textBoxName" runat="server" Width="208px"></asp:TextBox>
        <asp:GridView ID="dataGridView" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="True">
            <Columns>
                <asp:CommandField ShowSelectButton="true" SelectText=">>" />
            </Columns>
            <SelectedRowStyle BackColor="#CCCCCC" />
        </asp:GridView>
        <asp:Button ID="ButtonSave" runat="server" Text="Сохранить" OnClick="ButtonSave_Click" />
        <asp:Button ID="ButtonCancel" runat="server" Text="Отмена" OnClick="ButtonCancel_Click" />
    
    </div>
    </form>
</body>
</html>
