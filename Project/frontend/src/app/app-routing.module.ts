import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DomainAdministrationComponent } from './Domain/domain-administration/domain-administration.component';
import { AppComponent } from './app.component';
import { DomainBySysAdminComponent } from './Domain/domain-by-sys-admin/domain-by-sys-admin.component';
import { SysAdminByDomainComponent } from './Domain/sys-admin-by-domain/sys-admin-by-domain.component';
import { AccountComponent } from './Account/account.component';

const routes: Routes = [
  // { path: '', redirectTo: 'account', pathMatch: 'full' }, // default path
  { path: 'account', component: AccountComponent },
  { path: 'domain-administration', component: DomainAdministrationComponent },
  { path: 'sys-admin-by-domain', component: SysAdminByDomainComponent },
  { path: 'domain-by-sys-admin', component: DomainBySysAdminComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
