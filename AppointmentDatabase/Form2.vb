Imports System.Data.OleDb

Public Class frmAddAppointment
    Private Sub frmAddAppointment_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        cmbServices.Items.Add("Manucure")
        cmbServices.Items.Add("Chiskop")
        cmbServices.Items.Add("Brush Cut")
        cmbServices.Items.Add("Fade")

    End Sub

    Private Sub btnBook_Click(sender As Object, e As EventArgs) Handles btnBook.Click

        Dim strName As String
        Dim strEmail As String
        Dim intPhoneNumber As Integer
        Dim AppointmentDate As Date
        Dim strComment As String
        Dim strService As String

        If IsNumeric(txtEmail.Text) Or IsNumeric(txtName.Text) Then
            MsgBox("Enter valid details.")
            Exit Sub
        Else
            strName = CStr(txtName.Text)
            strEmail = CStr(txtEmail.Text)
        End If
        If IsNumeric(txtPhoneNumber.Text) Then
            intPhoneNumber = txtPhoneNumber.Text
        Else
            MsgBox("Please enter valid phone number")
            Exit Sub
        End If
        AppointmentDate = dtpAppointmentDate.Value.Date
        strComment = txtComment.Text

        Select Case cmbServices.SelectedIndex
            Case 0
                strService = 4
            Case 1
                strService = 1
            Case 2
                strService = 2
            Case 3
                strService = 3
        End Select

        Dim connectionString As String = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:\AppointmentDatabase\Appointment.accdb"
        Dim queryString As String = "INSERT INTO Appointment (AppointmentDate, Comment, ClientName) VALUES (AppointmentDate, strComment, strName);"
        Dim sqlQuery As String = "INSERT INTO Client (Name, Email, ContactNumber) VALUES (strName, strEmail, intPhoneNumber)"

        Using connection As New OleDbConnection(connectionString)
            Using command As New OleDbCommand(queryString, connection)
                connection.Open()

                Try

                    ' Set the parameter values
                    command.Parameters.AddWithValue("@AppointmentDate", AppointmentDate)
                    command.Parameters.AddWithValue("@Comment", strComment)
                    command.Parameters.AddWithValue("@ClientName", strName)
                    ' Execute the insert command
                    command.ExecuteNonQuery()
                    MessageBox.Show("Data inserted successfully.")
                Catch ex As Exception
                    MessageBox.Show("Error inserting data: " & ex.Message)
                End Try
            End Using
            Using command As New OleDbCommand(sqlQuery, connection)
                Try
                    ' Set the parameter values
                    command.Parameters.AddWithValue("@Name", strName)
                    command.Parameters.AddWithValue("@Email", strEmail)
                    command.Parameters.AddWithValue("@ContactNumber", intPhoneNumber)
                    ' Execute the insert command
                    command.ExecuteNonQuery()
                Catch ex As Exception
                    MessageBox.Show("Error inserting data: " & ex.Message)
                End Try
                connection.Close()
            End Using
        End Using

        txtComment.Clear()
        txtEmail.Clear()
        txtName.Clear()
        txtPhoneNumber.Clear()
        txtID.Enabled = False
        btnUpdate.Enabled = False

    End Sub

    Private Sub BtnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click

        Me.Hide()
        frmAppointments.Show()
        txtID.Enabled = False
        btnUpdate.Enabled = False

    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        btnUpdate.Enabled = True
        txtID.Enabled = True
    End Sub

    Private Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click

        Dim connectionString As String = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:\AppointmentDatabase\Appointment.accdb"

        Dim appointmentIdToUpdate As Integer = txtID.Text
        Dim clientName As String = txtName.Text.Trim() ' Get the updated ClientName from a TextBox
        Dim appointmentDate As DateTime = dtpAppointmentDate.Value.Date ' Get the updated AppointmentDate from a DateTimePicker
        Dim comment As String = txtComment.Text.Trim() ' Get the updated Comment from a TextBox

        Using connection As New OleDbConnection(connectionString)
            Try
                connection.Open()

                ' Execute the update query
                Dim queryString As String = "UPDATE Appointment SET ClientName = ?, AppointmentDate = ?, Comment = ? WHERE AppointmentID = ?;"
                Using command As New OleDbCommand(queryString, connection)
                    command.Parameters.AddWithValue("@ClientName", clientName)
                    command.Parameters.AddWithValue("@AppointmentDate", appointmentDate)
                    command.Parameters.AddWithValue("@Comment", comment)
                    command.Parameters.AddWithValue("@AppointmentID", appointmentIdToUpdate)

                    command.ExecuteNonQuery()

                    MessageBox.Show("Record updated successfully.")
                End Using
            Catch ex As Exception
                MessageBox.Show("Error updating record: " & ex.Message)
            End Try
        End Using

    End Sub
End Class