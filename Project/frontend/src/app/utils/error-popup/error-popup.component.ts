import { Component, Input, Output, EventEmitter } from '@angular/core';

/***************************************************************************************/
/***************************************************************************************/
/***************************************************************************************/
@Component({
  selector: 'app-error-popup',
  templateUrl: './error-popup.component.html',
  styleUrls: ['./error-popup.component.css'],
})
export class ErrorPopupComponent {
  // Error message to display
  @Input() errorMessage: string = '';

  // Event emitter to close the popup
  @Output() close = new EventEmitter<void>();

  /***************************************************************************************/
  /**
   * Closes the error popup component by emitting the close event.
   */
  onClose() {
    this.close.emit();
  }
}
/***************************************************************************************/
/***************************************************************************************/
/***************************************************************************************/
