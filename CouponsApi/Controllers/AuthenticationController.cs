using System;
using System.Net.Http;
using System.Web.Http;
using CouponsApi.Infrastructures;
using CouponsApi.Models.Requests;
using CouponsApi.Models.Response;
using CouponsApi.Utilities.Auth;
using Coupons.DAL.Repositories;
using Coupons.DAL.Interfaces;
using Coupons.DAL;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http.Cors;

namespace CouponsApi.Controllers
{
    [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
    public class AuthenticationController : ApplicationBaseController
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        readonly string controllerName = "Authentication";

        [HttpPost]
        public HttpResponseMessage Login(LoginRequest loginRequest)
        {
            string apiName = "Login(LoginRequest loginRequest)";
            var guid = Guid.NewGuid();
            logger.Info("Guid: \"{0}\" api/{1}/{2} was invoked", guid, controllerName, apiName);

            using (CouponsEntities db = new CouponsEntities())
            {
                try
                {
                    var user = db.Users.FirstOrDefault(u => u.Email.Equals(loginRequest.UserName));
                    if (user == null)
                    {
                        return CreateGeneralResultMessage("Wrong Password or User Name", false, ApiStatusCodes.BadRequest);
                    }
                    var userEncryptedPassword = db.AuthUsers.FirstOrDefault(a => a.UserId.Equals(user.Id)).EncryptedPassword;
                    //if (loginRequest.Password.Equals(PasswordEncryptor.Decrypt(userEncryptedPassword)))
                    if(PasswordEncryptor.Encrypt(loginRequest.Password).Equals(userEncryptedPassword))
                    {
                        AuthenticationResponse response = new AuthenticationResponse();
                        response.TokenId = user.Token.ToString();

                        return CreateResponseMessage(response);
                    }
                    else
                    {
                        logger.Error("Guid: \"{0}\" Wrong Password or User Name:", guid);
                        return CreateGeneralResultMessage("Wrong Password or User Name", false, ApiStatusCodes.BadRequest);
                    }
                }
                catch (Exception ex)
                {
                    logger.Error("Guid: \"{0}\" General Error: {1}", guid, ex);
                    return CreateGeneralResultMessage(ex.Message, false, ApiStatusCodes.InternalServerError);
                }
            }
        }
    }
}
