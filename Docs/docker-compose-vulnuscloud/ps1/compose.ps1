function Reset ($obj) {
	# Reset	
	docker-compose down -v
	
	# StartDatabase
	docker-compose up -d $obj.containerDb
	
	# Build `vulnuscloud-git-clone`
	docker-compose build --no-cache
	
	# StartWeb
	docker-compose up -d $obj.containerWeb
}

function BuildSource ($obj) {
	# Reset
	docker-compose -f $obj.alternateComposeFileBuildSource down -v
	
	# StartDatabase
	docker-compose -f $obj.alternateComposeFileBuildSource up -d $obj.containerDb
	
	# Build `vulnuscloud-git-clone` and `vulnuscloud-web`
	docker-compose -f $obj.alternateComposeFileBuildSource build --no-cache
	
	# StartWeb
	docker-compose -f $obj.alternateComposeFileBuildSource up -d $obj.containerWeb
}

function StartAll ($obj) {
	# StartDatabase
	docker-compose up -d $obj.containerDb
	
	# StartWeb
	docker-compose up -d $obj.containerWeb
}


# Write-Host " ... wait for" $obj.globalWait "seconds for" $obj.containerDb "to start"
# Start-Sleep $obj.globalWait