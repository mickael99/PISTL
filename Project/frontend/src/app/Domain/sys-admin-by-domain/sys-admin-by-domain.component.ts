import { HttpClient } from '@angular/common/http';
import { Component, Renderer2, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { SysAdminByDomainDialog } from './sys-admin-by-domain-dialog.component'; // Replace with the actual path
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { DatePipe } from '@angular/common';
import { forkJoin } from 'rxjs';

@Component({
  selector: 'app-sys-admin-by-domain',
  templateUrl: './sys-admin-by-domain.component.html',
  styleUrls: ['./sys-admin-by-domain.component.css'],
})

/**
 * Component for managing system administrators by domain.
 */
export class SysAdminByDomainComponent {
  domains:  { [domain_id: number]: any } = {};
  logins: any[] = [];
  users: any[] = [];

  // dictionnary key is loginId and value is an triplet (login.email, array of sysAdmin rights by env, array of admin users)
  login_users: { [login_id: number]: any } = {};
  // data displayed in the table
  data_domain_table: any[] = [];
  selected_domain: any;
  one_checked: {[env: number]: boolean} = {1: false, 2: false, 3: false, 4: false, 5: false, 6: false};

  show_add = true;
  show_calendar: { [login_id: number]: any } = {};

  data_source: any;
  displayed_columns: string[] = ['email', 'dev', 'preprod', 'prod', 'test', 'prodCopy', 'staging'];
  @ViewChild(MatPaginator) paginator !: MatPaginator;
  @ViewChild(MatSort) sort !: MatSort;
  datepipe: DatePipe = new DatePipe('en-US');

  constructor(private http: HttpClient, private dialog: MatDialog) {
    this.getSysAdminByDomain(351, false);
  }

  getSysAdminByDomain(domain_id: any, all: boolean): void {
    this.domains = {};
    this.logins = [];
    this.users = [];

    this.http.get('http://localhost:5050/api/sysadminbydomain/' + domain_id).subscribe(
      (data: any) => {
        for (const domain of data.domains) {
          this.domains[domain.domainId] = domain;
        }

        this.selected_domain = this.domains[domain_id];

        for (const login of data.logins) {
          this.logins.push(login);
        }

        for (const user of data.users) {
          if(user.userId === "99999999-9999-9999-9999-999999999999")
            this.users.push(user);
        }

        this.loadDomainUsers(all);
        this.updateOneChecked();
      },
      (error) => {
        alert('Connection error: ' + error.message);
      }
    );
  }

  loadDomainUsers(all: boolean): void {
    var login_object;
    for (const login of this.logins) {
      login_object = [];
      var tab_users: { [key: string] : any} = {};
      for (const user of this.users) {
        if(user.loginId === login.loginId && user.domainId === this.selected_domain.domainId){
          tab_users[user.environment] = user;
        }
      }
      if(Object.keys(tab_users).length > 0){
        login_object.push(login.email + '[' + login.loginId + ']');
        var user_env: {[key: number] : boolean} = {1: false, 2: false, 3: false, 4: false, 5: false, 6: false};
        for(const user of Object.keys(tab_users)){
          for(var i=1; i<7; i++){
            if(tab_users[user].environment === i && tab_users[user].sysAdmin === true){
              user_env[i] = true;
            }
          }
        }
        login_object.push(user_env);
        login_object.push(tab_users);
      }
      else if (all){
        login_object.push(login.email + '[' + login.loginId + ']');
        login_object.push({1: false, 2: false, 3: false, 4: false, 5: false, 6: false});
        login_object.push({});
      }
      this.login_users[login.loginId] = login_object;
    }

    this.data_domain_table = [];
    for (const login of this.logins) {
      var line = [login.loginId ,(this.login_users[login.loginId][0])]
      if(this.login_users[login.loginId][1]){
        for(var i=1; i<7; i++){
          line.push(this.login_users[login.loginId][1][i]);
        }
        this.data_domain_table.push(line);
      }
    }

    if(Object.keys(this.show_calendar).length === 0){

      for (const loginId of Object.keys(this.login_users)) {
        this.show_calendar[loginId] = {};
        if(this.login_users[loginId].length > 0){
          for(const env of Object.keys(this.login_users[loginId][2])){
            this.show_calendar[loginId][env] = false;
            if(this.login_users[loginId][2][env].sysAdminEndDate || null)
                this.show_calendar[loginId][env] = true;
          }
        }
      } 
    }

    this.data_source = new MatTableDataSource(this.data_domain_table);
    this.data_source.paginator = this.paginator;
    this.data_source.sort = this.sort;
  }

  onChange(event: any): void {
    this.selected_domain = event.value;
    this.getSysAdminByDomain(this.selected_domain.domainId, false);
  }


  check(loginId: any, env: any): void {
    const dialogConfig = new MatDialogConfig();
    dialogConfig.disableClose = true;
    dialogConfig.autoFocus = true;
    
    if(this.login_users[loginId][2][env]){

      dialogConfig.data = {user: this.login_users[loginId][2][env], name: this.login_users[loginId][0]};

      // Open the dialog
      const dialogRef = this.dialog.open(SysAdminByDomainDialog, dialogConfig);
    
      // Subscribe to the dialog's afterClosed event to handle the result
      dialogRef.afterClosed().subscribe(result => {
        if(result.from === null && result.to === null && result.comment === null){
          this.login_users[loginId][1][env] = false;
          this.show_add = true;
          this.loadDomainUsers(false);
        }
        else if(result.from != "") {
          
          this.login_users[loginId][2][env].sysAdmin = true;
          this.login_users[loginId][2][env].sysAdminStartDate = result.from;
          this.login_users[loginId][2][env].sysAdminEndDate = result.to || null;
          this.login_users[loginId][2][env].comment = result.comment || "";
          this.login_users[loginId][2][env].modifiedBy = 'admin'; // TODO: Replace with the actual login

          this.login_users[loginId][1][env] = true;
    
          if(this.login_users[loginId][2][env].sysAdminEndDate != null)
              this.show_calendar[loginId][env] = true;
          else
              this.show_calendar[loginId][env] = false;

          this.show_add = false;
        }
      });
    } 

    else {

      var new_user = {
        loginId: loginId,
        domainId: this.selected_domain.domainId, 
        userId: "99999999-9999-9999-9999-999999999999", 
        userName: "SYSADMIN",
        environment: env
      };

      dialogConfig.data = {user: new_user, name: this.login_users[loginId][0]};

      // Open the dialog
      const dialogRef = this.dialog.open(SysAdminByDomainDialog, dialogConfig);
    
      // Subscribe to the dialog's afterClosed event to handle the result
      dialogRef.afterClosed().subscribe(result => {
        if(result.from === null && result.to === null && result.comment === null){
          this.login_users[loginId][1][env] = false;
          this.show_add = true;
          this.loadDomainUsers(false);
        }
        else if(result.from != "") {
          
          this.login_users[loginId][2][env] = {
            loginId: loginId,
            domainId: this.selected_domain.domainId, 
            userId: "99999999-9999-9999-9999-999999999999", 
            userName: "SYSADMIN",
            environment: env,
            sysAdmin: true,
            sysAdminStartDate: result.from,
            sysAdminEndDate: result.to || null,
            comment: result.comment || "",
            modifiedBy: 'admin' // TODO: Replace with the actual login
          }
    
          this.login_users[loginId][1][env] = true;
          
          if(this.login_users[loginId][2][env].sysAdminEndDate != null)
              this.show_calendar[loginId][env] = true;
          else
              this.show_calendar[loginId][env] = false;

          this.show_add = false;
        }
      });
    }
  }

  uncheck(loginId: any, env: any): void {
    this.login_users[loginId][2][env].sysAdmin = false;
    this.login_users[loginId][1][env] = false;

    this.show_calendar[loginId][env] = false;
    this.show_add = false;
  }



  // TODO: Open form to add a new sys admin to current domain
  addSysAdmin(): void {
    this.show_add = false;
    this.getSysAdminByDomain(this.selected_domain.domainId, true);
  }

  saveSysAdmin(): void {
    this.show_add = true;
    
    // TODO : send request to backend to save all users in context
    const requests = [];

    for(const loginId of Object.keys(this.login_users)){
      if(this.login_users[loginId].length > 0){
        for(const env of Object.keys(this.login_users[loginId][2])){
          if(this.login_users[loginId][2][env].userId === "99999999-9999-9999-9999-999999999999" && this.login_users[loginId][2][env].sysAdmin === true){
            const request = this.http.put('http://localhost:5050/api/sysadminbydomain', this.login_users[loginId][2][env]);

            requests.push(request);
          }
          else if(this.login_users[loginId][2][env].userId === "99999999-9999-9999-9999-999999999999" && this.login_users[loginId][2][env].sysAdmin === false){
            let user = this.login_users[loginId][2][env];

            const request = this.http.delete('http://localhost:5050/api/sysadminbydomain/' + user.loginId+ '+' +user.userId+ '+' +user.domainId+ '+' +user.environment);
            requests.push(request);
          }
        }
      }
    }

    forkJoin(requests).subscribe(
      () => {
        // All requests completed successfully
        this.getSysAdminByDomain(this.selected_domain.domainId, false);
      },
      (error) => {
        alert('Connection error: ' + error.message);
      }
    );
  }

  cancelSysAdmin(): void {
    this.show_add = true;

    this.getSysAdminByDomain(this.selected_domain.domainId, false);
  }



  /**
   * Updates the one_checked dictionnary.
   */
  updateOneChecked() {
    this.one_checked = {1: this.oneChecked(1), 2: this.oneChecked(2),
      3: this.oneChecked(3), 4: this.oneChecked(4), 
      5: this.oneChecked(5), 6: this.oneChecked(6)};
  }

  /**
   * Checks if at least one user is checked for a given environment.
   * 
   * @param env - The environment to check.
   * @returns True if at least one user is checked for the given environment, false otherwise.
   */
  oneChecked(env: any): boolean {
    for (const loginId of Object.keys(this.login_users)) {
      if(this.login_users[loginId].length > 0){
        if(this.login_users[loginId][2][env])
          if(this.login_users[loginId][2][env].userId === "99999999-9999-9999-9999-999999999999" && 
                                                              this.login_users[loginId][2][env].sysAdmin === true){
            return true;
          }
      }
    }
    return false;
  }

  /**
   * Unchecks all login users for a given environment.
   * @param env - The environment to uncheck for.
   */
  uncheckAll(env: any): void {
    this.show_add = false;
    for (const loginId of Object.keys(this.login_users)) {
      if (this.login_users[loginId].length > 0) {
        if (this.login_users[loginId][1][env]) {
          this.login_users[loginId][2][env].sysAdmin = false;
          this.login_users[loginId][1][env] = false;
        }
      }
    }

    this.data_domain_table = [];
    for (const login of this.logins) {
      var line = [login.loginId ,(this.login_users[login.loginId][0])]
      if(this.login_users[login.loginId][1]){
        for(var i=1; i<7; i++){
          line.push(this.login_users[login.loginId][1][i]);
        }
        this.data_domain_table.push(line);
      }
    }

    this.data_source = new MatTableDataSource(this.data_domain_table);
    this.data_source.paginator = this.paginator;
    this.data_source.sort = this.sort;
  }

  /**
   * Checks all login users for a given environment.
   * @param env - The environment to check for.
   */
  checkAll(env: any): void {
    this.show_add = false; 
    // TODO : ask if it is all users or only the users that are shown in the table

    for (const loginId of Object.keys(this.login_users)) {
      if (this.login_users[loginId].length > 0) {
        const currentDate = new Date();
        
        this.login_users[loginId][2][env] = {
          loginId: loginId,
          domainId: this.selected_domain.domainId, 
          userId: "99999999-9999-9999-9999-999999999999", 
          userName: "SYSADMIN",
          environment: env,
          sysAdmin: true,
          sysAdminStartDate: this.datepipe.transform(currentDate, 'yyyy-MM-dd'),
          sysAdminEndDate: null,
          comment: "given all users in this environment sys admin rights",
          modifiedBy: 'admin'
        };
        
        this.login_users[loginId][1][env] = true;

      }
    }

    this.data_domain_table = [];
    for (const login of this.logins) {
      var line = [login.loginId ,(this.login_users[login.loginId][0])]
      if(this.login_users[login.loginId][1]){
        for(var i=1; i<7; i++){
          line.push(this.login_users[login.loginId][1][i]);
        }
        this.data_domain_table.push(line);
      }
    }

    this.data_source = new MatTableDataSource(this.data_domain_table);
    this.data_source.paginator = this.paginator;
    this.data_source.sort = this.sort;
  }

  /**
   * Checks if a user has been given sys admin rights for a given environment.
   * @param loginId - The login id of the user to check.
   * @param env - The environment to check for.
   * @returns True if the user has been given sys admin rights for the given environment, false otherwise.
   */
  isModified(loginId: any, env: any): boolean {
    return this.show_calendar[loginId][env];
  }
}
