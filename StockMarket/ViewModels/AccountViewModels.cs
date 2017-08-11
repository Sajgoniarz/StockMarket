using StockMarket.Models.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StockMarket.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "E-mail Address")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "E-mail address")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} must contain minumum {2} characters.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "Passwords are different")]
        public string ConfirmPassword { get; set; }

        [Required]
        public decimal Founds { get; set; }
        public List<UserStockViewModel> UserStocks { get; set; }
    }

    public class UserStockViewModel
    {
        public int StockId { get; set; }
        public int? Amount { get; set; }
        public string Code { get; set; }
    }
}
