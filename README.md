# 🏠 Real Estate Application

Aplicación completa de bienes raíces desarrollada con .NET 8, MongoDB y React. Esta aplicación permite gestionar propiedades inmobiliarias con funcionalidades avanzadas de búsqueda y filtrado.

## 📋 Descripción del Proyecto

Esta es una aplicación full-stack que incluye:

- **Backend API**: Desarrollado con .NET 8, C# y MongoDB
- **Frontend Web**: Desarrollado con React, TypeScript y Material-UI
- **Base de Datos**: MongoDB con datos de prueba
- **Arquitectura**: Clean Architecture con separación de responsabilidades

## 🚀 Características Principales

### Backend (API)

- ✅ API REST completa para gestión de propiedades
- ✅ Filtros avanzados (nombre, dirección, rango de precios)
- ✅ DTOs específicos para la prueba técnica
- ✅ Validación con FluentValidation
- ✅ Mapeo automático con AutoMapper
- ✅ Logging y manejo de errores
- ✅ Documentación con Swagger
- ✅ Pruebas unitarias con NUnit

### Frontend (React)

- ✅ Listado de propiedades con diseño responsive
- ✅ Filtros de búsqueda en tiempo real
- ✅ Paginación automática
- ✅ Vista detallada de propiedades
- ✅ Interfaz moderna con Material-UI
- ✅ Optimización con React Query
- ✅ Tipado fuerte con TypeScript

## 🛠️ Tecnologías Utilizadas

### Backend

- **.NET 8** - Framework principal
- **C#** - Lenguaje de programación
- **MongoDB** - Base de datos NoSQL
- **AutoMapper** - Mapeo de objetos
- **FluentValidation** - Validación de datos
- **Swagger** - Documentación de API
- **NUnit** - Framework de pruebas

### Frontend

- **React 18** - Biblioteca de interfaz
- **TypeScript** - Tipado estático
- **Material-UI** - Componentes de interfaz
- **React Query** - Manejo de estado del servidor
- **React Router** - Navegación
- **Axios** - Cliente HTTP

## 📁 Estructura del Proyecto

```
real-estate-million/
├── src/
│   ├── RealEstate.Domain/          # Entidades y interfaces de dominio
│   ├── RealEstate.Application/     # Lógica de aplicación y servicios
│   ├── RealEstate.Infrastructure/  # Implementación de repositorios
│   └── RealEstate.WebApi/          # API REST y controladores
├── client/                         # Frontend React
├── tests/                          # Pruebas unitarias
├── docker-compose.yml              # Configuración de Docker
├── init-mongo.js                   # Script de inicialización de MongoDB
└── README.md                       # Este archivo
```

## 🚀 Instalación y Configuración

### 🐳 Docker (Recomendado para Evaluación)

**¡La forma más fácil! Todo funciona con un solo comando:**

```bash
# 1. Clonar el repositorio
git clone <repository-url>
cd real-estate-million

# 2. Ejecutar toda la aplicación (DB + Backend + Frontend)
docker-compose up --build -d

# 3. ¡Listo! Accede a:
# - 🌐 Frontend: http://localhost:3000
# - 🔧 Backend API: http://localhost:5000  
# - 📚 Swagger: http://localhost:5000/swagger
# - 🗄️ MongoDB: localhost:27017
```

**📖 Documentación completa de Docker**: [DOCKER_SETUP.md](./DOCKER_SETUP.md)

---

### 💻 Opción 2: Desarrollo Local

#### Prerrequisitos

- .NET 8 SDK
- Node.js 18+
- MongoDB 6+
- Docker (opcional)

### 1. Clonar el Repositorio

```bash
git clone <repository-url>
cd real-estate-million
```

### 2. Configurar la Base de Datos

#### Opción A: Con Docker (Recomendado)

```bash
docker-compose up -d
```

#### Opción B: MongoDB Local

1. Instalar MongoDB
2. Ejecutar el script de inicialización:

```bash
mongosh < init-mongo.js
```

### 3. Configurar el Backend

```bash
cd src/RealEstate.WebApi
dotnet restore
dotnet run
```

La API estará disponible en `https://localhost:7000` o `http://localhost:5000`

### 4. Configurar el Frontend

```bash
cd client
npm install
npm start
```

El frontend estará disponible en `http://localhost:3000`

## 📊 Datos de Prueba

El proyecto incluye datos de prueba que se cargan automáticamente:

- **7 Propiedades** con diferentes características
- **3 Propietarios** con información completa
- **7 Imágenes** de propiedades
- **3 Trazas** de transacciones
- **Datos realistas** de Colombia

### 🔄 Carga Automática de Datos

Los datos se cargan automáticamente cuando ejecutas Docker Compose. Si necesitas cargar los datos manualmente:

#### Opción 1: Scripts Automatizados

**En Linux/macOS:**

```bash
# Hacer el script ejecutable
chmod +x scripts/setup-database.sh

# Ejecutar el script
./scripts/setup-database.sh
```

**En Windows (PowerShell):**

```powershell
# Ejecutar el script de PowerShell
.\scripts\setup-database.ps1
```

#### Opción 2: Carga Manual

```bash
# Copiar el script al contenedor de MongoDB
docker cp scripts/load-data.js realestate-mongodb:/tmp/load-data.js

# Ejecutar el script de carga
docker exec realestate-mongodb mongosh --file /tmp/load-data.js
```

#### Opción 3: Verificar Estado de la Base de Datos

```bash
# Verificar si los datos están cargados
docker exec realestate-mongodb mongosh --eval "
  db = db.getSiblingDB('RealEstateDB');
  print('Propietarios:', db.Owners.countDocuments());
  print('Propiedades:', db.Properties.countDocuments());
  print('Imágenes:', db.PropertyImages.countDocuments());
  print('Trazas:', db.PropertyTraces.countDocuments());
"
```

## 🔧 Endpoints de la API

### Propiedades

- `GET /api/property/simple` - Listado de propiedades (formato simple)
- `GET /api/property/search/simple` - Búsqueda con filtros (formato simple)
- `GET /api/property/{id}` - Detalles de una propiedad
- `POST /api/property` - Crear nueva propiedad
- `PUT /api/property/{id}` - Actualizar propiedad
- `DELETE /api/property/{id}` - Eliminar propiedad

### Filtros Disponibles

- `name` - Búsqueda por nombre (coincidencia parcial)
- `address` - Búsqueda por dirección (coincidencia parcial)
- `minPrice` - Precio mínimo
- `maxPrice` - Precio máximo

### Ejemplo de Uso

```bash
# Obtener todas las propiedades
GET /api/property/simple

# Buscar propiedades con filtros
GET /api/property/search/simple?name=Casa&minPrice=100000&maxPrice=500000
```

## 🧪 Pruebas

### Ejecutar Pruebas del Backend

```bash
cd tests/RealEstate.Application.Tests
dotnet test

cd ../RealEstate.WebApi.Tests
dotnet test
```

### Ejecutar Pruebas del Frontend

```bash
cd client
npm test
```

## 🔧 Solución de Problemas

### Problema: La Base de Datos No Tiene Datos

Si después de ejecutar Docker Compose la base de datos no tiene datos, puedes solucionarlo de las siguientes maneras:

#### Solución 1: Reiniciar el Servicio de Configuración

```bash
# Detener todos los servicios
docker-compose down

# Eliminar el contenedor de configuración (si existe)
docker rm realestate-db-setup 2>/dev/null || true

# Volver a ejecutar con los cambios actualizados
docker-compose up --build
```

#### Solución 2: Ejecutar Manualmente el Script de Carga

```bash
# Asegúrate de que MongoDB esté ejecutándose
docker-compose up -d mongodb

# Espera unos segundos y ejecuta el script
./scripts/setup-database.sh
# O en Windows: .\scripts\setup-database.ps1
```

#### Solución 3: Limpiar y Reiniciar Completamente

```bash
# Detener y eliminar todo
docker-compose down -v

# Eliminar imágenes (opcional)
docker rmi $(docker images "realestate*" -q)

# Reconstruir desde cero
docker-compose up --build
```

### Problema: Error de Conexión a MongoDB

Si ves errores de conexión a MongoDB:

1. **Verificar que MongoDB esté ejecutándose:**

   ```bash
   docker ps | grep mongodb
   ```

2. **Verificar los logs de MongoDB:**

   ```bash
   docker logs realestate-mongodb
   ```

3. **Probar conexión manual:**

   ```bash
   docker exec -it realestate-mongodb mongosh
   ```

### Problema: El Frontend No Se Conecta al Backend

1. **Verificar que el backend esté ejecutándose:**

   ```bash
   curl http://localhost:5000/api/property/simple
   ```

2. **Verificar los logs del backend:**

   ```bash
   docker logs realestate-api-dev
   ```

3. **Verificar los logs del frontend:**

   ```bash
   docker logs realestate-frontend-dev
   ```

### Problema: Error de Nginx "host not found in upstream"

Si ves errores como `nginx: [emerg] host not found in upstream "realestate-api"`:

1. **Ejecutar el comando correcto:**

   ```bash
   # Comando estándar para la aplicación:
   docker-compose up --build
   ```

2. **Reconstruir el frontend:**

   ```bash
   docker-compose build frontend --no-cache
   docker-compose up frontend
   ```

3. **Verificar la configuración de red:**

   ```bash
   docker network inspect realestate-network
   ```

4. **Verificar la configuración de CORS en el backend**

## 📱 Funcionalidades del Frontend

### Listado de Propiedades

- Grid responsive con tarjetas de propiedades
- Paginación automática
- Carga de imágenes con fallback
- Indicadores de estado

### Filtros de Búsqueda

- Búsqueda por texto en nombre y dirección
- Filtro por rango de precios con slider interactivo
- Limpieza de filtros
- Búsqueda en tiempo real

### Vista de Detalles

- Información completa de la propiedad
- Datos del propietario
- Galería de imágenes
- Historial de transacciones

## 🎨 Diseño y UX

- **Responsive Design**: Optimizado para móviles, tablets y desktop
- **Material Design**: Componentes consistentes y modernos
- **Accesibilidad**: Cumple estándares de accesibilidad web
- **Performance**: Optimizado con React Query y lazy loading
- **UX Intuitiva**: Navegación clara y feedback visual

## 📈 Rendimiento

### Backend

- Consultas optimizadas con índices de MongoDB
- Caché de consultas frecuentes
- Paginación en base de datos
- Compresión de respuestas

### Frontend

- Lazy loading de componentes
- Caché inteligente con React Query
- Optimización de imágenes
- Bundle splitting

## 🔒 Seguridad

- Validación de entrada en backend y frontend
- Sanitización de datos
- Headers de seguridad
- CORS configurado correctamente
- Manejo seguro de errores

## 📚 Documentación

- **API**: Documentación automática con Swagger
- **Código**: Comentarios XML en métodos públicos
- **README**: Documentación completa del proyecto
- **Tipos**: Documentación de tipos TypeScript

## 🤝 Contribución

1. Fork el proyecto
2. Crea una rama para tu feature (`git checkout -b feature/AmazingFeature`)
3. Commit tus cambios (`git commit -m 'Add some AmazingFeature'`)
4. Push a la rama (`git push origin feature/AmazingFeature`)
5. Abre un Pull Request

## 📄 Licencia

Este proyecto está bajo la Licencia MIT. Ver el archivo `LICENSE` para más detalles.

## 👥 Contacto

Para preguntas sobre este proyecto, contacta a:

- <crios@millionluxury.com>
- <amorau@millionluxury.com>

## 🎯 Criterios de Evaluación Cumplidos

### ✅ Arquitectura

- Clean Architecture implementada
- Separación clara de responsabilidades
- Patrones de diseño aplicados

### ✅ Código

- Código limpio y mantenible
- Convenciones de nomenclatura
- Documentación adecuada

### ✅ Performance

- Consultas optimizadas
- Caché implementado
- Paginación eficiente

### ✅ Testing

- Pruebas unitarias para backend
- Cobertura de casos principales
- Mocks y stubs apropiados

### ✅ UX/UI

- Diseño responsive
- Interfaz intuitiva
- Feedback visual adecuado

---

**¡Gracias por revisar este proyecto! 🚀**
