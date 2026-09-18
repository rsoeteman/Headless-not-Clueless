using HeadlessNotClueless.UmbracoExtensions.Services;
using Umbraco.Cms.Core.DeliveryApi;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Web.Common.PublishedModels;

namespace HeadlessNotClueless.UmbracoExtensions.Indexers;

public class SiteIndexer(IPublishedContentService _publishedContentService) : IContentIndexHandler
{
    public IEnumerable<IndexFieldValue> GetFieldValues(IContent content, string? culture)
    {
        var publishedContent = _publishedContentService.ToPublishedContent(content);

        if (publishedContent is not IWebsitePickerComposition websitePicker)
        {
            return Enumerable.Empty<IndexFieldValue>();
        }
        
        var selectedWebsites = websitePicker.WebsitePicker?.Select(s => s.Key.ToString())??[""];
        return
        [
            new IndexFieldValue
            {
                FieldName = DeliveryAPIConstants.WebsiteFieldName,
                Values = selectedWebsites
            }
        ];
    }

    public IEnumerable<IndexField> GetFields()
    {
        return
        [
            new IndexField
            {
                FieldName = DeliveryAPIConstants.WebsiteFieldName,
                FieldType = FieldType.StringRaw,
                VariesByCulture = false
            }
        ];
    }

}