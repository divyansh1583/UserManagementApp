using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace UserManagementAPI.Domain.Common_Models
{
    public static class EmailBody
    {
        public static string EmailStringBody(string email, string emailToken)
        {
            string encodedToken = HttpUtility.UrlEncode(emailToken);
            return $@"<html>
                    <head></head>
                    <body style=""margin:0; padding:0;font-family: Arial, Helvetica,   
                     sans-serif;"">
                        <div style=""height: auto; background: linear-gradient(to top, #c9c9ff 50%, #6e6ef6 90%) no-repeat; width: 400px; padding:30px"">
                            <div>
                                <div>
                                    <h1>Reset your Password</h1>
                                    <hr>
                                    <p>You're receiving this e-mail because you requested a password reset for your   
                     Let's Program account.</p>
                                    <p>Please tap the button below to choose a new password.</p>
                                    <a href=""http://localhost:4200/reset-password?email={email}&code={encodedToken}"" target=""_blank"" style=""background:   
                     #9d6ef; color:white; border-radius: 4px; display:block;margin:0 auto;width:50%;text-align:center;text-decoration:none"">Reset</a>
                                    <p>Kind Regards, <br><br>   
                    
                                    Divyansh Chauhan<br>
                                    Admin DC LLC</p>
                                </div>
                            </div>
                        </div>
                    </body>
                    </html>";
        }
    }
}
