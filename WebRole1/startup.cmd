REM   This startup batch file installs perl if it's not already installed
ECHO %time%  >> .\startuplogs\startupLog.txt

REM create directory to store startup logs
mkdir startuplogs

REM Check to see if perl is already installed. If it isn't then install it
if exist %CD%\p\perl\bin\perl.exe (
	ECHO Perl already installed >> .\startuplogs\startupLog.txt
) else (
	ECHO Installing Perl >> .\startuplogs\startupLog.txt

	REM Create a directory for perl and unzip the perl exe into it, using a Powershell script
	ECHO %time%  >> .\startuplogs\startupLog.txt
	mkdir p 
	PowerShell -ExecutionPolicy Unrestricted .\installPerl.ps1 >> .\startuplogs\startupLog.txt 2>> .\startuplogs\startupErr.txt
	ECHO %time%  >>  .\startuplogs\startupLog.txt

	if %errorlevel% == 0 (
		ECHO Perl installation completed. >>  .\startuplogs\startupLog.txt
	) else (
		ECHO Perl installation failed. >>  .\startuplogs\startupLog.txt
		EXIT /B %errorlevel%
	)
)

REM If perl install is successful, go ahead and install the modules we need
ECHO Installing CPAN Modules >>  .\startuplogs\startupLog.txt
p\perl\bin\cpan.bat Config::Tiny HTML::TreeBuilder	>>  .\startuplogs\startupLog.txt  2>> .\startuplogs\startupErr.txt

ECHO %time%  >>  .\startuplogs\startupLog.txt
EXIT /B 0
