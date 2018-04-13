using Kingsun.IBS.IBLL.IBS_TBX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kingsun.IBS.BLL.IBS_TBX
{
    public class IBS_TBXClassBLL : IIBS_TBXClassBLL
    {

        public bool AddClassInfo(string schId, string className, int userId)
        {
            throw new NotImplementedException();
        }

        public bool AddTeacherToClass(int userId, string classId, string schoolId, string subjectId)
        {
            throw new NotImplementedException();
        }

        public bool AddStudentToClass(int selfId, string otherId, int userId, string message)
        {
            throw new NotImplementedException();
        }

        public Model.IBS_ClassUserRelation GetClassInfoByClassId(string classId)
        {
            throw new NotImplementedException();
        }

        public Model.IBS_ClassUserRelation GetClassInfoByClassNum(int classNum)
        {
            throw new NotImplementedException();
        }

        public List<Model.IBS_ClassUserRelation> GetClassInfoListBySchoolId(string schId)
        {
            throw new NotImplementedException();
        }
    }
}
