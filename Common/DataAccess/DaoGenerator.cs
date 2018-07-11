using System;
using System.Data;
using System.IO;
using System.Collections;

namespace Common.DataAccess
{
    public static class DaoGenerator
    {
        private static String DaoDirPath = AppDomain.CurrentDomain.BaseDirectory + @"\";
        public static String SQLServerKeyword = "ADD,EXTERNAL,PROCEDURE,ALL,FETCH,PUBLIC,ALTER,FILE,RAISERROR,AND,FILLFACTOR,READ,ANY,FOR,READTEXT,AS,FOREIGN,RECONFIGURE,ASC,FREETEXT,REFERENCES,AUTHORIZATION,FREETEXTTABLE,REPLICATION,BACKUP,FROM,RESTORE,BEGIN,FULL,RESTRICT,BETWEEN,FUNCTION,RETURN,BREAK,GOTO,REVERT,BROWSE,GRANT,REVOKE,BULK,GROUP,RIGHT,BY,HAVING,ROLLBACK,CASCADE,HOLDLOCK,ROWCOUNT,CASE,IDENTITY,ROWGUIDCOL,CHECK,IDENTITY_INSERT,RULE,CHECKPOINT,IDENTITYCOL,SAVE,CLOSE,IF,SCHEMA,CLUSTERED,IN,SECURITYAUDIT,COALESCE,INDEX,SELECT,COLLATE,INNER,SEMANTICKEYPHRASETABLE,COLUMN,INSERT,SEMANTICSIMILARITYDETAILSTABLE,COMMIT,INTERSECT,SEMANTICSIMILARITYTABLE,COMPUTE,INTO,SESSION_USER,CONSTRAINT,IS,SET,CONTAINS,JOIN,SETUSER,CONTAINSTABLE,KEY,SHUTDOWN,CONTINUE,KILL,SOME,CONVERT,LEFT,STATISTICS,CREATE,LIKE,SYSTEM_USER,CROSS,LINENO,TABLE,CURRENT,LOAD,TABLESAMPLE,CURRENT_DATE,MERGE,TEXTSIZE,CURRENT_TIME,NATIONAL,THEN,CURRENT_TIMESTAMP,NOCHECK,CURRENT_USER,NONCLUSTERED,TOP,CURSOR,NOT,TRAN,DATABASE,NULL,TRANSACTION,DBCC,NULLIF,TRIGGER,DEALLOCATE,OF,TRUNCATE,DECLARE,OFF,TRY_CONVERT,DEFAULT,OFFSETS,TSEQUAL,DELETE,ON,UNION,DENY,OPEN,UNIQUE,DESC,OPENDATASOURCE,UNPIVOT,DISK,OPENQUERY,UPDATE,DISTINCT,OPENROWSET,UPDATETEXT,DISTRIBUTED,OPENXML,USE,DOUBLE,OPTION,USER,DROP,VALUES,DUMP,ORDER,VARYING,ELSE,OUTER,VIEW,END,OVER,WAITFOR,ERRLVL,PERCENT,WHEN,ESCAPE,PIVOT,WHERE,EXCEPT,PLAN,WHILE,EXEC,PRECISION,EXECUTE,PRIMARY,WITHIN GROUP,EXISTS,PRINT,WRITETEXT,EXIT,PROC,";

        //public static void doGenClass(String DbName, string ConnectionString, string GenTableNames = "")
        //{
        //    if (GenTableNames == "")
        //    {
        //        Hashtable ColNames = new System.Collections.Hashtable();
        //        ColNames.Add("name", "name");
        //        DataTable dt = Dao.SelectFn("dbo.uvSmsPlatformTables", ColNames, null, ConnectionString);
        //        foreach (DataRow R in dt.Rows)
        //            GenTableNames += R["name"].ToString() + "-";
        //    }

        //    string[] tables = GenTableNames.TrimEnd('-').Split('-');

        //    Hashtable ht = new Hashtable();

        //    String FilePath = DaoDirPath + DbName + ".xml";

        //    if (!Directory.Exists(Path.GetDirectoryName(FilePath)))
        //        Directory.CreateDirectory(Path.GetDirectoryName(FilePath));

        //    if (File.Exists(FilePath))
        //    {
        //        File.Delete(FilePath);
        //    }

        //    string COLNAMESSS = "";
        //    foreach (String tablename in tables)
        //    {
        //        Hashtable values = new Hashtable();
        //        values.Add("TableName", tablename);
        //        DataTable dt = Dao.SelectUSP("dbo.uspGetTableColumnsByName", values, null, ConnectionString);
        //        string COLNAME = "";
        //        foreach (DataRow row in dt.Rows)
        //        {
        //            String CNAME = row["COLUMN_NAME"].ToString();
        //            if (SQLServerKeyword.IndexOf(CNAME.ToUpper() + ",") > -1)
        //            {
        //                COLNAME += "[" + row["COLUMN_NAME"] + "],";
        //            }
        //            else
        //            {
        //                COLNAME += row["COLUMN_NAME"] + ",";
        //            }
        //        }

        //        COLNAMESSS += "Tables.Add(\"" + tablename + "\", \"" + COLNAME.TrimEnd(',') + "\".Split(','));\n";
        //    }

        //    using (StreamWriter sw = new StreamWriter(FilePath, true, System.Text.Encoding.UTF8))
        //    {
        //        sw.WriteLine(COLNAMESSS);
        //        sw.Flush();
        //        sw.Close();
        //    }
        //}

        public static void doGenSettingConfig(String DbName, string ConnectionString, string GenTableNames = "")
        {
            if (GenTableNames == "")
            {
                Hashtable ColNames = new System.Collections.Hashtable();
                ColNames.Add("name", "name");
                DataTable dt = new DataTable();//= Dao.SelectFn("dbo.uvSmsPlatformTables", ColNames, null, ConnectionString);
                //uvSmsPlatformTables =select
                //distinct name
                //from sys.tables WITH(NOLOCK) 
                //where name not in ('sysdiagrams')
                foreach (DataRow R in dt.Rows)
                    GenTableNames += R["name"].ToString() + "-";
            }

            string[] tables = GenTableNames.TrimEnd('-').Split('-');

            Hashtable ht = new Hashtable();

            String FilePath = DaoDirPath + DbName + ".xml";

            if (!Directory.Exists(Path.GetDirectoryName(FilePath)))
                Directory.CreateDirectory(Path.GetDirectoryName(FilePath));

            if (File.Exists(FilePath))
            {
                File.Delete(FilePath);
            }

            string XML = "";
            foreach (String tablename in tables)
            {
                Hashtable values = new Hashtable();
                values.Add("TableName", tablename);
                DataTable dt = Dao.SelectUSP("dbo.uspGetTableColumnsByName", values, null, ConnectionString);
                string COLNAME = "";
                foreach (DataRow row in dt.Rows)
                {
                    String CNAME = row["COLUMN_NAME"].ToString();
                    if (SQLServerKeyword.IndexOf(CNAME.ToUpper() + ",") > -1)
                    {
                        COLNAME += "[" + row["COLUMN_NAME"] + "],";
                    }
                    else
                    {
                        COLNAME += row["COLUMN_NAME"] + ",";
                    }
                }
                XML += "<" + DbName + "-" + tablename + ">" + COLNAME.TrimEnd(',') + "</" + DbName + "-" + tablename + ">\n";
            }
            XML = "<Tools.DataAccess>" + XML + "</Tools.DataAccess>";
            using (StreamWriter sw = new StreamWriter(FilePath, true, System.Text.Encoding.UTF8))
            {
                sw.WriteLine(XML);
                sw.Flush();
                sw.Close();
            }
        }
    }
}