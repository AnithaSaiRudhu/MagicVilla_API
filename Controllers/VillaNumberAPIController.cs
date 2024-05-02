using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.DTO;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using MagicVilla_VillaAPI.Logging;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using MagicVilla_VillaAPI.Repository.IRepository;
using System.Net;

namespace MagicVilla_VillaAPI.Controllers
{
    [Route("api/VillaNumberAPI")]    
    [ApiController]
    public class VillaNumberAPIController : ControllerBase
    {
        protected APIResponse _response;
        private readonly ILogging _logger;        

        private readonly IMapper _mapper;

        private readonly IVillaNumberRepository _dbVillaNumber;

        public VillaNumberAPIController(ILogging logger,ApplicationDBContext db,IMapper mapper, IVillaNumberRepository dbVillaNum)
        {
            _logger = logger;            
            _mapper = mapper;
            _dbVillaNumber = dbVillaNum;
            this._response = new ();
        }

        [HttpGet]
        public async Task<ActionResult<APIResponse>> GetVillaNumbers()
        {
            try
            {
                IEnumerable<VillaNumber> villaNumberList = await _dbVillaNumber.GetAllAsync();
                _response.Result = _mapper.Map<List<VillaNumberDTO>>(villaNumberList);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch(Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpGet("{id:int}", Name = "GetVillaNumber")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> GetVillaNumber(int villaNo)
        {
            try
            {
                if (villaNo == 0)
                {
                    _logger.Log("Get villa ID" + villaNo, "warning");
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return Ok(_response);
                }
                var villaNumber = await _dbVillaNumber.GetAsync(u => u.villaNo == villaNo, false);

                if (villaNumber == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return Ok(_response);                    
                }

                _response.Result = _mapper.Map<VillaNumberDTO>(villaNumber);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CreateVillaNumber([FromBody] VillaNumberCreateDTO villaNumberCreateDTO)
        {
            try
            {
                if (await _dbVillaNumber.GetAsync(u => u.villaNo == villaNumberCreateDTO.villaNo) != null)
                {
                    ModelState.AddModelError("Custome Error", "Villa Number Already Exists");                    
                    _response.Result = ModelState;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return Ok(_response);
                }

                if (villaNumberCreateDTO == null)
                {
                    _response.Result = villaNumberCreateDTO;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return Ok(_response);                    
                }

                VillaNumber VillaNumber = _mapper.Map<VillaNumber>(villaNumberCreateDTO);

                await _dbVillaNumber.CreateAsync(VillaNumber);
                _response.Result = _mapper.Map<VillaNumberDTO>(VillaNumber);
                _response.StatusCode = HttpStatusCode.OK;
                return CreatedAtRoute("GetVilla", new { id = VillaNumber.villaNo }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;

        }


        [HttpDelete("{id:int}", Name = "DeleteVillaNumber")]

        public async Task<ActionResult<APIResponse>> DeleteVillaNumber(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return Ok(_response);
                }

                var villaNumber = await _dbVillaNumber.GetAsync(u => u.villaNo == id);

                if (villaNumber == null)
                {
                    _response.StatusCode = HttpStatusCode.NoContent;
                    return Ok(_response);
                }

                await _dbVillaNumber.DeleteAsync(villaNumber);
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpPut("{id:int}", Name = "UpdateVillaNumber")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult<APIResponse>> UpdateVillaNumber(int id, [FromBody] VillaNumberUpdateDTO villaNumberupdateDTO)
        {
            try
            {
                if (villaNumberupdateDTO == null || id != villaNumberupdateDTO.villaNo)
                {
                    return BadRequest();
                }
                VillaNumber model = _mapper.Map<VillaNumber>(villaNumberupdateDTO);

                await _dbVillaNumber.UpdateAsync(model);
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }


        [HttpPatch("{id:int}", Name = "UpdatePartialVillaNumber")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<ActionResult<APIResponse>> UpdatePartialVillaNumber(int id, JsonPatchDocument<VillaNumberUpdateDTO> villaNumberpatchDTO)
        {
            try
            {
                if (villaNumberpatchDTO == null || id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return Ok(_response);
                }
                var villaNumber = await _dbVillaNumber.GetAsync(u => u.villaNo == id, false);

                VillaNumberUpdateDTO villaNumberUpdDTO = _mapper.Map<VillaNumberUpdateDTO>(villaNumber);

                if (villaNumber == null)
                {                    
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return Ok(_response);
                }

                villaNumberpatchDTO.ApplyTo(villaNumberUpdDTO, ModelState);

                VillaNumber model = _mapper.Map<VillaNumber>(villaNumberUpdDTO);

                await _dbVillaNumber.UpdateAsync(model);

                if (!ModelState.IsValid)
                {
                    _response.Result = ModelState;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return Ok(_response);
                }
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }
    }
}
