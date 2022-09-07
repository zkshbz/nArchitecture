using Core.Persistence.Repositories;

namespace Domain.Entities;

public class Brand : Entity
{
    public string Name { get; set; }

    public virtual ICollection<Model> Models { get; set; } //Ef de virtual vermek zorunda değilisiniz
                                                           //farklı ORM'lerden gelen alışkanlık(nHibernate)

    public Brand()
    {
    }

    public Brand(int id, string name) : this()
    {
        Id = id;
        Name = name;
    }
}