# Test Benchmark Runner

Scripts to run tests multiple times and calculate average execution time statistics.

## Files

- `run-tests-benchmark.ps1` - PowerShell script (Windows/Linux/macOS with PowerShell Core)
- `run-tests-benchmark.bat` - Windows batch file wrapper
- `run-tests-benchmark.sh` - Bash script (Linux/macOS/Git Bash)

## Usage

### Windows (PowerShell)

```powershell
# Run all tests 20 times (default)
.\run-tests-benchmark.ps1

# Run with custom iteration count
.\run-tests-benchmark.ps1 -Iterations 50

# Run only integration tests
.\run-tests-benchmark.ps1 -Filter "FullyQualifiedName~LLPlayerCoreIntegrationTests"

# Run unit tests only
.\run-tests-benchmark.ps1 -Filter "FullyQualifiedName~SubManagerTests"

# Custom iterations and filter
.\run-tests-benchmark.ps1 -Iterations 30 -Filter "FullyQualifiedName~LLPlayerCoreIntegrationTests"
```

### Windows (Batch File)

```batch
REM Run all tests 20 times
run-tests-benchmark.bat

REM Run with custom iterations
run-tests-benchmark.bat 50

REM Run with filter
run-tests-benchmark.bat 20 "FullyQualifiedName~LLPlayerCoreIntegrationTests"
```

### Linux/macOS (Bash)

```bash
# Make script executable (first time only)
chmod +x run-tests-benchmark.sh

# Run all tests 20 times
./run-tests-benchmark.sh

# Run with custom iteration count
./run-tests-benchmark.sh 50

# Run only integration tests
./run-tests-benchmark.sh 20 "FullyQualifiedName~LLPlayerCoreIntegrationTests"
```

## Output

The scripts provide comprehensive statistics:

```
=====================================================
Benchmark Results
=====================================================

Total Runs:            20
Successful Runs:       20
Failed Runs:           0

Test Statistics:
  Total Tests:         18
  Passed Tests:        18

Execution Time Statistics:
  Average Time:        0.1194 seconds
  Median Time:         0.097 seconds
  Minimum Time:        0.08 seconds
  Maximum Time:        0.287 seconds
  Total Time:          2.388 seconds

  Standard Deviation:  0.0536 seconds

Individual Run Times:
  Run 1: 0.115 seconds
  Run 2: 0.09 seconds
  ...
```

## Metrics Explained

- **Average Time**: Mean execution time across all runs
- **Median Time**: Middle value when all times are sorted
- **Minimum/Maximum**: Fastest and slowest run times
- **Standard Deviation**: Measure of variability/consistency
- **Total Time**: Sum of all execution times

## Filter Examples

```powershell
# Run all tests
.\run-tests-benchmark.ps1

# Run only integration tests
.\run-tests-benchmark.ps1 -Filter "FullyQualifiedName~LLPlayerCoreIntegrationTests"

# Run only unit tests
.\run-tests-benchmark.ps1 -Filter "FullyQualifiedName~SubManagerTests|SubtitleTextUtilTests"

# Run specific endpoint tests
.\run-tests-benchmark.ps1 -Filter "FullyQualifiedName~Endpoint7_SpeedPlayback"

# Run only positive scenario tests
.\run-tests-benchmark.ps1 -Filter "FullyQualifiedName~POSITIVE"

# Run only negative scenario tests
.\run-tests-benchmark.ps1 -Filter "FullyQualifiedName~NEGATIVE"
```

## Requirements

- .NET 9.0 SDK or later
- PowerShell 5.1+ (Windows) or PowerShell Core 7+ (cross-platform)
- Bash 4.0+ (for `.sh` script)

## Performance Tips

1. **Close other applications** before running benchmarks for more consistent results
2. **Run multiple iterations** (30-50) for more accurate averages
3. **Standard deviation** indicates consistency - lower is better
4. **Median** is often more reliable than average for skewed distributions

## Example: Comparing Test Performance

```powershell
# Benchmark all tests
.\run-tests-benchmark.ps1 -Iterations 30 > results-all.txt

# Benchmark integration tests only
.\run-tests-benchmark.ps1 -Iterations 30 -Filter "FullyQualifiedName~LLPlayerCoreIntegrationTests" > results-integration.txt

# Benchmark unit tests only
.\run-tests-benchmark.ps1 -Iterations 30 -Filter "FullyQualifiedName~SubManagerTests|SubtitleTextUtilTests" > results-unit.txt
```

## Troubleshooting

### PowerShell Execution Policy Error

```powershell
# Temporarily bypass execution policy
powershell -ExecutionPolicy Bypass -File run-tests-benchmark.ps1
```

### Script Not Found

Make sure you're in the `FlyleafLibTests` directory:

```bash
cd FlyleafLibTests
```

### Permission Denied (Linux/macOS)

```bash
chmod +x run-tests-benchmark.sh
```
