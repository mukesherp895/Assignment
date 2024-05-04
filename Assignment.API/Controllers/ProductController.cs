using Assignment.Common;
using Assignment.DataAccess.Repository;
using Assignment.Model.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Assignment.API.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/v1/product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        [HttpGet("getallproductbyfilter")]
        public async Task<IActionResult> GetAllProduts(int iDisplayStart, int iDisplayLength, string? sSearch, int sEcho, string? sSortDir_0, int iSortCol_0, string? schema)
        {
            //Response Body Initialization
            ResponseDto responseDto = new ResponseDto();
            try
            {
                DataTableParamDto paramDto = new DataTableParamDto
                {
                    iDisplayLength = iDisplayLength,
                    sSearch = sSearch,
                    sEcho = sEcho,
                    sSortDir_0 = sSortDir_0,
                    iDisplayStart = iDisplayStart,
                    iSortCol_0 = iSortCol_0,
                    Schema = schema
                };
                int? recCount = 0;
                var data = await _productRepository.GetAllProductAsync(paramDto);
                if (data != null && data.Count > 0)
                {
                    recCount = data[0].RecCount;
                }
                responseDto.Status = ResponseStatus.Success;
                responseDto.Message = "Success";
                responseDto.Data = new { aaData = data, sEcho = paramDto.sEcho, iTotalDisplayRecords = recCount, iTotalRecords = recCount };
                return Ok(responseDto);
            }
            catch (Exception ex)
            {
                responseDto.Status = ResponseStatus.Fail;
                responseDto.Message = ex.Message;
                responseDto.Data = new { aaData = new List<CompanyInfosDto>(), sEcho = sEcho, iTotalDisplayRecords = 0, iTotalRecords = 0 };
                return this.StatusCode(StatusCodes.Status500InternalServerError, responseDto);
            }
        }
        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] ProductReqDto reqDto)
        {
            //Response Body Initialization
            ResponseDto responseDto = new ResponseDto();
            try
            {
                //ModelState Check
                if (ModelState.IsValid)
                {
                    //Product Added
                    if (await _productRepository.AddAsync(reqDto, User.Identity.Name) == EnumData.DBAttempt.Success)
                    {
                        //Success Response
                        responseDto.Status = ResponseStatus.Success;
                        responseDto.Message = "Product Added Successfully";
                        return this.StatusCode(StatusCodes.Status201Created, responseDto);
                    }
                    else
                    {
                        //Fail Response
                        responseDto.Status = ResponseStatus.Fail;
                        responseDto.Message = "Product Add Fail";
                        return Ok(responseDto);
                    }
                }
                else
                {
                    //ModelState Validation Response
                    responseDto.Status = ResponseStatus.Fail;
                    responseDto.Message = string.Join(',', ModelState.Values.SelectMany(sm => sm.Errors).Select(s => s.ErrorMessage).ToList());
                    return this.StatusCode(StatusCodes.Status400BadRequest, responseDto);
                }
            }
            catch (Exception ex)
            {
                //Exception Response
                responseDto.Status = ResponseStatus.Fail;
                responseDto.Message = ex.Message;
                return this.StatusCode(StatusCodes.Status500InternalServerError, responseDto);
            }
        }
        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ProductReqDto reqDto)
        {
            //Response Body Initialization
            ResponseDto responseDto = new ResponseDto();
            try
            {
                //Model State Check
                if (ModelState.IsValid)
                {
                    //CompanyInfo Updated
                    if (await _productRepository.UpdateAsync(id, reqDto, User.Identity.Name) == EnumData.DBAttempt.Success)
                    {
                        //Success Response
                        responseDto.Status = ResponseStatus.Success;
                        responseDto.Message = "Product Updated Successfully";
                        return this.StatusCode(StatusCodes.Status201Created, responseDto);
                    }
                    else
                    {
                        //Fail Response
                        responseDto.Status = ResponseStatus.Fail;
                        responseDto.Message = "Product Update Fail";
                        return Ok(responseDto);
                    }
                }
                else
                {
                    //ModelState Validation Response
                    responseDto.Status = ResponseStatus.Fail;
                    responseDto.Message = string.Join(',', ModelState.Values.SelectMany(sm => sm.Errors).Select(s => s.ErrorMessage).ToList());
                    return this.StatusCode(StatusCodes.Status400BadRequest, responseDto);
                }
            }
            catch (Exception ex)
            {
                //Eeception Response
                responseDto.Status = ResponseStatus.Fail;
                responseDto.Message = ex.Message;
                return this.StatusCode(StatusCodes.Status500InternalServerError, responseDto);
            }
        }
        [HttpDelete("delete/{id}/{companyInfoId}")]
        public async Task<IActionResult> Delete(int id, int companyInfoId)
        {
            //Response Body Initialization
            ResponseDto responseDto = new ResponseDto();
            try
            {
                //Model State Check
                if (id > 0 && companyInfoId > 0)
                {
                    //CompanyInfo Updated
                    if (await _productRepository.DeleteAsync(id, companyInfoId) == EnumData.DBAttempt.Success)
                    {
                        //Success Response
                        responseDto.Status = ResponseStatus.Success;
                        responseDto.Message = "Product Deleted Successfully";
                        return this.StatusCode(StatusCodes.Status201Created, responseDto);
                    }
                    else
                    {
                        //Fail Response
                        responseDto.Status = ResponseStatus.Fail;
                        responseDto.Message = "Product Delete Fail";
                        return Ok(responseDto);
                    }
                }
                else
                {
                    //ModelState Validation Response
                    responseDto.Status = ResponseStatus.Fail;
                    responseDto.Message = "Bad Request";
                    return this.StatusCode(StatusCodes.Status400BadRequest, responseDto);
                }
            }
            catch (Exception ex)
            {
                //Eeception Response
                responseDto.Status = ResponseStatus.Fail;
                responseDto.Message = ex.Message;
                return this.StatusCode(StatusCodes.Status500InternalServerError, responseDto);
            }
        }
    }
}
