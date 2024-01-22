import { Component, EventEmitter, Input, Output } from '@angular/core';

/***************************************************************************************/
/***************************************************************************************/
/***************************************************************************************/
@Component({
  selector: 'app-inform-popup',
  templateUrl: './inform-popup.component.html',
  styleUrls: ['./inform-popup.component.css'],
})
export class InformPopupComponent {
  // Password reseted message to display
  @Input() passwordReseted: string = '';

  // Information message to display
  @Input() informationMessage: string = '';

  // Event emitter to close the popup
  @Output() close = new EventEmitter<void>();

  // Boolean to mask the password
  isPasswordMasked: boolean = true;

  /***************************************************************************************/
  /**
   * Masks the password.
   *
   * @param password The password to mask
   * @returns The masked password
   */
  maskPassword(password: string): string {
    return '*'.repeat(password.length);
  }

  /***************************************************************************************/
  /**
   * Toggles the password visibility.
   */
  togglePasswordVisibility(): void {
    this.isPasswordMasked = !this.isPasswordMasked;
  }

  /***************************************************************************************/
  /**
   * Closes the error popup component by emitting the close event.
   */
  onClose() {
    this.close.emit();
    this.isPasswordMasked = true;
  }
}
/***************************************************************************************/
/***************************************************************************************/
/***************************************************************************************/
