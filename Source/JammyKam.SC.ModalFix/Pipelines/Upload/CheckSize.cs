using System.Collections.Specialized;
using System.Web;
using Sitecore;
using Sitecore.Configuration;
using Sitecore.Diagnostics;
using Sitecore.Pipelines.Upload;
using Sitecore.Zip;

namespace JammyKam.SC.ModalFix.Pipelines.Upload
{
    public class CheckSize : Sitecore.Pipelines.Upload.CheckSize
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
            if (args.Destination == UploadDestination.File)
                return;
            foreach (string index in (NameObjectCollectionBase)args.Files)
            {
                HttpPostedFile file = args.Files[index];
                if (!string.IsNullOrEmpty(file.FileName))
                {
                    if (UploadProcessor.IsUnpack(args, file))
                    {
                        ZipReader zipReader = new ZipReader(file.InputStream);
                        try
                        {
                            foreach (ZipEntry zipEntry in zipReader.Entries)
                            {
                                if (!zipEntry.IsDirectory && zipEntry.Size > Settings.Media.MaxSizeInDatabase)
                                {
                                    string text = file.FileName + "/" + zipEntry.Name;
                                    HttpContext.Current.Response.Write("<html><head><script type=\"text/JavaScript\" language=\"javascript\">window.top.scForm.browser.getTopModalDialog().frames[0].scForm.postRequest(\"\", \"\", \"\", 'ShowFileTooBig(" + StringUtil.EscapeJavascriptString(text) + ")')</script></head><body>Done</body></html>");
                                    args.ErrorText = string.Format("The file \"{0}\" is too big to be uploaded. The maximum size for uploading files is {1}.", (object)text, (object)MainUtil.FormatSize(Settings.Media.MaxSizeInDatabase));
                                    args.AbortPipeline();
                                    return;
                                }
                            }
                        }
                        finally
                        {
                            file.InputStream.Position = 0L;
                        }
                    }
                    else if ((long)file.ContentLength > Settings.Media.MaxSizeInDatabase)
                    {
                        string fileName = file.FileName;
                        HttpContext.Current.Response.Write("<html><head><script type=\"text/JavaScript\" language=\"javascript\">window.top.scForm.browser.getTopModalDialog().frames[0].scForm.postRequest(\"\", \"\", \"\", 'ShowFileTooBig(" + StringUtil.EscapeJavascriptString(fileName) + ")')</script></head><body>Done</body></html>");
                        args.ErrorText = string.Format("The file \"{0}\" is too big to be uploaded. The maximum size for uploading files is {1}.", (object)fileName, (object)MainUtil.FormatSize(Settings.Media.MaxSizeInDatabase));
                        args.AbortPipeline();
                        break;
                    }
                }
            }
        }
    }
}