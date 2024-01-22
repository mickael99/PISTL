
namespace Project.Models.DTO
{
    /// <summary>
    /// Represents a data transfer object for logging in a domain user.
    /// </summary>
    public class LoginDomainUserDTO
    {
        /// <summary>
        /// Gets or sets the login ID.
        /// </summary>
        public int LoginId { get; set; }

        /// <summary>
        /// Gets or sets the domain ID.
        /// </summary>
        public int DomainId { get; set; }

        /// <summary>
        /// Gets or sets the user ID.
        /// </summary>
        public required string UserId { get; set; }

        /// <summary>
        /// Gets or sets the environment.
        /// </summary>
        public int Environment { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user is a system administrator.
        /// </summary>
        public bool SysAdmin { get; set; }

        /// <summary>
        /// Gets or sets the start date of the system administrator role.
        /// </summary>
        public DateTime? SysAdminStartDate { get; set; }

        /// <summary>
        /// Gets or sets the end date of the system administrator role.
        /// </summary>
        public DateTime? SysAdminEndDate { get; set; }

        /// <summary>
        /// Gets or sets the comment.
        /// </summary>
        public required string Comment { get; set; }

        /// <summary>
        /// Gets or sets the ModifiedBy.
        /// </summary>
        public required string ModifiedBy { get; set; }

        /// <summary>
        /// Gets or sets the user name.
        /// </summary>
        public string? UserName { get; set; }
    }
}
