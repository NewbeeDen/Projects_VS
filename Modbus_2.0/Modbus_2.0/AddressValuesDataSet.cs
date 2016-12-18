using System.Data;

namespace Modbus_2._0
{
    public partial class AddressValuesDataSet
    {
        internal AddressValuesDataSet AddDataTable(DataTable dataTable)
        {
            return new AddressValuesDataSet {Tables = {dataTable}};
        }
    }
}
