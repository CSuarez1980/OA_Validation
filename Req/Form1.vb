Public Class Form1
    Public P As New DataTable
    Dim C As New OAConnection.Connection

    Public SAP As New SAPCOM.SAPConnector
    Public Conn As Object = SAP.GetSAPConnection("L7P", "CF9019", "LAT") ' -> Change TNumber; use machine owner, password is taken from LA Tool Password setup @ System menu/Variants/SAP Passwords
    Public Segundos As Integer = 3
    Public Status As String = ""


    Public Sub Done()
        'Me.txtStatus.Items.Add(My.Computer.Clock.LocalTime.ToString & " - Waiting for next download...")
        End
    End Sub

    Public Function CheckBW() As Boolean
        If Not BGRequis.IsBusy AndAlso Not BGEKKO.IsBusy AndAlso Not BGEKPO.IsBusy AndAlso Not BGMasterData.IsBusy AndAlso Not BGSourceList.IsBusy AndAlso Not BGEKKOPO.IsBusy Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Sub StartDownload()
        BGEKKOPO.RunWorkerAsync()
        bgManufacter.RunWorkerAsync()
        BGOTD.RunWorkerAsync()
        BGRequis.RunWorkerAsync()
        BGEKKO.RunWorkerAsync()
        BGEKPO.RunWorkerAsync()
        BGMasterData.RunWorkerAsync()
        BGSourceList.RunWorkerAsync()
    End Sub


    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        P = C.GetActivesPlants().Tables(0)
    End Sub

    Private Sub BGRequis_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles BGRequis.DoWork
        Status = "BGRequis: Starting download."
        BGRequis.ReportProgress(10)

        Dim [OR] As New SAPCOM.OpenReqs_Report("L7P", "CF9019", "LAT") ' -> Change TNumber; use machine owner, password is taken from LA Tool Password setup @ System menu/Variants/SAP Passwords
        Dim R As New SAPCOM.EBAN_Report("L7P", "CF9019", "LAT") ' -> Change TNumber; use machine owner, password is taken from LA Tool Password setup @ System menu/Variants/SAP Passwords
        Dim r2 As DataRow

        'Dim Q As New SAPCOM.OpenReqs_Report()
        'Dim R As New SAPCOM.EBAN_Report(Conn)

        [OR].IncludeMaterialFromTo("30000000", "39999999")
        Dim row As DataRow

        For Each row In P.Rows
            [OR].IncludePlant(row("Code"))
        Next


        [OR].Execute()

        If [OR].Success Then

            For Each r2 In [OR].Data.Rows
                R.IncludeDocument(r2("Req Number"))
            Next
        End If

        R.AddCustomField("MATNR", "Gica")
        R.AddCustomField("EKGRP", "Purch Group")
        R.AddCustomField("ERNAM", "Created By")
        R.AddCustomField("AFNAM", "Requisitioner")
        R.AddCustomField("ERDAT", "Changed On")
        R.AddCustomField("TXZ01", "Material")
        R.AddCustomField("WERKS", "Plant")
        R.AddCustomField("MENGE", "Qty")
        R.AddCustomField("MEINS", "UOM")
        R.AddCustomField("LFDAT", "Delivery Date")
        R.AddCustomField("FRGDT", "Release Date")
        R.AddCustomField("WEBAZ", "GR Proc Time")
        R.AddCustomField("LIFNR", "Desired Vendor")
        R.AddCustomField("FLIEF", "Fixed Vendor")
        R.AddCustomField("KONNR", "Outline Agreement")
        R.AddCustomField("KTPNR", "Outline Agre Item")
        R.AddCustomField("INFNR", "Inforecord")
        R.AddCustomField("PLIFZ", "Planted Del Time")

        R.DeletionIndicator = False
        R.CloseIndicator = False
        R.MaterialFrom = "30000000"
        R.MaterialTo = "39999999"

        R.Execute()

        If R.Success Then
            R.Data.Columns.Remove("Purch Doc")
            R.Data.Columns.Remove("PO Item")
            R.Data.Columns.Remove("Closed")

            R.ColumnToDoubleStr("Gica")
            R.ColumnToDateStr("Delivery Date")
            R.ColumnToDateStr("Release Date")
            R.ColumnToDateStr("Changed On")
            R.ColumnToDoubleStr("Gica")
            R.ColumnToDoubleStr("Qty")
            R.ColumnToDoubleStr("Desired Vendor")
            R.ColumnToDoubleStr("Fixed Vendor")
            R.ColumnToDoubleStr("Outline Agreement")
            R.ColumnToIntStr("Outline Agre Item")
            R.ColumnToDoubleStr("Inforecord")
            R.ColumnToIntStr("Planted Del Time")

            Status = "BGRequis: Uploadig to server."
            BGRequis.ReportProgress(20)

            C.RunSentence("Delete From Eban")
            C.AppendTableToSqlServer("EBAN", R.Data)

            Status = "BGRequis: Download Finished."
            BGRequis.ReportProgress(30)

        Else
            Status = "BGRequis: Download Failed."
            BGRequis.ReportProgress(30)
        End If

    End Sub
    Private Sub BGRequis_ProgressChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles BGRequis.ProgressChanged
        Me.txtStatus.Items.Add(My.Computer.Clock.LocalTime.ToString & " - " & Status)
    End Sub
    Private Sub BGRequis_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BGRequis.RunWorkerCompleted
        If CheckBW() Then
            Done()
        End If
    End Sub
    Private Sub BGEKKO_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles BGEKKO.DoWork
        'Dim R As New SAPCOM.EKKO_Report("L7P", "CF9019", "LAT")
        Status = "BGEKKO: Starting download."
        BGEKKO.ReportProgress(10)

        Dim R As New SAPCOM.EKKO_Report("L7P", "CF9019", "LAT") ' -> Change TNumber; use machine owner, password is taken from LA Tool Password setup @ System menu/Variants/SAP Passwords

        R.DeletionIndicator = False
        R.DocumentFrom = "4600000000"
        R.DocumentTo = "4699999999"
        R.Execute()

        If R.Success Then
            R.Data.Columns.Remove("Company Code")
            R.Data.Columns.Remove("Doc Type")
            R.Data.Columns.Remove("Y Refer")
            R.Data.Columns.Remove("Salesperson")
            R.Data.Columns.Remove("Telephone")
            R.Data.Columns.Remove("OA")
            R.Data.Columns.Remove("O Reference")


            Status = "BGEKKO: Uploading to server"
            BGEKKO.ReportProgress(20)

            C.RunSentence("Delete From HeaderContrato")
            C.AppendTableToSqlServer("HeaderContrato", R.Data)

            Status = "BGEKKO: Download Finished."
            BGEKKO.ReportProgress(30)

        Else
            Status = "BGEKKO: Download Failed."
            BGEKKO.ReportProgress(30)

        End If

    End Sub
    Private Sub BGEKKO_ProgressChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles BGEKKO.ProgressChanged
        Me.txtStatus.Items.Add(My.Computer.Clock.LocalTime.ToString & " - " & Status)
    End Sub
    Private Sub BGEKKO_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BGEKKO.RunWorkerCompleted
        If CheckBW() Then
            Done()
        End If
    End Sub

    Private Sub BGEKPO_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles BGEKPO.DoWork
        Status = "BGEKPO: Starting download."
        BGEKPO.ReportProgress(10)


        Dim R As New SAPCOM.EKPO_Report("L7P", "CF9019", "LAT") ' -> Change TNumber; use machine owner, password is taken from LA Tool Password setup @ System menu/Variants/SAP Passwords
        Dim dr As DataRow

        R.DeletionIndicator = False
        R.DocumentFrom = "4600000000"
        R.DocumentTo = "4699999999"

        R.MaterialFrom = "30000000"
        R.MaterialTo = "39999999"

        For Each dr In P.Rows
            R.IncludePlant(dr("Code"))
        Next

        R.Execute()

        If R.Success Then
            R.Data.Columns.Remove("Quantity")
            R.Data.Columns.Remove("Mat Group")
            R.Data.Columns.Remove("Tracking Fld")
            R.Data.Columns.Remove("Price Unit")

            Status = "BGEKPO: Uploading to server."
            BGEKPO.ReportProgress(20)

            C.RunSentence("Delete From DetalleContrato")
            C.AppendTableToSqlServer("DetalleContrato", R.Data)



            Status = "BGEKPO: Download Finished."
            BGEKPO.ReportProgress(30)

        End If

    End Sub

    Private Sub BGEKPO_ProgressChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles BGEKPO.ProgressChanged
        Me.txtStatus.Items.Add(My.Computer.Clock.LocalTime.ToString & " - " & Status)
    End Sub

    Private Sub BGEKPO_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BGEKPO.RunWorkerCompleted
        If CheckBW() Then
            Done()
        End If
    End Sub

    Private Sub BGMasterData_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles BGMasterData.DoWork
        Status = "BGMasterData: Starting download."
        BGMasterData.ReportProgress(10)


        'Dim R As New SAPCOM.MARC_Report(Conn) -> Problema con el metodo sobrecargado
        Dim R As New SAPCOM.MARC_Report("L7P", "CF9019", "LAT") ' -> Change TNumber; use machine owner, password is taken from LA Tool Password setup @ System menu/Variants/SAP Passwords
        Dim dr As DataRow

        R.MaterialFrom = "30000000"
        R.MaterialTo = "39999999"

        For Each dr In P.Rows
            R.IncludePlant(dr("Code"))
        Next

        R.Execute()

        If R.Success Then
            Status = "BGMasterData: Uploading to server."
            BGMasterData.ReportProgress(20)

            C.RunSentence("Delete From MasterData")
            C.AppendTableToSqlServer("MasterData", R.Data)

            Status = "BGMasterData: Download Finished."
            BGMasterData.ReportProgress(30)

        Else
            Status = "BGMasterData: Download Failed."
            BGMasterData.ReportProgress(30)


        End If

    End Sub

    Private Sub BGMasterData_ProgressChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles BGMasterData.ProgressChanged
        Me.txtStatus.Items.Add(My.Computer.Clock.LocalTime.ToString & " - " & Status)
    End Sub

    Private Sub BGMasterData_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BGMasterData.RunWorkerCompleted
        If CheckBW() Then
            Done()
        End If
    End Sub

    Private Sub BGSourceList_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles BGSourceList.DoWork
        Status = "BGSourceLis: Starting download."
        BGSourceList.ReportProgress(30)


        'Dim R As New SAPCOM.MARC_Report(Conn) -> Problema con el metodo sobrecargado
        Dim R As New SAPCOM.EORD_Report("L7P", "CF9019", "LAT") ' -> Change TNumber; use machine owner, password is taken from LA Tool Password setup @ System menu/Variants/SAP Passwords

        R.Execute()

        If R.Success Then
            R.Data.Columns.Remove("Number")
            R.Data.Columns.Remove("Created On")
            R.Data.Columns.Remove("Created By")
            R.Data.Columns.Remove("Vendor")
            R.Data.Columns.Remove("Fixed Vendor")
            R.Data.Columns.Remove("Fixed Agreement Item")
            R.Data.Columns.Remove("Procurement Plant")
            R.Data.Columns.Remove("Fixed Issuing Plant")
            R.Data.Columns.Remove("MPN Material")
            R.Data.Columns.Remove("Blocked")
            R.Data.Columns.Remove("Purch Org")
            R.Data.Columns.Remove("Doc Category")
            R.Data.Columns.Remove("Control Ind")
            R.Data.Columns.Remove("Materials Planning")
            R.Data.Columns.Remove("UOM")
            R.Data.Columns.Remove("Logical System")
            R.Data.Columns.Remove("Special Stock")
            R.Data.Columns.Remove("Central Contract Item")
            R.Data.Columns.Remove("Central Contract")


            Status = "BGSourceLis: Uploading to server."
            BGSourceList.ReportProgress(20)

            C.RunSentence("Delete From EORD")
            C.AppendTableToSqlServer("EORD", R.Data)

            C.RunSentence("Delete From EORD Where Agreement = ''")

            Status = "BGSourceLis: Download finished."
            BGSourceList.ReportProgress(30)

        Else
            Status = "BGSourceLis: Download Falied."
            BGSourceList.ReportProgress(30)


        End If

    End Sub

    Private Sub BGSourceList_ProgressChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles BGSourceList.ProgressChanged
        Me.txtStatus.Items.Add(My.Computer.Clock.LocalTime.ToString & " - " & Status)
    End Sub

    Private Sub BGSourceList_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BGSourceList.RunWorkerCompleted
        If CheckBW() Then
            Done()
        End If
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        If Segundos > 0 Then
            Segundos -= 1
        End If

        PB1.Value += 1

        If Segundos <> 0 Then
            If Segundos > 60 Then
                Me.lblNextUpload.Text = Mid(Segundos / 60, 1, 2) & " Min."
            Else
                Me.lblNextUpload.Text = Segundos.ToString & " Seg."
            End If
        Else
            'Inicia la descarga de la información

            Segundos = 3600
            PB1.Value = 0
            StartDownload()

        End If
    End Sub

    Private Sub BGOTD_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles BGOTD.DoWork
        Dim R As New SAPCOM.OTD_Report("L7P", "CF9019", "LAT") ' -> Change TNumber; use machine owner, password is taken from LA Tool Password setup @ System menu/Variants/SAP Passwords

        'For Each dr In P.Rows
        'R.IncludePlant(dr("Code"))
        'Next

        R.IncludePlant("7761")

        R.MaterialFrom = "30000000"
        R.MaterialTo = "39999999"

        'R.MaterialFrom = "39743294"
        ' R.MaterialTo = "39743294"



        R.MonthFrom = "08/2011"
        R.MonthTo = "08/2011"

        R.Execute()

        If R.Success Then

        End If

    End Sub

    Private Sub bgManufacter_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles bgManufacter.DoWork
        Dim R As New SAPCOM.Manufacturers_Report("L7P", "CF9019", "LAT") ' -> Change TNumber; use machine owner, password is taken from LA Tool Password setup @ System menu/Variants/SAP Passwords
        Dim cn As New OAConnection.Connection

        For Each dr In P.Rows
            R.IncludePlant(dr("Code"))
        Next

        Status = "BGManufacter: Starting download."
        bgManufacter.ReportProgress(30)

        R.Execute()

        If R.Success Then
            Status = "BGManufacter: Uploading to server."
            bgManufacter.ReportProgress(30)

            R.Data.Columns.Remove("Client")
            cn.RunSentence("Delete From Manufacter")
            cn.AppendTableToSqlServer("Manufacter", R.Data)
            Status = "BGManufacters: Download finished."
            bgManufacter.ReportProgress(30)

        Else
            Status = "BGManufacter: Download failed."
            bgManufacter.ReportProgress(30)

        End If

    End Sub

    Private Sub BGEKKOPO_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles BGEKKOPO.DoWork
        Status = "BGEKKOPO: Starting download."
        BGEKKOPO.ReportProgress(10)

        Dim R As New SAPCOM.EKKO_Report("L7P", "CF9019", "LAT") ' -> Change TNumber; use machine owner, password is taken from LA Tool Password setup @ System menu/Variants/SAP Passwords

        R.DeletionIndicator = False
        R.DocumentFrom = "4500000000"
        R.DocumentTo = "4599999999"
        R.DocDateFrom = Now.Month & "/" & Now.Day & "/" & Now.Year - 1
        R.DocDateTo = Now.Month & "/" & Now.Day & "/" & Now.Year

        R.Execute()

        If R.Success Then
            R.Data.Columns.Remove("Doc Type")
            R.Data.Columns.Remove("Doc Date")
            R.Data.Columns.Remove("Validity Start")
            R.Data.Columns.Remove("Validity End")
            R.Data.Columns.Remove("Y Refer")
            R.Data.Columns.Remove("Salesperson")
            R.Data.Columns.Remove("Telephone")
            R.Data.Columns.Remove("OA")
            R.Data.Columns.Remove("O Reference")


            Status = "BGEKKOPO: Uploading to server"
            BGEKKOPO.ReportProgress(20)

            C.RunSentence("Delete From HeaderCompras")
            C.AppendTableToSqlServer("HeaderCompras", R.Data)

            Status = "BGEKKOPO: Starting download PO Detail."
            BGEKKOPO.ReportProgress(10)

            Dim R2 As New SAPCOM.EKPO_Report("L7P", "CF9019", "LAT") ' -> Change TNumber; use machine owner, password is taken from LA Tool Password setup @ System menu/Variants/SAP Passwords
            Dim dr As DataRow

            R2.DeletionIndicator = False

            Dim MinPO = (From C In R.Data.AsEnumerable() _
                         Select C.Item("Doc Number")).Min

            Dim MaxPO = (From C In R.Data.AsEnumerable() _
                         Select C.Item("Doc Number")).Max

            R2.DocumentFrom = MinPO
            R2.DocumentTo = MaxPO

            R2.MaterialFrom = "30000000"
            R2.MaterialTo = "39999999"

            For Each dr In P.Rows
                R2.IncludePlant(dr("Code"))
            Next

            R2.Execute()

            If R2.Success Then
                R2.Data.Columns.Remove("Inforecord")
                R2.Data.Columns.Remove("PDT")
                R2.Data.Columns.Remove("Mat Group")
                R2.Data.Columns.Remove("Tracking Fld")
                R2.Data.Columns.Remove("Price Unit")

                Status = "BGEKKOPO: Uploading PO Detail to server."
                BGEKKOPO.ReportProgress(20)

                C.RunSentence("Delete From DetalleCompras")
                C.AppendTableToSqlServer("DetalleCompras", R2.Data)

                Status = "BGEKKOPO: Download Finished."
                BGEKKOPO.ReportProgress(30)
            End If

        Else
            Status = "BGEKKOPO: Download Failed."
            BGEKKOPO.ReportProgress(30)
        End If

    End Sub

    Private Sub BGEKKOPO_ProgressChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles BGEKKOPO.ProgressChanged
        Me.txtStatus.Items.Add(My.Computer.Clock.LocalTime.ToString & " - " & Status)
    End Sub

    Private Sub BGEKKOPO_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BGEKKOPO.RunWorkerCompleted
        If CheckBW() Then
            Done()
        End If
    End Sub
End Class
