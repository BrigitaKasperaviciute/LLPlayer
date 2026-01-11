#!/bin/bash
# Test Benchmark Script - Runs tests 20 times and calculates average execution time
# Usage: ./run-tests-benchmark.sh [iterations] [filter]

ITERATIONS=${1:-20}
FILTER=${2:-""}

echo "====================================================="
echo "Test Benchmark Runner"
echo "Running tests $ITERATIONS times to calculate average execution time"
echo "====================================================="
echo ""

declare -a times
failed_runs=0
passed_tests=0
total_tests=0

for ((i=1; i<=ITERATIONS; i++)); do
    echo "[$i/$ITERATIONS] Running tests..."

    if [ -n "$FILTER" ]; then
        output=$(dotnet test FlyleafLibTests.csproj --verbosity quiet --nologo --filter "$FILTER" 2>&1)
    else
        output=$(dotnet test FlyleafLibTests.csproj --verbosity quiet --nologo 2>&1)
    fi

    # Extract execution time from output (handles both formats)
    if [[ $output =~ Total\ time:\ ([0-9.]+)\ Seconds ]]; then
        execution_time="${BASH_REMATCH[1]}"
        times+=("$execution_time")
        echo "  Execution Time: $execution_time seconds"
    elif [[ $output =~ Duration:\ ([0-9.]+)\ ms ]]; then
        execution_time=$(echo "scale=4; ${BASH_REMATCH[1]} / 1000" | bc)
        times+=("$execution_time")
        echo "  Execution Time: $execution_time seconds"
    else
        echo "  Could not parse execution time"
        ((failed_runs++))
    fi

    # Extract test results
    if [[ $output =~ Passed:\ +([0-9]+) ]]; then
        passed_tests="${BASH_REMATCH[1]}"
    fi
    if [[ $output =~ Total:\ +([0-9]+) ]]; then
        total_tests="${BASH_REMATCH[1]}"
    fi

    # Check for failures
    if [[ $output =~ Failed:\ +([0-9]+) ]] && [ "${BASH_REMATCH[1]}" -gt 0 ]; then
        echo "  WARNING: Some tests failed!"
        ((failed_runs++))
    fi
done

echo ""
echo "====================================================="
echo "Benchmark Results"
echo "====================================================="

if [ ${#times[@]} -gt 0 ]; then
    # Calculate average
    sum=0
    for time in "${times[@]}"; do
        sum=$(echo "scale=4; $sum + $time" | bc)
    done
    average=$(echo "scale=4; $sum / ${#times[@]}" | bc)

    # Find min and max
    min=${times[0]}
    max=${times[0]}
    for time in "${times[@]}"; do
        if (( $(echo "$time < $min" | bc -l) )); then
            min=$time
        fi
        if (( $(echo "$time > $max" | bc -l) )); then
            max=$time
        fi
    done

    # Calculate median
    sorted=($(printf '%s\n' "${times[@]}" | sort -n))
    median_index=$((${#sorted[@]} / 2))
    median=${sorted[$median_index]}

    echo ""
    echo "Total Runs:            $ITERATIONS"
    echo "Successful Runs:       $((ITERATIONS - failed_runs))"
    echo "Failed Runs:           $failed_runs"
    echo ""
    echo "Test Statistics:"
    echo "  Total Tests:         $total_tests"
    echo "  Passed Tests:        $passed_tests"
    echo ""
    echo "Execution Time Statistics:"
    echo "  Average Time:        $average seconds"
    echo "  Median Time:         $median seconds"
    echo "  Minimum Time:        $min seconds"
    echo "  Maximum Time:        $max seconds"
    echo "  Total Time:          $sum seconds"
    echo ""

    # Calculate standard deviation
    variance=0
    for time in "${times[@]}"; do
        diff=$(echo "scale=4; $time - $average" | bc)
        sq=$(echo "scale=4; $diff * $diff" | bc)
        variance=$(echo "scale=4; $variance + $sq" | bc)
    done
    variance=$(echo "scale=4; $variance / ${#times[@]}" | bc)
    stddev=$(echo "scale=4; sqrt($variance)" | bc)
    echo "  Standard Deviation:  $stddev seconds"

    # Show all individual times
    echo ""
    echo "Individual Run Times:"
    for i in "${!times[@]}"; do
        echo "  Run $((i + 1)): ${times[$i]} seconds"
    done
else
    echo "No valid execution times captured!"
fi

echo ""
echo "====================================================="
