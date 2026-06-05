# ═══════════════════════════════════════════════════════════════════
# deploy-rfid.ps1 — Empaquetador R2000Demo por entorno
# Sprint 8 T7 · Pystelectronic · Ing. José Hernán Liza Garavito
#
# USO:
#   .\deploy-rfid.ps1 -Entorno dev
#   .\deploy-rfid.ps1 -Entorno prod
#   .\deploy-rfid.ps1 -Entorno prod -ModuloId "PORTAL-TIENDA-LIMA-02"
# ═══════════════════════════════════════════════════════════════════
param(
    [ValidateSet("dev","prod")]
    [string]$Entorno = "prod",

    # Opcional: sobreescribir el ModuloId para esta PC específica
    [string]$ModuloId = ""
)

$ProyectoRaiz = Split-Path -Parent $MyInvocation.MyCommand.Path
$BinRelease   = Join-Path $ProyectoRaiz "bin\Release"
$ConfigSrc    = Join-Path $ProyectoRaiz "app.config.$Entorno"
$ConfigDest   = Join-Path $BinRelease   "R2000Demo.exe.config"
$ZipNombre    = "RFID_Pystelectronic_${Entorno}_v1.0.zip"
$ZipDestino   = Join-Path $HOME "Desktop\$ZipNombre"

Write-Host ""
Write-Host "═══════════════════════════════════════════════" -ForegroundColor Cyan
Write-Host "  R2000Demo — Empaquetador · Entorno: $($Entorno.ToUpper())" -ForegroundColor Cyan
Write-Host "═══════════════════════════════════════════════" -ForegroundColor Cyan

# Validaciones
if (-not (Test-Path $BinRelease)) {
    Write-Host "❌ No se encontró bin\Release\. Compila en modo Release primero." -ForegroundColor Red
    exit 1
}
if (-not (Test-Path $ConfigSrc)) {
    Write-Host "❌ No se encontró $ConfigSrc" -ForegroundColor Red
    exit 1
}

# Copiar app.config del entorno seleccionado
Write-Host "📋 Copiando app.config.$Entorno → bin\Release\..." -ForegroundColor Yellow
Copy-Item -Path $ConfigSrc -Destination $ConfigDest -Force

# Sobreescribir ModuloId si se indicó
if ($ModuloId -ne "") {
    Write-Host "🔧 Aplicando ModuloId = $ModuloId ..." -ForegroundColor Yellow
    $xml = [xml](Get-Content $ConfigDest)
    $node = $xml.configuration.appSettings.add | Where-Object { $_.key -eq "ModuloId" }
    if ($node) { $node.value = $ModuloId }
    $xml.Save($ConfigDest)
    Write-Host "   ✅ ModuloId actualizado" -ForegroundColor Green
}

# Generar LEEME.txt
$serverUrl = ([xml](Get-Content $ConfigDest)).configuration.appSettings.add |
             Where-Object { $_.key -eq "RfidServerUrl" } |
             Select-Object -ExpandProperty value
$moduloId  = ([xml](Get-Content $ConfigDest)).configuration.appSettings.add |
             Where-Object { $_.key -eq "ModuloId" } |
             Select-Object -ExpandProperty value

$readme = @"
══════════════════════════════════════════════════
  RFID Pystelectronic v1.0 — Entorno: $($Entorno.ToUpper())
  Generado: $(Get-Date -Format 'dd/MM/yyyy HH:mm')
══════════════════════════════════════════════════

REQUISITOS:
  - Windows 7/10/11 · .NET Framework 4.8
  - SQL Server Express (sqlexpress)
  - Conexión de red al servidor RFID

CONFIGURACIÓN ACTIVA:
  Servidor : $serverUrl
  Módulo ID: $moduloId

SOPORTE: Ing. José Hernán Liza Garavito — Pystelectronic
══════════════════════════════════════════════════
"@
Set-Content -Path (Join-Path $BinRelease "LEEME.txt") -Value $readme -Encoding UTF8

# Crear ZIP
Write-Host "📦 Creando $ZipNombre ..." -ForegroundColor Yellow
if (Test-Path $ZipDestino) { Remove-Item $ZipDestino -Force }
Add-Type -AssemblyName System.IO.Compression.FileSystem
[System.IO.Compression.ZipFile]::CreateFromDirectory(
    $BinRelease, $ZipDestino,
    [System.IO.Compression.CompressionLevel]::Optimal, $false)

$mb = [math]::Round((Get-Item $ZipDestino).Length / 1MB, 2)
Write-Host ""
Write-Host "✅ ZIP generado: $ZipNombre ($mb MB)" -ForegroundColor Green
Write-Host "   Destino: $ZipDestino"
Write-Host ""
Write-Host "Para otras PCs con ID distinto:" -ForegroundColor Cyan
Write-Host '  .\deploy-rfid.ps1 -Entorno prod -ModuloId "PORTAL-TIENDA-LIMA-02"'
