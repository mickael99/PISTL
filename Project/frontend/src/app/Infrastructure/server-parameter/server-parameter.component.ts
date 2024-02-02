import { HttpClient } from '@angular/common/http';
import { Component, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-server-parameter',
  templateUrl: './server-parameter.component.html',
  styleUrls: ['./server-parameter.component.css'],
})
export class ServerParameterComponent {
  servers: any[];
  server_parameters: any[];

  data_server_table: any[] = [];
  selected_server: any;

  show_new_form: boolean = false;
  show_edit_form: boolean = false;
  show_error: boolean = false;
  edit_enabled: boolean = false;

  data_source: any;
  displayed_columns: string[] = ['parameterKey', 'parameterValue'];
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;
  error_message: string;
  selected_parameter_copy: any;
  show_edit: boolean;

  selected_parameter = {
    parameterKey: '',
    parameterValue: '',
    serverId: '',
  };

  // Hover the two columns in the table
  isHovered: boolean = false;

  // User hovered in the table
  userHovered: string = '';

  // Bool used to display the delete confirmation popup
  showDeleteConfirmation: boolean = false;

  // Bool used to display the reset password confirmation popup
  showResetPasswordConfirmation: boolean = false;

  // Password reseted
  passwordReseted: string = '';

  // Bool used to display the information popup
  showInformPopup: boolean = false;

  // Bool that allows or not to display the error popup
  showPopupError: boolean = false;

  // Bool used to display the copy confirmation popup
  showCopyConfirmation: boolean = false;

  // Pop-up message to display
  popupMessage: string = '';

  constructor(private http: HttpClient) {
    this.getServerParameterByServer(1); //
  }

  getServerParameterByServer(server_id: any) {
    this.servers = [];
    this.server_parameters = [];

    this.http
      .get('http://localhost:5050/api/serverparameter/' + server_id)
      .subscribe(
        (data: any) => {
          console.log(data);
          for (const server of data.servers) {
            this.servers.push(server);
            if (server.serverId === server_id) {
              this.selected_server = server;
            }
          }
          if (this.selected_server == null && this.servers.length > 0)
            this.selected_server = this.servers[0];

          for (const parameter of data.server_parameters) {
            this.server_parameters.push(parameter);
          }

          this.data_server_table = [];
          for (const parameter of this.server_parameters) {
            this.data_server_table.push([
              parameter.parameterKey,
              parameter.parameterValue,
            ]);
          }

          this.data_source = new MatTableDataSource(this.data_server_table);
          this.data_source.paginator = this.paginator;
          this.data_source.sort = this.sort;
        },
        (error: any) => {
          console.log(error);
        }
      );
  }

  onChange(event: any) {
    this.selected_server = event.value;
    this.reinitaliseParameterSelectedForm();
    this.getServerParameterByServer(this.selected_server.serverId);
  }

  changeParameter(element: any) {
    if (this.selected_parameter == element) {
      this.show_edit = false;
      this.selected_parameter = null;
      return;
    } else {
      this.reinitaliseParameterSelectedForm();
      this.show_edit = true;
      this.show_edit_form = true;
      this.selected_parameter = element;
    }
  }

  copy() {
    this.selected_parameter_copy = [
      this.selected_parameter[0],
      this.selected_parameter[1],
    ];

    // Check if the last two characters of this.selected_parameter[0] match the pattern "_"+ (number)
    const regex = /_\d$/;
    if (regex.test(this.selected_parameter[0])) {
      this.selected_parameter_copy[0] =
        this.selected_parameter_copy[0].slice(0, -1) +
        (parseInt(this.selected_parameter_copy[0].slice(-1)) + 1).toString();
    } else {
      this.selected_parameter_copy[0] = this.selected_parameter_copy[0] + '_1';
    }

    this.http
      .post('http://localhost:5050/api/serverparameter', {
        parameterKey: this.selected_parameter_copy[0],
        parameterValue: this.selected_parameter_copy[1],
        serverId: this.selected_server.serverId,
      })
      .subscribe(
        (data: any) => {
          this.server_parameters = [];
          for (const parameter of data) {
            this.server_parameters.push(parameter);
          }
          this.data_server_table = [];
          for (const parameter of this.server_parameters) {
            this.data_server_table.push([
              parameter.parameterKey,
              parameter.parameterValue,
            ]);
          }
          this.data_source = new MatTableDataSource(this.data_server_table);
          this.data_source.paginator = this.paginator;
          this.data_source.sort = this.sort;

          this.reinitaliseParameterSelectedForm();
        },
        (error: any) => {
          console.log(error);
          this.show_error = true;
        }
      );
  }

  delete() {
    this.http
      .delete(
        'http://localhost:5050/api/serverparameter/' +
          this.selected_server.serverId +
          '/' +
          this.selected_parameter[0]
      )
      .subscribe(
        (data: any) => {
          this.server_parameters = [];
          for (const parameter of data) {
            this.server_parameters.push(parameter);
          }
          this.data_server_table = [];
          for (const parameter of this.server_parameters) {
            this.data_server_table.push([
              parameter.parameterKey,
              parameter.parameterValue,
            ]);
          }
          this.data_source = new MatTableDataSource(this.data_server_table);
          this.data_source.paginator = this.paginator;
          this.data_source.sort = this.sort;

          this.reinitaliseParameterSelectedForm();
        },
        (error: any) => {
          console.log(error);
          this.show_error = true;
        }
      );
  }

  editForm() {
    this.edit_enabled = true;
    this.selected_parameter_copy = [
      this.selected_parameter[0],
      this.selected_parameter[1],
    ];
  }

  newForm() {
    this.reinitaliseParameterSelectedForm();
    this.show_new_form = true;
    this.selected_parameter = {
      parameterKey: '',
      parameterValue: '',
      serverId: this.selected_server.serverId,
    };
    this.edit_enabled = true;
  }

  createParameter() {
    if (this.selected_parameter[0] == '' || this.selected_parameter[1] == '') {
      this.showErrorPopup('Please fill all the fields.');
      return;
    }
    this.http
      .post('http://localhost:5050/api/serverparameter', {
        parameterKey: this.selected_parameter[0],
        parameterValue: this.selected_parameter[1],
        serverId: this.selected_server.serverId,
      })
      .subscribe(
        (data: any) => {
          this.reinitaliseParameterSelectedForm(data);
        },
        (error: any) => {
          console.log(error);
          this.show_error = true;
        }
      );
  }

  editParameter() {
    this.edit_enabled = true;
    if (
      this.selected_parameter[0] == this.selected_parameter_copy[0] &&
      this.selected_parameter[1] == this.selected_parameter_copy[1]
    ) {
      this.showErrorPopup('Please do some changes before Save.');
      return;
    }
    this.http
      .put('http://localhost:5050/api/serverparameter', {
        parameterKey: this.selected_parameter[0],
        parameterValue: this.selected_parameter[1],
        serverId: this.selected_server.serverId,
      })
      .subscribe(
        (data: any) => {
          this.reinitaliseParameterSelectedForm(data);
        },
        (error: any) => {
          console.log(error);
          this.show_error = true;
        }
      );
  }

  reinitaliseParameterSelectedForm(data: any = this.server_parameters) {
    this.show_new_form = false;
    this.show_edit_form = false;
    this.selected_parameter = null;
    this.show_edit = false;

    // Disable the 'Edit' button
    this.edit_enabled = false;

    // Reset the form
    this.selected_parameter = {
      parameterKey: '',
      parameterValue: '',
      serverId: this.selected_server.serverId,
    };

    // Hide the error message
    this.show_error = false;

    // Reset table
    this.server_parameters = [];
    for (const parameter of data) {
      this.server_parameters.push(parameter);
    }
    this.data_server_table = [];
    for (const parameter of this.server_parameters) {
      this.data_server_table.push([
        parameter.parameterKey,
        parameter.parameterValue,
      ]);
    }
    this.data_source = new MatTableDataSource(this.data_server_table);
    this.data_source.paginator = this.paginator;
    this.data_source.sort = this.sort;
  }

  /***************************************************************************************/
  /**
   * Function used to activate the hover.
   */
  onMouseEnter(userHovered: string) {
    this.isHovered = true;
    this.userHovered = userHovered;
  }

  /***************************************************************************************/
  /**
   * Function used to deactivate the hover.
   */
  onMouseLeave() {
    this.isHovered = false;
    this.userHovered = '';
  }

  /***************************************************************************************/
  /**
   * Function used to display activate the error popup.
   * @param message - Error message.
   */
  showErrorPopup(message: string) {
    this.showPopupError = true;
    this.popupMessage = message;
  }

  /***************************************************************************************/
  /**
   * Function used to close the error popup.
   */
  closeErrorPopup() {
    this.showPopupError = false;
  }

  /***************************************************************************************/
  /**
   * Function used to close the Reset Password confirmation popup.
   */
  onResetPasswordClose() {
    this.showResetPasswordConfirmation = false;
  }

  /***************************************************************************************/
  /**
   * Function used to display activate the inform popup.
   * @param message - Error message.
   */
  show_inform_popup(message: string) {
    this.popupMessage = message;
    this.showInformPopup = true;
  }

  /***************************************************************************************/
  /**
   * Function used to close the inform popup.
   */
  close_inform_popup() {
    this.showInformPopup = false;
    this.popupMessage = '';
    this.passwordReseted = '';
  }

  /***************************************************************************************/
  /**
   * Opens the copy confirmation dialog.
   */
  openCopyConfirmation() {
    this.showCopyConfirmation = true;
    this.popupMessage =
      'Are you sure you want to copy this server parameter: ' +
      this.selected_parameter[0] +
      '?';
  }

  /***************************************************************************************/
  /**
   * Function used to close the copy confirmation popup.
   */
  onCopyClose() {
    this.showCopyConfirmation = false;
    this.popupMessage = '';
  }

  /***************************************************************************************/
  /**
   * Function used to close the delete confirmation popup.
   */
  onDeleteClose() {
    this.showDeleteConfirmation = false;
    this.popupMessage = '';
  }

  /***************************************************************************************/
  /**
   * Opens the delete confirmation dialog.
   * Sets the showDeleteConfirmation flag to true, logs the value of showDeleteConfirmation,
   * sets the confirmation message to a formatted string, logs the confirmation message,
   * and triggers change detection.
   */
  openDeleteConfirmation() {
    this.showDeleteConfirmation = true;
    this.popupMessage =
      'Are you sure you want to delete this server: ' +
      this.selected_parameter[0] +
      '?';
  }
}
/***************************************************************************************/
/***************************************************************************************/
/***************************************************************************************/
