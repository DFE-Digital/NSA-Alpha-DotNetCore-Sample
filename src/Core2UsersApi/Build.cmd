@echo off
cls

.\.paket\paket.exe update

.\packages\FAKE.Core\tools\FAKE.exe build.fsx publishDirectory=.\..\Publish buildMode=Release