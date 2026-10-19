
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.Property(o => o.CustomerId)
            .IsRequired()
            .HasMaxLength(5)
            .IsFixedLength();

        builder.Property(o => o.CustomerName)
            .IsRequired()
            .HasMaxLength(40);

        builder.Property(o => o.SalespersonName)
            .IsRequired()
            .HasMaxLength(100)
            .HasDefaultValue("Vendedor no registrado");

        builder.HasIndex(o => o.CustomerId);

        builder.Property(o => o.ShipAddress)
            .IsRequired()
            .HasMaxLength(60);

        builder.Property(o => o.ShipCity)
            .HasMaxLength(15);

        builder.Property(o => o.ShipCountry)
            .HasMaxLength(15);

        builder.Property(o => o.ShipPostalCode)
            .HasMaxLength(10);

    }
}
