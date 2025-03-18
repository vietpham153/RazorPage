using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using RazorPage.Models;
using RazorPage.Services;
using System.Configuration;

namespace RazorPage
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddOptions();
            var mailsetting = builder.Configuration.GetSection("MailSettings");
            builder.Services.Configure<MailSettings>(mailsetting);
            builder.Services.AddSingleton<IEmailSender, SendMailService>();
            builder.Services.AddSingleton<IdentityErrorDescriber,AddIdentityErrorMessage>();

            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddDbContext<MyBlogContext>(options =>
            {
                string connectString = builder.Configuration.GetConnectionString("DbConnect");
                options.UseSqlServer(connectString);
            });
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("AllowEditRole", optionBuilder =>
                {
                    // Yeu cau dang nhap
                    optionBuilder.RequireAuthenticatedUser();
                    // Yeu cau role nguoi dung la Admin
                    optionBuilder.RequireRole("Admin");
                    // Chuyen qua dung claim-based authorization:

                    //optionBuilder.RequireClaim()

                });
            });

            //Dang ky dich vu Identity
            builder.Services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<MyBlogContext>()
                .AddDefaultTokenProviders();

            //builder.Services.AddDefaultIdentity<AppUser>()
            //    .AddEntityFrameworkStores<MyBlogContext>()
            //    .AddDefaultTokenProviders();


            // Truy cập IdentityOptions
            builder.Services.Configure<IdentityOptions>(options => {
                // Thiết lập về Password
                options.Password.RequireDigit = false; // Không bắt phải có số
                options.Password.RequireLowercase = false; // Không bắt phải có chữ thường
                options.Password.RequireNonAlphanumeric = false; // Không bắt ký tự đặc biệt
                options.Password.RequireUppercase = false; // Không bắt buộc chữ in
                options.Password.RequiredLength = 3; // Số ký tự tối thiểu của password
                options.Password.RequiredUniqueChars = 1; // Số ký tự riêng biệt

                // Cấu hình Lockout - khóa user
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5); // Khóa 5 phút
                options.Lockout.MaxFailedAccessAttempts = 3; // Thất bại 3 lầ thì khóa
                options.Lockout.AllowedForNewUsers = true;

                // Cấu hình về User.
                options.User.AllowedUserNameCharacters = // các ký tự đặt tên user
                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;  // Email là duy nhất

                // Cấu hình đăng nhập.
                options.SignIn.RequireConfirmedEmail = true;            // Cấu hình xác thực địa chỉ email (email phải tồn tại)
                options.SignIn.RequireConfirmedPhoneNumber = false;     // Xác thực số điện thoại
                options.SignIn.RequireConfirmedAccount = true;         // Xác thực tài khoản

            });

            builder.Services.AddAuthentication().AddGoogle(googleOptions =>
            {
                // Đọc thông tin Authentication:Google từ appsettings.json
                IConfigurationSection googleAuthNSection = builder.Configuration.GetSection("Authentication:Google");

                // Thiết lập ClientID và ClientSecret để truy cập API google
                googleOptions.ClientId = googleAuthNSection["ClientId"];
                googleOptions.ClientSecret = googleAuthNSection["ClientSecret"];
                // Cấu hình Url callback lại từ Google (không thiết lập thì mặc định là /signin-google)
                // Đây là Url mà Google sẽ gọi lại để trả về thông tin user setup ở URIs trong Google API
                googleOptions.CallbackPath = "/dang-nhap-tu-google";
            })
               .AddFacebook(facebookOptions =>
               {
                   // Đọc thông tin Authentication:Facebook từ appsettings.json
                   IConfigurationSection FaceNAuth = builder.Configuration.GetSection("Authentication:Facebook");

                   // Thiết lập ClientID và ClientSecret để truy cập API google
                   facebookOptions.ClientId = FaceNAuth["ClientId"];
                   facebookOptions.ClientSecret = FaceNAuth["ClientSecret"];
                   // Cấu hình Url callback lại từ Google (không thiết lập thì mặc định là /signin-google)
                   // Đây là Url mà Google sẽ gọi lại để trả về thông tin user setup ở URIs trong Google API
                   facebookOptions.CallbackPath = "/dang-nhap-tu-facebook";
               });

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Dangnhap";
                options.LogoutPath = "/Dangxuat";
                options.AccessDeniedPath = "/Khongcoquyen";
            });

            builder.Services.Configure<DataProtectionTokenProviderOptions>(options =>
            {
                options.TokenLifespan = TimeSpan.FromMinutes(5); // Token chỉ có hiệu lực 5 phút
            });


            builder.Services.Configure<RouteOptions>(options =>
            {
                options.LowercaseUrls = true;
                options.AppendTrailingSlash = true;
            });

            var app = builder.Build();

            //add Google Authentication
           

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();
        }
    }
}
