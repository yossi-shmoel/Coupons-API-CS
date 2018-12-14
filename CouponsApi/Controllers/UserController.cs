using System;
using System.Net.Http;
using System.Web.Http;
using CouponsApi.Infrastructures;
using CouponsApi.Models.Requests;
using CouponsApi.Utilities.Auth;
using Coupons.DAL.Repositories;
using Coupons.DAL.Interfaces;
using Coupons.DAL;
using System.Web.Http.Cors;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace CouponsApi.Controllers
{
    [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
    public class UserController : ApplicationBaseController
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        readonly string controllerName = "Users";

        [HttpPost]
        public HttpResponseMessage CreateOrUpdate(UserRequest userRequest)
        {
            string apiName = "CreateOrUpdate(UserRequest userRequest=\n{" + userRequest + "})";
            var guid = Guid.NewGuid();
            logger.Info("Guid: \"{0}\" api/{1}/{2} was invoked", guid, controllerName, apiName);

            IUsersRepository usersRepository = new UsersRepository();

            try
            {
                if (!ModelState.IsValidField("userRequest.Email"))
                {
                    ;//  return CreateGeneralResultMessage("Invalid Email Address", false, ApiStatusCodes.InvalidEmail);
                }

                var userToUpdate = usersRepository.GetUserByEmail(userRequest.Email);

                if (userToUpdate == null)
                {//Create
                    logger.Info("Guid: \"{0}\" Start Creating new User", guid);

                    var token = TokenGenerator.GenerateToken(userRequest.Email);
                    var newUser = new Users
                    {
                        Name = userRequest.Name,
                        Email = userRequest.Email,
                        CompanyId = userRequest.CompanyId,
                        Type = userRequest.Type,
                        Token = token
                    };
                    usersRepository.Create(newUser);

                    var createdUser = usersRepository.GetUserByToken(token);
                    usersRepository.AddNewAuthUser(createdUser.Id, PasswordEncryptor.Encrypt(userRequest.Password));

                    return CreateResponseMessage(newUser);
                }
                else
                {//Update
                    logger.Info("Guid: \"{0}\" Start Updating User", guid);

                    userToUpdate.Name = userRequest.Name;
                    userToUpdate.Email = userRequest.Email;
                    userToUpdate.CompanyId = userRequest.CompanyId;
                    userToUpdate.Type = userRequest.Type;
                   
                    return CreateResponseMessage(userToUpdate);
                }
            }
            catch (Exception ex)
            {
                logger.Error("Guid: \"{0}\" General Error: {1}", guid, ex);
                return CreateGeneralResultMessage(ex.ToString(), false, ApiStatusCodes.InternalServerError);
            }
        }

        [HttpGet]
        public HttpResponseMessage Get(long userId)
        {
            string apiName = "Get()";
            var guid = Guid.NewGuid();
            logger.Info("Guid: \"{0}\" api/{1}/{2} was invoked", guid, controllerName, apiName);

            IUsersRepository usersRepository = new UsersRepository();
            try
            {
                var user = usersRepository.Get(userId);
                return CreateResponseMessage(user);
            }
            catch (Exception ex)
            {
                logger.Error("Guid: \"{0}\" General Error: {1}", guid, ex);
                return CreateGeneralResultMessage(ex.ToString(), false, ApiStatusCodes.InternalServerError);
            }
        }

        [HttpGet]
        //[EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
        public HttpResponseMessage GetAll()
        {
            string apiName = "GetAll()";
            var guid = Guid.NewGuid();
            logger.Info("Guid: \"{0}\" api/{1}/{2} was invoked", guid, controllerName, apiName);

            IUsersRepository usersRepository = new UsersRepository();
            try
            {
                var response = usersRepository.GetUsers();

                string logResponseData=string.Empty;
                foreach (var data in response)
                {
                    logResponseData += data.ToString() +"\n";
                }

                logger.
                    Info("Guid: \"{0}\" api/{1}/{2}\nResponse Data:\n{3}",
                    guid, controllerName, apiName, logResponseData);
                return CreateResponseMessage(response);
            }
            catch (Exception ex)
            {
                logger.Error("Guid: \"{0}\" General Error: {1}", guid, ex);
                return CreateGeneralResultMessage(ex.ToString(), false, ApiStatusCodes.InternalServerError);
            }
        }

        [HttpDelete]
        public HttpResponseMessage Delete(long userId)
        {
            string apiName = "Delete(long userId={" + userId+"})";
            var guid = Guid.NewGuid();
            logger.Info("Guid: \"{0}\" api/{1}/{2} was invoked", guid, controllerName, apiName);

            IUsersRepository usersRepository = new UsersRepository();
            IUserCouponsRepository userCouponsRepository = new UserCouponsRepository();
            try
            {
                userCouponsRepository.RemoveAllByUser(userId);
                usersRepository.Delete(userId);

                logger.
                    Info("Guid: \"{0}\" api/{1}/{2}, Message:{3} UserId={4}",
                    guid, controllerName, apiName, "User was deleted", userId);
                return CreateGeneralResultMessage("Success", true);
            }
            catch (Exception ex)
            {
                logger.Error("Guid: \"{0}\" General Error: {1}", guid, ex);
                return CreateGeneralResultMessage(ex.ToString(), false, ApiStatusCodes.InternalServerError);
            }
        }

        [HttpPost]
        public HttpResponseMessage SetCouponToUser(PurchaseRequest purchaseRequest)
        {
            string apiName = "SetCouponUser(PurchaseRequest purchaseRequest={"+ purchaseRequest + "})";
            var guid = Guid.NewGuid();
            logger.Info("Guid: \"{0}\" api/{1}/{2} was invoked", guid, controllerName, apiName);

            IUsersRepository usersRepository = new UsersRepository();
            IUserCouponsRepository userCouponsRepository = new UserCouponsRepository();
            try
            {
                var currentUser = usersRepository.GetUserByToken(GetToken());
                if (currentUser == null)
                {
                    logger.Error("Guid: \"{0}\"User Not found", guid);
                    return CreateGeneralResultMessage("User Not found", false, ApiStatusCodes.UserNotExists);
                }

                var isCouponPurchased = userCouponsRepository.purchaseCoupon(currentUser.Id, purchaseRequest.CouponId);

                if(isCouponPurchased)
                {
                    return CreateGeneralResultMessage("Success", true);
                }
                else
                {
                    return CreateGeneralResultMessage("no more Coupons left", false, ApiStatusCodes.NoCouponLeft);
                }
            }
            catch (Exception ex)
            {
                logger.Error("Guid: \"{0}\" General Error: {1}", guid, ex);
                return CreateGeneralResultMessage(ex.ToString(), false, ApiStatusCodes.InternalServerError);
            }
        }
    }
}
