@ECHO off
PUSHD "%~dp0"

SET MSBuild=%SystemRoot%\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe
SET MSBuild35=%SystemRoot%\Microsoft.NET\Framework\v3.5\MSBuild.exe
IF NOT EXIST "%MSBuild%" (
	ECHO Installation of .NET Framework 4.0 is required to build this project, including .NET v2.0 and v3.5 releases
	ECHO http://www.microsoft.com/downloads/details.aspx?FamilyID=0a391abd-25c1-4fc0-919f-b21f31ab88b7
	START /d "~\iexplore.exe" http://www.microsoft.com/downloads/details.aspx?FamilyID=0a391abd-25c1-4fc0-919f-b21f31ab88b7
	EXIT /b 1
	GOTO END
)

REM Unit Tests ------------------------------------------------------

ECHO.
ECHO Building unit test pass...
ECHO.

"%MSBuild%" ExifUtils.sln /target:rebuild /property:TargetFrameworkVersion=v4.0;Configuration=Release;RunTests=True

REM Standard CLR ----------------------------------------------------

IF NOT EXIST "keys\ExifUtils_Key.pfx" (
	SET Configuration=Release
) ELSE (
	SET Configuration=Signed
)

SET FrameworkVer=v2.0 v3.5 v4.0

ECHO.
ECHO Building specific releases for .NET Framework (%FrameworkVer%)...
ECHO.

FOR %%i IN (%FrameworkVer%) DO "%MSBuild%" ExifUtils/ExifUtils.csproj /target:rebuild /property:TargetFrameworkVersion=%%i;Configuration=%Configuration%

ECHO.
ECHO Copying files for packages...
xcopy build\%Configuration%_v4.0\*.* "packages\lib\net40\" /Y
xcopy build\%Configuration%_v3.5\*.* "packages\lib\net35\" /Y
xcopy build\%Configuration%_v2.0\*.* "packages\lib\net20\" /Y
xcopy build\%Configuration%_v3.5_netcf\*.* "packages\lib\netcf35\" /Y
xcopy build\%Configuration%_v3.5_Silverlight\*.* "packages\lib\sl35\" /Y
xcopy build\%Configuration%_v4.0_Silverlight\*.* "packages\lib\sl40\" /Y
xcopy build\%Configuration%_v4.0_WindowsPhone\*.* "packages\lib\sl40-wp\" /Y

:END
POPD
