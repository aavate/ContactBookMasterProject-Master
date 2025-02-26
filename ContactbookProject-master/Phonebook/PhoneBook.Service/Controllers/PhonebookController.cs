﻿namespace PhoneBook.Service.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using Phonebook.Common;
    using Phonebook.Database;
    using PhoneBook.Service.Filters;

    /// <summary>
    /// Phonebook controller to server phonebook service.
    /// </summary>
    [CustomAuthentication]
    [RoutePrefix("api/phonebook")]
    public class PhonebookController : ApiController
    {
        /// <summary>
        /// The phonebook repository
        /// </summary>
        private IPhonebookRepository phonebookRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="PhonebookController"/> class.
        /// </summary>
        /// <param name="phonebookRepository">The phonebook repository.</param>
        public PhonebookController(IPhonebookRepository phonebookRepository)
        {
            this.phonebookRepository = phonebookRepository;
        }

        /// <summary>
        /// Gets all the phonebook records.
        /// </summary>
        /// <returns>The <see cref="HttpResponseMessage"/></returns>
        [HttpGet]
        [Route("getall")]
        public HttpResponseMessage Get()
        {
            try
            {
                var phonebook = this.phonebookRepository.GetPhonebookRecords();

                return Request.CreateResponse<List<Phonebook>>(HttpStatusCode.OK, phonebook);
            }
            catch (System.Exception ex)
            {
                //Log exception by using log4net.
                return InternalServerErrorMessage();
            }
        }

        /// <summary>
        /// Gets the phonebook record by specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>The <see cref="Phonebook"/></returns>
        [HttpGet]
        [Route("getbyid/{id}")]
        public HttpResponseMessage Get(int id)
        {
            try
            {
                var phonebook = this.phonebookRepository.GetPhonebookRecord(id);

                if (phonebook != null)
                {
                    return Request.CreateResponse<Phonebook>(HttpStatusCode.OK, phonebook);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound,"Resource not found.");
                }
            }
            catch (System.Exception ex)
            {
                //Log exception by using log4net.
                return InternalServerErrorMessage();
            }
        }

        /// <summary>
        /// Adds the specified phonebook record.
        /// </summary>
        /// <param name="phonebook">The phonebook.</param>
        /// <returns>The <see cref="HttpResponseMessage"/></returns>
        [HttpPost]
        [Route("add")]
        public HttpResponseMessage Post([FromBody]Phonebook phonebook)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var phonebookId = this.phonebookRepository.AddNewPhonebookRecord(phonebook);

                    return Request.CreateResponse(HttpStatusCode.OK, phonebookId);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                }
            }
            catch (Exception ex)
            {
                //Log exception by using log4net.
                return InternalServerErrorMessage();
            }
        }

        /// <summary>
        /// Updates the specified phonebook record.
        /// </summary>
        /// <param name="phonebook">The phonebook.</param>
        /// <returns>The <see cref="HttpResponseMessage"/></returns>
        [HttpPut]
        [Route("update")]
        public HttpResponseMessage Put([FromBody]Phonebook phonebook)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    this.phonebookRepository.UpdatePhonebookRecord(phonebook);

                    return Request.CreateResponse(HttpStatusCode.OK, true);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                }
            }
            catch (Exception ex)
            {
                //Log exception by using log4net.
                return InternalServerErrorMessage();
            }
        }

        /// <summary>
        /// Deletes the specified phonebook record.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete/{id}")]
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                if (id > 0)
                {
                    this.phonebookRepository.DeletePhonebookRecord(id);

                    return Request.CreateResponse(HttpStatusCode.OK, true);
                }
            }
            catch (Exception ex)
            {
                //Log exception by using log4net.
                return InternalServerErrorMessage();
            }
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Resource not found.");
        }

        /// <summary>
        /// Internal server error message.
        /// </summary>
        /// <returns>The <see cref="HttpResponseMessage"/></returns>
        private HttpResponseMessage InternalServerErrorMessage()
        {
            return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Internal Server Error.");
        }
    }
}
