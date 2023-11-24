import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DomainBySysAdminComponent } from './domain-by-sys-admin.component';

describe('DomainBySysAdminComponent', () => {
  let component: DomainBySysAdminComponent;
  let fixture: ComponentFixture<DomainBySysAdminComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [DomainBySysAdminComponent]
    });
    fixture = TestBed.createComponent(DomainBySysAdminComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
