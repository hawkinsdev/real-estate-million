export interface PropertySimple {
  idOwner: string;
  name: string;
  address: string;
  price: number;
  image: string;
}

export interface Property {
  idProperty: string;
  name: string;
  address: string;
  price: number;
  codeInternal: string;
  year: number;
  idOwner: string;
  createdAt: string;
  updatedAt: string;
  owner?: Owner;
  images: PropertyImage[];
  propertyTraces: PropertyTrace[];
}

export interface Owner {
  idOwner: string;
  name: string;
  address: string;
  photo: string;
  birthday: string;
  createdAt: string;
  updatedAt: string;
}

export interface PropertyImage {
  idPropertyImage: string;
  idProperty: string;
  file: string;
  enabled: boolean;
  createdAt: string;
  updatedAt: string;
}

export interface PropertyTrace {
  idPropertyTrace: string;
  dateSale: string;
  name: string;
  value: number;
  tax: number;
  idProperty: string;
  createdAt: string;
  updatedAt: string;
}

export interface PropertyFilter {
  name?: string;
  address?: string;
  minPrice?: number;
  maxPrice?: number;
  year?: number;
  idOwner?: string;
  codeInternal?: string;
}
