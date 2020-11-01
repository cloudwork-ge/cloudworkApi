using System;
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
    public class ProjectController : MainController
    {
        private readonly PKG_PROJECT _pkg_project = new PKG_PROJECT();
        public ProjectController() {
        //    _pkg_users = new pkg_USERS;
        }

        [Authorization]
        [HttpPost]
        public JsonDocument GetCategories([FromBody] Grid grid)
        {
            //grid.Criteria = string.Format("WHERE Id = {0}", authUser.ID);
            grid.dsViewName = "V_PROJECT_CATEGORIES";
            return Success(grid.GetData<dsProjectCategory>());
        }
        [Authorization]
        [HttpPost]
        public JsonDocument GetProjects([FromBody] Grid grid)
        {
            //grid.Criteria = string.Format("WHERE Id = {0}", authUser.ID);
            grid.dsViewName = "V_PROJECTS";
            return Success(grid.GetData<Project>());
        }
        [Authorization]
        [HttpPost]
        public JsonDocument AddProject([FromBody] Project project)
        {
            try
            {
                _pkg_project.AddProject(authUser.ID, project);
            }
            catch(Exception ex)
            {
                return throwError(ex.Message, -1);
            }
            return Success();
        }
    }
}