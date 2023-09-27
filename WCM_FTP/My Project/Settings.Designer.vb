﻿'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated by a tool.
'     Runtime Version:4.0.30319.42000
'
'     Changes to this file may cause incorrect behavior and will be lost if
'     the code is regenerated.
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict On
Option Explicit On


Namespace My
    
    <Global.System.Runtime.CompilerServices.CompilerGeneratedAttribute(),  _
     Global.System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "16.10.0.0"),  _
     Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)>  _
    Partial Friend NotInheritable Class MySettings
        Inherits Global.System.Configuration.ApplicationSettingsBase
        
        Private Shared defaultInstance As MySettings = CType(Global.System.Configuration.ApplicationSettingsBase.Synchronized(New MySettings()),MySettings)
        
#Region "My.Settings Auto-Save Functionality"
#If _MyType = "WindowsForms" Then
    Private Shared addedHandler As Boolean

    Private Shared addedHandlerLockObject As New Object

    <Global.System.Diagnostics.DebuggerNonUserCodeAttribute(), Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)> _
    Private Shared Sub AutoSaveSettings(sender As Global.System.Object, e As Global.System.EventArgs)
        If My.Application.SaveMySettingsOnExit Then
            My.Settings.Save()
        End If
    End Sub
#End If
#End Region
        
        Public Shared ReadOnly Property [Default]() As MySettings
            Get
                
#If _MyType = "WindowsForms" Then
               If Not addedHandler Then
                    SyncLock addedHandlerLockObject
                        If Not addedHandler Then
                            AddHandler My.Application.Shutdown, AddressOf AutoSaveSettings
                            addedHandler = True
                        End If
                    End SyncLock
                End If
#End If
                Return defaultInstance
            End Get
        End Property
        
        <Global.System.Configuration.ApplicationScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("90000")>  _
        Public ReadOnly Property TimerInterval() As String
            Get
                Return CType(Me("TimerInterval"),String)
            End Get
        End Property
        
        <Global.System.Configuration.ApplicationScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("ftp://ftp.epsys.co.uk/Outbox")>  _
        Public ReadOnly Property FoodBuyOnline_FTP_SITE() As String
            Get
                Return CType(Me("FoodBuyOnline_FTP_SITE"),String)
            End Get
        End Property
        
        <Global.System.Configuration.ApplicationScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("wcmilk")>  _
        Public ReadOnly Property FoodBuyOnline_FTP_USER() As String
            Get
                Return CType(Me("FoodBuyOnline_FTP_USER"),String)
            End Get
        End Property
        
        <Global.System.Configuration.ApplicationScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("QWkgMxwC5")>  _
        Public ReadOnly Property FoodBuyOnline_FTP_PW() As String
            Get
                Return CType(Me("FoodBuyOnline_FTP_PW"),String)
            End Get
        End Property
        
        <Global.System.Configuration.ApplicationScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("administrator@wcmilk.co.uk")>  _
        Public ReadOnly Property EMAIL_FROM() As String
            Get
                Return CType(Me("EMAIL_FROM"),String)
            End Get
        End Property
        
        <Global.System.Configuration.ApplicationScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("peter@wcmilk.co.uk")>  _
        Public ReadOnly Property Email_Success() As String
            Get
                Return CType(Me("Email_Success"),String)
            End Get
        End Property
        
        <Global.System.Configuration.ApplicationScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("\\wcm-exe-fp01\OfficeShare\FoodBuy_Online\ORDERS")>  _
        Public ReadOnly Property FoodBuyOnline_ORDERS() As String
            Get
                Return CType(Me("FoodBuyOnline_ORDERS"),String)
            End Get
        End Property
        
        <Global.System.Configuration.ApplicationScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("peter@wcmilk.co.uk")>  _
        Public ReadOnly Property Email_Error() As String
            Get
                Return CType(Me("Email_Error"),String)
            End Get
        End Property
        
        <Global.System.Configuration.ApplicationScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("0")>  _
        Public ReadOnly Property Switch_FoodBuyOnline() As Byte
            Get
                Return CType(Me("Switch_FoodBuyOnline"),Byte)
            End Get
        End Property
        
        <Global.System.Configuration.ApplicationScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("1")>  _
        Public ReadOnly Property Switch_Interserve_saffron() As String
            Get
                Return CType(Me("Switch_Interserve_saffron"),String)
            End Get
        End Property
        
        <Global.System.Configuration.ApplicationScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("ftp://fdh.ftpstream.com/Live/ToSaffron")>  _
        Public ReadOnly Property INTERSERVE_FTP_SITE_RESPONSE_SAFFRON() As String
            Get
                Return CType(Me("INTERSERVE_FTP_SITE_RESPONSE_SAFFRON"),String)
            End Get
        End Property
        
        <Global.System.Configuration.ApplicationScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("ftp://fdh.ftpstream.com/Test/ToSaffron")>  _
        Public ReadOnly Property INTERSERVE_FTP_SITE_RESPONSE_SAFFRON_TEST() As String
            Get
                Return CType(Me("INTERSERVE_FTP_SITE_RESPONSE_SAFFRON_TEST"),String)
            End Get
        End Property
        
        <Global.System.Configuration.ApplicationScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("ftp://fdh.ftpstream.com/Live/FromSaffron")>  _
        Public ReadOnly Property INTERSERVE_FTP_SITE_SAFFRON() As String
            Get
                Return CType(Me("INTERSERVE_FTP_SITE_SAFFRON"),String)
            End Get
        End Property
        
        <Global.System.Configuration.ApplicationScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("ftp://fdh.ftpstream.com/Test/FromSaffron")>  _
        Public ReadOnly Property INTERSERVE_FTP_SITE_SAFFRON_TEST() As String
            Get
                Return CType(Me("INTERSERVE_FTP_SITE_SAFFRON_TEST"),String)
            End Get
        End Property
        
        <Global.System.Configuration.ApplicationScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("westcountrymilk")>  _
        Public ReadOnly Property INTERSERVE_FTP_USER_SAFFRON() As String
            Get
                Return CType(Me("INTERSERVE_FTP_USER_SAFFRON"),String)
            End Get
        End Property
        
        <Global.System.Configuration.ApplicationScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("Se{Ng3@{w.")>  _
        Public ReadOnly Property INTERSERVE_FTP_PW_SAFFRON() As String
            Get
                Return CType(Me("INTERSERVE_FTP_PW_SAFFRON"),String)
            End Get
        End Property
        
        <Global.System.Configuration.ApplicationScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("\\wcm-exe-fp01\OfficeShare\INTERSERVE\ORDER_IN_SAFFRON")>  _
        Public ReadOnly Property INTERSERVE_ORDERS_SAFFRON() As String
            Get
                Return CType(Me("INTERSERVE_ORDERS_SAFFRON"),String)
            End Get
        End Property
        
        <Global.System.Configuration.ApplicationScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("\\wcm-exe-fp01\OfficeShare\INTERSERVE\ASN_SAFFRON")>  _
        Public ReadOnly Property INTERSERVE_RESPONSE_SAFFRON() As String
            Get
                Return CType(Me("INTERSERVE_RESPONSE_SAFFRON"),String)
            End Get
        End Property
        
        <Global.System.Configuration.ApplicationScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("\\wcm-exe-fp01\OfficeShare\INTERSERVE\ASN_TEST_SAFFRON")>  _
        Public ReadOnly Property INTERSERVE_RESPONSE_SAFFRON_TEST() As String
            Get
                Return CType(Me("INTERSERVE_RESPONSE_SAFFRON_TEST"),String)
            End Get
        End Property
        
        <Global.System.Configuration.ApplicationScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("\\wcm-exe-fp01\OfficeShare\INTERSERVE\ORDER_IN_TEST_SAFFRON")>  _
        Public ReadOnly Property INTERSERVE_ORDERS_SAFFRON_TEST() As String
            Get
                Return CType(Me("INTERSERVE_ORDERS_SAFFRON_TEST"),String)
            End Get
        End Property
        
        <Global.System.Configuration.ApplicationScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("2")>  _
        Public ReadOnly Property Switch_Freshpastures() As String
            Get
                Return CType(Me("Switch_Freshpastures"),String)
            End Get
        End Property
        
        <Global.System.Configuration.ApplicationScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("ftp://edi-fp.deliverysoftware.co.uk")>  _
        Public ReadOnly Property Freshpastures_FTP_SITE() As String
            Get
                Return CType(Me("Freshpastures_FTP_SITE"),String)
            End Get
        End Property
        
        <Global.System.Configuration.ApplicationScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("wcmilk-fp")>  _
        Public ReadOnly Property Freshpastures_FTP_USER() As String
            Get
                Return CType(Me("Freshpastures_FTP_USER"),String)
            End Get
        End Property
        
        <Global.System.Configuration.ApplicationScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("v0Fq65TjkD*fUq#x9a9e")>  _
        Public ReadOnly Property Freshpastures_FTP_PW() As String
            Get
                Return CType(Me("Freshpastures_FTP_PW"),String)
            End Get
        End Property
        
        <Global.System.Configuration.ApplicationScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("\\WCM-EXE-FP01\Officeshare\DairyData\FreshPastures\out")>  _
        Public ReadOnly Property Freshpastures_ORDERS() As String
            Get
                Return CType(Me("Freshpastures_ORDERS"),String)
            End Get
        End Property
        
        <Global.System.Configuration.ApplicationScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("0")>  _
        Public ReadOnly Property Switch_MillsMilk() As Integer
            Get
                Return CType(Me("Switch_MillsMilk"),Integer)
            End Get
        End Property
        
        <Global.System.Configuration.ApplicationScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("")>  _
        Public ReadOnly Property MillsMilk_FTP_SITE() As String
            Get
                Return CType(Me("MillsMilk_FTP_SITE"),String)
            End Get
        End Property
        
        <Global.System.Configuration.ApplicationScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("")>  _
        Public ReadOnly Property MillsMilk_FTP_USER() As String
            Get
                Return CType(Me("MillsMilk_FTP_USER"),String)
            End Get
        End Property
        
        <Global.System.Configuration.ApplicationScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("")>  _
        Public ReadOnly Property MillsMilk_FTP_PW() As String
            Get
                Return CType(Me("MillsMilk_FTP_PW"),String)
            End Get
        End Property
        
        <Global.System.Configuration.ApplicationScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("\\WCM-EXE-FP01\Officeshare\DairyData\MillsMilk")>  _
        Public ReadOnly Property MillsMilk_ORDERS() As String
            Get
                Return CType(Me("MillsMilk_ORDERS"),String)
            End Get
        End Property
    End Class
End Namespace

Namespace My
    
    <Global.Microsoft.VisualBasic.HideModuleNameAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Runtime.CompilerServices.CompilerGeneratedAttribute()>  _
    Friend Module MySettingsProperty
        
        <Global.System.ComponentModel.Design.HelpKeywordAttribute("My.Settings")>  _
        Friend ReadOnly Property Settings() As Global.WCM_FTP.My.MySettings
            Get
                Return Global.WCM_FTP.My.MySettings.Default
            End Get
        End Property
    End Module
End Namespace