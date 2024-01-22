
namespace Project.Models.DTO
{
    /// <summary>
    /// Represents a domain data transfer object.
    /// </summary>
    public class DomainDTO
    {
        /// <summary>
        /// Gets or sets the domain ID.
        /// </summary>
        public int DomainId { get; set; }

        /// <summary>
        /// Gets or sets the name of the domain.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Gets or sets the environments presence of the domain.
        /// </summary>
        public bool[] Environments { get; set; } = new bool[6];
    }
}
