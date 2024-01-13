Imports System.Diagnostics.Eventing
Imports System.Windows.Forms.VisualStyles.VisualStyleElement
Imports MySql.Data.MySqlClient

Public Class ListItems
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Assuming you have a MySqlConnection object named "mysqlConnection"
        Dim connectionString As String = "Server=localhost;user id=root;password=;database=tallycountervb"
        Dim queryItems As String = "SELECT Items_ID, Total_Qty FROM items"

        Dim dataTableItems As New DataTable()
        Dim dataTableTransactions As New DataTable()

        Using mysqlConnection As New MySqlConnection(connectionString)
            Using sqlCommandItems As New MySqlCommand(queryItems, mysqlConnection)
                Using adapterItems As New MySqlDataAdapter(sqlCommandItems)
                    mysqlConnection.Open()
                    adapterItems.Fill(dataTableItems)
                End Using
            End Using
        End Using

        ' Set the DataSource property of the DataGridView for items
        DataGridView1.DataSource = dataTableItems

        ' Populate ListView with data from the transaction table


    End Sub

    ' You can handle the CellClick event to get additional information when a cell is clicked
    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick
        If e.RowIndex >= 0 AndAlso e.ColumnIndex >= 0 Then
            Dim selectedRow As DataGridViewRow = DataGridView1.Rows(e.RowIndex)
            Dim itemsID As String = selectedRow.Cells("Items_ID").Value.ToString()
            Dim totalQty As String = selectedRow.Cells("Total_Qty").Value.ToString()

            MessageBox.Show($"Items_ID: {itemsID}, Total_Qty: {totalQty}")
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            ' Prompt the user to enter the item name
            Dim itemName As String = InputBox("Enter the item name:", "Add Item")

            ' Validate item name
            If String.IsNullOrEmpty(itemName) Then
                MessageBox.Show("Please enter a valid item name.")
                Return
            End If
            OpenConnection()

            ' Insert new item into the database
            Dim insertCommand As New MySqlCommand($"INSERT INTO items (Items_ID, Total_Qty) VALUES ('{itemName}', 0)", con)
            insertCommand.ExecuteNonQuery()

            ' Refresh DataGridView with updated item list
            RefreshDataGridView()

            ' Populate ListView and Dropdown
            CloseConnection()

            MessageBox.Show($"Item '{itemName}' added successfully.")
        Catch ex As Exception
            MessageBox.Show($"Error adding item: {ex.Message}")
        End Try
    End Sub

    Private Sub RefreshDataGridView()
        ' Refresh DataGridView with the latest data from the items table
        Dim queryItems As String = "SELECT * FROM items"

        Dim dataTableItems As New DataTable()

        Try
            Using sqlCommandItems As New MySqlCommand(queryItems, con)
                Using adapterItems As New MySqlDataAdapter(sqlCommandItems)
                    ' Open the connection
                    OpenConnection()

                    ' Fill the DataTable with the updated data
                    adapterItems.Fill(dataTableItems)
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show($"Error refreshing item list: {ex.Message}")
        Finally
            ' Close the connection
            CloseConnection()

        End Try

        ' Set the DataSource property of the DataGridView for items
        DataGridView1.DataSource = dataTableItems
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        ' Delete selected item
        If DataGridView1.SelectedRows.Count > 0 Then
            ' Show a confirmation dialog
            Dim result As DialogResult = MessageBox.Show("Are you sure you want to delete the selected item?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

            ' Check the user's choice
            If result = DialogResult.Yes Then
                ' Delete the selected item from the database
                Dim selectedRow As DataGridViewRow = DataGridView1.SelectedRows(0)
                Dim itemsID As String = selectedRow.Cells("Items_ID").Value.ToString()

                Try
                    OpenConnection()

                    ' Delete the item
                    Dim deleteCommand As New MySqlCommand($"DELETE FROM items WHERE Items_ID = '{itemsID}'", con)
                    deleteCommand.ExecuteNonQuery()

                    ' Refresh DataGridView with updated item list
                    RefreshDataGridView()

                    MessageBox.Show($"Item '{itemsID}' deleted successfully.")
                Catch ex As Exception
                    MessageBox.Show($"Error deleting item: {ex.Message}")
                Finally
                    ' Close the connection
                    CloseConnection()
                End Try
            End If
        Else
            MessageBox.Show("Please select an item to delete.")
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        ' Edit selected item
        If DataGridView1.SelectedRows.Count > 0 Then
            ' Assuming you have an EditItemForm for editing an item
            Dim selectedRow As DataGridViewRow = DataGridView1.SelectedRows(0)
            Dim itemsID As String = selectedRow.Cells("Items_ID").Value.ToString()
            Dim totalQty As String = selectedRow.Cells("Total_Qty").Value.ToString()

            ' Create an instance of the EditItemForm and pass the selected item information

            ' Refresh DataGridView with updated item list
            RefreshDataGridView()
        Else
            MessageBox.Show("Please select an item to edit.")
        End If
    End Sub

    Private Sub DataGridView1_SelectionChanged(sender As Object, e As EventArgs) Handles DataGridView1.SelectionChanged
        Button2.Enabled = DataGridView1.SelectedRows.Count > 0
        Button3.Enabled = DataGridView1.SelectedRows.Count > 0
    End Sub
End Class
