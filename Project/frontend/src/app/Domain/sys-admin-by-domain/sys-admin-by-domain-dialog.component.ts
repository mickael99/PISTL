import { Component, Inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-sys-admin-by-domain-dialog',
  template: `
    <h2 mat-dialog-title>From/To Date</h2>
    <mat-dialog-content>
      <form [formGroup]="newAdminForm" (ngSubmit)="onSubmit(data)">
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
    private fb: FormBuilder
  ) {
    this.newAdminForm = this.fb.group({
      from: ['', Validators.required],
      to: ['', Validators.required],
      comment: ['', Validators.required],
      user: [data],
    });
  }

  onSubmit(data: any): void {
    this.dialogRef.close(this.newAdminForm.value);
  }

  onCancel(): void {
    this.dialogRef.close();
  }
}