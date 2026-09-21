using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Api.Delivery.Controllers;
using Umbraco.Cms.Api.Delivery.Filters;
using Umbraco.Cms.Api.Delivery.Routing;
using Umbraco.Cms.Core.Services;

namespace HeadlessNotClueless.UmbracoExtensions.Controllers;

[ApiVersion("2.0")]
[VersionedDeliveryApiRoute("translations")]
[ApiExplorerSettings(GroupName = "Translations")]
[DeliveryApiAccess]
public class TranslationAPIController(IDictionaryItemService _dictionaryItemService) : DeliveryApiControllerBase
{
    [HttpGet]
    [MapToApiVersion("2.0")]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<object> GetAllTranslationsAsync(string locale, string? startItem)
    {
        var startItemKey = await GetStartItemKeyAsync(startItem);
        //Get all descendants of startItemKey, or all dictionary items when startItemKey is null
        var allItems = await _dictionaryItemService.GetDescendantsAsync(startItemKey);
     
        //Based on allItems return all keys and translations for a given locale, only 
        //translations that contain an actual value will be included in the result.
        return allItems
            .Select(item => new
            {
                ItemKey = char.ToLowerInvariant(item.ItemKey[0]) + item.ItemKey[1..],
                Translation = item.Translations
                    .FirstOrDefault(t => t.LanguageIsoCode.StartsWith(locale, StringComparison.OrdinalIgnoreCase))
            })
            .Where(x => !string.IsNullOrWhiteSpace(x.Translation?.Value))
            .ToDictionary(x => x.ItemKey, x => x.Translation!.Value, StringComparer.OrdinalIgnoreCase);
    }

    /// <summary>
    /// When A startitem is given, the guid (id) of that dictionary item so that the
    /// results will be descendants of that key 
    /// </summary>
    private async Task<Guid?> GetStartItemKeyAsync(string? startItem)
    {
        if (string.IsNullOrWhiteSpace(startItem))
        {
            return null;
        }

        var dictionaryItem = await _dictionaryItemService.GetAsync(startItem);
        return dictionaryItem?.Key;
    }

    
}