# Script de PowerShell para configurar y cargar datos en MongoDB
# Este script espera a que MongoDB esté listo y luego carga los datos iniciales

Write-Host "🚀 Configurando base de datos Real Estate..." -ForegroundColor Green

# Función para verificar si MongoDB está listo
function Wait-ForMongoDB {
    Write-Host "⏳ Esperando a que MongoDB esté listo..." -ForegroundColor Yellow
    
    $maxAttempts = 30
    $attempt = 1
    
    while ($attempt -le $maxAttempts) {
        try {
            $result = docker exec realestate-mongodb mongosh --eval "db.adminCommand('ping')" --quiet 2>$null
            if ($LASTEXITCODE -eq 0) {
                Write-Host "✅ MongoDB está listo!" -ForegroundColor Green
                return $true
            }
        }
        catch {
            # Continuar intentando
        }
        
        Write-Host "   Intento $attempt/$maxAttempts - MongoDB no está listo aún..." -ForegroundColor Yellow
        Start-Sleep -Seconds 2
        $attempt++
    }
    
    Write-Host "❌ Error: MongoDB no respondió después de $maxAttempts intentos" -ForegroundColor Red
    return $false
}

# Función para cargar datos
function Load-Data {
    Write-Host "📊 Cargando datos iniciales..." -ForegroundColor Cyan
    
    try {
        # Copiar el script al contenedor
        docker cp scripts/load-data.js realestate-mongodb:/tmp/load-data.js
        
        # Ejecutar el script de carga de datos
        $result = docker exec realestate-mongodb mongosh --file /tmp/load-data.js
        
        if ($LASTEXITCODE -eq 0) {
            Write-Host "✅ Datos cargados exitosamente!" -ForegroundColor Green
            return $true
        } else {
            Write-Host "❌ Error al cargar los datos" -ForegroundColor Red
            return $false
        }
    }
    catch {
        Write-Host "❌ Error al cargar los datos: $($_.Exception.Message)" -ForegroundColor Red
        return $false
    }
}

# Función para verificar si los datos ya existen
function Test-ExistingData {
    Write-Host "🔍 Verificando si ya existen datos..." -ForegroundColor Cyan
    
    try {
        $ownersCount = docker exec realestate-mongodb mongosh --eval "db.getSiblingDB('RealEstateDB').Owners.countDocuments()" --quiet 2>$null | Select-Object -Last 1
        $propertiesCount = docker exec realestate-mongodb mongosh --eval "db.getSiblingDB('RealEstateDB').Properties.countDocuments()" --quiet 2>$null | Select-Object -Last 1
        
        if ([int]$ownersCount -gt 0 -or [int]$propertiesCount -gt 0) {
            Write-Host "⚠️  La base de datos ya contiene datos:" -ForegroundColor Yellow
            Write-Host "   - Propietarios: $ownersCount" -ForegroundColor Yellow
            Write-Host "   - Propiedades: $propertiesCount" -ForegroundColor Yellow
            Write-Host "   Saltando la carga de datos iniciales." -ForegroundColor Yellow
            return $false
        }
        
        return $true
    }
    catch {
        Write-Host "⚠️  No se pudo verificar los datos existentes, continuando con la carga..." -ForegroundColor Yellow
        return $true
    }
}

# Función principal
function Main {
    Write-Host "🏠 Real Estate Database Setup" -ForegroundColor Magenta
    Write-Host "==============================" -ForegroundColor Magenta
    
    # Verificar si MongoDB está ejecutándose
    $mongoContainer = docker ps --filter "name=realestate-mongodb" --format "table {{.Names}}" | Select-String "realestate-mongodb"
    
    if (-not $mongoContainer) {
        Write-Host "❌ Error: El contenedor de MongoDB no está ejecutándose" -ForegroundColor Red
        Write-Host "   Ejecuta primero: docker-compose up -d mongodb" -ForegroundColor Yellow
        exit 1
    }
    
    # Esperar a que MongoDB esté listo
    if (-not (Wait-ForMongoDB)) {
        exit 1
    }
    
    # Verificar si ya existen datos
    if (Test-ExistingData) {
        # Cargar datos si no existen
        if (Load-Data) {
            Write-Host ""
            Write-Host "🎉 ¡Configuración completada exitosamente!" -ForegroundColor Green
            Write-Host "   La base de datos está lista para usar." -ForegroundColor Green
        } else {
            Write-Host ""
            Write-Host "❌ Error durante la configuración" -ForegroundColor Red
            exit 1
        }
    } else {
        Write-Host ""
        Write-Host "✅ La base de datos ya está configurada" -ForegroundColor Green
    }
    
    Write-Host ""
    Write-Host "📋 Información de conexión:" -ForegroundColor Cyan
    Write-Host "   - Host: localhost" -ForegroundColor White
    Write-Host "   - Puerto: 27017" -ForegroundColor White
    Write-Host "   - Base de datos: RealEstateDB" -ForegroundColor White
    Write-Host "   - Usuario: admin" -ForegroundColor White
    Write-Host "   - Contraseña: password123" -ForegroundColor White
}

# Ejecutar función principal
Main
