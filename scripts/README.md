# 📁 Scripts de Base de Datos

Esta carpeta contiene scripts para configurar y cargar datos iniciales en la base de datos MongoDB del proyecto Real Estate.

## 📋 Archivos Incluidos

### `load-data.js`

Script de MongoDB que carga datos iniciales en la base de datos. Incluye:

- 3 propietarios de ejemplo
- 7 propiedades con diferentes características
- 7 imágenes de propiedades
- 3 trazas de transacciones

**Características:**

- ✅ Verifica si ya existen datos antes de cargar
- ✅ Manejo de errores robusto
- ✅ Mensajes informativos durante la ejecución
- ✅ Datos realistas de Colombia

### `setup-database.sh`

Script de Bash para Linux/macOS que automatiza la configuración de la base de datos:

- Espera a que MongoDB esté listo
- Verifica si ya existen datos
- Carga los datos iniciales si es necesario
- Proporciona información de conexión

### `setup-database.ps1`

Script de PowerShell para Windows con la misma funcionalidad que el script de Bash:

- Compatible con Windows PowerShell
- Colores y formato mejorado
- Manejo de errores específico para Windows

## 🚀 Uso

### Automático (Recomendado)

Los datos se cargan automáticamente cuando ejecutas:

```bash
docker-compose up --build
```

### Manual - Linux/macOS

```bash
# Hacer el script ejecutable
chmod +x scripts/setup-database.sh

# Ejecutar
./scripts/setup-database.sh
```

### Manual - Windows

```powershell
# Ejecutar en PowerShell
.\scripts\setup-database.ps1
```

### Manual - Directo

```bash
# Copiar script al contenedor
docker cp scripts/load-data.js realestate-mongodb:/tmp/load-data.js

# Ejecutar script
docker exec realestate-mongodb mongosh --file /tmp/load-data.js
```

## 🔍 Verificación

Para verificar que los datos se cargaron correctamente:

```bash
docker exec realestate-mongodb mongosh --eval "
  db = db.getSiblingDB('RealEstateDB');
  print('📊 Resumen de datos:');
  print('   - Propietarios:', db.Owners.countDocuments());
  print('   - Propiedades:', db.Properties.countDocuments());
  print('   - Imágenes:', db.PropertyImages.countDocuments());
  print('   - Trazas:', db.PropertyTraces.countDocuments());
"
```

## 📊 Datos de Ejemplo

### Propietarios

1. **Juan Carlos Pérez** - Bogotá
2. **María Elena Rodríguez** - Medellín  
3. **Carlos Alberto Mendoza** - Cali

### Propiedades

1. **Casa Moderna en Zona Norte** - Bogotá ($450,000,000)
2. **Villa Campestre El Poblado** - Medellín ($780,000,000)
3. **Apartamento Ejecutivo Centro** - Bogotá ($320,000,000)
4. **Casa Colonial Restaurada** - Cartagena ($1,200,000,000)
5. **Penthouse Vista al Mar** - Santa Marta ($950,000,000)
6. **Loft Industrial Zona Rosa** - Bogotá ($680,000,000)
7. **Casa Campestre Valle del Cauca** - Cali ($550,000,000)

## 🔧 Solución de Problemas

### El script no se ejecuta

1. Verificar que MongoDB esté ejecutándose:

   ```bash
   docker ps | grep mongodb
   ```

2. Verificar permisos del script (Linux/macOS):

   ```bash
   chmod +x scripts/setup-database.sh
   ```

### Los datos no se cargan

1. Verificar logs del contenedor:

   ```bash
   docker logs realestate-mongodb
   ```

2. Ejecutar manualmente:

   ```bash
   docker exec -it realestate-mongodb mongosh
   ```

### Error de conexión

1. Verificar que el contenedor esté en la red correcta:

   ```bash
   docker network ls
   docker network inspect realestate-network
   ```

## 📝 Personalización

Para agregar más datos o modificar los existentes:

1. Editar `load-data.js`
2. Seguir el formato de los datos existentes
3. Mantener las referencias entre ObjectIds
4. Probar el script antes de usar en producción

## 🔒 Seguridad

**Importante:** Los scripts incluyen credenciales de desarrollo. En producción:

- Cambiar las credenciales por defecto
- Usar variables de entorno
- Implementar autenticación robusta
- Revisar permisos de acceso
