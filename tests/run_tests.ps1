# Wrapper para ejecutar run_tests.ps1 desde tests\
$scriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$parentDir = Split-Path -Parent $scriptDir
$mainScript = Join-Path $parentDir "run_tests.ps1"
& $mainScript
