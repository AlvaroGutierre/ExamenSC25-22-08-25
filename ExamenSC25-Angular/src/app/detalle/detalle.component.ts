import { Component, OnInit } from '@angular/core';
import { ChangeDetectorRef } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { PeliculasService, Pelicula } from '../peliculas.service';

@Component({
  selector: 'app-detalle',
  templateUrl: './detalle.component.html',
  styleUrls: ['./detalle.component.css'],
  standalone: true,
  imports: [CommonModule],
})
export class DetalleComponent implements OnInit {
  volverLista() {
    // Navega a la ruta raíz
    this.router.navigate(['/']);
  }
  pelicula: Pelicula | null = null;

  constructor(
    private route: ActivatedRoute,
    private peliculasService: PeliculasService,
    private cdr: ChangeDetectorRef,
    private router: Router
  ) {}

  ngOnInit() {
    const id = this.route.snapshot.paramMap.get('id');
    console.log('DetalleComponent ngOnInit, id:', id);
    if (id !== null && id !== undefined && id !== '') {
      this.peliculasService.getPeliculaById(+id).subscribe({
        next: (data: Pelicula) => {
          console.log('DetalleComponent: data received', data);
          this.pelicula = data;
          this.cdr.detectChanges();
        },
        error: (err) => {
          console.error('DetalleComponent: error', err);
        },
      });
    } else {
      console.warn('DetalleComponent: id param is missing or invalid');
    }
  }
}
