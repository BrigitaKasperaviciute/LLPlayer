# Test Benchmark Script - Runs tests 20 times and calculates average execution time
# Usage: .\run-tests-benchmark.ps1

param(
    [int]$Iterations = 20,
    [string]$Filter = ""
)

Write-Host "=====================================================" -ForegroundColor Cyan
Write-Host "Test Benchmark Runner" -ForegroundColor Cyan
Write-Host "Running tests $Iterations times to calculate average execution time" -ForegroundColor Cyan
Write-Host "=====================================================" -ForegroundColor Cyan
Write-Host ""

$times = @()
$failedRuns = 0
$passedTests = 0
$totalTests = 0

for ($i = 1; $i -le $Iterations; $i++) {
    Write-Host "[$i/$Iterations] Running tests..." -ForegroundColor Yellow

    # Run tests and capture output
    if ($Filter) {
        $output = & dotnet test FlyleafLibTests.csproj --verbosity quiet --nologo --filter $Filter 2>&1 | Out-String
    }
    else {
        $output = & dotnet test FlyleafLibTests.csproj --verbosity quiet --nologo 2>&1 | Out-String
    }

    # Extract execution time from output (multiple formats)
    $executionTime = $null

    if ($output -match "Total time:\s+([\d.]+)\s+Seconds") {
        $executionTime = [double]$matches[1]
    }
    elseif ($output -match "Duration:\s+([\d.]+)\s*ms") {
        $executionTime = [double]$matches[1] / 1000
    }
    elseif ($output -match "Duration:\s+(\d+:\d+:\d+\.\d+)") {
        # Handle format like "0:00:00.123"
        if ($matches[1] -match "(\d+):(\d+):(\d+)\.(\d+)") {
            $hours = [int]$matches[1]
            $minutes = [int]$matches[2]
            $seconds = [int]$matches[3]
            $fraction = "0." + $matches[4]
            $executionTime = $hours * 3600 + $minutes * 60 + $seconds + [double]$fraction
        }
    }

    if ($executionTime) {
        $times += $executionTime
        Write-Host "  Execution Time: $executionTime seconds" -ForegroundColor Green
    }
    else {
        Write-Host "  Could not parse execution time from output" -ForegroundColor Red
        # Debug: Show output to help diagnose (uncomment if needed)
        # Write-Host "  Output: $($output.Substring(0, [Math]::Min(300, $output.Length)))" -ForegroundColor DarkGray
    }

    # Extract test results
    if ($output -match "Passed:\s+(\d+)") {
        $passedTests = [int]$matches[1]
    }
    if ($output -match "Total:\s+(\d+)") {
        $totalTests = [int]$matches[1]
    }

    # Check for failures
    if ($output -match "Failed:\s+(\d+)" -and [int]$matches[1] -gt 0) {
        Write-Host "  WARNING: Some tests failed!" -ForegroundColor Red
        $failedRuns++
    }
}

Write-Host ""
Write-Host "=====================================================" -ForegroundColor Cyan
Write-Host "Benchmark Results" -ForegroundColor Cyan
Write-Host "=====================================================" -ForegroundColor Cyan

if ($times.Count -gt 0) {
    $average = ($times | Measure-Object -Average).Average
    $min = ($times | Measure-Object -Minimum).Minimum
    $max = ($times | Measure-Object -Maximum).Maximum
    $median = ($times | Sort-Object)[[Math]::Floor($times.Count / 2)]

    Write-Host ""
    Write-Host "Total Runs:            $Iterations" -ForegroundColor White
    Write-Host "Successful Runs:       $($Iterations - $failedRuns)" -ForegroundColor Green
    Write-Host "Failed Runs:           $failedRuns" -ForegroundColor $(if ($failedRuns -gt 0) { "Red" } else { "Green" })
    Write-Host ""
    Write-Host "Test Statistics:" -ForegroundColor Yellow
    Write-Host "  Total Tests:         $totalTests" -ForegroundColor White
    Write-Host "  Passed Tests:        $passedTests" -ForegroundColor Green
    Write-Host ""
    Write-Host "Execution Time Statistics:" -ForegroundColor Yellow
    Write-Host "  Average Time:        $([Math]::Round($average, 4)) seconds" -ForegroundColor Cyan
    Write-Host "  Median Time:         $([Math]::Round($median, 4)) seconds" -ForegroundColor White
    Write-Host "  Minimum Time:        $([Math]::Round($min, 4)) seconds" -ForegroundColor Green
    Write-Host "  Maximum Time:        $([Math]::Round($max, 4)) seconds" -ForegroundColor Magenta
    Write-Host "  Total Time:          $([Math]::Round(($times | Measure-Object -Sum).Sum, 4)) seconds" -ForegroundColor White
    Write-Host ""

    # Calculate standard deviation
    $variance = ($times | ForEach-Object { [Math]::Pow($_ - $average, 2) } | Measure-Object -Average).Average
    $stdDev = [Math]::Sqrt($variance)
    Write-Host "  Standard Deviation:  $([Math]::Round($stdDev, 4)) seconds" -ForegroundColor White

    # Show all individual times
    Write-Host ""
    Write-Host "Individual Run Times:" -ForegroundColor Yellow
    for ($i = 0; $i -lt $times.Count; $i++) {
        $runColor = if ($times[$i] -lt $average) { "Green" } elseif ($times[$i] -gt $average) { "Magenta" } else { "White" }
        Write-Host "  Run $($i + 1): $([Math]::Round($times[$i], 4)) seconds" -ForegroundColor $runColor
    }
}
else {
    Write-Host "No valid execution times captured!" -ForegroundColor Red
}

Write-Host ""
Write-Host "=====================================================" -ForegroundColor Cyan
