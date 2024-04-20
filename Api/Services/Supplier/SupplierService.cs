using Api.Data;
using Api.Models;
using Api.ServicesErrors;
using ErrorOr;

namespace Api.Services.Supplier;

public class SupplierService(DataContext dataContext) : ISupplierService
{
    private readonly DataContext _dataContext = dataContext;

    public async Task<ErrorOr<Created>> CreateSupplier(SupplierModel supplier)
    {
        _dataContext.Suppliers.Add(supplier);
        await _dataContext.SaveChangesAsync();
        return Result.Created;
    }

    public async Task<ErrorOr<SupplierModel>> GetSupplier(Guid id)
    {
        if(await _dataContext.Suppliers.FindAsync(id) is SupplierModel supplier) return supplier;
        return Errors.Supplier.NotFound(id);
    }

    public Task<ErrorOr<Updated>> UpdateSupplier(SupplierModel supplier)
    {
        throw new NotImplementedException();
    }

    public async Task<ErrorOr<Deleted>> DeleteSupplier(Guid id)
    {
        if (await _dataContext.Suppliers.FindAsync(id) is SupplierModel supplier){
            _dataContext.Suppliers.Remove(supplier);
            _dataContext.SaveChanges();
            return Result.Deleted;
        }
        return Errors.Supplier.NotFound(id);
    }
}