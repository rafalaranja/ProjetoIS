using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SOMIODMiddleware.Helper;
using SOMIODMiddleware.Models;

namespace SOMIODMiddleware.Controllers
{
    public class NotificationController : Controller
    {

        string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["SOMIODMiddleware.Properties.Settings.ConnStr"].ConnectionString;


        // Obtém o nome de todas as Notifications
        public List<string> GetAllNotificationsNames()
        {
            return DataHelper.GetDataFromDatabase<string>(
                "SELECT name FROM Notification ORDER BY Id",
                null
            );
        }

        public List<String> GetNotificationsByApplicationId(int parent)
        {
            return DataHelper.GetDataFromDatabase<String>(
                $"SELECT Notification.name " +
                $"FROM Notification " +
                $"JOIN Container ON Notification.parent = Container.id " +
                $"WHERE Container.parent = {parent}", // Insere o valor diretamente
                 null

            );
        }


        // Obtém todas ãs notificacoes
        public List<Notification> GetAllNotifications()
        {
            return DataHelper.GetDataFromDatabase<Notification>(
                "SELECT name FROM Notification ORDER BY Id",
                null
            );
        }

        public List<Notification> GetNotificationByName(string name)
        {
            return DataHelper.GetDataFromDatabase<Notification>(
                "SELECT * FROM Notification WHERE Name = @name",
                new Notification { name = name }
            );
        }

        public string GetNotificationNameById(int id)
        {
            List<Notification> notificationList = DataHelper.GetDataFromDatabase<Notification>(
                "SELECT * FROM Notification WHERE Id = @id",
                new Notification { id = id }
            );
            if (notificationList.Count > 0)
            {
                return notificationList[0].name;
            }
            return "";
        }

        // Gets a specific notification by its name
        public int GetNotificationIdByName(string notificationName)
        {

            var record = GetNotificationByName(notificationName);
            if (record.Count == 0)
            {
                return 0; // Retorna 0 se a aplicação não existir
            }

            return record[0]?.id ?? 0; // Retorna o ID se existir
        }


        // Cria uma nova notificação
        public int CreateNotification(string name, int parent, string @event, string endpoint)
        {
            // Insere uma nova notificação e retorna o ID gerado
            int newlyInsertedNotificationId = DataHelper.TransactWithDatabase<Notification>(
                "INSERT INTO Notification(Name, Parent, Event, Endpoint) OUTPUT INSERTED.ID VALUES (@name, @parent, @event, @endpoint)",
                new Notification { name = name, @event = @event, endpoint = endpoint, parent = parent }
            );
            return newlyInsertedNotificationId;
        }

        // Obtém notificações pelo endpoint
        public List<Notification> GetNotificationsByEndpoint(string endpoint)
        {
            // Retorna uma lista de notificações que correspondem ao endpoint
            List<Notification> notificationsByEndpoint = DataHelper.GetDataFromDatabase<Notification>(
                "SELECT * FROM Notification WHERE Endpoint LIKE @endpoint",
                new Notification { endpoint = endpoint }
            );
            return notificationsByEndpoint;
        }

        // Elimina uma notificação pelo ID
        public string DeleteNotification(int id)
        {
            // Remove a notificação da base de dados
            DataHelper.TransactWithDatabase<Notification>(
                "DELETE FROM Notification WHERE Id = @id",
                new Notification { id = id }
            );
            return "Notification ID " + id.ToString() + " deleted successfully";
        }

        // Obtém uma notificação pelo nome e pelo ID do container (Parent)
        public int GetNotificationByNameAndParentId(string notificationName, int parentId)
        {
            // Retorna o ID da notificação se existir
            List<Notification> notificationList = DataHelper.GetDataFromDatabase<Notification>(
                "SELECT * FROM Notification WHERE Name = @name AND Parent = @parent",
                new Notification { name = notificationName, parent = parentId }
            );
            if (notificationList.Count > 0)
            {
                return notificationList[0].id;
            }
            return 0; // Retorna 0 se não existir
        }

        // Obtém uma lista de notificações por IDs
        public List<Notification> GetNotificationsByIds(string notificationsIds)
        {
            List<Notification> notificationList = new List<Notification>();
            string[] notificationIdList = notificationsIds.Split(',');

            // Itera sobre os IDs fornecidos e obtém as notificações correspondentes
            foreach (string notificationId in notificationIdList)
            {
                List<Notification> tempNotificationList = DataHelper.GetDataFromDatabase<Notification>(
                    "SELECT * FROM Notification WHERE Id = @id",
                    new Notification { id = int.Parse(notificationId) }
                );
                if (tempNotificationList.Count > 0)
                {
                    notificationList.Add(tempNotificationList[0]);
                }
            }
            return notificationList;
        }

        public int GetNotificationNameByParentId(int parentId)
        {
            List<Notification> notificationList = DataHelper.GetDataFromDatabase<Notification>("SELECT * FROM Notification WHERE Parent = @parent AND Name = @name", new Notification { parent = parentId, name = notificationName });
            int notificationById = 0;
            if (notificationList.Count > 0)
            {
                notificationById = notificationList[0].id;
            }
            return notificationById;
        }

        public List<String> GetNotificationsByContainerId(int containerId)
        {
            return DataHelper.GetDataFromDatabase<String>(
                $"SELECT Notification.name " +
                $"FROM Notification " +
                $"JOIN Container ON Notification.parent = Container.id " +
                $"WHERE Container.id = {containerId}", // Insere o valor diretamente
                 null

            );
        }

    }
}
