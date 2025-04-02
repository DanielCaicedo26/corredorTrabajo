
using Entity.Model;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Dto
{
    public class UserInfraction
    {
        public int Id { get; set; }
        public int TypeInfractionId { get; set; }
        [ForeignKey("TypeInfractionId")]
        public virtual TypeInfraction TypeInfraction { get; set; }
    }
}
