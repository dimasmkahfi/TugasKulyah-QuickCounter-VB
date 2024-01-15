Public Class LoginForm1

    ' TODO: Insert code to perform custom authentication using the provided username and password 
    ' (See https://go.microsoft.com/fwlink/?LinkId=35339).  
    ' The custom principal can then be attached to the current thread's principal as follows: 
    '     My.User.CurrentPrincipal = CustomPrincipal
    ' where CustomPrincipal is the IPrincipal implementation used to perform authentication. 
    ' Subsequently, My.User will return identity information encapsulated in the CustomPrincipal object
    ' such as the username, display name, etc.

    Private Sub OK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK.Click
        ' Get the entered username and password from the textboxes
        Dim username As String = UsernameTextBox.Text
        Dim password As String = PasswordTextBox.Text

        ' Call the Login function
        Dim isLoggedIn As Boolean = Login(username, password)

        ' Check the result and take appropriate action
        If isLoggedIn Then
            MessageBox.Show("Login successful", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Me.Hide()

            Dim user As New User(username, password)
            Module1.LoggedInUser = user



            ' Close the login form (you can also hide it if needed)
            MainPage.Show()

            ' TODO: Perform actions for a successful login (e.g., show the main form, etc.)
            ' For example, you can show the main form:
            ' Dim mainForm As New MainForm()
            ' mainForm.Show()

        Else
            MessageBox.Show("Invalid username or password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)

            ' Clear the password textbox for security reasons
            PasswordTextBox.Clear()

            ' Optionally, set focus to the username textbox for user convenience
            UsernameTextBox.Focus()
        End If
    End Sub

    Private Sub Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel.Click
        Me.Close()
    End Sub

    Private Sub LoginForm1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        OpenConnection()


    End Sub
End Class
Public Class User
    Public Property Username As String
    Public Property Password As String

    ' Constructor that takes parameters for username and password
    Public Sub New(username As String, password As String)
        Me.Username = username
        Me.Password = password
    End Sub
End Class