import { HttpClient } from '@angular/common/http';
import { Component, OnInit, ViewChild} from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { DomainBySysAdminComponentDialog } from './domain-by-sys-admin-dialog.component';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { DatePipe } from '@angular/common';
import { forkJoin } from 'rxjs/internal/observable/forkJoin';

@Component({
  selector: 'app-domain-by-sys-admin',
  templateUrl: './domain-by-sys-admin.component.html',
  styleUrls: ['./domain-by-sys-admin.component.css'],
})
export class DomainBySysAdminComponent {
  domains:  any[] = [];
  users:  any[] = [];
  logins: {[login_id: number]: any} = {};
  data_login_table: any[] = [];
  
  selected_login: any;
  domain_users: {[domainId: number]: any} = {};
  one_checked: {[env: number]: boolean} = {1: false, 2: false, 3: false, 4: false, 5: false, 6: false};

  show_add = true;
  show_calendar: { [domainId: number]: any } = {};

  dataSource: any;
  displayedColumns: string[] = ['name', 'dev', 'preprod', 'prod', 'test', 'prodCopy', 'staging'];
  @ViewChild(MatPaginator) paginator !: MatPaginator;
  @ViewChild(MatSort) sort !: MatSort;
  datepipe: DatePipe = new DatePipe('en-US');

  constructor(private http: HttpClient, private dialog: MatDialog) {
    this.getDomainBySysAdmin(26996, false);
  }

  getDomainBySysAdmin(login_id: any, all: boolean = true): void {
    this.domains = [];
    this.logins = {};
    this.users = [];

    this.http.get('http://localhost:5050/api/domainbysysadmin/' + login_id).subscribe(
      (data: any) => {
        //console.log('data', data);
        for(const login of data.logins){
          this.logins[login.loginId] = login;
        }

        this.selected_login = this.logins[login_id];

        for(const domain of data.domains){
          this.domains.push(domain);
        }

        for(const user of data.users){
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

  onChange(event: any): void {
    this.selected_login = event.value;
    this.getDomainBySysAdmin(this.selected_login.loginId, false);
  }

  loadDomainUsers(all: boolean): void {
    var domain_object;
    
    for (const domain of this.domains) {
      domain_object = [];
      var tab_users = {};
      // max 6 users per login for a domain
      for (const user of this.users) {
      if(user.domainId === domain.domainId && user.loginId === this.selected_login.loginId){ 
          tab_users[user.environment] = user;
        }
      }
      if(Object.keys(tab_users).length > 0){
        domain_object.push(domain.name + "["+domain.domainId+"]");
        var user_env = {1: false, 2: false, 3: false, 4: false, 5: false, 6: false};
        for(const user of Object.keys(tab_users)){
          for(var i=1; i<7; i++){
            if(tab_users[user].environment === i && tab_users[user].sysAdmin === true){
              user_env[i] = true;
            }
          }
        }
        domain_object.push(user_env);
        domain_object.push(tab_users);
      }
      else if (all){
        domain_object.push(domain.name + "["+domain.domainId+"]");
        domain_object.push({1: false, 2: false, 3: false, 4: false, 5: false, 6: false});
        domain_object.push({});
      }
      this.domain_users[domain.domainId] = domain_object;
    }

    //console.log("domain_users: ", this.domain_users);

    this.data_login_table = [];
    for (const domain of this.domains) {
      var line = [domain.domainId ,(this.domain_users[domain.domainId][0])]
      if(this.domain_users[domain.domainId][1]){
        for(var i=1; i<7; i++){
          line.push(this.domain_users[domain.domainId][1][i]);
        }
        this.data_login_table.push(line);
      }
    }

    if(Object.keys(this.show_calendar).length === 0){

      for (const domainId of Object.keys(this.domain_users)) {
        this.show_calendar[domainId] = {};
        if(this.domain_users[domainId].length > 0){
          for(const env of Object.keys(this.domain_users[domainId][2])){
            this.show_calendar[domainId][env] = false;
            
            if(this.domain_users[domainId][2][env].sysAdminEndDate != null)
                this.show_calendar[domainId][env] = true;
          }
        }
      } 
    }

    //console.log("Domain_users: ", this.domain_users);
    //console.log('dataDomainTable', this.data_login_table);

    this.dataSource = new MatTableDataSource(this.data_login_table);
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }

  
  check(domainId: any, env: any): void {
    const dialogConfig = new MatDialogConfig();
    dialogConfig.disableClose = true;
    dialogConfig.autoFocus = true;

    if(this.domain_users[domainId][2][env]){
      console.log("User found");

      dialogConfig.data = {user: this.domain_users[domainId][2][env], name: this.domain_users[domainId][0]};

      const dialogRef = this.dialog.open(DomainBySysAdminComponentDialog, dialogConfig);

      dialogRef.afterClosed().subscribe(result => {

        if(result.from === null && result.to === null && result.comment === null){
          console.log("User not added");
          this.domain_users[domainId][1][env] = false;
          this.show_add = true;
          this.loadDomainUsers(false);
        }

        else if (result.from != "") {
          
          this.domain_users[domainId][2][env].sysAdmin = true;
          this.domain_users[domainId][2][env].sysAdminStartDate = result.from;
          this.domain_users[domainId][2][env].sysAdminEndDate = result.to || null;
          this.domain_users[domainId][2][env].comment = result.comment || "";
          this.domain_users[domainId][2][env].modifiedBy = 'admin';

          this.domain_users[domainId][1][env] = true;

          if(this.domain_users[domainId][2][env].sysAdminEndDate != null)
            this.show_calendar[domainId][env] = true;
          else
            this.show_calendar[domainId][env] = false;

          this.show_add = false;
        }
      });
    }

    else {
      console.log("User not found");

      var new_user = {
        domainId: domainId,
        loginId: this.selected_login.loginId, 
        userId: "99999999-9999-9999-9999-999999999999", 
        environment: env
      };

      dialogConfig.data = {user: new_user, name: this.domain_users[domainId][0]};

      const dialogRef = this.dialog.open(DomainBySysAdminComponentDialog, dialogConfig);

      dialogRef.afterClosed().subscribe(result => {

        if(result.from === null && result.to === null && result.comment === null){
          console.log("User not added");
          this.domain_users[domainId][1][env] = false;
          this.show_add = true;
          this.loadDomainUsers(false);
        }

        else if (result.from != "") {
          
          this.domain_users[domainId][2][env] = {
            domainId: domainId,
            loginId: this.selected_login.loginId, 
            userId: "99999999-9999-9999-9999-999999999999", 
            environment: env,
            sysAdmin: true,
            sysAdminStartDate: result.from,
            sysAdminEndDate: result.to || null,
            comment: result.comment || "",
            modifiedBy: 'admin'
          };

          this.domain_users[domainId][1][env] = true;

          if(this.domain_users[domainId][2][env].sysAdminEndDate != null)
            this.show_calendar[domainId][env] = true;
          else
            this.show_calendar[domainId][env] = false;

          this.show_add = false;
        }
      });
    }
  }

  uncheck(domainId: any, env: any): void {
    this.domain_users[domainId][2][env].sysAdmin = false;
    this.domain_users[domainId][1][env] = false;

    this.show_calendar[domainId][env] = false;
    this.show_add = false;
  }

  // TODO: Open form to add a new sys admin to current domain
  addDomain(): void {
    this.show_add = false;
    this.getDomainBySysAdmin(this.selected_login.loginId, true);
  }

  saveDomain(): void {
    this.show_add = true;
    // TODO : send request to backend to save all users in context
    const requests = [];

    for(const domainId of Object.keys(this.domain_users)){
      if(this.domain_users[domainId].length > 0){
        for(const env of Object.keys(this.domain_users[domainId][2])){
          if(this.domain_users[domainId][2][env].userId === "99999999-9999-9999-9999-999999999999" && this.domain_users[domainId][2][env].sysAdmin === true){
            const request = this.http.put('http://localhost:5050/api/sysadminbydomain', this.domain_users[domainId][2][env]);

            requests.push(request);
          }
          else if(this.domain_users[domainId][2][env].userId === "99999999-9999-9999-9999-999999999999" && this.domain_users[domainId][2][env].sysAdmin === false){
            let user = this.domain_users[domainId][2][env];

            const request = this.http.delete('http://localhost:5050/api/sysadminbydomain/' + user.loginId+ '+' +user.userId+ '+' +user.domainId+ '+' +user.environment);
            requests.push(request);
          }
        }
      }
    }

    forkJoin(requests).subscribe(
      () => {
        // All requests completed successfully
        this.getDomainBySysAdmin(this.selected_login.loginId, false);
      },
      (error) => {
        alert('Connection error: ' + error.message);
      }
    );
  }

  cancelDomain(): void {
    this.show_add = true;

    this.getDomainBySysAdmin(this.selected_login.loginId, false);
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
    for (const domainId of Object.keys(this.domain_users)) {
      if(this.domain_users[domainId].length > 0){
        if(this.domain_users[domainId][2][env])
          if(this.domain_users[domainId][2][env].userId === "99999999-9999-9999-9999-999999999999" && 
                                                              this.domain_users[domainId][2][env].sysAdmin === true){
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
    for (const domainId of Object.keys(this.domain_users)) {
      if (this.domain_users[domainId].length > 0) {
        if (this.domain_users[domainId][1][env]) {
          this.domain_users[domainId][2][env].sysAdmin = false;
          this.domain_users[domainId][1][env] = false;
        }
      }
    }

    this.data_login_table = [];
    for (const domain of this.domains) {
      var line = [domain.domainId ,(this.domain_users[domain.domainId][0])]
      if(this.domain_users[domain.domainId][1]){
        for(var i=1; i<7; i++){
          line.push(this.domain_users[domain.domainId][1][i]);
        }
        this.data_login_table.push(line);
      }
    }

    //console.log("domain_users: ", this.domain_users);
    //console.log("data_login_table: ", this.data_login_table);

    this.dataSource = new MatTableDataSource(this.data_login_table);
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }

  /**
   * Checks all login users for a given environment.
   * @param env - The environment to check for.
   */
  checkAll(env: any): void {
    this.show_add = false;  
    // TODO : ask if it is all users or only the users that are shown in the table

    for (const domainId of Object.keys(this.domain_users)) {
      if (this.domain_users[domainId].length > 0) {
        const currentDate = new Date();
        
        this.domain_users[domainId][2][env] = {
          domainId: domainId,
          loginId: this.selected_login.loginId, 
          userId: "99999999-9999-9999-9999-999999999999", 
          environment: env,
          sysAdmin: true,
          sysAdminStartDate: this.datepipe.transform(currentDate, 'yyyy-MM-dd'),
          sysAdminEndDate: null,
          comment: "given all users in this environment sys admin rights",
          modifiedBy: 'admin'
        };
        
        this.domain_users[domainId][1][env] = true;

      }
    }

    this.data_login_table = [];
    for (const domain of this.domains) {
      var line = [domain.domainId ,(this.domain_users[domain.domainId][0])]
      if(this.domain_users[domain.domainId][1]){
        for(var i=1; i<7; i++){
          line.push(this.domain_users[domain.domainId][1][i]);
        }
        this.data_login_table.push(line);
      }
    }
    
    //console.log("domain_users: ", this.domain_users);
    //console.log("DataDomainTable: ", this.data_login_table);

    this.dataSource = new MatTableDataSource(this.data_login_table);
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }

  /**
   * Checks if a user has been given sys admin rights for a given environment.
   * @param domainId - The login id of the user to check.
   * @param env - The environment to check for.
   * @returns True if the user has been given sys admin rights for the given environment, false otherwise.
   */
  isModified(domainId: any, env: any): boolean {
    return this.show_calendar[domainId][env];
  }
}
