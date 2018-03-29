using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Text.RegularExpressions;
using System.Data.SqlClient;

namespace CAS.DataAccess.BaseDAModels
{
    public class ExceptionEmptyQuery : Exception { }
    public class ExceptionBadFilterSequence : Exception { } // indicate wrong query sequence inside filter
    /// <summary>
    /// Query Object - a class to define a query. Pass into DAO to get items (e.g. via GetItem)
    /// </summary>
    public class Query
    {

        public struct ParameterItem
        {

            public string Value {get;set;}
            public SqlDbType Type {get;set;}

            private ParameterDirection _direction;

            public ParameterDirection Direction
            {
                get { return _direction; }
                set { _direction = value; }
            }

        
            public override string ToString()
            {
                return Value;
            }
        }

        public struct SelectItem
        {

            public string Value { get; set; }
            public string[] Parameters { get; set; }
           
        }

        public class SelectItemCollection
        {
            private List<SelectItem> items = new List<SelectItem>();

            public int Count
            {
                get
                {
                    return items.Count;
                }
            }

            public SelectItem this[int i]
            {
                get
                {
                    return items[i];
                }
            }

            internal List<SelectItem> Items
            {
                get { return items; }
            }

            public SelectItem Add(string value)
            {
                SelectItem item = new SelectItem { Value = value };
                items.Add(item);                
                return item;
            }

            public SelectItem Add(string value, string[] parameters)
            {
                SelectItem item = new SelectItem { Value = value, Parameters = parameters};
                items.Add(item);
                return item;
            }
        }

        public struct SqlWhereItem
        {
            public string key;
            public object value;
            public string compare;
            public SqlDbType type;

            private string[] parameters;

            /// <summary>
            /// if using plain SQL, use this to add parameters
            /// </summary>
            public string[] Parameters {
                get
                {
                    return parameters;
                }
                set
                {
                    parameters = value;
                }
            }
            public string sql;

            public SqlWhereItem(string key, object value, SqlDbType type)
            {
                this.key = key;
                this.value = value;
                this.type = type;
                this.parameters = null;
                this.sql = null;
                this.compare = null;
                
            }

        }

        public class SqlWhereItemCollection
        {

            public SqlWhereItemCollection()
            {
                this.items = new List<SqlWhereItem>();
            }

            private List<SqlWhereItem> items;

            public List<SqlWhereItem> Items
            {
                get { return items; }
            }

            public void Add(SqlWhereItemCollection wheres)
            {
                this.items = (List<SqlWhereItem>) this.items.Concat(wheres.items);
            }

            public void Add(SqlWhereItem where)
            {
                items.Add(where);
            }

            /// <summary>
            /// Add one where clause sql statement.
            /// </summary>
            /// <param name="sql">One where clause sql statement</param>
            public void AddSql(string sql)
            {
                if (!string.IsNullOrEmpty(sql))
                {
                    SqlWhereItem item = new SqlWhereItem();
                    item.sql = sql;
                    item.type = 0; // invalid value
                    this.items.Add(item);
                }
            }

            /// <summary>
            /// Add one where clause sql statement.
            /// </summary>
            /// <param name="sql">One where clause sql statement</param>
            public void AddSql(string sql, string[] parameters)
            {
                if (!string.IsNullOrEmpty(sql))
                {
                    SqlWhereItem item = new SqlWhereItem();
                    item.sql = sql;
                    item.type = 0; // invalid value
                    item.Parameters = parameters;
                    this.items.Add(item);
                }
            }

            public void AddPair(string key, string value)
            {
                AddGeneric(key, value, string.Empty, SqlDbType.NVarChar);
            }

            public void AddEqualPair(string key, string value, SqlDbType type)
            {
                AddGeneric(key, value, " = ", type);
            }

            /// <summary>
            /// Add equal where clause sql.
            /// ie. key = value
            /// </summary>
            /// <param name="key">The field in the database</param>
            /// <param name="value">The value to be compared</param>
            public void AddEqualPair(string key, string value)
            {
                AddGeneric(key, value, " = ", SqlDbType.NVarChar);
            }

            /// <summary>
            /// support query like >= 1 or != 1
            /// </summary>
            /// <param name="key"></param>
            /// <param name="opsvalue"></param>
            public void AddOperatorIntPair(string key, string opsvalue)
            {
                string opr;
                int val;

                Regex regex = new Regex(@"^(<=|>=|<|>|=|!=|<>)\s*([0-9]+)$", RegexOptions.None);
                var matches = regex.Match(opsvalue);
                if (matches.Success)
                {
                    opr = matches.Groups[1].Value;
                    val = int.Parse(matches.Groups[2].Value);
                }
                else
                {
                    if (!int.TryParse(opsvalue, out val)) return;
                    opr = "=";
                }

                AddGeneric(key, val, opr, SqlDbType.Int);

            }


            /// <summary>
            /// support query like >= 1 or != 1
            /// </summary>
            /// <param name="key"></param>
            /// <param name="opsvalue"></param>
            public void AddOperatorEnumPair(string key, string opsvalue, Type enumtype)
            {
                string opr;
                int val;

                if (string.IsNullOrEmpty(opsvalue) || (opsvalue.Trim() == string.Empty)) return;

                Regex regex = new Regex(@"^(<=|>=|<|>|=|!=|<>)?\s*([0-9]+)?([^0-9].+)?$", RegexOptions.None);
                var matches = regex.Match(opsvalue);
                if (matches.Success)
                {
                    if (matches.Groups[1].Value != "")
                    {
                        opr = matches.Groups[1].Value;
                    }
                    else
                    {
                        opr = "=";
                    }
                    if (matches.Groups[2].Value != "")
                    {
                        if (!int.TryParse(matches.Groups[2].Value, out val)) return;

                    }
                    else if (enumtype != null)
                    {
                        object obj = Enum.Parse(enumtype, matches.Groups[3].Value, true);
                        if (obj != null) val = (int)obj;
                        else throw new Exception("Invalid Format:" + opsvalue);
                    }
                    else
                    {
                        throw new Exception("Invalid Format:" + opsvalue);
                    }

                }
                else
                {
                    throw new Exception("Invalid Format:" + opsvalue);
                }

                AddGeneric(key, val, opr, SqlDbType.Int);
            }

            public void AddOperatorPair(string key, string ops, int value)
            {
                AddGeneric(key, value, ops, SqlDbType.Int);
            }

            public void AddEqualPair(string key, long value)
            {
                AddGeneric(key, value, " = ", SqlDbType.BigInt);
            }

            public void AddEqualPair(string key, int value)
            {
                AddGeneric(key, value, " = ", SqlDbType.Int);
            }

            public void AddEqualPair(string key, bool value)
            {
                AddGeneric(key, value, " = ", SqlDbType.Bit);
            }
            /// <summary>
            /// Add greater than where clause sql
            /// ie. key > value
            /// </summary>
            /// <param name="key">The field in the database</param>
            /// <param name="value">The value to be compared</param>
            public void AddGreaterPair(string key, int value)
            {
                AddGeneric(key, value, " > ", SqlDbType.Int);
            }

            /// <summary>
            /// Add smaller than where clause sql
            /// ie. key (smaller than) value
            /// </summary>
            /// <param name="key">The field in the database</param>
            /// <param name="value">The value to be compared</param>
            public void AddSmallerPair(string key, int value)
            {
                AddGeneric(key, value, " < ", SqlDbType.Int);
            }

            private void AddGeneric(string key, object value, string comparer, SqlDbType type)
            {

                switch (type)
                {
                    case SqlDbType.BigInt:
                        if (value.GetType() == typeof(String))
                        {
                            long result;
                            if (!long.TryParse((string)value, out result))
                            {
                                throw new ExceptionEmptyQuery();
                            }
                        }
                        break;
                    case SqlDbType.Int:
                    case SqlDbType.SmallInt:
                    case SqlDbType.TinyInt:
                        if (value.GetType() == typeof(String))
                        {
                            int result;
                            if (!int.TryParse((string)value, out result))
                            {
                                throw new ExceptionEmptyQuery();
                            }
                        }
                        break;
                }
                SqlWhereItem item = new SqlWhereItem();
                item.key = key;
                item.compare = comparer;
                item.value = value;
                item.type = type;
                this.items.Add(item);
            }

            /// <summary>
            /// Returns true if there exists where clause
            /// </summary>
            public bool HasWhereClause
            {
                get { return this.items.Count > 0; }
            }

        }

        public struct StoreProcType
        {
            public string Name;
            public Dictionary<string, SqlDbType> Parameters;
        }

        private bool hasgroupbys = false;

        public bool HasGroupBys
        {
            get { return hasgroupbys; }
            set { hasgroupbys = value; }
        }


        private List<string> unionSqls = new List<string>();

        public List<string> UnionSqls
        {
            get { return unionSqls; }
            set { unionSqls = value; }
        }

        private List<SqlParameter> unionParameters = new List<SqlParameter>();

        public List<SqlParameter> UnionParameters
        {
            get { return unionParameters; }
            set { unionParameters = value; }
        }



        private int limit = 0;       

        public int Limit
        {
            get { return limit; }
            set { limit = value; }
        }
        private int offset = 0;

        public int Offset
        {
            get { return offset; }
            set { offset = value; }
        }
        private int page = 1;

        public int Page
        {
            get { return page; }
            set { page = value; }
        }

       [NonSerialized]
        private int _total = -1;


       /// <summary>
       /// set externally if available
       /// </summary>
       public int Total
        {
            set { _total = value; }
        }


       private SelectItemCollection selects = new SelectItemCollection();

       public SelectItemCollection Selects
        {
            get { return selects; }
            set { selects = value; }
        }
        //private Dictionary<string, string[]> selectParams = new Dictionary<string, string[]>();

        //public Dictionary<string, string[]> SelectParams
        //{
        //    get { return selectParams; }
        //    set { selectParams = value; }
        //}

        private Dictionary<string, ParameterItem> parameters = new Dictionary<string, ParameterItem>();

        private Dictionary<string, string> ands = new Dictionary<string, string>();

        public Dictionary<string, string> Ands
        {
            get { return ands; }
            set { ands = value; }
        }



        private SqlWhereItemCollection wheres = new SqlWhereItemCollection();

        /// <summary>
        /// where individual parts, using AND to join together
        /// </summary>
        public SqlWhereItemCollection Wheres
        {
            get { return wheres; }
            set { wheres = value; }
        }


        private List<string> orderBys = new List<string>();

        public List<string> OrderBys
        {
            get { return orderBys; }
            set { orderBys = value; }
        }

        private List<string> groupBys = new List<string>();

        public List<string> GroupBys
        {
            get { return groupBys; }
            set { groupBys = value; }
        }



        private string procedure = null;

        public string Procedure
        {
            get { return procedure; }
            set { procedure = value; }
        }

        public Dictionary<string, ParameterItem> Parameters
        {
            get { return parameters; }
            set { parameters = value; }
        }

        private bool isTotalEnabled = false; // default off

        public bool IsTotalEnabled
        {
            get { return isTotalEnabled; }
            set { isTotalEnabled = value; }
        }

        private bool isTotalOnly = false; // default off
        public bool IsTotalOnly
        {
            get { return isTotalOnly; }
            set { isTotalOnly = value; }
        }

        private string tableName = null;

        /// <summary>
        /// TableName should not be used if already defined inside DAO
        /// This is only used inside a generic DB-level DAO, to access the table defined here.
        /// </summary>
        public string TableName
        {
            get { return tableName; }
            set { tableName = value; }
        }


        List<string> resolves = new List<string>();

        public List<string> Resolves
        {
            get { return resolves; }
            set { resolves = value; }
        }

        #region Build SqlParameter
        // add by Peter
        // date 2010-11-18
        public SqlParameter BuilderParam(string v_sParamName, SqlDbType v_oDBtype, object v_oValue)
        {
            return BuilderParam(v_sParamName, v_oDBtype, -1, ParameterDirection.Input, v_oValue);
        }

        public SqlParameter BuilderParam(string v_sParamName, SqlDbType v_oDBtype, int v_iSize, object v_oValue)
        {
            return BuilderParam(v_sParamName, v_oDBtype, v_iSize, ParameterDirection.Input, v_oValue);
        }

        public SqlParameter BuilderParam(string v_sParamName, SqlDbType v_oDBtype, int Legth,ParameterDirection ParamenterOutput)
        {
            return BuilderParam(v_sParamName, v_oDBtype, Legth, ParamenterOutput, null);
        }

        public SqlParameter BuilderParam(string v_sParamName, SqlDbType v_oDBtype, ParameterDirection v_oDirection, object v_oValue)
        {
            return BuilderParam(v_sParamName, v_oDBtype, -1, v_oDirection, v_oValue);
        }

        public SqlParameter BuilderParam(string v_sParamName, SqlDbType v_oDBtype, int v_iSize, ParameterDirection v_oDirection, object v_oValue)
        {
            SqlParameter param;
            if (v_iSize > 0)
                param = new SqlParameter(v_sParamName, v_oDBtype, v_iSize);
            else
                param = new SqlParameter(v_sParamName, v_oDBtype);
            param.Direction = v_oDirection;

            if (v_oValue == null || string.IsNullOrEmpty(v_oValue.ToString()))
                v_oValue = DBNull.Value;

            if (!(v_oDirection == ParameterDirection.Output && v_oValue == null))
                param.Value = v_oValue;

            
            return param;
        }

        static public SqlParameter BuilderParam(string v_sParamName, object v_oValue)
        {
            if (v_oValue == null)
            {
                v_oValue = DBNull.Value;
            }
            Type type = v_oValue.GetType();
            SqlParameter param = new SqlParameter();
            switch (type.Name.ToLower())
            {
                case "datetime":
                    if ((DateTime)v_oValue <= DateTime.MinValue)
                    {
                        v_oValue = DBNull.Value;
                    }
                    param.SqlDbType = SqlDbType.DateTime;
                    break;
                case "int64":
                    param.SqlDbType = SqlDbType.BigInt;
                    break;
                default:
                    break;
            }
            param.ParameterName = v_sParamName;
            param.Value = v_oValue;
            return param;
        }
        #endregion
    }
}
