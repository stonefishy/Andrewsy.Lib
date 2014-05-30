using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.Odbc;

namespace Andrewsy.Lib.Util.DataBase
{
    public abstract class OdbcHelper
    {
        //Access数据连接字符串
        //public static readonly string AccessConnString = @"DRIVER={Microsoft Access Driver (*.mdb)}; DBQ=C:\FWDB.mdb";
        //连接Odbc数据库字符串
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
        public static int ExecuteNonQuery(string connectionString, string cmdText, CommandType cmdType, params OdbcParameter[] cmdParams)
        {
            OdbcCommand cmd = new OdbcCommand();

            using (OdbcConnection conn = new OdbcConnection(connectionString))
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
        /// <param name="trans"></param>
        /// <param name="cmdText">存储过程名或sql命令</param>
        /// <param name="cmdType"></param>
        /// <param name="cmdParams"></param>
        /// <returns></returns>
        public static int ExecuteNonQuery(OdbcTransaction trans, string cmdText, CommandType cmdType, params OdbcParameter[] cmdParams)
        {
            if (trans == null)
                throw new ArgumentNullException("transaction");

            if (trans != null && trans.Connection == null)
                throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");

            OdbcCommand cmd = new OdbcCommand();

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
        public static int ExecuteNonQuery(OdbcConnection conn, string cmdText, CommandType cmdType, params OdbcParameter[] cmdParams)
        {
            OdbcCommand cmd = new OdbcCommand();

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
        public static OdbcDataReader ExecuteReader(string connectionString, string cmdText, CommandType cmdType, params OdbcParameter[] cmdParameters)
        {
            //创建连接
            OdbcConnection conn = new OdbcConnection(connectionString);
            OdbcCommand cmd = new OdbcCommand();

            try
            {
                PrepareCommand(conn, cmd, null, cmdType, cmdText, cmdParameters);

                //执行
                OdbcDataReader odr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
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
        public static OdbcDataReader ExecuteReader(OdbcConnection conn, string cmdText, CommandType cmdType, params OdbcParameter[] cmdParams)
        {
            OdbcCommand cmd = new OdbcCommand();

            try
            {
                PrepareCommand(conn, cmd, null, cmdType, cmdText, cmdParams);
                OdbcDataReader odr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
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
        /// <param name="trans">事物对象</param>
        /// <param name="cmdText">命令字符串</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="cmdParameters">参数</param>
        /// <returns></returns>
        public static OdbcDataReader ExecuteReader(OdbcTransaction trans, string cmdText, CommandType cmdType, params OdbcParameter[] cmdParasm)
        {
            if (trans == null)
                throw new ArgumentNullException("transaction");

            if (trans != null && trans.Connection == null)
                throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");

            OdbcCommand cmd = new OdbcCommand();

            try
            {
                PrepareCommand(trans.Connection, cmd, trans, cmdType, cmdText, cmdParasm);
                OdbcDataReader odr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
                return odr;
            }
            catch (System.Exception ex)
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
        public static object ExecuteScalar(string connectionString, string cmdText, CommandType cmdType, params OdbcParameter[] cmdParameters)
        {
            OdbcCommand cmd = new OdbcCommand();

            using (OdbcConnection conn = new OdbcConnection(connectionString))
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
        public static object ExecuteScalar(OdbcTransaction trans, string cmdText, CommandType cmdType, params OdbcParameter[] cmdParams)
        {
            if (trans == null)
                throw new ArgumentNullException("transaction");

            if (trans != null && trans.Connection == null)
                throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");

            OdbcCommand cmd = new OdbcCommand();

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
        public static object ExecuteScalar(OdbcConnection conn, string cmdText, CommandType cmdType, params OdbcParameter[] cmdParams)
        {
            OdbcCommand cmd = new OdbcCommand();

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
        public static void CacheParameters(string cacheKey, OdbcParameter[] cmdParams)
        {
            _paramCach[cacheKey] = cmdParams;
        }

        /// <summary>
        /// 获取缓存中指定键的参数
        /// </summary>
        /// <param name="cacheKey">键</param>
        /// <returns></returns>
        public static OdbcParameter[] GetCachedParameters(string cacheKey)
        {
            OdbcParameter[] cachedParams = (OdbcParameter[])_paramCach[cacheKey];

            if (cachedParams == null)
                return null;

            OdbcParameter[] clonedParams = new OdbcParameter[cachedParams.Length];
            for (int i = 0; i < cachedParams.Length; i++)
            {
                clonedParams[i] = (OdbcParameter)((ICloneable)cachedParams[i]).Clone();
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
        private static void PrepareCommand(OdbcConnection conn, OdbcCommand cmd, OdbcTransaction trans, CommandType cmdType, string cmdText, OdbcParameter[] commandParameters)
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
                foreach (OdbcParameter param in commandParameters)
                {
                    cmd.Parameters.Add(param);
                }
            }
        }
    }
}
