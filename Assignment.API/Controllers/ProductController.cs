using Assignment.Common;
using Assignment.DataAccess.Repository;
using Assignment.Model.Domain;
using Assignment.Model.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

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
        [HttpGet("getallproducts")]
        public async Task<IActionResult> GetAllProduts(string? schema)
        {
            //Response Body Initialization
            ResponseDto responseDto = new ResponseDto();
            try
            {
                //Get All Products
                responseDto.Status = ResponseStatus.Success;
                responseDto.Message = "Success";
                responseDto.Data = await _productRepository.GetAllProductAsync(schema);
                return Ok(responseDto);
            }
            catch (Exception ex)
            {
                //Exception Response
                responseDto.Status = ResponseStatus.Fail;
                responseDto.Message = ex.Message;
                return this.StatusCode(StatusCodes.Status500InternalServerError, responseDto);
            }
        }
        [HttpPost("add")]
        public async Task<IActionResult> Add(ProductReqDto reqDto)
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
        public async Task<IActionResult> Update(int id, ProductReqDto reqDto)
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
        [HttpDelete("detele/{id}/{companyInfoId}")]
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
