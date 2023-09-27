Imports System.Runtime.InteropServices
Imports System.IO
Imports System.Net
Imports System.Net.Mail

Public Class WCM_FTP
    Private Const _MeName As String = "WCM_FTP"

#Region "Declarations --------------------------------------------------------------"
    Private Const c_MYSOURCE As String = "WCM_FTP_MySource"
    Private Const c_MYLOG As String = "WCM_FTP_Log"

    Private Property _eventId As Integer

    Private _Test_Mode As Boolean = False
    Private WithEvents mFTP As CFTP

    Private _FTPSite_FROM As String = String.Empty
    Private _FTPSite_TO As String = String.Empty
    Private _FTPUser As String = String.Empty
    Private _FTPPW As String = String.Empty
    Private _FilesIN_Path As String = String.Empty
    Private _FilesOUT_Path As String = String.Empty
    Private timer As System.Timers.Timer = New System.Timers.Timer(My.Settings.TimerInterval)

    Public Enum ServiceState
        SERVICE_STOPPED = 1
        SERVICE_START_PENDING = 2
        SERVICE_STOP_PENDING = 3
        SERVICE_RUNNING = 4
        SERVICE_CONTINUE_PENDING = 5
        SERVICE_PAUSE_PENDING = 6
        SERVICE_PAUSED = 7
    End Enum

    <StructLayout(LayoutKind.Sequential)>
    Public Structure ServiceStatus
        Public dwServiceType As Long
        Public dwCurrentState As ServiceState
        Public dwControlsAccepted As Long
        Public dwWin32ExitCode As Long
        Public dwServiceSpecificExitCode As Long
        Public dwCheckPoint As Long
        Public dwWaitHint As Long
    End Structure

    Declare Auto Function SetServiceStatus Lib "advapi32.dll" (ByVal handle As IntPtr, ByRef serviceStatus As ServiceStatus) As Boolean

#End Region
#Region "Constructors --------------------------------------------------------------"
    Public Sub New()
        MyBase.New()
        ' This call is required by the designer.
        InitializeComponent()

        MyEventLog = New System.Diagnostics.EventLog
        If Not System.Diagnostics.EventLog.SourceExists(c_MYSOURCE) Then
            System.Diagnostics.EventLog.CreateEventSource(c_MYSOURCE, c_MYLOG)
        End If
        MyEventLog.Source = c_MYSOURCE
        MyEventLog.Log = c_MYLOG

    End Sub

#End Region

#Region "On-Actions --------------------------------------------------------------"

    Protected Overrides Sub OnStart(ByVal args() As String)
        ' Add code here to start your service. This method should set things in motion so your service can do its work.
        Try
            MyEventLog.WriteEntry("WCM_FTP Started")

            ' Update the service state to Start Pending.
            Dim serviceStatus As ServiceStatus = New ServiceStatus()
            serviceStatus.dwCurrentState = ServiceState.SERVICE_START_PENDING
            serviceStatus.dwWaitHint = 100000
            SetServiceStatus(Me.ServiceHandle, serviceStatus)

            ' use a timer that trigger every minute.
            AddHandler timer.Elapsed, AddressOf Me.OnTimer

            timer.Start()

            'Update the service state to Running.
            serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING
            SetServiceStatus(Me.ServiceHandle, serviceStatus)

            _EmailServiceMessage("WCM_FTP service started " & Date.Now)

        Catch ex As Exception
            MyEventLog.WriteEntry("ERROR: " & ex.Message & " in " & _MeName & "." & System.Reflection.MethodInfo.GetCurrentMethod.ToString, EventLogEntryType.Error, _eventId) : _eventId += 1
            _EmailServiceMessage(Date.Now & " " & "ERROR: " & ex.Message & " in " & _MeName & "." & System.Reflection.MethodInfo.GetCurrentMethod.ToString, True)
        End Try
    End Sub

    Protected Overrides Sub OnContinue()
        Try
            ' MyEventLog.WriteEntry("In OnContinue.")

        Catch ex As Exception
            MyEventLog.WriteEntry("ERROR: " & ex.Message & " in " & _MeName & "." & System.Reflection.MethodInfo.GetCurrentMethod.ToString, EventLogEntryType.Error, _eventId) : _eventId += 1
            _EmailServiceMessage(Date.Now & " " & "ERROR: " & ex.Message & " in " & _MeName & "." & System.Reflection.MethodInfo.GetCurrentMethod.ToString, True)
        End Try
    End Sub

    Protected Overrides Sub OnStop()
        ' Add code here to perform any tear-down necessary to stop your service.
        Try
            MyEventLog.WriteEntry("WCM_FTP Service Stopped")

            _EmailServiceMessage(Date.Now & " " & "WCM_FTP Service Stopped")
        Catch ex As Exception
            MyEventLog.WriteEntry("ERROR: " & ex.Message & " in " & _MeName & "." & System.Reflection.MethodInfo.GetCurrentMethod.ToString, EventLogEntryType.Error, _eventId) : _eventId += 1
            _EmailServiceMessage(Date.Now & " " & "ERROR: " & ex.Message & " in " & _MeName & "." & System.Reflection.MethodInfo.GetCurrentMethod.ToString, True)
        End Try
    End Sub


    ' Insert monitoring activities here.
    Private Sub OnTimer(sender As Object, e As Timers.ElapsedEventArgs)

        Try

            'Changed timer so it pulls from the FTP ever 
            Select Case Weekday(Date.Now)
                Case 2 To 6 'monday - saturday 
                    If Hour(Date.Now) > 5 And Hour(Date.Now) < 18 Then
                        If My.Settings.TimerInterval > 1000 Then
                            timer.Interval = My.Settings.TimerInterval ' 90 seconds
                        Else
                            timer.Interval = 90000 ' default 90 seconds 
                        End If
                    Else
                        timer.Interval = 600000 ' 10 mins      
                    End If
                Case Else 'sunday
                    timer.Interval = 600000 ' 10 mins                
            End Select


            If GetSetting_Interserve_SaffRon() Then
                Download_Orders()
                System.Threading.Thread.Sleep(500)
                Upload_Responses_or_Orders()
                System.Threading.Thread.Sleep(500)
            End If

            If GetSetting_FoodBuyOnline() Then
                Download_Orders()
                System.Threading.Thread.Sleep(500)
            End If

            If GetSetting_FreshPastures() Then
                Upload_Responses_or_Orders()
                System.Threading.Thread.Sleep(500)
            End If
            If GetSetting_MillsMilk() Then
                Upload_Responses_or_Orders()
                System.Threading.Thread.Sleep(500)
            End If
        Catch ex As Exception
            MyEventLog.WriteEntry("ERROR: " & ex.Message & " in " & _MeName & "." & System.Reflection.MethodInfo.GetCurrentMethod.ToString, EventLogEntryType.Error, _eventId) : _eventId += 1
            _EmailServiceMessage(Date.Now & " " & "ERROR: " & ex.Message & " in " & _MeName & "." & System.Reflection.MethodInfo.GetCurrentMethod.ToString, True)
        End Try
    End Sub



    Private Function GetSetting_FoodBuyOnline() As Boolean
        Try
            If My.Settings.Switch_FoodBuyOnline = 0 Then
                _FTPSite_FROM = String.Empty
                _FTPUser = String.Empty
                _FTPPW = String.Empty
                _FilesIN_Path = String.Empty
            ElseIf My.Settings.Switch_FoodBuyOnline = 1 OrElse My.Settings.Switch_FoodBuyOnline = 2 Then
                _FTPSite_FROM = My.Settings.FoodBuyOnline_FTP_SITE
                _FilesIN_Path = My.Settings.FoodBuyOnline_ORDERS
                _FTPUser = My.Settings.FoodBuyOnline_FTP_USER
                _FTPPW = My.Settings.FoodBuyOnline_FTP_PW
                _Test_Mode = False
                Return True
            End If
        Catch ex As Exception
            MyEventLog.WriteEntry("ERROR: " & ex.Message & " in " & _MeName & "." & System.Reflection.MethodInfo.GetCurrentMethod.ToString, EventLogEntryType.Error, _eventId) : _eventId += 1
            _EmailServiceMessage(Date.Now & " " & "ERROR: " & ex.Message & " in " & _MeName & "." & System.Reflection.MethodInfo.GetCurrentMethod.ToString, True)
        End Try
        Return False
    End Function


    Private Function GetSetting_Interserve_Saffron() As Boolean
        Try
            If My.Settings.Switch_Interserve_saffron = 0 Then
                _FTPSite_FROM = String.Empty
                _FTPUser = String.Empty
                _FTPPW = String.Empty
                _FilesIN_Path = String.Empty
                _FilesOUT_Path = String.Empty
                _FTPSite_TO = String.Empty
            ElseIf My.Settings.Switch_Interserve_saffron = 1 Then
                _FTPSite_FROM = My.Settings.INTERSERVE_FTP_SITE_SAFFRON_TEST
                _FTPUser = My.Settings.INTERSERVE_FTP_USER_SAFFRON
                _FTPPW = My.Settings.INTERSERVE_FTP_PW_SAFFRON
                _FilesIN_Path = My.Settings.INTERSERVE_ORDERS_SAFFRON_TEST
                _FilesOUT_Path = My.Settings.INTERSERVE_RESPONSE_SAFFRON_TEST
                _FTPSite_TO = My.Settings.INTERSERVE_FTP_SITE_RESPONSE_SAFFRON_TEST
                _Test_Mode = True
                Return True
            ElseIf My.Settings.Switch_Interserve_saffron = 2 Then 'production
                _FTPSite_FROM = My.Settings.INTERSERVE_FTP_SITE_SAFFRON
                _FTPUser = My.Settings.INTERSERVE_FTP_USER_SAFFRON
                _FTPPW = My.Settings.INTERSERVE_FTP_PW_SAFFRON
                _FilesIN_Path = My.Settings.INTERSERVE_ORDERS_SAFFRON
                _FilesOUT_Path = My.Settings.INTERSERVE_RESPONSE_SAFFRON
                _FTPSite_TO = My.Settings.INTERSERVE_FTP_SITE_RESPONSE_SAFFRON
                _Test_Mode = False
                Return True
            End If
        Catch ex As Exception
            MyEventLog.WriteEntry("ERROR: " & ex.Message & " in " & _MeName & "." & System.Reflection.MethodInfo.GetCurrentMethod.ToString, EventLogEntryType.Error, _eventId) : _eventId += 1
            _EmailServiceMessage(Date.Now & " " & "ERROR: " & ex.Message & " in " & _MeName & "." & System.Reflection.MethodInfo.GetCurrentMethod.ToString, True)
        End Try
        Return False
    End Function

    Private Function GetSetting_FreshPastures() As Boolean
        Try
            If My.Settings.Switch_Freshpastures = 0 Then
                _FTPSite_FROM = String.Empty
                _FTPUser = String.Empty
                _FTPPW = String.Empty
                _FilesIN_Path = String.Empty
                _FilesOUT_Path = String.Empty
                _FTPSite_TO = String.Empty
            ElseIf My.Settings.Switch_Freshpastures = 1 Then
                _FTPSite_FROM = String.Empty
                _FTPUser = My.Settings.Freshpastures_FTP_USER
                _FTPPW = My.Settings.Freshpastures_FTP_PW
                _FilesIN_Path = String.Empty
                _FilesOUT_Path = My.Settings.Freshpastures_ORDERS
                _FTPSite_TO = My.Settings.Freshpastures_FTP_SITE
                _Test_Mode = True
                Return True
            ElseIf My.Settings.Switch_Freshpastures = 2 Then 'production
                _FTPSite_FROM = String.Empty
                _FTPUser = My.Settings.Freshpastures_FTP_USER
                _FTPPW = My.Settings.Freshpastures_FTP_PW
                _FilesIN_Path = String.Empty
                _FilesOUT_Path = My.Settings.Freshpastures_ORDERS
                _FTPSite_TO = My.Settings.Freshpastures_FTP_SITE
                _Test_Mode = False
                Return True
            End If
        Catch ex As Exception
            MyEventLog.WriteEntry("ERROR: " & ex.Message & " in " & _MeName & "." & System.Reflection.MethodInfo.GetCurrentMethod.ToString, EventLogEntryType.Error, _eventId) : _eventId += 1
            _EmailServiceMessage(Date.Now & " " & "ERROR: " & ex.Message & " in " & _MeName & "." & System.Reflection.MethodInfo.GetCurrentMethod.ToString, True)
        End Try
        Return False
    End Function
    Private Function GetSetting_MillsMilk() As Boolean
        Try
            If My.Settings.Switch_MillsMilk = 0 Then
                _FTPSite_FROM = String.Empty
                _FTPUser = String.Empty
                _FTPPW = String.Empty
                _FilesIN_Path = String.Empty
                _FilesOUT_Path = String.Empty
                _FTPSite_TO = String.Empty
            ElseIf My.Settings.Switch_MillsMilk = 1 Then
                _FTPSite_FROM = String.Empty
                _FTPUser = My.Settings.MillsMilk_FTP_USER
                _FTPPW = My.Settings.MillsMilk_FTP_PW
                _FilesIN_Path = String.Empty
                _FilesOUT_Path = My.Settings.MillsMilk_ORDERS
                _FTPSite_TO = My.Settings.MillsMilk_FTP_SITE
                _Test_Mode = True
                Return True
            ElseIf My.Settings.Switch_MillsMilk = 2 Then 'production
                _FTPSite_FROM = String.Empty
                _FTPUser = My.Settings.MillsMilk_FTP_USER
                _FTPPW = My.Settings.MillsMilk_FTP_PW
                _FilesIN_Path = String.Empty
                _FilesOUT_Path = My.Settings.MillsMilk_ORDERS
                _FTPSite_TO = My.Settings.MillsMilk_FTP_SITE
                _Test_Mode = False
                Return True
            End If
        Catch ex As Exception
            MyEventLog.WriteEntry("ERROR: " & ex.Message & " in " & _MeName & "." & System.Reflection.MethodInfo.GetCurrentMethod.ToString, EventLogEntryType.Error, _eventId) : _eventId += 1
            _EmailServiceMessage(Date.Now & " " & "ERROR: " & ex.Message & " in " & _MeName & "." & System.Reflection.MethodInfo.GetCurrentMethod.ToString, True)
        End Try
        Return False
    End Function
    Private Function Download_Orders() As Boolean
        Dim l_Files() As String = Nothing
        Dim lProcessed As String = String.Empty

        Try

            mFTP = New CFTP

            With mFTP
                If .FTP_ListRemoteFiles(_FTPSite_FROM, _FTPUser, _FTPPW, l_Files) Then
                    If Not l_Files Is Nothing Then
                        For Each lFileName As String In l_Files
                            If Not String.IsNullOrEmpty(lFileName.Trim) Then
                                lFileName = lFileName.Trim
                                If .FTP_Download(_FTPSite_FROM, _FTPUser, _FTPPW, _FilesIN_Path, lFileName) Then
                                    If .FTP_Delete(_FTPSite_FROM, _FTPUser, _FTPPW, lFileName) Then
                                        lProcessed &= lFileName & ", "
                                    End If
                                End If
                            End If
                        Next
                    End If
                End If
            End With

            If Not String.IsNullOrEmpty(lProcessed) Then
                If _Test_Mode Then
                    _EmailServiceMessage(Date.Now & "Downloaded to " & _FilesIN_Path & " : " & lProcessed)
                End If
                MyEventLog.WriteEntry("Downloaded to " & _FilesIN_Path & " : " & lProcessed, EventLogEntryType.Information, _eventId) : _eventId += 1

            End If
            'reset mFTP object - need to create fresh one for the next run
            mFTP = Nothing
            Return True
        Catch ex As Exception
            MyEventLog.WriteEntry("ERROR: " & ex.Message & " in " & _MeName & "." & System.Reflection.MethodInfo.GetCurrentMethod.ToString, EventLogEntryType.Error, _eventId) : _eventId += 1
            _EmailServiceMessage(Date.Now & " " & "ERROR: " & ex.Message & " in " & _MeName & "." & System.Reflection.MethodInfo.GetCurrentMethod.ToString, True)
        End Try
        Return False
    End Function

    Private Function Upload_Responses_or_Orders() As Boolean
        Dim l_Files() As String = Nothing
        Dim lProcessed As String = String.Empty

        Try
            If String.IsNullOrEmpty(_FilesOUT_Path) Then
                Return False
            End If
            If Not _FilesOUT_Path.EndsWith("\") Then
                _FilesOUT_Path &= "\"
            End If

            l_Files = Directory.GetFiles(_FilesOUT_Path)
            If Not l_Files Is Nothing Then
                mFTP = New CFTP
                With mFTP
                    For Each lFileName As String In l_Files
                        If Not String.IsNullOrEmpty(lFileName.Trim) Then
                            lFileName = lFileName.Trim
                            If .FTP_Upload(_FTPSite_TO, _FTPUser, _FTPPW, lFileName) Then
                                lProcessed &= lFileName & ", "
                                If Directory.Exists(_FilesOUT_Path & "Processed\") Then
                                    File.Copy(lFileName, _FilesOUT_Path & "Processed\" & lFileName.Replace(_FilesOUT_Path, ""), True)
                                    If File.Exists(lFileName) Then
                                        File.Delete(lFileName)
                                    End If
                                ElseIf Directory.Exists(_FilesOUT_Path & "Archived\") Then
                                    File.Copy(lFileName, _FilesOUT_Path & "Archived\" & lFileName.Replace(_FilesOUT_Path, ""), True)
                                    If File.Exists(lFileName) Then
                                        File.Delete(lFileName)
                                    End If
                                End If
                            End If
                        End If
                    Next
                End With

                If Not String.IsNullOrEmpty(lProcessed) Then
                    If _Test_Mode Then
                        _EmailServiceMessage(Date.Now & "Uploaded to " & _FTPSite_TO & " : " & lProcessed)
                    End If
                    MyEventLog.WriteEntry("Uploaded to " & _FTPSite_TO & " : " & lProcessed, EventLogEntryType.Information, _eventId) : _eventId += 1

                End If
                'reset mFTP object - need to create fresh one for the next run
                mFTP = Nothing
            End If
            Return True
        Catch ex As Exception
            MyEventLog.WriteEntry("ERROR: " & ex.Message & " in " & _MeName & "." & System.Reflection.MethodInfo.GetCurrentMethod.ToString, EventLogEntryType.Error, _eventId) : _eventId += 1
            _EmailServiceMessage(Date.Now & " " & "ERROR: " & ex.Message & " in " & _MeName & "." & System.Reflection.MethodInfo.GetCurrentMethod.ToString, True)
        End Try
        Return False
    End Function
#End Region

#Region "Events --------------------------------------------------------------"
    Private Sub MyEventLog_EntryWritten(sender As System.Object, e As EntryWrittenEventArgs) Handles MyEventLog.EntryWritten

    End Sub

    Private Sub mFTP_Report_Error(pErrMsg As String) Handles mFTP.Report_Error
        MyEventLog.WriteEntry(pErrMsg, EventLogEntryType.Error, _eventId) : _eventId += 1
        _EmailServiceMessage(Date.Now & pErrMsg, True)
    End Sub

#End Region
#Region "Emailing --------------------------------------------------------------"

    Private Function _EmailServiceMessage(ByVal pMessage As String, Optional ByVal pError As Boolean = False) As Boolean
        Dim oMessage As MailMessage
        Dim sTest As String = " ", sError As String = " "
        Try
            Dim astrTo As String() = My.Settings.Email_Success.Split(";")


            If _Test_Mode Then
                sTest = "TEST "
            End If

            If pError Then
                sError = "ERROR "
                astrTo = My.Settings.Email_Error.Split(";")
            End If

            oMessage = New MailMessage()
            With oMessage
                For Each sWCM_To As String In astrTo
                    If sWCM_To.IndexOf("@") > -1 Then
                        .To.Add(New MailAddress(sWCM_To))
                    End If
                Next
                If .To.Count = 0 Then
                    .To.Add(New MailAddress("victor@wcmilk.co.uk"))
                End If

                If My.Settings.EMAIL_FROM <> "" Then
                    .From = New MailAddress(My.Settings.EMAIL_FROM)
                Else
                    .From = New MailAddress("administrator@wcmilk.co.uk")
                End If

                .Subject = sTest & sError & " WCM_FTP"
                If pError Then
                    .Body = sError & vbNewLine & vbNewLine
                End If
                .Body &= pMessage & vbNewLine & vbNewLine

                .IsBodyHtml = False

            End With

            Dim oSMTP As New SmtpClient()

            'oSMTP.Host  defined in app.config<system.net><mailSettings> section
            'oSMTP.DeliveryMethod = SmtpDeliveryMethod.Network
            'oSMTP.DeliveryMethod = SmtpDeliveryMethod.PickupDirectoryFromIis

            'oSMTP.UseDefaultCredentials = False
            'oSMTP.Credentials = New System.Net.NetworkCredential("WCMAdmin", "M1lkAdm1nP@55!") '("WCMApplication", "M1lkyW4y!")

            oSMTP.UseDefaultCredentials = True
            oSMTP.Send(oMessage)

            Return True

        Catch ex As SmtpFailedRecipientsException
            MyEventLog.WriteEntry("ERROR: " & ex.Message & " in " & _MeName & "." & System.Reflection.MethodInfo.GetCurrentMethod.ToString, EventLogEntryType.Error, _eventId) : _eventId += 1
        Catch ex As SmtpFailedRecipientException
            MyEventLog.WriteEntry("ERROR: " & ex.Message & " in " & _MeName & "." & System.Reflection.MethodInfo.GetCurrentMethod.ToString, EventLogEntryType.Error, _eventId) : _eventId += 1
        Catch ex As SmtpException
            MyEventLog.WriteEntry("ERROR: " & ex.Message & " in " & _MeName & "." & System.Reflection.MethodInfo.GetCurrentMethod.ToString, EventLogEntryType.Error, _eventId) : _eventId += 1
        Catch ex As Exception
            MyEventLog.WriteEntry("ERROR: " & ex.Message & " in " & _MeName & "." & System.Reflection.MethodInfo.GetCurrentMethod.ToString, EventLogEntryType.Error, _eventId) : _eventId += 1
        Finally
            oMessage = Nothing
        End Try
        Return False
    End Function

#End Region

End Class
