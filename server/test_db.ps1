$hosts = @("db.ucxdhrikpgxgzbdkwzfc.supabase.co", "aws-0-ap-southeast-1.pooler.supabase.com")
$ports = @(5432, 6543)

foreach ($h in $hosts) {
    foreach ($p in $ports) {
        try {
            $tcp = New-Object System.Net.Sockets.TcpClient
            $tcp.Connect($h, $p)
            Write-Host "SUCCESS: Connection to $h on Port $p successful!" -ForegroundColor Green
            $tcp.Close()
        }
        catch {
            Write-Host "FAILED: $h on Port $p. Error: $($_.Exception.Message)" -ForegroundColor Red
        }
    }
}
