using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ModernizationDemo.LinqToSqlToEfCore.Model;

public partial class DotNetCollegeContext : DbContext
{
    public DotNetCollegeContext()
    {
    }

    public DotNetCollegeContext(DbContextOptions<DotNetCollegeContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<AccountTransaction> AccountTransactions { get; set; }

    public virtual DbSet<Address> Addresses { get; set; }

    public virtual DbSet<Attachment> Attachments { get; set; }

    public virtual DbSet<AttendeeRegistration> AttendeeRegistrations { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<CourseDate> CourseDates { get; set; }

    public virtual DbSet<CourseReminder> CourseReminders { get; set; }

    public virtual DbSet<CourseTemplate> CourseTemplates { get; set; }

    public virtual DbSet<CourseTemplateRelation> CourseTemplateRelations { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<DiscountCoupon> DiscountCoupons { get; set; }

    public virtual DbSet<Invoice> Invoices { get; set; }

    public virtual DbSet<InvoiceLine> InvoiceLines { get; set; }

    public virtual DbSet<Lector> Lectors { get; set; }

    public virtual DbSet<Location> Locations { get; set; }

    public virtual DbSet<MainCategory> MainCategories { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderAvailableDate> OrderAvailableDates { get; set; }

    public virtual DbSet<PrivateCourseRequest> PrivateCourseRequests { get; set; }

    public virtual DbSet<Recording> Recordings { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=.\\SQLEXPRESS;Initial Catalog=LinqToSqlTest;Integrated Security=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.Accounts");

            entity.Property(e => e.Name).HasMaxLength(100);

            entity.HasOne(d => d.Lector).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.LectorId)
                .HasConstraintName("Lector_Account");

            entity.HasOne(d => d.Supplier).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.SupplierId)
                .HasConstraintName("Supplier_Account");
        });

        modelBuilder.Entity<AccountTransaction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.AccountTransactions");

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CustomAccountTransactionAmount)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("CustomAccountTransaction_Amount");
            entity.Property(e => e.CustomAccountTransactionCreatedDate).HasColumnName("CustomAccountTransaction_CreatedDate");
            entity.Property(e => e.CustomAccountTransactionPaidDate).HasColumnName("CustomAccountTransaction_PaidDate");
            entity.Property(e => e.InvoiceUrl).HasMaxLength(200);
            entity.Property(e => e.LectorInvoiceUrl).HasMaxLength(200);

            entity.HasOne(d => d.Account).WithMany(p => p.AccountTransactions)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Account_AccountTransaction");

            entity.HasOne(d => d.Course).WithMany(p => p.AccountTransactions)
                .HasForeignKey(d => d.CourseId)
                .HasConstraintName("Course_AccountTransaction");

            entity.HasOne(d => d.Invoice).WithMany(p => p.AccountTransactions)
                .HasForeignKey(d => d.InvoiceId)
                .HasConstraintName("Invoice_AccountTransaction");
        });

        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.Addresses");

            entity.Property(e => e.BankAccount).HasMaxLength(50);
            entity.Property(e => e.City).HasMaxLength(200);
            entity.Property(e => e.CompanyRegistration).HasMaxLength(200);
            entity.Property(e => e.Dic)
                .HasMaxLength(20)
                .HasColumnName("DIC");
            entity.Property(e => e.Iban).HasMaxLength(50);
            entity.Property(e => e.Ic)
                .HasMaxLength(20)
                .HasColumnName("IC");
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.Street).HasMaxLength(200);
            entity.Property(e => e.Swift).HasMaxLength(50);
            entity.Property(e => e.Zip)
                .HasMaxLength(20)
                .HasColumnName("ZIP");

            entity.HasOne(d => d.Country).WithMany(p => p.Addresses)
                .HasForeignKey(d => d.CountryId)
                .HasConstraintName("Country_Address");
        });

        modelBuilder.Entity<Attachment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.Attachments");

            entity.Property(e => e.FileName).HasMaxLength(200);
            entity.Property(e => e.Url).HasMaxLength(200);

            entity.HasOne(d => d.Course).WithMany(p => p.Attachments)
                .HasForeignKey(d => d.CourseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Course_Attachment");
        });

        modelBuilder.Entity<AttendeeRegistration>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.AttendeeRegistrations");

            entity.Property(e => e.Email).HasMaxLength(200);
            entity.Property(e => e.FirstName).HasMaxLength(200);
            entity.Property(e => e.LastName).HasMaxLength(200);

            entity.HasOne(d => d.Order).WithMany(p => p.AttendeeRegistrations)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Order_AttendeeRegistration");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.Categories");

            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.UrlName).HasMaxLength(200);

            entity.HasOne(d => d.MainCategory).WithMany(p => p.Categories)
                .HasForeignKey(d => d.MainCategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("MainCategory_Category");

            entity.HasMany(d => d.CourseTemplates).WithMany(p => p.Categories)
                .UsingEntity<Dictionary<string, object>>(
                    "CategoryCourseTemplate",
                    r => r.HasOne<CourseTemplate>().WithMany()
                        .HasForeignKey("CourseTemplatesId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("CourseTemplate_CategoryCourseTemplate"),
                    l => l.HasOne<Category>().WithMany()
                        .HasForeignKey("CategoriesId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("Category_CategoryCourseTemplate"),
                    j =>
                    {
                        j.HasKey("CategoriesId", "CourseTemplatesId").HasName("PK_dbo.CategoryCourseTemplate");
                        j.ToTable("CategoryCourseTemplate");
                    });
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.Countries");

            entity.Property(e => e.Name).HasMaxLength(200);
        });

        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.Courses");

            entity.Property(e => e.CourseSubtitle).HasMaxLength(200);
            entity.Property(e => e.Margin).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.PricePerDay).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.CourseTemplate).WithMany(p => p.Courses)
                .HasForeignKey(d => d.CourseTemplateId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("CourseTemplate_Course");

            entity.HasOne(d => d.Customer).WithMany(p => p.Courses)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("Customer_Course");

            entity.HasOne(d => d.Location).WithMany(p => p.Courses)
                .HasForeignKey(d => d.LocationId)
                .HasConstraintName("Location_Course");

            entity.HasOne(d => d.Supplier).WithMany(p => p.Courses)
                .HasForeignKey(d => d.SupplierId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Supplier_Course");

            entity.HasMany(d => d.Lectors).WithMany(p => p.Courses)
                .UsingEntity<Dictionary<string, object>>(
                    "CourseLector",
                    r => r.HasOne<Lector>().WithMany()
                        .HasForeignKey("LectorsId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("Lector_CourseLector"),
                    l => l.HasOne<Course>().WithMany()
                        .HasForeignKey("CoursesId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("Course_CourseLector"),
                    j =>
                    {
                        j.HasKey("CoursesId", "LectorsId").HasName("PK_dbo.CourseLector");
                        j.ToTable("CourseLector");
                    });
        });

        modelBuilder.Entity<CourseDate>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.CourseDates");

            entity.HasOne(d => d.Course).WithMany(p => p.CourseDates)
                .HasForeignKey(d => d.CourseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Course_CourseDate");
        });

        modelBuilder.Entity<CourseReminder>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.CourseReminders");

            entity.HasOne(d => d.Course).WithMany(p => p.CourseReminders)
                .HasForeignKey(d => d.CourseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Course_CourseReminder");
        });

        modelBuilder.Entity<CourseTemplate>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.CourseTemplates");

            entity.Property(e => e.Code).HasMaxLength(50);
            entity.Property(e => e.Keywords).HasMaxLength(500);
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.RequiredKnowledge).HasMaxLength(200);
            entity.Property(e => e.UrlName).HasMaxLength(200);
        });

        modelBuilder.Entity<CourseTemplateRelation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.CourseTemplateRelations");

            entity.HasOne(d => d.CourseTemplate).WithMany(p => p.CourseTemplateRelationCourseTemplates)
                .HasForeignKey(d => d.CourseTemplateId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("CourseTemplate_CourseTemplateRelation");

            entity.HasOne(d => d.RelatedCourseTemplate).WithMany(p => p.CourseTemplateRelationRelatedCourseTemplates)
                .HasForeignKey(d => d.RelatedCourseTemplateId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("CourseTemplate_CourseTemplateRelation1");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.Customers");

            entity.HasOne(d => d.Address).WithMany(p => p.Customers)
                .HasForeignKey(d => d.AddressId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Address_Customer");
        });

        modelBuilder.Entity<DiscountCoupon>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.DiscountCoupons");

            entity.Property(e => e.Code).HasMaxLength(100);
            entity.Property(e => e.Percent).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.RestrictToCourse).WithMany(p => p.DiscountCoupons)
                .HasForeignKey(d => d.RestrictToCourseId)
                .HasConstraintName("Course_DiscountCoupon");
        });

        modelBuilder.Entity<Invoice>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.Invoices");

            entity.Property(e => e.CorrectedInvoiceNumber).HasMaxLength(20);
            entity.Property(e => e.FileUrl).HasMaxLength(200);
            entity.Property(e => e.Number).HasMaxLength(20);
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TotalAmountWithoutVat)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("TotalAmountWithoutVAT");

            entity.HasOne(d => d.Address).WithMany(p => p.Invoices)
                .HasForeignKey(d => d.AddressId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Address_Invoice");

            entity.HasOne(d => d.Customer).WithMany(p => p.Invoices)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("Customer_Invoice");

            entity.HasOne(d => d.Supplier).WithMany(p => p.Invoices)
                .HasForeignKey(d => d.SupplierId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Supplier_Invoice");
        });

        modelBuilder.Entity<InvoiceLine>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.InvoiceLines");

            entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Units).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Vatrate)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("VATRate");

            entity.HasOne(d => d.Invoice).WithMany(p => p.InvoiceLines)
                .HasForeignKey(d => d.InvoiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Invoice_InvoiceLine");
        });

        modelBuilder.Entity<Lector>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.Lectors");

            entity.Property(e => e.AvatarUrl).HasMaxLength(200);
            entity.Property(e => e.Blog).HasMaxLength(200);
            entity.Property(e => e.CommissionPercent).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CommissionPrice).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.LinkedIn).HasMaxLength(200);
            entity.Property(e => e.Twitter).HasMaxLength(200);
            entity.Property(e => e.UrlName).HasMaxLength(200);
            entity.Property(e => e.Website).HasMaxLength(200);

            entity.HasOne(d => d.CommissionAccount).WithMany(p => p.Lectors)
                .HasForeignKey(d => d.CommissionAccountId)
                .HasConstraintName("Account_Lector");

            entity.HasOne(d => d.DefaultSupplier).WithMany(p => p.Lectors)
                .HasForeignKey(d => d.DefaultSupplierId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Supplier_Lector");

            entity.HasMany(d => d.PrivateCourseRequests).WithMany(p => p.Lectors)
                .UsingEntity<Dictionary<string, object>>(
                    "LectorPrivateCourseRequest",
                    r => r.HasOne<PrivateCourseRequest>().WithMany()
                        .HasForeignKey("PrivateCourseRequestsId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("PrivateCourseRequest_LectorPrivateCourseRequest"),
                    l => l.HasOne<Lector>().WithMany()
                        .HasForeignKey("LectorsId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("Lector_LectorPrivateCourseRequest"),
                    j =>
                    {
                        j.HasKey("LectorsId", "PrivateCourseRequestsId").HasName("PK_dbo.LectorPrivateCourseRequest");
                        j.ToTable("LectorPrivateCourseRequest");
                    });
        });

        modelBuilder.Entity<Location>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.Locations");

            entity.Property(e => e.Address).HasMaxLength(200);
            entity.Property(e => e.City).HasMaxLength(200);
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.Zip)
                .HasMaxLength(200)
                .HasColumnName("ZIP");
        });

        modelBuilder.Entity<MainCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.MainCategories");

            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.UrlName).HasMaxLength(200);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.Orders");

            entity.Property(e => e.BraintreeTransactionId).HasMaxLength(200);
            entity.Property(e => e.Currency).HasMaxLength(20);
            entity.Property(e => e.DiscountPercent).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.HasVat).HasColumnName("HasVAT");
            entity.Property(e => e.TotalPrice).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Vatrate)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("VATRate");

            entity.HasOne(d => d.BillingAddress).WithMany(p => p.Orders)
                .HasForeignKey(d => d.BillingAddressId)
                .HasConstraintName("Address_Order");

            entity.HasOne(d => d.Course).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CourseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Course_Order");

            entity.HasOne(d => d.CourseTemplate).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CourseTemplateId)
                .HasConstraintName("CourseTemplate_Order");

            entity.HasOne(d => d.DiscountCoupon).WithMany(p => p.Orders)
                .HasForeignKey(d => d.DiscountCouponId)
                .HasConstraintName("DiscountCoupon_Order");

            entity.HasOne(d => d.FinalInvoice).WithMany(p => p.OrderFinalInvoices)
                .HasForeignKey(d => d.FinalInvoiceId)
                .HasConstraintName("Invoice_Order");

            entity.HasOne(d => d.ProformaInvoice).WithMany(p => p.OrderProformaInvoices)
                .HasForeignKey(d => d.ProformaInvoiceId)
                .HasConstraintName("Invoice_Order1");
        });

        modelBuilder.Entity<OrderAvailableDate>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.OrderAvailableDates");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderAvailableDates)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Order_OrderAvailableDate");
        });

        modelBuilder.Entity<PrivateCourseRequest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.PrivateCourseRequests");

            entity.Property(e => e.City).HasMaxLength(100);
            entity.Property(e => e.CompanyName).HasMaxLength(100);
        });

        modelBuilder.Entity<Recording>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.Recordings");

            entity.HasOne(d => d.Course).WithMany(p => p.Recordings)
                .HasForeignKey(d => d.CourseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Course_Recording");
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.Suppliers");

            entity.Property(e => e.Currency).HasMaxLength(20);
            entity.Property(e => e.IsVatpayer).HasColumnName("IsVATPayer");
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.Vatrate)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("VATRate");

            entity.HasOne(d => d.Address).WithMany(p => p.Suppliers)
                .HasForeignKey(d => d.AddressId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Address_Supplier");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
