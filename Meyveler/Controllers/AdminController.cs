using Meyveler.Data;
using Meyveler.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly MeyveDbContext _dbContext;
    private readonly UserManager<IdentityUser> _userManager;

    public AdminController(MeyveDbContext dbContext, UserManager<IdentityUser> userManager)
    {
        _dbContext = dbContext;
        _userManager = userManager;
    }

    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> SliderIndex()
    {
        var sliders = await _dbContext.Slider.ToListAsync();
        return View("Slider/Index", sliders);
    }

    public IActionResult SliderCreate()
    {
        return View("Slider/Create");
    }

    [HttpPost]
    public async Task<IActionResult> SliderCreate(Slider slider)
    {
        if (ModelState.IsValid)
        {
            _dbContext.Add(slider);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(SliderIndex));
        }
        return View("Slider/Create", slider);
    }

    public async Task<IActionResult> SliderEdit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var slider = await _dbContext.Slider.FindAsync(id);

        if (slider == null)
        {
            return NotFound();
        }

        return View("Slider/Edit", slider);
    }

    [HttpPost]
    public async Task<IActionResult> SliderEdit(int id, Slider slider)
    {
        if (id != slider.SliderId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _dbContext.Update(slider);
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SliderExists(slider.SliderId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(SliderIndex));
        }
        return View("Slider/Edit", slider);
    }

    public async Task<IActionResult> SliderDelete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var slider = await _dbContext.Slider.FindAsync(id);
        _dbContext.Slider.Remove(slider);
        await _dbContext.SaveChangesAsync();
        return RedirectToAction(nameof(SliderIndex));
    }

    private bool SliderExists(int id)
    {
        return _dbContext.Slider.Any(e => e.SliderId == id);
    }


    [HttpGet]
    public async Task<IActionResult> EditDescription()
    {
        // Veritabanından meyve bilgilerini al
        var meyve = await _dbContext.Meyveler.FirstOrDefaultAsync();

        if (meyve == null)
        {
            return NotFound();
        }

        return View("Meyve/Index", meyve);
    }

    [HttpPost]
    public async Task<IActionResult> EditDescription(Meyve meyve)
    {
        if (ModelState.IsValid)
        {
            _dbContext.Update(meyve);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(EditDescription));
        }

        return View("Meyve/Index", meyve);
    }


    [HttpGet]
    public async Task<IActionResult> UretimEdit()
    {
        var uretim = await _dbContext.Uretimler.FirstOrDefaultAsync();

        if (uretim == null)
        {
            return NotFound();
        }

        return View("Uretim/Index", uretim);
    }

    [HttpPost]
    public async Task<IActionResult> UretimEdit(Uretim uretim)
    {
        if (ModelState.IsValid)
        {
            _dbContext.Update(uretim);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(UretimEdit));
        }

        return View("Uretim/Index", uretim);
    }



    public async Task<IActionResult> CityIndex()
    {
        var cities = await _dbContext.Sehirler.ToListAsync();
        return View("City/Index", cities);
    }

    public IActionResult CityCreate()
    {
        return View("City/Create");
    }

    [HttpPost]
    public async Task<IActionResult> CityCreate(Sehir sehir)
    {
        if (ModelState.IsValid)
        {
            _dbContext.Add(sehir);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(CityIndex));
        }
        return View("City/Create", sehir);
    }

    public async Task<IActionResult> CityEdit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var city = await _dbContext.Sehirler.FindAsync(id);

        if (city == null)
        {
            return NotFound();
        }

        return View("City/Edit", city);
    }

    [HttpPost]
    public async Task<IActionResult> CityEdit(int id, Sehir sehir)
    {
        if (id != sehir.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _dbContext.Update(sehir);
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CityExists(sehir.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(CityIndex));
        }
        return View("City/Edit", sehir);
    }

    public async Task<IActionResult> CityDelete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var city = await _dbContext.Sehirler.FindAsync(id);
        _dbContext.Sehirler.Remove(city);
        await _dbContext.SaveChangesAsync();
        return RedirectToAction(nameof(CityIndex));
    }

    private bool CityExists(int id)
    {
        return _dbContext.Sehirler.Any(e => e.Id == id);
    }


    public async Task<IActionResult> CesitIndex()
    {
        var cesitler = await _dbContext.Cesitler.ToListAsync();
        return View("Cesit/Index", cesitler);
    }

    public IActionResult CesitCreate()
    {
        return View("Cesit/Create");
    }

    [HttpPost]
    public async Task<IActionResult> CesitCreate(Cesit cesit)
    {
        if (ModelState.IsValid)
        {
            _dbContext.Add(cesit);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(CesitIndex));
        }
        return View("Cesit/Create", cesit);
    }

    public async Task<IActionResult> CesitEdit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var cesit = await _dbContext.Cesitler.FindAsync(id);

        if (cesit == null)
        {
            return NotFound();
        }

        return View("Cesit/Edit", cesit);
    }

    [HttpPost]
    public async Task<IActionResult> CesitEdit(int id, Cesit cesit)
    {
        if (id != cesit.CesitId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _dbContext.Update(cesit);
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CesitExists(cesit.CesitId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(CesitIndex));
        }
        return View("Cesit/Edit", cesit);
    }

    public async Task<IActionResult> CesitDelete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var cesit = await _dbContext.Cesitler.FindAsync(id);
        _dbContext.Cesitler.Remove(cesit);
        await _dbContext.SaveChangesAsync();
        return RedirectToAction(nameof(CesitIndex));

    }

    private bool CesitExists(int id)
    {
        return _dbContext.Cesitler.Any(e => e.CesitId == id);
    }


    public async Task<IActionResult> KullaniciIndex()
    {
        var kullanicilar = await _dbContext.Users.ToListAsync();
        return View("Kullanici/Index", kullanicilar);
    }

    public IActionResult KullaniciCreate()
    {
        return View("Kullanici/Create");
    }

    [HttpPost]
    public async Task<IActionResult> KullaniciCreate(IdentityUser user, string PasswordHash)
    {
        if (ModelState.IsValid)
        {
            var result = await _userManager.CreateAsync(user, PasswordHash);

            if (result.Succeeded)
            {
                return RedirectToAction(nameof(KullaniciIndex));
            }

            foreach (var error in result.Errors)
            {
                Console.WriteLine("HAta oluştu");
                ModelState.AddModelError(string.Empty, error.Description);
            }
        } else
        {
            Console.WriteLine("Err....");

            foreach (var modelStateKey in ModelState.Keys)
            {
                var modelStateVal = ModelState[modelStateKey];
                foreach (var error in modelStateVal.Errors)
                {
                    Console.WriteLine($"Err....: {error.ErrorMessage}");
                }
            }   
        }

        return View("Kullanici/Create", user);
    }

    public async Task<IActionResult> KullaniciEdit(string id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var user = await _dbContext.Users.FindAsync(id);

        if (user == null)
        {
            return NotFound();
        }

        return View("Kullanici/Edit", user);
    }

    [HttpPost]
    public async Task<IActionResult> KullaniciEdit(string id, IdentityUser user)
    {
        if (id != user.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _dbContext.Update(user);
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!KullaniciExists(user.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(KullaniciIndex));
        }
        return View("Kullanici/Edit", user);
    }

    public async Task<IActionResult> KullaniciDelete(string id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var user = await _dbContext.Users.FindAsync(id);
        _dbContext.Users.Remove(user);
        await _dbContext.SaveChangesAsync();
        return RedirectToAction(nameof(KullaniciIndex));
    }

    private bool KullaniciExists(string id)
    {
        return _dbContext.Users.Any(e => e.Id == id);
    }
}
