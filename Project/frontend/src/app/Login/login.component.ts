import { HttpClient } from '@angular/common/http';
import { Component, EventEmitter, Output, Renderer2 } from '@angular/core';

/***************************************************************************************/
/***************************************************************************************/
/***************************************************************************************/
@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css', './../app.component.css'],
})
/***************************************************************************************/
export class LoginComponent {
  // Message shown to the user if the login fails.
  showErrorMessage: string = '';

  /** The email entered by the user.                        */
  email: string = '';

  /** The password entered by the user.                     */
  motDePasse: string = '';

  /** Event emitter for passing the login data.             */
  @Output() loginData = new EventEmitter<{
    email: string;
  }>();

  /** Event emitter for hiding the login form.              */
  @Output() hideForm = new EventEmitter<void>();

  /** Event emitter to show the 2FA form.                */
  @Output() allow2FA = new EventEmitter<void>();

  /***************************************************************************************/
  /**
   * Creates an instance of LoginComponent.
   * @param renderer - The renderer used to create the login form.
   * @param http - The http client used to connect to the database.
   */
  constructor(private renderer: Renderer2, private http: HttpClient) {}

  /***************************************************************************************/
  /**
   * Connects to the database using the provided email and password.
   */
  connectDB() {
    const email = this.email;
    const password = this.motDePasse;

    this.http
      .post('http://localhost:5050/api/auth', { email, password })
      .subscribe(
        (data: any) => {
          if (data.exist) {
            console.log('data.exist === true');
            this.allow2FA.emit();
            localStorage.setItem('2FA', 'exists');
            localStorage.setItem('email', this.email); // TODO voir si bonne methode
          } else {
            // storage the jwt token in the local storage
            localStorage.setItem('token', data.token);
            localStorage.setItem('email', this.email); // TODO voir si bonne methode
            this.hideForm.emit();
            this.loginData.emit({
              email: this.email,
            });
          }
        },
        (error) => {
          if (error.error.message === 'User not found.') {
            this.showErrorMessage = 'Incorrect email or password!';
          } else if (
            error.error.message === 'User blocked, invalid attempts cout = 3.'
          ) {
            this.showErrorMessage = 'User blocked, 3 invalid attempts made!';
          }
        }
      );
  }

  /***************************************************************************************/
  /**
   * Login function used for testing Jasmine & Karma.
   * @param username - The username to be tested.
   * @param password - The password to be tested.
   * @returns True if the username and password match, false otherwise.
   */
  login(username: string, password: string): boolean {
    return username === 'test' && password == 'password';
  }
}
/***************************************************************************************/
/***************************************************************************************/
/***************************************************************************************/
