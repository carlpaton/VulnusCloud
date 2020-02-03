Param(
    [switch]$Reset,
	[switch]$Debug
)

. ".\ps1\functions.ps1"

InformUser($config)

if ($Reset) 
{
	Reset($config)
	Build($config)
	CreateDatabaseSql($config)
	Flyway($config)
} 
else 
{
	StartAll($config)
}

Debug($config)
EndOfScript($config)