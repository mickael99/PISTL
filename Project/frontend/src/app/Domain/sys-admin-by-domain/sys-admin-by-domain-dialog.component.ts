import { Component, Inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import {DatePipe, formatDate} from '@angular/common';
import {MatDatepicker} from '@angular/material/datepicker';
import { NativeDateAdapter, DateAdapter } from '@angular/material/core';
import { AbstractControl, ValidationErrors, AsyncValidatorFn } from '@angular/forms';

@Component({
  selector: 'app-sys-admin-by-domain-dialog',  providers: [DatePipe, MatDatepicker, { provide: DateAdapter, useClass: NativeDateAdapter }],
  styleUrls: ['./sys-admin-by-domain.component.css'],
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
          <button mat-raised-button color="primary" (click)="onCancel()">Cancel</button>
          <button mat-raised-button class="color-button" type="submit">Submit</button>
        </div>
      </form>
    </mat-dialog-content>
  `,
})

export class SysAdminByDomainDialog {
  newAdminForm: FormGroup;
  user: any;

  constructor(
    private dialogRef: MatDialogRef<SysAdminByDomainDialog>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private fb: FormBuilder,
    public datepipe: DatePipe
  ) {
    // Set the default value for "from" to the current date
    const currentDate = new Date();
    const formattedCurrentDate = this.datepipe.transform(currentDate, 'yyyy-MM-dd');

    this.newAdminForm = this.fb.group({
      from: [formattedCurrentDate, Validators.required], // Set default value
      to: ['', Validators.required],
      comment: [data.comment, Validators.required],
      user: [data],
    });
  }

  asyncDateValidation(): AsyncValidatorFn {
    return async (control: AbstractControl): Promise<ValidationErrors | null> => {
      const from = this.newAdminForm.get('from').value;
      const to = control.value;

      if (from && to) {
        const fromDate = new Date(from);
        const toDate = new Date(to);

        if (toDate <= fromDate) {
          return { dateError: 'To date must be after From date' };
        }
      }

      return null;
    };
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
    this.dialogRef.close(null);
  }
}