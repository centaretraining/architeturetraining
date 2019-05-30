<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Cart.aspx.cs" Inherits="LegacyApp.Web.Cart" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
<form id="form1" runat="server">
    <h1>Cart</h1>
    <table>
        <asp:Repeater ID="CartRepeater" runat="server" OnItemCommand="CartRepeater_OnItemCommand">
            <ItemTemplate>
                <tr>
                    <td><%# DataBinder.Eval(Container.DataItem, "Name") %></td>
                    <td><%# ((decimal)DataBinder.Eval(Container.DataItem, "Price")).ToString("C") %></td>
                    <td><%# DataBinder.Eval(Container.DataItem, "Quantity") %></td>
                    <td><asp:Button ID="RemoveItemButton" Text="Remove" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CartItemId") %>' runat="server" /></td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
        <tr>
            <td align="right">Total:</td>
            <td><asp:Label runat="server" ID="CartTotalLabel" /></td>
            <td></td>
        </tr>
    </table>
    
    <asp:Button ID="MenuButton" Text="Add More Food" OnClick="MenuButton_OnClick" runat="server" />
    <asp:Button ID="CheckoutButton" Text="Check Out" OnClick="CheckoutButton_OnClick" runat="server" />
</form>
</body>
</html>
