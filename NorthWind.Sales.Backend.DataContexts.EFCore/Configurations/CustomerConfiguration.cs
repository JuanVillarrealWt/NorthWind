namespace NorthWind.Sales.Backend.DataContexts.EFCore.Configurations;
internal sealed class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
 public void Configure(EntityTypeBuilder<Customer> b)
 {
  b.HasKey(c=>c.Id); b.Property(c=>c.Id).HasMaxLength(5).IsFixedLength(); b.Property(c=>c.Name).IsRequired().HasMaxLength(40); b.Property(c=>c.CurrentBalance).HasPrecision(8,2);
  b.HasData(new Customer{Id="ALFKI",Name="Alfreds Futterkiste",CurrentBalance=0},new Customer{Id="ANATR",Name="Ana Trujillo Emparedados y helados",CurrentBalance=0},new Customer{Id="ANTON",Name="Antonio Moreno Taquería",CurrentBalance=100});
 }
}
