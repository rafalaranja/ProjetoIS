using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Results;
using System.Web.Mvc;
using System.Xml;
using SOMIODMiddleware.Helper;
using SOMIODMiddleware.Models;

namespace SOMIODMiddleware.Controllers
{
    public class ApplicationController : Controller
    {
        private readonly ContainerController containerController = new ContainerController();
        private readonly ControllerHelper controllerHelper = new ControllerHelper();
        string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["SOMIODMiddleware.Properties.Settings.ConnStr"].ConnectionString;

        // Obtém todas as aplicações
        public List<Application> GetAllApplications()
        {
            return DataHelper.GetDataFromDatabase<Application>(
                "SELECT * FROM Application ORDER BY Id",
                null
            );
        }

        // Obtém uma aplicação pelo nome
        public List<Application> GetApplicationByName(string name)
        {
            return DataHelper.GetDataFromDatabase<Application>(
                "SELECT * FROM Application WHERE Name = @name",
                new Application { name = name }
            );
        }

        // Adiciona uma nova aplicação
        public string PostApplications()
        {
            // Extrai o elemento "name" do XML
            XmlNode nodeApplication = controllerHelper.BuildXmlNodeFromRequest("/application");
            string name = nodeApplication["name"].InnerText;

            // Verifica se já existe uma aplicação com o mesmo nome
            var existingApplications = GetApplicationByName(name);
            if (existingApplications.Count > 0)
            {
                return "An application with this name already exists.";
            }

            // Cria a aplicação
            int newAppId = CreateApplication(name);
            return $"Application created successfully with ID: {newAppId}";
        }

        // Atualiza o nome de uma aplicação
        public string PutApplication(string applicationName, int id)
        {
            DataHelper.TransactWithDatabase<Application>(
                "UPDATE Application SET Name = @name WHERE Id = @id",
                new Application { name = applicationName, id = id }
            );
            return "Application name updated successfully.";
        }

        // Cria uma nova aplicação na base de dados
        public int CreateApplication(string applicationName)
        {
            return DataHelper.TransactWithDatabase<Application>(
                "INSERT INTO Application(name) OUTPUT INSERTED.ID VALUES(@name)",
                new Application { name = applicationName }
            );
        }

        // Elimina uma aplicação e os seus dados associados
        public string DeleteApplication(int id)
        {
            // TODO: Eliminar os dados relacionados à aplicação

            //int containerID = containerController.GetContainerIdByParentId(id);
            //if (containerID > 0)
            //{
            //    //Elimina o container e as dependências relacionadas
            //    DataHelper.TransactWithDatabase<Container>(
            //        "DELETE FROM Container WHERE Container.Id = @id",
            //        new Container { id = containerID }
            //    );
            //    DataHelper.TransactWithDatabase<Notification>(
            //        "DELETE FROM Notification WHERE Notification.Parent = @parent",
            //        new Notification { parent = containerID }
            //    );
            //    DataHelper.TransactWithDatabase<Record>(
            //        "DELETE FROM Record WHERE DataRecord.Parent = @parent",
            //        new Record { parent = containerID }
            //    );
            //}

            // Elimina a aplicação
            DataHelper.TransactWithDatabase<Application>(
                "DELETE FROM Application WHERE Application.Id = @id",
                new Application { id = id }
            );

            return $"Application ID {id} deleted successfully.";
        }

        // Obtém o ID de uma aplicação pelo nome
        public int GetApplicationIdByName(string applicationName)
        {
            // Valida se a aplicação existe
            var applicationList = GetApplicationByName(applicationName);
            if (applicationList.Count == 0)
            {
                return 0; // Retorna 0 se a aplicação não existir
            }

            return applicationList[0]?.id ?? 0; // Retorna o ID se existir
        }

        // Verifica se o nome da aplicação está disponível
        public bool IsApplicationNameAvailable(string applicationName)
        {
            int applicationId = GetApplicationIdByName(applicationName);
            return applicationId == 0;
        }

    }
}
