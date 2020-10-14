using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Configuration.EnvironmentVariables;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace cloudworkApi.DataManagers
{
    public class GridManager
    {
        public GridManager()
        {
        }
    }
    public class Grid
    {
        public Grid()
        {
            //dataSourceModel = value;
        }
        private Int32 _maximumRows = 10; // -1 means return all row.
        public Int32 MaximumRows
        {
            get
            {
                return _maximumRows;
            }
            set
            {
                _maximumRows = value;
            }
        }
        public string Criteria = "";
        public string OrderBy = "";
        public Int32 Count = 0;
        public List<FilterParam> FilterParams { get; set; }
        private Int32 _page = 1; // current page number
        public Int32 Page { get { return _page; } set { if (value < 1) throw new Exception("გვერდის ნომერი უნდა იყოს 1 ზე მეტი"); _page = value; } }
        public string view { get; set; }
        public string dsViewName = string.Empty;
        public Dictionary<string,object> GetData<T>() where T : class, new()
        {
            List<T> list = new List<T>();
            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("Rows",list);
            if (dsViewName.Trim().Length == 0 || this.dsViewName.Contains(" ")) return dict;

            T dataSource = new T();

          //  foreach(PropertyInfo prop in dataSource.GetType().GetProperties())
          //  {
                var reader = this.GetDataFromDb(dataSource);
            
            while(reader.Read())
            {
                var ds = new T();
                this.Count = (int)reader["Count"];
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    PropertyInfo dsProp;
                    try
                    {
                        dsProp = dataSource.GetType().GetProperty(reader.GetName(i).ToString(), BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                        if (dsProp == null) continue;
                        
                        var value = reader[reader.GetName(i)];
                        var type = reader.GetFieldType(i);
                        if (type == typeof(int))
                            dsProp.SetValue(ds, Convert.ToInt32(value));
                        else if (type == typeof(string))
                            dsProp.SetValue(ds, Convert.ToString(value));
                        else if (type == typeof(DateTime))
                        {
                            dsProp.SetValue(ds, Convert.ToDateTime(value));
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
                list.Add(ds);
            }
            //}
            dict.Add("Count", this.Count);
            return dict;
        }
        private IDataReader GetDataFromDb(object dataSource)
        {
            var connString = dataSource.GetType().GetProperty("connectionString",BindingFlags.IgnoreCase | BindingFlags.NonPublic | BindingFlags.Instance).GetValue(dataSource).ToString();
            SqlConnection connection = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connection;
            cmd.CommandType = System.Data.CommandType.Text;

            var criteria = "";
            var filters = "";
            var orderBy = "";

            if (this.OrderBy.Trim().Length > 0)
                orderBy = this.OrderBy;

            if (!String.IsNullOrEmpty(this.Criteria))
                criteria = " " + this.Criteria.Replace("WHERE","");
            if (this.FilterParams != null)
            {
                foreach (var x in this.FilterParams)
                {
                    if (!string.IsNullOrWhiteSpace(filters)) filters += " AND ";

                    filters += string.Format("{0}{1}{2}", x.FieldName, x.FilterTypeSymbol(), "@" + x.FieldName);
                    if (x.FilterType == FilterType.Contains)
                    {
                        x.DataType = DataType.String;
                        cmd.Parameters.AddWithValue("@" + x.FieldName, "%" + x.FilterValue.ToString() + "%").SqlDbType = x.TypeToSqlDbType();
                    }
                    else
                        cmd.Parameters.AddWithValue("@" + x.FieldName, x.FilterValue).SqlDbType = x.TypeToSqlDbType();
                }
            }
            if (this.dsViewName.Trim().Length == 0 || this.dsViewName.Contains(" ")) 
                throw new Exception("Please provide ViewName");

                string criteriaWithFilters = filters.Trim().Length > 0 ? " AND (" + filters + ")" : "";
                criteriaWithFilters += criteria.Trim().Length > 0 ? " AND " + criteria + "" : "";

                var orderBySql = orderBy.Trim().Length > 0 ? "ORDER BY " + orderBy : "ORDER BY (SELECT NULL)";


                var endRowIndex = this.Page * this.MaximumRows;
                var startRowIndex = (endRowIndex - this.MaximumRows) + 1; // + 1 იმიტომ რომ between 1 and 4 მუშაობს 4 ის ჩათვლით და მეორე გვერდზე რო გადავალ წინაზე რაც ბოლო ჩანაწერი იყო ის აღარ წამოვიდეს
                var rangeSql = "WHERE RowNum BETWEEN " + startRowIndex + " AND " + endRowIndex;

                cmd.CommandText = String.Format("select * from (select (select COUNT(*) from Users) Count, *, ROW_NUMBER() OVER({3}) AS RowNum from {0} WHERE 1=1 {1}) as t {2}", this.dsViewName,criteriaWithFilters,rangeSql,orderBySql);
                cmd.Parameters.AddWithValue("viewName", this.dsViewName);
                connection.Open();
                //cmd.ExecuteNonQuery();
                var list = new List<IDataRecord>();
                SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                return reader;
                //while (reader.Read())
                //{
                //list.Add(reader);
                //}
                connection.Close();
            }
    }
    public class FilterParam 
    {
        public string FieldName { get; set; }
        public string FilterValue { get; set; }
        public DataType DataType { get; set; }
        public FilterType FilterType { get; set; }
        public string FilterTypeSymbol()
        {
            switch (FilterType) {
                case FilterType.Equal: {
                        return "=";
                    }
                case FilterType.Contains: {
                        return " LIKE ";
                }
                case FilterType.Greater:
                    {
                        return ">";
                    }
                case FilterType.GreaterOrEq:
                    {
                        return ">=";
                    }
                case FilterType.Less:
                    {
                        return "<";
                }
                case FilterType.LessOrEq:
                    {
                        return "<=";
                }
                case FilterType.NotEqual:
                {
                        return "<>";
                }
            }
            return "="; // by default it returns equal
        }
      public SqlDbType TypeToSqlDbType()
        {
            switch (DataType)
            {
                case DataType.String: return SqlDbType.NVarChar;
                case DataType.Number: return SqlDbType.Int;
                case DataType.Date: return SqlDbType.DateTime;
            }
            return SqlDbType.NVarChar;
        }
    }
    public class Filters
    {
        public List<FilterParam> filterParams { get; set; }
    }
    public enum FilterType { 
    Equal,
    Contains,
    Greater,
    GreaterOrEq,
    Less,
    LessOrEq,
    NotEqual
    }    
    public enum DataType {
    String,
    Number,
    Date
    }
}