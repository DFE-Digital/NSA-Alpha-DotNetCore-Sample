@echo off
cls

Powershell.exe -executionpolicy remotesigned -File .\Init.ps1

.\.paket\paket.exe update

.\packages\FAKE.Core\tools\FAKE.exe build.fsx publishDirectory=.\..\Publish buildMode=Release