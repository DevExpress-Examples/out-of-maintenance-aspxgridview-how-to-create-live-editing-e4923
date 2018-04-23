using System;
using System.Data;
using DevExpress.Web.ASPxGridView;
using System.Collections;

public partial class _Default : System.Web.UI.Page {
    private DataTable Data {
        get {
            DataTable data = (DataTable)Session["Data"];
            if (data == null) {
                data = new DataTable();
                data.Columns.Add("ID", typeof(Int32));
                data.Columns.Add("ProductID", typeof(Int32));
                data.Columns.Add("Quantity", typeof(Int32));
                data.Columns.Add("Description", typeof(String));
                Session["Data"] = data;
            }
            data.PrimaryKey = new DataColumn[] { data.Columns["ID"] };
            data.Columns[0].AutoIncrement = true;
            return data;
        }
    }

    protected void Page_Init(object sender, EventArgs e) {
        ASPxGridView1.DataBind();
    }

    protected void ASPxGridView1_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e) {
        Data.Rows.Remove(Data.Rows.Find(e.Keys["ID"]));

        ASPxGridView1.CancelEdit();
        e.Cancel = true;
        ASPxGridView1.DataBind();
    }

    protected void ASPxGridView1_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e) {
        if (e.NewValues["ProductID"] != null) {
            Int32 q = 0;
            if (Int32.TryParse(e.NewValues["Quantity"].ToString(), out q)) {
                Data.Rows.Add(new Object[] { e.NewValues["ID"], e.NewValues["ProductID"], q, e.NewValues["Description"] });
            }
        }
        ASPxGridView1.CancelEdit();
        e.Cancel = true;
        ASPxGridView1.DataBind();
    }

    protected void ASPxGridView1_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e) {
        Boolean modified = false;
        foreach (DictionaryEntry val in e.OldValues) {
            if (e.NewValues[val.Key] == val.Value) modified = true;
        }

        Int32 q = 0;
        if (modified && Int32.TryParse(e.NewValues["Quantity"].ToString(), out q)) {
            DataRow row = Data.Rows.Find(e.Keys["ID"]);

            row["ProductID"] = e.NewValues["ProductID"];
            row["Quantity"] = q;
            row["Description"] = e.NewValues["Description"];
        }

        ASPxGridView1.CancelEdit();
        e.Cancel = true;
        ASPxGridView1.DataBind();
    }

    protected void ASPxGridView1_DataBinding(object sender, EventArgs e) {
        ASPxGridView1.DataSource = Data;
        ASPxGridView1.KeyFieldName = "ID";
    }

    protected void ASPxGridView1_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e) {
        e.Visible = e.VisibleIndex == -1;
    }
}