# 🚀 Guía de Despliegue - Real Estate App

## Requisitos Previos

- Docker Desktop instalado y ejecutándose
- Git (para clonar el repositorio)

## Despliegue Rápido

### Opción 1: Script Automático (Recomendado)

**En Windows:**

```bash
deploy.bat
```

**En Linux/Mac:**

```bash
./deploy.sh
```

### Opción 2: Comandos Manuales

1. **Clonar el repositorio:**

```bash
git clone <tu-repositorio>
cd real-estate-million
```

2. **Ejecutar la aplicación:**

```bash
docker-compose up --build
```

3. **Acceder a la aplicación:**

- **Frontend:** <http://localhost:3000>
- **Backend API:** <http://localhost:5000>
- **MongoDB:** localhost:27017

## Servicios Incluidos

### 🗄️ Base de Datos (MongoDB)

- **Puerto:** 27017
- **Usuario:** admin
- **Contraseña:** password123
- **Base de datos:** RealEstateDB
- **Datos iniciales:** Se cargan automáticamente

### 🔧 Backend API (.NET)

- **Puerto:** 5000
- **Endpoints disponibles:**
  - `GET /api/property` - Listar propiedades
  - `GET /api/property/{id}` - Obtener propiedad por ID
  - `GET /api/owner` - Listar propietarios
  - `GET /api/owner/{id}` - Obtener propietario por ID

### 🎨 Frontend (React)

- **Puerto:** 3000
- **Funcionalidades:**
  - Lista de propiedades con filtros
  - Detalles de propiedades
  - Lista de propietarios
  - Detalles de propietarios con sus propiedades

## Comandos Útiles

### Ver logs de todos los servicios

```bash
docker-compose logs -f
```

### Ver logs de un servicio específico

```bash
docker-compose logs -f frontend
docker-compose logs -f api
docker-compose logs -f mongodb
```

### Detener la aplicación

```bash
docker-compose down
```

### Reiniciar un servicio específico

```bash
docker-compose restart frontend
```

### Ver estado de los servicios

```bash
docker-compose ps
```

## Solución de Problemas

### Si el frontend no se conecta al backend

1. Verificar que el backend esté ejecutándose en el puerto 5000
2. Revisar los logs: `docker-compose logs -f api`

### Si la base de datos no tiene datos

1. Verificar que el servicio db-setup se ejecutó correctamente
2. Revisar los logs: `docker-compose logs -f db-setup`

### Si hay problemas de puertos

- Verificar que los puertos 3000, 5000 y 27017 estén libres
- Cambiar los puertos en docker-compose.yml si es necesario

## Estructura de la Aplicación

```
real-estate-million/
├── client/                 # Frontend React
├── src/                   # Backend .NET
├── scripts/               # Scripts de base de datos
├── docker-compose.yml     # Configuración de Docker
├── deploy.bat            # Script de despliegue Windows
├── deploy.sh             # Script de despliegue Linux/Mac
└── DEPLOYMENT.md         # Esta guía
```

## Datos de Prueba

La aplicación incluye datos de prueba que se cargan automáticamente:

- 10 propiedades de ejemplo
- 5 propietarios con sus respectivas propiedades
- Imágenes de ejemplo para las propiedades

## Notas Importantes

- La aplicación está configurada para desarrollo
- Los datos se persisten en un volumen de Docker
- El frontend se conecta automáticamente al backend
- Todos los servicios se reinician automáticamente si fallan
