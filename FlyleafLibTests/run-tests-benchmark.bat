@echo off
REM Test Benchmark Script - Runs tests 20 times and calculates average execution time
REM Usage: run-tests-benchmark.bat [iterations] [filter]

powershell -ExecutionPolicy Bypass -File "%~dp0run-tests-benchmark.ps1" %*
