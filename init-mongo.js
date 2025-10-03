// Script de inicialización de MongoDB
// Este script se ejecuta cuando MongoDB se inicia por primera vez

print('🚀 Iniciando configuración inicial de MongoDB...');

// Crear usuario administrador
db = db.getSiblingDB('admin');

// Verificar si el usuario ya existe
const existingUser = db.getUser('admin');
if (!existingUser) {
    db.createUser({
        user: 'admin',
        pwd: 'password123',
        roles: [
            { role: 'userAdminAnyDatabase', db: 'admin' },
            { role: 'readWriteAnyDatabase', db: 'admin' },
            { role: 'dbAdminAnyDatabase', db: 'admin' }
        ]
    });
    print('✅ Usuario administrador creado exitosamente');
} else {
    print('ℹ️  Usuario administrador ya existe');
}

// Cambiar a la base de datos de la aplicación
db = db.getSiblingDB('RealEstateDB');

// Crear usuario específico para la aplicación
const appUser = db.getUser('realestate_user');
if (!appUser) {
    db.createUser({
        user: 'realestate_user',
        pwd: 'realestate_pass',
        roles: [
            { role: 'readWrite', db: 'RealEstateDB' }
        ]
    });
    print('✅ Usuario de aplicación creado exitosamente');
} else {
    print('ℹ️  Usuario de aplicación ya existe');
}

// Crear colecciones con validación
print('📊 Creando colecciones...');

// Colección de Propietarios
if (!db.getCollectionNames().includes('Owners')) {
    db.createCollection('Owners', {
        validator: {
            $jsonSchema: {
                bsonType: 'object',
                required: ['name', 'address'],
                properties: {
                    name: {
                        bsonType: 'string',
                        description: 'Nombre del propietario es requerido'
                    },
                    address: {
                        bsonType: 'string',
                        description: 'Dirección del propietario es requerida'
                    },
                    photo: {
                        bsonType: 'string',
                        description: 'URL de la foto del propietario'
                    },
                    birthday: {
                        bsonType: 'date',
                        description: 'Fecha de nacimiento del propietario'
                    }
                }
            }
        }
    });
    print('✅ Colección Owners creada');
}

// Colección de Propiedades
if (!db.getCollectionNames().includes('Properties')) {
    db.createCollection('Properties', {
        validator: {
            $jsonSchema: {
                bsonType: 'object',
                required: ['name', 'address', 'price', 'codeInternal', 'year', 'idOwner'],
                properties: {
                    name: {
                        bsonType: 'string',
                        description: 'Nombre de la propiedad es requerido'
                    },
                    address: {
                        bsonType: 'string',
                        description: 'Dirección de la propiedad es requerida'
                    },
                    price: {
                        bsonType: 'number',
                        minimum: 0,
                        description: 'Precio debe ser un número positivo'
                    },
                    codeInternal: {
                        bsonType: 'string',
                        description: 'Código interno es requerido'
                    },
                    year: {
                        bsonType: 'int',
                        minimum: 1900,
                        maximum: 2100,
                        description: 'Año debe estar entre 1900 y 2100'
                    },
                    idOwner: {
                        bsonType: 'objectId',
                        description: 'ID del propietario es requerido'
                    }
                }
            }
        }
    });
    print('✅ Colección Properties creada');
}

// Colección de Imágenes de Propiedades
if (!db.getCollectionNames().includes('PropertyImages')) {
    db.createCollection('PropertyImages', {
        validator: {
            $jsonSchema: {
                bsonType: 'object',
                required: ['idProperty', 'file', 'enabled'],
                properties: {
                    idProperty: {
                        bsonType: 'objectId',
                        description: 'ID de la propiedad es requerido'
                    },
                    file: {
                        bsonType: 'string',
                        description: 'URL del archivo es requerida'
                    },
                    enabled: {
                        bsonType: 'bool',
                        description: 'Estado habilitado es requerido'
                    }
                }
            }
        }
    });
    print('✅ Colección PropertyImages creada');
}

// Colección de Trazas de Propiedades
if (!db.getCollectionNames().includes('PropertyTraces')) {
    db.createCollection('PropertyTraces', {
        validator: {
            $jsonSchema: {
                bsonType: 'object',
                required: ['dateSale', 'name', 'value', 'tax', 'idProperty'],
                properties: {
                    dateSale: {
                        bsonType: 'date',
                        description: 'Fecha de venta es requerida'
                    },
                    name: {
                        bsonType: 'string',
                        description: 'Nombre de la transacción es requerido'
                    },
                    value: {
                        bsonType: 'number',
                        minimum: 0,
                        description: 'Valor debe ser un número positivo'
                    },
                    tax: {
                        bsonType: 'number',
                        minimum: 0,
                        description: 'Impuesto debe ser un número positivo'
                    },
                    idProperty: {
                        bsonType: 'objectId',
                        description: 'ID de la propiedad es requerido'
                    }
                }
            }
        }
    });
    print('✅ Colección PropertyTraces creada');
}

// Crear índices para mejorar el rendimiento
print('🔍 Creando índices...');

// Índices para Owners
db.Owners.createIndex({ "name": 1 });
db.Owners.createIndex({ "createdAt": -1 });

// Índices para Properties
db.Properties.createIndex({ "name": 1 });
db.Properties.createIndex({ "codeInternal": 1 }, { unique: true });
db.Properties.createIndex({ "idOwner": 1 });
db.Properties.createIndex({ "price": 1 });
db.Properties.createIndex({ "year": 1 });
db.Properties.createIndex({ "createdAt": -1 });

// Índices para PropertyImages
db.PropertyImages.createIndex({ "idProperty": 1 });
db.PropertyImages.createIndex({ "enabled": 1 });

// Índices para PropertyTraces
db.PropertyTraces.createIndex({ "idProperty": 1 });
db.PropertyTraces.createIndex({ "dateSale": -1 });

print('✅ Índices creados exitosamente');

print('🎉 Configuración inicial de MongoDB completada!');
print('📊 Resumen:');
print('   - Base de datos: RealEstateDB');
print('   - Colecciones: Owners, Properties, PropertyImages, PropertyTraces');
print('   - Usuarios: admin, realestate_user');
print('   - Índices: Creados para optimizar consultas');
print('✨ MongoDB está listo para usar!');
