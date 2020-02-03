class ConfigurationData {
   [Boolean]$reset
   [Boolean]$debug
   [int]$globalWait
   [string]$containerDb
   [string]$containerWeb
   [string]$containerBaseline
   [string]$containerMigrate   
   [string]$databaseName
   [string]$connStr
   [string]$network   
}

$config = [ConfigurationData]::new()
$config.reset = $Reset
$config.debug = $Debug
$config.globalWait = 1
$config.containerDb = "vulnuscloud-db"
$config.containerWeb = "vulnuscloud-web"
$config.containerBaseline = "vulnuscloud-baseline"
$config.containerMigrate = "vulnuscloud-migrate"
$config.databaseName = "vulnuscloud"
$config.connStr = "Server=localhost;User Id=sa;Password=Password123;"  # Connection to SQL Instance to create database
$config.network = "docker-vulnuscloud_default"                         # Network name prefix comes from the executing folders dir `Docker-VulnusCloud`