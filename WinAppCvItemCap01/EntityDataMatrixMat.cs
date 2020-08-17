using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinAppCvItemCap01
{
    class EntityDataMatrixMat
    {
        public long TimeStampId { get; set; }

        public long ParentTimeStampId { get; set; } //相同的Mat的id

        public int IndexNo { get; set; }

        public Mat DmMat { get; set; }

        public OpenCvSharp.Point2f CenterPt { get; set; }//圆心坐标

        public string DmCode { get; set; }

        public double RunTimeSpanMs { get; set; }

        public bool IsDecoded { get; set; }

        public EntityDataMatrixMat()
        {
            TimeStampId = 0;
            ParentTimeStampId = -1;
            IndexNo = -1;
            DmCode = "";
            RunTimeSpanMs = 0;
            IsDecoded = false;
        }
    }
}
