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



namespace SOMIODMiddleware.Controllers
{
    public class SOMIODController : ApiController
    {
        private readonly ApplicationController applicationController = new ApplicationController();
        private readonly ContainerController containerController = new ContainerController();
        private readonly RecordController recordController = new RecordController();
        private readonly NotificationController notificationController = new NotificationController();
        private readonly ControllerHelper utilsController = new ControllerHelper();
        private readonly ConnHelper connHelper = new ConnHelper();

        #region API Applications

        [Route("api/somiod/applications")]
        [HttpGet]
        public IHttpActionResult GetAllApplications()
        {
            var applications = applicationController.GetAllApplications();
            return Ok(applications);
        }

        [Route("api/somiod/applications")]
        [HttpPost]
        public IHttpActionResult PostApplication()
        {
            XmlNode nodeApplication = utilsController.BuildXmlNodeFromRequest("Application");
            if (!nodeApplication.HasChildNodes)
                return BadRequest("Empty request body.");

            string applicationName = nodeApplication["name"].InnerText;
            if (string.IsNullOrWhiteSpace(applicationName))
                return BadRequest("Application name is required.");

            if (!applicationController.IsApplicationNameAvailable(applicationName))
                return BadRequest("Application name is already taken.");

            int appId = applicationController.CreateApplication(applicationName);
            return Ok($"Application created successfully with ID: {appId}");
        }

        [Route("api/somiod/applications/{application}")]
        [HttpDelete]
        public IHttpActionResult DeleteApplication(string application)
        {
            int applicationId = applicationController.GetApplicationIdByName(application);
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

        [Route("api/somiod/applications/{application}/containers")]
        [HttpPost]
        public IHttpActionResult PostContainer(string application)
        {
            int applicationId = applicationController.GetApplicationIdByName(application);
            if (applicationId == 0)
                return BadRequest("Application does not exist.");

            XmlNode nodeContainer = utilsController.BuildXmlNodeFromRequest("Container");
            if (!nodeContainer.HasChildNodes)
                return BadRequest("Empty request body.");

            string containerName = nodeContainer["name"].InnerText;
            if (string.IsNullOrWhiteSpace(containerName))
                return BadRequest("Container name is required.");

            int containerId = containerController.CreateContainer(containerName, applicationId);
            return Ok($"Container created successfully with ID: {containerId}");
        }

        [Route("api/somiod/applications/{application}/containers/{container}")]
        [HttpDelete]
        public IHttpActionResult DeleteContainer(string application, string container)
        {
            int applicationId = applicationController.GetApplicationIdByName(application);
            if (applicationId == 0)
                return BadRequest("Application does not exist.");

            int containerId = containerController.GetContainerByNameAndParentId(container, applicationId);
            if (containerId == 0)
                return BadRequest("Container does not exist.");

            containerController.DeleteContainer(containerId);
            return Ok("Container deleted successfully.");
        }

      
        #endregion

        #region API Records (Data)

        [Route("api/somiod/applications/{application}/containers/{container}/records")]
        [HttpPost]
        public IHttpActionResult PostRecord(string application, string container)
        {
            int applicationId = applicationController.GetApplicationIdByName(application);
            if (applicationId == 0)
                return BadRequest("Application does not exist.");

            int containerId = containerController.GetContainerByNameAndParentId(container, applicationId);
            if (containerId == 0)
                return BadRequest("Container does not exist.");

            XmlNode nodeRecord = utilsController.BuildXmlNodeFromRequest("Record");
            if (!nodeRecord.HasChildNodes)
                return BadRequest("Empty request body.");

            string recordContent = nodeRecord["content"].InnerText;
            if (string.IsNullOrWhiteSpace(recordContent))
                return BadRequest("Record content is required.");

            int recordId = recordController.CreateRecord(recordContent, containerId);
            connHelper.EmitMessageToTopic($"{application}/{container}/creation", recordContent);

            return Ok($"Record created successfully with ID: {recordId}");
        }

        [Route("api/somiod/applications/{application}/containers/{container}/records/{recordId}")]
        [HttpDelete]
        public IHttpActionResult DeleteRecord(string application, string container, int recordId)
        {
            int applicationId = applicationController.GetApplicationIdByName(application);
            if (applicationId == 0)
                return BadRequest("Application does not exist.");

            int containerId = containerController.GetContainerByNameAndParentId(container, applicationId);
            if (containerId == 0)
                return BadRequest("Container does not exist.");

            recordController.DeleteRecord(recordId);
            connHelper.EmitMessageToTopic($"{application}/{container}/deletion", $"Deleted record ID: {recordId}");

            return Ok("Record deleted successfully.");
        }

        private IHttpActionResult BadRequest(string v)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region API Notifications (Subscriptions)

        [Route("api/somiod/applications/{application}/containers/{container}/notifications")]
        [HttpPost]
        public IHttpActionResult PostNotification(string application, string container)
        {
            int applicationId = applicationController.GetApplicationIdByName(application);
            if (applicationId == 0)
                return BadRequest("Application does not exist.");

            int containerId = containerController.GetContainerByNameAndParentId(container, applicationId);
            if (containerId == 0)
                return BadRequest("Container does not exist.");

            XmlNode nodeNotification = utilsController.BuildXmlNodeFromRequest("Notification");
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
