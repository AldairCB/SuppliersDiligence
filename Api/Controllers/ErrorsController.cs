using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class ErrorsController : ApiController // IT WAS ControllerBase BUT I THINK IT SHOULD BE LIKE THI, THO IT MIGH BRAKE SOMEHTING?
{
    [Route("/error")]
    public IActionResult Error() => Problem();
}