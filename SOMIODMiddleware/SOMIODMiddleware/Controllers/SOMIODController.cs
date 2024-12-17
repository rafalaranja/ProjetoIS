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

        #region API Applications

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
            if (!applicationController.IsApplicationNameAvailable(applicationName))
                return BadRequest("Application name is already taken.");
            applicationController.PutApplication(applicationName, applicationId);
            return Ok("Application name updated successfully.");
        }

        [Route("api/somiod/{applicationName}")]
        [HttpDelete]
        public IHttpActionResult DeleteApplication(string applicationName)
        {
            int applicationId = applicationController.GetApplicationIdByName(applicationName);
            if (applicationId == 0)
                return BadRequest("Application does not exist.");

            applicationController.DeleteApplication(applicationId);
            return Ok("Application deleted successfully.");
        }



        #endregion

        #region API Containers

        [Route("api/somiod/applications/{application}/containers")]
        [HttpGet]
        public IHttpActionResult GetContainersByApplication(string application)
        {
            int applicationId = applicationController.GetApplicationIdByName(application);
            if (applicationId == 0)
                return BadRequest("Application does not exist.");

            var containers = containerController.GetContainersByApplicationId(applicationId);
            return Ok(containers);
        }

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
                return BadRequest("Empty request body.");
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

        #region API Records (Data)

        [Route("api/somiod/{applicationName}/{containerName}/records")]
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

        [Route("api/somiod/{applicationName}/{containerName}/records/{recordId}")]
        [HttpDelete]
        public IHttpActionResult DeleteRecord(string applicationName, string containerName, int recordId)
        {
            int applicationId = applicationController.GetApplicationIdByName(applicationName);
            if (applicationId == 0)
                return BadRequest("Application does not exist.");

            int containerId = containerController.GetContainerByNameAndParentId(containerName, applicationId);
            if (containerId == 0)
                return BadRequest("Container does not exist.");

            recordController.DeleteRecord(recordId);
            connHelper.EmitMessageToTopic($"{applicationName}/{containerName}/deletion", $"Deleted record ID: {recordId}");

            return Ok("Record deleted successfully.");
        }


        #endregion

        #region API Notifications (Subscriptions)

        [Route("api/somiod/{applicationName}/{containerName}/notifications")]
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
            if (!nodeNotification.HasChildNodes)
                return BadRequest("Empty request body.");

            string notificationName = nodeNotification["name"].InnerText;
            string notificationEvent = nodeNotification["event"].InnerText;
            string endpoint = nodeNotification["endpoint"].InnerText;

            if (string.IsNullOrWhiteSpace(notificationName) || string.IsNullOrWhiteSpace(notificationEvent) || string.IsNullOrWhiteSpace(endpoint))
                return BadRequest("Notification name, event, and endpoint are required.");

            int notificationId = notificationController.CreateNotification(notificationName, containerId, notificationEvent, endpoint);
            return Ok($"Notification created successfully with ID: {notificationId}");
        }
        #endregion
    }

}
