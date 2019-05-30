<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Checkout.aspx.cs" Inherits="LegacyApp.Web.Checkout" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <h1>Check Out</h1>
        <table>
            <asp:Repeater ID="CartRepeater" runat="server">
                <ItemTemplate>
                    <tr>
                        <td><%# DataBinder.Eval(Container.DataItem, "Name") %></td>
                        <td><%# ((decimal)DataBinder.Eval(Container.DataItem, "Price")).ToString("C") %></td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
            <tr>
                <td align="right">Total:</td>
                <td>
                    <asp:Label runat="server" ID="CartTotalLabel" /></td>
            </tr>
        </table>

        <div>
            First Name:
            <asp:TextBox runat="server" ID="FirstNameTextBox" />
            Last Name:
            <asp:TextBox runat="server" ID="LastNameTextBox" />
        </div>
        <div>
            Address:<br />
            <asp:TextBox runat="server" ID="AddressLine1TextBox" MaxLength="50" Columns="30" /><br />
            <asp:TextBox runat="server" ID="AddressLine2TextBox" MaxLength="50" Columns="30" /><br />
            <asp:TextBox runat="server" ID="CityTextBox" MaxLength="50" Columns="20" />, 
        <asp:TextBox runat="server" ID="StateTextBox" MaxLength="2" Columns="2" />
            <asp:TextBox runat="server" ID="ZipTextBox" MaxLength="5" Columns="5" />
        </div>

        <div>
            Credit Card:
            <asp:TextBox runat="server" ID="CreditCardNumberTextBox" MaxLength="20" Columns="18" /><br />
            Expiration (MM/YY):
            <asp:TextBox runat="server" ID="CreditCardExpirationDate" MaxLength="5" Columns="5"></asp:TextBox>
        </div>

        <div>
            <asp:Button ID="CheckoutButton" runat="server" OnClick="CheckoutButton_OnClick" Text="Place Order" />
        </div>

    </form>
</body>
</html>
