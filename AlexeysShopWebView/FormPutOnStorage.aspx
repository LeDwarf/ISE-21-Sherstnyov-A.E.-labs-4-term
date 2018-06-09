<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FormPutOnStorage.aspx.cs" Inherits="AlexeysShopWebView.FormPutOnStorage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        Склад&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:DropDownList ID="DropDownListStorage" runat="server" Width="196px">
        </asp:DropDownList>
        <br />
        <br />
        Компонент&nbsp;&nbsp;
        <asp:DropDownList ID="DropDownListPart" runat="server" Height="17px" Width="194px">
        </asp:DropDownList>
        <br />
        <br />
        Количество&nbsp;
        <asp:TextBox ID="TextBoxCount" runat="server" Width="186px"></asp:TextBox>
        <br />
        <br />
        <asp:Button ID="ButtonSave" runat="server" OnClick="ButtonSave_Click" Text="Сохранить" />
        <asp:Button ID="ButtonCancel" runat="server" OnClick="ButtonCancel_Click" Text="Отмена" />
    
    </div>
    </form>
</body>
</html>
