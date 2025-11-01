namespace TourTravel.Models
{
    public class ResetPasswordModel
    {
        public string Email { get; set; }              // From reset link
        public string Token { get; set; }              // Reset token from email
        public string NewPassword { get; set; }        // New password entered by user
        public string ConfirmPassword { get; set; }    // Confirm password entered by user
    }
}
