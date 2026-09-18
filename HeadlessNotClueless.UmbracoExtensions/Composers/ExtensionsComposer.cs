using HeadlessNotClueless.UmbracoExtensions.Services;
using Microsoft.Extensions.DependencyInjection;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;

namespace HeadlessNotClueless.UmbracoExtensions.Composers;

public class ExtensionsComposer : IComposer
{
    public void Compose(IUmbracoBuilder builder)
    {
        builder.Services.AddSingleton<IPublishedContentService, PublishedContentService>();
    }
}