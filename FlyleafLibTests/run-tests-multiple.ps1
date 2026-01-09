#!/usr/bin/env pwsh

# Run dotnet tests 20 times and calculate average run time

$projectPath = "c:\Users\pauli\Desktop\experiment1\LLPlayer\FlyleafLibTests\FlyleafLibTests.csproj"
$iterations = 20
$times = @()

Write-Host "Running tests $iterations times..." -ForegroundColor Cyan
Write-Host ""

for ($i = 1; $i -le $iterations; $i++) {
    Write-Host "Run $i/$iterations..." -NoNewline
    
    $startTime = Get-Date
    $output = dotnet test $projectPath --no-build -v minimal 2>&1
    $endTime = Get-Date
    
    $duration = ($endTime - $startTime).TotalSeconds
    $times += $duration
    
    Write-Host " ${duration}s" -ForegroundColor Green
}

Write-Host ""
Write-Host "Test Results:" -ForegroundColor Cyan
Write-Host "============="

$avgTime = ($times | Measure-Object -Average).Average
$minTime = ($times | Measure-Object -Minimum).Minimum
$maxTime = ($times | Measure-Object -Maximum).Maximum

Write-Host "Total runs:  $iterations"
Write-Host "Average:     $([math]::Round($avgTime, 2))s" -ForegroundColor Yellow
Write-Host "Minimum:     $([math]::Round($minTime, 2))s"
Write-Host "Maximum:     $([math]::Round($maxTime, 2))s"
Write-Host ""
Write-Host "All run times:"
for ($i = 0; $i -lt $times.Count; $i++) {
    Write-Host "  Run $($i+1): $([math]::Round($times[$i], 2))s"
}
