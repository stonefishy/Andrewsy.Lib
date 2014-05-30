using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.OracleClient;

namespace Andrewsy.Lib.Util.DataBase
{
    //����oracle���ݿ�������
    public abstract class OracleHelper
    {
        //����Oracle���ݿ��ַ���
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
        public static int ExecuteNonQuery(string connectionString, string cmdText, CommandType cmdType, params OracleParameter[] cmdParams)
        {
            OracleCommand cmd = new OracleCommand();

            using (OracleConnection conn = new OracleConnection(connectionString))
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
        /// <param name="trans">�������</param>
        /// <param name="cmdText">�洢��������sql����</param>
        /// <param name="cmdType">��������</param>
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
        /// ִ��Sql ����
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
        /// ִ�� ����һ�������
        /// </summary>
        /// <param name="connectionString">�����ַ���</param>
        /// <param name="cmdText">�����ַ���</param>
        /// <param name="cmdType">��������</param>
        /// <param name="cmdParameters">����</param>
        /// <returns></returns>
        public static OracleDataReader ExecuteReader(string connectionString, string cmdText, CommandType cmdType, params OracleParameter[] cmdParameters)
        {
            //��������
            OracleConnection conn = new OracleConnection(connectionString);
            OracleCommand cmd = new OracleCommand();

            try
            {
                PrepareCommand(conn, cmd, null, cmdType, cmdText, cmdParameters);

                //ִ��
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
        /// ִ��sql������ؽ���ĵ�һ��¼��һ��
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
        /// ִ�������sql������ؽ���ĵ�һ��¼��һ��
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="cmdText">�洢��������sql����</param>
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
        /// ִ��sql������ؽ���ĵ�һ��¼��һ��
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
        /// ���ò�������
        /// </summary>
        /// <param name="cacheKey">��</param>
        /// <param name="cmdParams">����</param>
        public static void CacheParameters(string cacheKey, OracleParameter[] cmdParams)
        {
            _paramCach[cacheKey] = cmdParams;
        }

        /// <summary>
        /// ��ȡ������ָ�����Ĳ���
        /// </summary>
        /// <param name="cacheKey">��</param>
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
        /// ����������ִ������
        /// </summary>
        /// <param name="conn">����</param>
        /// <param name="cmd">����</param>
        /// <param name="trans">����</param>
        /// <param name="cmdType">��������</param>
        /// <param name="cmdText">sql�������</param>
        /// <param name="commandParameters">����</param>
        /// <returns></returns>
        private static void PrepareCommand(OracleConnection conn, OracleCommand cmd, OracleTransaction trans, CommandType cmdType, string cmdText, OracleParameter[] commandParameters)
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
                foreach (OracleParameter param in commandParameters)
                {
                    cmd.Parameters.Add(param);
                }
            }
        }
    }
}
