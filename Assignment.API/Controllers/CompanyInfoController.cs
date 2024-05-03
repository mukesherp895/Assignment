﻿using Assignment.API.Attributes;
using Assignment.Common;
using Assignment.DataAccess.Repository;
using Assignment.Model.Domain;
using Assignment.Model.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Assignment.API.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/v1/companyinfo")]
    [ApiController]
    //[TypeFilter(typeof(CustomAuthorizationFilter))]
    public class CompanyInfoController : ControllerBase
    {
        private readonly ICompanyInfoRepository _companyInfoRepository;
        public CompanyInfoController(ICompanyInfoRepository companyInfoRepository)
        {
            _companyInfoRepository = companyInfoRepository;
        }
        [HttpGet("getallcompanyinfos")]
        public async Task<IActionResult> GetAllCompanyInfos()
        {
            //Response Body Initialization
            ResponseDto responseDto = new ResponseDto();
            try
            {
                responseDto.Status = ResponseStatus.Success;
                responseDto.Message = "Success";
                responseDto.Data = await _companyInfoRepository.GetAllCompanyInfoAsync();
                return Ok(responseDto);
            }
            catch (Exception ex)
            {
                responseDto.Status = ResponseStatus.Fail;
                responseDto.Message = ex.Message;
                return this.StatusCode(StatusCodes.Status500InternalServerError, responseDto);
            }
        }
        [HttpPost("add")]
        public async Task<IActionResult> Add(CompanyInfoReqDto reqDto)
        {
            //Response Body Initialization
            ResponseDto responseDto = new ResponseDto();
            try
            {
                //ModelState Check
                if(ModelState.IsValid)
                {
                    //Request Data Binding to CompanyInfos Doamin
                    CompanyInfos companyInfos = new CompanyInfos
                    {
                        CompanyName = reqDto.CompanyName,
                        CreatedBy = User.Identity.Name,
                        CreatedDateTime = DateTime.Now
                    };
                    //Company Info Added
                    if(await _companyInfoRepository.AddAsync(companyInfos) == EnumData.DBAttempt.Success)
                    {
                        //Success Response
                        responseDto.Status = ResponseStatus.Success;
                        responseDto.Message = "Company Info Added Successfully";
                        return this.StatusCode(StatusCodes.Status201Created, responseDto);
                    }
                    else
                    {
                        //Fail Response
                        responseDto.Status = ResponseStatus.Fail;
                        responseDto.Message = "Company Info Add Fail";
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
        public async Task<IActionResult> Update(int id, CompanyInfoReqDto reqDto)
        {
            //Response Body Initialization
            ResponseDto responseDto = new ResponseDto();
            try
            {
                //Model State Check
                if (ModelState.IsValid)
                {
                    var companyInfo = await _companyInfoRepository.GetByIdCompanyInfoAsync(id);
                    //Check CompanyInfo Existing
                    if(companyInfo != null)
                    {
                        //Request Data Binding to Existing CompanyInfo Domain
                        companyInfo.CompanyName = reqDto.CompanyName;
                        companyInfo.UpdatedBy = User.Identity.Name;
                        companyInfo.UpdatedDateTime = DateTime.Now;

                        //CompanyInfo Updated
                        if (await _companyInfoRepository.UpdateAsync(companyInfo) == EnumData.DBAttempt.Success)
                        {
                            //Success Response
                            responseDto.Status = ResponseStatus.Success;
                            responseDto.Message = "Company Info Updated Successfully";
                            return this.StatusCode(StatusCodes.Status201Created, responseDto);
                        }
                        else
                        {
                            //Fail Response
                            responseDto.Status = ResponseStatus.Fail;
                            responseDto.Message = "Company Info Update Fail";
                            return Ok(responseDto);
                        }
                    }
                    else
                    {
                        //Invvalid Id Response
                        responseDto.Status = ResponseStatus.Fail;
                        responseDto.Message = "Invalid CompanyInfo Id";
                        return this.StatusCode(StatusCodes.Status400BadRequest, responseDto);
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
        [HttpDelete("detele/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            //Response Body Initialization
            ResponseDto responseDto = new ResponseDto();
            try
            {
                //Model State Check
                if (id > 0)
                {
                    var companyInfo = await _companyInfoRepository.GetByIdCompanyInfoAsync(id);
                    //Check CompanyInfo Existing
                    if (companyInfo != null)
                    {
                        //CompanyInfo Updated
                        if (await _companyInfoRepository.DeleteAsync(companyInfo) == EnumData.DBAttempt.Success)
                        {
                            //Success Response
                            responseDto.Status = ResponseStatus.Success;
                            responseDto.Message = "Company Info Deleted Successfully";
                            return this.StatusCode(StatusCodes.Status201Created, responseDto);
                        }
                        else
                        {
                            //Fail Response
                            responseDto.Status = ResponseStatus.Fail;
                            responseDto.Message = "Company Info Delete Fail";
                            return Ok(responseDto);
                        }
                    }
                    else
                    {
                        //Invvalid Id Response
                        responseDto.Status = ResponseStatus.Fail;
                        responseDto.Message = "Invalid Company Info Id";
                        return this.StatusCode(StatusCodes.Status400BadRequest, responseDto);
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
