# Script de ejecución y verificación de pruebas para QuizLang
$ErrorActionPreference = "Stop"

$scriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$tfDir = $scriptDir
$slnPath = Join-Path $tfDir "QuizLang.sln"
$testsDir = Join-Path $tfDir "tests"
$cliExe = Join-Path $tfDir "QuizLang.Cli\bin\Debug\net10.0\QuizLang.Cli.exe"

[Console]::OutputEncoding = [System.Text.Encoding]::UTF8

Write-Host "=============================================" -ForegroundColor Cyan
Write-Host " Compilando QuizLang.sln..." -ForegroundColor Cyan
Write-Host "=============================================" -ForegroundColor Cyan

$buildResult = dotnet build $slnPath --nologo
if ($LASTEXITCODE -ne 0) {
    Write-Host "Error al compilar la solución QuizLang.sln" -ForegroundColor Red
    exit 1
}

Write-Host "`nEjecutando suite de pruebas en: $testsDir`n" -ForegroundColor Cyan

$quizFiles = Get-ChildItem -Path $testsDir -Filter "*.quiz" | Sort-Object Name
$total = $quizFiles.Count
$passed = 0
$failed = 0

foreach ($quiz in $quizFiles) {
    $baseName = [System.IO.Path]::GetFileNameWithoutExtension($quiz.Name)
    $expectedFile = Join-Path $testsDir "$baseName.expected"

    if (-not (Test-Path $expectedFile)) {
        Write-Host "[FAIL] $($quiz.Name) - No se encontró archivo .expected" -ForegroundColor Red
        $failed++
        continue
    }

    $expectedText = (Get-Content -Path $expectedFile -Raw -Encoding UTF8).Trim()
    
    # Ejecutar CLI
    $output = (& $cliExe $quiz.FullName 2>&1) -join "`n"
    $exitCode = $LASTEXITCODE
    $actualTrimmed = $output.Trim()

    $isPass = $false

    if ($expectedText -eq "OK") {
        if ($exitCode -eq 0) {
            $isPass = $true
        } else {
            $isPass = $false
        }
    } else {
        # Para casos de error: exit code debe ser 1 y contener el mensaje esperado
        if ($exitCode -eq 1 -and $actualTrimmed.Contains($expectedText)) {
            $isPass = $true
        } else {
            $isPass = $false
        }
    }

    if ($isPass) {
        Write-Host "  [PASS] $($quiz.Name)" -ForegroundColor Green
        $passed++
    } else {
        Write-Host "  [FAIL] $($quiz.Name)" -ForegroundColor Red
        Write-Host "         Esperado : $expectedText" -ForegroundColor Yellow
        Write-Host "         Obtenido : $actualTrimmed (ExitCode: $exitCode)" -ForegroundColor Red
        $failed++
    }
}

Write-Host "`n---------------------------------------------" -ForegroundColor Cyan
$summaryLine = "Resultados: $passed/$total pruebas pasaron (PASS)."
if ($failed -eq 0) {
    Write-Host $summaryLine -ForegroundColor Green
    exit 0
} else {
    Write-Host "$summaryLine ($failed fallaron)" -ForegroundColor Red
    exit 1
}
