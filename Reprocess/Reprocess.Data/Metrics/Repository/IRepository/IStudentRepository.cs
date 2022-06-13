using Reprocess.Model.Metrics.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reprocess.Data.Metrics.Repository.IRepository
{
    public interface IStudentRepository
    {
        List<StudentEntity> GetStudents();
        StudentEntity GetStudentById(int id);
        bool InsertStudent(StudentEntity studentEntity);
        bool UpdateStudent( StudentEntity item);
        bool DeteteStudentById(int id);
        bool IsExistStudentInDb(int id);
    }
}
