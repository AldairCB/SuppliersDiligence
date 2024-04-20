using ApiModel.Supplier;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class SupplierController : ApiController
{
    [HttpPost]
    public IActionResult CreateSupplier(CreateSupplierRequest request){
        return Ok();
    }
    [HttpGet]
    public IActionResult GetSupplier(Guid id){
        return Ok();
    }
    [HttpPut]
    public IActionResult UpdateSupplier(CreateSupplierRequest request){
        return Ok();
    }
    [HttpDelete]
    public IActionResult DeleteSupplier(Guid id){
        return Ok();
    }
}