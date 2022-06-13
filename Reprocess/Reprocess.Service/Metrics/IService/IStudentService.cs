using Reprocess.Model.Metrics.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reprocess.Service.Metrics.IService
{
    public interface IStudentService
    {
        List<StudentEntity> GetStudents();
        StudentEntity GetStudentById(int id);
        bool  InsertStudent(StudentEntity studentEntity);
        bool DeleteStudent(int id);
        bool UpdateStudent( StudentEntity studentEntity);
        bool IsValidStudentData(StudentEntity studentEntity);
        bool IsExistStudentInDB(int id);
        
    }
}
