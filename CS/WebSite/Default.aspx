<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
        var callbackQueue = [];

        function OnInit(s, e) {
            QueueCallback(function() { grid.AddNewRow(); });
        }

        function OnRowClick(s, e) {
            if (grid.IsEditing()) TryUpdate(false);

            QueueCallback(function () { grid.StartEditRow(e.visibleIndex); });
            e.htmlEvent.cancelBubble = false;
        }

        function OnBtnClick(s, e) {
            switch (e.buttonID) {
                case "updateBtn":
                case "cancelBtn":
                    TryUpdate(true);
                    break;
                case "deleteBtn":
                    QueueCallback(function () { grid.DeleteRow(e.visibleIndex); });
                    QueueCallback(function () { grid.AddNewRow(); });
                    break;
            }
        }

        function ClearEditors() {
            if (grid.IsNewRowEditing()) {
                pcmb.SetValue(null);
                qtxt.SetValue(null);
                dtxt.SetValue(null);
            }
        }

        function OnKeyPress(s, e, last) {
            if ((last == true && e.htmlEvent.keyCode == 9) || e.htmlEvent.keyCode == 13) {
                ASPxClientUtils.PreventEventAndBubble(e.htmlEvent);
                TryUpdate(true);
            }
        }

        function OnEndCallback(s, e) {
            if (callbackQueue.length > 0) callbackQueue.shift()();
        }

        function TryUpdate(createNew) {
            if (pcmb.GetValue()) QueueCallback(function () { grid.UpdateEdit(); });
            if (createNew && !grid.IsNewRowEditing()) QueueCallback(function () { grid.AddNewRow(); });
        }

        function QueueCallback(func) {
            grid.InCallback() ? callbackQueue.push(func) : func();
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <dx:ASPxGridView ID="ASPxGridView1" runat="server" OnCommandButtonInitialize="ASPxGridView1_CommandButtonInitialize" ClientInstanceName="grid" Width="100%" SettingsEditing-NewItemRowPosition="Bottom" Theme="BlackGlass" OnDataBinding="ASPxGridView1_DataBinding" SettingsEditing-Mode="Inline" AutoGenerateColumns="False" OnRowDeleting="ASPxGridView1_RowDeleting" OnRowInserting="ASPxGridView1_RowInserting" OnRowUpdating="ASPxGridView1_RowUpdating">
            <Columns>
                <dx:GridViewCommandColumn VisibleIndex="0" Width="32" ButtonType="Image" ShowNewButton="true">
                    <CustomButtons>
                        <dx:GridViewCommandColumnCustomButton Image-Url="Images/update.png" ID="updateBtn" Visibility="EditableRow">
                        </dx:GridViewCommandColumnCustomButton>
                        <dx:GridViewCommandColumnCustomButton Image-Url="Images/cancel.png" ID="cancelBtn" Visibility="EditableRow">
                        </dx:GridViewCommandColumnCustomButton>
                        <dx:GridViewCommandColumnCustomButton Image-Url="Images/delete.png" ID="deleteBtn" Visibility="BrowsableRow">
                        </dx:GridViewCommandColumnCustomButton>
                    </CustomButtons>
                </dx:GridViewCommandColumn>
                <dx:GridViewDataColumn VisibleIndex="2" FieldName="ProductID">
                    <EditItemTemplate>
                        <dx:ASPxComboBox ClientInstanceName="pcmb" ID="ASPxComboBox1" Width="100%" IncrementalFilteringMode="StartsWith" runat="server" Value='<%# Bind("ProductID")%>' ClientVisible="true" ValueField="ProductID" TextField="ProductName" DataSourceID="SqlDataSource1" ValueType="System.Int32">
                            <ClientSideEvents Init="function(s, e) { ClearEditors(); s.SetFocus(); }" KeyPress="OnKeyPress" />
                        </dx:ASPxComboBox>
                    </EditItemTemplate>
                </dx:GridViewDataColumn>
                <dx:GridViewDataColumn VisibleIndex="3" FieldName="Quantity">
                    <EditItemTemplate>
                        <dx:ASPxTextBox ClientInstanceName="qtxt" ID="QTxt" runat="server" ClientVisible="true" Text='<%# Bind("Quantity") %>' Width="100%">
                            <ClientSideEvents KeyPress="OnKeyPress" />
                        </dx:ASPxTextBox>
                    </EditItemTemplate>
                </dx:GridViewDataColumn>
                <dx:GridViewDataColumn VisibleIndex="4" FieldName="Description">
                    <EditItemTemplate>
                        <dx:ASPxTextBox ClientInstanceName="dtxt" ID="DTxt" runat="server" ClientVisible="true" Text='<%# Bind("Description") %>' Width="100%">
                            <ClientSideEvents KeyPress="function (s, e) { OnKeyPress(s, e, true); }" />
                        </dx:ASPxTextBox>
                    </EditItemTemplate>
                </dx:GridViewDataColumn>
            </Columns>
            <ClientSideEvents Init="OnInit" RowClick="OnRowClick" CustomButtonClick="OnBtnClick" EndCallback="OnEndCallback" />
            <SettingsCommandButton>
                <NewButton Image-Url="Images/add.png"/>
            </SettingsCommandButton>
        </dx:ASPxGridView>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:NorthwindConnectionString %>" SelectCommand="SELECT [ProductID], [ProductName] FROM [Products]"></asp:SqlDataSource>
    </form>
</body>
</html>
