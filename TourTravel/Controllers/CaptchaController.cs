using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using System.Drawing.Imaging;

public class CaptchaController : Controller
{
  [HttpGet]
  public IActionResult GenerateCaptcha()
  {
    string captchaText = GenerateRandomText(6);

    HttpContext.Session.SetString("CaptchaValue", captchaText);

    // Generate image
    var image = GenerateCaptchaImage(captchaText);
    using var ms = new MemoryStream();
    image.Save(ms, ImageFormat.Png);
    string base64 = Convert.ToBase64String(ms.ToArray());

    return Ok(new
    {
      captchaImage = "data:image/png;base64," + base64,
      captchaText = captchaText
    });
  }



  [HttpGet]
  public IActionResult GetText()
  {
    string captchaText = HttpContext.Session.GetString("CaptchaValue");

    // If somehow session expired → regenerate
    if (string.IsNullOrEmpty(captchaText))
    {
      captchaText = GenerateRandomText(6);
      HttpContext.Session.SetString("CaptchaValue", captchaText);
    }

    return Ok(new { captchaText });
  }

  private string GenerateRandomText(int length)
  {
    const string chars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";
    var random = new Random();
    return new string(Enumerable.Repeat(chars, length)
        .Select(s => s[random.Next(s.Length)]).ToArray());
  }

  private Bitmap GenerateCaptchaImage(string captchaText)
  {
    int width = 180, height = 60;
    Bitmap bmp = new Bitmap(width, height);
    Graphics g = Graphics.FromImage(bmp);

    g.Clear(Color.White);

    Random rnd = new Random();
    for (int i = 0; i < 10; i++)
    {
      g.DrawLine(new Pen(Color.LightGray, 2),
          rnd.Next(width), rnd.Next(height),
          rnd.Next(width), rnd.Next(height));
    }

    Font font = new Font("Arial", 28, FontStyle.Bold);
    using (Brush brush = new SolidBrush(Color.Black))
    {
      g.DrawString(captchaText, font, brush, 20, 10);
    }

    return bmp;
  }


}
