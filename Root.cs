using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engraving
{
    public class Root
    {
        public string name { get; set; }
        public string DeviceID { get; set; }

        public string ParName { get; set; }

        public string data { get; set; }    

        public double dAngle { get; set; }
        public double dOffsetX { get; set; }
        public double dOffsetY { get; set; }
        public double dCenterX { get; set; }
        public double dCenterY { get; set; }
        public string strFile { get; set; }
        public string shpID { get; set; }


        public List<object> RootData() 
        {
            List<object> listData = new List<object>();
            listData.Add(data);
            listData.Add(ParName);
            listData.Add(DeviceID);
            listData.Add(strFile);

            return listData;
        }
        
        public List<object> Data()
        {
            List<object> listData = new List<object>();
            listData.Add(DeviceID);
            listData.Add(strFile);
            listData.Add(shpID);
            listData.Add(dAngle);
            listData.Add(dOffsetX);
            listData.Add(dOffsetY);
            listData.Add(dCenterX);
            listData.Add(dCenterY);

            return listData;

        }
    }

    public class DataInfo 
    {
        public string DevId { get; set; }

        public string ParName { get; set; }    

    }
}
