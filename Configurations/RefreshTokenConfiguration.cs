using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineCourse.Entities;

namespace OnlineCourse.Configurations;

public class RefreshTokenConfiguration(IConfiguration configuration) : IEntityTypeConfiguration<RefreshToken>
{
    private readonly IConfiguration _configuration = configuration;
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        // Table name & constraints
        builder.ToTable("RefreshTokens");
        // Indexes
        builder.HasIndex(rt=>rt.Token).IsUnique();
        // Properties
        builder.Property(rt => rt.CreationDate).HasDefaultValueSql("GETUTCDATE()");
        builder.Property(rt => rt.IsUsed).HasDefaultValue(false);
        int refreshTokenExpiryDays = _configuration.GetSection("JwtSettings").GetValue("RefreshTokenExpiryDays", 7);
        builder.Property(rt => rt.ExpiryDate)
            .HasDefaultValueSql($"DATEADD(day, {refreshTokenExpiryDays}, GETUTCDATE())");
        
    }
}