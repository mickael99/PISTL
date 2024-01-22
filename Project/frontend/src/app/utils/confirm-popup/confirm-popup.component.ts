import { Component, Input, Output, EventEmitter } from '@angular/core';

/***************************************************************************************/
/***************************************************************************************/
/***************************************************************************************/
@Component({
  selector: 'app-confirm-popup',
  templateUrl: './confirm-popup.component.html',
  styleUrls: ['./confirm-popup.component.css'],
})
export class ConfirmPopupComponent {
  // Confirmation message to display
  @Input() confirmationMessage: string = '';

  // Event emitter for confirming the action
  @Output() confirm = new EventEmitter<void>();

  // Event emitter to close the popup
  @Output() close = new EventEmitter<void>();

  /***************************************************************************************/
  /**
   * Confirms the action and closes the confirmation popup component.
   */
  onConfirm() {
    this.confirm.emit();
    this.closePopup();
  }

  /***************************************************************************************/
  /**
   * Closes the confirmation popup component.
   */
  onClose() {
    this.closePopup();
  }

  /***************************************************************************************/
  /**
   * Closes the confirmation popup component by emitting the close event.
   */
  private closePopup() {
    this.close.emit();
  }
}
/***************************************************************************************/
/***************************************************************************************/
/***************************************************************************************/
