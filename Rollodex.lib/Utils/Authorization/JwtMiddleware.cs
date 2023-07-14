namespace Rolodex.Lib.Utils.Authorization;

using Rolodex.Lib.Data;
using Rolodex.Lib.Utils.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;


public class JwtMiddleware
{
    private readonly RequestDelegate _next;
    private readonly AppSettings _appSettings;

    public JwtMiddleware(RequestDelegate next, IOptions<AppSettings> appSettings)
    {
        _next = next;
        _appSettings = appSettings.Value;
    }

    public async Task Invoke(HttpContext context, ApplicationDbContext dataContext, IJwtUtils jwtUtils)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        var accountId = jwtUtils.ValidateJwtToken(token);
        if (accountId != null)
        {
            // attach account to context on successful jwt validation
            context.Items["Account"] = await dataContext.Accounts.FindAsync(accountId.Value);

            //get all user permissions and attach them
            /*  var alluserTeams = dataContext.AdminTeams.Where(x => x.AdminId == accountId.Value);
              if (alluserTeams.Any())
              {
                  var allTeamIds = alluserTeams.Select(x => x.Id);
                  //get all permissions for the team
                  var allUserTeamPermissions = dataContext.TeamPermissions.Where(x=> allTeamIds.Contains(x.TeamId)).Select(x=>x.PermissionId).ToList();

                  var allPermissions =  dataContext.Permissions.Where(x=> allUserTeamPermissions.Contains(x.Id)).ToList();

                  context.Items["AccountPermissions"] = allPermissions;

              }*/
        }

        await _next(context);
    }
}