Imports System.Data.OleDb
Imports System.Windows.Forms.VisualStyles.VisualStyleElement

Public Class frmAppointments

    Dim connectionString As String = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:\AppointmentDatabase\Appointment.accdb"
    Dim strPath As String = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:\AppointmentDatabase\Appointment.accdb"
    Private Sub btnView_Click(sender As Object, e As EventArgs) Handles btnView.Click

        Dim DBcon As New OleDbConnection
        Dim DbCommand As New OleDbCommand
        Dim Dbtable As New Data.DataTable

        Dim sqlQuery As String = "SELECT * From Appointment"

        Try

            DBcon.ConnectionString = strPath
            DBcon.Open()
            DbCommand.Connection = DBcon

            DbCommand.CommandText = sqlQuery
            Dbtable.Load(DbCommand.ExecuteReader())
            DBcon.Close()

            dgvAppointments.DataSource = Dbtable

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

        Dim countString As String = "SELECT COUNT(*) From Appointment"

        Using conn As New OleDbConnection(connectionString)
            Using Command As New OleDbCommand(countString, conn)
                Try
                    conn.Open()

                    Dim AppointmentCount As Integer = Command.ExecuteScalar()
                    lblDisplay.Text = "Total booked appointments: " & AppointmentCount
                Catch ex As Exception
                    MsgBox("Error counting appointments: " & ex.Message)
                End Try
            End Using
        End Using
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Application.Exit()
    End Sub

    Private Sub EditAppointmentsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles EditAppointmentsToolStripMenuItem.Click
        frmAddAppointment.Show()
        Me.Hide()
    End Sub

    Private Sub AppointmentsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AppointmentsToolStripMenuItem.Click
        Me.Show()
    End Sub

    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click

        Dim searchKeyword As String = txtSearch.Text.Trim()

        Using connection As New OleDbConnection(connectionString)
            Try
                connection.Open()

                ' Execute the search query
                Dim queryString As String = "SELECT * FROM Appointment WHERE ClientName LIKE '%' & @Keyword & '%';"
                Using command As New OleDbCommand(queryString, connection)
                    command.Parameters.AddWithValue("@Keyword", searchKeyword)

                    Dim adapter As New OleDbDataAdapter(command)
                    Dim dataSet As New DataSet()
                    adapter.Fill(dataSet)

                    ' Display the search results
                    dgvAppointments.DataSource = dataSet.Tables(0)
                End Using
            Catch ex As Exception
                MessageBox.Show("Error searching data: " & ex.Message)
            End Try
        End Using

    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click

        Dim recordIdToDelete As Integer = txtID.Text

        Using connection As New OleDbConnection(connectionString)
            Try
                connection.Open()

                ' Execute the delete query
                Dim queryString As String = "DELETE FROM Appointment WHERE AppointmentID = @RecordId;"
                Using command As New OleDbCommand(queryString, connection)
                    command.Parameters.AddWithValue("@RecordId", recordIdToDelete)

                    command.ExecuteNonQuery()

                    MessageBox.Show("Appointment deleted successfully.")
                End Using
            Catch ex As Exception
                MessageBox.Show("Error deleting appointment: " & ex.Message)
            End Try
        End Using

    End Sub

    Private Sub btnUpdate_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub MenuStrip1_ItemClicked(sender As Object, e As ToolStripItemClickedEventArgs) Handles MenuStrip1.ItemClicked

    End Sub
End Class
