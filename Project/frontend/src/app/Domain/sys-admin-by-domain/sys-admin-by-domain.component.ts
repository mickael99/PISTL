import { HttpClient } from '@angular/common/http';
import { Component, Renderer2, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { SysAdminByDomainDialog } from './sys-admin-by-domain-dialog.component'; // Replace with the actual path
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import {MatDatepicker, MatDatepickerModule} from '@angular/material/datepicker';

@Component({
  selector: 'app-sys-admin-by-domain',
  templateUrl: './sys-admin-by-domain.component.html',
  styleUrls: ['./sys-admin-by-domain.component.css'],
})
export class SysAdminByDomainComponent {
  domains:  { [domainId: number]: any } = {};
  logins: any[] = [];
  users: any[] = [];
  login_users: { [loginId: number]: any } = {};
  dataDomainTable: any[] = [];
  selectedDomain: any;

  showAdd = true;

  dataSource: any;
  displayedColumns: string[] = ['email', 'dev', 'preprod', 'prod', 'test', 'prodCopy', 'staging'];
  @ViewChild(MatPaginator) paginator !: MatPaginator;
  @ViewChild(MatSort) sort !: MatSort;

  constructor(private http: HttpClient, private dialog: MatDialog) { this.loadSysAdmin(); }

  loadSysAdmin(): void {
    this.domains = {};
    this.logins = [];
    this.users = [];

    this.http.get('http://localhost:5050/api/sysadminbydomain').subscribe(
      (data: any) => {
        for (const domain of data.domains) {
          console.log(domain.domainId)
          this.domains[domain.domainId] = domain;
        }
        this.selectedDomain = this.domains[351];

        for (const login of data.logins) {
          this.logins.push(login);
        }

        for (const user of data.users) {
          if(user.userId === "99999999-9999-9999-9999-999999999999")
            this.users.push(user);
        }

        this.onChange({value: this.selectedDomain}, false);
      },
      (error) => {
        alert('Connection error: ' + error.message);
      }
    );
  }

  loadAll(): void {
    this.users = [];
    this.http.get('http://localhost:5050/api/sysadminbydomain').subscribe(
      (data: any) => {
        for (const user of data.users) {
          this.users.push(user);
        }

        this.onChange({value: this.selectedDomain}, true);
      },
      (error) => {
        alert('Connection error: ' + error.message);
      }
    );
  }

  onChange(event: any, all: boolean): void {
    this.selectedDomain = event.value;

    var login_object = [];
    for (const login of this.logins) {
      login_object = [];
      var tab_users = {};
      for (const user of this.users) {
        if(user.loginId === login.loginId && user.domainId === this.selectedDomain.domainId){
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

    this.dataDomainTable = [];
    for (const login of this.logins) {
      var line = [login.loginId ,(this.login_users[login.loginId][0])]
      if(this.login_users[login.loginId][1]){
        for(var i=1; i<7; i++){
          line.push(this.login_users[login.loginId][1][i]);
        }
        this.dataDomainTable.push(line);
      }
    }
    
    console.log("Login_users: ", this.login_users);
    console.log("DataDomainTable: ", this.dataDomainTable);

    this.dataSource = new MatTableDataSource(this.dataDomainTable);
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }

  check(loginId: any, env: any): void {
    const dialogConfig = new MatDialogConfig();
    dialogConfig.disableClose = true;
    dialogConfig.autoFocus = true;

    console.log(loginId, env);
    console.log(this.login_users[loginId]);
    
    let user = this.login_users[loginId][2][env];
    if(user){
      console.log("User found");

      dialogConfig.data = user;
      // Open the dialog
      const dialogRef = this.dialog.open(SysAdminByDomainDialog, dialogConfig);

      user.sysAdmin = !user.sysAdmin;
      // Subscribe to the dialog's afterClosed event to handle the result
      dialogRef.afterClosed().subscribe(result => {
        if (result.from != "" && result.to != "" && result.comment != "") {
          this.http.put('http://localhost:5050/api/sysadminbydomain', {
            loginId: result.user.loginId,
            domainId: result.user.domainId,
            userId: result.user.userId,
            environment: result.user.environment,
            sysAdmin: result.user.sysAdmin,
            sysAdminStartDate: result.from,
            sysAdminEndDate: result.to,
            comment: result.comment,
            modifiedBy: 'admin', // TODO: Replace with the actual login
          }).subscribe(
            (data: any) => {
              console.log('Dialog result:', data);
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

      user = {
        loginId: loginId,
        domainId: this.selectedDomain.domainId, 
        userId: "99999999-9999-9999-9999-999999999999", 
        environment: env
      };

      dialogConfig.data = user;
      console.log('Dialog data:', dialogConfig.data);

      // Open the dialog
      const dialogRef = this.dialog.open(SysAdminByDomainDialog, dialogConfig);
    
      // Subscribe to the dialog's afterClosed event to handle the result
      dialogRef.afterClosed().subscribe(result => {
        if(result.from != "" && result.to != "" && result.comment != "") {
          console.log('Dialog result:', result);
          this.http.post('http://localhost:5050/api/sysadminbydomain', {
            loginId: result.user.loginId,
            domainId: result.user.domainId,
            userId: "99999999-9999-9999-9999-999999999999",
            environment: result.user.environment,
            sysAdmin: true,
            sysAdminStartDate: result.from,
            sysAdminEndDate: result.to,
            comment: result.comment,
            modifiedBy: 'admin', // TODO: Replace with the actual login
          }).subscribe(
            (data: any) => {
              console.log('Dialog result:', data);
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
    const dialogConfig = new MatDialogConfig();
    dialogConfig.disableClose = true;
    dialogConfig.autoFocus = true;

    let user = this.login_users[loginId][2][env];

    dialogConfig.data = user;
    console.log('Dialog data:', dialogConfig.data);

    // Open the dialog
    const dialogRef = this.dialog.open(SysAdminByDomainDialog, dialogConfig);

    dialogRef.afterClosed().subscribe(result => {
      if(result.from != "" && result.to != "" && result.comment != "") {
        console.log('Dialog result:', result);
        this.http.delete('http://localhost:5050/api/sysadminbydomain', {
          body: {
            loginId: result.user.loginId,
            domainId: result.user.domainId,
            userId: result.user.userId,
            environment: result.user.environment,
            sysAdmin: result.user.sysAdmin,
            sysAdminStartDate: result.from,
            sysAdminEndDate: result.to,
            comment: result.comment,
            modifiedBy: 'admin', // TODO: Replace with the actual login
          }
        }).subscribe(
          (data: any) => {
            console.log('Dialog result:', data);
          },
          (error) => {  
            alert('Connection error: ' + error.message);
          }
        );
      }
    });
  }

  // TODO: Open form to add a new sys admin to current domain
  addSysAdmin(user: any): void {
    this.showAdd = false;
    console.log('Add sys admin');
    this.loadAll();
  }

  saveSysAdmin(): void {
    this.showAdd = true;
    console.log('Save sys admin');
    this.loadSysAdmin();
  }

  cancelSysAdmin(): void {
    this.showAdd = true;
    console.log('Cancel sys admin');
    this.loadSysAdmin();
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
          console.log('oneChecked');
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
  unCheckAll(env: any): void {
    console.log('unCheckAll');
    for (const id of Object.keys(this.dataDomainTable)) {
      if(this.dataDomainTable[id].length > 0){
        this.dataDomainTable[id][env+2] = false;
        let loginId = this.dataDomainTable[id][0];
        let user = this.login_users[loginId][2][env-1];
        console.log(user);
      }
    }
  }
}
