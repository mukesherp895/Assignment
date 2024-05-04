using Assignment.Common;
using Assignment.Model.DTO;
using Assignment.WebApp.Attributes;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace Assignment.WebApp.Controllers
{
    [CheckAthorization]
    [Route("companyinfo")]
    public class CompanyInfoController : Controller
    {
        [Route("index")]
        public IActionResult Index()
        {
            ViewBag.PageHeader = "Company Information";
            return View();
        }
        [Route("companyinfoget")]
        [HttpGet]
        public async Task<IActionResult> CompanyInfoGet(DataTableParamDto param)
        {
            try
            {
                string baseUrl = AppSettings.BaseUrlV1;
                var tokenAccess = HttpContext.Session.GetString("AccessToken");
                var tokenType = HttpContext.Session.GetString("TokenType");
                using (HttpClient client = new HttpClient())
                {
                    var bodyContent = JsonConvert.SerializeObject(param);
                    var content = new StringContent(bodyContent, Encoding.UTF8, "application/json");
                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri(baseUrl + "/companyinfo/getallcompanyinfobyfilter?" + $"iDisplayStart={param.iDisplayStart}&iDisplayLength={param.iDisplayLength}&sSearch={param.sSearch}&sEcho={param.sEcho}&sSortDir_0={param.sSortDir_0}&iSortCol_0={param.iSortCol_0}"),
                        Content = content,
                        Headers = {
                            { HttpRequestHeader.Authorization.ToString(), $"{tokenType} {tokenAccess}" }
                        }
                    };
                    var httpMessageResponse = await client.SendAsync(request);
                    var readContent = await httpMessageResponse.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<ResponseDto>(readContent);
                    if (result != null && result.Status.Equals(ResponseStatus.Success))
                    {
                        return Ok(result.Data.ToString());
                    }
                }
            }
            catch
            {
            }
            return Ok(new { aaData = new List<CompanyInfosDto>(), sEcho = param.sEcho, iTotalDisplayRecords = 0, iTotalRecords = 0 });
        }
        [Route("addupdate")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddUpdate(CompanyInfoAddUpdateReqDto companyInfoAddUpdateReqDto)
        {
            ResponseDto responseDto = new ResponseDto();
            try
            {
                string baseUrl = AppSettings.BaseUrlV1;
                var tokenAccess = HttpContext.Session.GetString("AccessToken");
                var tokenType = HttpContext.Session.GetString("TokenType");
                using (HttpClient client = new HttpClient())
                {
                    var bodyContent = JsonConvert.SerializeObject(companyInfoAddUpdateReqDto);
                    var content = new StringContent(bodyContent, Encoding.UTF8, "application/json");
                    var request = new HttpRequestMessage
                    {
                        Method = companyInfoAddUpdateReqDto.Id == 0 ? HttpMethod.Post : HttpMethod.Put,
                        RequestUri = new Uri(baseUrl + (companyInfoAddUpdateReqDto.Id == 0 ? "/companyinfo/add" : $"/companyinfo/update/{companyInfoAddUpdateReqDto.Id}")),
                        Content = content,
                        Headers = {
                            { HttpRequestHeader.Authorization.ToString(), $"{tokenType} {tokenAccess}" }
                        }
                    };
                    var httpMessageResponse = await client.SendAsync(request);
                    var readContent = await httpMessageResponse.Content.ReadAsStringAsync();
                    responseDto = JsonConvert.DeserializeObject<ResponseDto>(readContent);
                    if (responseDto != null && responseDto.Status.Equals(ResponseStatus.Success))
                    {
                        return Ok(responseDto);
                    }
                    else
                    {
                        responseDto.Status = ResponseStatus.Fail;
                        responseDto.Message = httpMessageResponse.StatusCode.ToString();
                    }
                }
            }
            catch
            {
                responseDto.Status = ResponseStatus.Fail;
                responseDto.Message = "Inter Server Error";
            }
            return Ok(responseDto);
        }
        [Route("delete/{id}")]
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            ResponseDto responseDto = new ResponseDto();
            try
            {
                string baseUrl = AppSettings.BaseUrlV1;
                var tokenAccess = HttpContext.Session.GetString("AccessToken");
                var tokenType = HttpContext.Session.GetString("TokenType");
                using (HttpClient client = new HttpClient())
                {
                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Delete,
                        RequestUri = new Uri(baseUrl + $"/companyinfo/delete/{id}"),
                        Headers = {
                            { HttpRequestHeader.Authorization.ToString(), $"{tokenType} {tokenAccess}" }
                        }
                    };
                    var httpMessageResponse = await client.SendAsync(request);
                    var readContent = await httpMessageResponse.Content.ReadAsStringAsync();
                    responseDto = JsonConvert.DeserializeObject<ResponseDto>(readContent);
                    if (responseDto != null && responseDto.Status.Equals(ResponseStatus.Success))
                    {
                        return Ok(responseDto);
                    }
                    else
                    {
                        responseDto.Status = ResponseStatus.Fail;
                        responseDto.Message = httpMessageResponse.StatusCode.ToString();
                    }
                }
            }
            catch
            {
                responseDto.Status = ResponseStatus.Fail;
                responseDto.Message = "Inter Server Error";
            }
            return Ok(responseDto);
        }
    }
}
