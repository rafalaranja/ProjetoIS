using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SOMIODMiddleware.Helper;
using SOMIODMiddleware.Models;

namespace SOMIODMiddleware.Controllers
{
    public class RecordController
    {
        private ApplicationController applicationController = new ApplicationController();
        private ContainerController containerController = new ContainerController();
        string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["SOMIODMiddleware.Properties.Settings.ConnStr"].ConnectionString;

        // Obtém o nome de todas os Records
        public List<string> GetAllRecordsNames()
        {
            return DataHelper.GetDataFromDatabase<string>(
                "SELECT name FROM Record ORDER BY Id",
                null
            );
        }

        // Obtém todas os records
        public List<Record> GetAllRecords()
        {
            return DataHelper.GetDataFromDatabase<Record>(
                "SELECT name FROM Record ORDER BY Id",
                null
            );
        }


        // Cria um novo registo associado a um container
        public int CreateRecord(string content, int containerId)
        {
            // Inserir o registo na base de dados
            int recordId = DataHelper.TransactWithDatabase<Record>(
                "INSERT INTO Record(Content, Parent) OUTPUT INSERTED.ID VALUES(@content, @parent)",
                new Record { content = content, parent = containerId }
            );

            return recordId; // Retorna o ID gerado
        }

        // Elimina um registo pelo ID
        public void DeleteRecord(int recordId)
        {
            // Remove o registo da base de dados
            DataHelper.TransactWithDatabase<Record>(
                "DELETE FROM Record WHERE Id = @id",
                new Record { id = recordId }
            );
        }

        // Obtém registos com base nos seus IDs
        public List<Record> GetRecordById(string recordId)
        {
            return DataHelper.GetDataFromDatabase<Record>(
                "SELECT * FROM Record WHERE Id = @id",
                new Record { id = int.Parse(recordId) }
            );
        }

        // Obtém todos os registos de um container
        public List<Record> GetRecordsByContainerId(int containerId)
        {
            return DataHelper.GetDataFromDatabase<Record>(
                "SELECT * FROM Record WHERE Parent = @parent",
                new Record { parent = containerId }
            );
        }

        // Verifica se um registo existe pelo ID
        public bool DoesRecordExist(int recordId)
        {
            var records = GetRecordById(recordId.ToString());
            return records != null && records.Count > 0;
        }

        public int GetRecordByContentAndParentId(String content, int containerId)
        {
            List<Record> recordList = DataHelper.GetDataFromDatabase<Record>("SELECT * FROM Record WHERE Parent = @parent AND Content = @content", new Record { parent = containerId, content = content });
            int recordById = 0;
            if (recordList.Count > 0)
            {
                recordById = recordList[0].id;
            }
            return recordById;
        }

    }
}
