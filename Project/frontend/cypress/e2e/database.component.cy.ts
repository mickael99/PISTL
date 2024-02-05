describe('template spec', () => {
  it('passes', () => {
    cy.visit('http://localhost:4200/');

    // Enter email
    cy.get('input[name="email"]').type('test2@example.com');

    // Enter password
    cy.get('input[name="motDePasse"]').type('FZROZPCH');

    // Add your login button selector and click it
    cy.get('div.btn--center button[type="submit"]').click();

    // Go to the sysadmin page
    cy.get('a[href="/database"]').click({ force: true });

    // Add a new sysadmin
    cy.get('button[name="new"]').click({ force: true });

    // Fill in the name input box
    cy.get('input[name="name"]').type('New Database', { force: true });

    // Fill in the username input box
    cy.get('input[name="userName"]').type('DATUser', { force: true });

    // Fill in the password input box
    cy.get('input[name="password"]').type('1234', { force: true });

    // Click on the mat-select element to open the options dropdown
    cy.get('mat-select[name="Server"]').click();

    // Click on the first mat-option element
    cy.get('mat-option:first').click();

    // Click on the mat-select element to open the options dropdown
    cy.get('mat-select[name="Context"]').click();

    // Click on the first mat-option element
    cy.get('mat-option:first').click();

    // Add a new sysadmin
    cy.get('button[name="save"]').click({ force: true });

    cy.wait(5000); // Wait for 5 seconds
  });
});
