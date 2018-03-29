using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace FXT.DataCenter.Infrastructure.Common.ExtensionMethod
{
    public static class DataTableConvertToList
    {
        //使用静态方法扩展datatable
        public static IEnumerable<T> FillObjects<T>(this DataTable dataTable) where T : class
        {
            return new DataTableEnumerable<T>() { Data = dataTable };
        }
    }

    public class DataTableEnumerable<T> : IEnumerable<T>
    {
        public DataTable Data;

        public IEnumerator<T> GetEnumerator()
        {
            return new DataTableEnumerator<T>(Data);
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return new DataTableEnumerator<T>(Data);
        }

    }

    public class DataTableEnumerator<T> : IEnumerator<T>
    {
        private DataTable data;
        private int index = -1;

        public DataTableEnumerator(DataTable data)
        {
            if (data == null)
            {
                data = new DataTable();
            }
            this.data = data;
        }

        public T Current
        {
            get { return convert(); }
        }

        public void Dispose() { }

        object System.Collections.IEnumerator.Current
        {
            get { return convert(); }
        }

        public bool MoveNext()
        {
            index++;
            return index < data.Rows.Count;
        }

        public void Reset()
        {
            index = -1;
        }

        private T convert()
        {
            var row = data.Rows[index];

            var tType = typeof(T);

            var properties = tType.GetProperties();

            //反射动态调用这个类
            var obj = tType.GetConstructor(new Type[] { }).Invoke(null);

            foreach (DataColumn col in data.Columns)
            {
                var val = row[col];
                if (val.GetType() == typeof(DBNull))
                {
                    continue;
                }
                var prop = properties.SingleOrDefault(m => m.Name.ToUpper() == col.ColumnName.ToUpper());
                if (prop == null)
                {
                    continue;
                }
                if (prop.PropertyType.IsGenericType && prop.PropertyType.Name == typeof(Nullable<>).Name)
                {
                    prop.SetValue(obj, Convert.ChangeType(val, prop.PropertyType.GetGenericArguments()[0]), null);
                }
                else
                {

                    prop.SetValue(obj, Convert.ChangeType(val, prop.PropertyType), null);
                }
            }

            return (T)obj;
        }
    }
}
