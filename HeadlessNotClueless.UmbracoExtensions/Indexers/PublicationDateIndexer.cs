using HeadlessNotClueless.UmbracoExtensions.Services;
using Umbraco.Cms.Core.DeliveryApi;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Web.Common.PublishedModels;

namespace HeadlessNotClueless.UmbracoExtensions.Indexers;

public class PublicationDateIndexer(IPublishedContentService _publishedContentService) : IContentIndexHandler
{
    public IEnumerable<IndexFieldValue> GetFieldValues(IContent content, string? culture)
    {
        var results = new List<IndexFieldValue>();
        var publishedContent = _publishedContentService.ToPublishedContent(content);
        var dateValue = DateTime.MinValue;

        //Might be nice to change document types to Composition so we can check against an interface in the future
        //With a composition we don't have these structures.
        if (publishedContent is IPublicationDateComposition publicationDate)
        {
            dateValue = publicationDate.PublicationDate;
        }

        results.Add(new IndexFieldValue
        {
            FieldName = DeliveryAPIConstants.PublicationDateFieldName,
            Values = [dateValue]
        });

        return results;
    }

    public IEnumerable<IndexField> GetFields()
    {
        return
        [
            new IndexField()
            {
                FieldName =DeliveryAPIConstants.PublicationDateFieldName,
                FieldType = FieldType.Date,
                VariesByCulture = false
            }
        ];
    }
}