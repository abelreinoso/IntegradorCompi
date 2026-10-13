# Script para regenerar Scanner.cs y Parser.cs desde QuizLang.atg usando Coco/R
$ErrorActionPreference = "Stop"

$scriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$repoToolsDir = $scriptDir
$cocoExe = Join-Path $repoToolsDir "coco\Coco.exe"
$framesDir = Join-Path $repoToolsDir "coco"

$coreDir = Join-Path (Split-Path -Parent $repoToolsDir) "QuizLang.Core"
$atgFile = Join-Path $coreDir "QuizLang.atg"

if (-not (Test-Path $cocoExe)) {
    Write-Error "No se encontró Coco.exe en: $cocoExe"
    exit 1
}

if (-not (Test-Path $atgFile)) {
    Write-Error "No se encontró la gramática en: $atgFile"
    exit 1
}

Write-Host "Regenerando Scanner y Parser con Coco/R..." -ForegroundColor Cyan
Write-Host "  Gramática: $atgFile"
Write-Host "  Frames:    $framesDir"
Write-Host "  Destino:   $coreDir"

$proc = Start-Process -FilePath $cocoExe -ArgumentList "`"$atgFile`"", "-namespace", "QuizLang.Core", "-frames", "`"$framesDir`"", "-o", "`"$coreDir`"" -NoNewWindow -Wait -PassThru

if ($proc.ExitCode -ne 0) {
    Write-Error "Coco/R finalizó con código de error $($proc.ExitCode)"
    exit $proc.ExitCode
}

Write-Host "Generación exitosa sin errores." -ForegroundColor Green
exit 0
