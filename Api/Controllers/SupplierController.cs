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
    public async Task<IActionResult> CreateSupplier(SupplierRequest request){
        var errorOrSupplier = RequestToSupplier(request);
        if(errorOrSupplier.IsError) return Problem(errorOrSupplier.Errors);
        SupplierModel supplier = errorOrSupplier.Value;
        var result = await _supplierService.CreateSupplier(supplier);

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
    public async Task<IActionResult> UpdateSupplier(Guid id, SupplierRequest request){
        var errorOrSupplier = RequestToSupplier(request, id);
        if(errorOrSupplier.IsError) return Problem(errorOrSupplier.Errors);
        SupplierModel supplier = errorOrSupplier.Value;
        var result = await _supplierService.UpdateSupplier(supplier);

        return result.Match(
            updated => NoContent(),
            errors => Problem(errors)
        );
    }
    
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteSupplier(Guid id){
        var result = await _supplierService.DeleteSupplier(id);
        return result.Match(
            deleted => NoContent(),
            errors => Problem(errors)
        );
    }

    private static ErrorOr<SupplierModel> RequestToSupplier(SupplierRequest request, Guid? id = null){
        // If no id is provided (is null) then it will return a brand new supplier
        return SupplierModel.Create(
            id,
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