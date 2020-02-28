function StartDatabase ($obj) {
	if ($obj.debug) {
		Write-Host "* Run container detatched (" $obj.containerDb ")" -f magenta
	}
	docker-compose up -d $obj.containerDb

	if ($obj.debug) {
		Write-Host " ... wait for" $obj.globalWait "seconds for" $obj.containerDb "to start"
	}	
	Start-Sleep $obj.globalWait
}

function StartWeb ($obj) {
	docker-compose up -d $obj.containerWeb
}

function StartAll ($obj) {
	StartDatabase($obj)
	StartWeb($obj)	
}

function Reset ($obj) {
	if ($obj.debug) {
		Write-Host "* Kill and delete containers/volumes/network" -f magenta
	}	
	docker-compose down -v
	StartAll($obj)
}

function Build ($obj) {
	if ($obj.debug) {
		Write-Host "* Docker compose build" -f magenta
	}	
	docker-compose build --no-cache
}