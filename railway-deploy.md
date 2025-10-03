# 🚀 Despliegue en Railway

## Pasos para Desplegar en Railway

### 1. Preparar el Proyecto

1. **Crear archivo railway.json en la raíz:**

```json
{
  "build": {
    "builder": "NIXPACKS"
  },
  "deploy": {
    "startCommand": "docker-compose up",
    "healthcheckPath": "/",
    "healthcheckTimeout": 100,
    "restartPolicyType": "ON_FAILURE",
    "restartPolicyMaxRetries": 10
  }
}
```

2. **Modificar docker-compose.yml para producción:**

```yaml
version: '3.8'
services:
  mongodb:
    image: mongo:7
    restart: always
    environment:
      MONGO_INITDB_ROOT_USERNAME: admin
      MONGO_INITDB_ROOT_PASSWORD: ${MONGO_PASSWORD}
      MONGO_INITDB_DATABASE: RealEstateDB
    volumes:
      - mongodb_data:/data/db
      - ./init-mongo.js:/docker-entrypoint-initdb.d/init-mongo.js:ro

  api:
    build:
      context: .
      dockerfile: src/RealEstate.WebApi/Dockerfile
    restart: always
    environment:
      - ASPNETCORE_ENVIRONMENT=Production
      - MongoDbSettings__ConnectionString=mongodb://admin:${MONGO_PASSWORD}@mongodb:27017/RealEstateDB?authSource=admin
      - MongoDbSettings__DatabaseName=RealEstateDB
    depends_on:
      - mongodb

  frontend:
    build:
      context: ./client
      dockerfile: Dockerfile
    restart: always
    environment:
      - REACT_APP_API_URL=${RAILWAY_PUBLIC_DOMAIN}/api
    depends_on:
      - api

volumes:
  mongodb_data:
```

### 2. Desplegar en Railway

1. **Ir a [railway.app](https://railway.app)**
2. **Conectar con GitHub**
3. **Seleccionar tu repositorio**
4. **Railway detectará automáticamente el docker-compose.yml**
5. **Configurar variables de entorno:**
   - `MONGO_PASSWORD`: contraseña segura para MongoDB
6. **Deploy automático**

### 3. Resultado

- ✅ URL pública automática (ej: `https://tu-app.railway.app`)
- ✅ Base de datos persistente
- ✅ SSL automático
- ✅ Escalado automático
