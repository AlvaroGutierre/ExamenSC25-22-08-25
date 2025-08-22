import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Pelicula {
  id: number;
  titulo: string;
  director: string;
  fechaEstreno: number;
  genero: string;
  duracion: number;
}

@Injectable({ providedIn: 'root' })
export class PeliculasService {
  private apiUrl = 'https://localhost:7020/api/Peliculas';

  constructor(private http: HttpClient) {}

  getPeliculas(
    page: number,
    pageSize: number,
    genero?: string,
    director?: string,
    fechaEstreno?: number
  ): Observable<Pelicula[]> {
    let params = new HttpParams().set('page', page).set('pageSize', pageSize);
    if (genero) params = params.set('genero', genero);
    if (director) params = params.set('director', director);
    if (fechaEstreno) params = params.set('fechaEstreno', fechaEstreno);
    return this.http.get<Pelicula[]>(this.apiUrl, { params });
  }

  getPeliculaById(id: number): Observable<Pelicula> {
    return this.http.get<Pelicula>(`${this.apiUrl}/${id}`);
  }
}
