describe('template spec', () => {
    it('passes', () => {
        cy.visit('http://localhost:4200/')
        
        // Enter email
        cy.get('input[name="email"]').type('simadaniel@hotmail.com')

        // Enter password
        cy.get('input[name="motDePasse"]').type('STL')

        // Add your login button selector and click it
        cy.get('input[type="submit"]').click()

        // Go to the sysadmin page
        cy.get('a[href="/sys-admin-by-domain"]').click({force: true})

        // Add a new sysadmin
        cy.get('button[name="add"]').click({force: true})

        // Check if user 27995 (Daniel Sima) is in the list
        cy.get('tr').contains('27995').should('exist')

        // Get the td element with class "mat-cell-dev" inside the tr that contains '27995'
        cy.get('tr:contains("27995") td.mat-column-dev').should('exist')

        // Get the checkbox inside the td with class "mat-cell-dev" inside the tr that contains '27995'
        cy.get('tr:contains("27995") td.mat-column-dev input[type="checkbox"]').check()

        cy.wait(500)

        // Check if the user 27995 is now checked
        cy.get('tr:contains("27995") td.mat-column-dev input[type="checkbox"]').should('not.be.checked')

        // if (cy.get('tr:contains("27995") td.mat-column-dev input[type="checkbox"]').should('not.be.checked')) {
        //     // Perform some action if the checkbox is unchecked
        //     // wait 0.5 second
        //     cy.wait(500)

        //     // fill the field "to" with the value 02/01/2024
        //     cy.get('input[type="date"][formControlName="to"]').type('2024-02-01');

        //     // fill the field "comment" with the value "test"
        //     cy.get('textarea[formControlName="comment"]').type('test')

        //     // Click on the submit button
        //     cy.get('button[type="submit"]').click()

        // }
        // else {
        //     cy.wait(500)

        //     // Check if the user 27995 is now unchecked
        //     cy.get('tr:contains("27995") td.mat-column-dev input[type="checkbox"]').should('not.be.checked')
        // }

        // // Click on the save button
        // cy.get('button[name="save"]').click()
    })
})
