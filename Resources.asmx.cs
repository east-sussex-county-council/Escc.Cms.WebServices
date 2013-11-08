using System;
using System.Web.Services;
using EsccWebTeam.Cms;
using Microsoft.ContentManagement.Publishing;

namespace Escc.Cms.WebServices
{
    /// <summary>
    /// Use the MCMS API to access information about resources in the CMS resource gallery
    /// </summary>
    [WebService(Namespace = "http://www.eastsussex.gov.uk/Escc.Cms.WebServices")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]

    public class Resources : System.Web.Services.WebService
    {
        /// <summary>
        /// Gets details of the root resource gallery and its immediate children from the current server.
        /// </summary>
        /// <returns></returns>
        [WebMethod(Description = "Gets details of the root resource gallery and its immediate children from the current server.")]
        public CmsResourceGallery RootResourceGallery()
        {
            string rootGuid = String.Empty;
            using (var cms = CmsUtilities.GetAdminContext(PublishingMode.Published))
            {
                rootGuid = cms.RootResourceGallery.Guid;
            }
            return ResourceGallery(new Guid(rootGuid));
        }


        /// <summary>
        /// Gets details of a resource gallery and its immediate children from the current server.
        /// </summary>
        /// <returns></returns>
        [WebMethod(Description = "Gets details of a resource gallery and its immediate children from the current server.")]
        public CmsResourceGallery ResourceGallery(Guid guid)
        {
            using (var cms = CmsUtilities.GetAdminContext(PublishingMode.Published))
            {
                var gallery = cms.Searches.GetByGuid("{" + guid.ToString() + "}") as ResourceGallery;
                if (gallery == null) return null;

                // Get the requested gallery and its child galleries. Only go one level, not recursive, 
                // because otherwise this method would take a long time to execute and build up a massive string.
                var cmsGallery = BuildGallery(gallery);
                foreach (ResourceGallery child in gallery.ResourceGalleries)
                {
                    cmsGallery.Galleries.Add(BuildGallery(child));
                }
                foreach (Resource child in gallery.Resources)
                {
                    cmsGallery.Resources.Add(BuildResource(child));
                }
                return cmsGallery;
            }
        }

        /// <summary>
        /// Copy the properties of a <see cref="ResourceGallery"/> to a <see cref="CmsResourceGallery"/> so that it can be serialised.
        /// </summary>
        /// <param name="gallery">The gallery.</param>
        /// <returns></returns>
        private static CmsResourceGallery BuildGallery(ResourceGallery gallery)
        {
            var cmsGallery = new CmsResourceGallery();
            cmsGallery.Name = gallery.Name;
            cmsGallery.Guid = new Guid(gallery.Guid);
            cmsGallery.Path = gallery.Path;
            return cmsGallery;
        }

        /// <summary>
        /// Copy the properties of a <see cref="Resource"/> to a <see cref="CmsResource"/> so that it can be serialised.
        /// </summary>
        /// <param name="resource">The resource.</param>
        /// <returns></returns>
        private static CmsResource BuildResource(Resource resource)
        {
            var cmsResource = new CmsResource();
            cmsResource.Name = resource.Name;
            cmsResource.DisplayName = resource.DisplayName;
            cmsResource.Guid = new Guid(resource.Guid);
            cmsResource.Path = resource.Path;
            return cmsResource;
        }
    }
}
