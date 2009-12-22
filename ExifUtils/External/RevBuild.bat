@ECHO off

SET AutoVer=AutoVersioning\AutoVersioning.exe
SET SolnDir=..\
SET BaseVersion=1 1

IF NOT EXIST %AutoVer% (
	ECHO ERROR %AutoVer% not found.
	GOTO :Done
)

%AutoVer% "%SolnDir%ExifUtils\Properties\AssemblyVersion.cs" %BaseVersion%

:Done
PAUSE