# 🚀 Instrucciones de Ejecución - Real Estate Application

## 📋 Pasos para Ejecutar el Proyecto

### 1. Prerrequisitos

Asegúrate de tener instalado:

- ✅ .NET 8 SDK
- ✅ Node.js 18+
- ✅ MongoDB 6+ (o Docker)
- ✅ Git

### 2. Clonar y Configurar el Proyecto

```bash
# Clonar el repositorio
git clone <repository-url>
cd real-estate-million

# Verificar que estás en la rama correcta
git status
```

### 3. Configurar la Base de Datos

#### Opción A: Con Docker (Recomendado)

```bash
# Ejecutar MongoDB con Docker
docker-compose up -d

# Verificar que MongoDB está corriendo
docker ps
```

#### Opción B: MongoDB Local

```bash
# Si tienes MongoDB instalado localmente
mongod --dbpath /ruta/a/tu/db

# En otra terminal, ejecutar el script de inicialización
mongosh < init-mongo.js
```

### 4. Ejecutar el Backend (API)

```bash
# Navegar al directorio del backend
cd src/RealEstate.WebApi

# Restaurar paquetes NuGet
dotnet restore

# Compilar el proyecto
dotnet build

# Ejecutar la API
dotnet run
```

**La API estará disponible en:**

- 🔗 <https://localhost:7000> (HTTPS)
- 🔗 <http://localhost:5000> (HTTP)
- 📚 Swagger UI: <https://localhost:7000> o <http://localhost:5000>

### 5. Ejecutar el Frontend (React)

```bash
# Abrir una nueva terminal
cd client

# Instalar dependencias
npm install

# Ejecutar en modo desarrollo
npm start
```

**El frontend estará disponible en:**

- 🌐 <http://localhost:3000>

### 6. Verificar que Todo Funciona

1. **Backend**: Ve a <http://localhost:5000> y verifica que Swagger se carga
2. **Frontend**: Ve a <http://localhost:3000> y verifica que la aplicación se carga
3. **API**: Prueba el endpoint `GET /api/property/simple` en Swagger

## 🧪 Ejecutar Pruebas

### Pruebas del Backend

```bash
# Desde la raíz del proyecto
cd tests/RealEstate.Application.Tests
dotnet test

cd ../RealEstate.WebApi.Tests
dotnet test
```

### Pruebas del Frontend

```bash
cd client
npm test
```

## 🔧 Solución de Problemas

### Error de Conexión a MongoDB

```bash
# Verificar que MongoDB está corriendo
docker ps | grep mongo

# Si no está corriendo, iniciarlo
docker-compose up -d

# Verificar logs
docker-compose logs mongo
```

### Error de Puerto en Uso

```bash
# Para el backend (puerto 5000/7000)
netstat -ano | findstr :5000
# Matar el proceso si es necesario

# Para el frontend (puerto 3000)
netstat -ano | findstr :3000
# Matar el proceso si es necesario
```

### Error de Dependencias

```bash
# Backend
cd src/RealEstate.WebApi
dotnet clean
dotnet restore
dotnet build

# Frontend
cd client
rm -rf node_modules package-lock.json
npm install
```

## 📊 Verificar Datos de Prueba

### Conectar a MongoDB

```bash
# Con Docker
docker exec -it real-estate-million-mongo-1 mongosh

# Local
mongosh
```

### Verificar Colecciones

```javascript
use RealEstateDB
show collections

// Verificar datos
db.Properties.find().pretty()
db.Owners.find().pretty()
db.PropertyImages.find().pretty()
```

## 🌐 URLs Importantes

- **Frontend**: <http://localhost:3000>
- **Backend API**: <http://localhost:5000>
- **Swagger UI**: <http://localhost:5000>
- **MongoDB**: mongodb://localhost:27017/RealEstateDB

## 📱 Funcionalidades a Probar

### 1. Listado de Propiedades

- Ve a <http://localhost:3000>
- Verifica que se cargan las propiedades
- Prueba la paginación

### 2. Filtros de Búsqueda

- Usa el filtro de nombre: "Casa"
- Usa el filtro de dirección: "Bogotá"
- Ajusta el rango de precios
- Prueba "Limpiar Filtros"

### 3. API Endpoints

- `GET /api/property/simple` - Listado simple
- `GET /api/property/search/simple?name=Casa` - Búsqueda
- `GET /api/property/{id}` - Detalles (usa un ID de la base de datos)

## 🐳 Comandos Docker Útiles

```bash
# Iniciar servicios
docker-compose up -d

# Ver logs
docker-compose logs -f

# Parar servicios
docker-compose down

# Recrear contenedores
docker-compose up -d --force-recreate

# Limpiar volúmenes
docker-compose down -v
```

## 📝 Notas Importantes

1. **Primera Ejecución**: La primera vez puede tardar más en cargar las dependencias
2. **Datos de Prueba**: Se cargan automáticamente con el script `init-mongo.js`
3. **CORS**: Está configurado para permitir conexiones desde localhost:3000
4. **Hot Reload**: El frontend se recarga automáticamente al hacer cambios
5. **Logs**: Revisa la consola del navegador y la terminal para errores

## 🆘 Si Algo No Funciona

1. **Verifica los prerrequisitos** - .NET 8, Node.js, MongoDB
2. **Revisa los logs** - Tanto backend como frontend
3. **Verifica los puertos** - 3000, 5000, 7000, 27017
4. **Reinicia los servicios** - Docker, backend, frontend
5. **Limpia y reinstala** - node_modules, bin/obj folders

## 📞 Contacto

Si tienes problemas, contacta a:

- <crios@millionluxury.com>
- <amorau@millionluxury.com>

---

**¡Disfruta explorando la aplicación! 🎉**
