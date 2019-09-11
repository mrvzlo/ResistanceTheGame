using System.ComponentModel.DataAnnotations;

namespace Resistance.Entities
{
    public class Key
    {
        [Key]
        public virtual int Id { get; set; }
    }
}
