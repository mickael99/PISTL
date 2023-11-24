import { HttpClient } from '@angular/common/http';
import {
  Component,
  EventEmitter,
  Input,
  Output,
  Renderer2,
} from '@angular/core';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css', './../app.component.css'],
})
export class LoginComponent {
  @Output() hideForm = new EventEmitter<void>();
  showError: boolean = false;
  email: string = '';
  motDePasse: string = '';
  @Output() loginData = new EventEmitter<{
    email: string;
    motDePasse: string;
  }>();

  constructor(private renderer: Renderer2, private http: HttpClient) {}

  connectDB() {
    const email = this.email;
    const password = this.motDePasse;

    this.http
      .post('http://localhost:5050/api/auth', { email, password })
      .subscribe(
        (data: any) => {
          if (data.message === 'Successful connection') {
            this.hideLoginForm();
            this.loginData.emit({
              email: this.email,
              motDePasse: this.motDePasse,
            });
          } else {
            this.showError = true;
          }
        },
        (error) => {
          alert('Connection error: ' + error.message);
        }
      );
  }

  hideLoginForm() {
    this.hideForm.emit();
  }

  /** To test how Jasmine & Karma works */
  login(username: string, password: string): boolean {
    return username === 'test' && password == 'password';
  }
}
