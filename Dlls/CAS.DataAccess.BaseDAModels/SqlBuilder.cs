using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Text.RegularExpressions;

namespace CAS.DataAccess.BaseDAModels
{
    /// <summary>
    /// 用于SQL排序
    /// </summary>
    public enum SqlOrderByType
    {
        ASC = 0,
        DESC,
        RANDOM
    }
    public class SqlBuilder
    {

        public class OrderByItem
        {
            public bool IsAlias;
            public string Name;
            public SqlOrderByType Type;
            public string Table;

            public OrderByItem(string name, SqlOrderByType type, string table)
            {
                this.Name = name;
                this.Table = table;
                this.Type = type;
                this.IsAlias = false;
            }

            public OrderByItem(string name, SqlOrderByType type)
            {
                this.Type = type;
                this.Name = name;
                this.Table = string.Empty;
                this.IsAlias = true;
            }
        }

        public class GroupByItem
        {
            public bool IsAlias;
            public string Name;
            public string Table;

            public GroupByItem(string name, string table)
            {
                this.Name = name;
                this.Table = table;
                this.IsAlias = false;
            }

            public GroupByItem(string name)
            {
                this.Name = name;
                this.Table = string.Empty;
                this.IsAlias = true;
            }
        }

#region SQL Join

        /// <summary>
        /// SqlInnerJoinItem is the Inner-Join Clause sql statements for one table
        /// </summary>
        public class SqlJoinItem
        {

            /// <summary>
            /// SqlCompareItem is a data structure to store the Target table information for 
            /// Inner-Join sql statement 
            /// </summary>
            public struct TableColumn
            {
                /// <summary>
                /// The Target table name in Join Clause sql statement
                /// </summary>
                public string Table;
                /// <summary>
                /// The Target table's column name in Join Clause sql statement
                /// </summary>
                public string Column;

                /// <summary>
                /// SqlCompareItem struct constructor
                /// </summary>
                /// <param name="target">Target table name</param>
                /// <param name="targetColumn">Target table's column name</param>
                public TableColumn(string table, string column)
                {
                    this.Table = table;
                    this.Column = column;
                }


            }

            /// <summary>
            /// Tempory solution to handle Left Join in SqlWriter
            /// </summary>
            private SqlJoinType type = SqlJoinType.InnerJoin;

            public SqlJoinType Type
            {
                get { return type; }
                set { type = value; }
            }

            public Dictionary<string, TableColumn> Compares;
            public Dictionary<string, Query.SqlWhereItem> Items;

            public SqlJoinItem()
            {
                this.Compares = new Dictionary<string, TableColumn>();
                this.Items = new Dictionary<string, Query.SqlWhereItem>();
            }

            /// <summary>
            /// Check whether the table has registered the given column for Inner-Join
            /// </summary>
            /// <param name="key">The column name to be registered</param>
            /// <returns>True if the column has been registered</returns>
            public bool HasColumn(string key)
            {
                return this.Items.Keys.Contains(key);
            }

            /// <summary>
            /// Check whether the target table's column has been registered for Inner-Join
            /// sql statement
            /// </summary>
            /// <param name="key">The target table's column to be registered</param>
            /// <returns>True if the target table's column has been registered</returns>
            public bool HasCompare(string key)
            {
                return this.Compares.Keys.Contains(key);
            }

            public void AddCompare(string key, string targetTable, string targetColumn)
            {
                this.Compares.Add(key, new TableColumn(targetTable, targetColumn));
            }
        }

        /// post join is done befire distinct (mainly for filter)
        private Dictionary<string, SqlJoinItem> innerJoins = new Dictionary<string, SqlJoinItem>();

        /// post join is done After distinct (mainly for orderby)
        private Dictionary<string, SqlJoinItem> postJoins = new Dictionary<string, SqlJoinItem>();


        public enum SqlJoinType
        {
            InnerJoin = 0,
            LeftJoin
        }

        public void AddLeftJoin(string table, string column, string target, string targetColumn)
        {
            AddGenericJoin(table, column, target, targetColumn, string.Empty, string.Empty, SqlDbType.NVarChar, SqlJoinType.LeftJoin);
        }

        public void AddLeftJoin(string table, string column, string target,
            string targetColumn, string key, string value)
        {
            AddGenericJoin(table, column, target, targetColumn, key, value, SqlDbType.NVarChar, SqlJoinType.LeftJoin);
        }

        public void AddLeftJoin(string table, string column, string target,
            string targetColumn, string key, int value)
        {
            AddGenericJoin(table, column, target, targetColumn, key, value, SqlDbType.Int, SqlJoinType.LeftJoin);
        }

        /// <summary>
        /// Collects information for one table related to the Inner-Join Clause.
        /// </summary>
        /// <param name="table">The table name</param>
        /// <param name="column">The table column name for inner-join</param>
        /// <param name="target">The target inner-join table name</param>
        /// <param name="targetColumn">The target inner-join table's column name</param>
        public void AddInnerJoin(string table, string column, string target, string targetColumn)
        {
            AddGenericJoin(table, column, target, targetColumn, string.Empty, string.Empty, SqlDbType.NVarChar, SqlJoinType.InnerJoin);
        }

        /// <summary>
        /// Collects information for one table related to the Inner-Join Clause.
        /// </summary>
        /// <param name="table">The table name</param>
        /// <param name="column">The column name for inner-join</param>
        /// <param name="target">The target inner-join table name</param>
        /// <param name="targetColumn">The target inner-join table's column name</param>
        /// <param name="key">The column name using in the Where Clause</param>
        /// <param name="value">The filter value for the column using in the Where Clause</param>
        public void AddInnerJoin(string table, string column, string target,
            string targetColumn, string key, string value)
        {
            AddGenericJoin(table, column, target, targetColumn, key, value, SqlDbType.NVarChar, SqlJoinType.InnerJoin);
        }

        public void AddPostJoin(string table, string column, string target,
            string targetColumn)
        {
            AddGenericPostJoin(table, column, target, targetColumn, String.Empty, String.Empty, SqlDbType.NVarChar, SqlJoinType.InnerJoin);
        }

        public void AddPostJoin(string table, string column, string target,
            string targetColumn, string key, string value)
        {
            AddGenericPostJoin(table, column, target, targetColumn, key, value, SqlDbType.NVarChar, SqlJoinType.InnerJoin);
        }

        public void AddPostJoin(string table, string column, string target,
            string targetColumn, string key, int value)
        {
            AddGenericPostJoin(table, column, target, targetColumn, key, value, SqlDbType.Int, SqlJoinType.InnerJoin);
        }

        public void AddPostLeftJoin(string table, string column, string target,
           string targetColumn, string key, int value)
        {
            AddGenericPostJoin(table, column, target, targetColumn, key, value, SqlDbType.Int, SqlJoinType.LeftJoin);
        }

        public void AddInnerJoin(string table, string column, string target,
            string targetColumn, string key, int value)
        {
            AddGenericJoin(table, column, target, targetColumn, key, value, SqlDbType.Int, SqlJoinType.InnerJoin);
        }

        /// <summary>
        /// Public interface to AddGenericInnerJoin
        /// </summary>
        public void AddInnerJoin(string table, string column, string target,
             string targetColumn, string key, object value, SqlDbType type)
        {
            AddGenericJoin(table, column, target, targetColumn, key, value, type, SqlJoinType.InnerJoin);
        }

        private void AddGenericJoin(string table, string column, string target, string targetColumn,
            string key, object value, SqlDbType type, SqlJoinType joinType)
        {
            if (this.innerJoins.Keys.Contains(table))
            {
                SqlJoinItem item = this.innerJoins[table];
                if (!item.HasCompare(column))
                    item.AddCompare(column, target, targetColumn);
                if (!item.HasColumn(key) && !key.Equals(string.Empty))
                {
                    Query.SqlWhereItem ijItem = new Query.SqlWhereItem(key, value, type);
                    item.Items.Add(key, ijItem);
                }
            }
            else
            {
                SqlJoinItem item = new SqlJoinItem();
                item.Type = joinType;
                item.AddCompare(column, target, targetColumn);
                if (!key.Equals(string.Empty))
                {
                    Query.SqlWhereItem ijItem = new Query.SqlWhereItem(key, value, type);
                    item.Items.Add(key, ijItem);
                }
                this.innerJoins.Add(table, item);
            }
        }


        /// <summary>
        /// post join is done After distinct (mainly for orderby)
        /// </summary>
        /// <param name="table"></param>
        /// <param name="column"></param>
        /// <param name="target"></param>
        /// <param name="targetColumn"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <param name="joinType"></param>
        private void AddGenericPostJoin(string table, string column, string target, string targetColumn,
           string key, object value, SqlDbType type, SqlJoinType joinType)
        {
            if (!this.useDistinct)
            {
                AddGenericJoin(table, column, target, targetColumn, key, value, type, joinType);
            }
            else
            {
                if (this.postJoins.Keys.Contains(table))
                {
                    SqlJoinItem item = this.postJoins[table];
                    if (!item.HasCompare(column))
                        item.AddCompare(column, target, targetColumn);
                    if (!item.HasColumn(key) && !key.Equals(string.Empty))
                    {
                        Query.SqlWhereItem ijItem = new Query.SqlWhereItem(key, value, type);
                        item.Items.Add(key, ijItem);
                    }
                }
                else
                {
                    SqlJoinItem item = new SqlJoinItem();
                    item.Type = joinType;
                    item.AddCompare(column, target, targetColumn);
                    if (!key.Equals(string.Empty))
                    {
                        Query.SqlWhereItem ijItem = new Query.SqlWhereItem(key, value, type);
                        item.Items.Add(key, ijItem);
                    }
                    this.postJoins.Add(table, item);
                }
            }
        }
#endregion

        public void addGroupByItem(string table, string column)
        {
            this.groupBys.Add(new GroupByItem(column, table));
        }

        public void addGroupByItem(string name)
        {
            this.groupBys.Add(new GroupByItem(name));
        }
        /// <summary>
        /// Collects information for one complete Order By Clause sql statement.
        /// </summary>
        /// <param name="table">The table name</param>
        /// <param name="column">The column name</param>
        /// <param name="type">Order Type, ASC/DESC</param>
        public void AddOrderByItem(string table, string column, SqlOrderByType type)
        {
            this.orderBys.Add(new OrderByItem(column, type, table));
        }

        public void AddOrderByItem(string name, SqlOrderByType type)
        {
            this.orderBys.Add(new OrderByItem(name, type));
        }

        StringBuilder builder;

        /// <summary>
        /// indicate whether to use param binding for joining and where clauses inside sqlbuilding
        /// default is false
        /// </summary>
        private bool internalBinding = false;

        public void SetInternalBinding(bool onoff)
        {
            internalBinding = onoff;
        }

        /// <summary>
        /// for user provided parameters
        /// </summary>
        private List<SqlParameter> bindParams = new List<SqlParameter>();


        public string AddToBindParams(object obj, SqlDbType type)
        {
            string name = "@param" + bindParams.Count;
            var param = new SqlParameter(name, type);
            param.Value = obj;
            bindParams.Add(param);
            return name;
        }

        public string AddToBindParams(string key, object obj, SqlDbType type)
        {
            var param = new SqlParameter(key, type);
            param.Value = obj;
            bindParams.Add(param);
            return key;
        }

        /// <summary>
        /// for system provided parameters (e.g. joins)
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        protected string AddToInternalParams(object obj, SqlDbType type)
        {
            string name = "@iparam" + internalParams.Count;
            var param = new SqlParameter(name, type);
            param.Value = obj;
            internalParams.Add(param);
            return name;
        }

        /// <summary>
        /// for system provided parameters (e.g. joins)
        /// </summary>
        private List<SqlParameter> internalParams;

        private List<string> selects = new List<string>();

        public List<string> Selects
        {
            get { return selects; }
            set { selects = value; }
        }

        private List<OrderByItem> orderBys = new List<OrderByItem>();

        public List<OrderByItem> OrderBys
        {
            get { return orderBys; }
            set { orderBys = value; }
        }

        private List<GroupByItem> groupBys = new List<GroupByItem>();
        public List<GroupByItem> GroupBys
        {
            get { return groupBys; }
            set { groupBys = value; }
        }

        private Query.SqlWhereItemCollection wheres = new Query.SqlWhereItemCollection();

        public Query.SqlWhereItemCollection Wheres
        {
            get { return wheres; }
            set { wheres = value; }
        }

        private bool useDistinct = false;

        private string distinctCol=null;

        private StringBuilder preselect = new StringBuilder();

        private bool hasgroupbys;
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

        public string UnionOperator = "union all";

        /// <summary>
        /// define the name of the resulting table after the union
        /// </summary>
        public const string UnionResult = "unionresult";

        private int limit;

        public int Limit
        {
            get { return limit; }
            set { limit = value; }
        }

        private int absLimit=0;

        private int page;

        public int Page
        {
            get { return page; }
            set { page = value; }
        }

        private int offset;

        public int Offset
        {
            get { return offset; }
            set { offset = value; }
        }

        private string fromTableName;

        public string FromTableName
        {
            get { return fromTableName; }
            set { fromTableName = value; }
        }
        private string fromTableAlias;

        public string FromTableAlias
        {
            get { return fromTableAlias; }
            set { fromTableAlias = value; }
        }

        private bool isBuildCount = false;

        private void buildSelectSql()
        {
            if (this.isBuildCount)
            {
                this.builder.Append("select count(*) as total ");
                return;
            }

            if (limit > 0 && (this.page > 1 || this.offset != 0))
            {
                this.builder.Append("select * from( select Row_Number() over (");
                buildOrderBy();
                this.builder.Append(") as rownum, ");
            }
            else
            {
                this.builder.Append("select ");
                if (limit > 0)
                {
                    this.builder.Append("top ");
                    if (absLimit > 0)
                    {
                        this.builder.Append(Math.Min(limit, absLimit));
                    }
                    else
                    {
                        this.builder.Append(limit);
                    }
                    this.builder.Append(" ");
                }
                else if (absLimit > 0) // still need to apply limit if abslimit is set
                {
                    this.builder.Append("top ");
                    this.builder.Append(absLimit);
                    this.builder.Append(" ");
                }
            }

            if (this.unionSqls != null && this.unionSqls.Count > 0 && !this.isBuildCount)
            {
                this.builder.Append(" * from ( select ");
            }
            if (this.selects.Count == 0)
                this.builder.Append(" * ");
            else
            {
                this.builder.Append(this.selects[0]);
                for (int i = 1; i < this.selects.Count; i++)
                {
                    this.builder.Append(", ");
                    this.builder.Append(this.selects[i]);
                }
            }
        }

        private void buildGroupBy()
        {
            if (this.groupBys.Count > 0 && !this.isBuildCount)
            {
                this.builder.Append(" group by ");
                int i = 0;
                foreach (GroupByItem item in this.groupBys)
                {
                    if (i++ > 0)
                        this.builder.Append(", ");

                    if (item.Table == null)
                    {
                        this.builder.Append(item.Name);
                        this.builder.Append(" ");
                    }
                    else
                    {
                        if (!item.IsAlias)
                        {
                            this.builder.Append(item.Table.Equals(string.Empty) ? this.fromTableName : item.Table);
                            this.builder.Append(".");
                        }
                        this.builder.Append(item.Name);                        
                        this.builder.Append(" ");
                    }
                }
            }
        }

        private void buildOrderBy()
        {
            if (this.orderBys.Count > 0 && !this.isBuildCount)
            {
                this.builder.Append(" order by ");
                int i = 0;
                foreach (OrderByItem item in this.orderBys)
                {
                    if (i++ > 0)
                        this.builder.Append(", ");
                    if (item.Type.Equals(SqlOrderByType.RANDOM))
                    {
                        this.builder.Append(" NEWID() ");
                    }
                    else if (item.Table == null)
                    { // special case!
                        this.builder.Append(item.Name);
                        if (item.Type.Equals(SqlOrderByType.DESC))
                            this.builder.Append(" desc ");
                        else
                            this.builder.Append(" ");
                    }
                    else
                    {
                        if (!item.IsAlias)
                        {
                            this.builder.Append(item.Table.Equals(string.Empty) ? this.fromTableName : item.Table);
                            this.builder.Append(".");
                        }
                        this.builder.Append(item.Name);
                        if (item.Type.Equals(SqlOrderByType.DESC))
                            this.builder.Append(" desc ");
                        else
                            this.builder.Append(" ");
                    }
                }
            }
        }


        private void buildFromSql()
        {
            if (!string.IsNullOrEmpty(fromTableName))
            {
                this.builder.Append(" from ");
                this.builder.Append(this.fromTableName);
                this.builder.Append(" ");
                this.builder.Append(this.fromTableAlias);
            }
        }


        private void buildWhereSql(out int whereCount)
        {
            whereCount = 0;
            //if (this.innerJoins.Count == 0 && !this.wheres.HasWhereClause)
            if (!this.wheres.HasWhereClause)
                return;


            if (this.wheres.HasWhereClause)
            {
                if (whereCount == 0)
                    this.builder.Append(" where ");

                foreach (Query.SqlWhereItem item in this.wheres.Items)
                {
                    if (whereCount++ > 0)
                        this.builder.Append(" and ");

                    this.builder.Append("( ");
                    if (string.IsNullOrEmpty(item.sql))
                    {
                        this.builder.Append(item.key);
                        if (item.compare != null)
                            this.builder.Append(item.compare);

                        if (item.value.ToString().StartsWith("@"))
                        {
                            this.builder.Append(item.value);
                        }
                        else if (internalBinding)
                        {
                            this.builder.Append(this.addToInternalParams(item.value, item.type));
                        }
                        else
                        {
                            if (!item.type.Equals(SqlDbType.Int) && item.compare != null)
                                this.builder.Append(" '");
                            builder.Append(item.value);
                            if (!item.type.Equals(SqlDbType.Int) && item.compare != null)
                                this.builder.Append("' ");
                        }

                    }
                    else
                    {
                        builder.Append(item.sql);
                   }
                    this.builder.Append(" )");
                }
            }
        }

        private void buildLimit()
        {
            if (this.limit > 0 && !this.isBuildCount && (this.page > 1 || this.offset != 0))
            {
                int start = 1 + (this.page - 1) * this.limit + this.offset;
                int end = this.limit + start - 1;
                if (absLimit > 0 && end > absLimit)
                {
                    end = absLimit;
                }
                this.builder.Append(") as result where rownum between ");
                this.builder.Append(start);
                this.builder.Append(" and ");
                this.builder.Append(end);
                this.builder.Append(" order by rownum");
            }


        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="joins"></param>
        /// <param name="postfix">in case there is more than one level of join, postfix will be used to distinguish the parameters</param>
        private void buildJoinSql(Dictionary<string, SqlJoinItem> joins, string postfix)
        {
            int joinCount = 0;
            foreach (KeyValuePair<string, SqlJoinItem> join in joins)
            {
                SqlJoinItem n = join.Value;
                foreach (KeyValuePair<string, SqlJoinItem.TableColumn> m in n.Compares)
                {
                    switch (n.Type)
                    {
                        case SqlJoinType.InnerJoin:
                            this.builder.Append(" inner");
                            break;
                        case SqlJoinType.LeftJoin:
                            this.builder.Append(" left");
                            break;
                    }
                    this.builder.Append(" join ");
                    this.builder.Append(join.Key);
                    if (join.Key.Substring(0, 1) == "(")
                    {
                        this.builder.Append(" as ij").Append(postfix).Append(joinCount).Append(" on ij").Append(postfix).Append(joinCount);
                        joinCount++;
                    }
                    else
                    {
                        this.builder.Append(" on ").Append(join.Key);
                    }

                    this.builder.Append(".");

                    
                    this.builder.Append(m.Key);
                    this.builder.Append(" = ");
                    this.builder.Append(m.Value.Table);
                    this.builder.Append(".");
                    this.builder.Append(m.Value.Column);

                }

                foreach (KeyValuePair<string, Query.SqlWhereItem> m in n.Items)
                {
                    this.builder.Append(" and ");

                    this.builder.Append("( ");
                    this.builder.Append(join.Key);
                    this.builder.Append(".");
                    this.builder.Append(m.Key);
                    this.builder.Append(" = ");
                    if (m.Value.value.ToString().StartsWith("@")) // already binding
                    {
                        this.builder.Append(m.Value.value);
                    }
                    else if (internalBinding)
                    {
                        this.builder.Append(this.AddToInternalParams(m.Value.value, m.Value.type));
                    }
                    else
                    {
                        if (!m.Value.type.Equals(SqlDbType.Int))
                            this.builder.Append(" '");
                        this.builder.Append(m.Value.value);
                        if (!m.Value.type.Equals(SqlDbType.Int))
                            this.builder.Append(" '");
                    }
                    this.builder.Append(" )");
                }

            }
        }

        private void buildJoinSql()
        {
            buildJoinSql(this.innerJoins, "");
        }

        private void buildPostJoinSql()
        {
            buildJoinSql(this.postJoins, "p");
        }

        private string addToInternalParams(object obj, SqlDbType type)
        {
            string name = "@iparam" + internalParams.Count;
            var param = new SqlParameter(name, type);
            param.Value = obj;
            internalParams.Add(param);
            return name;
        }

        public List<SqlParameter> GetSqlParams()
        {
            List<SqlParameter> sqlParams = new List<SqlParameter>();
            sqlParams.AddRange(bindParams);
            sqlParams.AddRange(internalParams);
            return sqlParams;
        }

        /// <summary>
        /// Gather all information and build a complete sql statement.
        /// </summary>
        /// <returns>A complete sql statement</returns>
        public string Build()
        {
            return build(false);
        }

        public string BuildDelete()
        {
            return build(true);
        }

        private string build(bool buildDelete)
        {
            // Empty the StringBuilder before building SQL statements
            this.builder = new StringBuilder();
            this.internalParams = new List<SqlParameter>();

            int whereCount = 0;
            //prepend some sql statements
            //this.builder.Append(preselect);
            if (buildDelete)
            {
                builder.Append("delete ");
            }
            else
            {
                buildSelectSql();
            }
            buildFromSql();

            if (!this.useDistinct)
            {
                buildJoinSql();
                buildPostJoinSql(); // same as join in case of not using distinct
                buildWhereSql(out whereCount);
            }
            else
            {
                this.builder.Append(" inner join (select distinct ");
                this.builder.Append(this.distinctCol);
                this.builder.Append(" from ");
                this.builder.Append(this.FromTableName);
                buildJoinSql();
                buildWhereSql(out whereCount);
                this.builder.Append(") as DistinctTable on DistinctTable.");
                this.builder.Append(this.distinctCol.Split(new char[] { '.' })[1]);
                this.builder.Append(" = ");
                this.builder.Append(this.distinctCol);
                buildPostJoinSql();

            }
            
            
            if (this.hasgroupbys)
            {
                buildGroupBy();
            }

            if (this.unionSqls != null && this.unionSqls.Count > 0)
            {
                int i = 0;
                if (isBuildCount)
                {
                    this.builder.Insert(0, "select sum(total) as total from (");
                }
                foreach (string unionSql in unionSqls)
                {
                    this.builder.Append(" ").Append(UnionOperator).Append(" ");
                    if (isBuildCount)
                    {
                        this.builder.Append(" select count(*) as total from ( ");
                    }
                    this.builder.Append(unionSql);
                    if (isBuildCount)
                    {
                        this.builder.Append(" ) as x");
                        this.builder.Append(i++);
                    }
                }
                if (isBuildCount)
                {
                    this.builder.Append(") as x");
                    this.builder.Append(i++);
                }

            }

            if (this.unionSqls != null && this.unionSqls.Count > 0 && !this.isBuildCount)
            {
                this.builder.Append(" ) ").Append(UnionResult).Append(" ");
            }

            if (limit > 0 && (this.page <= 1 && this.offset == 0) || limit == 0)
            {
                buildOrderBy();
            }
            
            buildLimit();
            return this.builder.ToString();
        }

        public string BuildCountSql()
        {
            this.isBuildCount = true;
            string sql = Build();
            return sql;
        }


        public class FormatSql
        {
            public static string EscapeLikeString(string value, string escapechar)
            {
                string[] str_escape = { escapechar, "%", "_", "[", "]", "^" }; // escapechar must go first
                if (!string.IsNullOrEmpty(value))
                {
                    foreach (string str in str_escape)
                    {
                        value = value.Replace(str, escapechar + str);
                    }
                }

                return value;
            }

            public static string EscapeContainString(string value)
            {
                value = value.Replace("\"", "\"\"");
                value = value.Replace("'", "''");
                return value;
            }
        }
    }
}
