# Test Benchmark Script - Runs tests multiple times and calculates average execution time
param(
    [int]$Iterations = 10,
    [string]$Filter = "",
    [switch]$Coverage = $false
)

Write-Host "=====================================================" -ForegroundColor Cyan
Write-Host "Test Benchmark Runner" -ForegroundColor Cyan
Write-Host "Running tests $Iterations times to calculate average execution time" -ForegroundColor Cyan
Write-Host "=====================================================" -ForegroundColor Cyan
Write-Host ""

# Suppress workload warnings
$env:DOTNET_NOLOGO = "true"

# Build once before benchmarking
Write-Host "Building project (one-time)..." -ForegroundColor Yellow
dotnet build --nologo --verbosity quiet 2>&1 | Out-Null
Write-Host "Build complete." -ForegroundColor Green
Write-Host ""

# Warmup run to initialize JIT and caches
Write-Host "Running warmup..." -ForegroundColor Yellow
$warmupCommand = "dotnet test --nologo --no-build --verbosity quiet"
if ($Filter -ne "") {
    $warmupCommand += " --filter `"$Filter`""
}
Invoke-Expression $warmupCommand 2>&1 | Out-Null
Write-Host "Warmup complete. Starting benchmark..." -ForegroundColor Green
Write-Host ""

$times = @()
$failedRuns = 0
$passedTests = 0
$totalTests = 0

for ($i = 1; $i -le $Iterations; $i++) {
    Write-Host "[$i/$Iterations] Running tests..." -ForegroundColor Yellow

    # Build test command
    $testCommand = "dotnet test --nologo --no-build"
    if ($Filter -ne "") {
        $testCommand += " --filter `"$Filter`""
    }
    if ($Coverage) {
        $testCommand += " --collect:`"XPlat Code Coverage`""
    }

    # Measure execution time
    $stopwatch = [System.Diagnostics.Stopwatch]::StartNew()

    # Run the test
    $output = Invoke-Expression $testCommand 2>&1 | Where-Object { $_ -notmatch "Workload" } | Out-String

    $stopwatch.Stop()
    $elapsed = $stopwatch.Elapsed.TotalSeconds
    $times += $elapsed

    Write-Host "  Execution Time: $([Math]::Round($elapsed, 4)) seconds" -ForegroundColor Green

    # Extract test results
    if ($output -match "Passed:\s+(\d+)") {
        $passedTests = [int]$matches[1]
    }
    if ($output -match "Total:\s+(\d+)") {
        $totalTests = [int]$matches[1]
    }
    if ($output -match "Failed:\s+(\d+)" -and [int]$matches[1] -gt 0) {
        Write-Host "  WARNING: Some tests failed!" -ForegroundColor Red
        $failedRuns++
    }

    Write-Host ""
}

Write-Host "=====================================================" -ForegroundColor Cyan
Write-Host "Benchmark Results" -ForegroundColor Cyan
Write-Host "=====================================================" -ForegroundColor Cyan

if ($times.Count -gt 0) {
    $average = ($times | Measure-Object -Average).Average
    $min = ($times | Measure-Object -Minimum).Minimum
    $max = ($times | Measure-Object -Maximum).Maximum
    $median = ($times | Sort-Object)[[Math]::Floor($times.Count / 2)]
    $totalTime = ($times | Measure-Object -Sum).Sum

    Write-Host ""
    Write-Host "Run Statistics:" -ForegroundColor Yellow
    Write-Host "  Total Runs:          $Iterations" -ForegroundColor White
    Write-Host "  Successful Runs:     $($Iterations - $failedRuns)" -ForegroundColor Green
    Write-Host "  Failed Runs:         $failedRuns" -ForegroundColor $(if ($failedRuns -gt 0) { "Red" } else { "Green" })
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
    Write-Host "  Total Time:          $([Math]::Round($totalTime, 4)) seconds" -ForegroundColor White

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
