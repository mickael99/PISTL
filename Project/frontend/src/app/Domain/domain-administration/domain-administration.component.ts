import { HttpClient } from '@angular/common/http';
import { Component, Renderer2 } from '@angular/core';
import { Router } from '@angular/router';

/***************************************************************************************/
/***************************************************************************************/
/***************************************************************************************/
/* represent a domain linked to an environment */
export class EnvironmentModel {
  public environmentId: number;
  public domainId: number;
  public environment: number;
  public bpwebserverId: number;
  public bpdatabaseId: number;
  public eaidatabaseId: number;
  public ssrsserverId: number;
  public tableauserverId: number;
  public eaiftpserverId: number;
  public isBp5Enabled: boolean;

  public nameBpwebServerId: string;
  public nameBpDatabaseId: string;
  public nameEaiDatabaseId: string;
  public nameSsrsServerId: string;
  public nameTableauServerId: string;
  public nameEaiftpServerId: string;
}

/***************************************************************************************/
export class EnvironmentModelForBackend {
  public environment: number;
  public bpwebServerId: number;
  public bpDatabaseId: number;
  public eaiDatabaseId: number;
  public ssrsServerId: number;
  public tableauServerId: number;
  public eaiftpServerId: number;
  public isBp5Enabled: boolean;
}

@Component({
  selector: 'app-domain-administration',
  templateUrl: './domain-administration.component.html',
  styleUrls: ['./domain-administration.component.css'],
})
export class DomainAdministrationComponent {
  /* data get into the Databases */
  domains: any[];
  domainEnvironments: any[]
  databases: any[];
  servers: any[];

  serversAndDatabasesName: ServersAndDatabasesName;

  /* domain selected from the list */
  selectedDomain: any;
  selectedDomainEnvironments: any[];
  selectedDomainEnvironment: any;

  /*domain fields */
  name: string;
  logo: string;
  edition: string;
  isSsoEnabled: boolean;
  comment: string;
  parentCompany: string;

  /* selected environment among dev, preprod, prod, test, prodcopy and staging*/
  selectedEnvironment: EnvironmentModel;

  /* selected environment while adding or editing */
  idEnvSelected: number;

  /* boolean values to manage the options to create and edit a domain */
  isNewDomainMode: boolean = false;
  isEditDomainMode: boolean = false;

  //bp5 link to each environment
  selectedBp5: { [key: number]: boolean } = {};

  /* selected servers when the DAT user want to edit or create a domain.
   *  Each servers is link to the selected environment by the user
   */
  selectedWebServers: { [key: number]: any } = {};
  selectedEaiFtpServers: { [key: number]: any } = {};
  selectedTableauServers: { [key: number]: any } = {};
  selectedSsrsServers: { [key: number]: any } = {};

  /* selected databases when the DAT user want to edit or create a domain.
   *  Each servers is link to the selected environment by the user
   */
  selectedDatabaseServers: { [key: number]: any } = {};
  selectedElasticPools: { [key: number]: any } = {};

  // Logo used to load to 'this.logo' after the user has selected a img file
  logoToAdd: any;

  /*
   *  Update the selected domain from the list and print the dev information.
   *  If the selected domain is not link to the dev environment, preprod is printed and so on
   *
   * @selectedDomain the selected domain
   */
  onSelect(selectedDomain: any): void {
    this.selectedDomain = selectedDomain;
    if(selectedDomain && selectedDomain.logo)
      this.displayLogo(selectedDomain.logo);
    this.onEnvironmentSelect(2);
  }

  onSelectServer(
    serverType: string,
    selectedServer: any,
    environmentId: number
  ): void {
    switch (serverType) {
      case 'webServer':
        this.selectedWebServers[environmentId] = selectedServer;
        break;
      case 'eaiFtpServer':
        this.selectedEaiFtpServers[environmentId] = selectedServer;
        break;
      case 'tableauServer':
        this.selectedTableauServers[environmentId] = selectedServer;
        break;
      case 'ssrsServer':
        this.selectedSsrsServers[environmentId] = selectedServer;
        break;
      default:
        console.error('Server not recognized:', serverType);
        break;
    }
  }

  onSelectDatabase(
    databaseType: string,
    selectedDatabase: any,
    environmentId: number
  ): void {
    switch (databaseType) {
      case 'databaseServer':
        this.selectedDatabaseServers[environmentId] = selectedDatabase;
        break;
      case 'elasticPool':
        this.selectedElasticPools[environmentId] = selectedDatabase;
        break;
      default:
        console.error('Database not recognized:', databaseType);
        break;
    }
  }

  resetBp5ServersAndDatabasesList(): void {
    const interne = (environment: number): void => {
      this.selectedBp5[environment] = false;
      this.selectedDatabaseServers[environment] = null;
      this.selectedWebServers[environment] = null;
      this.selectedEaiFtpServers[environment] = null;
      this.selectedTableauServers[environment] = null;
      this.selectedSsrsServers[environment] = null;
      this.selectedElasticPools[environment] = null
    }
    
    interne(2);
    interne(4);
    interne(8);
    interne(16);
    interne(32);
    interne(256);
  }

  /***************************************************************************************/
  /*
  * Get domains, servers and databases from the Databases.
  * Select the first occurence into the domain
  */
  constructor(private renderer: Renderer2, private http: HttpClient, private router: Router) {
    this.http.get('http://localhost:5050/api/database').subscribe(
        (data: any) => {
          this.databases = data;
        },
        (error) => {
          alert('Connection error: ' + error.message);
        }
    );
    };

    this.http.get('http://localhost:5050/api/server').subscribe(
        (data: any) => {
          this.servers = data;
        },
        (error) => {
          alert('Connection error: ' + error.message);
        }
    );
    };

    this.serversAndDatabasesName = new ServersAndDatabasesName();

    this.http.get('http://localhost:5050/api/database').subscribe(
      (data: any) => {
        this.domains = data;
        if(this.domains.length)
          setTimeout( () => { this.onSelect(this.domains[0]) }, 500);
      },
      (error) => {
        alert('Connection error: ' + error.message);
      }
    );
  }

  /***************************************************************************************/
  /*
   * Disable the environment radio button if the selected domain is not link
   * to the environment
   *
   * @environmentId the environment that need to check
   *
   * @return true if the domain is link to the environment
   *         false if not
   */
  isEnvironmentEnabled(environmentId: number): boolean {
    return !!this.selectedDomain?.environments?.some(env => env.environment === environmentId);
  }

  findDatabaseNameFromSelectedEnvironment(): void {
    if(this.selectedEnvironment) {
      //console.log("qsnjdfnjqsd -> ", this.databases.length);
      const databaseServer = this.databases.find(db => db.databaseId === this.selectedEnvironment.bpDatabaseId);
      if(databaseServer) 
        this.selectedEnvironment.nameBpDatabaseId = databaseServer.name;

      const elasticPool = this.databases.find(db => db.databaseId === this.selectedEnvironment.eaiDatabaseId);
      if(elasticPool) 
        this.selectedEnvironment.nameEaiDatabaseId = elasticPool.name;
    }
  }

  findServerNameFromSelectedEnvironment(): void {
    if(this.selectedEnvironment) {
      //console.log("qsnjdfnjqsd -> ", this.servers.length);
      const webServer = this.servers.find(s => s.serverId === this.selectedEnvironment.bpwebServerId);
      if(webServer) 
        this.selectedEnvironment.nameBpwebServerId = webServer.name;

      const eaiFTP = this.servers.find(s => s.serverId === this.selectedEnvironment.eaiftpServerId);
      if(eaiFTP) 
        this.selectedEnvironment.nameEaiftpServerId = eaiFTP.name;

      const tableauServer = this.servers.find(s => s.serverId === this.selectedEnvironment.tableauServerId);
      if(tableauServer) 
        this.selectedEnvironment.nameTableauServerId = tableauServer.name;

      const ssrs = this.servers.find(s => s.serverId === this.selectedEnvironment.ssrsServerId);
      if(ssrs) 
        this.selectedEnvironment.nameSsrsServerId = ssrs.name;
    }
  }

  setEnvironment(environmentId: number): void {
    this.idEnvSelected = environmentId;
  }

  onEnvironmentSelect(environmentId: number): void {
    this.setEnvironment(environmentId);
    if (this.selectedDomain && this.selectedDomainEnvironments) {
      this.selectedDomainEnvironment = this.selectedDomainEnvironments.find(
        (env) => env.environment == environmentId
      );
    }
    this.findDatabaseNameFromSelectedEnvironment();
    this.findServerNameFromSelectedEnvironment();
  }

  /***************************************************************************************/
  /**
   * Display the logo of the domain
   * @param encodingLogoPath the path of the logo encoded in base64
   */
  displayLogo(encodingLogoPath: string): void {
    const imagePreview = document.getElementById('printLogo') as HTMLDivElement;
    imagePreview.innerHTML = `<img src="${encodingLogoPath}" alt="Logo" style="width: 10rem; height: 10rem;">`;
  }

  /***************************************************************************************/
  startNewDomainMode(): void {
    this.resetBp5ServersAndDatabasesList();
    this.idEnvSelected = 2;
    this.isNewDomainMode = true;

    this.name = '';
    this.logo = '';
    this.edition = '';
    this.isSsoEnabled = false;
    this.comment = '';
    this.parentCompany = '';
  }

  endNewDomainMode(): void {
    if (this.isNewDomainMode) this.isNewDomainMode = false;
  }

  endEditDomainMode(): void {
    if (this.isEditDomainMode) this.isEditDomainMode = false;
  }

  getServerOrDatabaseIsSelected(): number[] {
    const checkIfServerOrDatabaseIsSelected = (environmentId: number): boolean => {
      return this.selectedDatabaseServers[environmentId] != null ||
              this.selectedWebServers[environmentId] != null ||
              this.selectedEaiFtpServers[environmentId] != null ||
              this.selectedTableauServers[environmentId] != null ||
              this.selectedSsrsServers[environmentId] != null ||
              this.selectedElasticPools[environmentId] != null;
    }
    let selectedEnvironment: number[] = [];
    if (checkIfServerOrDatabaseIsSelected(2)) selectedEnvironment.push(2);
    if (checkIfServerOrDatabaseIsSelected(4)) selectedEnvironment.push(4);
    if (checkIfServerOrDatabaseIsSelected(8)) selectedEnvironment.push(8);
    if (checkIfServerOrDatabaseIsSelected(16)) selectedEnvironment.push(16);
    if (checkIfServerOrDatabaseIsSelected(32)) selectedEnvironment.push(32);
    if (checkIfServerOrDatabaseIsSelected(256)) selectedEnvironment.push(256);

    return selectedEnvironment;
  }

  addDomain() : void {
    let selectedEnvironments: number[];
    selectedEnvironments = this.getServerOrDatabaseIsSelected();

    let environmentsModel = [];
    for(const e of selectedEnvironments) {
      let environmentToAdd = new EnvironmentModelForBackend();
      environmentToAdd.environment = e;
      environmentToAdd.bpwebServerId = this.selectedWebServers[e]?.serverId ?? null;
      environmentToAdd.bpDatabaseId = this.selectedDatabaseServers[e]?.databaseId ?? null;
      environmentToAdd.eaiDatabaseId = this.selectedElasticPools[e]?.databaseId ?? null;
      environmentToAdd.ssrsServerId = this.selectedSsrsServers[e]?.serverId ?? null;
      environmentToAdd.tableauServerId = this.selectedTableauServers[e]?.serverId ?? null;
      environmentToAdd.eaiftpServerId = this.selectedEaiFtpServers[e]?.serverId ?? null;
      environmentToAdd.isBp5Enabled = this.selectedBp5[e];

      environmentsModel.push(environmentToAdd);
    }

    console.log(environmentsModel);

    //add a new domain
    this.http
      .post('http://localhost:5050/api/domain', {
        name: this.name,
        logo: this.logo,
        edition: this.edition,
        isSsoEnabled: this.isSsoEnabled,
        comment: this.comment,
        parentCompany: this.parentCompany,
        environments: environmentsModel
      })
      .subscribe({
        next: (data: any) => {
          const newDomainId = data.domainId;
          console.log("success");
        },
        error: (error: any) => {
          alert('Connection error: ' + error.message);
        },
      });

    this.endNewDomainMode();
  }

  confirmSave(): void {
    if (!this.isNewDomainMode && !this.isEditDomainMode)
      throw new Error('problem while domain adding');

    //ajouter un domaine
    if (!this.isEditDomainMode) {
      const isConfirmed = window.confirm('Are you sure to add the domain ?');
      if (isConfirmed) this.addDomain();
    }
    //modifier un domaine
    else {
      const isConfirmed = window.confirm("Are you sure to edit the domain ?");
      if(isConfirmed)
        this.updateDomain();
    }
  }

  onLogoChange(event: any): void {
    if (this.isFileFormatConformed(this.logo)) {
      const file = event.target.files[0];
      const reader = new FileReader();

      reader.onload = (e) => {
        const imagePreview = document.getElementById('imagePreview');
        imagePreview.innerHTML = `<img src="${e.target.result}" alt="Preview" style="width: 10rem; height: 10rem;">`;
      };
      reader.readAsDataURL(input.files[0]);
    }
    else {
      alert("The format file is not correct, only jpeg jpg and png extention are allowed");
      this.logo = '';
    }
  }

  isFileFormatConformed(file: string): boolean {
    const regex = /\.(jpeg|jpg|png)$/i;
    return regex.test(file);
  }

  editDomain() : void {
    if(this.selectedDomain) {
      this.isNewDomainMode = true;
      this.isEditDomainMode = true;

      this.name = this.selectedDomain.name;
      this.logo = this.selectedDomain.logo;
      this.edition = this.selectedDomain.edition;
      this.isSsoEnabled = this.selectedDomain.isSsoEnabled;
      this.comment = this.selectedDomain.comment;
      this.parentCompany = this.selectedDomain.parentCompany;
    }
  }

  updateDomain() : void {
    if(this.selectedDomain && this.isEditDomainMode) {
      const updatedDomain = {
        domainId: this.selectedDomain.domainId,
        name: this.name,
        logo: this.logo,
        edition: this.edition,
        isSsoEnabled: this.isSsoEnabled,
        comment: this.comment,
        parentCompany: this.parentCompany, 
      };

      this.http.put(`http://localhost:5050/api/domain/${this.selectedDomain.domainId}`, {
        name: this.name,
        logo: this.logo,
        edition: this.edition,
        isSsoEnabled: this.isSsoEnabled,
        comment: this.comment,
        parentCompany: this.parentCompany
        })
        .subscribe((data: any) => {
          this.domains = data;
        }, (error) => {
          alert("Connection error: " + error.message);
        });
        this.endEditDomainMode();
        this.endNewDomainMode();
    }
  }

  copyDomain(): void {
    const postDomainEnvironment = (newDomainId) => {
      const domainId = this.selectedDomain.domainId;

      const envToCopied = this.domainEnvironments.filter(
        (e) => e.domainId == domainId);
      
      envToCopied.forEach(
        (e) => e.domainId = newDomainId);

        this.http.post('http://localhost:5050/api/domainEnvironment', envToCopied).subscribe({
          next: (data: any) => {
          this.domainEnvironments = data.env;
          const domain = this.domains.find(
            (e) => e.domainId == newDomainId);
          this.onSelect(domain);
          },
          error: (error: any) => {
            alert('Connection error: ' + error.message);
          },
        });
    };
    
    const postDomain = (domain) => {
      this.http.post('http://localhost:5050/api/domain', domain).subscribe({
        next: (data: any) => {
          this.domains = data.domains;
          const newDomainId = data.newDomainId;

          postDomainEnvironment(newDomainId);
        },
        error: (error: any) => {
          alert('Connection error: ' + error.message);
        },
      });
    };

    if (this.selectedDomain) {
      const isConfirmed = window.confirm('Are you sure to copy the domain?');
      if (isConfirmed) {
        this.name = "copy - " + this.selectedDomain.name;
        this.logo = this.selectedDomain.logo;
        this.edition = this.selectedDomain.edition;
        this.isSsoEnabled = this.selectedDomain.isSsoEnabled;
        this.comment = this.selectedDomain.comment;
        this.parentCompany = this.selectedDomain.parentCompany;
  
        // Ajoutez le domaine copié en appelant la fonction addDomain
        this.addDomain();
      }
    }
  }

  deleteDomain() : void {
    if(this.selectedDomain) {
      const isConfirmed = window.confirm("Are you sure to delete this domain ?");
      if(isConfirmed) {
        const domainIdToDelete = this.selectedDomain.domainId;

        this.http.delete(`http://localhost:5050/api/domain/${domainIdToDelete}`)
        .subscribe(
          (data: any) => {
            this.domains = data;
            this.name = '';
            this.logo = '';
            this.edition = '';
            this.isSsoEnabled = false;
            this.comment = '';
            this.parentCompany = '';
          },
          (error) => {
            alert("Connection error: " + error.message);
          }
        );
      }
    }
  }
}
