using System;
using System.Collections.Generic;
using System.Web.Services.Description;

namespace DataCrawling_Web.Models.Data
{
    public class DataInfoViewModel
    {
        public int Tab { get; set; }
        public IEnumerable<DataInfoModel> DataInfo { get; set; }
    }

    public class DataInfoEditView
    {
        public IEnumerable<DataInfoModel> DataInfo { get; set; }
        public DataInfoModel SelInfo { get; set; }
        public List<comboboxMpdel> Item_Sector { get; set; }
        public int Item_Sector_Idx { get; set; }
        public List<comboboxMpdel> Item_D_TYPE { get; set; }
        public int Item_D_TYPE_Idx { get; set; }
        public List<comboboxMpdel> Item_R_TYPE { get; set; }
        public int Item_R_TYPE_Idx { get; set; }
        public List<RequestParameter_Model> RequestList { get; set; }
        public List<ResponseElement_Model> ResponseList { get; set; }
    }

    public class comboboxMpdel
    {
        public int Idx { get; set; }
        public string Name { get; set; }
    }

    public class Authentication_Key_Model
    {
        public string AccessTb { get; set; }
        public int Auth_State { get; set; }
    }

    public class DataInfoModel
    {
        public int OrderNo { get; set; }
        public int DATA_IDX { get; set; }
        public string D_TYPE { get; set; }
        public string SECTOR { get; set; }
        public string TITLE { get; set; }
        public string SUMMARY { get; set; }
        public string ConnectDt { get; set; }
        public string EXPLANE { get; set; }
        public string F_NAME { get; set; }
        public int F_SIZE { get; set; }
        public string F_LINK { get; set; }
        public string EXTENTION { get; set; }
        public int UPDATE_CYCLE { get; set; }
        public string KEYWORD { get; set; }
        public string D_LINK { get; set; }
        public string R_TYPE { get; set; }
        public DateTime CREATE_DT { get; set; }
        public DateTime EDIT_DT { get; set; }
        public int READ_CNT { get; set; }
        public int DOWNLOAD_CNT { get; set; }
        public int REQUEST_CNT { get; set; }
        public DateTime LAST_UDT { get; set; }
    }

    public class Api_Model
    {
        public int P_IDX { get; set; }
        public string P_NAME_E { get; set; }
        public string P_TYPE { get; set; }
        public int P_NEED { get; set; }
        public string SAMPLE_TXT { get; set; }
        public string P_EXPLANE { get; set; }
        public int R_IDX { get; set; }
        public string R_NAME_K { get; set; }
        public string R_NAME_E { get; set; }
        public string R_TYPE { get; set; }
        public string R_EXPLANE { get; set; }
    }

    public class RequestParameter_Model
    {
        public int P_IDX { get; set; }
        public string P_NAME_E { get; set; }
        public string P_TYPE { get; set; }
        public int P_NEED { get; set; }
        public string SAMPLE_TXT { get; set; }
        public string P_EXPLANE { get; set; }
    }

    public class ResponseElement_Model
    {
        public int R_IDX { get; set; }
        public string R_NAME_K { get; set; }
        public string R_NAME_E { get; set; }
        public string R_TYPE { get; set; }
        public string R_EXPLANE { get; set; }
    }

    public class DataInfoView_Model
    {
        public DataInfoModel DataInfo { get; set; }
        public List<RequestParameter_Model> RequestList { get; set; }
        public List<ResponseElement_Model> ResponseList { get; set; }
    }
}