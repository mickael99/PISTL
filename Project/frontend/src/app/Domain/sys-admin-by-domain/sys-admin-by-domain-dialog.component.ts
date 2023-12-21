import { Component, Inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import {DatePipe, formatDate} from '@angular/common';
import {MatDatepicker} from '@angular/material/datepicker';
import { NativeDateAdapter, DateAdapter } from '@angular/material/core';

@Component({
  selector: 'app-sys-admin-by-domain-dialog',  providers: [DatePipe, MatDatepicker, { provide: DateAdapter, useClass: NativeDateAdapter }],
  template: `
    <h2 mat-dialog-title>From/To Date</h2>
    <mat-dialog-content>
      <form [formGroup]="newAdminForm" (ngSubmit)="onSubmit()">
        <mat-form-field>
          <mat-label>From</mat-label>
          <input matInput type="date" formControlName="from" />
        </mat-form-field>
        <mat-form-field>
          <mat-label>To</mat-label>
          <input matInput type="date" formControlName="to" />
        </mat-form-field>
        <mat-form-field>
          <mat-label>Comment</mat-label>
          <input matInput type="text" formControlName="comment" />
        </mat-form-field>
        <div mat-dialog-actions>
          <button mat-button (click)="onCancel()">Cancel</button>
          <button mat-button type="submit">Submit</button>
        </div>
      </form>
    </mat-dialog-content>
  `,
})

export class SysAdminByDomainDialog {
  newAdminForm: FormGroup;
  
  constructor(
    private dialogRef: MatDialogRef<SysAdminByDomainDialog>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private fb: FormBuilder,
    public datepipe: DatePipe
  ) {
    this.newAdminForm = this.fb.group({
      from: ['', Validators.required],
      to: ['', Validators.required],
      comment: ['', Validators.required],
      user: [data],
    });
  }

  setDate(event, dp) {
    this.newAdminForm.patchValue({
      to: this.datepipe.transform(event, 'yyyy-MM-dd')
    });
    dp.close();
  }

  onSubmit(): void {
    this.dialogRef.close(this.newAdminForm.value);
  }

  onCancel(): void {
    console.log("onCancel");
    this.dialogRef.close(null);
  }
}