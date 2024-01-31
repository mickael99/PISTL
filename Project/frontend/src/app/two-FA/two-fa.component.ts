import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, EventEmitter, Output, Renderer2 } from '@angular/core';

/***************************************************************************************/
/***************************************************************************************/
/***************************************************************************************/
@Component({
  selector: 'app-two-fa',
  templateUrl: './two-fa.component.html',
  styleUrls: ['./two-fa.component.css', './../app.component.css'],
})
/***************************************************************************************/
export class TwoFAComponent {
  /** The code entered by the user.                         */
  code: string = '';

  /** Indicates whether to show an error message or not.    */
  showError: boolean = false;

  /** Event emitter for hiding the 2FA form.                */
  @Output() hide2FA = new EventEmitter<void>();

  /** Event emitter for passing the login data.             */
  @Output() loginData = new EventEmitter<{
    email: string;
  }>();

  /***************************************************************************************/
  /**
   * Creates an instance of TwoFAComponent.
   * @param renderer - The renderer used to create the 2FA form.
   * @param http - The http client used to connect to the database.
   */
  constructor(private renderer: Renderer2, private http: HttpClient) {}

  /***************************************************************************************/
  testCode2FA() {
    const requestBody = {
      code: this.code,
      email: localStorage.getItem('email'),
    };
    this.http
      .post('http://localhost:5050/api/account/2fa', requestBody)
      .subscribe(
        (data: any) => {
          if (data.message === 'success') {
            this.hide2FA.emit();
            // storage the jwt token in the local storage
            localStorage.setItem('token', data.token);
            localStorage.removeItem('2FA');
            this.loginData.emit({
              email: localStorage.getItem('email'), // AV si mieux a faire
            });
          } else {
            this.showError = true;
            console.log(data.message);
          }
        },
        (error) => {
          this.showError = true;
        }
      );
  }

  /***************************************************************************************/
  cancel() {
    localStorage.removeItem('token');
    localStorage.removeItem('email');
    localStorage.removeItem('2FA');

    window.location.reload();
  }
}
/***************************************************************************************/
/***************************************************************************************/
/***************************************************************************************/
