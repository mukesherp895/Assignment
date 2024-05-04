using Assignment.Common;
using Assignment.Model.DTO;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace Assignment.WebApp.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            HttpContext.Session.Clear();
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Login(LoginReqDto loginReqDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (HttpClient client = new HttpClient())
                    {
                        var content = new StringContent(JsonConvert.SerializeObject(loginReqDto), Encoding.UTF8, "application/json");
                        var request = new HttpRequestMessage
                        {
                            Method = HttpMethod.Post,
                            RequestUri = new Uri(AppSettings.BaseUrlV1 + "/account/login"),
                            Content = content
                        };
                        var httpMessageResponse = await client.SendAsync(request);
                        var readContent = await httpMessageResponse.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<ResponseDto>(readContent);
                        if (result != null)
                        {
                            switch (result.Status)
                            {
                                case ResponseStatus.Success:
                                    TokenDto tokenDto = JsonConvert.DeserializeObject<TokenDto>(result.Data.ToString());
                                    HttpContext.Session.SetString("UserName", tokenDto.Email);
                                    HttpContext.Session.SetString("AccessToken", tokenDto.AccessToken);
                                    HttpContext.Session.SetString("TokenType", tokenDto.TokenType);
                                    return Redirect("~/home/index");
                                case ResponseStatus.Fail or ResponseStatus.UnAuthorized:
                                    ViewBag.IsError = true;
                                    ViewBag.Message = result.Message;
                                    return View();
                                default:
                                    ViewBag.IsError = true;
                                    ViewBag.Message = httpMessageResponse.StatusCode.ToString();
                                    return View();
                            }
                        }
                    }
                }
                else
                {
                    ViewBag.IsError = true;
                    ViewBag.Message = string.Join(',', ModelState.Values.SelectMany(sm => sm.Errors).Select(s => s.ErrorMessage).FirstOrDefault());
                    return View();
                }
            }
            catch (Exception ex)
            {
                ViewBag.IsError = true;
                ViewBag.Message = ex.Message;
            }
            return View();
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return Redirect("~/account/login");
        }
    }
}
