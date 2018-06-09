<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TakeContractInWork.aspx.cs" Inherits="AlexeysShopWebView.TakeContractInWork" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        Исполнитель&nbsp;
        <asp:DropDownList ID="DropDownListBuilder" runat="server" Height="24px" Width="223px">
        </asp:DropDownList>
        <br />
        <br />
        <asp:Button ID="ButtonSave" runat="server" OnClick="ButtonSave_Click" Text="Сохранить" />
        <asp:Button ID="ButtonCancel" runat="server" OnClick="ButtonCancel_Click" Text="Отмена" />
    
    </div>
    </form>
</body>
</html>
