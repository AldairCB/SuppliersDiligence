namespace ApiModel.Supplier;

public record CreateSupplierRequest(
    string BusinessName,
    string TradeName,
    int Ruc,
    string PhoneNumber,
    string Email,
    string Website,
    string PhysicalAddress,
    string Country, //needs to be dropdown
    float AnnualReportInUSD,
    DateTime LastModificationDate
);