using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace OccasionMessageAdmin.Web.Models;

public class ApplicationUser : IdentityUser
{
    public Guid RefId { get; set; }
    [StringLength(256)]
    public string FirstName { get; set; }
    [StringLength(256)]
    public string LastName { get; set; }
    [StringLength(10)]
    public string LanguageCode { get; set; }
    [StringLength(10)]
    public string EmailVerificationCode { get; set; }
    public DateTime EmailVerificationCodeExpiry { get; set; }
    [StringLength(10)]
    public string PhoneVerificationCode { get; set; }
    public DateTime PhoneVerificationCodeExpiry { get; set; }
    [StringLength(25)]
    public override string PhoneNumber { get; set; }
    public Int16 Prefix { get; set; }
    public string PendingNewEmail { get; set; }
    public string PendingNewPhoneNumber { get; set; }
    public Int16? PendingNewPrefix { get; set; }
    public bool SmsEnabled { get; set; }
    public bool EmailMfaEnabled { get; set; }
    public DateTime? LastMfaSetupDate { get; set; }
    public bool IsAccountTemporarilyLocked { get; set; }
    public Guid? PicId { get; set; }
}

