using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OracleClient;

namespace OracleDataAccress
{
    public class OrclHelper
    {
        #region OracleParameter
        public static OracleParameter GetOracleParameter(string parameterName,object parameterValue,OracleType oracleType,ParameterDirection parameterDirection)
        {
            try
            {
                OracleParameter parameter = new OracleParameter();
                parameter.ParameterName = parameterName;
                parameter.Value = parameterValue;
                parameter.OracleType = oracleType;
                parameter.Direction = parameterDirection;
                return parameter;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region ExecuteNonQuery
        /// <summary>
        /// ExecuteNonQuery
        /// </summary>
        /// <param name="ConnectionString"></param>
        /// <param name="commandType"></param>
        /// <param name="cmdText"></param>
        /// <returns></returns>
        public static int ExecuteNonQuery(string ConnectionString,CommandType commandType,string cmdText)
        {
            OracleConnection oConn = new OracleConnection(ConnectionString);
            try
            {
                int rtn = 0;
                
                
                    if (oConn.State == System.Data.ConnectionState.Closed) oConn.Open();
                   
                    OracleCommand cComm = new OracleCommand();
                    cComm.CommandType = commandType;
                    cComm.CommandText = cmdText;
                    cComm.Connection =oConn;
                    rtn = cComm.ExecuteNonQuery();
                    oConn.Close();
                return rtn;
            }
            catch(Exception ex)
            {
                oConn.Close();
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// ExecuteNonQuery
        /// </summary>
        /// <param name="ConnectionString"></param>
        /// <param name="commandType"></param>
        /// <param name="cmdText"></param>
        /// <param name="oracleParameters"></param>
        /// <returns></returns>
        public static int ExecuteNonQuery(string ConnectionString, CommandType commandType, string cmdText, OracleParameter[] oracleParameters)
        {
            OracleConnection oConn = new OracleConnection(ConnectionString);
            try
            {
                int rtn = 0;
                
                
                    if (oConn.State == System.Data.ConnectionState.Closed) oConn.Open();

                    OracleCommand cComm = new OracleCommand();
                    cComm.CommandType = commandType;
                    cComm.CommandText = cmdText;
                    cComm.Connection = oConn;
                    foreach (OracleParameter iPara in oracleParameters)
                    {
                        cComm.Parameters.Add(iPara);
                    }
                    rtn = cComm.ExecuteNonQuery();
                    oConn.Close();
                return rtn;
            }
            catch (Exception ex)
            {
                oConn.Close();
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// ExecuteNonQuery
        /// </summary>
        /// <param name="oConn"></param>
        /// <param name="commandType"></param>
        /// <param name="cmdText"></param>
        /// <returns></returns>
        public static int ExecuteNonQuery(IDbConnection oConn, CommandType commandType, string cmdText)
        {
            try
            {
                int rtn = 0;

                if (oConn.State == System.Data.ConnectionState.Closed) oConn.Open();

                OracleCommand cComm = new OracleCommand();
                cComm.CommandType = commandType;
                cComm.CommandText = cmdText;
                cComm.Connection = (OracleConnection)oConn;
                rtn = cComm.ExecuteNonQuery();
                oConn.Close();
                return rtn;
            }
            catch (Exception ex)
            {
                if (oConn.State == System.Data.ConnectionState.Open) oConn.Close();
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// ExecuteNonQuery
        /// </summary>
        /// <param name="oConn"></param>
        /// <param name="commandType"></param>
        /// <param name="cmdText"></param>
        /// <param name="oracleParameters"></param>
        /// <returns></returns>
        public static int ExecuteNonQuery(IDbConnection oConn, CommandType commandType, string cmdText, OracleParameter[] oracleParameters)
        {
            try
            {
                int rtn = 0;

                if (oConn.State == System.Data.ConnectionState.Closed) oConn.Open();

                OracleCommand cComm = new OracleCommand();
                cComm.CommandType = commandType;
                cComm.CommandText = cmdText;
                cComm.Connection = (OracleConnection)oConn;
                foreach (OracleParameter iPara in oracleParameters)
                {
                    cComm.Parameters.Add(iPara);
                }
                rtn = cComm.ExecuteNonQuery();
                oConn.Close();
                return rtn;
            }
            catch (Exception ex)
            {
                if (oConn.State == System.Data.ConnectionState.Open) oConn.Close();
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// ExecuteNonQuery
        /// </summary>
        /// <param name="iTransaction"></param>
        /// <param name="commandType"></param>
        /// <param name="cmdText"></param>
        /// <returns></returns>
        public static int ExecuteNonQuery(IDbTransaction iTransaction, CommandType commandType, string cmdText)
        {
            try
            {
                int rtn = 0;
                OracleCommand cComm = new OracleCommand();
                cComm.CommandType = commandType;
                cComm.CommandText = cmdText;
                cComm.Connection = (OracleConnection)iTransaction.Connection;
                rtn = cComm.ExecuteNonQuery();
                return rtn;
            }
            catch (Exception ex)
            {
                
                throw new Exception(ex.Message);
            }
        }
        
        /// <summary>
        /// ExecuteNonQuery
        /// </summary>
        /// <param name="iTransaction"></param>
        /// <param name="commandType"></param>
        /// <param name="cmdText"></param>
        /// <param name="oracleParameters"></param>
        /// <returns></returns>
        public static int ExecuteNonQuery(IDbTransaction iTransaction, CommandType commandType, string cmdText, OracleParameter[] oracleParameters)
        {
            try
            {
                int rtn = 0;
                OracleCommand cComm = new OracleCommand();
                cComm.CommandType = commandType;
                cComm.CommandText = cmdText;
                cComm.Connection = (OracleConnection)iTransaction.Connection;
                foreach (OracleParameter iPara in oracleParameters)
                {
                    cComm.Parameters.Add(iPara);
                }
                rtn = cComm.ExecuteNonQuery();

                return rtn;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        #endregion

        #region ExecuteScalar
        /// <summary>
        /// ExecuteScalar
        /// </summary>
        /// <param name="ConnectionString"></param>
        /// <param name="commandType"></param>
        /// <param name="cmdText"></param>
        /// <returns></returns>
        public static object ExecuteScalar(string ConnectionString, CommandType commandType, string cmdText)
        {
            OracleConnection oConn = new OracleConnection(ConnectionString);
            try
            {
                object rtn = null;
                
               
                    if (oConn.State == System.Data.ConnectionState.Closed) oConn.Open();

                    OracleCommand cComm = new OracleCommand();
                    cComm.CommandType = commandType;
                    cComm.CommandText = cmdText;
                    cComm.Connection = oConn;
                    rtn = cComm.ExecuteNonQuery();
                    //rtn = cComm.Parameters[cComm.Parameters.Count];
                    oConn.Close();
                return rtn;
            }
            catch (Exception ex)
            {
                oConn.Close();
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// ExecuteScalar
        /// </summary>
        /// <param name="ConnectionString"></param>
        /// <param name="commandType"></param>
        /// <param name="cmdText"></param>
        /// <param name="oracleParameters"></param>
        /// <returns></returns>
        public static object ExecuteScalar(string ConnectionString, CommandType commandType, string cmdText, OracleParameter[] oracleParameters)
        {
            OracleConnection oConn = new OracleConnection(ConnectionString);
            try
            {
                object rtn = 0;
                
                
                    if (oConn.State == System.Data.ConnectionState.Closed) oConn.Open();

                    OracleCommand cComm = new OracleCommand();
                    cComm.CommandType = commandType;
                    cComm.CommandText = cmdText;
                    cComm.Connection = oConn;
                    foreach (OracleParameter iPara in oracleParameters)
                    {
                        cComm.Parameters.Add(iPara);
                    }
                    cComm.ExecuteNonQuery();
                    rtn = cComm.Parameters[cComm.Parameters.Count-1];

                    oConn.Close();
                return rtn;
            }
            catch (Exception ex)
            {
                oConn.Close();
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// ExecuteScalar
        /// </summary>
        /// <param name="oConn"></param>
        /// <param name="commandType"></param>
        /// <param name="cmdText"></param>
        /// <returns></returns>
        public static object ExecuteScalar(IDbConnection oConn, CommandType commandType, string cmdText)
        {
            try
            {
                object rtn = null;

                if (oConn.State == System.Data.ConnectionState.Closed) oConn.Open();

                OracleCommand cComm = new OracleCommand();
                cComm.CommandType = commandType;
                cComm.CommandText = cmdText;
                cComm.Connection = (OracleConnection)oConn;
                cComm.ExecuteScalar();
                rtn = cComm.Parameters[cComm.Parameters.Count].Value;
                oConn.Close();
                return rtn;
            }
            catch (Exception ex)
            {
                oConn.Close();
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// ExecuteScalar
        /// </summary>
        /// <param name="oConn"></param>
        /// <param name="commandType"></param>
        /// <param name="cmdText"></param>
        /// <param name="oracleParameters"></param>
        /// <returns></returns>
        public static object ExecuteScalar(IDbConnection oConn, CommandType commandType, string cmdText, OracleParameter[] oracleParameters)
        {
            try
            {
                object rtn = 0;

                if (oConn.State == System.Data.ConnectionState.Closed) oConn.Open();

                OracleCommand cComm = new OracleCommand();
                cComm.CommandType = commandType;
                cComm.CommandText = cmdText;
                cComm.Connection = (OracleConnection)oConn;
                foreach (OracleParameter iPara in oracleParameters)
                {
                    cComm.Parameters.Add(iPara);
                }
                cComm.ExecuteScalar();
                rtn = cComm.Parameters[cComm.Parameters.Count-1].Value;
                return rtn;
            }
            catch (Exception ex)
            {
                oConn.Close();
               throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// ExecuteScalar
        /// </summary>
        /// <param name="iTransaction"></param>
        /// <param name="commandType"></param>
        /// <param name="cmdText"></param>
        /// <returns></returns>
        public static object ExecuteScalar(IDbTransaction iTransaction, CommandType commandType, string cmdText)
        {
            try
            {
                object rtn = 0;
                OracleCommand cComm = new OracleCommand();
                cComm.CommandType = commandType;
                cComm.CommandText = cmdText;
                cComm.Connection = (OracleConnection)iTransaction.Connection;
                cComm.ExecuteNonQuery();
                rtn = cComm.Parameters[cComm.Parameters.Count];
                return rtn;
            }
            catch (Exception ex)
            {
                 throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// ExecuteNonQuery
        /// </summary>
        /// <param name="iTransaction"></param>
        /// <param name="commandType"></param>
        /// <param name="cmdText"></param>
        /// <param name="oracleParameters"></param>
        /// <returns></returns>
        public static object ExecuteScalar(IDbTransaction iTransaction, CommandType commandType, string cmdText, OracleParameter[] oracleParameters)
        {
            try
            {
                object rtn = 0;
                OracleCommand cComm = new OracleCommand();
                cComm.CommandType = commandType;
                cComm.CommandText = cmdText;
                cComm.Connection = (OracleConnection)iTransaction.Connection;
                foreach (OracleParameter iPara in oracleParameters)
                {
                    cComm.Parameters.Add(iPara);
                }
                cComm.ExecuteNonQuery();
                rtn = cComm.Parameters[cComm.Parameters.Count];
                return rtn;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region ExecuteDataReader
        

        /// <summary>
        /// ExecuteReader
        /// </summary>
        /// <param name="oConn"></param>
        /// <param name="commandType"></param>
        /// <param name="cmdText"></param>
        /// <returns></returns>
        public static IDataReader ExecuteReader(IDbConnection oConn, CommandType commandType, string cmdText)
        {
            try
            {
                IDataReader dr = null;

                if (oConn.State == System.Data.ConnectionState.Closed) oConn.Open();

                OracleCommand cComm = new OracleCommand();
                cComm.CommandType = commandType;
                cComm.CommandText = cmdText;
                cComm.Connection = (OracleConnection)oConn;
                dr = cComm.ExecuteReader();
                return dr;
            }
            catch (Exception ex)
            {
                 throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// ExecuteReader
        /// </summary>
        /// <param name="oConn"></param>
        /// <param name="commandType"></param>
        /// <param name="cmdText"></param>
        /// <param name="oracleParameters"></param>
        /// <returns></returns>
        public static IDataReader ExecuteReader(IDbConnection oConn, CommandType commandType, string cmdText, OracleParameter[] oracleParameters)
        {
            try
            {
                IDataReader dr = null;

                if (oConn.State == System.Data.ConnectionState.Closed) oConn.Open();

                OracleCommand cComm = new OracleCommand();
                cComm.CommandType = commandType;
                cComm.CommandText = cmdText;
                cComm.Connection = (OracleConnection)oConn;
                foreach (OracleParameter iPara in oracleParameters)
                {
                    cComm.Parameters.Add(iPara);
                }
                dr = cComm.ExecuteReader();
                return dr;
            }
            catch (Exception ex)
            {
                oConn.Close();
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// ExecuteReader
        /// </summary>
        /// <param name="iTransaction"></param>
        /// <param name="commandType"></param>
        /// <param name="cmdText"></param>
        /// <returns></returns>
        public static IDataReader ExecuteReader(IDbTransaction iTransaction, CommandType commandType, string cmdText)
        {
            try
            {
                IDataReader rtn = null;
                OracleCommand cComm = new OracleCommand();
                cComm.CommandType = commandType;
                cComm.CommandText = cmdText;
                cComm.Connection = (OracleConnection)iTransaction.Connection;
                rtn = cComm.ExecuteReader();
                return rtn;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// ExecuteReader
        /// </summary>
        /// <param name="iTransaction"></param>
        /// <param name="commandType"></param>
        /// <param name="cmdText"></param>
        /// <param name="oracleParameters"></param>
        /// <returns></returns>
        public static object ExecuteReader(IDbTransaction iTransaction, CommandType commandType, string cmdText, OracleParameter[] oracleParameters)
        {
            try
            {
                IDataReader rtn = null;
                OracleCommand cComm = new OracleCommand();
                cComm.CommandType = commandType;
                cComm.CommandText = cmdText;
                cComm.Connection = (OracleConnection)iTransaction.Connection;
                foreach (OracleParameter iPara in oracleParameters)
                {
                    cComm.Parameters.Add(iPara);
                }
                rtn = cComm.ExecuteReader();

                return rtn;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Fill DataSet
        /// <summary>
        /// Fill
        /// </summary>
        /// <param name="ConnectionString"></param>
        /// <param name="commandType"></param>
        /// <param name="cmdText"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static DataSet Fill(string ConnectionString, CommandType commandType, string cmdText,string tableName)
        {
            OracleConnection oConn = new OracleConnection(ConnectionString);
            try
            {
                DataSet ds = new DataSet();
                
                
                    if (oConn.State == System.Data.ConnectionState.Closed) oConn.Open();
                    OracleCommand cComm = new OracleCommand();
                    cComm.CommandType = commandType;
                    cComm.CommandText = cmdText;
                    cComm.Connection = oConn;
                    OracleDataAdapter da = new OracleDataAdapter(cComm);
                    da.Fill(ds, tableName);
                oConn.Close();
                return ds;
            }
            catch (Exception ex)
            {
                oConn.Close();
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// Fill
        /// </summary>
        /// <param name="ConnectionString"></param>
        /// <param name="commandType"></param>
        /// <param name="cmdText"></param>
        /// <param name="tableName"></param>
        /// <param name="oracleParameters"></param>
        /// <returns></returns>
        public static DataSet Fill(string ConnectionString, CommandType commandType, string cmdText, string tableName, OracleParameter[] oracleParameters)
        {
            OracleConnection oConn = new OracleConnection(ConnectionString);
            try
            {
                DataSet ds = new DataSet();

                    if (oConn.State == System.Data.ConnectionState.Closed) oConn.Open();
                    OracleCommand cComm = new OracleCommand();
                    cComm.CommandType = commandType;
                    cComm.CommandText = cmdText;
                    cComm.Connection = oConn;
                    foreach (OracleParameter iPara in oracleParameters)
                    {
                        cComm.Parameters.Add(iPara);
                    }
                    OracleDataAdapter da = new OracleDataAdapter(cComm);
                    da.Fill(ds, tableName);
                oConn.Close();
                return ds;
            }
            catch (Exception ex)
            {
                oConn.Close();
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// Fill
        /// </summary>
        /// <param name="oConn"></param>
        /// <param name="commandType"></param>
        /// <param name="cmdText"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static DataSet Fill(IDbConnection oConn, CommandType commandType, string cmdText, string tableName)
        {
            try
            {
                DataSet ds = new DataSet();

                if (oConn.State == System.Data.ConnectionState.Closed) oConn.Open();
                OracleCommand cComm = new OracleCommand();
                cComm.CommandType = commandType;
                cComm.CommandText = cmdText;
                cComm.Connection = (OracleConnection)oConn;
                OracleDataAdapter da = new OracleDataAdapter(cComm);
                da.Fill(ds, tableName);
               // oConn.Close();
                return ds;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// Fill
        /// </summary>
        /// <param name="ConnectionString"></param>
        /// <param name="commandType"></param>
        /// <param name="cmdText"></param>
        /// <param name="tableName"></param>
        /// <param name="oracleParameters"></param>
        /// <returns></returns>
        public static DataSet Fill(IDbConnection oConn, CommandType commandType, string cmdText, string tableName, OracleParameter[] oracleParameters)
        {
            try
            {
                DataSet ds = new DataSet();

                if (oConn.State == System.Data.ConnectionState.Closed) oConn.Open();
                OracleCommand cComm = new OracleCommand();
                cComm.CommandType = commandType;
                cComm.CommandText = cmdText;
                cComm.Connection = (OracleConnection)oConn;
                foreach (OracleParameter iPara in oracleParameters)
                {
                    cComm.Parameters.Add(iPara);
                }
                OracleDataAdapter da = new OracleDataAdapter(cComm);
                da.Fill(ds, tableName);
                //oConn.Close();
                return ds;
            }
            catch (Exception ex)
            {
throw new Exception(ex.Message);
            }
        }
        #endregion
    }
}
