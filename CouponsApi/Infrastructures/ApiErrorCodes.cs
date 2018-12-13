using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CouponsApi.Infrastructures
{
    public enum ApiStatusCodes
    {
        OK = 200,
        BadRequest = 400,
        Forbidden = 403,
        NotFound = 404,
        Conflict = 409,
        Unprocessable = 422,
        GeneralError = 500,
        InternalServerError = 500,
        NotImplemented = 10000,
        UserNotExists = 10001,
        EntitiesNotFound = 10003,
        TrainingNotFound = 10004,
        ExerciseNotFound = 10005,
        UserGymNotFound = 10006,
        InvalidEmail = 10007,
        NoCouponLeft = 10008,
        CompanyNotExists = 10009
    }
}