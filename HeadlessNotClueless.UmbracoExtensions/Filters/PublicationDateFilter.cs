using Umbraco.Cms.Core.DeliveryApi;

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
        var fieldValue = filter.Substring(specifierName.Length);
        
        return new FilterOption
        {
            FieldName = DeliveryAPIConstants.PublicationDateFieldName,
            Values = [fieldValue],
            Operator = FilterOperation.GreaterThanOrEqual
        };

    }
}