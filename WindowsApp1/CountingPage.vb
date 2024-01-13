Imports MySql.Data.MySqlClient

Public Class CountingPage
    Dim connectionString As String = "Server=localhost;user id=root;password=;database=tallycountervb"
    Dim mysqlConnection As MySqlConnection
    Dim sqlCommand As MySqlCommand
    Private Sub CountingPage_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        ' When the Dashboard form is closed, show it again
        Dim mainPage As MainPage = CType(MdiParent, MainPage)
        mainPage.ShowDashboardForm()
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Initialize MySqlConnection and MySqlCommand
        mysqlConnection = New MySqlConnection(connectionString)
        sqlCommand = New MySqlCommand("SELECT * FROM items", mysqlConnection)

        ' Open the connection
        mysqlConnection.Open()

        ' Execute the command to get data from the database
        Dim reader As MySqlDataReader = sqlCommand.ExecuteReader()

        ' Iterate through the records and create buttons dynamically

        ' Create a FlowLayoutPanel to host the buttons
        Dim flowLayoutPanel As New FlowLayoutPanel()
        flowLayoutPanel.Dock = DockStyle.Fill
        Controls.Add(flowLayoutPanel)

        ' Iterate through the records and create buttons dynamically
        While reader.Read()
            ' Create a Panel to group the components
            Dim panel As New Panel()
            panel.Width = 120
            panel.Height = 100

            ' Create a Label to display Items_ID
            Dim label As New Label()
            label.Text = "Items_ID: " & reader("Items_ID").ToString()
            label.Dock = DockStyle.Top
            label.TextAlign = ContentAlignment.MiddleCenter
            panel.Controls.Add(label)

            ' Create a Label to display the counter
            Dim counterLabel As New Label()
            counterLabel.Text = "Counter: " & reader("Total_Qty").ToString()
            counterLabel.Dock = DockStyle.Top
            counterLabel.TextAlign = ContentAlignment.MiddleCenter
            panel.Controls.Add(counterLabel)

            ' Create a Button with a "+" sign
            Dim plusButton As New Button()
            plusButton.Text = "+"
            plusButton.Width = 30
            plusButton.Height = 30
            plusButton.Dock = DockStyle.Top
            plusButton.TextAlign = ContentAlignment.MiddleCenter
            AddHandler plusButton.Click, Sub(sender0, e0) IncrementCounter(panel, 1)

            panel.Controls.Add(plusButton)

            ' Set the tag for Total_Qty
            panel.Tag = reader("Total_Qty") ' Assuming you have a column for the counter value

            ' Add a ContextMenuStrip to the panel
            Dim contextMenu As New ContextMenuStrip()
            contextMenu.Items.Add("Add Incrementally", Nothing, Sub(sender1, e1) IncrementCounter(panel, 1))
            contextMenu.Items.Add("Add by Value", Nothing, Sub(sender2, e2) AddByValue(panel))
            contextMenu.Items.Add("Update", Nothing, Sub(sender3, e3) UpdateCounterByInput(panel))
            panel.ContextMenuStrip = contextMenu

            ' Add the panel to the FlowLayoutPanel
            flowLayoutPanel.Controls.Add(panel)
        End While




        ' Close the data reader and connection
        reader.Close()
    End Sub
    Private Sub ContextMenuItem_Click(panel As Panel, actionType As String)
        Select Case actionType
            Case "AddIncrementally"
                IncrementCounter(panel, 1)
            Case "AddByValue"
                AddByValue(panel)
            Case "Update"
                UpdateCounterByInput(panel)
        End Select
    End Sub

    Private Sub IncrementCounter(panel As Panel, incrementBy As Integer)
        Dim counterLabel As Label = GetCounterLabel(panel)
        Dim counterValue As Integer = CInt(panel.Tag)
        counterValue += incrementBy
        panel.Tag = counterValue
        UpdateCounterInDatabase(GetItemsID(panel), counterValue, "AddIncrementally")
        counterLabel.Text = "Counter: " & counterValue.ToString()
        MessageBox.Show($"Item {GetItemsID(panel)} incremented by {incrementBy}. Counter: {counterValue}")
    End Sub

    Private Sub AddByValue(panel As Panel)
        Dim inputValue As String = InputBox($"Enter the value to add to Item {GetItemsID(panel)}:", "Add by Value")
        If Not String.IsNullOrEmpty(inputValue) AndAlso IsNumeric(inputValue) Then
            Dim incrementBy As Integer = CInt(inputValue)
            IncrementCounter(panel, incrementBy)
        Else
            MessageBox.Show("Invalid input. Please enter a numeric value.")
        End If
    End Sub





    Private Sub UpdateCounterByInput(panel As Panel)
        Dim inputValue As String = InputBox($"Enter the new value for Item {GetItemsID(panel)}:", "Update Counter")
        If Not String.IsNullOrEmpty(inputValue) AndAlso IsNumeric(inputValue) Then
            Dim newValue As Integer = CInt(inputValue)
            panel.Tag = newValue
            UpdateCounterInDatabase(GetItemsID(panel), newValue, "Update")
            GetCounterLabel(panel).Text = "Counter: " & newValue.ToString()
            MessageBox.Show($"Item {GetItemsID(panel)} updated. New Counter: {newValue}")
        Else
            MessageBox.Show("Invalid input. Please enter a numeric value.")
        End If
    End Sub

    Private Function GetCounterLabel(panel As Panel) As Label
        ' Find the counter label within the panel
        Return panel.Controls.OfType(Of Label)().FirstOrDefault(Function(lbl) lbl.Text.StartsWith("Counter: "))
    End Function

    Private Function GetItemsID(panel As Panel) As String
        ' Find the Items_ID label within the panel
        Dim itemsIDLabel As Label = panel.Controls.OfType(Of Label)().FirstOrDefault(Function(lbl) lbl.Text.StartsWith("Items_ID: "))
        If itemsIDLabel IsNot Nothing Then
            Return itemsIDLabel.Text.Substring("Items_ID: ".Length)
        Else
            Return String.Empty
        End If
    End Function


    Private Sub UpdateCounterInDatabase(itemsID As String, counterValue As Integer, actionType As String)
        Try
            Dim transactionType As String = ""

            ' Update the counter value in the master items table
            Dim updateMasterItemsCommand As New MySqlCommand($"UPDATE items SET Total_Qty = {counterValue} WHERE Items_ID = '{itemsID}'", mysqlConnection)
            Dim rowsUpdatedMasterItems As Integer = updateMasterItemsCommand.ExecuteNonQuery()

            Select Case actionType
                Case "AddIncrementally"
                    transactionType = "Increment"
                Case "AddByValue"
                    transactionType = "ByValue"
                Case "Update"
                    transactionType = "Update"
            End Select

            If rowsUpdatedMasterItems > 0 Then
                ' Insert a log record into the transactions table
                Dim insertLogCommand As New MySqlCommand(
                    $"INSERT INTO transaction (Transaction_Type, Items_ID, Depot_ID, Status, Updated_Quantity, Create_Date, Create_By, Last_Update, Last_Update_By) " &
                    $"VALUES ('{transactionType}', '{itemsID}', 'YourDepotID', 'YourStatus', {counterValue}, NOW(), 'YourCreateBy', NOW(), 'YourUpdateBy')",
                    mysqlConnection
                )

                Dim rowsInsertedLog As Integer = insertLogCommand.ExecuteNonQuery()

                If rowsInsertedLog > 0 Then
                    MessageBox.Show($"Item {itemsID} {transactionType} successful. Log record inserted.")
                Else
                    MessageBox.Show($"Item {itemsID} {transactionType} successful. Log record not inserted.")
                End If
            Else
                MessageBox.Show($"No records updated for Item {itemsID}.")
            End If
        Catch ex As Exception
            MessageBox.Show($"Error interacting with the database: {ex.Message}")
        End Try
    End Sub


End Class
