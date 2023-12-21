import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DomainAdministrationComponent } from './domain-administration.component';

describe('DomainAdministrationComponent', () => {
  let component: DomainAdministrationComponent;
  let fixture: ComponentFixture<DomainAdministrationComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [DomainAdministrationComponent]
    });
    fixture = TestBed.createComponent(DomainAdministrationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
