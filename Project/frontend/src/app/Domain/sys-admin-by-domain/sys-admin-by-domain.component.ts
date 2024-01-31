import { HttpClient } from '@angular/common/http';
import { Component, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { SysAdminByDomainDialog } from './sys-admin-by-domain-dialog.component';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { DatePipe } from '@angular/common';
import { forkJoin } from 'rxjs';

/**
 * Component for managing system administrators by domain.
 */
@Component({
  selector: 'app-sys-admin-by-domain',
  templateUrl: './sys-admin-by-domain.component.html',
  styleUrls: ['./sys-admin-by-domain.component.css'],
})

/**
 * Component for managing system administrators by domain.
 */
export class SysAdminByDomainComponent {
  // domains : dictionnary key is domain_id and value is the domain object
  domains:  any[] = [];
  // logins : array of login objects
  logins: any[] = [];
  // users : array of user objects
  users: any[] = [];
  // connectedUser : the email of the connected user
  connectedUser: string | null = localStorage.getItem('email') || null;

  // dictionnary key is loginId and value is an triplet (login.email, array of sysAdmin rights by env, array of admin users)
  login_users: { [login_id: number]: any } = {};
  // data_domain_table : data displayed in the table
  data_domain_table: any[] = [];
  // selected domain : the domain selected in the dropdown
  selected_domain: any;

  // one_checked : dictionnary key is env and value is a boolean indicating if at least one user is checked for this env
  one_checked: {[env: number]: boolean} = {1: false, 2: false, 3: false, 4: false, 5: false, 6: false};
  // all_checked : dictionnary key is env and value is a boolean indicating if all users are checked for this env
  all_checked: {[env: number]: boolean} = {1: false, 2: false, 3: false, 4: false, 5: false, 6: false};

  // show_add : boolean indicating if the add button is shown or not
  show_add: boolean = true;
  // show_calendar : dictionnary key is loginId and value is a dictionnary key is env and value is a boolean indicating if the calendar is shown or not
  show_calendar: { [login_id: number]: any } = {};

  // data_source : data source for the table
  data_source: any;
  // displayed_columns : array of the columns to display in the table
  displayed_columns: string[] = ['email', 'dev', 'preprod', 'prod', 'test', 'prodCopy', 'staging'];
  // paginator : paginator object for the table
  @ViewChild(MatPaginator) paginator !: MatPaginator;
  // sort : sort object for the table
  @ViewChild(MatSort, {static: true}) sort : MatSort;
  // datepipe : datepipe object for formatting dates
  datepipe: DatePipe = new DatePipe('en-US');

  /**
   * Constructs a new instance of the SysAdminByDomainComponent.
   * @param http - The HttpClient used for making HTTP requests.
   * @param dialog - The MatDialog used for displaying dialogs.
   */
  constructor(private http: HttpClient, private dialog: MatDialog) {
    console.log('connectedUser', this.connectedUser);
    this.getSysAdminByDomain(351, false);
  }

  /**
   * Retrieves the system administrator by domain.
   * 
   * @param domain_id - The ID of the domain.
   * @param all - A boolean indicating whether to retrieve all domain users.
   */
  getSysAdminByDomain(domain_id: any, all: boolean): void {
    this.domains = [];
    this.logins = [];
    this.users = [];

    this.http.get('http://localhost:5050/api/sysadminbydomain/' + domain_id).subscribe(
      (data: any) => {
        data.domains.sort((a: any, b: any) => { return a.name.localeCompare(b.name); });
        for (const domain of data.domains) {
          this.domains.push(domain);
          if (domain.domainId === domain_id) {
            this.selected_domain = domain;
          }
        }


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

  /**
   * Loads domain users based on the specified criteria.
   * 
   * @param all - A boolean value indicating whether to load all domain users or not.
   */
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

    this.data_domain_table.sort((a, b) => {
      const nameA = a[1].toLowerCase();
      const nameB = b[1].toLowerCase();
      if (nameA < nameB) {
        return -1;
      }
      if (nameA > nameB) {
        return 1;
      }
      return 0;
    });

    

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
    this.data_source.sortingDataAccessor = (item, property) => {
      switch(property) {
        case 'email': return item[0];
        case 'dev': return item[2];
        case 'preprod': return item[3];
        case 'prod': return item[4];
        case 'test': return item[5];
        case 'prodCopy': return item[6];
        case 'staging': return item[7];
        default: return item[0];
      }
    };
    this.data_source.sort = this.sort;
  }

  /**
   * Handles the change of the selected domain.
   * @param event - The event object.
   * @returns void
   */
  onChange(event: any): void {
    this.selected_domain = event.value;
    this.show_calendar = {};
    this.getSysAdminByDomain(this.selected_domain.domainId, false);
  }

  /**
   * Called when a checkbox is checked :
   * - if the user is not in the database, open a dialog to add him
   * - if the user is in the database, open a dialog to modify his sys admin rights
   * @param domainId - The ID of the domain.
   * @param env - The environment.
   */
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
          localStorage.setItem('show_add_sysadmin', 'true');
          this.loadDomainUsers(false);
        }
        else if(result.from != "") {
          this.login_users[loginId][2][env].sysAdmin = true;
          this.login_users[loginId][2][env].sysAdminStartDate = result.from;
          this.login_users[loginId][2][env].sysAdminEndDate = result.to || null;
          this.login_users[loginId][2][env].comment = result.comment || "";
          this.login_users[loginId][2][env].modifiedBy = this.connectedUser; // TODO: Replace with the actual login

          this.login_users[loginId][1][env] = true;
    
          if(this.login_users[loginId][2][env].sysAdminEndDate != null)
              this.show_calendar[loginId][env] = true;
          else
              this.show_calendar[loginId][env] = false;

          this.show_add = false;
          localStorage.setItem('show_add_sysadmin', 'false');
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
          localStorage.setItem('show_add_sysadmin', 'true');
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
            modifiedBy: this.connectedUser
          }
    
          this.login_users[loginId][1][env] = true;
          
          if(this.login_users[loginId][2][env].sysAdminEndDate != null)
              this.show_calendar[loginId][env] = true;
          else
              this.show_calendar[loginId][env] = false;

          this.show_add = false;
          localStorage.setItem('show_add_sysadmin', 'false');
        }
      });
    }
  }

  /**
   * Unchecks the sysAdmin and login checkboxes for a specific loginId and environment.
   * @param loginId - The ID of the login.
   * @param env - The environment.
   */
  uncheck(loginId: any, env: any): void {
    this.login_users[loginId][2][env].sysAdmin = false;
    this.login_users[loginId][1][env] = false;

    this.show_calendar[loginId][env] = false;
    this.show_add = false;
    localStorage.setItem('show_add_sysadmin', 'false');
  }

  /**
   * Called when the user clicks on the 'Add' button.
   * Sets the 'show_add_sysadmin' flag in the local storage to false.
   * Shows all users in the table.
   */
  addSysAdmin(): void {
    this.show_add = false;
    localStorage.setItem('show_add_sysadmin', 'false');
    this.getSysAdminByDomain(this.selected_domain.domainId, true);
  }

  /**
   * Saves the system administrator by sending a request to the backend to save all users in context.
   * It iterates through the login users and sends PUT or DELETE requests based on the user's sysAdmin status.
   * After all requests are completed successfully, it retrieves the system administrator by domain.
   */
  saveSysAdmin(): void {
    this.show_add = true;
    localStorage.setItem('show_add_sysadmin', 'true');
    
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

  /**
   * Cancel the operation of adding a system administrator.
   * Sets the 'show_add_sysadmin' flag in the local storage to true.
   * Retrieves the system administrators for the selected domain.
   */
  cancelSysAdmin(): void {
    this.show_add = true;
    localStorage.setItem('show_add_sysadmin', 'true');

    this.getSysAdminByDomain(this.selected_domain.domainId, false);
  }

  /**
   * Updates the one_checked dictionnary and the all_checked dictionnary.
   */
  updateOneChecked() {
    this.one_checked = {1: this.oneChecked(1), 2: this.oneChecked(2),
      3: this.oneChecked(3), 4: this.oneChecked(4), 
      5: this.oneChecked(5), 6: this.oneChecked(6)};
    this.all_checked = {1: this.allChecked(1), 2: this.allChecked(2),
      3: this.allChecked(3), 4: this.allChecked(4),
      5: this.allChecked(5), 6: this.allChecked(6)}
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
   * Checks if all users are checked for a given environment.
   * @param env - The environment to check.
   * @returns True if all users are checked for the given environment, false otherwise.
   */
  allChecked(env: any): boolean {
    for (const loginId of Object.keys(this.login_users)) {
      if(this.login_users[loginId].length == 0) 
        return false;
      if(this.login_users[loginId].length > 0){
        if(this.login_users[loginId][2][env] == null)
          return false;
      }
    }
    return true;
  }

  /**
   * Unchecks all login users for a given environment.
   * @param env - The environment to uncheck for.
   */
  uncheckAll(env: any): void {
    this.show_add = false;
    localStorage.setItem('show_add_sysadmin', 'false');
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
    this.data_source.sortingDataAccessor = (item, property) => {
      switch(property) {
        case 'email': return item[0];
        case 'dev': return item[2];
        case 'preprod': return item[3];
        case 'prod': return item[4];
        case 'test': return item[5];
        case 'prodCopy': return item[6];
        case 'staging': return item[7];
        default: return item[0];
      }
    }
    this.data_source.sort = this.sort;

    this.updateOneChecked();
  }

  /**
   * Checks all login users for a given environment.
   * @param env - The environment to check for.
   */
  checkAll(env: any): void {
    this.show_add = false; 
    localStorage.setItem('show_add_sysadmin', 'false');
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
          modifiedBy: this.connectedUser
        };
        
        this.login_users[loginId][1][env] = true;

      }

      this.updateOneChecked();
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
    this.data_source.sortingDataAccessor = (item, property) => {
      switch(property) {
        case 'email': return item[0];
        case 'dev': return item[2];
        case 'preprod': return item[3];
        case 'prod': return item[4];
        case 'test': return item[5];
        case 'prodCopy': return item[6];
        case 'staging': return item[7];
        default: return item[0];
      }
    }
    this.data_source.sort = this.sort;
  }

  /**
   * Checks if a user has been given sys admin rights for a given environment.
   * @param loginId - The login id of the user to check.
   * @param env - The environment to check for.
   * @returns True if the user has been given sys admin rights for the given environment, false otherwise.
   */
  isModified(loginId: any, env: any): boolean {
    if(Object.keys(this.show_calendar).length > 0){
      return this.show_calendar[loginId][env];
    }
    return false;
  }
}
