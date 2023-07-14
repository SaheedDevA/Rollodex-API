using Rolodex.Lib.Utils.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Rollodex.lib.Models.Entities;
using Rollodex.lib.Models.Response;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeAttribute : Attribute, IAuthorizationFilter
{
    private readonly IList<string> _permissions;
   

    public AuthorizeAttribute(params string[] permissions)
    {
        permissions = permissions== null ? new string[] { } : permissions;
        _permissions = permissions ?? new string[] { };
    }



    public void OnAuthorization(AuthorizationFilterContext context)
    {
        // skip authorization if action is decorated with [AllowAnonymous] attribute
        var allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
        if (allowAnonymous)
            return;

        // authorization
        var account = (Account)context.HttpContext.Items["Account"];
        List<string> permissions = new List<string>();
        if(account != null)
        {
           /* var allPerm = (List<Permission>)context.HttpContext.Items["AccountPermissions"];
             permissions = allPerm == null ? new List<string>() : allPerm.Select(x=>x.Name).ToList();*/
           // if (permissions != null) { permissions.Add(account.Role); }

        }



        if (account == null || _permissions.Intersect(permissions).Any())
        {
            // not logged in or role not authorized
            context.Result = new JsonResult(new Response<string>
            {
                Message = "Unauthorized",
                Data = null,
                Succeeded = false
            })
            { StatusCode = StatusCodes.Status401Unauthorized };
        }
    }


  
}