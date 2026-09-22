using HeadlessNotClueless.UmbracoExtensions.Resolvers;
using HeadlessNotClueless.UmbracoExtensions.Services;
using Microsoft.Extensions.DependencyInjection;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DeliveryApi;
using Umbraco.Cms.Core.DependencyInjection;

namespace HeadlessNotClueless.UmbracoExtensions.Composers;

public class ExtensionsComposer : IComposer
{
    public void Compose(IUmbracoBuilder builder)
    {
        //Helper for publishedcontentservice
        builder.Services.AddSingleton<IPublishedContentService, PublishedContentService>();
        
        //Resolver to find delivery api nodes elsewhere in the site.
        builder.Services.AddSingleton<IApiContentPathResolver, CustomPathResolver>();
    }
}