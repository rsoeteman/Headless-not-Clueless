using Umbraco.Cms.Core.DeliveryApi;

namespace HeadlessNotClueless.UmbracoExtensions.Filters;

public class SiteFilter : IFilterHandler
{
    public bool CanHandle(string query)
    {
        return !string.IsNullOrWhiteSpace(query) &&
               query.StartsWith(DeliveryAPIConstants.WebsiteFieldName, StringComparison.OrdinalIgnoreCase);
    }

    public FilterOption BuildFilterOption(string filter)
    {
        if (string.IsNullOrWhiteSpace(filter))
        {
            throw new ArgumentException("Filter parameter cannot be null or empty", nameof(filter));
        }
        var specifierName = $"{DeliveryAPIConstants.WebsiteFieldName}:";
        
        var fieldValue = filter.Substring(specifierName.Length);
        var values = fieldValue.Split(',').Select(x => x.ToLower()).ToArray();

        return new FilterOption
        {
            FieldName = DeliveryAPIConstants.WebsiteFieldName,
            Values = values,
            Operator = FilterOperation.Contains
        };

    }
}