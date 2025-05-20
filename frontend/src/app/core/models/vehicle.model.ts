export class Vehicle {
  id: number;
  marca: string | number;
  modelo: string | number;
  anio: number;
  precio: string;
  stock: number;

  constructor(
    id: number,
    marca: string,
    modelo: string,
    anio: number,
    precio: string,
    stock: number
  ) {
    this.id = id;
    this.marca = marca;
    this.modelo = modelo;
    this.anio = anio;
    this.precio = precio;
    this.stock = stock;
  }
}
