Remove-Item *.nupkg
nuget pack .\RoutePlannerLib.csproj -Symbols -Prop Configuration=Release
nuget setApiKey 743ab68c-34d7-43e8-8b3a-67c951020200 -s https://int.nugettest.org
nuget push .\Ch.Fhnw.Ecnf.JFLW.RoutePlannerLib.*.nupkg -s https://int.nugettest.org
read-host -Prompt "Press Enter to continue"