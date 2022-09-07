using Core.Persistence.Repositories;

namespace Domain.Entities;

public class Model : Entity
{
    public int BrandId { get; set; }
    public string Name { get; set; }
    public decimal DailyPrice { get; set; }
    public string ImageUrl { get; set; }

    public virtual Brand? Brand { get; set; }

    public Model()
    {
    }

    //Brand modelini direkt ctor de geçmek yerine onun id'sini geçmek daha iyi olabiliyor
    public Model(int id, int brandId, string name, decimal dailyPrice, string imageUrl) : this()
    {
        Id = id;
        BrandId = brandId;
        Name = name;
        DailyPrice = dailyPrice;
        ImageUrl = imageUrl;
    }
}