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
using cloudworkApi.Models.tableModels;

namespace cloudworkApi.Controllers
{
    public class ProjectController : MainController
    {
        private readonly PKG_PROJECT _pkg_project;
        public ProjectController() {
            _pkg_project = new PKG_PROJECT();
        }

        [Authorization]
        [HttpPost]
        public JsonDocument GetCategories([FromBody] Grid grid)
        {
            //grid.Criteria = string.Format("WHERE Id = {0}", authUser.ID);
            //var x = _pkg_project.GetProjects();
            grid.dsViewName = "V_PROJECT_CATEGORIES";
            return Success(grid.GetData<dsProjectCategory>());
        }
        [HttpPost]
        public JsonDocument GetProjects([FromBody] Grid grid)
        {
            grid.dsViewName = "V_PROJECTS";
            grid.OrderBy = "ID DESC";
            grid.Criteria = string.Format("WHERE status = 0"); // მხოლოდ ღია პროექტები

            return Success(grid.GetData<Project>());
        }
        [HttpPost]
        [Authorization]
        public JsonDocument GetProjectDetails([FromBody] ProjectDetails project)
        {
            var s = _pkg_project.GetProject(project.ID, authUser.ID);
            return Success(s);
            //var grid = new Grid();
            //grid.Criteria = string.Format("WHERE Id = {0}", authUser.ID);
            //grid.Criteria = string.Format("WHERE ID = {0}", project.ID);
            //grid.dsViewName = "V_PROJECTS";
            //grid.OrderBy = "ID DESC";
            //return Success(grid.GetData<Project>());
            //return this.GetProjects(grid);
        }
        [HttpPost]
        [Authorization]
        public JsonDocument GetProjectBids([FromBody] JsonElement project)
        {
            var grid = new Grid();
            //grid.Criteria = string.Format("WHERE Id = {0}", authUser.ID);
            grid.dsViewName = "V_BIDS";
            grid.OrderBy = "ID DESC";
            grid.Criteria = string.Format("WHERE projectId = {0}", project.GetProperty("projectId"));
            return Success(grid.GetData<ProjectBids>());
        }
        [HttpPost]
        [Authorization]
        public JsonDocument GetMyProjects([FromBody] Grid grid)
        {
            //grid.Criteria = string.Format("WHERE Id = {0}", authUser.ID);

            grid.dsViewName = "V_PROJECTS_AND_BIDS";

            if (grid.CustomParams != null && grid.CustomParams.Count > 0)
            {
                var profileUserId = grid.CustomParams != null ? grid.CustomParams.Find(x => x.FieldName == "ProfileUserID") : null;
                if (profileUserId != null && Convert.ToInt32(profileUserId.FilterValue) > 0)
                {
                    grid.Criteria = string.Format("WHERE userID = {0}", profileUserId.FilterValue);
                    return Success(grid.GetData<dsUserProfile>());
                }
            }

            grid.Criteria = String.Format("WHERE (userID = {0} OR workerUserID = {0})", authUser.ID);
            grid.OrderBy = "ID DESC";
            return Success(grid.GetData<Project>());
        }
        [HttpPost]
        [Authorization]
        public JsonDocument AcceptBid([FromBody] ProjectBids bid)
        {
            _pkg_project.AcceptBid(bid.ID, authUser.ID);
            return Success();
            //var grid = new Grid();
            //grid.Criteria = string.Format("WHERE Id = {0}", authUser.ID);
            //grid.Criteria = string.Format("WHERE ID = {0}", project.ID);
            //grid.dsViewName = "V_PROJECTS";
            //grid.OrderBy = "ID DESC";
            //return Success(grid.GetData<Project>());
            //return this.GetProjects(grid);
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
        [Authorization]
        [HttpPost]
        public JsonDocument BidProject([FromBody] tbProjectBids bid)
        {
            bid.userID = authUser.ID;
            _pkg_project.BidProject(bid);
            return Success();
        }
        [Authorization]
        [HttpPost]
        public JsonDocument ProjectDoneFreelancer([FromBody] tbProjects project)
        {
            project.workerUserId = authUser.ID;

            if (project.ID > 0)
            {
               project = _pkg_project.ProjectDoneFreelancer(project);
                var ownerEmail = new PKG_USERS().getUserEmail(project.userId);
                var projectDetailsHtml = project.name + "<br />" + project.ID + "<br />" + project.description + "<br />" + project.category + "<br />" + project.type + "<br />" + project.budget;
                var email = new EmailService();
                email.SendEmail(ownerEmail, "პროექტის დასრულების მოთხოვნა - ID: " + project.ID, projectDetailsHtml);
                return Success();
            }
            else return throwError("პროექტი არ მოიძებნა");

        }
        [Authorization]
        [HttpPost]
        public JsonDocument ProjectDoneOwner([FromBody] tbProjects project)
        {
            project.userId = authUser.ID;
            if (project.ID > 0)
            {
                project = _pkg_project.ProjectDoneOwner(project);
                var email = new EmailService();
                var projectDetailsHtml = project.name + "<br />" + project.ID + "<br />" + project.description + "<br />" + project.category + "<br />" + project.type + "<br />" + project.budget;
                email.SendEmail("dima@ants.ge", "cloudwork.ge - პროექტი დასრულდა", projectDetailsHtml);
                return Success();
            }
            else return throwError("პროექტი არ მოიძებნა");
        }
    }
}