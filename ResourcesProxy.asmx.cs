using System;
using System.Configuration;
using System.Net;
using System.Web.Services;

namespace Escc.Cms.WebServices
{
    /// <summary>
    /// Proxy for the <see cref="Resources"/> service.
    /// </summary>
    /// <remarks>This service can be hosted on a server without Microsoft CMS, and call the <see cref="Resources"/>
    /// service on a CMS server to access the CMS API remotely.</remarks>
    [WebService(Namespace = "http://www.eastsussex.gov.uk/Escc.Cms.WebServices")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]

    public class ResourcesProxy : System.Web.Services.WebService
    {
        /// <summary>
        /// Gets details of the root resource gallery and its immediate children from another server.
        /// </summary>
        /// <returns></returns>
        [WebMethod(Description = "Gets details of the root resource gallery and its immediate children from another server.")]
        public CmsResourcesProxy.CmsResourceGallery RootResourceGalleryProxy()
        {
            using (var proxy = new CmsResourcesProxy.Resources())
            {
                proxy.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["CmsAdminUser"], ConfigurationManager.AppSettings["CmsAdminPassword"], ConfigurationManager.AppSettings["CmsAdminDomain"]);
                return proxy.RootResourceGallery();
            }
        }

        /// <summary>
        /// Gets details of a resource gallery and its immediate children from another server.
        /// </summary>
        /// <returns></returns>
        [WebMethod(Description = "Gets details of a resource gallery and its immediate children from another server.")]
        public CmsResourcesProxy.CmsResourceGallery ResourceGalleryProxy(Guid guid)
        {
            using (var proxy = new CmsResourcesProxy.Resources())
            {
                proxy.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["CmsAdminUser"], ConfigurationManager.AppSettings["CmsAdminPassword"], ConfigurationManager.AppSettings["CmsAdminDomain"]);
                return proxy.ResourceGallery(guid);
            }
        }
    }
}
