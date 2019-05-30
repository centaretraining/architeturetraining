<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="LegacyApp.Web.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <table>
            <asp:Repeater ID="MenuRepeater" runat="server" OnItemCommand="MenuRepeater_OnItemCommand">
                <ItemTemplate>
                    <tr>
                        <td><%# DataBinder.Eval(Container.DataItem, "Name") %></td>
                        <td><%# ((decimal)DataBinder.Eval(Container.DataItem, "Price")).ToString("C") %></td>
                        <td><asp:Button ID="AddToOrderButton" Text="Add To Order" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ProductId") %>' runat="server" /></td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
        
        <div>
            <asp:Button ID="CartButton" Text="Cart" OnClick="CartButton_OnClick" runat="server" /> <asp:Label runat="server" ID="CartTotalLabel"></asp:Label>
        </div>
    </form>
</body>
</html>
