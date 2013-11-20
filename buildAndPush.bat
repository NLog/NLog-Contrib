cd _output
nuget.exe update -self
ECHO Y | DEL *.nupkg

#Restore packages in solution
nuget.exe restore "..\Nlog-Contrib.sln"

#NOTE - Using an account for nlog-contrib@meinershagen.net to publish to NuGet.org.  This can be changed in the future.
nuget.exe setApiKey 11967121-c94f-4d54-9461-c2e441758bc0

#NOTE - If you are only updating one of the packages you can comment out the "nuget.exe pack" statement of the one that you are not deploying.
#nuget.exe pack "..\NLog.Elmah\NLog.Elmah.csproj" -Build -Properties Configuration=Release
#nuget.exe pack "..\NLog.ManualFlush\NLog.ManualFlush.csproj" -Build -Properties Configuration=Release
nuget.exe pack "..\NLog.Contrib\NLog.Contrib.csproj" -Build -Properties Configuration=Release

nuget.exe push *.nupkg
cd ..
