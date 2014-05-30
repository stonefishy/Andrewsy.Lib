using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Andrewsy.Lib.Util.DataBase
{
    public abstract class SqlHelper
    {
        //����SQL���ݿ��ַ���
        public static readonly string ConnString = ConfigurationManager.AppSettings["connectionString"];

        //����һ����ϣ���������
        private static Hashtable _paramCach = Hashtable.Synchronized(new Hashtable());

        /// <summary>
        /// ִ�����ݲ�ѯ
        /// </summary>
        /// <param name="connectionString">���ݿ������ַ���</param>
        /// <param name="cmdType">�������� sql����洢����</param>
        /// <param name="cmdText">sql����</param>
        /// <param name="cmdParams">�������</param>
        /// <returns></returns>
        public static int ExecuteNonQuery(string connectionString, string cmdText, CommandType cmdType, params SqlParameter[] cmdParams)
        {
            SqlCommand cmd = new SqlCommand();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                //׼�����̲���
                PrepareCommand(conn, cmd, null, cmdType, cmdText, cmdParams);

                //ִ��
                int rtn = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                return rtn;
            }
        }

        /// <summary>
        /// ִ�����������
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="cmdText">�洢��������sql����</param>
        /// <param name="cmdType"></param>
        /// <param name="cmdParams"></param>
        /// <returns></returns>
        public static int ExecuteNonQuery(SqlTransaction trans, string cmdText, CommandType cmdType, params SqlParameter[] cmdParams)
        {
            if (trans == null)
                throw new ArgumentNullException("transaction");

            if (trans != null && trans.Connection == null)
                throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");

            SqlCommand cmd = new SqlCommand();

            PrepareCommand(trans.Connection, cmd, trans, cmdType, cmdText, cmdParams);

            int rtn = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            return rtn;
        }

        /// <summary>
        /// ִ��Sql ����
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="cmdText"></param>
        /// <param name="cmdType"></param>
        /// <param name="cmdParams"></param>
        /// <returns></returns>
        public static int ExecuteNonQuery(SqlConnection conn, string cmdText, CommandType cmdType, params SqlParameter[] cmdParams)
        {
            SqlCommand cmd = new SqlCommand();

            PrepareCommand(conn, cmd, null, cmdType, cmdText, cmdParams);

            int rtn = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            return rtn;
        }

        /// <summary>
        /// ִ�� ����һ�������
        /// </summary>
        /// <param name="connectionString">�����ַ���</param>
        /// <param name="cmdText">�����ַ���</param>
        /// <param name="cmdType">��������</param>
        /// <param name="cmdParameters">����</param>
        /// <returns></returns>
        public static SqlDataReader ExecuteReader(string connectionString, string cmdText, CommandType cmdType, params SqlParameter[] cmdParameters)
        {
            //��������
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();

            try
            {
                PrepareCommand(conn, cmd, null, cmdType, cmdText, cmdParameters);

                //ִ��
                SqlDataReader odr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
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
        /// ִ�� ����һ�������
        /// </summary>
        /// <param name="conn">���Ӷ���</param>
        /// <param name="cmdText">�����ַ���</param>
        /// <param name="cmdType">��������</param>
        /// <param name="cmdParameters">����</param>
        /// <returns></returns>
        public static SqlDataReader ExecuteReader(SqlConnection conn, string cmdText, CommandType cmdType, params SqlParameter[] cmdParams)
        {
            SqlCommand cmd = new SqlCommand();

            try
            {
                PrepareCommand(conn, cmd, null, cmdType, cmdText, cmdParams);
                SqlDataReader odr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
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
        /// ִ�� ����һ�������
        /// </summary>
        /// <param name="trans">�������</param>
        /// <param name="cmdText">�����ַ���</param>
        /// <param name="cmdType">��������</param>
        /// <param name="cmdParameters">����</param>
        /// <returns></returns>
        public static SqlDataReader ExecuteReader(SqlTransaction trans, string cmdText, CommandType cmdType, params SqlParameter[] cmdParasm)
        {
            if (trans == null)
                throw new ArgumentNullException("transaction");

            if (trans != null && trans.Connection == null)
                throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");

            SqlCommand cmd = new SqlCommand();

            try
            {
                PrepareCommand(trans.Connection, cmd, trans, cmdType, cmdText, cmdParasm);
                SqlDataReader odr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
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
        /// ִ��sql������ؽ���ĵ�һ��¼��һ��
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="cmdText"></param>
        /// <param name="cmdType"></param>
        /// <param name="cmdParameters"></param>
        /// <returns></returns>
        public static object ExecuteScalar(string connectionString, string cmdText, CommandType cmdType, params SqlParameter[] cmdParameters)
        {
            SqlCommand cmd = new SqlCommand();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                PrepareCommand(conn, cmd, null, cmdType, cmdText, cmdParameters);

                object rtn = cmd.ExecuteScalar();
                cmd.Parameters.Clear();
                return rtn;
            }
        }

        /// <summary>
        /// ִ�������sql������ؽ���ĵ�һ��¼��һ��
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="cmdText">�洢��������sql����</param>
        /// <param name="cmdType"></param>
        /// <param name="cmdParams"></param>
        /// <returns></returns>
        public static object ExecuteScalar(SqlTransaction trans, string cmdText, CommandType cmdType, params SqlParameter[] cmdParams)
        {
            if (trans == null)
                throw new ArgumentNullException("transaction");

            if (trans != null && trans.Connection == null)
                throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");

            SqlCommand cmd = new SqlCommand();

            PrepareCommand(trans.Connection, cmd, trans, cmdType, cmdText, cmdParams);

            object rtn = cmd.ExecuteScalar();
            cmd.Parameters.Clear();
            return rtn;
        }

        /// <summary>
        /// ִ��sql������ؽ���ĵ�һ��¼��һ��
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="cmdText"></param>
        /// <param name="cmdType"></param>
        /// <param name="cmdParams"></param>
        /// <returns></returns>
        public static object ExecuteScalar(SqlConnection conn, string cmdText, CommandType cmdType, params SqlParameter[] cmdParams)
        {
            SqlCommand cmd = new SqlCommand();

            PrepareCommand(conn, cmd, null, cmdType, cmdText, cmdParams);

            object rtn = cmd.ExecuteScalar();
            cmd.Parameters.Clear();
            return rtn;
        }

        /// <summary>
        /// ���ò�������
        /// </summary>
        /// <param name="cacheKey">��</param>
        /// <param name="cmdParams">����</param>
        public static void CacheParameters(string cacheKey, SqlParameter[] cmdParams)
        {
            _paramCach[cacheKey] = cmdParams;
        }

        /// <summary>
        /// ��ȡ������ָ�����Ĳ���
        /// </summary>
        /// <param name="cacheKey">��</param>
        /// <returns></returns>
        public static SqlParameter[] GetCachedParameters(string cacheKey)
        {
            SqlParameter[] cachedParams = (SqlParameter[])_paramCach[cacheKey];

            if (cachedParams == null)
                return null;

            SqlParameter[] clonedParams = new SqlParameter[cachedParams.Length];
            for (int i = 0; i < cachedParams.Length; i++)
            {
                clonedParams[i] = (SqlParameter)((ICloneable)cachedParams[i]).Clone();
            }

            return clonedParams;
        }


        /// <summary>
        /// ����������ִ������
        /// </summary>
        /// <param name="conn">����</param>
        /// <param name="cmd">����</param>
        /// <param name="trans">����</param>
        /// <param name="cmdType">��������</param>
        /// <param name="cmdText">sql�������</param>
        /// <param name="commandParameters">����</param>
        /// <returns></returns>
        private static void PrepareCommand(SqlConnection conn, SqlCommand cmd, SqlTransaction trans, CommandType cmdType, string cmdText, SqlParameter[] commandParameters)
        {
            //������ӶϿ������´�
            if (conn.State != ConnectionState.Open)
                conn.Open();

            //������
            cmd.Connection = conn;
            cmd.CommandType = cmdType;
            cmd.CommandText = cmdText;

            //���������ڣ�������
            if (trans != null)
                cmd.Transaction = trans;

            //�󶨲���
            if (commandParameters != null)
            {
                foreach (SqlParameter param in commandParameters)
                {
                    cmd.Parameters.Add(param);
                }
            }
        }
    }
}
