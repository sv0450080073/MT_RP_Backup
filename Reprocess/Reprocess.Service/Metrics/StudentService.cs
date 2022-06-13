using Reprocess.Data.Metrics.Repository.IRepository;
using Reprocess.Model.Metrics.Dto;
using Reprocess.Service.Metrics.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reprocess.Service.Metrics
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;
        public StudentService(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public bool DeleteStudent(int id)
        {
          return _studentRepository.DeteteStudentById(id);
        }

        public StudentEntity GetStudentById(int id)
        {
            return _studentRepository.GetStudentById(id);
        }

        public List<StudentEntity> GetStudents()
        {
            return _studentRepository.GetStudents();
        }

        public bool InsertStudent(StudentEntity studentEntity)
        {
            return _studentRepository.InsertStudent(studentEntity);
        }

        public bool IsExistStudentInDB(int id)
        {
            return _studentRepository.IsExistStudentInDb(id);
        }

        public bool IsValidStudentData(StudentEntity data)
        {
            try
            {
                if(data!= null)
                {
                    if (!string.IsNullOrEmpty(data.Name) && !string.IsNullOrEmpty(data.Address) && data.Age > 0)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool UpdateStudent( StudentEntity studentEntity)
        {
            if(IsExistStudentInDB(studentEntity.Id))
            {
                return _studentRepository.UpdateStudent(studentEntity);
            }
            return false;
        }
    }
}
