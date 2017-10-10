@ECHO OFF
for /f "delims=" %%a in ('hostname') do @set myvar=%%a
set dbLoc="Data Source=%myvar%\SQLEXPRESS;Integrated Security=True;"
dbCreator "MediaDashboard" %dbLoc% ".\DBScripts"