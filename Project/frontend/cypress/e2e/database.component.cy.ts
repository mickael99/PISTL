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
    cy.get('a[href="/database"]').click({force: true})
  })
})