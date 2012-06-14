RunningPerlInAzure
==================

An ASP.Net WebRole which downloads and installs Strawberry Perl, and then uses quartz.net to schedule the running of perl scripts.

VS2010 Instructions:
1. Open solution
2. Use nuget to retrieve Quartz.Net and its dependencies
3. Download 7za.exe, the 7-Zip Command Line Version, available here: http://www.7-zip.org/download.html. Place this in the root of WebRole1 and set “Copy to Output Directory” to “Copy always”.
4. Edit downloadPerl.ps1 and set the url to the strawberry perl zip file.
5. Build and Run

Because the startup task downloads a large zip file and extracts it, the startup will take longer than normal.

Note that the Azure development environment will create directories in your project. 
You can view the startup logs by browsing to this directory:
\approot\bin\startuplogs 

In the development environment, during debugging, this would be under [Your Cloud Project]\csx\Debug\roles\WebRoot1.  In the cloud I found this under an E:\ or F:\ drive (while connecting via RDP).

Also in \approot\bin you can find the perl zip download (during/after it's been downloaded), and the perl install in \p (during/after it's been installed).
