using Application.Features.Models.Queries.GetListModel;
using Application.Features.Models.Queries.GetListModelByDynamic;
using Core.Application.Requests;
using Core.Persistence.Dynamic;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ModelController : BaseController
{
    [HttpGet]
    public async Task<IActionResult> GetList([FromQuery] PageRequest pageRequest)
    {
        GetListModelQuery getListModelQuery = new() { PageRequest = pageRequest };
        
        var result = await Mediator.Send(getListModelQuery);
        
        return Ok(result);
    }
    
    /*örnek bir dynamic query object'ti
     object istiyorsak body'den bir girdi olarak istiyorsak queryden alıyoruz
     *
     { "sort": [
        {
        "field": "name",
        "dir": "asc"
        }
    ],
        "filter": {
        "field": "name",
        "operator": "eq",
        "value": "a180",
        "logic": "or",
        "filters": [
            {
            "field": "dailyPrice",
            "operator": "lte",
            "value": "1300"}
            ]
        }
    }
     * 
     */
    [HttpPost("GetList/ByDynamic")]
    public async Task<IActionResult> GetListByDynamic([FromQuery] PageRequest pageRequest,[FromBody] Dynamic dynamic)
    {
        var getListByDynamicModelQuery = new GetListModelByDynamicQuery
        {
            Dynamic = dynamic,
            PageRequest = pageRequest
        };
        
        var result = await Mediator.Send(getListByDynamicModelQuery);
        
        return Ok(result);
    }
}
