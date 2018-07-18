using System;
using System.Collections;
using System.Data.SqlClient;
using System.Data;
using Common.Extensions;
using Common.Tools;

namespace Common.DataAccess
{
    public class Dao
    {
        #region "Infrastructure Method"
        public static Hashtable Tables = new Hashtable();
        // public static List<DataBase> DataBases = new List<DataBase>();
        static Dao()
        {
            LoadTables();
        }

        /// <summary>
        /// LoadTable Info
        /// </summary>
        public static void LoadTables()
        {
            lock (Tables)
            {
                Tables.Clear();
                foreach (String Key in Setting.GetCategory("Package", "Tools.DataAccess").Keys)
                {
                    Tables.AddItem(Key,
                        Setting.GetCategory("Package", "Tools.DataAccess").GetString(Key).Split(',')
                    );
                }
            }
        }
        #endregion

        #region "Basis USP Method"
        /// <summary>
        /// Edit USP Row Data (Insert/Update/Delete)
        /// </summary>
        /// <param name="ProName">Pro Name</param>
        /// <param name="Conditions">@ColName, string[2]{Value,@ColName}</param>
        /// <param name="ConnectionString">Connection String</param>
        /// <returns>rows affected</returns>
        public static int EditUSP(string ProName, Hashtable Conditions, string ConnectionString)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = ProName;
                    cmd.CommandTimeout = 0;

                    if (Conditions != null && Conditions.Count >= 0)
                    {
                        foreach (String ConditionKey in Conditions.Keys)
                        {
                            cmd.Parameters.AddWithValue("@" + ConditionKey, Conditions.GetString(ConditionKey));
                        }
                    }

                    int outputData = Convert.ToInt32(cmd.ExecuteScalar());
                    return outputData;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("[Dao.EditUSP]" + ex.Message);
            }
        }

        /// <summary>
        /// Edit USP Row Data (Insert/Update/Delete)
        /// </summary>
        /// <param name="ProName">Pro Name</param>
        /// <param name="cmd">SqlCommand Parameters</param>
        /// <param name="ConnectionString">Connection String</param>
        /// <returns>rows affected</returns>
        /*
        public static int EditUSP(string ProName, SqlCommand cmd, string ConnectionString)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = ProName;
                    cmd.CommandTimeout = 0;

                    int outputData = Convert.ToInt32(cmd.ExecuteScalar());
                    return outputData;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("[Dao.EditUSP]" + ex.Message);
            }
        }
        */
        /// <summary>
        /// Select USP Row Data
        /// </summary>
        /// <param name="ProName">Pro Name</param>
        /// <param name="Conditions">Conditions Element</param>
        /// <param name="Orders">Order By Element</param>
        /// <param name="ConnectionString">Connection String</param>
        /// <returns>DataTable</returns>
        public static DataTable SelectUSP(string ProName, Hashtable Conditions, Hashtable Orders, string ConnectionString)
        {
            try
            {
                String Order = "";
                if (Orders != null && Orders.Count >= 0)
                    foreach (String OrderKey in Orders.Keys)
                        Order += Orders.GetString(OrderKey) + ",";

                DataTable dt = new DataTable();
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = ProName;
                    cmd.CommandTimeout = 0;

                    if (Conditions != null && Conditions.Count >= 0)
                    {
                        foreach (String ConditionKey in Conditions.Keys)
                        {
                            cmd.Parameters.AddWithValue("@" + ConditionKey, Conditions.GetString(ConditionKey));
                        }
                    }
                    if (!String.IsNullOrWhiteSpace(Order))
                        cmd.Parameters.AddWithValue("@OrderBy", Order.TrimEnd(','));

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                    return dt;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("[Dao.SelectUSP]" + ex.Message);
            }
        }

        /// <summary>
        /// Select USP Row Data
        /// </summary>
        /// <param name="ProName">Pro Name</param>
        /// <param name="cmd">SqlCommand Parameters</param>
        /// <param name="ConnectionString">Connection String</param>
        /// <returns>DataTable</returns>
        public static DataTable SelectUSP(string ProName, SqlCommand cmd, string ConnectionString)
        {
            try
            {
                DataTable dt = new DataTable();
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = ProName;
                    cmd.CommandTimeout = 0;

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                    return dt;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("[Dao.SelectUSP]" + ex.Message);
            }
        }

        #endregion

        #region "因為和DBA政策，TSQL全改成走SP，故mark此區塊"

        #region "Basis Method"
        ///// <summary>
        ///// Insert Row Data
        ///// </summary>
        ///// <param name="DataBaseName">DataBase Name</param>
        ///// <param name="TableName">Table Name</param>
        ///// <param name="Values">@ColName, string[2]{Value,@ColName}</param>
        ///// <param name="ConnectionString">Connection String</param>
        ///// <param name="Schema">Schema(default :dbo)</param>
        ///// <param name="ResultIdentity">Return Identity(default :false)</param>
        ///// <returns>rows affected</returns>
        //public static int Insert(string DataBaseName, string TableName, Hashtable Values, string ConnectionString, string Schema = "dbo", bool ResultIdentity = false)
        //{
        //    try
        //    {
        //        IsExistTable(DataBaseName, TableName);

        //        String[] ColumnNames = Tables.GetStrings(DataBaseName + "-" + TableName);

        //        String COLS = "";
        //        String VALS = "";

        //        ArrayList SQLParameter = new ArrayList();

        //        for (int i = 0; i < ColumnNames.Length; i++)
        //        {
        //            if (!Values.ContainsKey(ColumnNames[i]))
        //                continue;

        //            Object Value = "";
        //            String Column = ColumnNames[i];
        //            String ColumnNoBoard = ColumnNames[i].Replace("[", "").Replace("]", ""); //無邊框

        //            if (typeof(String[]) == Values[Column].GetType())
        //            {
        //                Value = Values.GetStrings(Column)[0];
        //                Column = Values.GetStrings(Column)[1];
        //            }
        //            else
        //            {
        //                Value = Values[Column];
        //                Column = "@" + ColumnNoBoard;
        //            }

        //            SQLParameter.Add(new SqlParameter(Column, Value));
        //            COLS += ColumnNames[i] + ",";
        //            VALS += Column + ",";
        //        }
        //        COLS = COLS.TrimEnd(',');
        //        VALS = VALS.TrimEnd(',').Replace("[", "").Replace("]", "");

        //        String TSQL = InsertSql.Replace("<Schema>", Schema).Replace("<TABLE>", TableName).Replace("<COLS>", COLS).Replace("<VALS>", VALS);
        //        if (!ResultIdentity)
        //            return Execute(TSQL, SQLParameter, ConnectionString);
        //        else
        //            return ExecuteResultIdentity(TSQL, SQLParameter, ConnectionString);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("[DAO.Insert]" + ex.Message);
        //    }
        //}

        ///// <summary>
        ///// Update Row Data
        ///// </summary>
        ///// <param name="DataBaseName">DataBase Name</param>
        ///// <param name="TableName">Table Name</param>
        ///// <param name="Values">@ColName, string[2]{Value,@ColName}</param>
        ///// <param name="Conditions">@ColName, string[2]{Value,@ColName}</param>
        ///// <param name="ConnectionString">Connection String</param>
        ///// <param name="Nolock">With Nolock is Enable (default :true)</param>
        ///// <param name="Schema">Schema(default :dbo)</param>
        ///// <returns>rows affected</returns>
        //public static int Update(string DataBaseName, string TableName, Hashtable Values, Hashtable Conditions, string ConnectionString, bool Nolock = true, string Schema = "dbo")
        //{
        //    try
        //    {
        //        IsExistTable(DataBaseName, TableName);

        //        String[] ColumnNames = Tables.GetStrings(DataBaseName + "-" + TableName);

        //        String ColumnName = "";
        //        String Condition = "";

        //        ArrayList SQLParameter = GetSqlConditionsParameterList(ref Condition, Conditions, ColumnNames);
        //        for (int i = 0; i < ColumnNames.Length; i++)
        //        {
        //            if (Conditions.Contains(ColumnNames[i])) continue;
        //            String Column = ColumnNames[i];
        //            String ColumnNoBoard = ColumnNames[i].Replace("[", "").Replace("]", ""); //無邊框

        //            if (Values.ContainsKey(Column))
        //            {
        //                Object Value = "";
        //                if (typeof(String[]) == Values[Column].GetType())
        //                {
        //                    Value = Values.GetStrings(Column)[0];
        //                    ColumnName += Values.GetStrings(Column)[1] + ",";
        //                }
        //                else
        //                {
        //                    Value = Values[Column];
        //                    ColumnName += Column + "=@" + ColumnNoBoard + ",";
        //                }
        //                SQLParameter.Add(new SqlParameter("@" + ColumnNoBoard, Value));
        //            }
        //        }

        //        ColumnName = ColumnName.TrimEnd(',');

        //        String SqlCommand = UpdateNolockSql.Replace("<Schema>", Schema).Replace("<TABLE>", TableName).Replace("<COL&VAL>", ColumnName);
        //        if (!Nolock)
        //            SqlCommand = UpdateSql.Replace("<Schema>", Schema).Replace("<TABLE>", TableName).Replace("<COL&VAL>", ColumnName);

        //        if (Condition == "")
        //        {
        //            SqlCommand = SqlCommand.Replace("WHERE 0=0 <WHERE>", "");
        //        }
        //        else
        //        {
        //            SqlCommand = SqlCommand.Replace("<WHERE>", Condition);
        //        }
        //        return Execute(SqlCommand, SQLParameter, ConnectionString);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("[Dao.Update]" + ex.Message);
        //    }
        //}

        ///// <summary>
        ///// Delete Row Data
        ///// </summary>
        ///// <param name="DataBaseName">DataBase Name</param>
        ///// <param name="TableName">Table Name</param>
        ///// <param name="Conditions">@ColName, string[2]{Value,@ColName}</param>
        ///// <param name="ConnectionString">Connection String</param>
        ///// <param name="Schema">Schema(default :dbo)</param>
        ///// <returns>rows affected</returns>
        //public static int Delete(string DataBaseName, string TableName, Hashtable Conditions, string ConnectionString, bool Nolock = true, string Schema = "dbo")
        //{
        //    try
        //    {
        //        IsExistTable(DataBaseName, TableName);

        //        String[] ColumnNames = Tables.GetStrings(DataBaseName + "-" + TableName);

        //        String Condition = "";

        //        ArrayList SQLParameter = GetSqlConditionsParameterList(ref Condition, Conditions, ColumnNames);

        //        String SqlCommand = DeleteNolockSql.Replace("<Schema>", Schema).Replace("<TABLE>", TableName);
        //        if (!Nolock)
        //            SqlCommand = DeleteSql.Replace("<Schema>", Schema).Replace("<TABLE>", TableName);

        //        if (Condition == "")
        //        {
        //            SqlCommand = SqlCommand.Replace("WHERE 0=0 <WHERE>", "");
        //        }
        //        else
        //        {
        //            SqlCommand = SqlCommand.Replace("<WHERE>", Condition);
        //        }

        //        return Execute(SqlCommand, SQLParameter, ConnectionString);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("[Dao.Delete]" + ex.Message);
        //    }
        //}

        ///// <summary>
        ///// Select Row Data
        ///// </summary>
        ///// <param name="DataBaseName">DataBase Name</param>
        ///// <param name="TableName">Table Name</param>
        ///// <param name="ColNames">Col Name</param>
        ///// <param name="Conditions">Conditions Element</param>
        ///// <param name="Orders">Order By Element</param>
        ///// <param name="ConnectionString">Connection String</param>
        ///// <param name="Nolock">With Nolock is Enable (default :true)</param>
        ///// <param name="Distinct">Distinct (default :false)</param>
        ///// <param name="Top">Get Top RowData (default :0)(0:Get All)</param>
        ///// <param name="Schema">Schema(default :dbo)</param>
        ///// <returns>DataTable</returns>
        //public static DataTable Select(string DataBaseName, string TableName, Hashtable ColNames, Hashtable Conditions, Hashtable Orders, string ConnectionString, bool Nolock = true, bool Distinct = false, int Top = 0, string Schema = "dbo")
        //{
        //    try
        //    {
        //        IsExistTable(DataBaseName, TableName);

        //        String[] ColumnNames = Tables.GetStrings(DataBaseName + "-" + TableName);

        //        String SqlCommand = SelectSql.Replace("<Schema>", Schema).Replace("<TABLE>", TableName);
        //        if (!Nolock)
        //            SqlCommand = SqlCommand.Replace("WITH (NOLOCK)", "");

        //        if (!Distinct)
        //            SqlCommand = SqlCommand.Replace("<DISTINCT>", "");
        //        else
        //            SqlCommand = SqlCommand.Replace("<DISTINCT>", "Distinct");

        //        if (Top > 0)
        //            SqlCommand = SqlCommand.Replace("<TOP>", "Top " + Top);
        //        else
        //            SqlCommand = SqlCommand.Replace("<TOP>", "");

        //        String ColumnName = "";
        //        if (ColNames != null)
        //        {
        //            for (int i = 0; i < ColumnNames.Length; i++)
        //            {
        //                if (ColNames.ContainsKey(ColumnNames[i]))
        //                    ColumnName += ColNames[ColumnNames[i]] + ",";
        //            }
        //        }
        //        if (ColumnName == "")
        //        {
        //            SqlCommand = SqlCommand.Replace("<COLS>", String.Join(",", (Tables[TableName] as string[])));
        //        }
        //        else
        //        {
        //            SqlCommand = SqlCommand.Replace("<COLS>", ColumnName.TrimEnd(','));
        //        }

        //        String Condition = "";

        //        ArrayList SQLParameter = GetSqlConditionsParameterList(ref Condition, Conditions, ColumnNames);

        //        if (Condition == "")
        //        {
        //            SqlCommand = SqlCommand.Replace("WHERE 0=0 <WHERE>", "");
        //        }
        //        else
        //        {
        //            SqlCommand = SqlCommand.Replace("<WHERE>", Condition);
        //        }

        //        String Order = "";
        //        if (Orders != null && Orders.Count >= 0)
        //            foreach (String OrderKey in Orders.Keys)
        //                Order += Orders.GetString(OrderKey) + ",";

        //        if (!String.IsNullOrWhiteSpace(Order))
        //            SqlCommand += " ORDER BY  " + Order.TrimEnd(',');


        //        DataTable DT = new DataTable();
        //        using (SqlConnection conn = new SqlConnection(ConnectionString))
        //        {
        //            conn.Open();
        //            SqlCommand MyCommand = new SqlCommand(SqlCommand, conn);
        //            MyCommand.CommandTimeout = 0;
        //            if (SQLParameter.Count > 0)
        //                MyCommand.Parameters.AddRange(SQLParameter.ToArray());


        //            using (SqlDataAdapter da = new SqlDataAdapter(MyCommand))
        //            {
        //                da.Fill(DT);
        //            }
        //            return DT;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("[Dao.Select]" + ex.Message);
        //    }
        //}

        ///// <summary>
        ///// Exist Data in Table
        ///// </summary>
        ///// <param name="DataBaseName">DataBase Name</param>
        ///// <param name="TableName">Table Name</param>
        ///// <param name="Conditions">Conditions Element</param>
        ///// <param name="ConnectionString">Connection String</param>
        ///// <param name="Nolock">With Nolock is Enable (default :true)</param>
        ///// <param name="Schema">Schema(default :dbo)</param>
        ///// <returns></returns>
        //public static bool IsExist(string DataBaseName, string TableName, Hashtable Conditions, string ConnectionString, bool Nolock = true, string Schema = "dbo")
        //{
        //    try
        //    {

        //        IsExistTable(DataBaseName, TableName);

        //        String[] ColumnNames = Tables.GetStrings(DataBaseName + "-" + TableName);

        //        String SqlCommand = IsExistSql.Replace("<Schema>", Schema).Replace("<TABLE>", TableName);
        //        if (!Nolock)
        //            SqlCommand = SqlCommand.Replace("WITH (NOLOCK)", "");

        //        String Condition = "";
        //        ArrayList SQLParameter = GetSqlConditionsParameterList(ref Condition, Conditions, ColumnNames);

        //        if (Condition == "")
        //            SqlCommand = SqlCommand.Replace("WHERE 0=0 <WHERE>", "");
        //        else
        //            SqlCommand = SqlCommand.Replace("<WHERE>", Condition);

        //        DataTable DT = new DataTable();
        //        using (SqlConnection conn = new SqlConnection(ConnectionString))
        //        {
        //            conn.Open();
        //            SqlCommand MyCommand = new SqlCommand(SqlCommand, conn);
        //            MyCommand.CommandTimeout = 0;
        //            if (SQLParameter.Count > 0)
        //                MyCommand.Parameters.AddRange(SQLParameter.ToArray());
        //            using (SqlDataAdapter da = new SqlDataAdapter(MyCommand))
        //            {
        //                da.Fill(DT);
        //            }
        //            return (int.Parse(DT.Rows[0]["C"].ToString()) > 0);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("[Dao.IsExist]" + ex.Message);
        //    }
        //}

        ///// <summary>
        ///// Insert By Not Exist , Update By Exist .
        ///// </summary>
        ///// <param name="DataBaseName">DataBase Name</param>
        ///// <param name="TableName">Table Name</param>
        ///// <param name="UpdateValues">Update Values</param>
        ///// <param name="InsertValues">Insert Values</param>
        ///// <param name="Conditions">Conditions Element</param>
        ///// <param name="ConnectionString">Connection String</param>
        ///// <param name="Nolock">With Nolock is Enable (default :true)</param>
        ///// <param name="Schema">Schema(default :dbo)</param>
        ///// <returns></returns>
        //public static int UpdateOrInsert(string DataBaseName, string TableName, Hashtable UpdateValues, Hashtable InsertValues, Hashtable Conditions, string ConnectionString, bool Nolock = true, string Schema = "dbo")
        //{
        //    try
        //    {
        //        int result = 0;
        //        if (IsExist(DataBaseName, TableName, Conditions, ConnectionString, Nolock))
        //        {
        //            result = Update(DataBaseName, TableName, UpdateValues, Conditions, ConnectionString, Nolock);
        //        }
        //        else
        //        {
        //            result = Insert(DataBaseName, TableName, InsertValues, ConnectionString, Schema);
        //        }
        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("[Dao.UpdateOrInsert]" + ex.Message);
        //    }
        //}
        #endregion

        #region "Customized Method"
        ///// <summary>
        /////  Query DataTable 
        ///// </summary>
        ///// <param name="SqlCommand">SQL</param>
        ///// <param name="Conditions">Conditions</param>
        ///// <param name="ConnectionString">Connection String</param>
        ///// <returns>DataTable</returns>
        public static DataTable QueryDataTable(string SqlCommand, Hashtable Conditions, string ConnectionString)
        {
            DataTable DT = new DataTable();
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    SqlCommand MyCommand = new SqlCommand(SqlCommand, conn);
                    MyCommand.CommandTimeout = 0;
                    if (Conditions != null)
                    {
                        foreach (string key in Conditions.Keys)
                            MyCommand.Parameters.AddWithValue(key, Conditions[key]);
                    }

                    using (SqlDataAdapter da = new SqlDataAdapter(MyCommand))
                    {
                        da.Fill(DT);
                    }
                    return DT;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("[Dao.QueryDataTable]" + ex.Message);
            }
        }




        /// <summary>
        /// single sql command execution
        /// </summary>
        /// <param name="sqlcommand">sql command</param>
        /// <param name="parameters">parameters</param>
        /// <param name="connectionstring">connection string</param>
        /// <returns>rows affected</returns>
        public static int execute(string sqlcommand, ArrayList parameters, string connectionstring)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionstring))
                {
                    conn.Open();

                    SqlCommand mycommand = new SqlCommand(sqlcommand, conn);
                    mycommand.CommandTimeout = 0;
                    mycommand.Parameters.AddRange(parameters.ToArray());
                    return mycommand.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("[dao.execute]" + ex.Message);
            }
        }

        ///// <summary>
        ///// Multi-Sql Command execution
        ///// </summary>
        ///// <param name="SqlCommand">Sql Command</param>
        ///// <param name="Parameters">Parameters</param>
        ///// <param name="ConnectionString">Connection String</param>
        ///// <returns>rows affected</returns>
        //public static int Executes(string[] SqlCommands, Hashtable Parameters, string ConnectionString)
        //{
        //    int SuccessNum = 0;
        //    SqlTransaction tran;
        //    try
        //    {
        //        using (SqlConnection conn = new SqlConnection(ConnectionString))
        //        {
        //            conn.Open();

        //            SqlCommand command = conn.CreateCommand();

        //            // Start a local transaction.
        //            tran = conn.BeginTransaction("");
        //            command.Connection = conn;
        //            command.Transaction = tran;
        //            command.CommandTimeout = 0;

        //            try
        //            {

        //                for (int i = 0; i < SqlCommands.Length; i++)
        //                {

        //                    command.Parameters.Clear();
        //                    command.CommandText = SqlCommands[i];

        //                    if (Parameters != null)
        //                    {
        //                        foreach (string key in Parameters.Keys)
        //                            command.Parameters.AddWithValue(key, Parameters[key]);
        //                    }
        //                    SuccessNum += command.ExecuteNonQuery();
        //                }
        //            }
        //            catch (Exception ex2)
        //            {
        //                tran.Rollback();
        //                throw ex2;
        //            }

        //            tran.Commit();
        //        }

        //        return SuccessNum;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("[Dao.doExecutes]" + ex.Message);
        //    }
        //}

        ///// <summary>
        ///// Single Sql Command execution
        ///// </summary>
        ///// <param name="SqlCommand">Sql Command</param>
        ///// <param name="Parameters">Parameters</param>
        ///// <param name="ConnectionString">Connection String</param>
        ///// <returns>rows affected</returns>
        //public static int Execute(string SqlCommand, Hashtable Parameters, string ConnectionString)
        //{
        //    try
        //    {
        //        using (SqlConnection conn = new SqlConnection(ConnectionString))
        //        {
        //            conn.Open();

        //            SqlCommand command = conn.CreateCommand();
        //            command.Connection = conn;
        //            command.CommandTimeout = 0;
        //            command.Parameters.Clear();
        //            command.CommandText = SqlCommand;

        //            if (Parameters != null)
        //            {
        //                foreach (string key in Parameters.Keys)
        //                    command.Parameters.AddWithValue(key, Parameters[key]);
        //            }
        //            return command.ExecuteNonQuery();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("[Dao.doExecutes]" + ex.Message);
        //    }
        //}

        ///// <summary>
        ///// Single Sql Command execution
        ///// </summary>
        ///// <param name="SqlCommand">Sql Command</param>
        ///// <param name="Parameters">Parameters</param>
        ///// <param name="ConnectionString">Connection String</param>
        ///// <returns>rows Identity Number</returns>
        //public static int ExecuteResultIdentity(string SqlCommand, ArrayList Parameters, string ConnectionString)
        //{
        //    try
        //    {
        //        String _SqlCommand = SqlCommand.TrimEnd(';') + ";Select isnull(SCOPE_IDENTITY(),0) as sn";


        //        using (SqlConnection conn = new SqlConnection(ConnectionString))
        //        {
        //            conn.Open();

        //            SqlCommand MyCommand = new SqlCommand(_SqlCommand, conn);
        //            MyCommand.CommandTimeout = 0;
        //            MyCommand.Parameters.AddRange(Parameters.ToArray());
        //            return Convert.ToInt32(MyCommand.ExecuteScalar());
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("[Dao.ExecuteResultIdentity]" + ex.Message);
        //    }
        //    finally
        //    {

        //    }
        //}
        #endregion

        #region "運算式"
        //private static ArrayList GetSqlConditionsParameterList(ref String Condition, Hashtable Conditions, String[] ColumnNames)
        //{
        //    ArrayList SQLParameter = new ArrayList();
        //    if (Conditions != null)
        //    {
        //        for (int i = 0; i < ColumnNames.Length; i++)
        //        {
        //            if (!Conditions.ContainsKey(ColumnNames[i]))
        //                continue;

        //            Object Value = "";
        //            String Column = ColumnNames[i];
        //            String ColumnNoBoard = ColumnNames[i].Replace("[", "").Replace("]", ""); //無邊框
        //            if (typeof(String[]) == Conditions[Column].GetType())
        //            {
        //                Value = Conditions.GetStrings(Column)[0];
        //                Condition += " " + Conditions.GetStrings(Column)[1];
        //            }
        //            else
        //            {
        //                Value = Conditions[Column];
        //                Condition += " AND " + Column + "=@" + ColumnNoBoard;
        //            }
        //            SQLParameter.Add(new SqlParameter("@" + ColumnNoBoard, Value));
        //        }
        //    }
        //    return SQLParameter;
        //}
        #endregion

        #region 教學範例
        /*
        ==教學範例==
        Hashtable Colnames = new Hashtable();
        Hashtable UpdateValues = new Hashtable();
        Hashtable InsertValue = new Hashtable();
        Hashtable Conditions = new Hashtable();
        Hashtable Order = new Hashtable();
    */
        //●Update 
        /*
            values.Clear();
            where.Clear();
            values.Add("Value", DbParam.Get("Value11", "Value=@Value"));
            values.Add("ShowYN", DbParam.Get("N", "ShowYN=@ShowYN"));
            values.Add("Description", DbParam.Get("desc111", "Description=@Description"));
            values.Add("[Order]", DbParam.Get("2", "[Order]=@Order"));

            where.Add("Groups", DbParam.Get("ZZ", "And Groups=@Groups"));
            where.Add("Category", DbParam.Get("類別11", "And Category=@Category"));
            where.Add("Keys", DbParam.Get("值", "And Keys=@Keys"));
            Dao.Update("Configs", values, where, SettingServices.ConnectionString("Sms"));
        */

        //●Delete 
        /*
            where.Clear();
            where.Add("Groups", DbParam.Get("ZZ", "And Groups=@Groups"));
            where.Add("Category", DbParam.Get("類別11", "And Category=@Category"));
            where.Add("Keys", DbParam.Get("值", "And Keys=@Keys"));
            Dao.Delete("Configs", where, SettingServices.ConnectionString("Sms"),false);
        */

        //●Insert 
        /*
            values.Clear();
            values.Add("Groups", DbParam.Get("ZZ", "@Groups"));
            values.Add("Category", DbParam.Get("類別11", "@Category"));
            values.Add("Keys", DbParam.Get("值", "@Keys"));
            values.Add("Value", DbParam.Get("Value11", "@Value"));
            values.Add("ShowYN", DbParam.Get("N", "@ShowYN"));
            values.Add("Description", DbParam.Get("desc111", "@Description"));
            values.Add("[Order]", DbParam.Get("2", "@Order"));
            Dao.Insert("Configs", values, SettingServices.ConnectionString("Sms"));
        */

        //●UpdateOrInsert
        /*
            InsertValue.Clear();
            InsertValue.Add("Groups", DbParam.Get("ZZ", "@Groups"));
            InsertValue.Add("Category", DbParam.Get("類別11", "@Category"));
            InsertValue.Add("Keys", DbParam.Get("值", "@Keys"));

            InsertValue.Add("Value", DbParam.Get("InsertValue", "@Value"));
            InsertValue.Add("ShowYN", DbParam.Get("N", "@ShowYN"));
            InsertValue.Add("Description", DbParam.Get("新增資料", "@Description"));
            InsertValue.Add("[Order]", DbParam.Get("1", "@Order"));

            UpdateValues.Clear();
            UpdateValues.Add("Value", DbParam.Get("UpdaetValue", "Value=@Value"));
            UpdateValues.Add("ShowYN", DbParam.Get("Y", "ShowYN=@ShowYN"));
            UpdateValues.Add("Description", DbParam.Get("修改資料", "Description=@Description"));
            UpdateValues.Add("[Order]", DbParam.Get("2", "[Order]=@Order"));

            Conditions.Clear();
            Conditions.Add("Groups", DbParam.Get("ZZ", "And Groups=@Groups"));
            Conditions.Add("Category", DbParam.Get("類別11", "And Category=@Category"));
            Conditions.Add("Keys", DbParam.Get("值", "And Keys=@Keys"));
            Console.WriteLine("(UpdateOrInsert)異動筆數：{0}",
                Dao.UpdateOrInsert("Configs", UpdateValues, InsertValue, Conditions, SettingServices.ConnectionString("Sms"))
            );
        */

        //●IsExist 
        /*
            where.Clear();
            where.Add("Groups", DbParam.Get("ZZ", "And Groups=@Groups"));
            where.Add("Category", DbParam.Get("類別11", "And Category=@Category"));
            where.Add("Keys", DbParam.Get("值", "And Keys=@Keys"));
            Console.WriteLine("(IsExist)是否有發現資料：{0}",
                Dao.IsExist("Configs", where, SettingServices.ConnectionString("Sms"), false).ToString()
            );
        */

        //●Select 
        /*
            Order.Clear();
            Colnames.Clear();
            Conditions.Clear();

            Colnames.Add("Category", "Category as cat");
            Colnames.Add("Value", "Value as val");

            Conditions.Add("Category", DbParam.Get("PaidType", "And Category=Convert(nvarchar(10),@Category)"));
            Conditions.Add("[Order]", DbParam.GetLike("1", "[Order]", DbParam.LikeMode.Both));
            
            Order.Add("Category", "Category DESC");
            Order.Add("Value", "Value DESC");

            System.Data.DataTable DT = Dao.Select("Configs", Colnames, Conditions, Order, SettingServices.ConnectionString("Sms"),false);

            Console.WriteLine("===資料筆數:{0}===", DT.Rows.Count.ToString());

            String CName = "";
            foreach (DataColumn Cols in DT.Columns)
                CName += Cols.ColumnName + "|";

            Console.WriteLine(CName.TrimEnd('|'));

            foreach (DataRow Rows in DT.Rows)
            {
                String Values = "";
                foreach (DataColumn Cols in DT.Columns) {
                    Values += Rows[Cols.ColumnName] + "|";
                }
                Console.WriteLine(Values.TrimEnd('|'));
            }
            Console.WriteLine("===結束=========");
        */
        #endregion
        #endregion
    }
}
