using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.UI.WebControls.WebParts;
using System.Xml;
using SOMIODMiddleware.Helper;
using SOMIODMiddleware.Models;

namespace SOMIODMiddleware.Controllers
{
    public class ContainerController : Controller
    {
        private readonly ControllerHelper controllerHelper = new ControllerHelper();
        string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["SOMIODMiddleware.Properties.Settings.ConnStr"].ConnectionString;


        // Obtém o nome de todas os Containers
        public List<string> GetAllContainersNames()
        {
            return DataHelper.GetDataFromDatabase<string>(
                "SELECT name FROM Container ORDER BY Id",
                null
            );
        }

        // Obtém todas os container
        public List<Container> GetAllContainers()
        {
            return DataHelper.GetDataFromDatabase<Container>(
                "SELECT name FROM Container ORDER BY Id",
                null
            );
        }

        // Gets all containers for a specific application
        public List<Container> GetContainersByApplicationId(int parent)
        {
            return DataHelper.GetDataFromDatabase<Container>(
                "SELECT name FROM Container WHERE parent = @parent",
                new Container{ parent = parent }
            );
        }


        // Gets a specific container by its ID
        public Container GetContainerById(int id)
        {
            var containers = DataHelper.GetDataFromDatabase<Container>(
                "SELECT * FROM Container WHERE Id = @id",
                new Container { id = id }
            );

            return containers.Count > 0 ? containers[0] : null;
        }
        public int GetContainerByNameAndParentId(String container, int applicationId)
        {
            List<Container> containerList = DataHelper.GetDataFromDatabase<Container>("SELECT * FROM Container WHERE Parent = @parent AND Name = @name", new Container { parent = applicationId, name = container });

            int containerById = 0;
            if (containerList.Count > 0)
            {
                containerById = containerList[0].id;
            }

            return containerById;
        }

        // Creates a new container
        public string PostContainer()
        {
            // Extract the container information from the XML request
            XmlNode nodeContainer = controllerHelper.BuildXmlNodeFromRequest("/container");
            string name = nodeContainer["name"].InnerText;
            int applicationId = int.Parse(nodeContainer["applicationId"].InnerText);

            // Check if the application exists
            ApplicationController applicationController = new ApplicationController();
            if (applicationController.GetApplicationByName(name).Count == 0)
            {
                return "The specified application does not exist.";
            }

            // Create the container
            int newContainerId = CreateContainer(name, applicationId);
            return $"Container created successfully with ID: {newContainerId}";
        }

        // Updates a container's name
        public string PutContainer(string containerName, int id)
        {
            DataHelper.TransactWithDatabase<Container>(
                "UPDATE Container SET Name = @name WHERE Id = @id",
                new Container { name = containerName, id = id }
            );
            return "Container name updated successfully.";
        }

        // Deletes a container and its related data
        public string DeleteContainer(int id)
        {
            // Check if the container exists
            var container = GetContainerById(id);
            if (container == null)
            {
                return $"Container ID {id} does not exist.";
            }

            // Delete related data (e.g., records and notifications)
            DataHelper.TransactWithDatabase<Record>(
                "DELETE FROM Record WHERE Parent = @parent",
                new Record { parent = id }
            );

            DataHelper.TransactWithDatabase<Notification>(
                "DELETE FROM Notification WHERE Parent = @parent",
                new Notification { parent = id }
            );

            // Delete the container itself
            DataHelper.TransactWithDatabase<Container>(
                "DELETE FROM Container WHERE Id = @id",
                new Container { id = id }
            );

            return $"Container ID {id} deleted successfully.";
        }

        // Creates a container in the database
        public int CreateContainer(string name, int applicationId)
        {
            return DataHelper.TransactWithDatabase<Container>(
                "INSERT INTO Container (Name, ApplicationId) OUTPUT INSERTED.ID VALUES (@name, @applicationId)",
                new Container { name = name, id = applicationId }
            );
        }

        // Gets the ID of a container by its parent (application ID)
        public int GetContainerIdByParentId(int parentId)
        {
            var containers = DataHelper.GetDataFromDatabase<Container>(
                "SELECT Id FROM Container WHERE ApplicationId = @applicationId",
                new Container { id = parentId }
            );

            return containers.Count > 0 ? containers[0].id : 0;
        }
       
    }
}
