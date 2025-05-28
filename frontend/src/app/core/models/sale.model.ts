import { Client } from './client.model';
import { Vehicle } from './vehicle.model';

export interface Sale {
    id: number;
    cliente?: Client;
    vehiculo?: Vehicle;
    fecha: Date;
    total: number;
}
