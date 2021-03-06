﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using CgiApiRework.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CgiApiRework.Controllers
{
    public class VacancyController : ControllerBase
    {
        [HttpGet]
        //[Authorize]
        public ArrayList DefaultAction()
        {
            return Vacancy.GetListVacancy();
        }

        [Route("api/vacancy/asc/{columnName}")]
        [HttpGet]
        public ArrayList GetVacancyListASC(string columnName)
        {
            return Vacancy.GetListVacancyFilterASC(columnName);
        }

        [Route("api/vacancy/desc/{columnName}")]
        [HttpGet]
        public ArrayList GetVacancyListDESC(string columnName)
        {
            return Vacancy.GetListVacancyFilterDESC(columnName);
        }

        [Route("api/vacancy/{id}")]
        [HttpGet]
        public ArrayList GetVacancyList(int id)
        {
            return Vacancy.GetListVacancy(id);
        }

        [Route("api/vacancy/responses")]
        [HttpGet]
        public ArrayList GetListRespondVacancyUser()
        {
            return Vacancy.GetListRespondVacancyUser();
        }

        //For employee
        [Route("api/vacancy/responses/employee/{userid}")]
        [HttpGet]
        public ArrayList GetListRespondVacancyUserForEmployee(string userID)
        {
            string roleOfUser = Models.User.GetUserRole(userID);
            if (roleOfUser == Enum.GetName(typeof(ListOfUsers), ListOfUsers.employee))
            {
                return Vacancy.GetListRespondVacancyUserForEmployee(userID);
            }
            else
            {
                return new ArrayList {"the role isnt a: " + Enum.GetName(typeof(ListOfUsers), ListOfUsers.employee) + "its a " + roleOfUser };
            }
        }

        //For employer
        [Route("api/vacancy/responses/employer/{userid}")]
        [HttpGet]
        public ArrayList GetListRespondVacancyUserForEmployer(string userID)
        {
            string roleOfUser = Models.User.GetUserRole(userID);
            if (roleOfUser == Enum.GetName(typeof(ListOfUsers), ListOfUsers.employer))
            {
                return Vacancy.GetListRespondVacancyUserForEmployer(userID);
            }
            else
            {
                return new ArrayList { "the role isnt a: " + Enum.GetName(typeof(ListOfUsers), ListOfUsers.employer) + " its a " + roleOfUser };
            }
        }

        // Krijgt een lijst van gereageerde werknemers met het aangegeven VacancyID en StatusID
        [Route("api/vacancy/{vacancyID}/responses/{statusid}")]
        [HttpGet]
        public ArrayList GetRespondVacancyUserListByVacancy(int vacancyID, int statusID)
        {
            return Vacancy.GetListRespondVacancyUser(vacancyID, statusID);
        }

        [Route("api/vacancy/responses/{userid}/{statusid}")]
        [HttpGet]
        public ArrayList GetRespondVacancyUserListByUser(string userID, int statusID)
        {
            return Vacancy.GetListRespondVacancyUser(userID, statusID);
        }

        [Route("api/vacancy/responses/{userid}")]
        [HttpGet]
        public ArrayList GetRespondVacancyUserListByUser(string userID)
        {
            return Vacancy.GetListRespondVacancyUser(userID);
        }

        [Route("api/vacancy/{id}/responses")]
        [HttpGet]
        public ArrayList GetRespondVacancyUserList(int id)
        {
            return Vacancy.GetListRespondVacancyUser(id);
        }

        // Voegt een vacature in de database met de class Vacancy
        [Route("api/vacancy/add")]
        [HttpPost]
        public HttpResponseMessage AddVacancy([FromBody]Vacancy vacancy)
        {
            HttpResponseMessage message = new HttpResponseMessage();
            if (vacancy != null)
            {
                message.Content = new StringContent(Vacancy.AddVacancy(vacancy));
            }
            else
            {
                message.Content = new StringContent("object is null, please check parameters");
                message.ReasonPhrase = "check the json format or the right parameters";
            }

            return message;
        }

        [Route("api/vacancy/addResponse")]
        [HttpPost]
        public HttpResponseMessage AddRespondVacancyUser([FromBody]RespondVacancyUser user)
        {
            if (Vacancy.AddRespondVacancyUser(user))
            {
                return new HttpResponseMessage(HttpStatusCode.Created);
            }
            else
            {
                return new HttpResponseMessage(HttpStatusCode.Conflict);
            }
        }


        [Route("api/vacancy/response/update")]
        [HttpPut]
        public void Put([FromBody]RespondVacancyUser user)
        {
            Vacancy.UpdateRespondVacancyUser(user);
        }

        [Route("api/vacancy/updatelist")]
        [HttpPut]
        public void Put([FromBody]List<RespondVacancyUser> userList)
        {
            Vacancy.UpdateRespondVacancyUser(userList);
        }
    }
}
