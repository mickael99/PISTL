import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, Renderer2 } from '@angular/core';

/***************************************************************************************/
/***************************************************************************************/
/***************************************************************************************/
@Component({
  selector: 'app-account',
  templateUrl: './account.component.html',
  styleUrls: ['./account.component.css', './../app.component.css'],
})
/***************************************************************************************/
export class AccountComponent {
  // The QR code URL
  qrCodeUrl: string = '';

  // The manual entry key (2FA QR code)
  manualEntryKey: string = '';

  // Boolean to show/hide the 2FA (Two-Factor Authentication) configuration section
  hidden2FA: boolean = true;

  // Boolean to show/hide the QR code
  showQrCode: boolean = true;

  // The code entered by the user
  code: string = '';

  // Indicates whether to show an error message or not
  showError: boolean = false;

  // The error message to show
  messageError: string = '';

  /***************************************************************************************/
  /**
   * Creates an instance of AccountComponent.
   * @param renderer - The renderer used to create the account page.
   * @param http - The http client used to connect to the database.
   */
  constructor(private renderer: Renderer2, private http: HttpClient) {
    this.getQRCode();
  }

  /***************************************************************************************/
  /**
   * Gets the QR code from the server.
   */
  getQRCode() {
    let JWTToken = localStorage.getItem('token');
    const options = {
      headers: new HttpHeaders({
        Authorization: 'Bearer ' + JWTToken,
        'Content-Type': 'application/json',
      }),
    };

    this.http.get('http://localhost:5050/api/account', options).subscribe(
      (data: any) => {
        if (data.message !== '2FA is enabled for this user.') {
          this.qrCodeUrl = data.qrCodeUrl;
          this.manualEntryKey = data.manualEntryCode;
          this.hidden2FA = false;
        }
      },
      (error) => {
        console.log('2FA not enabled - ' + error.message);
      }
    );
  }

  /***************************************************************************************/
  /**
   * Performs the necessary steps to enable two-factor authentication for the account.
   */
  added2FA() {
    this.showError = false;
    this.messageError = '';
    let JWTToken = localStorage.getItem('token');

    const options = {
      headers: new HttpHeaders({
        Authorization: 'Bearer ' + JWTToken,
        'Content-Type': 'application/json',
      }),
    };

    const requestBody = {
      manualEntryKey: this.manualEntryKey,
      code: this.code,
    };

    this.http
      .post('http://localhost:5050/api/account', requestBody, options)
      .subscribe(
        (data: any) => {
          console.log('Response from server:', data);
          this.hidden2FA = true;
        },
        (error) => {
          this.showError = true;
          this.messageError = error.error.message;
        }
      );
  }

  /***************************************************************************************/
  /**
   * Toggles the QR code.
   */
  toggleQrCode() {
    this.showQrCode = true;
  }

  /***************************************************************************************/
  /**
   * Deletes the 2FA (Two-Factor Authentication) for the current user.
   * Requires a valid JWT token for authorization.
   * Sends a DELETE request to the server to remove the 2FA.
   */
  delete2FA() {
    let JWTToken = localStorage.getItem('token');

    const options = {
      headers: new HttpHeaders({
        Authorization: 'Bearer ' + JWTToken,
        'Content-Type': 'application/json',
      }),
    };

    this.http.delete('http://localhost:5050/api/account', options).subscribe(
      (data: any) => {
        console.log('Response from server:', data);
        this.hidden2FA = false;
        this.getQRCode();
      },
      (error) => {
        this.showError = true;
        this.messageError = error.error.message;
      }
    );
  }
}
/***************************************************************************************/
/***************************************************************************************/
/***************************************************************************************/
