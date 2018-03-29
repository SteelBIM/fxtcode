using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenPlatform.Domain.DTO
{
    public class EvaluationApiDto
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public string EstateStatus { get; set; }
        public List<EntrustAppraise4BonaDto> EstateInfo { get; set; }
    }
}
