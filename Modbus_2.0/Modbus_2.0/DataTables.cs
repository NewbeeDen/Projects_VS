using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modbus_2._0
{
    class DataTables
    {
        internal DataTable CreateAddressValueDataTable(string dataTableName)
        {
            var dt = new DataTable { TableName = dataTableName };
            dt.Columns.Add("Address");
            dt.Columns.Add("Value");
            dt.Columns.Add("queueNumber");
            return dt;
        }

        internal DataTable SortByAddressDataTable(DataTable dataTable)
        {
            var dv = new DataView(dataTable) {Sort = "Value ASC"};
            return dv.ToTable();
        }


    }
}
