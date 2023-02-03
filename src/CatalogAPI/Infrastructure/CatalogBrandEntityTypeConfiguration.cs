using CatalogAPI.Model;

namespace CatalogAPI.Infrastructure;

public class CatalogBrandEntityTypeConfiguration : IEntityTypeConfiguration<CatalogBrand>
{
    public void Configure(EntityTypeBuilder<CatalogBrand> builder)
    {
        builder.ToTable("CatalogBrand");

        builder.HasKey(brand => brand.Id);

        builder.Property(brand => brand.Id)
            .UseHiLo("catalog_brand_hilo")
            .IsRequired();

        builder.Property(brand => brand.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasData(
            new CatalogBrand(1, ".NET"),
            new CatalogBrand(2, "Dapr"),
            new CatalogBrand(3, "Other"),
            new CatalogBrand(4, "Container"),
            new CatalogBrand(5, "Kubernetes"));
    }
}