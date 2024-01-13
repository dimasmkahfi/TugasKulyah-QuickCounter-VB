Imports System.Windows.Forms.DataVisualization.Charting

Public Class Dashboard

    Private WithEvents chartRefreshTimer As New Timer()

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Open the connection
        OpenConnection()

        ' Fetch data from the database
        Dim dataTable As DataTable = SQLTable("SELECT Items_ID, Total_Qty FROM items")

        ' Close the connection

        ' Set the data source for the chart
        Chart1.DataSource = dataTable

        ' Set the X and Y values for the chart
        Chart1.Series("Series1").XValueMember = "Items_ID"
        Chart1.Series("Series1").YValueMembers = "Total_Qty"

        ' Refresh the chart to update the display
        Chart1.DataBind()

        chartRefreshTimer.Interval = 5000

        ' Start the timer
        chartRefreshTimer.Start()
    End Sub
    Private Sub chartRefreshTimer_Tick(sender As Object, e As EventArgs) Handles chartRefreshTimer.Tick
        ' Check if the chart is not disposed
        If Not Chart1.IsDisposed Then
            ' Refresh the chart
            Chart1.Refresh()
        Else
            ' Chart is disposed, handle accordingly (e.g., recreate the chart)
            ' For example, you can recreate the chart here:
            Chart1 = New Chart()
            ' Set up your chart properties and data binding
            ' ...
            ' Refresh the chart
            Chart1.Refresh()
        End If
    End Sub


    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim childForm1 As New ListItems()
        childForm1.Show()
        childForm1.MdiParent = MainPage
        childForm1.WindowState = FormWindowState.Normal
    End Sub
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim childForm2 As New CountingPage()
        childForm2.Show()
        childForm2.MdiParent = MainPage
        childForm2.WindowState = FormWindowState.Normal
    End Sub
    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Dim childForm3 As New AccountSettings()
        childForm3.Show()
        childForm3.MdiParent = MainPage
        childForm3.WindowState = FormWindowState.Normal
    End Sub

    'Exit Button
    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        ' Show a confirmation dialog
        Dim result As DialogResult = MessageBox.Show("Are you sure you want to exit?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

        ' Check the user's choice
        If result = DialogResult.Yes Then
            ' Close the application
            Application.Exit()
        End If
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Dim childForm4 As New TransactionLog()
        childForm4.Show()
        childForm4.MdiParent = MainPage
        childForm4.WindowState = FormWindowState.Normal
    End Sub






    Private Sub Chart1_Click(sender As Object, e As EventArgs) Handles Chart1.Click

    End Sub


End Class
