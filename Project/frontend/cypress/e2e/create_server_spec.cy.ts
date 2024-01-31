describe('template spec', () => {
  it('passes', () => {
    cy.visit('http://localhost:4200/')
    
    // Enter email
    cy.get('input[name="email"]').type('simadaniel@hotmail.com')

    // Enter password
    cy.get('input[name="motDePasse"]').type('STL')

    // Add your login button selector and click it
    cy.get('input[type="submit"]').click()

    // Click on the button to go to the server page
    cy.get('a[href="/server"]').click({force: true})

    // Create a server
    cy.get('button[name="new"]').type('test')
  })
})