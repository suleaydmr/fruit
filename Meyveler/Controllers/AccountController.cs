using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

public class AccountController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;

    public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(string username, string password, bool rememberMe)
    {


        try
        {
            Console.WriteLine(username + " " + password);
            var result = await _signInManager.PasswordSignInAsync(username, password, rememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            if (result.IsLockedOut)
            {
                // Hesap kilitlendiğinde yapılacak işlemleri burada gerçekleştirebilirsiniz.
                // Örneğin: Kullanıcıya kilitlendiği bilgisini gösterme, şifreyi sıfırlama seçeneği sunma, vb.
                ModelState.AddModelError(string.Empty, "Hesabınız kilitlendi. Lütfen daha sonra tekrar deneyin.");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Giriş başarısız. Lütfen tekrar deneyin.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            ModelState.AddModelError(string.Empty, "Giriş sırasında bir hata oluştu.");
        }


        return View();

    }

    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(string username, string password)
    {
        var user = new IdentityUser { UserName = username, Email = "" };
        var result = await _userManager.CreateAsync(user, password);

        if (result.Succeeded)
        {
            await _signInManager.SignInAsync(user, isPersistent: false);
            return RedirectToAction("Index", "Home");
        }
        
        Console.WriteLine(result);

        ModelState.AddModelError(string.Empty, "Kayıt olma başarısız. Lütfen tekrar deneyin.");
        return View();
    }

    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);

        return RedirectToAction("Index", "Home");
    }
}
