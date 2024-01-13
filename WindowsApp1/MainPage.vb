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

End Class
