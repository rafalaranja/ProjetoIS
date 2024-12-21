using SOMIODMiddleware.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Xml;
using SOMIODMiddleware.Controllers;
using SOMIODMiddleware.Models;
using Microsoft.AspNet.SignalR;
using static System.Net.Mime.MediaTypeNames;
using System.ComponentModel;
using Microsoft.AspNetCore.Http;



namespace SOMIODMiddleware.Controllers
{
    public class SOMIODController : ApiController
    {
        private readonly ApplicationController applicationController = new ApplicationController();
        private readonly ContainerController containerController = new ContainerController();
        private readonly RecordController recordController = new RecordController();
        private readonly NotificationController notificationController = new NotificationController();
        private readonly ControllerHelper controllerHelper = new ControllerHelper();
        private readonly ConnHelper connHelper = new ConnHelper();
        private Mosquitto mosquitto = new Mosquitto();


        #region GET Operations

        //GET ALL APPLICATIONS + LOCATE
        [Route("api/somiod")]
        [HttpGet]
        public IHttpActionResult GetAllApplications()
        {
            System.Net.Http.Headers.HttpRequestHeaders headers = this.Request.Headers;

            if (headers.Contains("somiod-locate"))
            {  //OK
                var valor = headers.GetValues("somiod-locate").First();  //build error - not OK
                switch (valor)
                {
                    case "application":
                        var applications = applicationController.GetAllApplicationNames();
                        return Ok(applications);

                    case "container":
                        var containers = containerController.GetAllContainersNames();
                        return Ok(containers);

                    case "record":
                        var records = recordController.GetAllRecordsNames();
                        return Ok(records);

                    case "notification":
                        var notifications = notificationController.GetAllNotificationsNames();
                        return Ok(notifications);

                    default:
                        return null;

                }
            }
            else
            {
                var applications = applicationController.GetAllApplications();
                return Ok(applications);
            }
        }


        //GET APLICATION + LOCATE
        [Route("api/somiod/{applicationName}")]
        [HttpGet]
        public IHttpActionResult GetApplication(string applicationName)
        {
            System.Net.Http.Headers.HttpRequestHeaders headers = this.Request.Headers;

            //ir buscar o ID da aplicação através do nome providenciado
            int applicationId = applicationController.GetApplicationIdByName(applicationName);

            //return Ok(applicationId);

            if (applicationId == 0)
                return BadRequest("Application does not exist.");

            if (headers.Contains("somiod-locate"))
            {
                var valor = headers.GetValues("somiod-locate").First();
                switch (valor)
                {
                    case "container":
                        var containers = containerController.GetContainersByApplicationId(applicationId);
                        return Ok(containers);

                    case "record":
                        var records = recordController.GetRecordsByApplicationId(applicationId);
                        return Ok(records);

                    case "notification":
                        var notifications = notificationController.GetNotificationsByApplicationId(applicationId);
                        return Ok(notifications);

                    default:
                        return null;

                }
            }
            else
            {
                var properties = applicationController.GetApplicationPropertiesByName(applicationName); // vai buscar as propriedades da aplicação especificada
                return Ok(properties);
            }
        }


        //GET CONTAINER + LOCATE
        [Route("api/somiod/{applicationName}/{containerName}")]
        [HttpGet]
        public IHttpActionResult GetContainer(string applicationName, string containerName)
        {
            System.Net.Http.Headers.HttpRequestHeaders headers = this.Request.Headers;

            //ir buscar o ID da aplicação através do nome providenciado
            int applicationId = applicationController.GetApplicationIdByName(applicationName);
            //ir buscar o ID do container através do nome providenciado
            int containerId = containerController.GetContainerIdByName(containerName);

            if (applicationId == 0)
                return BadRequest("Application does not exist.");

            if (containerId == 0)
                return BadRequest("Container does not exist.");

            if (headers.Contains("somiod-locate"))
            {
                var valor = headers.GetValues("somiod-locate").First();
                switch (valor)
                {
                    case "record":
                        var records = recordController.GetRecordsByContainerId(containerId);
                        return Ok(records);

                    case "notification":
                        var notifications = notificationController.GetNotificationsByContainerId(containerId);
                        return Ok(notifications);

                    default:
                        return null;

                }
            }
            else
            {
                var properties = containerController.GetContainerByName(containerName); // vai buscar as propriedades da record especificada
                return Ok(properties);
            }
        }


        //GET RECORD
        [Route("api/somiod/{applicationName}/{containerName}/record/{recordName}")]
        [HttpGet]
        public IHttpActionResult GetRecord(string applicationName, string containerName,string recordName)
        {
            System.Net.Http.Headers.HttpRequestHeaders headers = this.Request.Headers;

            //ir buscar o ID da aplicação através do nome providenciado
            int applicationId = applicationController.GetApplicationIdByName(applicationName);
            //ir buscar o ID do container através do nome providenciado
            int containerId = containerController.GetContainerIdByName(containerName);
            //ir buscar o ID do container através do nome providenciado
            int recordId = recordController.GetRecordIdByName(recordName);

            if (applicationId == 0)
                return BadRequest("Application does not exist.");

            if (containerId == 0)
                return BadRequest("Container does not exist.");

            if (recordId == 0)
                return BadRequest("Record does not exist.");

            var properties = recordController.GetRecordByName(recordName); // vai buscar as propriedades da record especificada
            return Ok(properties);
        }

        //GET NOTIFICATION
        [Route("api/somiod/{applicationName}/{containerName}/notif/{notificationName}")]
        [HttpGet]
        public IHttpActionResult GetNotification(string applicationName, string containerName, string notificationName)
        {
            System.Net.Http.Headers.HttpRequestHeaders headers = this.Request.Headers;

            //ir buscar o ID da aplicação através do nome providenciado
            int applicationId = applicationController.GetApplicationIdByName(applicationName);
            //ir buscar o ID do container através do nome providenciado
            int containerId = containerController.GetContainerIdByName(containerName);
            //ir buscar o ID do container através do nome providenciado
            int recordId = notificationController.GetNotificationIdByName(notificationName);

            if (applicationId == 0)
                return BadRequest("Application does not exist.");

            if (containerId == 0)
                return BadRequest("Container does not exist.");

            if (recordId == 0)
                return BadRequest("Record does not exist.");

            var properties = notificationController.GetNotificationByName(notificationName); // vai buscar as propriedades da record especificada
            return Ok(properties);
        }


        #endregion

        #region API Applications

        [Route("api/somiod")]
        [HttpPost]
        public IHttpActionResult PostApplication() //caso o nome ja exista, dar outro nome (date time sque) e devolver o nome da app criada, NAO o id
        {
            XmlNode nodeApplication = controllerHelper.BuildXmlNodeFromRequest("Application");
            if (nodeApplication == null || !nodeApplication.HasChildNodes)
            {
                return BadRequest("Empty or invalid request body.");
            }
            XmlNode nameNode = nodeApplication["name"];
            if (nameNode == null || string.IsNullOrWhiteSpace(nameNode.InnerText))
            {
                return BadRequest("Application name is required.");
            }
            string applicationName = nameNode.InnerText;

            if (!applicationController.IsApplicationNameAvailable(applicationName))
            {
                //adicionar datetime
                applicationName = applicationName + DateTime.Now.ToString("yyyyMMddHHmmss");
            }
            applicationController.CreateApplication(applicationName);

            return Ok($"Application created successfully with name: {applicationName}");
        }

        [Route("api/somiod/{applicationName}")]
        [HttpPut]
        public IHttpActionResult PutApplication(string applicationName)
        {
            int applicationId = applicationController.GetApplicationIdByName(applicationName);
            XmlNode nodeApplication = controllerHelper.BuildXmlNodeFromRequest("Application");
            if (nodeApplication == null || !nodeApplication.HasChildNodes)
            {
                return BadRequest("Empty or invalid request body.");
            }
            XmlNode nameNode = nodeApplication["name"];
            if (nameNode == null || string.IsNullOrWhiteSpace(nameNode.InnerText))
            {
                return BadRequest("Application name is required.");
            }
            if (!applicationController.IsApplicationNameAvailable(nameNode.InnerText))
                return BadRequest("Application name is already taken.");

            applicationController.PutApplication(nameNode.InnerText, applicationId);
            return Ok("Application name updated successfully.");
        }

        [Route("api/somiod/{applicationName}")]
        [HttpDelete]
        public IHttpActionResult DeleteApplication(string applicationName)
        {
            int applicationId = applicationController.GetApplicationIdByName(applicationName);
            //return Ok(applicationId);
            if (applicationId == 0)
                return BadRequest("Application does not exist.");

            applicationController.DeleteApplication(applicationId);
            return Ok("Application deleted successfully.");
        }



        #endregion

        #region API Containers

        [Route("api/somiod/{applicationName}")]
        [HttpPost]
        public IHttpActionResult PostContainer(string applicationName)
        {
            int applicationId = applicationController.GetApplicationIdByName(applicationName);
            if (applicationId == 0)
                return BadRequest("Application does not exist.");

            XmlNode nodeContainer = controllerHelper.BuildXmlNodeFromRequest("Container");
            if (!nodeContainer.HasChildNodes)
                return BadRequest("Empty request body.");

            string containerName = nodeContainer["name"].InnerText;
            if (string.IsNullOrWhiteSpace(containerName))
                return BadRequest("Container name is required.");
            if (containerController.GetContainerByNameAndParentId(containerName, applicationId) > 0)
            {
                containerName = containerName + DateTime.Now.ToString("yyyyMMddHHmmss");
            }

            containerController.CreateContainer(containerName, applicationId);
            return Ok($"Container created successfully with ID: {containerName}");
        }

        [Route("api/somiod/{applicationName}/{containerName}")]
        [HttpPut]
        public IHttpActionResult PutContainer(string applicationName, string containerName)
        {
            int applicationId = applicationController.GetApplicationIdByName(applicationName);

            if (applicationId == 0)
                return BadRequest("Application does not exist.");

            int containerId = containerController.GetContainerByNameAndParentId(containerName, applicationId);

            if (containerId == 0)
                return BadRequest("Container does not exist.");

            XmlNode nodeContainer = controllerHelper.BuildXmlNodeFromRequest("Container");

            if (!nodeContainer.HasChildNodes)
                return BadRequest("Empty request b ody.");

            string newContainerName = nodeContainer["name"].InnerText;

            if (string.IsNullOrWhiteSpace(newContainerName))
                return BadRequest("Container name is required.");

            if (containerController.GetContainerByNameAndParentId(newContainerName, applicationId) > 0)
            {
                return BadRequest("Container name is already taken.");
            }
            containerController.PutContainer(newContainerName, containerId);
            return Ok("Container name updated successfully.");
        }

        [Route("api/somiod/{applicationName}/{containerName}")]
        [HttpDelete]
        public IHttpActionResult DeleteContainer(string applicationName, string containerName)
        {
            int applicationId = applicationController.GetApplicationIdByName(applicationName);
            if (applicationId == 0)
                return BadRequest("Application does not exist.");

            int containerId = containerController.GetContainerByNameAndParentId(containerName, applicationId);
            if (containerId == 0)
                return BadRequest("Container does not exist.");

            containerController.DeleteContainer(containerId);
            return Ok("Container deleted successfully.");
        }


        #endregion

        #region API Records

        [Route("api/somiod/{applicationName}/{containerName}/record")]
        [HttpPost]
        public IHttpActionResult PostRecord(string applicationName, string containerName)
        {
            int applicationId = applicationController.GetApplicationIdByName(applicationName);
            if (applicationId == 0)
                return BadRequest("Application does not exist.");

            int containerId = containerController.GetContainerByNameAndParentId(containerName, applicationId);
            if (containerId == 0)
                return BadRequest("Container does not exist.");

            XmlNode nodeRecord = controllerHelper.BuildXmlNodeFromRequest("Record");
            if (!nodeRecord.HasChildNodes)
                return BadRequest("Empty request body.");

            string recordName = nodeRecord["content"].InnerText;
            if (string.IsNullOrWhiteSpace(recordName))
                return BadRequest("Record content is required.");
            if (recordController.GetRecordByContentAndParentId(recordName, containerId) > 0)
            {
                recordName = recordName + DateTime.Now.ToString("yyyyMMddHHmmss");
            }
            recordController.CreateRecord(recordName, containerId);
            connHelper.EmitMessageToTopic($"{applicationName}/{containerName}/creation", recordName);

            return Ok($"Record created successfully with name: {recordName}");
        }

        [Route("api/somiod/{applicationName}/{containerName}/record/{recordName}")]
        [HttpDelete]
        public IHttpActionResult DeleteRecord(string applicationName, string containerName, string recordName)
        {
            int applicationId = applicationController.GetApplicationIdByName(applicationName);
            if (applicationId == 0)
                return BadRequest("Application does not exist.");

            int containerId = containerController.GetContainerByNameAndParentId(containerName, applicationId);
            if (containerId == 0)
                return BadRequest("Container does not exist.");

            int recordId = recordController.GetRecordByContentAndParentId(recordName, containerId);
            if (recordId == 0)
                return BadRequest("Record does not exist.");

            recordController.DeleteRecord(recordId);
            connHelper.EmitMessageToTopic($"{applicationName}/{containerName}/deletion", $"Deleted record name: {recordName}");

            return Ok("Record deleted successfully.");
        }


        #endregion

        #region API Notifications

        [Route("api/somiod/{applicationName}/{containerName}/notification")]
        [HttpPost]
        public IHttpActionResult PostNotification(string applicationName, string containerName)
        {
            int applicationId = applicationController.GetApplicationIdByName(applicationName);
            if (applicationId == 0)
                return BadRequest("Application does not exist.");

            int containerId = containerController.GetContainerByNameAndParentId(containerName, applicationId);
            if (containerId == 0)
                return BadRequest("Container does not exist.");

            XmlNode nodeNotification = controllerHelper.BuildXmlNodeFromRequest("Notification");
            if (nodeNotification == null)
            {
                return BadRequest("Empty request body.");
            }

            string notificationName = nodeNotification["name"].InnerText;
            if (string.IsNullOrWhiteSpace(notificationName))
            {
                return BadRequest("Notification name is required.");
            }

            if (notificationController.GetNotificationByNameAndParentId(notificationName, containerId) > 0)
            {
                notificationName = notificationName + DateTime.Now.ToString("yyyyMMddHHmmss");
            }

            string notificationEvent = nodeNotification["event"].InnerText;

            if (string.IsNullOrWhiteSpace(notificationEvent))
            {
                return BadRequest("Notification event is required.");
            }
            string notificationEndpoint = $"{applicationName}/{containerName}/{notificationEvent}";
            string name = "";
            if((notificationEvent == "creation" || notificationEvent == "deletion"))
            {
                int id = notificationController.CreateNotification(notificationName, containerId, notificationEvent, notificationEndpoint);
                name = notificationController.GetNotificationNameById(id);
            }
            else if(notificationEvent == "both")
            {
                int firstNot = notificationController.CreateNotification(notificationName, containerId, "creation", notificationEndpoint);
                int secNot = notificationController.CreateNotification(notificationName, containerId, "deletion", notificationEndpoint);

                string name1 = notificationController.GetNotificationNameById(firstNot);
                string name2 = notificationController.GetNotificationNameById(secNot);

                name = name1 + " and " + name2;
            }
            return Ok($"Notification created successfully with name: {name}");

        }

        [Route("api/somiod/{applicationName}/{containerName}/notification/{notificationName}")]
        [HttpDelete]
        public IHttpActionResult DeleteNotification(string applicationName, string containerName, string notificationName)
        {
            int applicationId = applicationController.GetApplicationIdByName(applicationName);
            if (applicationId == 0)
                return BadRequest("Application does not exist.");
            int containerId = containerController.GetContainerByNameAndParentId(containerName, applicationId);
            if (containerId == 0)
                return BadRequest("Container does not exist.");
            int notificationId = notificationController.GetNotificationByNameAndParentId(notificationName, containerId);
            if (notificationId == 0)
                return BadRequest("Notification does not exist.");
            notificationController.DeleteNotification(notificationId);
            return Ok("Notification deleted successfully.");
        }

        #endregion
    }

}
