using System.Web;
using Sitecore.Diagnostics;
using Sitecore.Pipelines.Upload;

namespace JammyKam.SC.ModalFix.Pipelines.Upload
{
    /// <summary>
    /// Upload has finished.
    /// 
    /// </summary>
    public class Done : Sitecore.Pipelines.Upload.Done
    {
        public new void Process(UploadArgs args)
        {
            if (Sitecore.UIUtil.IsIE())
            {
                // We need to use the original functionality for IE
                base.Process(args);
                return;
            }

            Assert.ArgumentNotNull((object)args, "args");
            if (!args.CloseDialogOnEnd)
                return;
            string str1 = args.Parameters["message"];
            if (!string.IsNullOrEmpty(str1))
            {
                string str2 = HttpUtility.UrlEncode(args.Properties["filename"] as string ?? string.Empty);
                HttpContext.Current.Response.Write(string.Format("<html><head><script type='text/JavaScript' language='javascript'>window.top.scForm.browser.getTopModalDialog().frames[0].scForm.postRequest('', '', '', '{0}(filename={1})')</script></head><body>Done</body></html>", (object)str1, (object)str2));
            }
            else
                HttpContext.Current.Response.Write("<html><head><script type=\"text/JavaScript\" language=\"javascript\">window.top.scForm.browsr.getTopModalDialog().frames[0].scForm.postRequest(\"\", \"\", \"\", \"EndUploading\")</script></head><body>Done</body></html>");
        }
    }
}
