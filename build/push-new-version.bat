@echo off
del *.nupkg
tools\nuget pack ..\source\pdfextract\PDFextract.csproj
tools\nuget push *.nupkg
