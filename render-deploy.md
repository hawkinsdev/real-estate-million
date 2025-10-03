# 🚀 Despliegue en Render

## Pasos para Desplegar en Render

### 1. Preparar el Proyecto

1. **Crear render.yaml en la raíz:**

```yaml
services:
  - type: web
    name: real-estate-api
    env: docker
    dockerfilePath: ./src/RealEstate.WebApi/Dockerfile
    envVars:
      - key: ASPNETCORE_ENVIRONMENT
        value: Production
      - key: MongoDbSettings__ConnectionString
        fromDatabase:
          name: real-estate-db
          property: connectionString

  - type: web
    name: real-estate-frontend
    env: docker
    dockerfilePath: ./client/Dockerfile
    envVars:
      - key: REACT_APP_API_URL
        value: https://real-estate-api.onrender.com/api

databases:
  - name: real-estate-db
    databaseName: RealEstateDB
    user: admin
```

### 2. Desplegar en Render

1. **Ir a [render.com](https://render.com)**
2. **Conectar con GitHub**
3. **Crear nuevo Web Service**
4. **Seleccionar tu repositorio**
5. **Configurar:**
   - Build Command: `docker build -t api ./src/RealEstate.WebApi`
   - Start Command: `docker run -p 10000:8080 api`
6. **Crear base de datos MongoDB**
7. **Configurar variables de entorno**

### 3. Resultado

- ✅ URL pública (ej: `https://tu-app.onrender.com`)
- ✅ Base de datos MongoDB gestionada
- ✅ SSL automático
