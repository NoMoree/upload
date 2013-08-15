using Spring.IO;
using Spring.Social.Dropbox.Api;
using Spring.Social.Dropbox.Connect;
using Spring.Social.OAuth1;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Diagnostics;

namespace DropboxUpload
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Dropbox : IDropboxModel
    {
        //Register your own Dropbox     app at https://www.dropbox.com/developers/apps
        //with "Full Dropbox" access level and set your app keys and app secret below
        public static string DropboxAppKey = "9v81y8fzhrllsfr";
        public static string DropboxAppSecret = "fe18znmwiipx4bu";

        public static DropboxServiceProvider dropboxServiceProvider =
            new DropboxServiceProvider(DropboxAppKey, DropboxAppSecret, AccessLevel.AppFolder);

        public static string[] oAuthToken = new string[]
                                            {   
                                                "8p3tkideq8apexvv",
                                                "8jdzfytgs1tu0ty"
                                            };


        public static OAuthToken LoadOAuthToken()
        {
            //string[] lines = File.ReadAllLines(OAuthTokenFileName);
            OAuthToken oauthAccessToken = new OAuthToken(oAuthToken[0], oAuthToken[1]);
            return oauthAccessToken;
        }

        public string Upload(string file)
        {
            OAuthToken oauthAccessToken = LoadOAuthToken();

            // Login in Dropbox
            IDropbox dropbox = dropboxServiceProvider.GetApi(oauthAccessToken.Value, oauthAccessToken.Secret);

            // Display user name (from his profile)
            DropboxProfile profile = dropbox.GetUserProfileAsync().Result;

            // Create new folder
            string newFolderName = "New_Folder_" + DateTime.Now.Ticks;
            Entry createFolderEntry = dropbox.CreateFolderAsync(newFolderName).Result;

            var splitetFileDirectory = file.Split(new char[] { '/', '\\' });
            string fileName = splitetFileDirectory[splitetFileDirectory.Length - 1];

            // Upload a file
            Entry uploadFileEntry = dropbox.UploadFileAsync(
                new FileResource(file),//("../../DropboxExample.cs"),
                "/" + newFolderName + fileName).Result;//"/DropboxExample.cs").Result;

            // Share a file
            DropboxLink sharedUrl = dropbox.GetShareableLinkAsync(uploadFileEntry.Path).Result;

            //Open the file location
            return sharedUrl.Url.ToString();
        }
    }
}
