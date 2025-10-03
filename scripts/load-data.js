// Script para cargar datos iniciales en MongoDB
// Este script se ejecuta después de que MongoDB esté completamente inicializado

print('🚀 Iniciando carga de datos para Real Estate DB...');

// Conectar a la base de datos
// Nota: Este script asume que se ejecuta dentro del contexto de Docker
// donde 'mongodb' es el nombre del servicio
db = db.getSiblingDB('RealEstateDB');

// Verificar si ya existen datos
const existingOwners = db.Owners.countDocuments();
const existingProperties = db.Properties.countDocuments();

if (existingOwners > 0 || existingProperties > 0) {
    print('⚠️  La base de datos ya contiene datos. Saltando la carga inicial.');
    print(`   - Propietarios existentes: ${existingOwners}`);
    print(`   - Propiedades existentes: ${existingProperties}`);
    quit();
}

print('📊 Cargando datos iniciales...');

// Cargar datos de propietarios
try {
    const ownersData = [
        {
            "_id": ObjectId("64a1b2c3d4e5f6789012345a"),
            "name": "Juan Carlos Pérez",
            "address": "Calle 123 # 45-67, Bogotá",
            "photo": "https://images.unsplash.com/photo-1507003211169-0a1dd7228f2d?w=300&h=300&fit=crop",
            "birthday": new Date("1985-03-15"),
            "createdAt": new Date("2024-01-15T10:30:00Z"),
            "updatedAt": new Date("2024-01-15T10:30:00Z")
        },
        {
            "_id": ObjectId("64a1b2c3d4e5f6789012345b"),
            "name": "María Elena Rodríguez",
            "address": "Avenida 80 # 123-45, Medellín",
            "photo": "https://images.unsplash.com/photo-1494790108755-2616b612b786?w=300&h=300&fit=crop",
            "birthday": new Date("1978-07-22"),
            "createdAt": new Date("2024-01-16T14:20:00Z"),
            "updatedAt": new Date("2024-01-16T14:20:00Z")
        },
        {
            "_id": ObjectId("64a1b2c3d4e5f6789012345c"),
            "name": "Carlos Alberto Mendoza",
            "address": "Carrera 15 # 85-20, Cali",
            "photo": "https://images.unsplash.com/photo-1472099645785-5658abf4ff4e?w=300&h=300&fit=crop",
            "birthday": new Date("1990-11-08"),
            "createdAt": new Date("2024-01-17T08:15:00Z"),
            "updatedAt": new Date("2024-01-17T08:15:00Z")
        }
    ];
    
    const ownersResult = db.Owners.insertMany(ownersData);
    print(`✅ ${ownersResult.insertedIds.length} propietarios cargados exitosamente`);
} catch (e) {
    print('❌ Error cargando propietarios:', e.message);
}

// Cargar datos de propiedades
try {
    const propertiesData = [
        {
            "_id": ObjectId("64b1c2d3e4f5a6789012345a"),
            "name": "Casa Moderna en Zona Norte",
            "address": "Calle 127 # 15-30, Bogotá",
            "price": 450000000,
            "codeInternal": "BOG-001-2024",
            "year": 2020,
            "idOwner": ObjectId("64a1b2c3d4e5f6789012345a"),
            "createdAt": new Date("2024-01-15T10:35:00Z"),
            "updatedAt": new Date("2024-01-15T10:35:00Z")
        },
        {
            "_id": ObjectId("64b1c2d3e4f5a6789012345b"),
            "name": "Villa Campestre El Poblado",
            "address": "Transversal 5A # 45-67, Medellín",
            "price": 780000000,
            "codeInternal": "MED-001-2024",
            "year": 2019,
            "idOwner": ObjectId("64a1b2c3d4e5f6789012345b"),
            "createdAt": new Date("2024-01-16T14:25:00Z"),
            "updatedAt": new Date("2024-01-16T14:25:00Z")
        },
        {
            "_id": ObjectId("64b1c2d3e4f5a6789012345c"),
            "name": "Apartamento Ejecutivo Centro",
            "address": "Carrera 7 # 32-16, Bogotá",
            "price": 320000000,
            "codeInternal": "BOG-002-2024",
            "year": 2021,
            "idOwner": ObjectId("64a1b2c3d4e5f6789012345a"),
            "createdAt": new Date("2024-01-17T09:15:00Z"),
            "updatedAt": new Date("2024-01-17T09:15:00Z")
        },
        {
            "_id": ObjectId("64b1c2d3e4f5a6789012345d"),
            "name": "Casa Colonial Restaurada",
            "address": "Calle 10 # 5-30, Cartagena",
            "price": 1200000000,
            "codeInternal": "CAR-001-2024",
            "year": 1950,
            "idOwner": ObjectId("64a1b2c3d4e5f6789012345b"),
            "createdAt": new Date("2024-01-18T16:45:00Z"),
            "updatedAt": new Date("2024-01-18T16:45:00Z")
        },
        {
            "_id": ObjectId("64b1c2d3e4f5a6789012345e"),
            "name": "Penthouse Vista al Mar",
            "address": "Avenida del Mar # 123-45, Santa Marta",
            "price": 950000000,
            "codeInternal": "SAM-001-2024",
            "year": 2022,
            "idOwner": ObjectId("64a1b2c3d4e5f6789012345a"),
            "createdAt": new Date("2024-01-19T11:30:00Z"),
            "updatedAt": new Date("2024-01-19T11:30:00Z")
        },
        {
            "_id": ObjectId("64b1c2d3e4f5a6789012345f"),
            "name": "Loft Industrial Zona Rosa",
            "address": "Calle 85 # 12-34, Bogotá",
            "price": 680000000,
            "codeInternal": "BOG-003-2024",
            "year": 2018,
            "idOwner": ObjectId("64a1b2c3d4e5f6789012345c"),
            "createdAt": new Date("2024-01-20T13:45:00Z"),
            "updatedAt": new Date("2024-01-20T13:45:00Z")
        },
        {
            "_id": ObjectId("64b1c2d3e4f5a678901234aa"),
            "name": "Casa Campestre Valle del Cauca",
            "address": "Vereda La Esperanza, Km 5, Cali",
            "price": 550000000,
            "codeInternal": "CAL-001-2024",
            "year": 2017,
            "idOwner": ObjectId("64a1b2c3d4e5f6789012345c"),
            "createdAt": new Date("2024-01-21T10:20:00Z"),
            "updatedAt": new Date("2024-01-21T10:20:00Z")
        }
    ];
    
    const propertiesResult = db.Properties.insertMany(propertiesData);
    print(`✅ ${propertiesResult.insertedIds.length} propiedades cargadas exitosamente`);
} catch (e) {
    print('❌ Error cargando propiedades:', e.message);
}

// Cargar imágenes de propiedades
try {
    const propertyImagesData = [
        {
            "_id": ObjectId("64c1d2e3f4a5b6789012345a"),
            "idProperty": ObjectId("64b1c2d3e4f5a6789012345a"),
            "file": "https://images.unsplash.com/photo-1600596542815-ffad4c1539a9?w=800&h=600&fit=crop",
            "enabled": true,
            "createdAt": new Date("2024-01-15T10:40:00Z"),
            "updatedAt": new Date("2024-01-15T10:40:00Z")
        },
        {
            "_id": ObjectId("64c1d2e3f4a5b6789012345b"),
            "idProperty": ObjectId("64b1c2d3e4f5a6789012345b"),
            "file": "https://images.unsplash.com/photo-1600607687939-ce8a6c25118c?w=800&h=600&fit=crop",
            "enabled": true,
            "createdAt": new Date("2024-01-16T14:30:00Z"),
            "updatedAt": new Date("2024-01-16T14:30:00Z")
        },
        {
            "_id": ObjectId("64c1d2e3f4a5b6789012345c"),
            "idProperty": ObjectId("64b1c2d3e4f5a6789012345c"),
            "file": "https://images.unsplash.com/photo-1600566753190-17f0baa2a6c3?w=800&h=600&fit=crop",
            "enabled": true,
            "createdAt": new Date("2024-01-17T09:20:00Z"),
            "updatedAt": new Date("2024-01-17T09:20:00Z")
        },
        {
            "_id": ObjectId("64c1d2e3f4a5b6789012345d"),
            "idProperty": ObjectId("64b1c2d3e4f5a6789012345d"),
            "file": "https://images.unsplash.com/photo-1600585154340-be6161a56a0c?w=800&h=600&fit=crop",
            "enabled": true,
            "createdAt": new Date("2024-01-18T16:50:00Z"),
            "updatedAt": new Date("2024-01-18T16:50:00Z")
        },
        {
            "_id": ObjectId("64c1d2e3f4a5b6789012345e"),
            "idProperty": ObjectId("64b1c2d3e4f5a6789012345e"),
            "file": "https://images.unsplash.com/photo-1600607687644-c7171b42498b?w=800&h=600&fit=crop",
            "enabled": true,
            "createdAt": new Date("2024-01-19T11:35:00Z"),
            "updatedAt": new Date("2024-01-19T11:35:00Z")
        },
        {
            "_id": ObjectId("64c1d2e3f4a5b6789012345f"),
            "idProperty": ObjectId("64b1c2d3e4f5a6789012345f"),
            "file": "https://images.unsplash.com/photo-1600607687920-4e2a09cf159d?w=800&h=600&fit=crop",
            "enabled": true,
            "createdAt": new Date("2024-01-20T13:50:00Z"),
            "updatedAt": new Date("2024-01-20T13:50:00Z")
        },
        {
            "_id": ObjectId("64c1d2e3f4a5b678901234aa"),
            "idProperty": ObjectId("64b1c2d3e4f5a678901234aa"),
            "file": "https://images.unsplash.com/photo-1600607688969-a5bfcd646154?w=800&h=600&fit=crop",
            "enabled": true,
            "createdAt": new Date("2024-01-21T10:25:00Z"),
            "updatedAt": new Date("2024-01-21T10:25:00Z")
        }
    ];
    
    const imagesResult = db.PropertyImages.insertMany(propertyImagesData);
    print(`✅ ${imagesResult.insertedIds.length} imágenes de propiedades cargadas exitosamente`);
} catch (e) {
    print('❌ Error cargando imágenes:', e.message);
}

// Cargar trazas de propiedades (historial de transacciones)
try {
    const propertyTracesData = [
        {
            "_id": ObjectId("64d1e2f3a4b5c6789012345a"),
            "dateSale": new Date("2024-01-15T10:35:00Z"),
            "name": "Venta inicial",
            "value": 450000000,
            "tax": 22500000,
            "idProperty": ObjectId("64b1c2d3e4f5a6789012345a"),
            "createdAt": new Date("2024-01-15T10:35:00Z"),
            "updatedAt": new Date("2024-01-15T10:35:00Z")
        },
        {
            "_id": ObjectId("64d1e2f3a4b5c6789012345b"),
            "dateSale": new Date("2024-01-16T14:25:00Z"),
            "name": "Venta inicial",
            "value": 780000000,
            "tax": 39000000,
            "idProperty": ObjectId("64b1c2d3e4f5a6789012345b"),
            "createdAt": new Date("2024-01-16T14:25:00Z"),
            "updatedAt": new Date("2024-01-16T14:25:00Z")
        },
        {
            "_id": ObjectId("64d1e2f3a4b5c6789012345c"),
            "dateSale": new Date("2024-01-17T09:15:00Z"),
            "name": "Venta inicial",
            "value": 320000000,
            "tax": 16000000,
            "idProperty": ObjectId("64b1c2d3e4f5a6789012345c"),
            "createdAt": new Date("2024-01-17T09:15:00Z"),
            "updatedAt": new Date("2024-01-17T09:15:00Z")
        }
    ];
    
    const tracesResult = db.PropertyTraces.insertMany(propertyTracesData);
    print(`✅ ${tracesResult.insertedIds.length} trazas de propiedades cargadas exitosamente`);
} catch (e) {
    print('❌ Error cargando trazas:', e.message);
}

// Mostrar resumen final
print('🎉 Carga de datos completada exitosamente!');
print('📊 Resumen de datos cargados:');
print(`   - Propietarios: ${db.Owners.countDocuments()}`);
print(`   - Propiedades: ${db.Properties.countDocuments()}`);
print(`   - Imágenes: ${db.PropertyImages.countDocuments()}`);
print(`   - Trazas: ${db.PropertyTraces.countDocuments()}`);

print('✨ La base de datos está lista para usar!');
