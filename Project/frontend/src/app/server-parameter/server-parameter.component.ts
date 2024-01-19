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

  show_form: boolean = false;
  selected_parameter: any;

  data_source: any;
  displayed_columns: string[] = ['parameterKey', 'parameterValue'];
  @ViewChild(MatPaginator) paginator !: MatPaginator;
  @ViewChild(MatSort) sort !: MatSort;

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
    this.getServerParameterByServer(this.selected_server.serverId);
  }

  changeParameter(element: any) {
    if(this.selected_parameter == element) {
      this.selected_parameter = null;
      return;
    }
    else {
      this.selected_parameter = element;
      console.log("selected_parameter: ", this.selected_parameter);
    }
  }

  copy() {
    throw new Error('Method not implemented.');
  }

  editForm() {
    this.show_form = true;
  }
  newForm() {
    this.show_form = true;

  }
}
