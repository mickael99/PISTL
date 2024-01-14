import { HttpClient } from '@angular/common/http';
import { Component, Renderer2, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { SysAdminByDomainDialog } from './sys-admin-by-domain-dialog.component'; // Replace with the actual path
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { DatePipe } from '@angular/common';
import { forkJoin, timeout } from 'rxjs';

@Component({
  selector: 'app-sys-admin-by-domain',
  templateUrl: './sys-admin-by-domain.component.html',
  styleUrls: ['./sys-admin-by-domain.component.css'],
})
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
        console.log(data);
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
    var login_object = [];
    for (const login of this.logins) {
      login_object = [];
      var tab_users = {};
      for (const user of this.users) {
        if(user.loginId === login.loginId && user.domainId === this.selected_domain.domainId){
          tab_users[user.environment] = user;
        }
      }
      if(Object.keys(tab_users).length > 0){
        login_object.push(login.email);
        var user_env = {1: false, 2: false, 3: false, 4: false, 5: false, 6: false};
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
        login_object.push(login.email);
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
      for (const login of this.logins) {
        this.show_calendar[login.loginId] = {};
        for(var i=1; i<7; i++){
          this.show_calendar[login.loginId][i] = false;
        }
      } 
    }

    console.log("Login_users: ", this.login_users);
    console.log("DataDomainTable: ", this.data_domain_table);

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

    console.log(loginId, env);
    console.log(this.login_users[loginId]);
    
    if(this.login_users[loginId][2][env]){
      console.log("User found");

      dialogConfig.data = this.login_users[loginId][2][env];

      // Open the dialog
      const dialogRef = this.dialog.open(SysAdminByDomainDialog, dialogConfig);
    
      // Subscribe to the dialog's afterClosed event to handle the result
      dialogRef.afterClosed().subscribe(result => {
        if(result.from != "" && result.comment != "") {
          console.log(result)
          const currentDate = new Date();
          const futureDate = new Date();
          futureDate.setFullYear(currentDate.getFullYear() + 100);

          this.http.put('http://localhost:5050/api/sysadminbydomain', {
            loginId: result.user.loginId,
            domainId: result.user.domainId,
            userId: "99999999-9999-9999-9999-999999999999",
            environment: result.user.environment,
            sysAdmin: true,
            sysAdminStartDate: result.from,
            sysAdminEndDate: result.to || this.datepipe.transform(futureDate, 'yyyy-MM-dd'),
            comment: result.comment,
            modifiedBy: 'admin', // TODO: Replace with the actual login
          }).subscribe(
            async (data: any) => {
              console.log('Dialog result:', data);
              if (this.show_add) {
                this.getSysAdminByDomain(this.selected_domain.domainId, false);
              }
            },
            (error) => {  
              alert('Connection error: ' + error.message);
            }
          );
        }
      });
    } 

    else {
      console.log("User not found");

      this.login_users[loginId][2][env] = {
        loginId: loginId,
        domainId: this.selected_domain.domainId, 
        userId: "99999999-9999-9999-9999-999999999999", 
        environment: env
      };

      dialogConfig.data = this.login_users[loginId][2][env];

      // Open the dialog
      const dialogRef = this.dialog.open(SysAdminByDomainDialog, dialogConfig);
    
      // Subscribe to the dialog's afterClosed event to handle the result
      dialogRef.afterClosed().subscribe(result => {
        if(result.from != "" && result.comment != "") {
          console.log(result)
          const currentDate = new Date();
          const futureDate = new Date();
          futureDate.setFullYear(currentDate.getFullYear() + 100);

          this.http.put('http://localhost:5050/api/sysadminbydomain', {
            loginId: result.user.loginId,
            domainId: result.user.domainId,
            userId: "99999999-9999-9999-9999-999999999999",
            environment: result.user.environment,
            sysAdmin: true,
            sysAdminStartDate: result.from,
            sysAdminEndDate: result.to || this.datepipe.transform(futureDate, 'yyyy-MM-dd'),
            comment: result.comment,
            modifiedBy: 'admin', // TODO: Replace with the actual login
          }).subscribe(
            async (data: any) => {
              console.log('Dialog result:', data);
              if (this.show_add) {
                this.getSysAdminByDomain(this.selected_domain.domainId, false);
              }
              
              this.show_calendar[loginId][env] = true;
              console.log(this.show_calendar);
            },
            (error) => {  
              alert('Connection error: ' + error.message);
            }
          );
        }
      });
    }
  }

  uncheck(loginId: any, env: any): void {

    let user = this.login_users[loginId][2][env];

    this.http.delete('http://localhost:5050/api/sysadminbydomain/' + user.loginId+ '+' +user.userId+ '+' +user.domainId+ '+' +user.environment, {
    }).subscribe(
      (data: any) => {
        console.log('Dialog result:', data);
        if(this.show_add) this.getSysAdminByDomain(this.selected_domain.domainId, false);
      },
      (error) => {  
        alert('Connection error: ' + error.message);
      }
    );
  }



  // TODO: Open form to add a new sys admin to current domain
  addSysAdmin(): void {
    this.show_add = false;
    console.log('Add sys admin');
    this.getSysAdminByDomain(this.selected_domain.domainId, true);
  }

  saveSysAdmin(): void {
    this.show_add = true;
    console.log('Save sys admin');
    // TODO : save current userDTOS into context 

    this.getSysAdminByDomain(this.selected_domain.domainId, false);
  }

  cancelSysAdmin(): void {
    this.show_add = true;
    console.log('Cancel sys admin');

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
    for (const login of Object.keys(this.login_users)) {
      if(this.login_users[login].length > 0){
        if(this.login_users[login][1][env]){
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
    console.log('unCheckAll');
    const requests = [];
    for (const id of Object.keys(this.login_users)) {
      if (this.login_users[id].length > 0) {
        if (this.login_users[id][1][env]) {
          const user = this.login_users[id][2][env];
          const request = this.http.delete('http://localhost:5050/api/sysadminbydomain/' + user.loginId
                                                                                         + '+' + user.userId
                                                                                         + '+' + user.domainId
                                                                                         + '+' + user.environment);
          requests.push(request);
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
   * Checks all login users for a given environment.
   * @param env - The environment to check for.
   */
  checkAll(env: any): void {
    console.log('checkAll');
    console.log(this.login_users);
    
    const requests = [];

    for (const id of Object.keys(this.login_users)) {
      const request = this.http.put('http://localhost:5050/api/sysadminbydomain', {
        loginId: id,
        domainId: this.selected_domain.domainId,
        userId: "99999999-9999-9999-9999-999999999999",
        environment: env,
        sysAdmin: true,
        sysAdminStartDate: this.datepipe.transform(new Date(), 'yyyy-MM-dd'),
        sysAdminEndDate: this.datepipe.transform(new Date().getFullYear() + 100, 'yyyy-MM-dd'),
        comment: "given rights to all users",
        modifiedBy: 'admin', // TODO: Replace with the actual login
      });
      requests.push(request);
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
   * Checks if a user has been given sys admin rights for a given environment.
   * @param loginId - The login id of the user to check.
   * @param env - The environment to check for.
   * @returns True if the user has been given sys admin rights for the given environment, false otherwise.
   */
  isModified(loginId: any, env: any): boolean {
    return this.show_calendar[loginId][env];
  }



  /**
   * Translate the environment number into a string.
   */
  getEnv(env: any): string {
    switch (env) {
      case 0:
        return 'Dev';
      case 1:
        return 'Preprod';
      case 2:
        return 'Prod';
      case 3:
        return 'Test';
      case 4:
        return 'ProdCopy';
      case 5:
        return 'Staging';
      default:
        return '';
    }
  }
}
