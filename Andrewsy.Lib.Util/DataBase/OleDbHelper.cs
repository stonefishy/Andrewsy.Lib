using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.OleDb;

namespace Andrewsy.Lib.Util.DataBase
{
    //操作OleDb数据库助手类
    public abstract class OleDbHelper
    {
        //连接OleDb数据库字符串
        public static readonly string ConnString = ConfigurationManager.AppSettings["connectionString"];

        //创建一个哈希表参数缓存
        private static Hashtable _paramCach = Hashtable.Synchronized(new Hashtable());

        /// <summary>
        /// 执行数据查询
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="cmdType">命令类型 sql语句或存储过程</param>
        /// <param name="cmdText">sql命令</param>
        /// <param name="cmdParams">命令参数</param>
        /// <returns></returns>
        public static int ExecuteNonQuery(string connectionString, string cmdText, CommandType cmdType, params OleDbParameter[] cmdParams)
        {
            OleDbCommand cmd = new OleDbCommand();

            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                //准备过程参数
                PrepareCommand(conn, cmd, null, cmdType, cmdText, cmdParams);

                //执行
                int rtn = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                return rtn;
            }
        }

        /// <summary>
        /// 执行事务或命令
        /// </summary>
        /// <param name="trans">事物对象</param>
        /// <param name="cmdText">存储过程名或sql命令</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="cmdParams"></param>
        /// <returns></returns>
        public static int ExecuteNonQuery(OleDbTransaction trans,string cmdText,CommandType cmdType,params OleDbParameter [] cmdParams)
        {
            if (trans == null)
                throw new ArgumentNullException("transaction");

            if (trans != null && trans.Connection == null)
                throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");

            OleDbCommand cmd = new OleDbCommand();

            PrepareCommand(trans.Connection, cmd, trans, cmdType, cmdText, cmdParams);

            int rtn = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            return rtn;
        }

        /// <summary>
        /// 执行Sql 命令
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="cmdText"></param>
        /// <param name="cmdType"></param>
        /// <param name="cmdParams"></param>
        /// <returns></returns>
        public static int ExecuteNonQuery(OleDbConnection conn,string cmdText,CommandType cmdType,params OleDbParameter[] cmdParams)
        {
            OleDbCommand cmd = new OleDbCommand();

            PrepareCommand(conn, cmd, null, cmdType, cmdText, cmdParams);

            int rtn = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            return rtn;
        }

        /// <summary>
        /// 执行 返回一个结果集
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        /// <param name="cmdText">命令字符串</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="cmdParameters">参数</param>
        /// <returns></returns>
        public static OleDbDataReader ExecuteReader(string connectionString, string cmdText, CommandType cmdType, params OleDbParameter[] cmdParameters)
        {
            //创建连接
            OleDbConnection conn = new OleDbConnection(connectionString);
            OleDbCommand cmd = new OleDbCommand();

            try
            {
                PrepareCommand(conn, cmd, null, cmdType, cmdText, cmdParameters);

                //执行
                OleDbDataReader odr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
                return odr;
            }
            catch (System.Exception ex)
            {
                conn.Close();
                throw ex;
            }
        }

        /// <summary>
        /// 执行 返回一个结果集
        /// </summary>
        /// <param name="conn">链接对象</param>
        /// <param name="cmdText">命令字符串</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="cmdParameters">参数</param>
        /// <returns></returns>
        public static OleDbDataReader ExecuteReader(OleDbConnection conn, string cmdText, CommandType cmdType, params OleDbParameter[] cmdParams)
        {
            OleDbCommand cmd = new OleDbCommand();

            try
            {
                PrepareCommand(conn, cmd, null, cmdType, cmdText, cmdParams);
                OleDbDataReader odr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
                return odr;
            }
            catch(System.Exception ex)
            {
                conn.Close();
                throw ex;
            }
        }

        /// <summary>
        /// 执行 返回一个结果集
        /// </summary>
        /// <param name="trans">事物对象</param>
        /// <param name="cmdText">命令字符串</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="cmdParameters">参数</param>
        /// <returns></returns>
        public static OleDbDataReader ExecuteReader(OleDbTransaction trans, string cmdText, CommandType cmdType, params OleDbParameter[] cmdParasm)
        {
            if (trans == null)
                throw new ArgumentNullException("transaction");

            if (trans != null && trans.Connection == null)
                throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");

            OleDbCommand cmd = new OleDbCommand();

            try
            {
                PrepareCommand(trans.Connection, cmd, trans, cmdType, cmdText, cmdParasm);
                OleDbDataReader odr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
                return odr;
            }
            catch(System.Exception ex)
            {
                trans.Connection.Close();
                throw ex;
            }
        }

        /// <summary>
        /// 执行sql命令，返回结果的第一记录第一列
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="cmdText"></param>
        /// <param name="cmdType"></param>
        /// <param name="cmdParameters"></param>
        /// <returns></returns>
        public static object ExecuteScalar(string connectionString, string cmdText, CommandType cmdType, params OleDbParameter[] cmdParameters)
        {
            OleDbCommand cmd = new OleDbCommand();

            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                PrepareCommand(conn, cmd, null, cmdType, cmdText, cmdParameters);

                object rtn = cmd.ExecuteScalar();
                cmd.Parameters.Clear();
                return rtn;
            }
        }
        
        /// <summary>
        /// 执行事务或sql命令，返回结果的第一记录第一列
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="cmdText">存储过程名或sql命令</param>
        /// <param name="cmdType"></param>
        /// <param name="cmdParams"></param>
        /// <returns></returns>
        public static object ExecuteScalar(OleDbTransaction trans,string cmdText,CommandType cmdType,params OleDbParameter[] cmdParams)
        {
            if (trans == null)
                throw new ArgumentNullException("transaction");

            if (trans != null && trans.Connection == null)
                throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");

            OleDbCommand cmd = new OleDbCommand();

            PrepareCommand(trans.Connection, cmd, trans, cmdType, cmdText, cmdParams);

            object rtn = cmd.ExecuteScalar();
            cmd.Parameters.Clear();
            return rtn;
        }

        /// <summary>
        /// 执行sql命令，返回结果的第一记录第一列
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="cmdText"></param>
        /// <param name="cmdType"></param>
        /// <param name="cmdParams"></param>
        /// <returns></returns>
        public static object ExecuteScalar(OleDbConnection conn,string cmdText,CommandType cmdType,params OleDbParameter[] cmdParams)
        {
            OleDbCommand cmd = new OleDbCommand();

            PrepareCommand(conn, cmd, null, cmdType, cmdText, cmdParams);

            object rtn = cmd.ExecuteScalar();
            cmd.Parameters.Clear();
            return rtn;
        }

        /// <summary>
        /// 设置参数缓存
        /// </summary>
        /// <param name="cacheKey">键</param>
        /// <param name="cmdParams">参数</param>
        public static void CacheParameters(string cacheKey,OleDbParameter[] cmdParams)
        {
            _paramCach[cacheKey] = cmdParams;
        }

        /// <summary>
        /// 获取缓存中指定键的参数
        /// </summary>
        /// <param name="cacheKey">键</param>
        /// <returns></returns>
        public static OleDbParameter[] GetCachedParameters(string cacheKey)
        {
            OleDbParameter[] cachedParams = (OleDbParameter[])_paramCach[cacheKey];

            if (cachedParams == null)
                return null;

            OleDbParameter[] clonedParams = new OleDbParameter[cachedParams.Length];
            for(int i = 0; i < cachedParams.Length; i++)
            {
                clonedParams[i] = (OleDbParameter)((ICloneable)cachedParams[i]).Clone();
            }

            return clonedParams;
        }


        /// <summary>
        /// 将参数加入执行命令
        /// </summary>
        /// <param name="conn">连接</param>
        /// <param name="cmd">命令</param>
        /// <param name="trans">事物</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="cmdText">sql命令语句</param>
        /// <param name="commandParameters">参数</param>
        /// <returns></returns>
        private static void PrepareCommand(OleDbConnection conn, OleDbCommand cmd, OleDbTransaction trans, CommandType cmdType, string cmdText, OleDbParameter[] commandParameters)
        {
            //如果连接断开，重新打开
            if (conn.State != ConnectionState.Open)
                conn.Open();

            //绑定命令
            cmd.Connection = conn;
            cmd.CommandType = cmdType;
            cmd.CommandText = cmdText;

            //如果事务存在，绑定事务
            if (trans != null)
                cmd.Transaction = trans;

            //绑定参数
            if (commandParameters != null)
            {
                foreach (OleDbParameter param in commandParameters)
                {
                    cmd.Parameters.Add(param);
                }
            }
        }
    }
}
