import { Component, OnInit } from '@angular/core';
import { ChangeDetectorRef } from '@angular/core';
import { PeliculasService, Pelicula } from './peliculas.service';
import { Router } from '@angular/router';

import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-peliculas',
  templateUrl: './peliculas.component.html',
  styleUrls: ['./peliculas.component.css'],
  standalone: true,
  imports: [CommonModule, FormsModule],
})
export class PeliculasComponent implements OnInit {
  peliculas: Pelicula[] = [];
  page = 1;
  pageSize = 10;
  genero = '';
  director = '';
  fechaEstreno?: number;
  loading = false;
  totalPages = 1;
  totalCount = 0;

  constructor(
    private peliculasService: PeliculasService,
    private router: Router,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit() {
    this.getPeliculas();
  }

  getPeliculas() {
    this.loading = true;
    this.peliculasService
      .getPeliculas(
        this.page,
        this.pageSize,
        this.genero,
        this.director,
        this.fechaEstreno
      )
      .subscribe({
        next: (data: any) => {
          this.peliculas = data.items;
          this.totalPages = data.totalPages;
          this.totalCount = data.totalCount;
          this.loading = false;
          this.cdr.detectChanges();
        },
        error: (err) => {
          console.error('PeliculasComponent: error', err);
          this.loading = false;
          this.cdr.detectChanges();
        },
      });
  }

  onFilterChange() {
    this.page = 1;
    this.getPeliculas();
  }

  nextPage() {
    this.page++;
    this.getPeliculas();
  }

  prevPage() {
    if (this.page > 1) {
      this.page--;
      this.getPeliculas();
    }
  }

  goToFirstPage() {
    this.page = 1;
    this.getPeliculas();
  }

  goToLastPage() {
    this.page = this.totalPages;
    this.getPeliculas();
  }

  verDetalles(pelicula: Pelicula) {
    this.router.navigate(['/detalle', pelicula.id]);
  }
}
