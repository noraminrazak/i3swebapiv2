using System;
using System.IO;
using System.Net;

namespace I3SwebAPIv2.Class
{
    public class FileTransferProtocol
    {
        public bool Check_Directory_Exists(string address, string directory, string username, string password)
        {
            /* Create an FTP Request */
            FtpWebRequest ftpRequest = (FtpWebRequest)FtpWebRequest.Create("ftp://" + address + directory);
            /* Log in to the FTP Server with the User Name and Password Provided */
            ftpRequest.Credentials = new NetworkCredential(username, password);
            /* Specify the Type of FTP Request */
            ftpRequest.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
            try
            {
                using (FtpWebResponse response = (FtpWebResponse)ftpRequest.GetResponse())
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                string status = Convert.ToString(ex);
                return false;
            }
            finally
            {
                ftpRequest = null;
            }
        }

        public bool Create_Directory(string ftp_address, string folder, string ftp_username, string ftp_password)
        {
            bool success = false;

            System.Net.FtpWebRequest ftp_web_request = null;
            System.Net.FtpWebResponse ftp_web_response = null;

            string ftp_path = "ftp://" + ftp_address + folder;

            try
            {
                ftp_web_request = (FtpWebRequest)WebRequest.Create(ftp_path);
                ftp_web_request.Method = WebRequestMethods.Ftp.MakeDirectory;
                ftp_web_request.Credentials = new NetworkCredential(ftp_username, ftp_password);

                ftp_web_response = (FtpWebResponse)ftp_web_request.GetResponse();

                string ftp_response = ftp_web_response.StatusDescription;
                string status_code = Convert.ToString(ftp_web_response.StatusCode);

                ftp_web_response.Close();

                success = true;
            }
            catch (Exception ex)
            {
                string status = Convert.ToString(ex);
                success = false;
                //MessageBox.Show("Failed to create folder." + Environment.NewLine + status); //debug
            }

            return success;
        }

        public bool Retrieve_Delete_Directory_File(string ftp_address, string folder, string ftp_username, string ftp_password)
        {
            bool success = true;
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://"+ ftp_address + folder);
            request.Credentials = new NetworkCredential(ftp_username, ftp_password);
            request.Method = WebRequestMethods.Ftp.ListDirectory;

            StreamReader streamReader = new StreamReader(request.GetResponse().GetResponseStream());

            string fileName = streamReader.ReadLine();

            while (fileName != null)
            {
                //Console.Writeline(fileName);
                Uri url = new Uri("ftp://" + ftp_address + folder + "/" + fileName);
                fileName = streamReader.ReadLine();
                if (Delete_File(url, ftp_username,ftp_password) == true) {
                    //success = true;
                }
            }

            request = null;
            streamReader = null;

            return success;
        }

        public bool Delete_File(Uri serverUri, string ftp_username, string ftp_password)
        {
            if (serverUri.Scheme != Uri.UriSchemeFtp)
            {
                return false;
            }

            FtpWebRequest requestFileDelete = (FtpWebRequest)WebRequest.Create(serverUri);
            requestFileDelete.Credentials = new NetworkCredential(ftp_username, ftp_password);
            requestFileDelete.Method = WebRequestMethods.Ftp.DeleteFile;

            FtpWebResponse responseFileDelete = (FtpWebResponse)requestFileDelete.GetResponse();
            responseFileDelete.Close();
            return true;
        }
    }
}