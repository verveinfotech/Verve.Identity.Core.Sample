# Verve.Identity.Core.Sample
###  What is it
 A sample project to demonstrate how to use Verve Identity libraries for authentication.
###  How it works
1. Uses Verve Identity Core libraries 
		a. Verve.Identity.Core.Model
		b. Verve.Identity.Core.Service
		c. Verve.Identity.Core.IdentityContext
2. Inherit `UserAccount` class from `VerveUserAccount` if you want to customize user account.
3. Inherit `Role` from `VerveRole` class if you want to customize your role class.
4. Inherit Application db context from `VerveIdentityDbContext<UserAccount, Role>`. 		
5. Create `ApplicationUserService` class to implement `VerveIdentityService` abstract class.
6. Add below lines in `startup.cs` file
	```
	 public void ConfigureServices(IServiceCollection services)
	 ...
	 ...
	 ...
	 services.AddVerveIdentityServices<ApplicationUserService, TestApplicationDbContext, UserAccount, Role>();
	services.AddVerveIdentity<UserAccount, Role>()
                .AddDefaultTokenProviders();
	 ...
	 ...        
	```
7. Now in `UserService` class inject `UserManager<UserAccount>` and `SignInManager<UserAccount>`classes to use them for Register, Login, Update user etc. 
