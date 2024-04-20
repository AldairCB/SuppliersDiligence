namespace ApiModel.Supplier;

public record SupplierRequest(
    string BusinessName,
    string TradeName,
    string Ruc,
    string PhoneNumber,
    string Email,
    string Website,
    string PhysicalAddress,
    string Country, //needs to be dropdown
    decimal AnnualReportInUSD
);