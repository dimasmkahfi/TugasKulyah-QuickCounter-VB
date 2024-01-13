Imports MySql.Data.MySqlClient

Module Module1
    Public con As MySqlConnection
    Public cmd As MySqlCommand
    Public ds As DataSet
    Public da As MySqlDataAdapter
    Public rd As MySqlDataReader
    Public db As String

    ' Method to open the connection
    Public Sub OpenConnection()
        If con Is Nothing Then
            db = "Server=localhost;user id=root;password=;database=tallycountervb"
            con = New MySqlConnection(db)
        End If

        Try
            If con.State = ConnectionState.Closed Then
                con.Open()
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Error")
        End Try
    End Sub

    ' Method to close the connection
    Public Sub CloseConnection()
        Try
            If con IsNot Nothing AndAlso con.State = ConnectionState.Open Then
                con.Close()
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Error")
        End Try
    End Sub

    ' Method to execute SQL query and return a DataTable
    Public Function SQLTable(ByVal Source As String) As DataTable
        Try
            ' Open the connection
            OpenConnection()

            ' Create a new MySqlDataAdapter with the SQL query and connection
            Using adp As New MySqlDataAdapter(Source, con)
                ' Create a new DataTable to store the result
                Dim DT As New DataTable

                ' Fill the DataTable using the adapter
                adp.Fill(DT)

                ' Return the filled DataTable
                Return DT
            End Using
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Error")
            Return Nothing
        Finally
            ' Close the connection
            CloseConnection()
        End Try
    End Function

    Public Function Login(username As String, password As String) As Boolean
        Dim query As String = $"SELECT * FROM users WHERE Officer_ID='{username}' AND Password='{password}'"
        Dim dataTable As DataTable = SQLTable(query)

        If dataTable IsNot Nothing AndAlso dataTable.Rows.Count > 0 Then
            ' User credentials are valid
            Return True
        Else
            ' Invalid credentials
            Return False
        End If
    End Function
End Module
