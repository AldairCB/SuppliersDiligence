using Api.Models;
using Api.Services.Supplier;
using ApiModel.Supplier;
using ErrorOr;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class SupplierController(ISupplierService supplierService) : ApiController
{
    //DEPENDENCY INJECTION
    private readonly ISupplierService _supplierService = supplierService;

    [HttpPost]
    public async Task<IActionResult> CreateSupplier(CreateSupplierRequest request){
        var errorOrSupplier = RequestToSupplier(request);
        if(errorOrSupplier.IsError) return Problem(errorOrSupplier.Errors);
        SupplierModel supplier = errorOrSupplier.Value;
        ErrorOr<Created> result = await _supplierService.CreateSupplier(supplier);

        return result.Match(
            created => CreatedAtAction(
                actionName: nameof(GetSupplier), // tells the user the action where they can retrieved the newly created supplier
                routeValues: new { id = supplier.Id }, // tells the user the id of the newly created supplier
                value: SupplierToResponse(supplier) //the actual response to the user
            ),
            errors => Problem(errors)
        );
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetSupplier(Guid id){
        var errorOrSupplier = await _supplierService.GetSupplier(id);
        return errorOrSupplier.Match(
            supplier => Ok(SupplierToResponse(supplier)),
            errors => Problem(errors)
        );
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateSupplier(Guid id, CreateSupplierRequest request){
        return Ok();
    }
    
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteSupplier(Guid id){
        return Ok();
    }

    private static ErrorOr<SupplierModel> RequestToSupplier(CreateSupplierRequest request){
        return SupplierModel.Create(
            request.BusinessName,
            request.TradeName,
            request.Ruc,
            request.PhoneNumber,
            request.Email,
            request.Website,
            request.PhysicalAddress,
            request.Country,
            request.AnnualReportInUSD
        );
    }
    private static SupplierResponse SupplierToResponse(SupplierModel supplier){
        return new SupplierResponse(
            supplier.Id,
            supplier.BusinessName,
            supplier.TradeName,
            supplier.Ruc,
            supplier.PhoneNumber,
            supplier.Email,
            supplier.Website,
            supplier.PhysicalAddress,
            supplier.Country,
            supplier.AnnualReportInUSD,
            supplier.LastModificationDate
        );
    }
}