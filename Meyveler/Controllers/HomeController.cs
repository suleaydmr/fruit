using Meyveler.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

public class HomeController : Controller
{
    private readonly MeyveDbContext _dbContext;
    private readonly UserManager<IdentityUser> _userManager;

    public HomeController(MeyveDbContext dbContext, UserManager<IdentityUser> userManager)
    {
        _dbContext = dbContext;
        _userManager = userManager;
    }   


    public IActionResult Index()
    {       

        var meyveler = _dbContext.Meyveler.ToList();
        var sliderImages = _dbContext.Slider.ToList(); // Slider verilerini çek

        return View("MeyveIndex", (Meyveler: meyveler, SliderImages: sliderImages));
    }


    [Authorize]
    public IActionResult Uretim()
    {
        var uretim = _dbContext.Uretimler.FirstOrDefault();
        return View("UretimIndex", uretim);
    }

    [Authorize]
    public IActionResult Sehirler()
    {

        var sehirler = _dbContext.Sehirler.ToList();
        return View("SehirlerIndex", sehirler);
    }

    [Authorize]
    public IActionResult Cesitler()
    {

		var cesitler = _dbContext.Cesitler.ToList();
		return View("CesitlerIndex", cesitler);
    }

}
