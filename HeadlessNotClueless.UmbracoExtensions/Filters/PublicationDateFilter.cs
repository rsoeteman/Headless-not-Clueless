using Umbraco.Cms.Core.DeliveryApi;
using Umbraco.Extensions;

namespace HeadlessNotClueless.UmbracoExtensions.Filters;

public class PublicationDateFilter : IFilterHandler
{
    public bool CanHandle(string query)
    {
        return query?.StartsWith(DeliveryAPIConstants.PublicationDateFieldName, StringComparison.InvariantCultureIgnoreCase) ?? false;

    }

    public FilterOption BuildFilterOption(string filter)
    {
        var specifierName = $"{DeliveryAPIConstants.PublicationDateFieldName}:";
        var fieldValue = DateTime.UtcNow.ToIsoString();
        
        return new FilterOption
        {
            FieldName = DeliveryAPIConstants.PublicationDateFieldName,
            Values = [fieldValue],
            Operator = FilterOperation.LessThanOrEqual
        };

    }
}