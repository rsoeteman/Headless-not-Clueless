using Umbraco.Cms.Core;
using Umbraco.Cms.Core.DeliveryApi;

namespace HeadlessNotClueless.UmbracoExtensions.Sorters;

public class PublicationDateSorter : ISortHandler
{
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
        //always sort descending in case of news
        return new SortOption
        {
            FieldName = DeliveryAPIConstants.PublicationDateFieldName,
            Direction = Direction.Descending
        };
    }

}