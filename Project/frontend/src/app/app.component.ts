import { Component, Renderer2 } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent {
  showForm: boolean = true;
  email: string = '';
  motDePasse: string = '';

  hideLoginForm() {
    this.showForm = false;
  }

  onLoginData(loginData: { email: string; motDePasse: string }) {
    this.email = loginData.email;
    this.motDePasse = loginData.motDePasse;
  }

  deconnexion() {
    this.showForm = true;
  }
}
