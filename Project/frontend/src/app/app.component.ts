import { Component, Renderer2 } from '@angular/core';
import { HttpClient } from '@angular/common/http';

/***************************************************************************************/
/***************************************************************************************/
/***************************************************************************************/
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
/***************************************************************************************/
export class AppComponent {
  /** Indicates whether the login form should be shown or not.          */
  showForm: boolean = true;

  /** Indicates wheter the 2FA part should be shown or not.             */
  show2FA: boolean = false;

  /** The email entered in the login form.                              */
  email: string = '';

  /** The password entered in the login form.                           */
  motDePasse: string = '';

  /***************************************************************************************/
  /**
   * Initializes the component.
   * If a jwt token is stored in local storage, a user has already been logged in.
   */
  ngOnInit() {
    if (localStorage.getItem('email')) {
      this.hideLoginForm();
      this.email = localStorage.getItem('email');
    }
    console.log('show2FA', this.show2FA);
  }

  /***************************************************************************************/
  /**
   * Hides the login form and shows the 2FA form.
   */
  hideLoginForm() {
    this.showForm = false;
    if (localStorage.getItem('2FA')) {
      this.show2FA = true;
    } else {
      this.show2FA = false;
    }
  }

  /***************************************************************************************/
  /**
   * Hides the 2FA form and shows the login form.
   */
  hide2FA() {
    console.log('hide2FA app');
    this.show2FA = false;
  }

  /***************************************************************************************/
  /**
   * Shows the 2FA form.
   */
  allow2FA() {
    console.log('show2FA app');
    this.show2FA = true;
    this.showForm = false;
  }

  /***************************************************************************************/
  /**
   * Handles the login data submitted from the login form.
   * @param loginData - The login data containing the email and password.
   */
  onLoginData(loginData: { email: string; motDePasse: string }) {
    this.email = loginData.email;
    this.motDePasse = loginData.motDePasse;
  }

  /***************************************************************************************/
  /**
   * Logs out the user by removing the jwt token and email from the local storage.
   * The login form appears again.
   */
  deconnexion() {
    this.showForm = true;
    localStorage.removeItem('token');
    localStorage.removeItem('email');
    localStorage.removeItem('2FA');
  }
}
/***************************************************************************************/
/***************************************************************************************/
/***************************************************************************************/
