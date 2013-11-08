using System;
using System.Web.Services;
using EsccWebTeam.Cms;
using Microsoft.ContentManagement.Publishing;

namespace Escc.Cms.WebServices
{
    /// <summary>
    /// Modify the publication and expiry dates of postings
    /// </summary>
    [WebService(Namespace = "http://www.eastsussex.gov.uk/Escc.Cms.WebServices")]
    public class PostingDateUtils : System.Web.Services.WebService
    {
        private bool updateExpired;

        /// <summary>
        /// Publishes all postings in a given channel which are not already published. Approves postings which are Saved or Waiting for Editor Approval.
        /// </summary>
        /// <param name="channelPath">A channel path, eg /environment/rubbishandrecycling/recyclingsites/.</param>
        /// <returns></returns>
        [WebMethod(Description = "Publishes all postings in a given channel which are not already published. Approves postings which are Saved or Waiting for Editor Approval.")]
        public string PublishPostings(string channelPath)
        {
            string retval;
            // get the current cms application context
            using (CmsApplicationContext cac = CmsUtilities.GetAdminContext(PublishingMode.Update))
            {
                var currentChannel = cac.Searches.GetByUrl(channelPath) as Channel;
                try
                {

                    foreach (Posting posting in currentChannel.Postings)
                    {
                        if (posting.StartDate > DateTime.Now)
                        {
                            posting.StartDate = DateTime.Now;
                            posting.Approve();
                        }

                        if (posting.State == PostingState.Saved | posting.State == PostingState.WaitingForEditorApproval)
                        {
                            posting.StartDate = DateTime.Now;
                            posting.Approve();
                        }

                    }
                    cac.CommitAll();
                    retval = "Channel postings have been approved and published.";
                }
                catch (Exception ex)
                {
                    retval = ex.Message;
                }
            }
            return retval;
        }

        DateTime newExpiry;

        /// <summary>
        /// Extends posting expiry dates by a number of months (or blank for never).
        /// </summary>
        /// <param name="channelPath">The channel path.</param>
        /// <param name="months">The months.</param>
        /// <param name="includeSubchannels">if set to <c>true</c> include subchannels.</param>
        /// <param name="updateExpiredPages">if set to <c>true</c> update expired pages.</param>
        /// <returns></returns>
        [WebMethod(Description = "Extends posting expiry dates by a number of months (or blank for never). Expects a channel path such as '/environment/rubbishandrecycling/recyclingsites/', the number of months to extend expiry by (or blank for 'never'), and either 'true' or 'false'")]
        public string SetExpiryDate(string channelPath, string months, bool includeSubchannels, bool updateExpiredPages)
        {
            this.newExpiry = (String.IsNullOrEmpty(months)) ? CmsUtilities.NeverExpires : DateTime.Now.AddMonths(Convert.ToInt32(months));
            string retval = String.Empty;
            // get the current cms application context
            string channelGuid = null;
            this.updateExpired = updateExpiredPages;

            using (CmsApplicationContext cac = CmsUtilities.GetAdminContext(PublishingMode.Published))
            {
                // we need to authenticate against a cms mode and MUST do this before using searches to get he current channel
                Channel currentChannel = CmsUtilities.ParseChannelUrl(channelPath, cac);
                if (currentChannel == null)
                {
                    retval = "Channel not found";
                }
                else
                {
                    channelGuid = currentChannel.Guid;
                }
            }

            if (channelGuid != null)
            {
                try
                {
                    if (includeSubchannels)
                    {

                        CmsTraverser traverser = new CmsTraverser();
                        traverser.TraversingChannel += new CmsEventHandler(traverser_TraversingChannel);
                        traverser.TraverseChannel(PublishingMode.Update, true, new Guid(channelGuid));
                    }
                    else
                    {

                        ExtendChannel(newExpiry, new Guid(channelGuid));
                    }
                    retval = "Channel posting expiry dates have been delayed until " + newExpiry;
                }
                catch (Exception ex)
                {
                    retval = ex.Message;
                }
            }

            return retval;
        }
        void traverser_TraversingChannel(object sender, CmsEventArgs e)
        {
            ExtendChannel(this.newExpiry, new Guid(e.Channel.Guid));

        }

        private void ExtendChannel(DateTime newExpiry, Guid channelGuid)
        {
            using (CmsContext cms = CmsUtilities.GetAdminContext(PublishingMode.Update))
            {
                Channel channelToExtend = cms.Searches.GetByGuid(channelGuid.ToString("B")) as Channel;
                foreach (Posting posting in channelToExtend.Postings)
                {
                    if (this.updateExpired || posting.State != PostingState.Expired)
                    {
                        posting.ExpiryDate = newExpiry;
                        posting.Approve();
                    }
                }
                cms.CommitAll();
            }
        }

    }
}
