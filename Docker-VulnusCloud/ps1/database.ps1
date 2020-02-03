function CreateDatabaseSql($obj) {
	if ($obj.debug) {
		Write-Host "* Create database (" $obj.databaseName ")" -f magenta
	}			
	$conn = New-Object Data.SqlClient.SqlConnection($obj.connStr)
	$conn.Open()
	$command = 'CREATE DATABASE ' + $obj.databaseName
	$cmd = New-Object Data.SqlClient.SqlCommand($command, $conn)
	$da = New-Object Data.SqlClient.SqlDataAdapter($cmd)
	$ds = New-Object System.Data.DataSet
	$da.Fill($ds)
}