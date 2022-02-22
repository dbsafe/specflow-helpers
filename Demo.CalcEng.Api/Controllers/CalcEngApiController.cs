﻿using Demo.CalcEng.Domain;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Demo.CalcEng.Api.Controllers
{
    public class CalcEngApiController<TController> : ControllerBase
    {
        protected ILogger<TController> _logger;

        public CalcEngApiController(ILogger<TController> logger)
        {
            _logger = logger;
        }
               
        protected IActionResult Execute<TResult>(Func<TResult> func)
            where TResult : OperationResponse
        {
            try
            {
                var result = func();
                return Ok(result);
            }
            catch (BadRequestException ex)
            {
                _logger.LogError($"Exception: {ex.ToString()}");
                return StatusCode((int)HttpStatusCode.BadRequest, OperationResponse.CreateFailed(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception: {ex.ToString()}");
                return StatusCode((int)HttpStatusCode.OK, OperationResponse.CreateFailed("Internal Server Error"));
            }
        }
    }
}