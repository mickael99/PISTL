import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SysAdminByDomainComponent } from './sys-admin-by-domain.component';

describe('SysAdminByDomainComponent', () => {
  let component: SysAdminByDomainComponent;
  let fixture: ComponentFixture<SysAdminByDomainComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SysAdminByDomainComponent]
    });
    fixture = TestBed.createComponent(SysAdminByDomainComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
