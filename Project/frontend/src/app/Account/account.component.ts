import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, Renderer2 } from '@angular/core';

/***************************************************************************************/
/***************************************************************************************/
/***************************************************************************************/
@Component({
  selector: 'app-account',
  templateUrl: './account.component.html',
  styleUrls: ['./account.component.css'],
})
/***************************************************************************************/
export class AccountComponent {
  qrCodeUrl: string = '';
  manualEntryKey: string = '';
  hidden2FA: boolean = true;

  /***************************************************************************************/
  /**
   * Creates an instance of AccountComponent.
   * @param renderer - The renderer used to create the account page.
   * @param http - The http client used to connect to the database.
   */
  constructor(private renderer: Renderer2, private http: HttpClient) {
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
  added2FA() {
    this.hidden2FA = true;
    console.log('===> this.manualEntryKey: ', this.manualEntryKey);
    let JWTToken = localStorage.getItem('token');

    const options = {
      headers: new HttpHeaders({
        Authorization: 'Bearer ' + JWTToken,
        'Content-Type': 'application/json',
      }),
    };

    const requestBody = { manualEntryKey: this.manualEntryKey };

    this.http
      .post('http://localhost:5050/api/account', requestBody, options)
      .subscribe(
        (data: any) => {
          console.log('Response from server:', data);
        },
        (error) => {
          console.error('Error from server:', error);
        }
      );
  }
}
/***************************************************************************************/
/***************************************************************************************/
/***************************************************************************************/
