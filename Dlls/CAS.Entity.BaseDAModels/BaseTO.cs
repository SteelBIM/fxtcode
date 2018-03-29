using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Data.SqlClient;
using System.Xml;
using System.Runtime.Serialization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace CAS.Entity.BaseDAModels
{
    [Serializable]
    public class BaseTO//: IDictionary<string, object>
    {

        static Dictionary<string, Dictionary<string, PropertyInfo>> _sqlFieldNameMappingAllIgnoreCase = new Dictionary<string, Dictionary<string, PropertyInfo>>();

        private Dictionary<string, object> columns = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
        /// <summary>
        /// Columns only store values that are not already in the TO's properties
        /// use method instead of property to prevent serialize. By Norman.Chen 2012-06-15
        /// </summary>
        public Dictionary<string, object> GetColumns()
        {
            return columns; 
        }

        public BaseTO() { }

        public BaseTO(SqlDataReader reader)
        {
            Initialize(reader);
            AfterInitialize();
        }

        public BaseTO(Dictionary<string, object> reader)
        {
            Initialize(reader);
            AfterInitialize();
        }

        public BaseTO(XmlNode node)
        {
            Initialize(node);
            AfterInitialize();
        }

        public BaseTO(XmlAttributeCollection attributes)
        {
            Initialize(attributes);
            AfterInitialize();
        }

        public virtual void AfterInitialize()
        {
        }

        /// <summary>
        /// 记录总数，用于分页时取值 kevin
        /// </summary>
        private int _recordcount = 0;
        public int recordcount
        {
            get { return _recordcount; }
            set { _recordcount = value; }
        }

        private string[] ignoreFields = null;
        /// <summary>
        /// 指定忽略操作的列，生成的sql脚本将不包含这些列
        /// </summary>
        public void SetIgnoreFields(string[] fields)
        {
            if (null != fields && 0 < fields.Length)
            {
                ignoreFields = fields.Select(t => t.ToLower()).ToArray();
            }
        }
        public string[] GetIgnoreFields()
        {
            return ignoreFields;
        }
        private string[] availableFields = null;
        /// <summary>
        /// 指定要操作的列
        /// </summary>
        public void SetAvailableFields(string[] fields)
        {
            if (null != fields && 0 < fields.Length)
            {
                availableFields = fields.Select(t => t.ToLower()).ToArray();
            }
        }
        public string[] GetAvailableFields() 
        {
            return availableFields;
        }
        /// <summary>
        /// 指定主键是否标示字段
        /// 0 - 根据attribute的设置，1 - 指定是标示, 2 - 指定不是标示
        /// </summary>
        [SQLIgnore]
        public int CustomPrimaryKeyIdentify { get; set; }
        public void SetPrimaryKeyIsIdentify(bool isIdentify)
        {
            Type type = this.GetType();
            PropertyInfo[] infos = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            bool hasAvailableFields = null != availableFields;
            foreach (PropertyInfo info in infos)
            {
                if ("System.Array" == info.PropertyType.BaseType.FullName) continue;
                SQLFieldAttribute[] atts = (SQLFieldAttribute[])info.GetCustomAttributes(typeof(SQLFieldAttribute), false);
                if (null != atts && atts.Length > 0)
                {
                    if (atts[0].FieldUsage == EnumDBFieldUsage.PrimaryKey && atts[0].IsIdentify)
                    {
                        CustomPrimaryKeyIdentify = isIdentify ? 1 : 2;
                        break;
                    }
                }
            }
        }
        [SQLIgnore]
        public bool IsSetCustomerFields
        {
            get
            {
                return (null != ignoreFields && ignoreFields.Length > 0) || (null != availableFields && availableFields.Length > 0);
            }
        }
        public void ClearFileds() {
            _sqlFieldNameMappingAllIgnoreCase.ThreadSafeRemove(
                    GetTableName(this.GetType()),
                    "getSqlFieldNameMappingIgnoreCase");
            SetAvailableFields(null);
            SetIgnoreFields(null);
        }

        private static Dictionary<string, PropertyInfo> buildSqlFieldNameMappingIgnoreCase(string tableName)
        {
            Type type = tableEntityType.ThreadSafeRead<string, Type>(tableName, "tableEntityType");
            Dictionary<string, PropertyInfo> _result = new Dictionary<string, PropertyInfo>(StringComparer.OrdinalIgnoreCase);

            PropertyInfo[] infos = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            //string[] customPK = entityPrimaryKeys.ThreadSafeRead<string, string[]>(tableName, "entityPrimaryKeys");
            foreach (PropertyInfo info in infos)
            {
                if ("System.Array" == info.PropertyType.BaseType.FullName) continue;
                // not ignored
                bool isValidField = false;
                if (info.GetCustomAttributes(typeof(SQLIgnoreAttribute), false).Length == 0)
                {
                    isValidField = true;
                }
                if (isValidField)
                {
                    SQLFieldAttribute[] atts = (SQLFieldAttribute[])info.GetCustomAttributes(typeof(SQLFieldAttribute), false);
                    SQLFieldAttribute[] nameAtt = atts.Where(t => !string.IsNullOrEmpty(t.Name)).ToArray<SQLFieldAttribute>();
                    string fieldName = nameAtt.Length > 0 ? nameAtt[0].Name : info.Name;

                    _result.Add(fieldName, info);
                }
            }
            return _result;
        }

        private Dictionary<string, PropertyInfo> getSqlFieldNameMappingIgnoreCase()
        {
            return _sqlFieldNameMappingAllIgnoreCase.ThreadSafeReadAndAddIfNotExists(
                    GetTableName(this.GetType()),
                    new ReadWriteLock.GetDictValueDelegate<string, Dictionary<string, PropertyInfo>>(buildSqlFieldNameMappingIgnoreCase),  
                    "getSqlFieldNameMappingIgnoreCase");
        }
        private static Dictionary<string, PropertyInfo> getSqlFieldNameMappingIgnoreCase(Type type)
        {
            return _sqlFieldNameMappingAllIgnoreCase.ThreadSafeReadAndAddIfNotExists(
                    GetTableName(type),
                    new ReadWriteLock.GetDictValueDelegate<string, Dictionary<string, PropertyInfo>>(buildSqlFieldNameMappingIgnoreCase),
                    "getSqlFieldNameMappingIgnoreCase");
        }


        public void Initialize(SqlDataReader reader)
        {
            Dictionary<string, PropertyInfo> sqlFieldNameMapping = getSqlFieldNameMappingIgnoreCase();

            for (int i = 0; i < reader.FieldCount; i++)
            {
                string field = reader.GetName(i);
                if (field.Equals("rownum"))
                    continue;
                PropertyInfo info;
                sqlFieldNameMapping.TryGetValue(field, out info);
#if DEBUG
                try
                {
#endif
                    if (info != null)
                    {
                        object value = reader.GetValue(i);
                        if (value != null && value != DBNull.Value)
                            info.SetValue(this, value, null);
                    }
                    else
                    {
                        // save undefined fields to values
                        columns[field] = reader.GetValue(i);
                    }
#if DEBUG
                }
                catch (Exception e)
                {
                    throw new Exception("Cannot Init TO: field " + field + " " + e.Message, e);
                }
#endif
            }
        }

        public Dictionary<string, object> GetSQLWriteValues()
        {
            Dictionary<string, PropertyInfo> sqlFieldNameMapping;
            string tableName = GetTableName(this.GetType());
            sqlFieldNameMapping = getSqlFieldNameMappingIgnoreCase();
            Dictionary<string, object> insertDict = new Dictionary<string,object>();

            string[] customPK = entityPrimaryKeys.ThreadSafeRead<string, string[]>(tableName, "entityPrimaryKeys");

            string fieldName = null;
            object fieldValue = null;
            foreach (PropertyInfo info in sqlFieldNameMapping.Values)
            {
                fieldName = null;
                fieldValue = null;
                if (info.CanRead)
                {
                    SQLFieldAttribute[] atts = (SQLFieldAttribute[])info.GetCustomAttributes(typeof(SQLFieldAttribute), false);
                    SQLReadOnlyAttribute[] readOnlyAtts = (SQLReadOnlyAttribute[])info.GetCustomAttributes(typeof(SQLReadOnlyAttribute), false);
                    bool isValidField = false;
                    bool isIgnored = info.GetCustomAttributes(typeof(SQLIgnoreAttribute), false).Length != 0;

                    isValidField = !isIgnored //didn't declared as ignored field
                        && 1 != CustomPrimaryKeyIdentify && !(0 == CustomPrimaryKeyIdentify && atts.Length > 0 && atts.Where(t => t.IsIdentify).Count() > 0)//identity field can't write
                        && ((null == availableFields || 0 == availableFields.Length) || (null != availableFields && availableFields.Length > 0 && availableFields.Contains<string>(info.Name.ToLower())))//field in available list
                        && !(null != ignoreFields && ignoreFields.Contains<string>(info.Name.ToLower()))//field not in ignored field list
                        && (null == readOnlyAtts || 0 == readOnlyAtts.Length)//readonly field can't write
                        ;

                    if (isValidField)
                    {
                        SQLFieldAttribute[] nameAtt = atts.Where(t => !string.IsNullOrEmpty(t.Name)).ToArray<SQLFieldAttribute>();
                        fieldName = nameAtt.Length > 0 ? nameAtt[0].Name : info.Name;

                        if (null != fieldName){
                            if (info.PropertyType == typeof(System.String))//check the max length for string type property
                            {
                                MaxLengthAttribute[] attrMaxLength = (MaxLengthAttribute[])info.GetCustomAttributes(typeof(MaxLengthAttribute), false);
                                if (attrMaxLength.Length > 0 && null != fieldValue && fieldValue.ToString().Length > attrMaxLength[0].MaxLength)
                                {
                                    fieldValue = fieldValue.ToString().Substring(0, attrMaxLength[0].MaxLength);
                                }
                            }
                            fieldValue = info.GetValue(this, null);
                            insertDict.Add(fieldName, fieldValue);
                        }
                    }
                }
            }

            return insertDict;
        }

        #region handle field attribute
        //add by Norman.Chen 2012-06-15
        //Get the database primary keys.
        public static string[] GetSQLPrimaryKey(string tableName)
        {
            List<string> primaryKey = new List<string>();
            Type type = tableEntityType.ThreadSafeRead<string, Type>(tableName, "tableEntityType");
            Dictionary<string, PropertyInfo> sqlFieldNameMapping = getSqlFieldNameMappingIgnoreCase(type);
            Dictionary<string, object> insertDict = new Dictionary<string, object>();

            foreach (PropertyInfo info in sqlFieldNameMapping.Values)
            {
                if (info.CanRead)
                {
                    SQLFieldAttribute[] atts = (SQLFieldAttribute[])info.GetCustomAttributes(typeof(SQLFieldAttribute), false);
                    if (atts.Length > 0)
                    {
                        if (EnumDBFieldUsage.PrimaryKey == atts[0].FieldUsage)
                        {
                            primaryKey.Add(atts[0].Name ?? info.Name);
                        }
                    }
                }
            }

            return primaryKey.ToArray();
        }
        public static List<string> GetSQLPrimaryKey(Type type)
        {
            List<string> primaryKey = new List<string>();
            Dictionary<string, PropertyInfo> sqlFieldNameMapping = getSqlFieldNameMappingIgnoreCase(type);
            Dictionary<string, object> insertDict = new Dictionary<string, object>();

            foreach (PropertyInfo info in sqlFieldNameMapping.Values)
            {
                if (info.CanRead)
                {
                    SQLFieldAttribute[] atts = (SQLFieldAttribute[])info.GetCustomAttributes(typeof(SQLFieldAttribute), false);
                    if (atts.Length > 0)
                    {
                        if (EnumDBFieldUsage.PrimaryKey == atts[0].FieldUsage)
                        {
                            primaryKey.Add(atts[0].Name ?? info.Name);
                        }
                    }
                }
            }

            return primaryKey;
        }
        //保存主键列表
        //这里用static导致并发处理时混乱，数据更新出错的问题 kevin 20140819
        private Dictionary<string, string[]> entityPrimaryKeys = new Dictionary<string, string[]>();
        public string[] GetPrimaryKey<T>()
        {
            return GetPrimaryKey(GetTableName<T>());
        }
        //Get the entity primary keys
        public string[] GetPrimaryKey(string tableName)
        {
            string[] primaryKeyName = entityPrimaryKeys.ThreadSafeRead<string, string[]>(tableName, "entityPrimaryKeys");
            if (null == primaryKeyName)
            {
                List<string> primaryKeyList = new List<string>();
                Type type = tableEntityType.ThreadSafeRead<string, Type>(tableName, "tableEntityType");
                Dictionary<string, PropertyInfo> sqlFieldNameMapping = getSqlFieldNameMappingIgnoreCase(type);
                Dictionary<string, object> insertDict = new Dictionary<string, object>();
                foreach (PropertyInfo info in sqlFieldNameMapping.Values)
                {
                    SQLFieldAttribute[] attrs = GetPropertyAttributes<SQLFieldAttribute>(info);
                    if (0 < attrs.Length)
                    {
                        if (EnumDBFieldUsage.PrimaryKey == attrs[0].FieldUsage)
                        {
                            primaryKeyList.Add(info.Name);
                        }
                    }
                }
                primaryKeyName = primaryKeyList.ToArray<string>();
                entityPrimaryKeys.ThreadSafeWrite(tableName, primaryKeyName, "entityPrimaryKeys");
            }

            return primaryKeyName;
        }
        public void SetPrimaryKey(string tableName, string[] primaryKeys)
        {
            string[] keys = null;
            if (null != primaryKeys)
            {
                keys = primaryKeys.Select(t => t.ToLower()).ToArray();
            }
            entityPrimaryKeys.ThreadSafeWrite(tableName, keys, "entityPrimaryKeys");
        }
        public void SetPrimaryKey<T>(string[] primaryKeys)
        {
            SetPrimaryKey(GetTableName<T>(), primaryKeys);
        }
        //Get primary key value that decared by SQLFieldAttribute
        public object GetFirstPrimaryKeyValue()
        {
            string[] primaryKey = GetPrimaryKey(GetTableName(this.GetType()));
            object primaryKeyValue = null;
            Dictionary<string, PropertyInfo> dicP = getSqlFieldNameMappingIgnoreCase();
            if (null != dicP)
            {
                PropertyInfo p = dicP[primaryKey[0]];
                if (null != p)
                {
                    primaryKeyValue = p.GetValue(this, null);
                }
            }
            return primaryKeyValue;
        }
        public PropertyInfo GetFieldPropertyInfo(string name)
        {
            PropertyInfo p = null;
            Dictionary<string, PropertyInfo> dicP = getSqlFieldNameMappingIgnoreCase(this.GetType());
            if (null != dicP)
            {
                p = dicP[name];
            }
            return p;
        }
        public A[] GetFieldPropertyAttribute<A>(string name) where A : Attribute
        {
            PropertyInfo p = GetFieldPropertyInfo(name);
            A[] attrs = null;
            if (null != p)
            {
                attrs = GetPropertyAttributes<A>(p);
            }
            return attrs;
        }
        public object GetPropertyValue(string propertyName)
        {
            object primaryKeyValue = null;
            Dictionary<string, PropertyInfo> dicP = getSqlFieldNameMappingIgnoreCase();
            if (null != dicP)
            {
                PropertyInfo p = dicP[propertyName];
                if (null != p)
                {
                    primaryKeyValue = p.GetValue(this, null);
                }
            }
            return primaryKeyValue;
        }
        /// <summary>
        /// 指定数据表对应的实体
        /// </summary>
        private static Dictionary<string, Type> tableEntityType = new Dictionary<string, Type>();
        /// <summary>
        /// 实体对应的表名
        /// </summary>
        public static Dictionary<Type, string> entityTableName = new Dictionary<Type, string>();
        public static string GetTableName<T>()
        {
            return GetTableName(typeof(T));
        }
        public static string GetTableName(Type type)
        {
            string customTableName = entityTableName.ThreadSafeRead<Type, string>(type, "entityTableName");
            if (string.IsNullOrEmpty(customTableName))
            {
                TableAttribute[] atts = Attribute.GetCustomAttributes(type, typeof(TableAttribute))
                .Cast<TableAttribute>()
                .ToArray();
                if (atts.Length > 0)
                {
                    customTableName = atts[0].Name;
                }
                else
                {
                    customTableName = type.Name;
                }
                //表名+名称空间+类名，防止1.因类继承导致字段集合不一致，2.子类和父类类名相同导致的类型匹配错误
                customTableName = customTableName + "_" + type.Namespace + "_" + type.Name;
            }
            tableEntityType.ThreadSafeWrite<string, Type>(customTableName, type, "tableEntityType");
            return customTableName;
        }
        public static void SetTableName<T>(string tableName)
        {
            Type type = typeof(T);
            entityTableName.ThreadSafeWrite(type, tableName + "_" + type.Namespace + "_" + type.Name, "entityTableName");
        }

        private static A[] GetPropertyAttributes<A>(PropertyInfo property) where A : Attribute
        {
            A[] atts = null;
            if (property.CanRead)
            {
                atts = (A[])property.GetCustomAttributes(typeof(A), false);
            }

            return atts;
        }
        #endregion

        public void Initialize(Dictionary<string, object> reader)
        {
            //Type type = this.GetType();

            Dictionary<string, PropertyInfo> sqlFieldNameMapping = getSqlFieldNameMappingIgnoreCase();

            foreach (string key in reader.Keys)
            {
                if (key.Equals("rownum"))
                    continue;
                PropertyInfo info;
                sqlFieldNameMapping.TryGetValue(key, out info);

                object value = reader[key];
                if (info != null)
                {
                    if (value != null && value != DBNull.Value)
                        info.SetValue(this, value, null);
                }
                else
                {
                    // save undefined fields to values
                    columns[key] = value;
                }
            }
        }

        public void Initialize(XmlNode node)
        {
            Dictionary<string, PropertyInfo> sqlFieldNameMapping = getSqlFieldNameMappingIgnoreCase();

            foreach (XmlNode child in node.ChildNodes)
            {
                PropertyInfo info;
                sqlFieldNameMapping.TryGetValue(child.Name, out info);
                if (info != null)
                {
                    object value = getTypeValue(info.PropertyType, child.InnerXml);

                    if (value != null)
                    {
                        try
                        {
                            info.SetValue(this, value, null);
                        }
                        catch (Exception e)
                        {
                            throw new Exception("Error when setting: " + info.PropertyType.Name + ", " + child.Name + " = " + child.InnerText, e.InnerException);
                        }
                    }
                }
                else
                {
                    // save undefined fields to values
                    columns[child.Name] = child.InnerText;
                }
            }
        }

        public void Initialize(XmlAttributeCollection attributes)
        {
            Dictionary<string, PropertyInfo> sqlFieldNameMapping = getSqlFieldNameMappingIgnoreCase();

            foreach (XmlAttribute attribute in attributes)
            {
                PropertyInfo info;
                sqlFieldNameMapping.TryGetValue(attribute.Name, out info);

                if (info != null)
                {
                    object value = getTypeValue(info.PropertyType, attribute.Value);
                    if (value != null)
                        info.SetValue(this, value, null);
                }
                else
                {
                    // save undefined fields to values
                    columns[attribute.Name] = attribute.Value;
                }
            }
        }

        internal static object getTypeValue(Type type, string value)
        {
            object returnValue = null;

            switch (type.Name.ToLower())
            {
                case "int64":
                    returnValue = long.Parse(value);
                    break;
                case "byte":
                    returnValue = byte.Parse(value);
                    break;
                case "int32":
                case "int16":
                    returnValue = int.Parse(value);
                    break;
                case "decimal":
                    returnValue = Convert.ToDecimal(value);
                    break;
                case "double":
                    returnValue = Convert.ToDouble(value);
                    break;
                case "datetime":
                    returnValue = Convert.ToDateTime(value);
                    break;
                case "boolean":
                    returnValue = Convert.ToBoolean(Convert.ToInt32(value));
                    break;
                case "string":
                    returnValue = value;
                    break;
                default:
                    if (type.IsValueType)
                    {
                        returnValue = value;
                    }
                    break;
            }

            return returnValue;

        }

        /// <summary>
        /// Perform a deep Copy of the object.
        /// </summary>
        /// <typeparam name="T">The type of object being copied.</typeparam>
        /// <param name="source">The object instance to copy.</param>
        /// <returns>The copied object.</returns>

        public T Clone<T>() where T : BaseTO
        {
            if (this.GetType() != typeof(T))
            {
                throw new Exception("Invalid clone");
            }

            // Don't serialize a null object, simply return the default for that object
            if (Object.ReferenceEquals(this, null))
            {
                return default(T);
            }

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new MemoryStream();
            using (stream)
            {
                formatter.Serialize(stream, this);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }
        }

        private Dictionary<string, object> dictionary = null;

        private Dictionary<string, object> getDictionary()
        {
            if (dictionary == null)
            {
                dictionary = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
                foreach (PropertyInfo info in getSqlFieldNameMappingIgnoreCase().Values)
                {
                    if (info.CanRead)
                    {
                        dictionary.Add(info.Name, info.GetValue(this, null));
                    }
                }


                foreach (string key in columns.Keys)
                {
                    if (!dictionary.ContainsKey(key))
                    {
                        dictionary.Add(key, columns[key]);
                    }
                }

                foreach (string key in dictionary.Keys)
                {
                    if (dictionary[key] is BaseTO)
                    {
                        dictionary[key] = ((BaseTO)dictionary[key]).AsDictionary();
                    }
                }
            }
            return dictionary;
        }

        public Dictionary<string, object> AsDictionary()
        {
            return getDictionary();
        }
    }

    public enum EnumDBFieldUsage
    {
        /// <summary>
        /// Undefind
        /// </summary>
        None = 0,
        /// <summary>
        /// PrimaryKey
        /// </summary>
        PrimaryKey = 1,
        /// <summary>
        /// UniqueKey
        /// </summary>
        UniqueKey = 2,
        /// <summary>
        /// PrimaryKey
        /// </summary>
        CombinPrimaryKey = 3
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class SQLFieldAttribute : Attribute
    {
        public string Name { get; set; }
        public EnumDBFieldUsage FieldUsage { get; set; }
        public bool IsIdentify { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">DB field name</param>
        public SQLFieldAttribute(string name)
        {
            this.Name = name;
            this.FieldUsage = EnumDBFieldUsage.None;
            this.IsIdentify = false;
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">DB field name</param>
        /// <param name="fieldProperty">Field property</param>
        public SQLFieldAttribute(string name, EnumDBFieldUsage fieldUsage)
        {
            this.Name = name;
            this.FieldUsage = fieldUsage;
            this.IsIdentify = false;
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">DB field name</param>
        /// <param name="fieldProperty">Field property</param>
        /// <param name="isIdentify">is Identify field</param>
        public SQLFieldAttribute(string name, EnumDBFieldUsage fieldUsage, bool isIdentify)
        {
            this.Name = name;
            this.FieldUsage = fieldUsage;
            this.IsIdentify = isIdentify;
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class SQLIgnoreAttribute: Attribute
    {
        public SQLIgnoreAttribute()
        {
        }

    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class SQLReadOnlyAttribute : Attribute
    {
        public SQLReadOnlyAttribute()
        {
        }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class MaxLengthAttribute : Attribute
    {
        public int MaxLength { get; set; }
        public MaxLengthAttribute(int length)
        {
            this.MaxLength = length;
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class TableAttribute : Attribute
    {
        public string Name { get;set; }
        public TableAttribute(string name)
        {
            this.Name = name;
        }
    }
}
