using System.Collections.Generic;

namespace DataCrawling_Web.Models.Files
{
    public class TextUserRequestModel
    {
        /// <summary>
        /// 상태값
        /// </summary>
        public string Stat { get; set; }
        /// <summary>
        /// 파일 구분 코드(1:이력서, 2:포트폴리오, 3:증명서, 4:자격증, 5:추천서, 6:기획서, 7:기타) 
        /// </summary>
        public byte File_Up_Stat { get; set; }
        /// <summary>
        /// 업로드 여부(1:파일업로드, 2:URL등록) 
        /// </summary>
        public byte File_Type { get; set; }
        /// <summary>
        /// 업로드 파일 상태
        /// </summary>
        public byte hidFile_Type { get; set; }
        /// <summary>
        /// 업로드 파일명
        /// </summary>
        public string hiddFileName { get; set; }
        /// <summary>
        /// 업로드 파일 구분 코드
        /// </summary>
        public byte hidFile_Up_Stat { get; set; }
        /// <summary>
        /// 업로드 파일 사이즈
        /// </summary>
        public string hidFIle_Size { get; set; }
        /// <summary>
        /// 반환 URL
        /// </summary>
        public string re_url { get; set; }
        /// <summary>
        /// 반환 URL
        /// </summary>
        public string pop_url { get; set; }

        /// <summary>
        /// 업로드 파일명
        /// </summary>
        public string up_file { get; set; }
        /// <summary>
        /// 파일 수정 번호
        /// </summary>
        public int FileNo { get; set; }
        /// <summary>
        /// 파일 내용 검증 여부
        /// </summary>
        public bool isCheckFileContent { get; set; }

        /// <summary>
        /// APP POST PARAM 회원아이디
        /// </summary>
        public string M_ID { get; set; }

        /// <summary>
        /// APP POST PARAM 회원아이디
        /// </summary>
        public string Idata { get; set; }

        /// <summary>
        /// APP POST PARAM 회원아이디
        /// </summary>
        public string Pdata { get; set; }

        public int Origin_w { get; set; }
        public int Origin_h { get; set; }

        /// <summary>
        /// crop 좌표값
        /// </summary>
        public int CropSize_nx { get; set; }
        public int CropSize_ny { get; set; }
        public int CropSize_nw { get; set; }
        public int CropSize_nh { get; set; }
        public string Type { get; set; }
    }

    public class TextUserResponseModel
    {
        /// <summary>
        /// 반환 코드
        /// </summary>
        public int rc { get; set; }
        /// <summary>
        /// 업로드 된 파일 목록
        /// </summary>
        public List<TextUserResponseFileModel> items { get; set; }
        /// <summary>
        /// 반환 URL
        /// </summary>
        public string re_url { get; set; }
        /// <summary>
        /// 반환 URL
        /// </summary>
        public string pop_url { get; set; }
        /// <summary>
        /// 검출카운트
        /// </summary>
        public int InvalidCount { get; internal set; }
    }

    public class TextUserResponseFileModel
    {
        /// <summary>
        /// 업로드 여부(1:파일업로드, 2:URL등록) 
        /// </summary>
        public byte File_Type { get; set; }
        /// <summary>
        /// 업로드 파일명
        /// </summary>
        public string dFileName { get; set; }
        /// <summary>
        /// 업로드 Temp 파일경로
        /// </summary>
        public string dFilePath { get; set; }
        /// <summary>
        /// 사용자 업로드 파일명
        /// </summary>
        public string OldFileName { get; set; }
        /// <summary>
        /// 파일 구분 코드(1:이력서, 2:포트폴리오, 3:증명서, 4:자격증, 5:추천서, 6:기획서, 7:기타) 
        /// </summary>
        public byte File_Up_Stat { get; set; }
        /// <summary>
        /// 업로드 파일 구분 코드
        /// </summary>
        public byte hidFile_Up_Stat { get; set; }
        /// <summary>
        /// 업로드 파일 사이즈
        /// </summary>
        public string FIle_Size { get; set; }
        /// <summary>
        /// 확장자
        /// </summary>
        public string Ext { get; set; }
        /// <summary>
        /// 반환 URL
        /// </summary>
        public string re_url { get; set; }
        /// <summary>
        /// 반환 URL
        /// </summary>
        public string pop_url { get; set; }
        public string Origin_Filepath { get; set; }
        /// <summary>
        /// temp폴더 사용여부
        /// </summary>
        public int isTempFile { get; set; }

        /// <summary>
        /// 업로드 파일명
        /// </summary>
        public string Origin_FileName { get; set; }
    }

    public class TextUserErrorModel
    {
        /// <summary>
        /// 반환 코드
        /// </summary>
        public int rc { get; set; }
        /// <summary>
        /// 반환 메시지
        /// </summary>
        public string msg { get; set; }
        /// <summary>
        /// 반환 주소
        /// </summary>
        public string re_url { get; set; }
        /// <summary>
        /// 반환 URL
        /// </summary>
        public string pop_url { get; set; }

        /// <summary>
        /// APP POST PARAM 회원아이디
        /// </summary>
        public string M_ID { get; set; }

        /// <summary>
        /// APP POST PARAM 회원아이디
        /// </summary>
        public string Idata { get; set; }

        /// <summary>
        /// APP POST PARAM 회원아이디
        /// </summary>
        public string Pdata { get; set; }
        public int fileExist { get; set; }
    }

    public class TextUserResultModel
    {
        /// <summary>
        /// 반환 코드
        /// </summary>
        public int rc { get; set; }
        /// <summary>
        /// 반환 메시지
        /// </summary>
        public string msg { get; set; }
        /// <summary>
        /// 유입 구분
        /// </summary>
        public string pType { get; set; }

    }

}