import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { MatDialogModule } from '@angular/material/dialog';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatSortModule } from '@angular/material/sort';
import { SysAdminByDomainComponent } from './sys-admin-by-domain.component';

describe('SysAdminByDomainComponent', () => {
  let component: SysAdminByDomainComponent;
  let fixture: ComponentFixture<SysAdminByDomainComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        HttpClientTestingModule,
        MatDialogModule,
        MatTableModule,
        MatPaginatorModule,
        MatSortModule
      ],
      declarations: [SysAdminByDomainComponent]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SysAdminByDomainComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create sys-admin-by-domain component', () => {
    expect(component).toBeTruthy();
  });
});