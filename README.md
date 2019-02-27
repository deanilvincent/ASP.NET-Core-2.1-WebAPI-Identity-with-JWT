# ASP.NET Core 2.1 WebAPI Identity with JWT

This project will help you how to setup and install [JWT (Json Web Tokens)](https://jwt.io/) as your authentication for your application. This project is good if you have front-end web clients like Angular, Vue.JS, React.JS etc. that will consume the apis with JWT as your security.

## Setting up JWT (JSON Web Token) for ASP.NET Core App

### Startup.cs configuration

```
using Microsoft.AspNetCore.Authentication.JwtBearer;
...

public void ConfigureServices(IServiceCollection services)
{
  // Add Identity is for razor and cookie based like asp.net core mvc
  // But we are using IdentityCore
  // Password configuration builder
  IdentityBuilder builder = services.AddIdentityCore<User>(opt =>
  {
    opt.Password.RequireDigit = false;
    opt.Password.RequiredLength = 4;
    opt.Password.RequireNonAlphanumeric = false;
    opt.Password.RequireUppercase = false;
  });
            
  builder = new IdentityBuilder(builder.UserType, typeof(Role), builder.Services);
  builder.AddEntityFrameworkStores<MyAppDbContext>(); // telling our identity to use our entity framework as our store
  builder.AddRoleValidator<RoleValidator<Role>>();
  builder.AddRoleManager<RoleManager<Role>>();
  builder.AddSignInManager<SignInManager<User>>();

  // auth middleware
  services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
  {
    ValidateIssuerSigningKey = true,
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.GetSection("AppSettings:Token").Value)),
    ValidateIssuer = false,
    ValidateAudience = false
   });

   // add authorization for policy based
   services.AddAuthorization(options =>
   {
    options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
    options.AddPolicy("NormalUserOnly", policy => policy.RequireRole("NormalUser"));
   });

   // every request is auth, the [Authorize] attribute can be removed because this is a setting for global authorization and for Policy
   services.AddMvc(options =>
   {
    var policy = new AuthorizationPolicyBuilder()
    .RequireAuthenticatedUser() // requires auth globally
    .Build();

    options.Filters.Add(new AuthorizeFilter(policy));
   });

}
```

You can run the application by cloning this repo: `https://github.com/deanilvincent/ASP.NET-Core-2.1-WebAPI-Identity-with-JWT.git`
