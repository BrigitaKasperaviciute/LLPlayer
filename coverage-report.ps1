# Generate Code Coverage Report
# Run tests with coverage collection
Write-Host "Collecting code coverage..."
dotnet test FlyleafLibTests/FlyleafLibTests.csproj --collect:"XPlat Code Coverage" -- DataCollectionRunSettings.DataCollectors.DataCollector.Configuration.Format=opencover

# Generate HTML report
Write-Host "Generating coverage report..."
reportgenerator -reports:"FlyleafLibTests\TestResults\*\coverage.opencover.xml" -targetdir:"coverage-report" -reporttypes:"Html"

Write-Host "Coverage report generated in 'coverage-report' folder"
Write-Host "Opening report..."
Invoke-Item coverage-report\index.html
