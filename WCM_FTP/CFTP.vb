Imports System.Net

Public Class CFTP
    Private _MeName As String = "CFTP"

    Public FTP_Credentials As System.Net.ICredentials = Nothing
    Public FTP_UserName_Used As String = String.Empty
    Public FTP_PW_Used As String = String.Empty

    Public Event Report_Error(pErrMsg As String)

    Public Function FTP_ListRemoteFiles(pFTPSite As String, pUserName As String, pPW As String, ByRef pFiles() As String) As Boolean
        Dim lRequest As System.Net.FtpWebRequest = Nothing
        Dim lResponse As System.Net.FtpWebResponse = Nothing
        Dim lFile() As Byte = Nothing
        Dim lStream As System.IO.Stream = Nothing
        Dim lReader As System.IO.StreamReader = Nothing

        Try
            If FTP_Credentials Is Nothing Then
                FTP_Credentials = New System.Net.NetworkCredential(pUserName, pPW)
                FTP_UserName_Used = pUserName
                FTP_PW_Used = pPW
            Else
                If FTP_UserName_Used <> pUserName OrElse FTP_PW_Used <> pPW Then
                    FTP_Credentials = New System.Net.NetworkCredential(pUserName, pPW)
                    FTP_UserName_Used = pUserName
                    FTP_PW_Used = pPW
                End If
            End If
            If Not pFTPSite.EndsWith("/") Then
                pFTPSite &= "/"
            End If
            lRequest = DirectCast(System.Net.WebRequest.Create(pFTPSite), System.Net.FtpWebRequest)
            lRequest.Credentials = FTP_Credentials ''New System.Net.NetworkCredential(pUserName, pPW)
            lRequest.UsePassive = False

            ' ''to prevent error : reuse the same FTP_Credentials object
            ' ''the remote server returned an error (503) bad sequence of commands
            ' ''set the KeepAlive property to false. Even thought the request object was destroyed, it was still keeping the connection to the file open
            ''lRequest.KeepAlive = False ' 

            ''============== first get list of files =======================
            lRequest.Method = System.Net.WebRequestMethods.Ftp.ListDirectory
            lResponse = lRequest.GetResponse

            lStream = lResponse.GetResponseStream
            lReader = New System.IO.StreamReader(lStream)

            pFiles = lReader.ReadToEnd().ToString.Split(vbNewLine)

            Return True

        Catch ex As Exception
            RaiseEvent Report_Error("ERROR: " & ex.Message & " in " & _MeName & "." & System.Reflection.MethodInfo.GetCurrentMethod.ToString)
        Finally
            If lReader IsNot Nothing Then
                lReader.Close()
                lReader.Dispose()
            End If
            If lStream IsNot Nothing Then
                lStream.Close()
                lStream.Dispose()
            End If
            If lResponse IsNot Nothing Then
                lResponse.Close()
            End If
        End Try
        Return False
    End Function

    Public Function FTP_Delete(pFTPSite As String, pUserName As String, pPW As String, pFileName As String) As Boolean
        Dim lRequest As System.Net.FtpWebRequest = Nothing
        Dim lResponse As System.Net.FtpWebResponse = Nothing

        Try
            If FTP_Credentials Is Nothing Then
                FTP_Credentials = New System.Net.NetworkCredential(pUserName, pPW)
                FTP_UserName_Used = pUserName
                FTP_PW_Used = pPW
            Else
                If FTP_UserName_Used <> pUserName OrElse FTP_PW_Used <> pPW Then
                    FTP_Credentials = New System.Net.NetworkCredential(pUserName, pPW)
                    FTP_UserName_Used = pUserName
                    FTP_PW_Used = pPW
                End If
            End If
            If Not pFTPSite.EndsWith("/") Then
                pFTPSite &= "/"
            End If

            lRequest = DirectCast(System.Net.WebRequest.Create(pFTPSite & pFileName), System.Net.FtpWebRequest)
            lRequest.Credentials = FTP_Credentials ''New System.Net.NetworkCredential(pUserName, pPW)
            lRequest.UsePassive = False
            lRequest.Method = System.Net.WebRequestMethods.Ftp.DeleteFile
            lResponse = lRequest.GetResponse

            ''Console.WriteLine("Delete status: {0} {1}", lResponse.StatusDescription, lResponse.StatusCode)

            Return lResponse.StatusCode = 250 'Delete operation successful
        Catch ex As Exception
            RaiseEvent Report_Error("ERROR: " & ex.Message & " in " & _MeName & "." & System.Reflection.MethodInfo.GetCurrentMethod.ToString)
        Finally
            If lResponse IsNot Nothing Then
                lResponse.Close()
            End If
        End Try
        Return False
    End Function

    Public Function FTP_Download(pFTPSite As String, pUserName As String, pPW As String, pDownloadPath As String, pFileName As String) As Boolean
        Dim lRequest As System.Net.FtpWebRequest = Nothing
        Dim lResponse As System.Net.FtpWebResponse = Nothing
        Dim lStream As System.IO.Stream = Nothing
        Dim lFile As System.IO.FileStream = Nothing
        Dim lBuffer(16384) As Byte
        Dim lBytesRead As Long

        Try
            If FTP_Credentials Is Nothing Then
                FTP_Credentials = New System.Net.NetworkCredential(pUserName, pPW)
                FTP_UserName_Used = pUserName
                FTP_PW_Used = pPW
            Else
                If FTP_UserName_Used <> pUserName OrElse FTP_PW_Used <> pPW Then
                    FTP_Credentials = New System.Net.NetworkCredential(pUserName, pPW)
                    FTP_UserName_Used = pUserName
                    FTP_PW_Used = pPW
                End If
            End If
            If Not pFTPSite.EndsWith("/") Then
                pFTPSite &= "/"
            End If

            lRequest = DirectCast(System.Net.WebRequest.Create(pFTPSite & pFileName), System.Net.FtpWebRequest)
            lRequest.Credentials = FTP_Credentials ''New System.Net.NetworkCredential(pUserName, pPW)
            lRequest.UsePassive = False
            lRequest.Method = System.Net.WebRequestMethods.Ftp.DownloadFile
            lResponse = lRequest.GetResponse
            lStream = lResponse.GetResponseStream

            If Not pDownloadPath.EndsWith("/") Then
                pDownloadPath &= "\"
            End If
            If IO.File.Exists(pFileName) Then
                lFile = New IO.FileStream(pDownloadPath & pFileName, IO.FileMode.Open)
            Else
                lFile = New IO.FileStream(pDownloadPath & pFileName, IO.FileMode.Create)
            End If

            ' Download the file until the download is completed.
            With lStream
                Do
                    lBytesRead = .Read(lBuffer, 0, 16384)
                    lFile.Write(lBuffer, 0, lBytesRead)
                Loop Until lBytesRead = 0
            End With
            'Force all data out of memory and into the file before closing 
            lFile.Flush()

            Return lFile.Length > 0

        Catch ex As Exception
            RaiseEvent Report_Error("ERROR: " & ex.Message & " in " & _MeName & "." & System.Reflection.MethodInfo.GetCurrentMethod.ToString)
        Finally
            If lFile IsNot Nothing Then
                lFile.Close()
                lFile.Dispose()
            End If
            If lStream IsNot Nothing Then
                lStream.Close()
                lStream.Dispose()
            End If
            If lResponse IsNot Nothing Then
                lResponse.Close()
            End If
        End Try
        Return False
    End Function

    Public Function FTP_Upload(pFTPSite As String, pUserName As String, pPW As String, pFileFullPath As String) As Boolean
        Dim lRequest As System.Net.FtpWebRequest = Nothing
        Dim lFile() As Byte = Nothing
        Dim lStream As System.IO.Stream = Nothing
        Dim lFileName As String = System.IO.Path.GetFileName(pFileFullPath)

        Try
            If FTP_Credentials Is Nothing Then
                FTP_Credentials = New System.Net.NetworkCredential(pUserName, pPW)
                FTP_UserName_Used = pUserName
                FTP_PW_Used = pPW
            Else
                If FTP_UserName_Used <> pUserName OrElse FTP_PW_Used <> pPW Then
                    FTP_Credentials = New System.Net.NetworkCredential(pUserName, pPW)
                    FTP_UserName_Used = pUserName
                    FTP_PW_Used = pPW
                End If
            End If
            pFTPSite = pFTPSite & "/" & lFileName

            lRequest = DirectCast(System.Net.WebRequest.Create(pFTPSite), System.Net.FtpWebRequest)
            lRequest.Credentials = FTP_Credentials ''New System.Net.NetworkCredential(pUserName, pPW)
            lRequest.UsePassive = False

            ' ''to prevent error : reuse the same FTP_Credentials object
            ' ''the remote server returned an error (503) bad sequence of commands
            ' ''set the KeepAlive property to false. Even thought the request object was destroyed, it was still keeping the connection to the file open
            ''lRequest.KeepAlive = False ' 

            lRequest.Method = System.Net.WebRequestMethods.Ftp.UploadFile

            '' read in file...
            lFile = System.IO.File.ReadAllBytes(pFileFullPath)

            '' upload file...
            lStream = lRequest.GetRequestStream()
            lStream.Write(lFile, 0, lFile.Length)

            Return True

        Catch ex As Exception
            RaiseEvent Report_Error("ERROR: " & ex.Message & " in " & _MeName & "." & System.Reflection.MethodInfo.GetCurrentMethod.ToString)
        Finally
            If lStream IsNot Nothing Then
                lStream.Close()
                lStream.Dispose()
            End If
        End Try
        Return False
    End Function
End Class
