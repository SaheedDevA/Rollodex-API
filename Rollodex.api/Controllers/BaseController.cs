namespace IntelApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Rollodex.lib.Models.Entities;

[Controller]
public abstract class BaseController : ControllerBase
{
    public Account Account => (Account)HttpContext.Items["Account"];
   // public List<Permission> AccountPermissions => (List<Permission>)HttpContext.Items["AccountPermissions"];

}