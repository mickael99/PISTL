using Microsoft.EntityFrameworkCore;
using Project.Models;
using System.Security.Cryptography;
using System.Text;

/***************************************************************************************/
/***************************************************************************************/
/***************************************************************************************/
namespace Project.Controllers.Login;

public class Utils
{
  /***************************************************************************************/
  /// <summary>
  /// Adds a new login with the specified email and password to the database.
  /// </summary>
  /// <param name="email">The email of the login.</param>
  /// <param name="password">The password of the login.</param>
  public static void AddLogin(string email, string password)
  {
    string passwordSalt = GetSalt(24);
    byte[] passwordHash = EncryptPassword(password, passwordSalt);
    // _create_password_hash(password, out passwordHash, out passwordSalt);
    var newLogin = new Project.Models.Login
    {
      Email = email,
      Name = "Daniel",
      Password = passwordHash,
      PasswordSalt = passwordSalt,
      PasswordModifiedDate = DateTime.Now,
      PasswordExpirationDate = DateTime.Now.AddDays(30),
      InvalidAttemptCount = 0,
      ResetPasswordEndDate = DateTime.Now.AddDays(1),
      ResetPasswordKey = null,
      ResetPasswordSentCount = 1,
      ResetPasswordInvalidAttemptCount = 1,
      LastLoginDate = DateTime.Now,
      TermsAccepted = true,
      Datenabled = true,
      Phone = "123456789",
      BlockedReason = "No reason",
      CreatedDate = DateTime.Now,
      CreatedBy = "Daniel",
      ModifiedDate = DateTime.Now,
      ModifiedBy = "Daniel"
    };
    using (var context = new DatContext())
    {
      context.Logins.Add(newLogin);
      context.SaveChanges();
    }
  }

  /***************************************************************************************/
  /// <summary>
  /// Adds a new domain to the database.
  /// </summary>
  /// <param name="domainName">The name of the domain.</param>
  static void addDomain(string domainName)
  {
    var newDomain = new Domain
    {
      Name = domainName,
      Logo = Encoding.UTF8.GetBytes("web"),
      Edition = "Enterprise",
      IsSsoEnabled = true,
      Comment = "No comment",
      ParentCompany = "No parent company",
      CreatedDate = DateTime.Now,
      CreatedBy = "Daniel",
      ModifiedDate = DateTime.Now,
      ModifiedBy = "Daniel"
    };
    var context = new DatContext();
    context.Domains.Add(newDomain);
    context.SaveChanges();
  }

  /***************************************************************************************/
  /// <summary>
  /// Generates a random salt.
  /// </summary>
  /// <param name="size">The size of the salt.</param>
  /// <returns>The generated salt.</returns>
  public static string GetSalt(int size)
  {
    return Convert.ToBase64String(RandomNumberGenerator.GetBytes(size));
  }

  /***************************************************************************************/
  /// <summary>
  /// Encrypts the password using SHA512 algorithm. // TODO change to PBKDF2
  /// </summary>
  /// <param name="password">The password to be encrypted.</param>
  /// <param name="salt">The salt to be used.</param>
  /// <returns>The encrypted password.</returns>
  public static byte[] EncryptPassword(string password, string salt)
  {
    // if (!string.IsNullOrWhiteSpace(salt)) salt += SECRET; // TODO pourquoi utiliser la variable SECRET?
    var bytes = Encoding.UTF8.GetBytes(password + salt);
    var encryptedBytes = SHA512.HashData(bytes);
    return encryptedBytes;
  }

  /***************************************************************************************/
  /// <summary>
  /// Removes the login with the specified email from the database.
  /// </summary>
  /// <param name="email">The email of the login to be removed.</param>
  public static void remove_login(string email) // TODO faire avec un where/find
  {
    using var context = new DatContext();
    var logins = context.Logins;
    Models.Login? loginOK = null;
    foreach (var login in logins)
    {
      if (login.Email == email)
      {
        loginOK = login;
        break;
      }
    }
    if (loginOK != null)
    {
      context.Logins.Remove(loginOK);
      context.SaveChanges();
    }
    return;
  }

  /***************************************************************************************/
  /// <summary>
  /// Generates a random string of the specified length.
  /// </summary>
  /// <param name="length">The length of the string to be generated.</param>
  /// <param name="allowableChars">The characters that can be used to generate the string.</param>
  /// <returns>The generated string.</returns>
  public static string GenerateRandomString(int length, string? allowableChars = null)
  {
    if (string.IsNullOrEmpty(allowableChars))
    {
      allowableChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    }

    // Generate random data 
    var rnd = new byte[length];
#pragma warning disable SYSLIB0023 // Type or member is obsolete
    using (var rng = new System.Security.Cryptography.RNGCryptoServiceProvider())
    {
      rng.GetBytes(rnd);
    }
#pragma warning restore SYSLIB0023 // Type or member is obsolete

    // Generate the output string
    var allowable = allowableChars.ToCharArray();
    var l = allowable.Length;
    var chars = new char[length];
    for (var i = 0; i < length; i++)
    {
      chars[i] = allowable[rnd[i] % l];
    }
    return new string(chars);
  }
}
/***************************************************************************************/
/***************************************************************************************/
/***************************************************************************************/