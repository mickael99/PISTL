using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAT_project.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Domain",
                columns: table => new
                {
                    DomainID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Logo = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    Edition = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    IsSsoEnabled = table.Column<bool>(type: "bit", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentCompany = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    CreatedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    ModifiedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Domain", x => x.DomainID);
                });

            migrationBuilder.CreateTable(
                name: "Domain_Log",
                columns: table => new
                {
                    Domain_LogID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LogAction = table.Column<string>(type: "char(1)", unicode: false, fixedLength: true, maxLength: 1, nullable: false),
                    LogDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    DomainID = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Logo = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    Edition = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    IsSsoEnabled = table.Column<bool>(type: "bit", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentCompany = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Domain_Log", x => x.Domain_LogID)
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateTable(
                name: "IpBlacklist",
                columns: table => new
                {
                    IP = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "Login",
                columns: table => new
                {
                    LoginID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Password = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    PasswordSalt = table.Column<string>(type: "char(32)", unicode: false, fixedLength: true, maxLength: 32, nullable: true),
                    PasswordModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    PasswordExpirationDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    InvalidAttemptCount = table.Column<int>(type: "int", nullable: true),
                    ResetPasswordEndDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ResetPasswordKey = table.Column<string>(type: "varchar(72)", unicode: false, maxLength: 72, nullable: true),
                    ResetPasswordSentCount = table.Column<short>(type: "smallint", nullable: false),
                    ResetPasswordInvalidAttemptCount = table.Column<short>(type: "smallint", nullable: false),
                    LastLoginDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    TermsAccepted = table.Column<bool>(type: "bit", nullable: false),
                    DATEnabled = table.Column<bool>(type: "bit", nullable: true),
                    Phone = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    BlockedReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    CreatedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    ModifiedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Login", x => x.LoginID);
                });

            migrationBuilder.CreateTable(
                name: "Login_Log",
                columns: table => new
                {
                    Login_LogID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LogAction = table.Column<string>(type: "char(1)", unicode: false, fixedLength: true, maxLength: 1, nullable: false),
                    LogDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    LoginID = table.Column<int>(type: "int", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Password = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    PasswordSalt = table.Column<string>(type: "char(32)", unicode: false, fixedLength: true, maxLength: 32, nullable: true),
                    PasswordModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    PasswordExpirationDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    InvalidAttemptCount = table.Column<int>(type: "int", nullable: true),
                    ResetPasswordEndDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ResetPasswordKey = table.Column<string>(type: "varchar(72)", unicode: false, maxLength: 72, nullable: true),
                    ResetPasswordSentCount = table.Column<short>(type: "smallint", nullable: false),
                    ResetPasswordInvalidAttemptCount = table.Column<short>(type: "smallint", nullable: false),
                    LastLoginDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    TermsAccepted = table.Column<bool>(type: "bit", nullable: false),
                    DATEnabled = table.Column<bool>(type: "bit", nullable: true),
                    Phone = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    BlockedReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Login_Log", x => x.Login_LogID)
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateTable(
                name: "LoginDomainUser_Log",
                columns: table => new
                {
                    LoginDomainUser_LogID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LogAction = table.Column<string>(type: "char(1)", unicode: false, fixedLength: true, maxLength: 1, nullable: false),
                    LogDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    LoginID = table.Column<int>(type: "int", nullable: false),
                    DomainID = table.Column<int>(type: "int", nullable: false),
                    UserID = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Environment = table.Column<int>(type: "int", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    UserActive = table.Column<bool>(type: "bit", nullable: false),
                    LoginEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LoginTypeId = table.Column<int>(type: "int", nullable: true),
                    AnalyticsEnabled = table.Column<bool>(type: "bit", nullable: true),
                    IsLight = table.Column<bool>(type: "bit", nullable: true),
                    SysAdmin = table.Column<bool>(type: "bit", nullable: false),
                    SysAdminStartDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    SysAdminEndDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    DomainLastLoginDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Comment = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "LoginImage",
                columns: table => new
                {
                    LoginImageID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExecutionContext = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    ImageUrl = table.Column<string>(type: "varchar(1024)", unicode: false, maxLength: 1024, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    CreatedBy = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoginImage", x => x.LoginImageID);
                });

            migrationBuilder.CreateTable(
                name: "LoginNews",
                columns: table => new
                {
                    LoginNewsID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImageUrl = table.Column<string>(type: "varchar(1024)", unicode: false, maxLength: 1024, nullable: false),
                    LinkUrl = table.Column<string>(type: "varchar(1024)", unicode: false, maxLength: 1024, nullable: true),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    CreatedBy = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoginNews", x => x.LoginNewsID);
                });

            migrationBuilder.CreateTable(
                name: "LoginPasswordRule",
                columns: table => new
                {
                    PasswordRuleID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DomainID = table.Column<int>(type: "int", nullable: false),
                    InternalDescription = table.Column<string>(type: "varchar(510)", unicode: false, maxLength: 510, nullable: false),
                    DictionaryItemCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Pattern = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "Redirect",
                columns: table => new
                {
                    RedirectID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Action = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Data = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DomainID = table.Column<int>(type: "int", nullable: false),
                    Environment = table.Column<int>(type: "int", nullable: false),
                    ProfileID = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Redirect", x => x.RedirectID);
                });

            migrationBuilder.CreateTable(
                name: "Server",
                columns: table => new
                {
                    ServerID = table.Column<int>(type: "int", nullable: false),
                    Address = table.Column<string>(type: "varchar(1024)", unicode: false, maxLength: 1024, nullable: false),
                    Bp5Address = table.Column<string>(type: "varchar(1024)", unicode: false, maxLength: 1024, nullable: true),
                    Bp5FrontAddress = table.Column<string>(type: "varchar(1024)", unicode: false, maxLength: 1024, nullable: true),
                    Name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Context = table.Column<int>(type: "int", nullable: true),
                    Type = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    SubscriptionID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ResourceGroup = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    CreatedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    ModifiedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Server", x => x.ServerID);
                });

            migrationBuilder.CreateTable(
                name: "ServerParameter",
                columns: table => new
                {
                    ServerID = table.Column<int>(type: "int", nullable: false),
                    ParameterKey = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    ParameterValue = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServerParameter", x => new { x.ServerID, x.ParameterKey });
                });

            migrationBuilder.CreateTable(
                name: "SingleSignOnSaml2ContactPerson",
                columns: table => new
                {
                    ContactPersonID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Company = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    EmailAddresses = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    GivenName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Surname = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__SingleSi__97C702DE6E6F4D07", x => x.ContactPersonID);
                });

            migrationBuilder.CreateTable(
                name: "SingleSignOnSaml2IdentityProviderInfo",
                columns: table => new
                {
                    Saml2IdentityProviderID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DomainID = table.Column<int>(type: "int", nullable: true),
                    IdentityProviderName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    RequestEntityID = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: false),
                    ResponseEntityID = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: false),
                    Subject = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: false),
                    CertificateFileName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    AudienceID = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: false),
                    IsCertificateRequired = table.Column<bool>(type: "bit", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    FederationMetaDataDocumentUri = table.Column<string>(type: "varchar(2000)", unicode: false, maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SingleSignOnSaml2IdentityProviderInfo", x => x.Saml2IdentityProviderID);
                    table.ForeignKey(
                        name: "FK_SingleSignOnSaml2IdentityProviderInfo_Domain",
                        column: x => x.DomainID,
                        principalTable: "Domain",
                        principalColumn: "DomainID");
                });

            migrationBuilder.CreateTable(
                name: "LoginAuthorizationException",
                columns: table => new
                {
                    LoginID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoginAuthorizationException", x => x.LoginID);
                    table.ForeignKey(
                        name: "FK_LoginAuthorizationException_Login",
                        column: x => x.LoginID,
                        principalTable: "Login",
                        principalColumn: "LoginID");
                });

            migrationBuilder.CreateTable(
                name: "PasswordHistory",
                columns: table => new
                {
                    PasswordID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LoginID = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime", nullable: false),
                    Password = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    PasswordSalt = table.Column<string>(type: "char(32)", unicode: false, fixedLength: true, maxLength: 32, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PasswordHistory_1", x => x.PasswordID);
                    table.ForeignKey(
                        name: "FK_PasswordHistory_Login",
                        column: x => x.LoginID,
                        principalTable: "Login",
                        principalColumn: "LoginID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Session",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SessionID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime", nullable: false),
                    LoginID = table.Column<int>(type: "int", nullable: false),
                    Action = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    UserHostAddress = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    UserHostName = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    UserAgent = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    Data = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Session_1", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Session_Login",
                        column: x => x.LoginID,
                        principalTable: "Login",
                        principalColumn: "LoginID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserSettings",
                columns: table => new
                {
                    UserSettingID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DomainID = table.Column<int>(type: "int", nullable: false),
                    LoginID = table.Column<int>(type: "int", nullable: false),
                    Settings = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "date", nullable: false, defaultValueSql: "(getdate())"),
                    ModifiedBy = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSetting", x => x.UserSettingID);
                    table.ForeignKey(
                        name: "FK_UserSetting_Domain",
                        column: x => x.DomainID,
                        principalTable: "Domain",
                        principalColumn: "DomainID");
                    table.ForeignKey(
                        name: "FK_UserSetting_Login",
                        column: x => x.LoginID,
                        principalTable: "Login",
                        principalColumn: "LoginID");
                });

            migrationBuilder.CreateTable(
                name: "Database",
                columns: table => new
                {
                    DatabaseID = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    UserName = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Context = table.Column<int>(type: "int", nullable: true),
                    ServerID = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    CreatedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    ModifiedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Database", x => x.DatabaseID);
                    table.ForeignKey(
                        name: "FK_Database_Server",
                        column: x => x.ServerID,
                        principalTable: "Server",
                        principalColumn: "ServerID");
                });

            migrationBuilder.CreateTable(
                name: "SingleSignOnCredentialMapping",
                columns: table => new
                {
                    CredentialMappingID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DomainID = table.Column<int>(type: "int", nullable: true),
                    SourcePrincipalName = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    LoginID = table.Column<int>(type: "int", nullable: false),
                    Saml2IdentityProviderID = table.Column<int>(type: "int", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SingleSignOnCredentialMapping", x => x.CredentialMappingID);
                    table.ForeignKey(
                        name: "FK_SingleSignOnCredentialMapping_Domain",
                        column: x => x.DomainID,
                        principalTable: "Domain",
                        principalColumn: "DomainID");
                    table.ForeignKey(
                        name: "FK_SingleSignOnCredentialMapping_Login",
                        column: x => x.LoginID,
                        principalTable: "Login",
                        principalColumn: "LoginID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SingleSignOnCredentialMapping_SingleSignOnSaml2IdentityProviderInfo",
                        column: x => x.Saml2IdentityProviderID,
                        principalTable: "SingleSignOnSaml2IdentityProviderInfo",
                        principalColumn: "Saml2IdentityProviderID");
                });

            migrationBuilder.CreateTable(
                name: "SingleSignOnMethod",
                columns: table => new
                {
                    SingleSignOnID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DomainID = table.Column<int>(type: "int", nullable: true),
                    UrlHost = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    MethodUrl = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Saml2IdentityProviderID = table.Column<int>(type: "int", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__SingleSi__CB1BECB8EB71497D", x => x.SingleSignOnID);
                    table.ForeignKey(
                        name: "FK_SingleSignOnMethod_Domain",
                        column: x => x.DomainID,
                        principalTable: "Domain",
                        principalColumn: "DomainID");
                    table.ForeignKey(
                        name: "Fk_SingleSignOnMethod_SingleSignOnSaml2IdentityProviderInfo",
                        column: x => x.Saml2IdentityProviderID,
                        principalTable: "SingleSignOnSaml2IdentityProviderInfo",
                        principalColumn: "Saml2IdentityProviderID");
                });

            migrationBuilder.CreateTable(
                name: "SingleSignOnSaml2IdentityProviderInfoDomain",
                columns: table => new
                {
                    Saml2IdentityProviderDomainID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Saml2IdentityProviderID = table.Column<int>(type: "int", nullable: false),
                    DomainID = table.Column<int>(type: "int", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SingleSignOnSaml2IdentityProviderInfoDomain", x => x.Saml2IdentityProviderDomainID);
                    table.ForeignKey(
                        name: "FK_SingleSignOnSaml2IdentityProviderInfoDomain_Domain",
                        column: x => x.DomainID,
                        principalTable: "Domain",
                        principalColumn: "DomainID");
                    table.ForeignKey(
                        name: "FK_SingleSignOnSaml2IdentityProviderInfoDomain_SingleSignOnSaml2IdentityProviderInfo",
                        column: x => x.Saml2IdentityProviderID,
                        principalTable: "SingleSignOnSaml2IdentityProviderInfo",
                        principalColumn: "Saml2IdentityProviderID");
                });

            migrationBuilder.CreateTable(
                name: "DomainEnvironment",
                columns: table => new
                {
                    DomainEnvironmentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DomainID = table.Column<int>(type: "int", nullable: false),
                    Environment = table.Column<int>(type: "int", nullable: false),
                    BPWebServerID = table.Column<int>(type: "int", nullable: false),
                    BPDatabaseID = table.Column<int>(type: "int", nullable: true),
                    EAIDatabaseID = table.Column<int>(type: "int", nullable: true),
                    SSRSServerID = table.Column<int>(type: "int", nullable: true),
                    TableauServerID = table.Column<int>(type: "int", nullable: true),
                    EAIFTPServerID = table.Column<int>(type: "int", nullable: true),
                    IsBp5Enabled = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    CreatedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    ModifiedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DomainEnvironment", x => x.DomainEnvironmentID);
                    table.ForeignKey(
                        name: "FK_DomainEnvironment_Database",
                        column: x => x.BPDatabaseID,
                        principalTable: "Database",
                        principalColumn: "DatabaseID");
                    table.ForeignKey(
                        name: "FK_DomainEnvironment_Database1",
                        column: x => x.EAIDatabaseID,
                        principalTable: "Database",
                        principalColumn: "DatabaseID");
                    table.ForeignKey(
                        name: "FK_DomainEnvironment_Domain",
                        column: x => x.DomainID,
                        principalTable: "Domain",
                        principalColumn: "DomainID");
                    table.ForeignKey(
                        name: "FK_DomainEnvironment_Server",
                        column: x => x.BPWebServerID,
                        principalTable: "Server",
                        principalColumn: "ServerID");
                    table.ForeignKey(
                        name: "FK_DomainEnvironment_Server1",
                        column: x => x.SSRSServerID,
                        principalTable: "Server",
                        principalColumn: "ServerID");
                    table.ForeignKey(
                        name: "FK_DomainEnvironment_Server4",
                        column: x => x.TableauServerID,
                        principalTable: "Server",
                        principalColumn: "ServerID");
                    table.ForeignKey(
                        name: "FK_DomainEnvironment_Server5",
                        column: x => x.EAIFTPServerID,
                        principalTable: "Server",
                        principalColumn: "ServerID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Database_NameServerID",
                table: "Database",
                columns: new[] { "Name", "ServerID" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Database_ServerID",
                table: "Database",
                column: "ServerID");

            migrationBuilder.CreateIndex(
                name: "IX_Domain_Log",
                table: "Domain_Log",
                column: "DomainID")
                .Annotation("SqlServer:Clustered", true);

            migrationBuilder.CreateIndex(
                name: "IX_DomainEnvironment_BPDatabaseID",
                table: "DomainEnvironment",
                column: "BPDatabaseID");

            migrationBuilder.CreateIndex(
                name: "IX_DomainEnvironment_BPWebServerID",
                table: "DomainEnvironment",
                column: "BPWebServerID");

            migrationBuilder.CreateIndex(
                name: "IX_DomainEnvironment_EAIDatabaseID",
                table: "DomainEnvironment",
                column: "EAIDatabaseID");

            migrationBuilder.CreateIndex(
                name: "IX_DomainEnvironment_EAIFTPServerID",
                table: "DomainEnvironment",
                column: "EAIFTPServerID");

            migrationBuilder.CreateIndex(
                name: "IX_DomainEnvironment_SSRSServerID",
                table: "DomainEnvironment",
                column: "SSRSServerID");

            migrationBuilder.CreateIndex(
                name: "IX_DomainEnvironment_TableauServerID",
                table: "DomainEnvironment",
                column: "TableauServerID");

            migrationBuilder.CreateIndex(
                name: "IX_DomainID_Environment",
                table: "DomainEnvironment",
                columns: new[] { "DomainID", "Environment" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Login_Log",
                table: "Login_Log",
                column: "LoginID")
                .Annotation("SqlServer:Clustered", true);

            migrationBuilder.CreateIndex(
                name: "IX_LoginDomainUser_Log",
                table: "LoginDomainUser_Log",
                column: "DomainID")
                .Annotation("SqlServer:Clustered", true);

            migrationBuilder.CreateIndex(
                name: "IX_PasswordHistory_LoginID",
                table: "PasswordHistory",
                column: "LoginID");

            migrationBuilder.CreateIndex(
                name: "IXN_Session_LoginID_Action_Date",
                table: "Session",
                columns: new[] { "LoginID", "Action", "Date" });

            migrationBuilder.CreateIndex(
                name: "IX_SingleSignOnCredentialMapping_DomainID",
                table: "SingleSignOnCredentialMapping",
                column: "DomainID");

            migrationBuilder.CreateIndex(
                name: "IX_SingleSignOnCredentialMapping_LoginID",
                table: "SingleSignOnCredentialMapping",
                column: "LoginID");

            migrationBuilder.CreateIndex(
                name: "IX_SingleSignOnCredentialMapping_Saml2IdentityProviderID",
                table: "SingleSignOnCredentialMapping",
                column: "Saml2IdentityProviderID");

            migrationBuilder.CreateIndex(
                name: "IX_SingleSignOnMethod_DomainID",
                table: "SingleSignOnMethod",
                column: "DomainID");

            migrationBuilder.CreateIndex(
                name: "IX_SingleSignOnMethod_Saml2IdentityProviderID",
                table: "SingleSignOnMethod",
                column: "Saml2IdentityProviderID");

            migrationBuilder.CreateIndex(
                name: "UQ_SingleSignOnMethod_UrlHost",
                table: "SingleSignOnMethod",
                column: "UrlHost",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SingleSignOnSaml2IdentityProviderInfo_DomainID",
                table: "SingleSignOnSaml2IdentityProviderInfo",
                column: "DomainID");

            migrationBuilder.CreateIndex(
                name: "IX_SingleSignOnSaml2IdentityProviderInfoDomain_DomainID",
                table: "SingleSignOnSaml2IdentityProviderInfoDomain",
                column: "DomainID");

            migrationBuilder.CreateIndex(
                name: "IX_SingleSignOnSaml2IdentityProviderInfoDomain_Saml2IdentityProviderID",
                table: "SingleSignOnSaml2IdentityProviderInfoDomain",
                column: "Saml2IdentityProviderID");

            migrationBuilder.CreateIndex(
                name: "IX_UserSettings_DomainID",
                table: "UserSettings",
                column: "DomainID");

            migrationBuilder.CreateIndex(
                name: "IX_UserSettings_LoginID",
                table: "UserSettings",
                column: "LoginID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Domain_Log");

            migrationBuilder.DropTable(
                name: "DomainEnvironment");

            migrationBuilder.DropTable(
                name: "IpBlacklist");

            migrationBuilder.DropTable(
                name: "Login_Log");

            migrationBuilder.DropTable(
                name: "LoginAuthorizationException");

            migrationBuilder.DropTable(
                name: "LoginDomainUser_Log");

            migrationBuilder.DropTable(
                name: "LoginImage");

            migrationBuilder.DropTable(
                name: "LoginNews");

            migrationBuilder.DropTable(
                name: "LoginPasswordRule");

            migrationBuilder.DropTable(
                name: "PasswordHistory");

            migrationBuilder.DropTable(
                name: "Redirect");

            migrationBuilder.DropTable(
                name: "ServerParameter");

            migrationBuilder.DropTable(
                name: "Session");

            migrationBuilder.DropTable(
                name: "SingleSignOnCredentialMapping");

            migrationBuilder.DropTable(
                name: "SingleSignOnMethod");

            migrationBuilder.DropTable(
                name: "SingleSignOnSaml2ContactPerson");

            migrationBuilder.DropTable(
                name: "SingleSignOnSaml2IdentityProviderInfoDomain");

            migrationBuilder.DropTable(
                name: "UserSettings");

            migrationBuilder.DropTable(
                name: "Database");

            migrationBuilder.DropTable(
                name: "SingleSignOnSaml2IdentityProviderInfo");

            migrationBuilder.DropTable(
                name: "Login");

            migrationBuilder.DropTable(
                name: "Server");

            migrationBuilder.DropTable(
                name: "Domain");
        }
    }
}
