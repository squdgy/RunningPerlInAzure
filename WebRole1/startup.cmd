REM   This startup batch file installs perl if it's not already installed
ECHO %time%  >> .\startuplogs\startupLog.txt
cd /d "%~dp0"

REM create directory to store startup logs
mkdir startuplogs

REM Check to see if perl is already installed. If it isn't then install it
if exist %CD%\p\perl\bin\perl.exe (
	ECHO Perl already installed >> .\startuplogs\startupLog.txt
) else (
	ECHO Installing Perl >> .\startuplogs\startupLog.txt

	REM Download the perl script from blob storage via PowerShell script
	PowerShell -ExecutionPolicy Unrestricted .\downloadPerl.ps1 >> .\startuplogs\startupLog.txt 2>> .\startuplogs\startupErr.txt

	if %errorlevel% == 0 (
		ECHO Unzipping perl >>  .\startuplogs\startupLog.txt
		7za x strawberry-perl-5.14.2.1-64bit-portable.zip -op -y >> .\startuplogs\startupLog.txt 2>> .\startuplogs\startupErr.txt
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
