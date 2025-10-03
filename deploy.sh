#!/bin/bash

echo "========================================"
echo "  Real Estate App - Despliegue Rapido"
echo "========================================"
echo

echo "[1/4] Deteniendo contenedores existentes..."
docker-compose down

echo
echo "[2/4] Construyendo y ejecutando la aplicacion..."
docker-compose up --build -d

echo
echo "[3/4] Esperando a que los servicios esten listos..."
sleep 30

echo
echo "[4/4] Verificando estado de los servicios..."
docker-compose ps

echo
echo "========================================"
echo "  Aplicacion desplegada exitosamente!"
echo "========================================"
echo
echo "Frontend: http://localhost:3000"
echo "Backend API: http://localhost:5000"
echo "MongoDB: localhost:27017"
echo
echo "Para detener la aplicacion: docker-compose down"
echo "Para ver logs: docker-compose logs -f"
echo
