using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.OracleClient;

namespace Andrewsy.Lib.Util.DataBase
{
    //操作oracle数据库助手类
    public abstract class OracleHelper
    {
        //连接Oracle数据库字符串
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
        public static int ExecuteNonQuery(string connectionString, string cmdText, CommandType cmdType, params OracleParameter[] cmdParams)
        {
            OracleCommand cmd = new OracleCommand();

            using (OracleConnection conn = new OracleConnection(connectionString))
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
        public static int ExecuteNonQuery(OracleTransaction trans, string cmdText, CommandType cmdType, params OracleParameter[] cmdParams)
        {
            if (trans == null)
                throw new ArgumentNullException("transaction");

            if (trans != null && trans.Connection == null)
                throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");

            OracleCommand cmd = new OracleCommand();

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
        public static int ExecuteNonQuery(OracleConnection conn, string cmdText, CommandType cmdType, params OracleParameter[] cmdParams)
        {
            OracleCommand cmd = new OracleCommand();

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
        public static OracleDataReader ExecuteReader(string connectionString, string cmdText, CommandType cmdType, params OracleParameter[] cmdParameters)
        {
            //创建连接
            OracleConnection conn = new OracleConnection(connectionString);
            OracleCommand cmd = new OracleCommand();

            try
            {
                PrepareCommand(conn, cmd, null, cmdType, cmdText, cmdParameters);

                //执行
                OracleDataReader odr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
                return odr;
            }
            catch (System.Exception ex)
            {
                conn.Close();
                throw ex;
            }
        }

        public static OracleDataReader ExecuteReader(OracleConnection conn, string cmdText, CommandType cmdType, params OracleParameter[] cmdParams)
        {
            OracleCommand cmd = new OracleCommand();

            try
            {
                PrepareCommand(conn, cmd, null, cmdType, cmdText, cmdParams);
                OracleDataReader odr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
                return odr;
            }
            catch (System.Exception ex)
            {
                conn.Close();
                throw ex;
            }
        }

        public static OracleDataReader ExecuteReader(OracleTransaction trans, string cmdText, CommandType cmdType, params OracleParameter[] cmdParasm)
        {
            if (trans == null)
                throw new ArgumentNullException("transaction");

            if (trans != null && trans.Connection == null)
                throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");

            OracleCommand cmd = new OracleCommand();

            try
            {
                PrepareCommand(trans.Connection, cmd, trans, cmdType, cmdText, cmdParasm);
                OracleDataReader odr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
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
        public static object ExecuteScalar(string connectionString, string cmdText, CommandType cmdType, params OracleParameter[] cmdParameters)
        {
            OracleCommand cmd = new OracleCommand();

            using (OracleConnection conn = new OracleConnection(connectionString))
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
        public static object ExecuteScalar(OracleTransaction trans, string cmdText, CommandType cmdType, params OracleParameter[] cmdParams)
        {
            if (trans == null)
                throw new ArgumentNullException("transaction");

            if (trans != null && trans.Connection == null)
                throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");

            OracleCommand cmd = new OracleCommand();

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
        public static object ExecuteScalar(OracleConnection conn, string cmdText, CommandType cmdType, params OracleParameter[] cmdParams)
        {
            OracleCommand cmd = new OracleCommand();

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
        public static void CacheParameters(string cacheKey, OracleParameter[] cmdParams)
        {
            _paramCach[cacheKey] = cmdParams;
        }

        /// <summary>
        /// 获取缓存中指定键的参数
        /// </summary>
        /// <param name="cacheKey">键</param>
        /// <returns></returns>
        public static OracleParameter[] GetCachedParameters(string cacheKey)
        {
            OracleParameter[] cachedParams = (OracleParameter[])_paramCach[cacheKey];

            if (cachedParams == null)
                return null;

            OracleParameter[] clonedParams = new OracleParameter[cachedParams.Length];
            for (int i = 0; i < cachedParams.Length; i++)
            {
                clonedParams[i] = (OracleParameter)((ICloneable)cachedParams[i]).Clone();
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
        private static void PrepareCommand(OracleConnection conn, OracleCommand cmd, OracleTransaction trans, CommandType cmdType, string cmdText, OracleParameter[] commandParameters)
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
                foreach (OracleParameter param in commandParameters)
                {
                    cmd.Parameters.Add(param);
                }
            }
        }
    }
}
