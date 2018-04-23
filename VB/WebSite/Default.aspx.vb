Imports Microsoft.VisualBasic
Imports System
Imports System.Data
Imports DevExpress.Web.ASPxGridView
Imports System.Collections

Partial Public Class _Default
	Inherits System.Web.UI.Page
	Private ReadOnly Property Data() As DataTable
		Get
'INSTANT VB NOTE: The local variable data was renamed since Visual Basic will not allow local variables with the same name as their enclosing function or property:
			Dim data_Renamed As DataTable = CType(Session("Data"), DataTable)
			If data_Renamed Is Nothing Then
				data_Renamed = New DataTable()
				data_Renamed.Columns.Add("ID", GetType(Int32))
				data_Renamed.Columns.Add("ProductID", GetType(Int32))
				data_Renamed.Columns.Add("Quantity", GetType(Int32))
				data_Renamed.Columns.Add("Description", GetType(String))
				Session("Data") = data_Renamed
			End If
			data_Renamed.PrimaryKey = New DataColumn() { data_Renamed.Columns("ID") }
			data_Renamed.Columns(0).AutoIncrement = True
			Return data_Renamed
		End Get
	End Property

	Protected Sub Page_Init(ByVal sender As Object, ByVal e As EventArgs)
		ASPxGridView1.DataBind()
	End Sub

	Protected Sub ASPxGridView1_RowDeleting(ByVal sender As Object, ByVal e As DevExpress.Web.Data.ASPxDataDeletingEventArgs)
		Data.Rows.Remove(Data.Rows.Find(e.Keys("ID")))

		ASPxGridView1.CancelEdit()
		e.Cancel = True
		ASPxGridView1.DataBind()
	End Sub

	Protected Sub ASPxGridView1_RowInserting(ByVal sender As Object, ByVal e As DevExpress.Web.Data.ASPxDataInsertingEventArgs)
		If e.NewValues("ProductID") IsNot Nothing Then
			Dim q As Int32 = 0
			If Int32.TryParse(e.NewValues("Quantity").ToString(), q) Then
				Data.Rows.Add(New Object() { e.NewValues("ID"), e.NewValues("ProductID"), q, e.NewValues("Description") })
			End If
		End If
		ASPxGridView1.CancelEdit()
		e.Cancel = True
		ASPxGridView1.DataBind()
	End Sub

	Protected Sub ASPxGridView1_RowUpdating(ByVal sender As Object, ByVal e As DevExpress.Web.Data.ASPxDataUpdatingEventArgs)
		Dim modified As Boolean = False
		For Each val As DictionaryEntry In e.OldValues
			If e.NewValues(val.Key) = val.Value Then
				modified = True
			End If
		Next val

		Dim q As Int32 = 0
		If modified AndAlso Int32.TryParse(e.NewValues("Quantity").ToString(), q) Then
			Dim row As DataRow = Data.Rows.Find(e.Keys("ID"))

			row("ProductID") = e.NewValues("ProductID")
			row("Quantity") = q
			row("Description") = e.NewValues("Description")
		End If

		ASPxGridView1.CancelEdit()
		e.Cancel = True
		ASPxGridView1.DataBind()
	End Sub

	Protected Sub ASPxGridView1_DataBinding(ByVal sender As Object, ByVal e As EventArgs)
		ASPxGridView1.DataSource = Data
		ASPxGridView1.KeyFieldName = "ID"
	End Sub

	Protected Sub ASPxGridView1_CommandButtonInitialize(ByVal sender As Object, ByVal e As ASPxGridViewCommandButtonEventArgs)
		e.Visible = e.VisibleIndex = -1
	End Sub
End Class