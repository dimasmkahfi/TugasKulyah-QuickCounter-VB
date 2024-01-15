Imports MySql.Data.MySqlClient

Public Class TransactionLog
    Private Sub PopulateDropdown()
        ' Clear existing items in the dropdown
        ComboBox1.Items.Clear()

        ' Add a default item to show all transactions
        ComboBox1.Items.Add("All Transactions")

        ' Query to get distinct Transaction_Type values
        Dim query As String = "SELECT DISTINCT Transaction_Type FROM transaction"

        Using cmd As New MySqlCommand(query, con)
            OpenConnection()
            Dim reader As MySqlDataReader = cmd.ExecuteReader()

            While reader.Read()
                ' Add distinct Transaction_Type values to the dropdown
                ComboBox1.Items.Add(reader("Transaction_Type").ToString())
            End While

            ' Close the data reader and connection
            reader.Close()
            CloseConnection()
        End Using

        ' Set the default selection to show all transactions
        ComboBox1.SelectedIndex = 0
    End Sub
    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs)
        ' Filter ListView based on the selected Transaction_Type
        FilterListView()
    End Sub

    Private Sub FilterListView()
        ' Get the selected Transaction_Type from the dropdown
        Dim selectedTransactionType As String = ComboBox1.SelectedItem.ToString()

        ' If "All Transactions" is selected, show all items; otherwise, filter by Transaction_Type
        If selectedTransactionType = "All Transactions" Then
            ' Show all items
            PopulateListView()
        Else
            ' Filter by Transaction_Type
            Dim query As String = $"SELECT * FROM transaction WHERE Transaction_Type = '{selectedTransactionType}'"

            Using cmd As New MySqlCommand(query, con)
                ' Clear existing items in the ListView
                ListView1.Items.Clear()

                ' Buka koneksi, jalankan query, dan baca hasil
                OpenConnection()
                Dim reader As MySqlDataReader = cmd.ExecuteReader()

                ' Add rows to the ListView
                While reader.Read()
                    ' Assuming the order of columns is Transaction_Type, Items_ID, Depot_ID, Status, Updated_Quantity, Create_Date, Create_By, Last_Update, Last_Update_By
                    Dim item As New ListViewItem(reader("Transaction_Type").ToString())
                    item.SubItems.Add(reader("Items_ID").ToString())
                    item.SubItems.Add(reader("Depot_ID").ToString())
                    item.SubItems.Add(reader("Status").ToString())
                    item.SubItems.Add(reader("Updated_Quantity").ToString())
                    item.SubItems.Add(reader("Create_Date").ToString())
                    item.SubItems.Add(reader("Create_By").ToString())
                    item.SubItems.Add(reader("Last_Update").ToString())
                    item.SubItems.Add(reader("Last_Update_By").ToString())

                    ' Tambahkan item ke dalam ListView
                    ListView1.Items.Add(item)
                End While

                ' Close the data reader and connection
                reader.Close()
                CloseConnection()
            End Using
        End If
    End Sub

    Private Sub PopulateListView()
        ' Assuming you have a ListView named "ListView1"
        ListView1.Items.Clear()

        ListView1.Columns.Add("Transaction_Type", 120)
        ListView1.Columns.Add("Items_ID", 80)
        ListView1.Columns.Add("Status", 80)
        ListView1.Columns.Add("Updated_Quantity", 100)
        ListView1.Columns.Add("Create_Date", 120)
        ListView1.Columns.Add("Create_By", 100)
        ListView1.Columns.Add("Last_Update", 120)
        ListView1.Columns.Add("Last_Update_By", 100)

        ' Atur properti GridLines
        ListView1.GridLines = True
        ListView1.View = View.Details

        Dim query As String = "SELECT * FROM transaction"

        Using cmd As New MySqlCommand(query, con)
            ' Buka koneksi, jalankan query, dan baca hasil
            OpenConnection()
            Dim reader As MySqlDataReader = cmd.ExecuteReader()

            ' Add rows to the ListView
            While reader.Read()
                Dim item As New ListViewItem(reader("Transaction_Type").ToString())
                item.SubItems.Add(reader("Items_ID").ToString())
                item.SubItems.Add(reader("Status").ToString())
                item.SubItems.Add(reader("Updated_Quantity").ToString())
                item.SubItems.Add(reader("Create_Date").ToString())
                item.SubItems.Add(reader("Create_By").ToString())
                item.SubItems.Add(reader("Last_Update").ToString())
                item.SubItems.Add(reader("Last_Update_By").ToString())

                ' Tambahkan item ke dalam ListView
                ListView1.Items.Add(item)
            End While

            ' Close the data reader and connection
            reader.Close()
            CloseConnection()
        End Using
    End Sub

    Private Sub TransactionLog_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        PopulateListView()
        PopulateDropdown()
    End Sub
End Class