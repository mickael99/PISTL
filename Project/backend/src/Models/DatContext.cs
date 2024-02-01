using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Project.Models;

public partial class DatContext : DbContext
{
    public DatContext()
    {
    }

    public DatContext(DbContextOptions<DatContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Database> Databases { get; set; }

    public virtual DbSet<Domain> Domains { get; set; }

    public virtual DbSet<DomainEnvironment> DomainEnvironments { get; set; }

    public virtual DbSet<DomainLog> DomainLogs { get; set; }

    public virtual DbSet<IpBlacklist> IpBlacklists { get; set; }

    public virtual DbSet<Login> Logins { get; set; }

    public virtual DbSet<LoginAuthorizationException> LoginAuthorizationExceptions { get; set; }

    public virtual DbSet<LoginDomainUser> LoginDomainUsers { get; set; }

    public virtual DbSet<LoginDomainUserLog> LoginDomainUserLogs { get; set; }

    public virtual DbSet<LoginImage> LoginImages { get; set; }

    public virtual DbSet<LoginLog> LoginLogs { get; set; }

    public virtual DbSet<LoginNews> LoginNews { get; set; }

    public virtual DbSet<LoginPasswordRule> LoginPasswordRules { get; set; }

    public virtual DbSet<PasswordHistory> PasswordHistories { get; set; }

    public virtual DbSet<Redirect> Redirects { get; set; }

    public virtual DbSet<Server> Servers { get; set; }

    public virtual DbSet<ServerParameter> ServerParameters { get; set; }

    public virtual DbSet<Session> Sessions { get; set; }

    public virtual DbSet<SingleSignOnCredentialMapping> SingleSignOnCredentialMappings { get; set; }

    public virtual DbSet<SingleSignOnMethod> SingleSignOnMethods { get; set; }

    public virtual DbSet<SingleSignOnSaml2ContactPerson> SingleSignOnSaml2ContactPeople { get; set; }

    public virtual DbSet<SingleSignOnSaml2IdentityProviderInfo> SingleSignOnSaml2IdentityProviderInfos { get; set; }

    public virtual DbSet<SingleSignOnSaml2IdentityProviderInfoDomain> SingleSignOnSaml2IdentityProviderInfoDomains { get; set; }

    public virtual DbSet<UserSetting> UserSettings { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        // => optionsBuilder.UseSqlServer("Server=tcp:datserver2.database.windows.net,1433;Initial Catalog=DAT;Persist Security Info=False;User ID=walter;Password=Daniel123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
        => optionsBuilder.UseSqlServer("Server=database,1434; Database=DAT; User Id=sa; Password=Daniel123; TrustServerCertificate=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Database>(entity =>
        {
            entity.ToTable("Database");

            entity.HasIndex(e => new { e.Name, e.ServerId }, "IX_Database_NameServerID").IsUnique();

            entity.Property(e => e.DatabaseId)
                .ValueGeneratedNever()
                .HasColumnName("DatabaseID");
            entity.Property(e => e.CreatedBy).HasMaxLength(200);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy).HasMaxLength(200);
            entity.Property(e => e.ModifiedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Name)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.ServerId).HasColumnName("ServerID");
            entity.Property(e => e.UserName)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.Server).WithMany(p => p.Databases)
                .HasForeignKey(d => d.ServerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Database_Server");
        });

        modelBuilder.Entity<Domain>(entity =>
        {
            entity.ToTable("Domain", tb =>
                {
                    tb.HasTrigger("tr_Domain_Delete");
                    tb.HasTrigger("tr_Domain_Insert");
                    tb.HasTrigger("tr_Domain_Update");
                });

            entity.Property(e => e.DomainId).HasColumnName("DomainID");
            entity.Property(e => e.CreatedBy).HasMaxLength(200);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Edition).IsUnicode(false);
            entity.Property(e => e.ModifiedBy).HasMaxLength(200);
            entity.Property(e => e.ModifiedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.ParentCompany).HasMaxLength(50);
        });

        modelBuilder.Entity<DomainEnvironment>(entity =>
        {
            entity.ToTable("DomainEnvironment");

            entity.HasIndex(e => new { e.DomainId, e.Environment }, "IX_DomainID_Environment").IsUnique();

            entity.Property(e => e.DomainEnvironmentId).HasColumnName("DomainEnvironmentID");
            entity.Property(e => e.BpdatabaseId).HasColumnName("BPDatabaseID");
            entity.Property(e => e.BpwebServerId).HasColumnName("BPWebServerID");
            entity.Property(e => e.CreatedBy).HasMaxLength(200);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DomainId).HasColumnName("DomainID");
            entity.Property(e => e.EaidatabaseId).HasColumnName("EAIDatabaseID");
            entity.Property(e => e.EaiftpserverId).HasColumnName("EAIFTPServerID");
            entity.Property(e => e.ModifiedBy).HasMaxLength(200);
            entity.Property(e => e.ModifiedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.SsrsserverId).HasColumnName("SSRSServerID");
            entity.Property(e => e.TableauServerId).HasColumnName("TableauServerID");

            entity.HasOne(d => d.Bpdatabase).WithMany(p => p.DomainEnvironmentBpdatabases)
                .HasForeignKey(d => d.BpdatabaseId)
                .HasConstraintName("FK_DomainEnvironment_Database");

            entity.HasOne(d => d.BpwebServer).WithMany(p => p.DomainEnvironmentBpwebServers)
                .HasForeignKey(d => d.BpwebServerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DomainEnvironment_Server");

            entity.HasOne(d => d.Domain).WithMany(p => p.DomainEnvironments)
                .HasForeignKey(d => d.DomainId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DomainEnvironment_Domain");

            entity.HasOne(d => d.Eaidatabase).WithMany(p => p.DomainEnvironmentEaidatabases)
                .HasForeignKey(d => d.EaidatabaseId)
                .HasConstraintName("FK_DomainEnvironment_Database1");

            entity.HasOne(d => d.Eaiftpserver).WithMany(p => p.DomainEnvironmentEaiftpservers)
                .HasForeignKey(d => d.EaiftpserverId)
                .HasConstraintName("FK_DomainEnvironment_Server5");

            entity.HasOne(d => d.Ssrsserver).WithMany(p => p.DomainEnvironmentSsrsservers)
                .HasForeignKey(d => d.SsrsserverId)
                .HasConstraintName("FK_DomainEnvironment_Server1");

            entity.HasOne(d => d.TableauServer).WithMany(p => p.DomainEnvironmentTableauServers)
                .HasForeignKey(d => d.TableauServerId)
                .HasConstraintName("FK_DomainEnvironment_Server4");
        });

        modelBuilder.Entity<DomainLog>(entity =>
        {
            entity.HasKey(e => e.DomainLogId).IsClustered(false);

            entity.ToTable("Domain_Log");

            entity.HasIndex(e => e.DomainId, "IX_Domain_Log").IsClustered();

            entity.Property(e => e.DomainLogId).HasColumnName("Domain_LogID");
            entity.Property(e => e.CreatedBy).HasMaxLength(200);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.DomainId).HasColumnName("DomainID");
            entity.Property(e => e.Edition).IsUnicode(false);
            entity.Property(e => e.LogAction)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.LogDate).HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy).HasMaxLength(200);
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.ParentCompany).HasMaxLength(50);
        });

        modelBuilder.Entity<IpBlacklist>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("IpBlacklist");

            entity.Property(e => e.Ip)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("IP");
        });

        modelBuilder.Entity<Login>(entity =>
        {
            entity.ToTable("Login", tb =>
                {
                    tb.HasTrigger("tr_Login_Delete");
                    tb.HasTrigger("tr_Login_Insert");
                    tb.HasTrigger("tr_Login_Update");
                });

            entity.Property(e => e.LoginId).HasColumnName("LoginID");
            entity.Property(e => e.CreatedBy).HasMaxLength(200);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Datenabled).HasColumnName("DATEnabled");
            entity.Property(e => e.Email).HasMaxLength(200);
            entity.Property(e => e.LastLoginDate).HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy).HasMaxLength(200);
            entity.Property(e => e.ModifiedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.PasswordExpirationDate).HasColumnType("datetime");
            entity.Property(e => e.PasswordModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.PasswordSalt)
                .HasMaxLength(32)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Phone)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ResetPasswordEndDate).HasColumnType("datetime");
            entity.Property(e => e.ResetPasswordKey)
                .HasMaxLength(72)
                .IsUnicode(false);
        });

        modelBuilder.Entity<LoginAuthorizationException>(entity =>
        {
            entity.HasKey(e => e.LoginId);

            entity.ToTable("LoginAuthorizationException");

            entity.Property(e => e.LoginId)
                .ValueGeneratedNever()
                .HasColumnName("LoginID");

            entity.HasOne(d => d.Login).WithOne(p => p.LoginAuthorizationException)
                .HasForeignKey<LoginAuthorizationException>(d => d.LoginId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LoginAuthorizationException_Login");
        });

        modelBuilder.Entity<LoginDomainUser>(entity =>
        {
            entity.HasKey(e => new { e.LoginId, e.DomainId, e.UserId, e.Environment });

            entity.ToTable("LoginDomainUser", tb =>
                {
                    tb.HasTrigger("tr_LoginDomainUser_Delete");
                    tb.HasTrigger("tr_LoginDomainUser_Insert");
                    tb.HasTrigger("tr_LoginDomainUser_Update");
                });

            entity.Property(e => e.LoginId).HasColumnName("LoginID");
            entity.Property(e => e.DomainId).HasColumnName("DomainID");
            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("UserID");
            entity.Property(e => e.Comment).HasMaxLength(2000);
            entity.Property(e => e.CreatedBy).HasMaxLength(200);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DomainLastLoginDate).HasColumnType("datetime");
            entity.Property(e => e.LoginEnabled).HasDefaultValue(true);
            entity.Property(e => e.ModifiedBy).HasMaxLength(200);
            entity.Property(e => e.ModifiedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.SysAdminEndDate).HasColumnType("datetime");
            entity.Property(e => e.SysAdminStartDate).HasColumnType("datetime");
            entity.Property(e => e.UserName).HasMaxLength(200);

            entity.HasOne(d => d.Domain).WithMany(p => p.LoginDomainUsers)
                .HasForeignKey(d => d.DomainId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LoginDomainUser_Domain");

            entity.HasOne(d => d.Login).WithMany(p => p.LoginDomainUsers)
                .HasForeignKey(d => d.LoginId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LoginDomainUser_Login");
        });

        modelBuilder.Entity<LoginDomainUserLog>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("LoginDomainUser_Log");

            entity.HasIndex(e => e.DomainId, "IX_LoginDomainUser_Log").IsClustered();

            entity.Property(e => e.Comment).HasMaxLength(2000);
            entity.Property(e => e.CreatedBy).HasMaxLength(200);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.DomainId).HasColumnName("DomainID");
            entity.Property(e => e.DomainLastLoginDate).HasColumnType("datetime");
            entity.Property(e => e.LogAction)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.LogDate).HasColumnType("datetime");
            entity.Property(e => e.LoginDomainUserLogId)
                .ValueGeneratedOnAdd()
                .HasColumnName("LoginDomainUser_LogID");
            entity.Property(e => e.LoginId).HasColumnName("LoginID");
            entity.Property(e => e.ModifiedBy).HasMaxLength(200);
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.SysAdminEndDate).HasColumnType("datetime");
            entity.Property(e => e.SysAdminStartDate).HasColumnType("datetime");
            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("UserID");
            entity.Property(e => e.UserName).HasMaxLength(200);
        });

        modelBuilder.Entity<LoginImage>(entity =>
        {
            entity.ToTable("LoginImage");

            entity.Property(e => e.LoginImageId).HasColumnName("LoginImageID");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.ExecutionContext)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(1024)
                .IsUnicode(false);
        });

        modelBuilder.Entity<LoginLog>(entity =>
        {
            entity.HasKey(e => e.LoginLogId).IsClustered(false);

            entity.ToTable("Login_Log");

            entity.HasIndex(e => e.LoginId, "IX_Login_Log").IsClustered();

            entity.Property(e => e.LoginLogId).HasColumnName("Login_LogID");
            entity.Property(e => e.CreatedBy).HasMaxLength(200);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Datenabled).HasColumnName("DATEnabled");
            entity.Property(e => e.Email).HasMaxLength(200);
            entity.Property(e => e.LastLoginDate).HasColumnType("datetime");
            entity.Property(e => e.LogAction)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.LogDate).HasColumnType("datetime");
            entity.Property(e => e.LoginId).HasColumnName("LoginID");
            entity.Property(e => e.ModifiedBy).HasMaxLength(200);
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.PasswordExpirationDate).HasColumnType("datetime");
            entity.Property(e => e.PasswordModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.PasswordSalt)
                .HasMaxLength(32)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Phone)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ResetPasswordEndDate).HasColumnType("datetime");
            entity.Property(e => e.ResetPasswordKey)
                .HasMaxLength(72)
                .IsUnicode(false);
        });

        modelBuilder.Entity<LoginNews>(entity =>
        {
            entity.Property(e => e.LoginNewsId).HasColumnName("LoginNewsID");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(1024)
                .IsUnicode(false);
            entity.Property(e => e.LinkUrl)
                .HasMaxLength(1024)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.Title).HasMaxLength(200);
        });

        modelBuilder.Entity<LoginPasswordRule>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("LoginPasswordRule");

            entity.Property(e => e.DictionaryItemCode).HasMaxLength(100);
            entity.Property(e => e.DomainId).HasColumnName("DomainID");
            entity.Property(e => e.InternalDescription)
                .HasMaxLength(510)
                .IsUnicode(false);
            entity.Property(e => e.PasswordRuleId).HasColumnName("PasswordRuleID");
        });

        modelBuilder.Entity<PasswordHistory>(entity =>
        {
            entity.HasKey(e => e.PasswordId).HasName("PK_PasswordHistory_1");

            entity.ToTable("PasswordHistory");

            entity.Property(e => e.PasswordId).HasColumnName("PasswordID");
            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.LoginId).HasColumnName("LoginID");
            entity.Property(e => e.PasswordSalt)
                .HasMaxLength(32)
                .IsUnicode(false)
                .IsFixedLength();

            entity.HasOne(d => d.Login).WithMany(p => p.PasswordHistories)
                .HasForeignKey(d => d.LoginId)
                .HasConstraintName("FK_PasswordHistory_Login");
        });

        modelBuilder.Entity<Redirect>(entity =>
        {
            entity.ToTable("Redirect");

            entity.Property(e => e.RedirectId)
                .ValueGeneratedNever()
                .HasColumnName("RedirectID");
            entity.Property(e => e.Action)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Data).IsUnicode(false);
            entity.Property(e => e.DomainId).HasColumnName("DomainID");
            entity.Property(e => e.Email).HasMaxLength(200);
            entity.Property(e => e.ExpirationDate).HasColumnType("datetime");
            entity.Property(e => e.ProfileId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ProfileID");
        });

        modelBuilder.Entity<Server>(entity =>
        {
            entity.ToTable("Server");

            entity.Property(e => e.ServerId)
                .ValueGeneratedNever()
                .HasColumnName("ServerID");
            entity.Property(e => e.Address)
                .HasMaxLength(1024)
                .IsUnicode(false);
            entity.Property(e => e.Bp5Address)
                .HasMaxLength(1024)
                .IsUnicode(false);
            entity.Property(e => e.Bp5FrontAddress)
                .HasMaxLength(1024)
                .IsUnicode(false);
            entity.Property(e => e.CreatedBy).HasMaxLength(200);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy).HasMaxLength(200);
            entity.Property(e => e.ModifiedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ResourceGroup)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.SubscriptionId).HasColumnName("SubscriptionID");
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ServerParameter>(entity =>
        {
            entity.HasKey(e => new { e.ServerId, e.ParameterKey });

            entity.ToTable("ServerParameter");

            entity.Property(e => e.ServerId).HasColumnName("ServerID");
            entity.Property(e => e.ParameterKey)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Session>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Session_1");

            entity.ToTable("Session");

            entity.HasIndex(e => new { e.LoginId, e.Action, e.Date }, "IXN_Session_LoginID_Action_Date");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Action)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Data).IsUnicode(false);
            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.LoginId).HasColumnName("LoginID");
            entity.Property(e => e.SessionId).HasColumnName("SessionID");
            entity.Property(e => e.UserAgent).IsUnicode(false);
            entity.Property(e => e.UserHostAddress)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UserHostName).IsUnicode(false);

            entity.HasOne(d => d.Login).WithMany(p => p.Sessions)
                .HasForeignKey(d => d.LoginId)
                .HasConstraintName("FK_Session_Login");
        });

        modelBuilder.Entity<SingleSignOnCredentialMapping>(entity =>
        {
            entity.HasKey(e => e.CredentialMappingId);

            entity.ToTable("SingleSignOnCredentialMapping");

            entity.Property(e => e.CredentialMappingId).HasColumnName("CredentialMappingID");
            entity.Property(e => e.DomainId).HasColumnName("DomainID");
            entity.Property(e => e.LoginId).HasColumnName("LoginID");
            entity.Property(e => e.ModifiedBy).HasMaxLength(200);
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.Saml2IdentityProviderId).HasColumnName("Saml2IdentityProviderID");
            entity.Property(e => e.SourcePrincipalName)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.Domain).WithMany(p => p.SingleSignOnCredentialMappings)
                .HasForeignKey(d => d.DomainId)
                .HasConstraintName("FK_SingleSignOnCredentialMapping_Domain");

            entity.HasOne(d => d.Login).WithMany(p => p.SingleSignOnCredentialMappings)
                .HasForeignKey(d => d.LoginId)
                .HasConstraintName("FK_SingleSignOnCredentialMapping_Login");

            entity.HasOne(d => d.Saml2IdentityProvider).WithMany(p => p.SingleSignOnCredentialMappings)
                .HasForeignKey(d => d.Saml2IdentityProviderId)
                .HasConstraintName("FK_SingleSignOnCredentialMapping_SingleSignOnSaml2IdentityProviderInfo");
        });

        modelBuilder.Entity<SingleSignOnMethod>(entity =>
        {
            entity.HasKey(e => e.SingleSignOnId).HasName("PK__SingleSi__CB1BECB8D19D0446");

            entity.ToTable("SingleSignOnMethod");

            entity.HasIndex(e => e.UrlHost, "UQ_SingleSignOnMethod_UrlHost").IsUnique();

            entity.Property(e => e.SingleSignOnId).HasColumnName("SingleSignOnID");
            entity.Property(e => e.DomainId).HasColumnName("DomainID");
            entity.Property(e => e.MethodUrl)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedBy).HasMaxLength(200);
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Saml2IdentityProviderId).HasColumnName("Saml2IdentityProviderID");
            entity.Property(e => e.UrlHost)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.Domain).WithMany(p => p.SingleSignOnMethods)
                .HasForeignKey(d => d.DomainId)
                .HasConstraintName("FK_SingleSignOnMethod_Domain");

            entity.HasOne(d => d.Saml2IdentityProvider).WithMany(p => p.SingleSignOnMethods)
                .HasForeignKey(d => d.Saml2IdentityProviderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Fk_SingleSignOnMethod_SingleSignOnSaml2IdentityProviderInfo");
        });

        modelBuilder.Entity<SingleSignOnSaml2ContactPerson>(entity =>
        {
            entity.HasKey(e => e.ContactPersonId).HasName("PK__SingleSi__97C702DE854896F8");

            entity.ToTable("SingleSignOnSaml2ContactPerson");

            entity.Property(e => e.ContactPersonId).HasColumnName("ContactPersonID");
            entity.Property(e => e.Company)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.EmailAddresses)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.GivenName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedBy).HasMaxLength(200);
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.Surname)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<SingleSignOnSaml2IdentityProviderInfo>(entity =>
        {
            entity.HasKey(e => e.Saml2IdentityProviderId);

            entity.ToTable("SingleSignOnSaml2IdentityProviderInfo");

            entity.Property(e => e.Saml2IdentityProviderId).HasColumnName("Saml2IdentityProviderID");
            entity.Property(e => e.AudienceId)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("AudienceID");
            entity.Property(e => e.CertificateFileName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DomainId).HasColumnName("DomainID");
            entity.Property(e => e.FederationMetaDataDocumentUri)
                .HasMaxLength(2000)
                .IsUnicode(false);
            entity.Property(e => e.IdentityProviderName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedBy).HasMaxLength(200);
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.RequestEntityId)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("RequestEntityID");
            entity.Property(e => e.ResponseEntityId)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("ResponseEntityID");
            entity.Property(e => e.Subject)
                .HasMaxLength(500)
                .IsUnicode(false);

            entity.HasOne(d => d.Domain).WithMany(p => p.SingleSignOnSaml2IdentityProviderInfos)
                .HasForeignKey(d => d.DomainId)
                .HasConstraintName("FK_SingleSignOnSaml2IdentityProviderInfo_Domain");
        });

        modelBuilder.Entity<SingleSignOnSaml2IdentityProviderInfoDomain>(entity =>
        {
            entity.HasKey(e => e.Saml2IdentityProviderDomainId);

            entity.ToTable("SingleSignOnSaml2IdentityProviderInfoDomain");

            entity.Property(e => e.Saml2IdentityProviderDomainId).HasColumnName("Saml2IdentityProviderDomainID");
            entity.Property(e => e.DomainId).HasColumnName("DomainID");
            entity.Property(e => e.ModifiedBy).HasMaxLength(200);
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.Saml2IdentityProviderId).HasColumnName("Saml2IdentityProviderID");

            entity.HasOne(d => d.Domain).WithMany(p => p.SingleSignOnSaml2IdentityProviderInfoDomains)
                .HasForeignKey(d => d.DomainId)
                .HasConstraintName("FK_SingleSignOnSaml2IdentityProviderInfoDomain_Domain");

            entity.HasOne(d => d.Saml2IdentityProvider).WithMany(p => p.SingleSignOnSaml2IdentityProviderInfoDomains)
                .HasForeignKey(d => d.Saml2IdentityProviderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SingleSignOnSaml2IdentityProviderInfoDomain_SingleSignOnSaml2IdentityProviderInfo");
        });

        modelBuilder.Entity<UserSetting>(entity =>
        {
            entity.HasKey(e => e.UserSettingId).HasName("PK_UserSetting");

            entity.Property(e => e.UserSettingId).HasColumnName("UserSettingID");
            entity.Property(e => e.DomainId).HasColumnName("DomainID");
            entity.Property(e => e.LoginId).HasColumnName("LoginID");
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Settings).IsUnicode(false);

            entity.HasOne(d => d.Domain).WithMany(p => p.UserSettings)
                .HasForeignKey(d => d.DomainId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserSetting_Domain");

            entity.HasOne(d => d.Login).WithMany(p => p.UserSettings)
                .HasForeignKey(d => d.LoginId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserSetting_Login");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
