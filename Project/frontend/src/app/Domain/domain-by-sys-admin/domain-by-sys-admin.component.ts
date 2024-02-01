import { HttpClient } from '@angular/common/http';
import { Component, ViewChild} from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { DomainBySysAdminComponentDialog } from './domain-by-sys-admin-dialog.component';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { DatePipe } from '@angular/common';
import { forkJoin } from 'rxjs/internal/observable/forkJoin';

/**
 * Component for displaying domain information by system administrator.
 */
@Component({
  selector: 'app-domain-by-sys-admin',
  templateUrl: './domain-by-sys-admin.component.html',
  styleUrls: ['./domain-by-sys-admin.component.css'],
})
/**
 * Represents a component for managing domain information by a system administrator.
 */
export class DomainBySysAdminComponent {
  // domains : list of domains
  domains:  any[] = [];
  // users : list of users
  users:  any[] = [];
  // logins : list of logins
  logins: any[] = [];
  // data_login_table : data for the table
  data_login_table: any[] = [];
  // connectedUser : the email of the connected user
  connectedUser = localStorage.getItem('email');
  // selected_login : the login selected in the dropdown
  selected_login: any;
  // domain_users : list of domain users
  domain_users: {[domainId: number]: any} = {};
  // one_checked : list of boolean values indicating whether at least one user is checked for a given environment
  one_checked: {[env: number]: boolean} = {1: false, 2: false, 3: false, 4: false, 5: false, 6: false};
  // all_checked : list of boolean values indicating whether all users are checked for a given environment
  all_checked: {[env: number]: boolean} = {1: false, 2: false, 3: false, 4: false, 5: false, 6: false};

  // show_add : boolean value indicating whether to show the add button or not
  show_add = true;
  addButton: any;
  saveButton: any;
  cancelButton: any;
  // show_calendar : list of boolean values indicating whether to show the calendar for a given environment or not
  show_calendar: { [domainId: number]: any } = {};

  // dataSource : data source for the table
  dataSource: any;
  // displayedColumns : list of displayed columns
  displayedColumns: string[] = ['name', 'dev', 'preprod', 'prod', 'test', 'prodCopy', 'staging'];
  // paginator : paginator for the table
  @ViewChild(MatPaginator) paginator !: MatPaginator;
  // sort : sort for the table
  @ViewChild(MatSort, {static: true}) sort : MatSort;
  // datepipe : date pipe for formatting dates
  datepipe: DatePipe = new DatePipe('en-US');

  /**
   * Constructs a new instance of the DomainBySysAdminComponent.
   * @param http - The HttpClient used for making HTTP requests.
   * @param dialog - The MatDialog used for displaying dialogs.
   */
  constructor(private http: HttpClient, private dialog: MatDialog) {
    this.getDomainBySysAdmin(1, false);
  }

  /**
   * Retrieves the domain information for a system administrator.
   * 
   * @param login_id The login ID of the system administrator.
   * @param all Determines whether to retrieve all domain information or not. Default is true.
   */
  getDomainBySysAdmin(login_id: any, all: boolean = true): void {
    this.domains = [];
    this.logins = [];
    this.users = [];

    this.http.get('http://localhost:5050/api/domainbysysadmin/' + login_id).subscribe(
      (data: any) => {
        data.logins.sort((a: any, b: any) => a.email.localeCompare(b.email));

        for(const login of data.logins){
          this.logins.push(login);
          if(login.loginId === login_id)
            this.selected_login = login;
        }
        if(this.selected_login === undefined && data.logins.length > 0) {
          this.selected_login = data.logins[0];
        }

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

  /**
   * Handles the change of the selected login user.
   * @param event - The event object.
   * @returns void
   */
  onChange(event: any): void {
    this.selected_login = event.value;
    this.show_calendar = {};
    this.getDomainBySysAdmin(this.selected_login.loginId, false);
  }

  /**
   * Loads domain users based on the specified criteria.
   * 
   * @param all - A boolean value indicating whether to load all domain users or not.
   */
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

    this.data_login_table.sort((a, b) => {
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
    this.dataSource.sortingDataAccessor = (item, property) => {
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
    this.dataSource.sort = this.sort;
  }

  /**
   * Called when a checkbox is checked :
   * - if the user is not in the database, open a dialog to add him
   * - if the user is in the database, open a dialog to modify his sys admin rights
   * @param domainId - The ID of the domain.
   * @param env - The environment.
   */
  check(domainId: any, env: any): void {
    const dialogConfig = new MatDialogConfig();
    dialogConfig.disableClose = true;
    dialogConfig.autoFocus = true;

    if(this.domain_users[domainId][2][env]){
      dialogConfig.data = {user: this.domain_users[domainId][2][env], name: this.domain_users[domainId][0]};

      const dialogRef = this.dialog.open(DomainBySysAdminComponentDialog, dialogConfig);

      dialogRef.afterClosed().subscribe(result => {

        if(result.from === null && result.to === null && result.comment === null){
          this.domain_users[domainId][1][env] = false;
          this.show_add = true;
          localStorage.setItem('show_add_domainby', 'true');
          this.loadDomainUsers(false);
        }

        else if (result.from != "") {
          
          this.domain_users[domainId][2][env].sysAdmin = true;
          this.domain_users[domainId][2][env].sysAdminStartDate = result.from;
          this.domain_users[domainId][2][env].sysAdminEndDate = result.to || null;
          this.domain_users[domainId][2][env].comment = result.comment || "";
          this.domain_users[domainId][2][env].modifiedBy = this.connectedUser;

          this.domain_users[domainId][1][env] = true;

          if(this.domain_users[domainId][2][env].sysAdminEndDate != null)
            this.show_calendar[domainId][env] = true;
          else
            this.show_calendar[domainId][env] = false;

          this.show_add = false;
          localStorage.setItem('show_add_domainby', 'false');
        }
      });
    }

    else {

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
          this.domain_users[domainId][1][env] = false;
          this.show_add = true;
          localStorage.setItem('show_add_domainby', 'true');
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
            modifiedBy: this.connectedUser
          };

          this.domain_users[domainId][1][env] = true;

          if(this.domain_users[domainId][2][env].sysAdminEndDate != null)
            this.show_calendar[domainId][env] = true;
          else
            this.show_calendar[domainId][env] = false;

          this.show_add = false;
          localStorage.setItem('show_add_domainby', 'false');
        }
      });
    }
  }

  /**
   * Called when checkbox is unchecked.
   * User is removed from the database.
   * @param domainId - The ID of the domain.
   * @param env - The environment.
   */
  uncheck(domainId: any, env: any): void {
    this.domain_users[domainId][2][env].sysAdmin = false;
    this.domain_users[domainId][1][env] = false;

    this.show_calendar[domainId][env] = false;
    this.show_add = false;
    localStorage.setItem('show_add_domainby', 'false');
  }

  /**
   * Called when Add button clicked.
   * Shows all users in the database.
   */
  addDomain(): void {
    this.show_add = false;
    this.showingAdd();

    localStorage.setItem('show_add_domainby', 'false');
    this.getDomainBySysAdmin(this.selected_login.loginId, true);

    const addButton = document.getElementById('addButton');
    if (addButton) {
      addButton.style.display = 'none';
    }
  }

  /**
   * Called when Save button clicked.
   * Saves all added or deleted users in the database.
   */
  saveDomain(): void {
    this.show_add = true;
    this.showingAdd();

    localStorage.setItem('show_add_domainby', 'true');
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

  /**
   * Cancel the operation of adding a system administrator.
   * Sets the 'show_add_domainby' flag in the local storage to true.
   * Retrieves the system administrators for the selected login.
   */
  cancelDomain(): void {
    this.show_add = true;
    this.showingAdd();

    localStorage.setItem('show_add_domainby', 'true');

    this.getDomainBySysAdmin(this.selected_login.loginId, false);
  }

  /**
   * Updates the values of `one_checked` and `all_checked` properties based on the result of `oneChecked` and `allChecked` methods.
   * Logs the updated values of `one_checked` and `all_checked` to the console.
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
   * Checks if all users are checked for a given environment.
   * @param env - The environment to check.
   * @returns True if all users are checked for the given environment, false otherwise.
   */
  allChecked(env: any): boolean {
    for (const domainId of Object.keys(this.domain_users)) {
      if(this.domain_users[domainId].length == 0)
        return false;
      if(this.domain_users[domainId].length > 0){
        if(this.domain_users[domainId][2][env] == null)
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
    localStorage.setItem('show_add_domainby', 'false');
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
    this.dataSource.sortingDataAccessor = (item, property) => {
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
    this.dataSource.sort = this.sort;
  }

  /**
   * Checks all login users for a given environment.
   * @param env - The environment to check for.
   */
  checkAll(env: any): void {
    this.show_add = false;  
    localStorage.setItem('show_add_domainby', 'false');
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
          modifiedBy: this.connectedUser
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
    this.dataSource.sortingDataAccessor = (item, property) => {
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
    this.dataSource.sort = this.sort;
  }

  /**
   * Checks if a user has been given sys admin rights for a given environment.
   * @param domainId - The login id of the user to check.
   * @param env - The environment to check for.
   * @returns True if the user has been given sys admin rights for the given environment, false otherwise.
   */
  isModified(domainId: any, env: any): boolean {
    if(Object.keys(this.show_calendar).length > 0){
      return this.show_calendar[domainId][env];
    }
    return false;
  }

  showingAdd() {
    if (this.addButton && this.saveButton && this.cancelButton) {
      if (this.show_add == true) {
        this.addButton.style.display = 'block';
        this.saveButton.style.display = 'none';
        this.cancelButton.style.display = 'none';
      } else {
        this.addButton.style.display = 'none';
        this.saveButton.style.display = 'block';
        this.cancelButton.style.display = 'block';
      }
    }
  }
}
