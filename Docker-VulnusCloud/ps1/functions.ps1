. ".\ps1\config.ps1"
. ".\ps1\database.ps1"
. ".\ps1\flyway.ps1"
. ".\ps1\compose.ps1"

function InformUser ($obj) {
	$debugLog = "Parameters: "
	$debugLog += "-Reset " + $obj.reset
	$debugLog += " -Debug " + $obj.debug

	Clear-Host
	Write-Host $debugLog `n -f green
}

function EndOfScript ($obj) {	
	$param = "network=" + $obj.network	
	docker ps --filter $param
	Write-Host ""
	Write-Host "End of script" -f green
}

function Debug ($obj) {
	if ($obj.debug) {
		Write-Host "* List all containers" -f magenta
		docker ps --all
		Write-Host "* List volume data" -f magenta
		#docker run -it --rm -v docker-compose-lexicon_flyway_sql:/vol busybox ls -l /vol
		#docker run -it --rm -v docker-compose-lexicon_flyway_sql:/vol busybox cat /vol/V1.0.4__entry.sql
		
		#docker run -it --rm -v docker-compose-lexicon_flyway_raw_code:/vol busybox ls -l /vol
		docker logs vulnuscloud-migrate
	}
}