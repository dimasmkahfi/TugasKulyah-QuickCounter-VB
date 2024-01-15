Imports System.Reflection.Emit

Public Class EditItemForm
    Public Property NewItemsID As String

    Private Sub EditItemForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Set the label text with the current Items_ID
        Label1.Text = $"Enter the new Items_ID for Item {NewItemsID}:"
    End Sub

    Private Sub OK_Click(sender As Object, e As EventArgs) Handles OK.Click
        ' Validate the input
        If Not String.IsNullOrEmpty(TextBox1.Text) Then
            ' Set the new Items_ID property
            NewItemsID = TextBox1.Text

            ' Set the DialogResult to OK
            Me.DialogResult = DialogResult.OK

            ' Close the form
            Me.Close()
        Else
            MessageBox.Show("Invalid input")
        End If
    End Sub


    Private Sub CancelButton_Click(sender As Object, e As EventArgs) Handles CancelButton.Click
        ' Set the new Items_ID property to an empty string
        NewItemsID = String.Empty
        ' Close the form
        Me.Close()
    End Sub
End Class