# 🐳 Configuración Docker - Real Estate Application

Este documento explica cómo ejecutar la aplicación completa usando Docker, ideal para que los clientes puedan probar la aplicación sin instalar nada en su equipo.

## 📋 Requisitos Previos

- Docker Desktop instalado
- Docker Compose v2.0+
- Puertos disponibles: 3000, 5000, 27017

## 🚀 Ejecución Completa con Docker

### Aplicación Completa (Recomendado para Evaluación)

```bash
# 1. Clonar el repositorio
git clone <repository-url>
cd real-estate-million

# 2. Ejecutar toda la aplicación
docker-compose up --build -d

# 3. Insertar datos de prueba (opcional)
docker exec realestate-mongodb mongosh --username admin --password password123 --authenticationDatabase admin --eval "
use RealEstateDB;
db.Owners.insertMany([
  {
    name: 'Juan Carlos Pérez',
    address: 'Calle 123 # 45-67, Bogotá',
    photo: 'https://images.unsplash.com/photo-1507003211169-0a1dd7228f2d?w=300&h=300&fit=crop',
    birthday: new Date('1985-03-15'),
    createdAt: new Date(),
    updatedAt: new Date()
  },
  {
    name: 'María Elena Rodríguez', 
    address: 'Avenida 80 # 123-45, Medellín',
    photo: 'https://images.unsplash.com/photo-1494790108755-2616b612b786?w=300&h=300&fit=crop',
    birthday: new Date('1978-07-22'),
    createdAt: new Date(),
    updatedAt: new Date()
  }
]);
"
```

### Opción 2: Solo Base de Datos (Para Desarrollo Local)

```bash
# Solo levantar MongoDB
docker-compose up -d mongodb
```

## 🌐 URLs de Acceso

Una vez que todos los servicios estén corriendo:

- **Frontend (React)**: <http://localhost:3000>
- **Backend API**: <http://localhost:5000>
- **Swagger Documentation**: <http://localhost:5000/swagger>
- **MongoDB**: localhost:27017

## 📊 Verificar Estado de los Servicios

```bash
# Ver contenedores corriendo
docker ps

# Ver logs de un servicio específico
docker logs realestate-frontend-dev
docker logs realestate-api-dev
docker logs realestate-mongodb

# Verificar salud de la API
curl http://localhost:5000/api/Property/simple
```

## 🛠️ Comandos Útiles

### Gestión de Contenedores

```bash
# Detener todos los servicios
docker-compose down

# Detener y eliminar volúmenes (limpia la BD)
docker-compose down -v

# Reconstruir imágenes
docker-compose up --build

# Ver logs en tiempo real
docker-compose logs -f
```

### Base de Datos

```bash
# Conectar a MongoDB
docker exec -it realestate-mongodb mongosh --username admin --password password123 --authenticationDatabase admin

# Verificar datos
docker exec realestate-mongodb mongosh --username admin --password password123 --authenticationDatabase admin --eval "
use RealEstateDB;
print('Owners:', db.Owners.countDocuments());
print('Properties:', db.Properties.countDocuments());
"
```

## 🏗️ Arquitectura Docker

```
┌─────────────────┐    ┌─────────────────┐    ┌─────────────────┐
│   Frontend      │    │   Backend       │    │   Database      │
│   (React)       │    │   (.NET Core)   │    │   (MongoDB)     │
│   Port: 3000    │◄──►│   Port: 5000    │◄──►│   Port: 27017   │
│   nginx         │    │   Swagger       │    │   Admin UI      │
└─────────────────┘    └─────────────────┘    └─────────────────┘
```

## 🔧 Configuración de Entornos

### Variables de Entorno

El sistema está configurado para funcionar automáticamente, pero puedes personalizar:

```yaml
# docker-compose.yml
environment:
  # Frontend
  - REACT_APP_API_URL=http://localhost:5000/api
  
  # Backend
  - ASPNETCORE_ENVIRONMENT=Development
  - MongoDbSettings__ConnectionString=mongodb://admin:password123@mongodb:27017/RealEstateDB?authSource=admin
  - MongoDbSettings__DatabaseName=RealEstateDB
```

## 🚨 Solución de Problemas

### Problema: Puerto ya en uso

```bash
# Ver qué proceso usa el puerto
netstat -ano | findstr :3000
netstat -ano | findstr :5000

# Cambiar puertos en docker-compose.dev.yml si es necesario
```

### Problema: Contenedor no inicia

```bash
# Ver logs detallados
docker logs <container-name> --details

# Reconstruir desde cero
docker-compose down -v
docker system prune -a
docker-compose up --build
```

### Problema: Base de datos vacía

```bash
# Ejecutar script de inicialización manualmente
docker cp init-mongo.js realestate-mongodb:/tmp/
docker exec realestate-mongodb mongosh --username admin --password password123 --authenticationDatabase admin --eval "load('/tmp/init-mongo.js')"
```

## 📝 Notas para Clientes

1. **Instalación Mínima**: Solo necesitas Docker Desktop
2. **Un Solo Comando**: `docker-compose up --build -d`
3. **Acceso Inmediato**: Ve a <http://localhost:3000> para usar la aplicación
4. **Datos de Prueba**: La aplicación incluye datos de ejemplo
5. **Limpieza Fácil**: `docker-compose down -v` elimina todo

## 🔄 Actualización de la Aplicación

```bash
# Obtener últimos cambios
git pull origin main

# Reconstruir y actualizar
docker-compose up --build -d
```

---

**¿Necesitas ayuda?** Contacta al equipo de desarrollo o revisa los logs con `docker-compose logs`.
