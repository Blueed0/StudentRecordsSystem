@echo off
cd /d "%~dp0"

REM Onceki kilitli sureci kapat (build hatasini onlemek icin)
taskkill /IM StudentRecordsSystem.exe /F >nul 2>&1

dotnet run
if errorlevel 1 pause
