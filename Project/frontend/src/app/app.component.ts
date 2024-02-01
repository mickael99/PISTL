import { Component, Renderer2 } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

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

  // Indicates whether the error popup should be shown or not.
  show_error_popup: boolean = false;
  // Error message to display in the popup
  error_message: string = '';
  // Indicates the active page the user is viewing.
  active_page: string = '';

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

  /***************************************************************************************/
  /**
   * Function used to check if there is activity to be finished on the page.
   */
  checkActivity(page: string) {
    console.log('checkActivity');
    console.log('new page: ', page);

    if (this.active_page == 'sys-admin-by-domain') {
      console.log('active page: sys-admin-by-domain');
      var show_add_sysadmin =
        localStorage.getItem('show_add_sysadmin') == 'true';
      console.log('show_add_sysadmin', show_add_sysadmin);
      if (!show_add_sysadmin) {
        this.showErrorPopup(
          'Les modifications ne sont pas encore enregistrées. Veuillez les enregistrer avant de quitter la page.'
        );
        return 'sys-admin-by-domain';
      }
    }
    if (this.active_page == 'domain-by-sys-admin') {
      console.log('active page: domain-by-sys-admin');
      var show_add_domainby =
        localStorage.getItem('show_add_domainby') == 'true';
      console.log('show_add_domainby', show_add_domainby);
      if (!show_add_domainby) {
        this.showErrorPopup(
          'Les modifications ne sont pas encore enregistrées. Veuillez les enregistrer avant de quitter la page.'
        );
        return 'domain-by-sys-admin';
      }
    }

    return page;
  }

  /***************************************************************************************/
  /**
   * Function used to display activate the error popup.
   * @param message - Error message.
   */
  showErrorPopup(message: string) {
    console.log('showErrorPopup');
    this.error_message = message;
    this.show_error_popup = true;
  }

  /***************************************************************************************/
  /**
   * Function used to close the error popup.
   */
  closeErrorPopup() {
    console.log('closeErrorPopup');
    this.show_error_popup = false;
  }
}
/***************************************************************************************/
/***************************************************************************************/
/***************************************************************************************/
