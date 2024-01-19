import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InformPopupComponent } from './inform-popup.component';

describe('InformPopupComponent', () => {
  let component: InformPopupComponent;
  let fixture: ComponentFixture<InformPopupComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [InformPopupComponent]
    });
    fixture = TestBed.createComponent(InformPopupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
