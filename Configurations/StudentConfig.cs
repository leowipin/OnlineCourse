using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineCourse.Entities;
using OnlineCourse.Enums;

namespace OnlineCourse.Configurations
{
    public class StudentConfig : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            // Constraints values
            var studentsConstraintVal = Enum.GetNames(typeof(StudentTypes))
                .Select(name => $"'{name}'");
            var studentConstraintListSql = string.Join(", ", studentsConstraintVal);
            string discriminatorColumnName = "StudentType";
            string subscriptionColumnName = "SubscriptionExpires";
            string enrollmentColumnName = "EnrollmentDate";
            // Table Name & Constraints
            builder.ToTable("Students", b => b.HasCheckConstraint(
                    name: "CK_Students_StudentType",
                    sql: $"[{discriminatorColumnName}] IN ({studentConstraintListSql})"
                ));
            builder.ToTable(b => b.HasCheckConstraint(
                    name: "CK_Students_Subscription",
                    sql: 
                    $"(" +
                        $"([{discriminatorColumnName}]='{nameof(StudentTypes.Premium)}') " +
                        $"AND" +
                        $"([{subscriptionColumnName}] IS NOT NULL) " +
                        $"AND " +
                        $"([{subscriptionColumnName}] > [{enrollmentColumnName}])" +
                    $")" +
                    $"OR" +
                    $"(" +
                        $"([{discriminatorColumnName}]<>'{nameof(StudentTypes.Premium)}') " +
                        $"AND" +
                        $"([{subscriptionColumnName}] IS NULL)" +
                    $")"
                ));
            // Properties
            builder.Property(x => x.EnrollmentDate).HasDefaultValueSql("GETUTCDATE()");
            builder.Property(x => x.StudentType).HasConversion<string>()
                .HasMaxLength(50);
            // Discriminator
            builder.HasDiscriminator(x => x.StudentType)
                .HasValue<Student>(StudentTypes.Standard)
                .HasValue<StudentPremium>(StudentTypes.Premium);
        }
    }
}