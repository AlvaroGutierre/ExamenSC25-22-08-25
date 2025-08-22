// Removed duplicate imports
import { routes } from './app.routes';
import { provideRouter } from '@angular/router';
import { provideHttpClient } from '@angular/common/http';
import { PeliculasComponent } from './peliculas.component';
import { bootstrapApplication } from '@angular/platform-browser';

import { AppComponent } from './app.component';
bootstrapApplication(AppComponent, {
  providers: [provideRouter(routes), provideHttpClient()],
});
