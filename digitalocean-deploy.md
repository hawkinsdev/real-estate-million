# 🚀 Despliegue en DigitalOcean App Platform

## Pasos para Desplegar en DigitalOcean

### 1. Preparar el Proyecto

1. **Crear .do/app.yaml:**

```yaml
name: real-estate-app
services:
- name: api
  source_dir: /src/RealEstate.WebApi
  github:
    repo: tu-usuario/real-estate-million
    branch: main
  run_command: dotnet run --urls=http://0.0.0.0:$PORT
  environment_slug: dotnet
  instance_count: 1
  instance_size_slug: basic-xxs
  envs:
  - key: ASPNETCORE_ENVIRONMENT
    value: Production
  - key: MongoDbSettings__ConnectionString
    value: ${db.CONNECTIONSTRING}
  http_port: 8080

- name: frontend
  source_dir: /client
  github:
    repo: tu-usuario/real-estate-million
    branch: main
  build_command: npm run build
  run_command: npx serve -s build -l $PORT
  environment_slug: node-js
  instance_count: 1
  instance_size_slug: basic-xxs
  envs:
  - key: REACT_APP_API_URL
    value: ${api.PUBLIC_URL}/api

databases:
- name: db
  engine: MONGODB
  version: "5"
  size: db-s-1vcpu-1gb
```

### 2. Desplegar en DigitalOcean

1. **Ir a [cloud.digitalocean.com](https://cloud.digitalocean.com)**
2. **Crear cuenta (tienen $200 de crédito gratis)**
3. **App Platform > Create App**
4. **Conectar GitHub**
5. **Seleccionar repositorio**
6. **DigitalOcean detectará el .do/app.yaml**
7. **Deploy**

### 3. Resultado

- ✅ URL pública (ej: `https://tu-app.ondigitalocean.app`)
- ✅ Base de datos MongoDB gestionada
- ✅ SSL automático
- ✅ Escalado automático
- ✅ $200 de crédito gratis (suficiente para meses)
