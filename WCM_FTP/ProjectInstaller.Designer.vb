<System.ComponentModel.RunInstaller(True)> Partial Class ProjectInstaller
    Inherits System.Configuration.Install.Installer

    'Installer overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Component Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Component Designer
    'It can be modified using the Component Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.WCM_FTPServiceProcessInstaller = New System.ServiceProcess.ServiceProcessInstaller()
        Me.WCM_FTPServiceInstaller = New System.ServiceProcess.ServiceInstaller()
        '
        'WCM_FTPServiceProcessInstaller
        '
        Me.WCM_FTPServiceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem
        Me.WCM_FTPServiceProcessInstaller.Password = Nothing
        Me.WCM_FTPServiceProcessInstaller.Username = Nothing
        '
        'WCM_FTPServiceInstaller
        '
        Me.WCM_FTPServiceInstaller.Description = "WCM FTP Service Apllication (Elior Waterfall P2P Integration) "
        Me.WCM_FTPServiceInstaller.DisplayName = "WCM FTP"
        Me.WCM_FTPServiceInstaller.ServiceName = "WCM_FTP"
        Me.WCM_FTPServiceInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic
        '
        'ProjectInstaller
        '
        Me.Installers.AddRange(New System.Configuration.Install.Installer() {Me.WCM_FTPServiceProcessInstaller, Me.WCM_FTPServiceInstaller})

    End Sub
    Friend WithEvents WCM_FTPServiceProcessInstaller As System.ServiceProcess.ServiceProcessInstaller
    Friend WithEvents WCM_FTPServiceInstaller As System.ServiceProcess.ServiceInstaller

End Class
