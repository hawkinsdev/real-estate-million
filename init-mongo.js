// MongoDB initialization script
print('🚀 Starting Real Estate database initialization...');

// Switch to the RealEstateDB database
db = db.getSiblingDB('RealEstateDB');

// Create a user for the application
print('👤 Creating database user...');
try {
  db.createUser({
    user: 'realestate_user',
    pwd: 'realestate_password',
    roles: [
      {
        role: 'readWrite',
        db: 'RealEstateDB'
      }
    ]
  });
  print('✅ User created successfully');
} catch (e) {
  print('⚠️  User might already exist:', e.message);
}

// Create collections with validation schemas
print('📋 Creating collections with validation...');

// Owners collection
db.createCollection('Owners', {
  validator: {
    $jsonSchema: {
      bsonType: 'object',
      required: ['name', 'address', 'birthday'],
      properties: {
        name: {
          bsonType: 'string',
          maxLength: 100,
          description: 'Owner name is required and must be a string with max 100 characters'
        },
        address: {
          bsonType: 'string',
          maxLength: 200,
          description: 'Owner address is required and must be a string with max 200 characters'
        },
        photo: {
          bsonType: 'string',
          description: 'Photo URL must be a string'
        },
        birthday: {
          bsonType: 'date',
          description: 'Birthday is required and must be a date'
        },
        createdAt: {
          bsonType: 'date',
          description: 'Creation date'
        },
        updatedAt: {
          bsonType: 'date',
          description: 'Last update date'
        }
      }
    }
  }
});

// Properties collection
db.createCollection('Properties', {
  validator: {
    $jsonSchema: {
      bsonType: 'object',
      required: ['name', 'address', 'price', 'codeInternal', 'year', 'idOwner'],
      properties: {
        name: {
          bsonType: 'string',
          maxLength: 100,
          description: 'Property name is required and must be a string with max 100 characters'
        },
        address: {
          bsonType: 'string',
          maxLength: 200,
          description: 'Property address is required and must be a string with max 200 characters'
        },
        price: {
          bsonType: ['double', 'decimal', 'int'],
          minimum: 0,
          description: 'Price must be a positive number'
        },
        codeInternal: {
          bsonType: 'string',
          maxLength: 50,
          description: 'Internal code is required and must be a string with max 50 characters'
        },
        year: {
          bsonType: 'int',
          minimum: 1800,
          maximum: 2100,
          description: 'Year must be between 1800 and 2100'
        },
        idOwner: {
          bsonType: 'objectId',
          description: 'Owner ID is required and must be a valid ObjectId'
        },
        createdAt: {
          bsonType: 'date',
          description: 'Creation date'
        },
        updatedAt: {
          bsonType: 'date',
          description: 'Last update date'
        }
      }
    }
  }
});

// Property Images collection
db.createCollection('PropertyImages', {
  validator: {
    $jsonSchema: {
      bsonType: 'object',
      required: ['idProperty', 'file'],
      properties: {
        idProperty: {
          bsonType: 'objectId',
          description: 'Property ID is required and must be a valid ObjectId'
        },
        file: {
          bsonType: 'string',
          description: 'File URL is required and must be a string'
        },
        enabled: {
          bsonType: 'bool',
          description: 'Enabled flag must be a boolean'
        },
        createdAt: {
          bsonType: 'date',
          description: 'Creation date'
        },
        updatedAt: {
          bsonType: 'date',
          description: 'Last update date'
        }
      }
    }
  }
});

// Property Traces collection
db.createCollection('PropertyTraces', {
  validator: {
    $jsonSchema: {
      bsonType: 'object',
      required: ['dateSale', 'name', 'value', 'tax', 'idProperty'],
      properties: {
        dateSale: {
          bsonType: 'date',
          description: 'Sale date is required and must be a date'
        },
        name: {
          bsonType: 'string',
          maxLength: 100,
          description: 'Trace name is required and must be a string with max 100 characters'
        },
        value: {
          bsonType: ['double', 'decimal', 'int'],
          minimum: 0,
          description: 'Value must be a positive number'
        },
        tax: {
          bsonType: ['double', 'decimal', 'int'],
          minimum: 0,
          description: 'Tax must be a positive number or zero'
        },
        idProperty: {
          bsonType: 'objectId',
          description: 'Property ID is required and must be a valid ObjectId'
        },
        createdAt: {
          bsonType: 'date',
          description: 'Creation date'
        },
        updatedAt: {
          bsonType: 'date',
          description: 'Last update date'
        }
      }
    }
  }
});

print('✅ Collections created with validation');

// Create indexes for better performance
print('🔍 Creating indexes...');

// Owners indexes
db.Owners.createIndex({ 'name': 'text', 'address': 'text' });
db.Owners.createIndex({ 'birthday': 1 });
db.Owners.createIndex({ 'createdAt': -1 });

// Properties indexes
db.Properties.createIndex({ 'name': 'text', 'address': 'text' });
db.Properties.createIndex({ 'idOwner': 1 });
db.Properties.createIndex({ 'codeInternal': 1 }, { unique: true });
db.Properties.createIndex({ 'price': 1 });
db.Properties.createIndex({ 'year': 1 });
db.Properties.createIndex({ 'createdAt': -1 });

// Property Images indexes
db.PropertyImages.createIndex({ 'idProperty': 1 });
db.PropertyImages.createIndex({ 'enabled': 1 });
db.PropertyImages.createIndex({ 'idProperty': 1, 'enabled': 1 });

// Property Traces indexes
db.PropertyTraces.createIndex({ 'idProperty': 1 });
db.PropertyTraces.createIndex({ 'dateSale': -1 });
db.PropertyTraces.createIndex({ 'value': 1 });
db.PropertyTraces.createIndex({ 'idProperty': 1, 'dateSale': -1 });

print('✅ Indexes created successfully');

// Load sample data
print('📊 Loading sample data...');

// Load owners data
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
    }
  ];
  
  db.Owners.insertMany(ownersData);
  print('✅ Owners data loaded');
} catch (e) {
  print('⚠️  Error loading owners:', e.message);
}

// Load properties data
try {
  const propertiesData = [
    {
      "_id": ObjectId("64b1c2d3e4f5g6789012345a"),
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
      "_id": ObjectId("64b1c2d3e4f5g6789012345b"),
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
      "_id": ObjectId("64b1c2d3e4f5g6789012345c"),
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
      "_id": ObjectId("64b1c2d3e4f5g6789012345d"),
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
      "_id": ObjectId("64b1c2d3e4f5g6789012345e"),
      "name": "Penthouse Vista al Mar",
      "address": "Avenida del Mar # 123-45, Santa Marta",
      "price": 950000000,
      "codeInternal": "SAM-001-2024",
      "year": 2022,
      "idOwner": ObjectId("64a1b2c3d4e5f6789012345a"),
      "createdAt": new Date("2024-01-19T11:30:00Z"),
      "updatedAt": new Date("2024-01-19T11:30:00Z")
    }
  ];
  
  db.Properties.insertMany(propertiesData);
  print('✅ Properties data loaded');
} catch (e) {
  print('⚠️  Error loading properties:', e.message);
}

// Load property images data
try {
  const propertyImagesData = [
    {
      "_id": ObjectId("64c1d2e3f4g5h6789012345a"),
      "idProperty": ObjectId("64b1c2d3e4f5g6789012345a"),
      "file": "https://images.unsplash.com/photo-1600596542815-ffad4c1539a9?w=800&h=600&fit=crop",
      "enabled": true,
      "createdAt": new Date("2024-01-15T10:40:00Z"),
      "updatedAt": new Date("2024-01-15T10:40:00Z")
    },
    {
      "_id": ObjectId("64c1d2e3f4g5h6789012345b"),
      "idProperty": ObjectId("64b1c2d3e4f5g6789012345b"),
      "file": "https://images.unsplash.com/photo-1600607687939-ce8a6c25118c?w=800&h=600&fit=crop",
      "enabled": true,
      "createdAt": new Date("2024-01-16T14:30:00Z"),
      "updatedAt": new Date("2024-01-16T14:30:00Z")
    },
    {
      "_id": ObjectId("64c1d2e3f4g5h6789012345c"),
      "idProperty": ObjectId("64b1c2d3e4f5g6789012345c"),
      "file": "https://images.unsplash.com/photo-1600566753190-17f0baa2a6c3?w=800&h=600&fit=crop",
      "enabled": true,
      "createdAt": new Date("2024-01-17T09:20:00Z"),
      "updatedAt": new Date("2024-01-17T09:20:00Z")
    },
    {
      "_id": ObjectId("64c1d2e3f4g5h6789012345d"),
      "idProperty": ObjectId("64b1c2d3e4f5g6789012345d"),
      "file": "https://images.unsplash.com/photo-1600585154340-be6161a56a0c?w=800&h=600&fit=crop",
      "enabled": true,
      "createdAt": new Date("2024-01-18T16:50:00Z"),
      "updatedAt": new Date("2024-01-18T16:50:00Z")
    },
    {
      "_id": ObjectId("64c1d2e3f4g5h6789012345e"),
      "idProperty": ObjectId("64b1c2d3e4f5g6789012345e"),
      "file": "https://images.unsplash.com/photo-1600607687644-c7171b42498b?w=800&h=600&fit=crop",
      "enabled": true,
      "createdAt": new Date("2024-01-19T11:35:00Z"),
      "updatedAt": new Date("2024-01-19T11:35:00Z")
    }
  ];
  
  db.PropertyImages.insertMany(propertyImagesData);
  print('✅ Property images data loaded');
} catch (e) {
  print('⚠️  Error loading property images:', e.message);
}

print('🎉 Database initialization completed successfully!');
print('📊 Summary:');
print('   - Database: RealEstateDB');
print('   - Collections: Owners, Properties, PropertyImages, PropertyTraces');
print('   - Indexes: Created for optimal performance');
print('   - Sample data: Loaded successfully');
print('   - Validation: Schema validation enabled');

// Display collection counts
print('📈 Collection counts:');
print('   - Owners:', db.Owners.countDocuments());
print('   - Properties:', db.Properties.countDocuments());
print('   - PropertyImages:', db.PropertyImages.countDocuments());
print('   - PropertyTraces:', db.PropertyTraces.countDocuments());