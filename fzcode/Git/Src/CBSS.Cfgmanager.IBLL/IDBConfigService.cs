using CBSS.Cfgmanager.Contract.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.Cfgmanager.IBLL
{
    public interface IDBConfigService
    {
        List<DbEntity> GetDBList();
        List<DataBaseTableEntity> GetTableList(string dbName, string tableName);
        List<DataBaseTableFieldEntity> GetTableFiledList(string dbName, string tableName);
        bool AlterTableColumn(string dbName, string tableName, string fieldJson);

        bool AddTableColumn(string dbName, string tableName, string fieldJson);

        bool AddTable(string dbName, string tableName, string des);

        bool DropTable(string dbName, string tableName);

        bool DropField(string dbName, string tableName, string field);
    }
}
