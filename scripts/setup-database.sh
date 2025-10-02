#!/bin/bash

# Script para configurar y cargar datos en MongoDB
# Este script espera a que MongoDB esté listo y luego carga los datos iniciales

echo "🚀 Configurando base de datos Real Estate..."

# Función para verificar si MongoDB está listo
wait_for_mongodb() {
    echo "⏳ Esperando a que MongoDB esté listo..."
    
    max_attempts=30
    attempt=1
    
    while [ $attempt -le $max_attempts ]; do
        if docker exec realestate-mongodb mongosh --eval "db.adminCommand('ping')" --quiet > /dev/null 2>&1; then
            echo "✅ MongoDB está listo!"
            return 0
        fi
        
        echo "   Intento $attempt/$max_attempts - MongoDB no está listo aún..."
        sleep 2
        ((attempt++))
    done
    
    echo "❌ Error: MongoDB no respondió después de $max_attempts intentos"
    return 1
}

# Función para cargar datos
load_data() {
    echo "📊 Cargando datos iniciales..."
    
    # Copiar el script al contenedor
    docker cp scripts/load-data.js realestate-mongodb:/tmp/load-data.js
    
    # Ejecutar el script de carga de datos
    if docker exec realestate-mongodb mongosh --file /tmp/load-data.js; then
        echo "✅ Datos cargados exitosamente!"
        return 0
    else
        echo "❌ Error al cargar los datos"
        return 1
    fi
}

# Función para verificar si los datos ya existen
check_existing_data() {
    echo "🔍 Verificando si ya existen datos..."
    
    owners_count=$(docker exec realestate-mongodb mongosh --eval "db.getSiblingDB('RealEstateDB').Owners.countDocuments()" --quiet 2>/dev/null | tail -1)
    properties_count=$(docker exec realestate-mongodb mongosh --eval "db.getSiblingDB('RealEstateDB').Properties.countDocuments()" --quiet 2>/dev/null | tail -1)
    
    if [ "$owners_count" -gt 0 ] || [ "$properties_count" -gt 0 ]; then
        echo "⚠️  La base de datos ya contiene datos:"
        echo "   - Propietarios: $owners_count"
        echo "   - Propiedades: $properties_count"
        echo "   Saltando la carga de datos iniciales."
        return 1
    fi
    
    return 0
}

# Función principal
main() {
    echo "🏠 Real Estate Database Setup"
    echo "=============================="
    
    # Verificar si MongoDB está ejecutándose
    if ! docker ps | grep -q realestate-mongodb; then
        echo "❌ Error: El contenedor de MongoDB no está ejecutándose"
        echo "   Ejecuta primero: docker-compose up -d mongodb"
        exit 1
    fi
    
    # Esperar a que MongoDB esté listo
    if ! wait_for_mongodb; then
        exit 1
    fi
    
    # Verificar si ya existen datos
    if check_existing_data; then
        # Cargar datos si no existen
        if load_data; then
            echo ""
            echo "🎉 ¡Configuración completada exitosamente!"
            echo "   La base de datos está lista para usar."
        else
            echo ""
            echo "❌ Error durante la configuración"
            exit 1
        fi
    else
        echo ""
        echo "✅ La base de datos ya está configurada"
    fi
    
    echo ""
    echo "📋 Información de conexión:"
    echo "   - Host: localhost"
    echo "   - Puerto: 27017"
    echo "   - Base de datos: RealEstateDB"
    echo "   - Usuario: admin"
    echo "   - Contraseña: password123"
}

# Ejecutar función principal
main "$@"
