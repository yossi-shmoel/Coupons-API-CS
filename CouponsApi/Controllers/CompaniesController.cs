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
using System.Web.Http.Cors;

namespace CouponsApi.Controllers
{
    [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
    public class CompaniesController : ApplicationBaseController
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        readonly string controllerName = "Companies";

        [HttpPost]
        public HttpResponseMessage CreateOrUpdate(CompanyRequest companyRequest)
        {
            string apiName = "CreateOrUpdate(CompanyRequest companyRequest=\n{" + companyRequest + "})";
            var guid = Guid.NewGuid();
            logger.Info("Guid: \"{0}\" api/{1}/{2} was invoked", guid, controllerName, apiName);

            ICompanyRepository companyRepository = new CompanyRepository();

            try
            {
                if (!ModelState.IsValidField("companyRequest.Email"))
                {
                    return CreateGeneralResultMessage("Invalid Email Address", false, ApiStatusCodes.InvalidEmail);
                }

                var companyToUpdate = companyRepository.GetByEmail(companyRequest.Email);

                if (companyToUpdate == null)
                {//Create
                    logger.Info("Guid: \"{0}\" Start Creating new Company", guid);
                    var newCompany = new Companies
                    {
                        Name = companyRequest.Name,
                        Email = companyRequest.Email
                    };
                    companyRepository.Create(newCompany);
                    return CreateResponseMessage(newCompany);
                }
                else
                {//Update
                    logger.Info("Guid: \"{0}\" Start Updating Company", guid);
                    companyToUpdate.Email = companyRequest.Email;
                    companyToUpdate.Name = companyRequest.Name;
                    companyRepository.SaveChanges();
                    return CreateResponseMessage(companyToUpdate);
                }
            }
            catch (Exception ex)
            {
                logger.Error("Guid: \"{0}\" General Error: {1}", guid, ex);
                return CreateGeneralResultMessage(ex.ToString(), false, ApiStatusCodes.InternalServerError);
            }
        }

        [HttpGet]
        public HttpResponseMessage Get(long id)
        {
            string apiName = "Get()";
            var guid = Guid.NewGuid();
            logger.Info("Guid: \"{0}\" api/{1}/{2} was invoked", guid, controllerName, apiName);

            ICompanyRepository companyRepository = new CompanyRepository();
            try
            {
                var company = companyRepository.Get(id);
                return CreateResponseMessage(company);
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

            ICompanyRepository companyRepository = new CompanyRepository();
            try
            {
                var response = companyRepository.GetCompanies();
                logger.
                    Info("Guid: \"{0}\" api/{1}/{2}\nResponse Data:\n{3}",
                    guid, controllerName, apiName, response.ToString());
                return CreateResponseMessage(response);
            }
            catch (Exception ex)
            {
                logger.Error("Guid: \"{0}\" General Error: {1}", guid, ex);
                return CreateGeneralResultMessage(ex.ToString(), false, ApiStatusCodes.InternalServerError);
            }
        }

        [HttpDelete]
        public HttpResponseMessage Delete(long companyId)
        {
            string apiName = "Delete(long companyId={"+companyId+"})";
            var guid = Guid.NewGuid();
            logger.Info("Guid: \"{0}\" api/{1}/{2} was invoked", guid, controllerName, apiName);

            ICompanyRepository companyRepository = new CompanyRepository();
            ICouponRepository couponRepository = new CouponRepository();
            try
            {
                var IsCompanyExsists = companyRepository.IsCompanyExsists(companyId);
                if(!IsCompanyExsists)
                {
                    logger.Error("Guid: \"{0}\"Company Not found", guid);
                    return CreateGeneralResultMessage("Company Not found", false, ApiStatusCodes.CompanyNotExists);
                }

                couponRepository.UpdateOnCompanyDeletion(companyId);
                companyRepository.Delete(companyId);

                logger.
                    Info("Guid: \"{0}\" api/{1}/{2}, Message:{3} CompanyId={4}",
                    guid, controllerName, apiName, "Company was deleted", companyId);
                return CreateGeneralResultMessage("Success", true);
            }
            catch (Exception ex)
            {
                logger.Error("Guid: \"{0}\" General Error: {1}", guid, ex);
                return CreateGeneralResultMessage(ex.ToString(), false, ApiStatusCodes.InternalServerError);
            }
        }
    }
}
