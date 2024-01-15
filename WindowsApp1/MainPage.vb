Public Class MainPage
    Private Sub MainPage_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Create an instance of the Dashboard form
        Dim dashboardForm As New Dashboard()

        ' Set the Dashboard form as MDI child
        dashboardForm.MdiParent = Me

        ' Maximize the Dashboard form
        dashboardForm.WindowState = FormWindowState.Maximized

        ' Show the Dashboard form
        dashboardForm.Show()
    End Sub
    Public Sub ShowDashboardForm()
        ' Check if the Dashboard form is already open
        Dim isDashboardOpen As Boolean = False

        For Each form As Form In Me.MdiChildren
            If TypeOf form Is Dashboard Then
                isDashboardOpen = True
                Exit For
            End If
        Next

        ' If Dashboard form is not open, create and show it
        If Not isDashboardOpen Then
            Dim dashboardForm As New Dashboard()
            dashboardForm.MdiParent = Me
            dashboardForm.WindowState = FormWindowState.Maximized
            dashboardForm.Show()
        End If
    End Sub

    Public Sub ShowOrAlertDashboard()
        ' Check if the Dashboard form is already open
        Dim isDashboardOpen As Boolean = False

        For Each form As Form In Me.MdiChildren
            If TypeOf form Is Dashboard Then
                isDashboardOpen = True
                Exit For
            End If
        Next

        ' If Dashboard form is open, show an alert; otherwise, show the form
        If isDashboardOpen Then
            MessageBox.Show("Dashboard is already open.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else
            ShowDashboardForm()
        End If
    End Sub



    Private Sub DashboardToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DashboardToolStripMenuItem.Click
        ' Call the function to show or alert the Dashboard form
        ShowOrAlertDashboard()
    End Sub

    Private Sub AccountSettingsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AccountSettingsToolStripMenuItem.Click
        ' Call the function to show or alert the Dashboard form
        Dim Accountsettings As New AccountSettings()

        ' Set the Dashboard form as MDI child
        Accountsettings.MdiParent = Me

        ' Show the Dashboard form
        Accountsettings.Show()
    End Sub

    Private Sub LogoutToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LogoutToolStripMenuItem.Click
        ' Call the function to show or alert the Dashboard form
        Dim result As DialogResult = MessageBox.Show("Are you sure you want to exit?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

        ' Check the user's choice
        If result = DialogResult.Yes Then
            ' Close the application
            Application.Exit()
        End If
    End Sub
End Class
