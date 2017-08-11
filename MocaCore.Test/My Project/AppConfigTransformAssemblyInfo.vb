
' プログラム要素が CLS (Common Language Specification) に準拠しているかどうかを示します
<Assembly: System.CLSCompliant(True)>

' The Apache Software Foundation log4net Logging Framework
' When you use , please install the log4net from nuget.
'   log4net.config を log4net の設定ファイルとする
<Assembly: log4net.Config.XmlConfigurator(ConfigFile:="log4net.config", Watch:=True)>

' App.config Section Protection setting
' When you use , please install the Moca.NETConfiguration from nuget.
'<Assembly: Moca.Configuration.SectionProtection(Moca.Configuration.ProtectionProviderType.DPAPI, "Section Name")>
