<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FormArticlePart.aspx.cs" Inherits="AlexeysShopWebView.FormArticlePart" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        Компонент&nbsp;&nbsp;
        <asp:DropDownList ID="DropDownListPart" runat="server" Height="16px" Width="200px">
        </asp:DropDownList>
        <br />
        <br />
        Количество&nbsp;
        <asp:TextBox ID="TextBoxCount" runat="server" Width="194px"></asp:TextBox>
        <br />
        <br />
        <asp:Button ID="ButtonSave" runat="server" OnClick="ButtonSave_Click" Text="Сохранить" />
        <asp:Button ID="ButtonCancel" runat="server" OnClick="ButtonCancel_Click" Text="Отмена" />
    
    </div>
    </form>
</body>
</html>
