function Flyway($obj) {
	#BASELINE
	docker-compose up -d $obj.containerBaseline
	Write-Host " ... wait for" $obj.globalWait "seconds for flyway baseline"
	Start-Sleep $obj.globalWait
	if ($obj.debug) {
		Write-Host "* logs from baseline" -f magenta
		docker logs $obj.containerBaseline
	}
	
	#MIGRATE
	docker-compose up -d $obj.containerMigrate
	Write-Host " ... wait for" $obj.globalWait "seconds for flyway migrate"
	Start-Sleep $obj.globalWait
	if ($obj.debug) {
		Write-Host "* logs from migrate" -f magenta
		docker logs $obj.containerMigrate	
	}	
}