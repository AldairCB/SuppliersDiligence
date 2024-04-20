using System.ComponentModel.DataAnnotations;
using ErrorOr;

namespace Api.Models;

public class SupplierModel
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public string BusinessName { get; set; }

    [Required]
    public string TradeName { get; set; }

    [Required]
    public string Ruc { get; set; }

    [Required]
    public string PhoneNumber { get; set; }

    [Required]
    public string Email { get; set; }

    [Required]
    public string Website { get; set; }

    [Required]
    public string PhysicalAddress { get; set; }

    [Required]
    public string Country { get; set; } //needs to be dropdown

    [Required]
    public decimal AnnualReportInUSD { get; set; }

    [Required]
    public DateTime LastModificationDate { get; set; }

    private SupplierModel(
        Guid id,
        string businessName,
        string tradeName,
        string ruc,
        string phoneNumber,
        string email,
        string website,
        string physicalAddress,
        string country,
        decimal annualReportInUSD,
        DateTime lastModificationDate
    ) {
        Id = id;
        BusinessName = businessName;
        TradeName = tradeName;
        Ruc = ruc;
        PhoneNumber = phoneNumber;
        Email = email;
        Website = website;
        PhysicalAddress = physicalAddress;
        Country = country;
        AnnualReportInUSD = annualReportInUSD;
        LastModificationDate = lastModificationDate;
    }

    public static ErrorOr<SupplierModel> Create(
        string businessName,
        string tradeName,
        string ruc,
        string phoneNumber,
        string email,
        string website,
        string physicalAddress,
        string country,
        decimal annualReportInUSD
    ) {
        
        // TODO: Validate inputs

        return new SupplierModel(
            Guid.NewGuid(),
            businessName,
            tradeName,
            ruc,
            phoneNumber,
            email,
            website,
            physicalAddress,
            country,
            annualReportInUSD,
            DateTime.UtcNow
        );
    }
}