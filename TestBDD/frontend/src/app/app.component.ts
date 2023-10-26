import { Component, Renderer2 } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent {
  showForm: boolean = true;
  showError: boolean = false;
  email: string = '';
  motDePasse: string = '';

  constructor(private renderer: Renderer2, private http: HttpClient) {}

  connectDB() {
    const email = this.email;
    const motDePasse = this.motDePasse;

    this.http
      .post('http://localhost:5050/api/auth', { email, motDePasse })
      .subscribe(
        (data: any) => {
          if (data.message === 'Successful connection') {
            this.showForm = false;
          } else {
            this.showError = true;
          }
        },
        (error) => {
          alert('Connection error: ' + error.message);
        }
      );
  }

  deconnexion() {
    this.showForm = true;
  }

  /** To test how Jasmine & Karma works */
  login(username: string, password: string): boolean {
    return username === 'test' && password == 'password';
  }
}
