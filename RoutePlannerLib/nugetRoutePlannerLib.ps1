Remove-Item *.nupkg
nuget pack .\RoutePlannerLib.csproj -Symbols -Prop Configuration=Release
nuget setApiKey 5e1f774e-7321-47bd-ab10-225e1758a777
nuget push .\Ch.Fhnw.Ecnf.JFLW.RoutePlannerLib.*.nupkg -s https://int.nugettest.org
read-host -Prompt "Press Enter to continue"