﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using cloudworkApi.StoredProcedures;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using cloudworkApi.DataManagers;
using cloudworkApi.Attributes;
using cloudworkApi.Models;
using System.Text.Json;
using System.Net.Http;
using System.Runtime.Caching;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Collections.Immutable;
using System.Security.Policy;
using System.Net;
using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;
using System.Reflection.Metadata;
using System.Web;
using cloudworkApi.Common;
using cloudworkApi.Services;
using cloudworkApi.Models.dsModels;

namespace cloudworkApi.Controllers
{
    public class ProfileController : MainController
    {
        [Authorization]
        public JsonDocument GetUserProfile([FromBody] Grid grid)
        {
            grid.dsViewName = "V_USERS";

            if (grid.CustomParams != null && grid.CustomParams.Count > 0)
            {
                var profileUserId = grid.CustomParams != null ? grid.CustomParams.Find(x => x.FieldName == "ProfileUserID") : null;
                if (profileUserId != null && Convert.ToInt32(profileUserId.FilterValue) > 0)
                {
                    grid.Criteria = string.Format("WHERE Id = {0}", profileUserId.FilterValue);
                    return Success(grid.GetData<dsUserProfile>());
                }
            }

            grid.Criteria = string.Format("WHERE Id = {0}", authUser.ID);

            return Success(grid.GetData<dsUserProfile>());

        }
        [Authorization]
        public JsonDocument ChangeProfile(dsUserProfile user)
        {
            var changed = new PKG_PROFILE().ChangeProfile(authUser.ID, user.fullname, user.phone,user.workType, user.description, user.bankNumber, user.tin);
            if (changed)
            {
                AuthUser newAuthUser = authUser;
                newAuthUser.fullName = user.fullname;
                newAuthUser.phone = user.phone;
                newAuthUser.tin = user.tin;
                new TokenManager().setToken(authUser.token, newAuthUser);
                return Success();
            }
            else
                return throwError("პროფილის დელატები ვერ შეივალა");
            return Success();

        }
        [Authorization]
        public JsonDocument ChangePassword(UserPasswords user)
        {
            var changed = new PKG_PROFILE().ChangePassword(authUser.ID, user.oldPassword, user.newPassword, user.confirmNewPassword);
            var status = string.Empty;
            if (changed)
                return Success();
            else
                return throwError("პაროლი ვერ შეიცვალა. გთხოვთ სწორად შეიყვანოთ არსებული პაროლი");
        }
        [HttpPost]
        public JsonDocument RecoverPassword([FromBody] string email)
        {
            string randomStr = "";
            var letters = "Aa1Bb2Cc3Dd4Ee5Ff6Gg7Hh8Ii9Jj%Kk#Ll$Mm!Nn^Oo*Pp&Qq1Rr4Ss5Tt2Uu0Vv%Ww!Xx@Yy!Zz";
            for (var i = 0; i <= 8; i++)
            {
                var randomNum = new Random().Next(0, letters.Length - 1);
                randomStr += letters[randomNum];
            }

            if (randomStr.Length >= 8)
            {
                bool succeed = new PKG_PROFILE().RecoverPassword(email, randomStr);
                if (succeed)
                {
                    new EmailService().SendEmail(email, "პაროლის აღდგენა", "თქვენ მიიღეთ დროებითი პაროლი, რომლის გამოყენებითაც შეგიძლიათ შეხვიდეთ და შემდეგ დააყენოთ თქვენთვის სასურველი პაროლი. <br /><br /><br /><b>" + randomStr + "</b><br /><br /><a href='http://crm.ants.ge/Profile/changePassword'>შეცვალე პაროლი</a>");
                    return Success("პაროლი გამოგზავნილია თქვენს ელ.ფოსტაზე");
                }
                else
                    return throwError("შეყვანილი ელ.ფოსტა არ მოიძებნა");
            }
            return throwError();
        }
    }
}