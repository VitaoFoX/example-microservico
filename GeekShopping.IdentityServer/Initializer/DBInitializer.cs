using GeekShopping.IdentityServer.Configuration;
using GeekShopping.IdentityServer.Model.Context;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace GeekShopping.IdentityServer.Initializer
{
    public class DBInitializer : IDbInitializer
    {
        private readonly MySQLContext _context;
        private readonly UserManager<ApplicationUser> _user;
        private readonly RoleManager<IdentityRole> _role;

        public DBInitializer(MySQLContext context, UserManager<ApplicationUser> user, RoleManager<IdentityRole> role)
        {
            _context = context;
            _user = user;
            _role = role;
        }

        public void Initialize()
        {
            //Verificando se tem algum admin logado, se tiver não executar a carga de banco novamente
            if (_role.FindByIdAsync(IdentityConfiguration.Admin).Result != null) return;

            //Criando as roles no banco
            _role.CreateAsync(new 
                IdentityRole(IdentityConfiguration.Admin)).GetAwaiter().GetResult();
            _role.CreateAsync(new
                IdentityRole(IdentityConfiguration.Client)).GetAwaiter().GetResult();

            //Criando um novo admin quando o banco está vazio
            ApplicationUser admin = new ApplicationUser()
            {
                UserName= "vitor-admin",
                Email = "vitor-admin@gmail.com",
                EmailConfirmed = true,
                PhoneNumber = "+55 35 12345-6789",
                FristName = "Vitor",
                LastName = "Admin"
            };
            
            //Vinculando a role ao usuário
            _user.CreateAsync(admin, "Teste@123").GetAwaiter().GetResult();
            _user.AddToRoleAsync(admin, 
                IdentityConfiguration.Admin).GetAwaiter().GetResult();

            var adminClaims = _user.AddClaimsAsync(admin, new Claim[]
            {
                new Claim(JwtClaimTypes.Name, $"{admin.FristName} {admin.LastName}"),
                new Claim(JwtClaimTypes.GivenName, admin.FristName),
                new Claim(JwtClaimTypes.FamilyName, admin.LastName),
                new Claim(JwtClaimTypes.Role, IdentityConfiguration.Admin),

            }).Result; 
            
            //Criando um novo client quando o banco está vazio
            ApplicationUser client = new ApplicationUser()
            {
                UserName= "vitor-client",
                Email = "vitor-client@gmail.com",
                EmailConfirmed = true,
                PhoneNumber = "+55 35 12345-6789",
                FristName = "Vitor",
                LastName = "Client"
            };
            
            //Vinculando a role ao usuário
            _user.CreateAsync(client, "Teste@123").GetAwaiter().GetResult();
            _user.AddToRoleAsync(client, 
                IdentityConfiguration.Client).GetAwaiter().GetResult();

            var clientClaims = _user.AddClaimsAsync(client, new Claim[]
            {
                new Claim(JwtClaimTypes.Name, $"{client.FristName} {client.LastName}"),
                new Claim(JwtClaimTypes.GivenName, client.FristName),
                new Claim(JwtClaimTypes.FamilyName, client.LastName),
                new Claim(JwtClaimTypes.Role, IdentityConfiguration.Client),

            }).Result;
        }
    }
}
