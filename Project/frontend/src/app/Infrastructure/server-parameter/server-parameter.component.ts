import { HttpClient } from '@angular/common/http';
import { Component, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-server-parameter',
  templateUrl: './server-parameter.component.html',
  styleUrls: ['./server-parameter.component.css']
})
export class ServerParameterComponent {
  servers: {[server_id: number]: any} = {};
  server_parameters: any[] = [];

  data_server_table: any[] = [];
  selected_server: any;

  show_new_form: boolean = false;
  show_edit_form: boolean = false;
  selected_parameter: any;
  show_error: boolean = false;
  edit_enabled: boolean = false;

  data_source: any;
  displayed_columns: string[] = ['parameterKey', 'parameterValue'];
  @ViewChild(MatPaginator) paginator !: MatPaginator;
  @ViewChild(MatSort) sort !: MatSort;
  error_message: string;
  selected_parameter_copy: any;
  show_edit: boolean;


  constructor(private http: HttpClient) {
    this.getServerParameterByServer(227);
  }

  getServerParameterByServer(server_id: number) {
    this.servers = {};
    this.server_parameters = [];

    this.http.get('http://localhost:5050/api/serverparameter/' + server_id).subscribe(
      (data: any) => {
        console.log(data);

        for(const server of data.servers) {
          this.servers[server.serverId] = server;
        }
        console.log("servers: ", this.servers);
        
        this.selected_server = this.servers[server_id];
        console.log("selected_server: ", this.selected_server);

        for(const parameter of data.server_parameters) {
          this.server_parameters.push(parameter);
        }

        this.data_server_table = [];
        for(const parameter of this.server_parameters) {
          this.data_server_table.push([parameter.parameterKey, parameter.parameterValue]);
        }

        console.log("data_server_table: ", this.data_server_table);

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
    if(this.selected_parameter == element) {
      this.show_edit = false;
      this.selected_parameter = null;
      return;
    }
    else {
      this.reinitaliseParameterSelectedForm();
      this.show_edit = true;
      this.show_edit_form = true;
      this.selected_parameter = element;
    }
  }

  copy() {
    this.selected_parameter_copy = [this.selected_parameter[0], this.selected_parameter[1]];
    
    // Check if the last two characters of this.selected_parameter[0] match the pattern "_"+ (number)
    const regex = /_\d$/;
    if (regex.test(this.selected_parameter[0])) {
      this.selected_parameter_copy[0] = this.selected_parameter_copy[0].slice(0, -1) + (parseInt(this.selected_parameter_copy[0].slice(-1)) + 1).toString();
    }
    else {
      this.selected_parameter_copy[0] = this.selected_parameter_copy[0] + "_1";
    }

    this.http.post('http://localhost:5050/api/serverparameter', {
      parameterKey: this.selected_parameter_copy[0],
      parameterValue: this.selected_parameter_copy[1],
      serverId: this.selected_server.serverId
    }).subscribe(
      (data: any) => {
        console.log(data);
        this.server_parameters = [];
        for(const parameter of data) {
          this.server_parameters.push(parameter);
        }
        this.data_server_table = [];
        for(const parameter of this.server_parameters) {
          this.data_server_table.push([parameter.parameterKey, parameter.parameterValue]);
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
    this.selected_parameter_copy = [this.selected_parameter[0], this.selected_parameter[1]];
  }

  newForm() {
    this.reinitaliseParameterSelectedForm();
    this.show_new_form = true;
    this.selected_parameter = {
      parameterKey: "",
      parameterValue: "",
      serverId: this.selected_server.serverId
    }
    this.edit_enabled = true;
  }

  createParameter() {
    if (
      this.selected_parameter[0] == '' ||
      this.selected_parameter[1] == ''
    ) {
      this.showError('Please fill all the fields.');
      console.log(this.error_message);
      return;
    }
    this.http.post('http://localhost:5050/api/serverparameter', {
      parameterKey: this.selected_parameter[0],
      parameterValue: this.selected_parameter[1],
      serverId: this.selected_server.serverId
    }).subscribe(
      (data: any) => {
        console.log(data);

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
      this.showError('Please do some changes before Save.');
      console.log(this.error_message);
      return;
    }
    this.http.put('http://localhost:5050/api/serverparameter', {
      parameterKey: this.selected_parameter[0],
      parameterValue: this.selected_parameter[1],
      serverId: this.selected_server.serverId
    }).subscribe(
      (data: any) => {
        console.log(data);

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
      parameterKey: "",
      parameterValue: "",
      serverId: this.selected_server.serverId
    }

    // Hide the error message
    this.show_error = false;

    // Reset table  
    this.server_parameters = [];
    for(const parameter of data) {
      this.server_parameters.push(parameter);
    }
    this.data_server_table = [];
    for(const parameter of this.server_parameters) {
      this.data_server_table.push([parameter.parameterKey, parameter.parameterValue]);
    }
    this.data_source = new MatTableDataSource(this.data_server_table);
    this.data_source.paginator = this.paginator;
    this.data_source.sort = this.sort;
  }

  showError(message: string) {
    this.show_error = true;
    this.error_message = message;
  }
}
