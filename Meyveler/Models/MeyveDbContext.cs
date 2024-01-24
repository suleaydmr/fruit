using Meyveler.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Reflection.Emit;

namespace Meyveler.Data
{
    public class MeyveDbContext : IdentityDbContext<IdentityUser>
    {
        public MeyveDbContext(DbContextOptions<MeyveDbContext> options)
            : base(options)
        {
        }

        public DbSet<Meyve> Meyveler { get; set; }
        public DbSet<Slider> Slider { get; set; }
        public DbSet<Sehir> Sehirler { get; set; }
        public DbSet<Cesit> Cesitler { get; set; }
        public DbSet<Uretim> Uretimler { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Meyve tablosunun özel ayarları (isteğe bağlı)
            modelBuilder.Entity<Meyve>()
                .HasKey(m => m.MeyveId);

            modelBuilder.Entity<Meyve>()
                .Property(m => m.Ad)
                .IsRequired();

            modelBuilder.Entity<Meyve>()
                .Property(m => m.TanitimMetni)
                .IsRequired();

            // Slider tablosunun özel ayarları (isteğe bağlı)
            modelBuilder.Entity<Slider>()
                .HasKey(s => s.SliderId);

            modelBuilder.Entity<Slider>()
                .Property(s => s.ImagePath)
                .IsRequired();

            modelBuilder.Entity<Sehir>()
        .Property(s => s.UretimOrani)
        .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Cesit>()
                .Property(m => m.Ad)
                .IsRequired();

                modelBuilder.Entity<Uretim>()
                .Property(m => m.UretimMetin)
                .IsRequired();

            base.OnModelCreating(modelBuilder);
        }

        public static void Seed(MeyveDbContext context, UserManager<IdentityUser> userManager)
        {
            Console.WriteLine("seed çağırıldı");

            var roleStore = new RoleStore<IdentityRole>(context);
            var roleManager = new RoleManager<IdentityRole>(roleStore, null, null, null, null);

            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                var role = new IdentityRole { Name = "Admin" };
                context.Roles.Add(role);
                context.SaveChanges();
            }

            // Admin kullanıcısını oluştur
            if (!context.Users.Any(u => u.UserName == "admin"))
            {


                var user = new IdentityUser
                {
                    UserName = "admin",
                    Email = "admin@sakarya.com"  // E-posta adresini uygun bir değerle değiştirin.
                };
                var password = "admin123";

                userManager.CreateAsync(user, password).Wait();

                // Oluşturulan kullanıcıya "Admin" rolünü ata
                var userId = context.Users.First(u => u.UserName == "admin").Id;
                var roleId = context.Roles.First(r => r.Name == "Admin").Id;
                context.UserRoles.Add(new IdentityUserRole<string> { UserId = userId, RoleId = roleId });
                context.SaveChanges();
            }

            if (!context.Meyveler.Any())
            {
                // Varsayılan meyve verilerini ekleyin
                context.Meyveler.AddRange(
                    new Meyve { Ad = "Armut: Doğanın Tatlı Hediyesi, Sağlığın Anahtarı!", TanitimMetni = "Armut, lezzeti ve sağlık avantajlarıyla öne çıkan bir doğal mucize. Hem tatlı hem tuzlu tariflere mükemmel uyum sağlayan bu meyve, sindirimi destekleyen yüksek lif içeriği ve güçlü antioksidanlarıyla biliniyor. Ayrıca düşük kalorili olması, diyet yapanlar için ideal bir tercih haline getiriyor. Sofralarınıza eklediğinizde, hem lezzet hem de sağlık dolu bir deneyim yaşayabilirsiniz. Armut, doğanın bize sunduğu tatlı bir hediye ve sağlık dolu bir zenginlik!" }
                    // Diğer meyveleri ekleyin
                );

                context.SaveChanges();
            }

            if (!context.Slider.Any())
            {
                context.Slider.AddRange(
                    new Slider { ImagePath = "https://images.unsplash.com/photo-1514756331096-242fdeb70d4a?q=80&w=3870&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D" }
                );

                context.SaveChanges();
            }


            if (!context.Sehirler.Any())
            {
                context.Sehirler.AddRange(
                    new Sehir { Ad = "Bursa", UretimOrani = 34},
                    new Sehir { Ad = "Ankara", UretimOrani = 17 }

                );


                context.SaveChanges();
            }

			if (!context.Cesitler.Any())
			{
				context.Cesitler.AddRange(
					new Cesit { Ad = "Tosca", CesitMetin = "İtalya’da Coscia ve Wiliams çeşitlerinin melezlenmesi ile elde edilmiştir. Ağaç yarı dik gelişir, geniş taç oluşturur, dallanma şekli ayvayı andırır. Meyve orta irilikte, armut şeklinde, meyve kabuğu pürüzsüz, orta kalınlıkta, sulu, tatlı depolama süresi kısadır Bu nedenle yeme olumuna yakın dönemde hasat edilmelidir. 20-25 Temmuz tarihlerinde hasat edilir. Meyve kabuğu zemini sarımtırak yeşil renkte, güneş gören taraf kırmızı yanak yapar. BA-29 MC gibi ayva kökenli anaçlar ve OHF 333 anaçları ile aşı uyuşması  ve ağaç gelişimi iyidir. Taze olarak tüketildiği kadar sanayilik olarak ta değerlendirilir." },
					new Cesit { Ad = "Abete Fetel", CesitMetin = "Fransız kökenli, ağaçları orta kuvvete, yarı dik gelişen verimli erken  meyveye yatan bir çeşittir. Erwinia amylovora (ateş yanıklığı) hastalığına kısmen dayanıklıdır. Meyve kabuk rengi; yeşil zemin üzerine az paslı, iri (275 gr.), konik biçiminde, boyun kısmı uzundur. Meyve eti beyaz, çok sulu, tatlı, aromalı ve az kumludur.Commice çeşidinden 2-3 gün sonra Eylül ayının son haftası hasat  edilir." }
				);

				context.SaveChanges();
			}

            if (!context.Uretimler.Any())
            {
                context.Uretimler.AddRange(
                    new Uretim { UretimMetin = "Son üç yılda armut üretimi, tarım sektöründe önemli bir yer tutmuş ve çeşitli faktörlere bağlı olarak değişiklik göstermiştir. Bu süreçte, armut üretimi üzerinde etkili olan çeşitli faktörler incelenmiş ve sektördeki gelişmeler gözlemlenmiştir.\r\n\r\nİklim koşulları, armut üretimini doğrudan etkileyen önemli bir faktördür. Son üç yılda yaşanan iklim değişiklikleri, bazı bölgelerde armut yetiştiriciliğini olumlu yönde etkilerken, bazı bölgelerde ise olumsuz etkiler yaratmış olabilir. Sıcaklık değişimleri, yağış miktarı ve mevsim normallerinin dışında seyri, armut ağaçlarının büyümesi, çiçeklenme ve meyve olgunlaşması üzerinde etkili olabilir.\r\n\r\nTarım teknolojisinin gelişmesi de armut üretiminde belirleyici bir faktördür. Son yıllarda kullanılan modern tarım teknikleri, sulama sistemlerindeki ilerlemeler ve verimli gübreleme yöntemleri, armut üretimini artırmış ve kaliteyi yükseltmiştir. Ayrıca, hastalıklara karşı dirençli armut çeşitlerinin geliştirilmesi, üretimde istikrarlı bir artış sağlamıştır.\r\n\r\nPazar talepleri ve tüketici tercihleri de armut üretimini etkileyen önemli unsurlardır. Tüketicilerin sağlık ve beslenme konusundaki bilinçlenmeleri, organik ürünlere olan ilginin artması gibi faktörler, armut yetiştiricilerini ürünlerini bu yönde geliştirmeye yönlendirebilir. Aynı zamanda, iç ve dış pazarlardaki talep de üretim miktarını etkiler ve çiftçilere yeni pazarlara açılma fırsatları sunar.\r\n\r\nSonuç olarak, son üç yılda armut üretimi, iklim koşulları, tarım teknolojisi ve pazar talepleri gibi faktörlere bağlı olarak değişiklik göstermiştir. Bu değişkenlerin etkileşimi, armut yetiştiriciliği alanında sürekli adaptasyon ve yenilik gerektiren dinamik bir süreci ortaya koymaktadır. Üreticiler, sürdürülebilir tarım uygulamalarını benimseyerek, teknolojik gelişmeleri takip ederek ve pazar ihtiyaçlarına uygun üretim yaparak sektörde başarılı olabilirler." }
                );

                context.SaveChanges();
            }
		}
    }
}
