
using Microsoft.ApplicationBlocks.Data;
using Reprocess.Common;
using Reprocess.Data.Metrics.Repository.IRepository;
using Reprocess.Model.Metrics.Dto;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reprocess.Data.Metrics.Repository
{
    public class StudentRepository : IStudentRepository
    {
        public bool DeteteStudentById(int id)
        {
            try
            {
                string commandText = "DELETE FROM Students WHERE Id = " + id;
                SqlHelper.ExecuteNonQuery(Constants.DiMetricsConnectionString, CommandType.Text, commandText);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public StudentEntity GetStudentById(int id)
        {
            string commandText = "SELECT *  FROM Students WHERE Id = " + id;
            StudentEntity result = new StudentEntity();
            DataSet ds = new DataSet();
            try
            {
                ds = SqlHelper.ExecuteDataset(Constants.DiMetricsConnectionString, CommandType.Text, commandText);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    result = FillStudent(ds.Tables[0].Rows[0]);
                }
            }
            catch (Exception ex)
            {

            }         
            return result;
        }

        public List<StudentEntity> GetStudents()
        {
            //DataSet ds;
            string commandText = "select *  from Students ";
            List<StudentEntity> result = new List<StudentEntity>();
            DataSet ds = new DataSet();
            ds = SqlHelper.ExecuteDataset(Constants.DiMetricsConnectionString, CommandType.Text, commandText);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    StudentEntity obj = new StudentEntity();
                    obj = FillStudent(ds.Tables[0].Rows[i]);
                    result.Add(obj);
                }

            }
            foreach (var (item, index) in result.Select((value, i) => (value, i)))
            {
                item.Index = index + 1;
            }
            return result;
        }

        public bool UpdateStudent(StudentEntity item)
        {        
            string commandText = "UPDATE Students SET Name = N'" + item.Name + "' , Age ='" + item.Age + "' , Address = N'" + item.Address + "' WHERE Id =@id";
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@id", SqlDbType.Int);
            param[0].Value = item.Id;
            try
            {
                SqlHelper.ExecuteNonQuery(Constants.DiMetricsConnectionString, CommandType.Text, commandText, param);
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
          
        }


        public StudentEntity FillStudent(DataRow dataRow)
        {
            StudentEntity result = new StudentEntity();
            try
            {
                result.Id = Convert.ToInt32(dataRow["Id"].ToString());
                result.Name = Convert.ToString(dataRow["Name"].ToString());
                result.Age = Convert.ToInt32(dataRow["Age"].ToString());
                result.Address = Convert.ToString(dataRow["Address"].ToString());
            }
            catch (Exception ex)
            {
                //todo log
            }
            return result;
        }

        public bool InsertStudent(StudentEntity item)
        {
            
            try
            {
                string commandText = "INSERT into Students(Name,Age,Address) " +
                 $" VALUES(N'{item.Name}', '{item.Age}',N'{item.Address}')";
                SqlHelper.ExecuteNonQuery(Constants.DiMetricsConnectionString, CommandType.Text, commandText);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
                   
        }

        public bool IsExistStudentInDb(int id)
        {
            try
            {
                string commandText = "select *  from Students  Where Id= " + id;
                DataSet ds = new DataSet();
                ds = SqlHelper.ExecuteDataset(Constants.DiMetricsConnectionString, CommandType.Text, commandText);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return true;
                }
                return false;

            }
            catch (Exception ex)
            {
                return false;
            }
         
        }
    }
}
