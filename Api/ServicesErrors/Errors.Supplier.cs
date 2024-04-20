using ErrorOr;

namespace Api.ServicesErrors;

public static class Errors
{
    public static class Supplier{
        public static Error NotFound(Guid id) => Error.NotFound(
            code: "Supplier.NotFound",
            description: $"Supplier with ID: {id} was not found"
        );
    }
}