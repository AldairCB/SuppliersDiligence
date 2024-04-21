using ErrorOr;
using Api.Models;

namespace Api.Services.Supplier;

public interface ISupplierService
{
    Task<ErrorOr<Created>> CreateSupplier(SupplierModel supplier);
    Task<ErrorOr<List<SupplierModel>>> GetAllSuppliers();
    Task<ErrorOr<SupplierModel>> GetSupplier(Guid id);
    Task<ErrorOr<Updated>> UpdateSupplier(SupplierModel supplier);
    Task<ErrorOr<Deleted>> DeleteSupplier(Guid id);
}