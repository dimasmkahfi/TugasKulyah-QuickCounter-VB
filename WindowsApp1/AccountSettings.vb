Imports System.Diagnostics.Contracts
Imports MySql.Data.MySqlClient

Public Class AccountSettings
    ' Add a private variable to store the logged-in username
    Dim loggedInUser As User = Module1.LoggedInUser


    ' Public method to enable/disable controls
    Public Sub EnableControls(enable As Boolean)
        TextBox1.Enabled = enable
        RichTextBox1.Enabled = enable
        TextBox2.Enabled = enable
        TextBox3.Enabled = enable
        RadioButtonMale.Enabled = enable
        RadioButtonFemale.Enabled = enable
        CheckBox1.Enabled = enable
        GroupBox1.Enabled = enable

        ' Add other controls as needed
    End Sub

    ' Public method to show/hide the "Simpan" button
    Public Sub ShowSaveButton(show As Boolean)
        ' Assuming "Simpan" is a button named ButtonSave
        Save.Visible = show
    End Sub

    ' Public method to show/hide the "Batal" (Cancel) button
    Public Sub ShowCancelButton(show As Boolean)
        ' Assuming "Batal" is a button named ButtonCancel
        Cancel.Visible = True
    End Sub

    ' Public method to show/hide the "Ubah" (Edit) button
    Public Sub ShowEditButton(show As Boolean)
        ' Assuming "Ubah" is a button named ButtonEdit
        Ubah.Visible = show
    End Sub

    ' Public method to set the logged-in username


    ' Private method to fetch data from the database and populate controls
    Private Sub FetchDataFromDatabase()
        ' Replace "YourConnectionString" with your MySQL connection string

        ' Replace "YourTableName" with the name of your user table
        OpenConnection()

        Dim query As String = "SELECT Officer_ID, Password, Officer_Name, Gender,Address, IsActive FROM users WHERE Officer_ID = @Username"
        Using cmd As New MySqlCommand(query, con)
            cmd.Parameters.AddWithValue("@Username", loggedInUser.Username)

            Using reader As MySqlDataReader = cmd.ExecuteReader()
                If reader.Read() Then
                    TextBox1.Text = reader("Officer_ID").ToString()
                    TextBox2.Text = reader("Password").ToString()
                    TextBox3.Text = reader("Officer_Name").ToString()
                    RichTextBox1.Text = reader("Address").ToString()

                    ' Fetch and set gender
                    Dim gender As String = reader("Gender").ToString()
                    If gender = "L" Then
                        RadioButtonMale.Checked = True
                    ElseIf gender = "P" Then
                        RadioButtonFemale.Checked = True
                    End If

                    ' Fetch and set IsActive
                    Dim isActive As Boolean = Convert.ToBoolean(reader("IsActive"))
                    CheckBox1.Checked = isActive

                    ' Add other controls as needed
                End If
            End Using
        End Using
    End Sub

    Public Sub SaveChanges()

        Dim query As String = "UPDATE users SET Officer_ID = @OfficerID, Password = @Password, Officer_Name = @Fullname, Address = @Address, Gender = @Gender, IsActive = @IsActive WHERE Officer_ID = @Username"

        Try
            OpenConnection()

            Using cmd As New MySqlCommand(query, con)
                ' Set parameters based on form controls
                cmd.Parameters.AddWithValue("@OfficerID", TextBox1.Text)
                cmd.Parameters.AddWithValue("@Password", TextBox2.Text)
                cmd.Parameters.AddWithValue("@Fullname", TextBox3.Text)
                cmd.Parameters.AddWithValue("@Address", RichTextBox1.Text)
                cmd.Parameters.AddWithValue("@Gender", If(RadioButtonMale.Checked, "L", "P"))
                cmd.Parameters.AddWithValue("@IsActive", CheckBox1.Checked)
                cmd.Parameters.AddWithValue("@Username", loggedInUser.Username)

                ' Execute the update query

                ' Optionally, show a success message
                Dim rowsAffected As Integer = cmd.ExecuteNonQuery()

                ' Optionally, show a success message
                MessageBox.Show($"Changes saved successfully. Rows affected: {rowsAffected}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                loggedInUser.Username = TextBox1.Text
                Me.Close()
            End Using
        Catch ex As Exception
            ' Log the exception for troubleshooting
            Console.WriteLine($"Error: {ex.Message}")
            MessageBox.Show("Error: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            ' Make sure to close the connection in the finally block to ensure it's closed even if an exception occurs
            CloseConnection()
        End Try

    End Sub

    Private Sub Save_Click(sender As Object, e As EventArgs) Handles Save.Click
        SaveChanges()

    End Sub

    Private Sub AccountSettings_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        FetchDataFromDatabase()
        EnableControls(False)
        ShowSaveButton(False)
        ShowCancelButton(False)
        ShowEditButton(True)
    End Sub

    Private Sub Ubah_Click(sender As Object, e As EventArgs) Handles Ubah.Click
        EnableControls(True)
        ShowSaveButton(True)
        ShowCancelButton(True)
        ShowEditButton(False)


    End Sub

    Private Sub Cancel_Click(sender As Object, e As EventArgs) Handles Cancel.Click
        Me.Close()

    End Sub
End Class
