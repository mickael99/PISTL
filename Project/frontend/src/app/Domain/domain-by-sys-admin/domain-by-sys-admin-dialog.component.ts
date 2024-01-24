import { DatePipe } from '@angular/common';
import { Component, Inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { DateAdapter, NativeDateAdapter } from '@angular/material/core';
import { MatDatepicker } from '@angular/material/datepicker';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { from } from 'rxjs';

@Component({
  selector: 'app-domain-by-sys-admin-dialog', 
  providers: [DatePipe, MatDatepicker, { provide: DateAdapter, useClass: NativeDateAdapter }],
  styleUrls: ['./domain-by-sys-admin.component.css'],
  template: `
    <h2 mat-dialog-title>
      System Administration rights for {{ data.name }}
    </h2>
    <mat-dialog-content>
      <form [formGroup]="newAdminForm" (ngSubmit)="onSubmit()">
        <div> 
          <mat-form-field>
            <mat-label>From</mat-label>
            <input matInput type="date" formControlName="from" />
          </mat-form-field>
          <mat-form-field>
            <mat-label>To</mat-label>
            <input matInput type="date" formControlName="to" />
          </mat-form-field>
        </div>
        <div>
          <mat-form-field style="width: 100%;">
            <mat-label>Comment</mat-label>
            <textarea matInput formControlName="comment"></textarea>
          </mat-form-field>
        </div>
        <div *ngIf="newAdminForm.controls.from.value !== '' && newAdminForm.controls.from.value < from_copy" style="color: red;">
          Please enter a valid 'from' date.
        </div>
        <div *ngIf="newAdminForm.controls.to.value !== '' && newAdminForm.controls.to.value < newAdminForm.controls.from.value" style="color: red;">
          Please enter a valid 'to' date.
        </div>
        <div mat-dialog-actions>
          <button mat-raised-button color="primary" (click)="onCancel()">Cancel</button>
          <button mat-raised-button 
                  class="color-button"
                  type="submit" 
                  [disabled]="newAdminForm.controls.to.value !== '' &&
                            newAdminForm.controls.from.value !== '' &&
                            (newAdminForm.controls.to.value < newAdminForm.controls.from.value ||
                            newAdminForm.controls.from.value < from_copy)">
                    Submit
          </button>
        </div>
      </form>
    </mat-dialog-content>
  `,
})
export class DomainBySysAdminComponentDialog {
  newAdminForm: FormGroup;
  from_copy: string;

  constructor(
    private dialogRef: MatDialogRef<DomainBySysAdminComponentDialog>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private fb: FormBuilder,
    public datepipe: DatePipe
  ) {
    // Set the default value for "from" to the current date
    this.from_copy = this.datepipe.transform(data.user.sysAdminStartDate, 'yyyy-MM-dd');
    const currentDate = new Date();
    const fromDate =
      this.datepipe.transform(data.user.sysAdminStartDate, 'yyyy-MM-dd') ||
      this.datepipe.transform(currentDate, 'yyyy-MM-dd');
    const toDate = this.datepipe.transform(data.user.sysAdminEndDate, 'yyyy-MM-dd');
    const comment = data.user.comment;

    this.newAdminForm = this.fb.group({
      from: [fromDate, Validators.required], // Set default value
      to: [toDate, Validators.required],
      comment: [comment, Validators.required],
      user: [data.user],
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
    this.newAdminForm.reset();
    this.dialogRef.close(null);
  }
}