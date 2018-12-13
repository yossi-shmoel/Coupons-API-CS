using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CouponsApi.Infrastructures;
using CouponsApi.Models.Requests;
using Coupons.DAL.Repositories;
using Coupons.DAL.Interfaces;
using Coupons.DAL;

namespace CouponsApi.Controllers
{
    public class CouponsController : ApplicationBaseController
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        readonly string controllerName = "Coupons";

        [HttpPost]
        public HttpResponseMessage CreateOrUpdate(CouponRequest couponRequest)
        {
            string apiName = "CreateOrUpdate(CouponRequest couponRequest=\n{" + couponRequest + "})";
            var guid = Guid.NewGuid();
            logger.Info("Guid: \"{0}\" api/{1}/{2} was invoked", guid, controllerName, apiName);

            ICouponRepository couponRepository = new CouponRepository();

            try
            {
                var couponToUpdate = couponRepository.GetByNameAndCompanyId(couponRequest.Name, couponRequest.CompanyId);

                if (couponToUpdate == null)
                {//Create
                    logger.Info("Guid: \"{0}\" Start Creating new Coupon", guid);
                    var newCoupon = new Coupons.DAL.Coupons
                    {
                        Name = couponRequest.Name,
                        CompanyId=couponRequest.CompanyId,
                        Start_Date= couponRequest.Start_date,
                        End_Date=couponRequest.End_date,
                        Amount=couponRequest.Amount,
                        Type=couponRequest.Type,
                        Message= couponRequest.Message,
                        Price = couponRequest.Price,
                        Image = couponRequest.Image
                    };
                    couponRepository.Create(newCoupon);
                    return CreateResponseMessage(newCoupon);
                }
                else
                {//Update
                    logger.Info("Guid: \"{0}\" Start Updating Coupon", guid);

                    couponToUpdate.Name = couponRequest.Name;
                    couponToUpdate.CompanyId = couponRequest.CompanyId;
                    couponToUpdate.Start_Date = couponRequest.Start_date;
                    couponToUpdate.End_Date = couponRequest.End_date;
                    couponToUpdate.Amount = couponRequest.Amount;
                    couponToUpdate.Type = couponRequest.Type;
                    couponToUpdate.Message = couponRequest.Message;
                    couponToUpdate.Price = couponRequest.Price;
                    couponToUpdate.Image = couponRequest.Image;

                    couponRepository.SaveChanges();
                    return CreateResponseMessage(couponToUpdate);
                }
            }
            catch (Exception ex)
            {
                logger.Error("Guid: \"{0}\" General Error: {1}", guid, ex);
                return CreateGeneralResultMessage(ex.ToString(), false, ApiStatusCodes.InternalServerError);
            }
        }

        [HttpGet]
        public HttpResponseMessage Get(int couponId)
        {
            string apiName = "Get(int couponId{" + couponId + "})";
            var guid = Guid.NewGuid();
            logger.Info("Guid: \"{0}\" api/{1}/{2} was invoked", guid, controllerName, apiName);

            ICouponRepository couponRepository = new CouponRepository();

            try
            {
                var response=couponRepository.Get(couponId);
                return CreateResponseMessage(response);
            }
            catch (Exception ex)
            {
                logger.Error("Guid: \"{0}\" General Error: {1}", guid, ex);
                return CreateGeneralResultMessage(ex.ToString(), false, ApiStatusCodes.InternalServerError);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetAll()
        {
            string apiName = "GetAll()";
            var guid = Guid.NewGuid();
            logger.Info("Guid: \"{0}\" api/{1}/{2} was invoked", guid, controllerName, apiName);

            ICouponRepository couponRepository = new CouponRepository();

            try
            {
                var response = couponRepository.GetCoupons();
                return CreateResponseMessage(response);
            }
            catch (Exception ex)
            {
                logger.Error("Guid: \"{0}\" General Error: {1}", guid, ex);
                return CreateGeneralResultMessage(ex.ToString(), false, ApiStatusCodes.InternalServerError);
            }
        }

        [HttpDelete]
        public HttpResponseMessage Delete(long couponId)
        {
            string apiName = "Delete(long couponId={" + couponId + "})";
            var guid = Guid.NewGuid();
            logger.Info("Guid: \"{0}\" api/{1}/{2} was invoked", guid, controllerName, apiName);

            ICouponRepository couponRepository = new CouponRepository();
            IUserCouponsRepository userCouponsRepository = new UserCouponsRepository();
            try
            {
                //TODO: add Transaction
                userCouponsRepository.RemoveAllByCoupon(couponId);
                couponRepository.Delete(couponId);

                logger.
                    Info("Guid: \"{0}\" api/{1}/{2}, Message:{3} CompanyId={4}",
                    guid, controllerName, apiName, "Company was deleted", couponId);
                return CreateGeneralResultMessage("Success", true);
            }
            catch (Exception ex)
            {
                logger.Error("Guid: \"{0}\" General Error: {1}", guid, ex);
                return CreateGeneralResultMessage(ex.ToString(), false, ApiStatusCodes.InternalServerError);
            }
        }

        [HttpDelete]
        public HttpResponseMessage tester()
        {
            using (CouponsEntities dbContext = new CouponsEntities())
            {
                var dataSet = (from c in dbContext.Coupons
                           where c.CompanyId == 1
                           join co in dbContext.Companies
                           on c.CompanyId equals co.Id
                           select new
                           {
                               co.Name,
                               c.CompanyId

                           }).ToList();
                return CreateResponseMessage(dataSet);
               // var user = dbContext.Users.FirstOrDefault(u => u.Id == 1);


                /*
                var data = dbContext.Coupons
                    .Join(
                        dbContext.Users.Where(u => u.Id == 1),
                        uu => uu.Id,
                        c => c.CompanyId,
                        (uu, c) => uu
                    ).ToList();

                var a = 1;*/
            }

                return null;
        }
    }
}
