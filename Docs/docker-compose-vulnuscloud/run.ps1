Param(
    [switch]$Reset,
	[switch]$BuildSource
)

. ".\ps1\functions.ps1"

InformUser($config)

if (($BuildSource) -and ($Reset))
{
    Write-Host " *** BuildSource and Reset " -f magenta
	BuildSource($config)
	CreateDatabaseSql($config)
	Flyway($config)
}
elseif ($BuildSource)
{
	Write-Host " *** BuildSource " -f magenta
	BuildSource($config)	
	CreateDatabaseSql($config)
	Flyway($config)	
}
elseif ($Reset) 
{
	Write-Host " *** Reset " -f magenta
	Reset($config)
	CreateDatabaseSql($config)
	Flyway($config)
} 
else 
{
	Write-Host " *** StartAll " -f magenta
	StartAll($config)
}

Debug($config)
EndOfScript($config)