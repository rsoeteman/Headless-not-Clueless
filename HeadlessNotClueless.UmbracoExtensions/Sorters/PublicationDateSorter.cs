using Umbraco.Cms.Core;
using Umbraco.Cms.Core.DeliveryApi;

namespace HeadlessNotClueless.UmbracoExtensions.Sorters;

public class PublicationDateSorter
{
    private const string Descending = "desc";
    
    public bool CanHandle(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            throw new ArgumentException("Sort parameter cannot be null or empty", nameof(query));
        }

        return query.StartsWith(DeliveryAPIConstants.PublicationDateFieldName, StringComparison.OrdinalIgnoreCase);
    }

    public SortOption BuildSortOption(string sort)
    {
        var specifierName = $"{DeliveryAPIConstants.PublicationDateFieldName}:";
        var sortValue = sort.Substring(specifierName.Length);
        var direction = sortValue.Equals(Descending,StringComparison.InvariantCultureIgnoreCase)? Direction.Descending:  Direction.Ascending;
        
        return new SortOption
        {
            FieldName = DeliveryAPIConstants.PublicationDateFieldName,
            Direction = direction
        };
    }

}