using DataCrawling_Web.BSL.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataCrawling_Web.Service.Util
{
    public class MKHttpCookie : SimpleCookie
    {
        #region Member

        public string Mem_Type_Code
        {
            get { return string.Format("{0}", this.Get("Mem_Type_Code") ?? "C"); }
        }

        public string Site_Code
        {
            get { return string.Format("{0}", this.Get("Site_Code") ?? "JK"); }
        }

        public string M_ID
        {
            get { return string.Format("{0}", this.Get("M_ID")); }
        }

        public string C_ID
        {
            get { return string.Format("{0}", this.Get("C_ID")); }
        }

        public string H_ID
        {
            get { return string.Format("{0}", this.Get("H_ID")); }
        }

        public string S_ID
        {
            get { return string.Format("{0}", this.Get("S_ID")); }
        }

        public string E_ID
        {
            get { return string.Format("{0}", this.Get("E_ID")); }
        }

        public string Emp_Code
        {
            get { return string.Format("{0}", this.Get("Emp_Code")); }
        }

        public string Dept_Code
        {
            get { return string.Format("{0}", this.Get("Dept_Code")); }
        }

        public string Emp_Team_Name
        {
            get { return string.Format("{0}", this.Get("Emp_Team_Name")); }
        }

        public string Emp_Name
        {
            get { return string.Format("{0}", this.Get("Emp_Name")); }
        }

        private string _mem_ID;
        public string Mem_ID
        {
            get
            {
                if (_mem_ID == null)
                    InitMember();

                return _mem_ID;
            }
        }

        private short? _mem_Chk;
        public short Mem_Chk
        {
            get
            {
                if (_mem_Chk.HasValue == false)
                    InitMember();

                return _mem_Chk.Value;
            }
        }

        private void InitMember()
        {
            if (string.IsNullOrEmpty(M_ID) == false)
            {
                _mem_ID = M_ID;
                _mem_Chk = 1;
            }
            else if (string.IsNullOrEmpty(C_ID) == false)
            {
                _mem_ID = C_ID;
                _mem_Chk = 2;
            }
            else if (string.IsNullOrEmpty(H_ID) == false)
            {
                _mem_ID = H_ID;
                _mem_Chk = 3;
            }
            else if (string.IsNullOrEmpty(S_ID) == false)
            {
                _mem_ID = S_ID;
                _mem_Chk = 4;
            }
            else if (string.IsNullOrEmpty(E_ID) == false)
            {
                _mem_ID = E_ID;
                _mem_Chk = 5;
            }
            else
            {
                _mem_ID = string.Empty;
                _mem_Chk = 0;
            }
        }

        #endregion
    }
}