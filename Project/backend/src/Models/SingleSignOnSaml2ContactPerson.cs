using System;
using System.Collections.Generic;

namespace Project.Models;

public partial class SingleSignOnSaml2ContactPerson
{
    public int ContactPersonId { get; set; }

    public string Company { get; set; } = null!;

    public int Type { get; set; }

    public string EmailAddresses { get; set; } = null!;

    public string GivenName { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public string ModifiedBy { get; set; } = null!;

    public DateTime ModifiedDate { get; set; }
}
