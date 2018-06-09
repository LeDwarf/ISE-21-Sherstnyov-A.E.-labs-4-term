<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FormBuilder.aspx.cs" Inherits="AlexeysShopWebView.FormBuilder" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        ФИО
        <asp:TextBox ID="TextBoxFIO" runat="server" Width="266px"></asp:TextBox>
        <br />
        <asp:Button ID="ButtonSave" runat="server" OnClick="ButtonSave_Click" Text="Сохранить" />
        <asp:Button ID="ButtonCancel" runat="server" OnClick="ButtonCancel_Click" Text="Отмена" />
    
    </div>
    </form>
</body>
</html>
