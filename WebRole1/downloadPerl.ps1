
$perlDir = "p"
$zip_file = 'strawberry-perl-5.14.2.1-64bit-portable.zip'
$local_path = (Get-Location).Path
$local_zip = "$local_path\$zip_file"
$remote_zip = "http://dfstloader.blob.core.windows.net/perl/$zip_file"

Write-Output "executing Powershell Script, downloadPerl.ps1"
Try
{
	if (!(Test-Path($local_zip))) #if the perl zip has already been downloaded, don't do it again
	{
		# Download the file from blob storage
		Write-Output "Downloading from $remote_zip and saving locally to $local_zip"
		$client = new-object System.Net.WebClient
		$client.DownloadFile($remote_zip, $local_zip)
	}
	exit 0
}
Catch [system.exception]
{
	Write-Error $_.Exception.Message
	Write-Error $remote_zip
	Write-Error $local_zip
}
exit 0