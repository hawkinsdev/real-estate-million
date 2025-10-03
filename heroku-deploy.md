# 🚀 Despliegue en Heroku

## Pasos para Desplegar en Heroku

### 1. Preparar el Proyecto

1. **Crear Procfile en la raíz:**

```
web: cd src/RealEstate.WebApi && dotnet run --urls=http://0.0.0.0:$PORT
```

2. **Crear app.json:**

```json
{
  "name": "Real Estate App",
  "description": "Sistema de gestión inmobiliaria",
  "repository": "https://github.com/tu-usuario/real-estate-million",
  "logo": "",
  "keywords": ["dotnet", "react", "mongodb", "real-estate"],
  "stack": "heroku-20",
  "buildpacks": [
    {
      "url": "https://github.com/jincod/dotnetcore-buildpack"
    }
  ],
  "addons": [
    {
      "plan": "mongolab:sandbox"
    }
  ],
  "env": {
    "ASPNETCORE_ENVIRONMENT": {
      "value": "Production"
    }
  }
}
```

### 2. Desplegar en Heroku

1. **Instalar Heroku CLI**
2. **Login:** `heroku login`
3. **Crear app:** `heroku create tu-app-name`
4. **Agregar MongoDB:** `heroku addons:create mongolab:sandbox`
5. **Deploy:** `git push heroku main`

### 3. Resultado

- ✅ URL pública (ej: `https://tu-app.herokuapp.com`)
- ⚠️ Plan gratuito limitado
- ⚠️ App se duerme después de 30 min de inactividad
