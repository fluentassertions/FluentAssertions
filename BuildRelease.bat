CALL "%VS120COMNTOOLS%vsvars32.bat"

msbuild /p:Configuration=Release /p:Platform="Any CPU" /t:Rebuild FluentAssertions.sln

tools\nuget pack package\.nuspec -o package