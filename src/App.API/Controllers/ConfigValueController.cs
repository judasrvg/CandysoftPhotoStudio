using App.Application.Abstractions;
using App.Application.Abstractions.Services;
using App.Application.DTOs;
using App.Domain.Enum;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigValueController : ControllerBase
    {
        private readonly IConfigValueCommandService _ConfigValueCommandService;
        private readonly INotificationCommandServices _NotificationCommandService;
        private readonly IConfigValueQueryService _ConfigValueQueryService;
                
        //[HttpPost("Notificate")]
        //public async Task NotificateMessage([FromBody] string message)//you can use MessageInfoDto, for now only the message
        //{
        //        var newMessageInfoList = new HashSet<MessageInfoDto>();
                
        //        newMessageInfoList.Add(new MessageInfoDto() { Message = message, ChatId = "" });
                
        //        await _NotificationCommandService.SendMessageAsync(newMessageInfoList);
        //}

        public ConfigValueController(IConfigValueCommandService ConfigValueCommandService, IConfigValueQueryService ConfigValueQueryService)
        {
            _ConfigValueCommandService = ConfigValueCommandService;
            _ConfigValueQueryService = ConfigValueQueryService;
        }

        [HttpGet("GetOfferConfigTypes")]
        public async Task<IActionResult> GetOfferConfigTypes()
        {
            var ConfigValue = await _ConfigValueQueryService.GetOfferConfigValuesAsync();
            if (ConfigValue == null || !ConfigValue.Any())
                return NotFound();
            return Ok(ConfigValue);
        }

        [HttpGet("GetBasicsConfigTypes")]
        public async Task<IActionResult> GetBasicsConfigTypes()
        {
            var ConfigValue = await _ConfigValueQueryService.GetBasicsConfigValuesAsync();
            if (ConfigValue == null || !ConfigValue.Any())
                return NotFound();
            return Ok(ConfigValue);
        }

        [HttpGet("{configType}")]
        public async Task<IActionResult> Get(CacheValueType configType)
        {
            var ConfigValue = await _ConfigValueQueryService.GetConfigValuesByTypeAsync( configType);
            if (ConfigValue == null || !ConfigValue.Any())
                return NotFound();
            return Ok(ConfigValue);
        }

        [HttpPost("AddConfigValue")]
        public async Task<IActionResult> AddConfigValue([FromBody] ConfigValueDto ConfigValue)
        {
            var configId= await _ConfigValueCommandService.AddOrUpdateConfigValueAsync(ConfigValue);
            ConfigValue.Id = configId;
            return CreatedAtAction(nameof(Get), new { id = configId, configType = ConfigValue.ValueType }, ConfigValue);
        }

        [HttpPut("UpdateConfigValue")]
        public async Task<IActionResult> UpdateConfigValue([FromBody] ConfigValueDto ConfigValue)
        {
            var configId = await _ConfigValueCommandService.AddOrUpdateConfigValueAsync(ConfigValue);
            ConfigValue.Id = configId;

            return CreatedAtAction(nameof(Get), new { id = configId, configType = ConfigValue.ValueType }, ConfigValue);
        }

        [HttpDelete("{id:long}/{configType}")]
        public async Task<IActionResult> Delete(long id, CacheValueType configType)
        {
            await _ConfigValueCommandService.DeleteConfigValueAsync(id, configType);
            return Ok(id);
        }
    }
}
