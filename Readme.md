<!-- default file list -->
*Files to look at*:

* [Default.aspx](./CS/WebSite/Default.aspx) (VB: [Default.aspx](./VB/WebSite/Default.aspx))
* [Default.aspx.cs](./CS/WebSite/Default.aspx.cs) (VB: [Default.aspx.vb](./VB/WebSite/Default.aspx.vb))
<!-- default file list end -->
# ASPxGridView - How to create live-editing


<p>This example allows inputting data by navigating with the Tab key. As soon as you filled the last cell, you can press Tab or Enter, and it will be saved to the database and a new row will be created. So, you do not need to use the mouse to create several rows.</p><p>If you press Update or Cancel when editing an existing record, the grid will finish editing and create a new row for you.</p><p><br />
To create this feature, we need to handle data operations of the grid, such as RowInserting, RowUpdating, RowDeleting. Here we will update our DataSource. In this example, data is stored in the DataTable, which is stored in a Session object.<br />
</p><p>Additionally, we need to add CustomButtons to have better control over operations a user can perform. We also need to handle the client-side RowClicked event to be able to start row editing when a user clicks another row.<br />
</p><p>Javascript keeps an empty row visible, except for the situation when a user edits an existing record.<br />
</p><p>In this example, only basic validation is performed. <br />
</p><p>Before updating, the function checks whether a value is selected in the ASPxComboBox. ASPxCombobox is used as an editor in the ProductID column. In this column, we store an ID of the product we need to request. When data row is in edit mode, a user can type an actual name of the product, instead of typing its ID. Additionally, we provide the auto complete feature, which will help find a product name in the long list of products.</p><p>To prevent the so-called "callback rush", we need to implement a queue for the callback using an Array object.</p>

<br/>


