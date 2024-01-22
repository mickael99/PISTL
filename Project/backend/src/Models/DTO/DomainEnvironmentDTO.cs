
namespace Project.Models.DTO
{
    /// <summary>
    /// Represents a data transfer object for the domain environment.
    /// </summary>
    public class DomainEnvironmentDTO
    {
        /// <summary>
        /// Gets or sets the ID of the domain environment.
        /// </summary>
        public int DomainEnvironmentId { get; set; }

        /// <summary>
        /// Gets or sets the ID of the domain.
        /// </summary>
        public int DomainId { get; set; }

        /// <summary>
        /// Gets or sets the environment value.
        /// </summary>
        public int Environment { get; set; }
    }
}
