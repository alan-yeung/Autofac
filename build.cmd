@echo off
pushd %~dp0

SETLOCAL
SET CACHED_NUGET=%LocalAppData%\NuGet\NuGet.exe

IF EXIST %CACHED_NUGET% goto copynuget
echo Downloading latest version of NuGet.exe...
IF NOT EXIST %LocalAppData%\NuGet md %LocalAppData%\NuGet
@powershell -NoProfile -ExecutionPolicy unrestricted -Command "$ProgressPreference = 'SilentlyContinue'; Invoke-WebRequest 'https://www.nuget.org/nuget.exe' -OutFile '%CACHED_NUGET%'"

:copynuget
IF EXIST .nuget\nuget.exe goto dnvminstall
md .nuget
copy %CACHED_NUGET% .nuget\nuget.exe > nul

:dnvminstall
set "DNX_NUGET_API_URL=https://www.nuget.org/api/v2"
setlocal EnableDelayedExpansion 
where dnvm
if %ERRORLEVEL% neq 0 (
    @powershell -NoProfile -ExecutionPolicy unrestricted -Command "&{$Branch='dev';iex ((new-object net.webclient).DownloadString('https://raw.githubusercontent.com/aspnet/Home/dev/dnvminstall.ps1'))}"
    set PATH=!PATH!;!userprofile!\.dnx\bin
    set DNX_HOME=!USERPROFILE!\.dnx
    goto install
)

:install
call dnvm install 1.0.0-beta5-11511
call dnvm use 1.0.0-beta5-11511

:run
call dnu restore src\Autofac
if %errorlevel% neq 0 exit /b %errorlevel%

call dnu restore src\Autofac.Dnx
if %errorlevel% neq 0 exit /b %errorlevel%

call dnu restore test\Autofac.Test
if %errorlevel% neq 0 exit /b %errorlevel%

call dnu restore test\Autofac.Dnx.Test
if %errorlevel% neq 0 exit /b %errorlevel%

SETLOCAL ENABLEEXTENSIONS
IF ERRORLEVEL 1 ECHO Unable to enable extensions
IF DEFINED APPVEYOR_BUILD_NUMBER (SET DNX_BUILD_VERSION=%APPVEYOR_BUILD_NUMBER%) ELSE (SET DNX_BUILD_VERSION=1)
ECHO DNX_BUILD_VERSION=%DNX_BUILD_VERSION%

call dnu pack src\Autofac --configuration Release --out artifacts\packages
if %errorlevel% neq 0 exit /b %errorlevel%

call dnu pack src\Autofac.Dnx --configuration Release --out artifacts\packages
if %errorlevel% neq 0 exit /b %errorlevel%

call dnx test\Autofac.Test test
if %errorlevel% neq 0 exit /b %errorlevel%

call dnx test\Autofac.Dnx.Test test
if %errorlevel% neq 0 exit /b %errorlevel%

popd