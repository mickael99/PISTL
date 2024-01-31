namespace Name.Models.DTO
{
    /// <summary>
    /// Represents a user data transfer object.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Gets or sets the email of the user.
        /// </summary>
        public required string Email { get; set; }

        /// <summary>
        /// Gets or sets the password of the user.
        /// </summary>
        public required string Password { get; set; }
    } 
}
