using DataCrawling_Web.BSL.File;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using DataCrawling_Web.BSL.Authentication;
using DataCrawling_Web.Models.Files;
using DataCrawling_Web.Service.Util;
using DataCrawling_Web.BSL.Common;
using DataCrawling_Web.Service;
using System.Drawing.Drawing2D;
using DataCrawling_Web.DSL.Files;
using DataCrawling_Web.DSL.Offer;

namespace DataCrawling_Web.Controllers
{
    public class TextUserController : BaseController
    {
        /// <summary>
        /// 업로드 최대 크기
        /// </summary>
        public readonly int FileMaxFileSize = 10 * 1024 * 1024;
        public readonly int CommonFileMaxFileSize = 10 * 1024 * 1024;

        /// <summary>
        /// 첨부파일 등록
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        /// <remarks>2018-02-27 정석환 메모리 누수 오류 의심 부분 수정</remarks>
        [HttpPost]
        [ReturnUrlValid("re_url")]
        public ActionResult File_Attach_Ok(TextUserRequestModel param)
        {
            try
            {
                Encoding eucKr = Encoding.GetEncoding(51949);

                Response.AddHeader("Access-Control-Allow-Origin", Request.UrlReferrer.GetLeftPart(UriPartial.Authority));
                Response.AddHeader("Access-Control-Allow-Credentials", "true");

                Response.Charset = "euc-kr";
                Response.ContentEncoding = eucKr;

                TextUserErrorModel errorModel = null;

                if (string.IsNullOrEmpty(AuthUser.M_ID))
                {
                    errorModel = new TextUserErrorModel()
                    {
                        msg = "로그인 해주세요.",
                        rc = 1,
                        pop_url = param.pop_url
                    };

                    return Content(ViewRendererHelper.RenderPartialView("~/Views/TextUser/Partial/Fail.cshtml", errorModel, this.ControllerContext));
                }

                // 1. 파일 삭제 확인
                if (!string.IsNullOrEmpty(param.hiddFileName))
                {
                    string path = FilePathGenerate.GetUserFilePath(AuthUser.M_ID);
                    string delFileFullPath = string.Concat(path, @"\", param.hiddFileName);

                    System.IO.File.Delete(delFileFullPath);

                    TextUserResponseModel successModel = new TextUserResponseModel()
                    {
                        rc = 0,
                        pop_url = param.pop_url,
                        re_url = param.re_url
                    };

                    return Content(ViewRendererHelper.RenderPartialView("~/Views/TextUser/Partial/Success.cshtml", successModel, this.ControllerContext));
                }

                // 2. 업로드 확인
                if (Request.Files == null || Request.Files.Count == 0)
                {
                    errorModel = new TextUserErrorModel()
                    {
                        msg = "파일을 선택해 주세요.",
                        rc = 2,
                        pop_url = param.pop_url
                    };

                    return Content(ViewRendererHelper.RenderPartialView("~/Views/TextUser/Partial/Fail.cshtml", errorModel, this.ControllerContext));
                }
                else if (Request.Files.Count == 1)
                {
                    HttpPostedFileBase file = Request.Files[0];
                    string requestFileName = Ctx.CMSvc.ReplaceBadCharacter(file.FileName);
                    string fileName = AuthUser.M_ID + "_" + Path.GetFileNameWithoutExtension(requestFileName);

                    string originFileName = Path.GetFileName(requestFileName);
                    originFileName = originFileName.IsNormalized(NormalizationForm.FormD) ? originFileName.Normalize() : originFileName;

                    // 특수문자 % Replace - 모바일 파일 다운로드 오류 수정 
                    if (fileName.IndexOf("%") >= 0)
                    {
                        fileName = fileName.Replace("%", "_");
                    }

                    // Mac Unicode 오류 수정
                    if (fileName.IsNormalized(NormalizationForm.FormD))
                    {
                        fileName = fileName.Normalize();
                    }

                    // DB 기록이 불가능한 파일명인 경우 파일명 변환
                    if (eucKr.GetString(eucKr.GetBytes(fileName)).IndexOf("?") > -1)
                    {
                        fileName = string.Concat(AuthUser.M_ID, "_", DateTime.Now.ToString("yyyyMMdd"), Path.GetRandomFileName().Substring(0, 3).ToUpper());
                    }

                    string fileExt = Path.GetExtension(file.FileName);
                    string fileContentType = file.ContentType;


                    int fileContentLength = file.ContentLength;
                    //string folderName = string.Concat(FilePathGenerate.GetUserFilePath(AuthUser.M_ID));
                    string fileSaveFullPath = FilePathGenerate.GetUserFilePath(AuthUser.M_ID);
                    int fileDupeCnt = 0;

                    // 파일 사이즈 확인 

                    if (fileContentLength == 0)
                    {
                        errorModel = new TextUserErrorModel()
                        {

                            msg = "파일을 선택해 주세요.",
                            rc = 2,
                            pop_url = param.pop_url
                        };

                        return Content(ViewRendererHelper.RenderPartialView("~/Views/TextUser/Partial/Fail.cshtml", errorModel, this.ControllerContext));
                    }

                    // 확장자 확인
                    if (Code.AcceptFileExt.IndexOf(fileExt.ToLower()) == -1)
                    {
                        errorModel = new TextUserErrorModel()
                        {

                            msg = "파일문서, 이미지파일, 압축 파일이 아닙니다.\n다시 업로드하세요.",
                            rc = 3,
                            pop_url = param.pop_url
                        };

                        return Content(ViewRendererHelper.RenderPartialView("~/Views/TextUser/Partial/Fail.cshtml", errorModel, this.ControllerContext));
                    }

                    // 형식 확인
                    if (Code.AcceptTxt.IndexOf(fileContentType) == -1)
                    {
                        errorModel = new TextUserErrorModel()
                        {
                            msg = "파일 형식이 틀립니다.\n\n3MB 이내의 doc, docx, hwp, ppt, pptx, xls, xlsx, pdf, zip, jpg, gif 파일형식만 가능합니다.",
                            rc = 3,
                            pop_url = param.pop_url
                        };

                        return Content(ViewRendererHelper.RenderPartialView("~/Views/TextUser/Partial/Fail.cshtml", errorModel, this.ControllerContext));
                    }

                    // 크기 확인
                    if (fileContentLength > FileMaxFileSize)
                    {
                        errorModel = new TextUserErrorModel()
                        {
                            msg = "업로드된 모든 파일의 용량이 100MB를 초과하였습니다.\n\n첨부파일 용량은 최대 100MB까지 가능합니다.",
                            rc = 4,
                            pop_url = param.pop_url
                        };

                        return Content(ViewRendererHelper.RenderPartialView("~/Views/TextUser/Partial/Fail.cshtml", errorModel, this.ControllerContext));
                    }

                    //개인정보 발견시 종료
                    //if (param.isCheckFileContent)
                    {
                        var checkResult = MKCtx.CMSvc.CheckFileContents();
                        if (checkResult.InvalidCount > 0)
                        {
                            errorModel = new TextUserErrorModel()
                            {
                                msg = $"업로드 파일에서 주민번호로 의심되는 정보가 {checkResult.InvalidCount}건 검출되었습니다. 개인정보보호를 위해 개인정보가 포함된 파일은 등록할 수 없습니다. 개인정보 삭제 후 다시 업로드 해주세요.",
                                rc = 5,
                                pop_url = param.pop_url
                            };
                            return Content(ViewRendererHelper.RenderPartialView("~/Views/TextUser/Partial/Fail.cshtml", errorModel, this.ControllerContext));
                        }
                    }

                    // 중복 파일명 조회
                    string fullFileNamePath = string.Concat(fileSaveFullPath, fileName, fileExt);
                    if (!System.IO.File.Exists(fullFileNamePath))
                    {
                        file.SaveAs(fullFileNamePath);
                    }
                    else
                    {
                        bool exist = true;

                        do
                        {
                            fileDupeCnt++;
                            fullFileNamePath = string.Concat(fileSaveFullPath, fileName, "_", fileDupeCnt, fileExt);
                            if (!System.IO.File.Exists(fullFileNamePath))
                            {
                                exist = false;
                            }
                        } while (exist);

                        file.SaveAs(fullFileNamePath);
                    }

                    List<TextUserResponseFileModel> fileModel = new List<TextUserResponseFileModel>();
                    fileModel.Add(new TextUserResponseFileModel()
                    {
                        File_Type = param.File_Type,
                        Origin_FileName = originFileName,
                        dFileName = Path.GetFileName(fullFileNamePath),
                        OldFileName = Path.GetFileName(fullFileNamePath).Replace(string.Concat(AuthUser.M_ID, "_"), ""),
                        File_Up_Stat = param.File_Up_Stat,
                        hidFile_Up_Stat = param.hidFile_Up_Stat,
                        FIle_Size = Convert.ToString(fileContentLength),
                        Ext = fileExt,
                        re_url = param.re_url,
                        pop_url = param.pop_url
                    });

                    // 파일 업로드 완료
                    TextUserResponseModel successModel = new TextUserResponseModel()
                    {
                        items = fileModel,
                        rc = 0
                    };

                    return Content(ViewRendererHelper.RenderPartialView("~/Views/TextUser/Partial/Success.cshtml", successModel, this.ControllerContext));
                }
                else
                {
                    // 다중 업로드일 경우 처리
                }
            }
            catch (Exception ex)
            {
                if (Response.IsClientConnected)
                {
                    return Json(new { msg = ex.ToString() });
                }
                else
                {
                    Response.End();
                }

            }

            return Content("");
        }

        /// <summary>
        /// 첨부파일 수정
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [ReturnUrlValid("re_url")]
        public ActionResult File_Attach_Re_Ok(TextUserRequestModel param)
        {
            Response.AddHeader("Access-Control-Allow-Origin", Request.UrlReferrer.GetLeftPart(UriPartial.Authority));
            Response.AddHeader("Access-Control-Allow-Credentials", "true");

            TextUserErrorModel errorModel = null;

            if (AuthUser.M_ID == "")
            {
                errorModel = new TextUserErrorModel()
                {
                    msg = "로그인 해주세요.",
                    rc = 1,
                    re_url = param.re_url,
                    pop_url = param.pop_url
                };

                return Content(ViewRendererHelper.RenderPartialView("~/Views/TextUser/Partial/Fail.cshtml", errorModel, this.ControllerContext));
            }

            if (string.IsNullOrEmpty(param.re_url) || param.FileNo <= 0)
            {
                errorModel = new TextUserErrorModel()
                {
                    msg = "비정상적인 경로로 들어오셨습니다.",
                    rc = 1,
                    re_url = param.re_url,
                    pop_url = param.pop_url
                };

                return Content(ViewRendererHelper.RenderPartialView("~/Views/TextUser/Partial/Fail.cshtml", errorModel, this.ControllerContext));
            }

            // 1. 파일 삭제 확인
            if (!string.IsNullOrEmpty(param.up_file))
            {
                string path = FilePathGenerate.GetUserFilePath(AuthUser.M_ID);
                string delFileFullPath = string.Concat(path, @"\", param.up_file);

                System.IO.File.Delete(delFileFullPath);

                errorModel = new TextUserErrorModel()
                {
                    rc = 0,
                    re_url = param.re_url,
                    pop_url = param.pop_url
                };

                return Content(ViewRendererHelper.RenderPartialView("~/Views/TextUser/Partial/Fail.cshtml", errorModel, this.ControllerContext));
            }

            // 2. 업로드 확인
            if (Request.Files == null || Request.Files.Count == 0)
            {
                errorModel = new TextUserErrorModel()
                {
                    msg = "파일을 선택해 주세요.",
                    rc = 2,
                    re_url = param.re_url,
                    pop_url = param.pop_url
                };

                return Content(ViewRendererHelper.RenderPartialView("~/Views/TextUser/Partial/Fail.cshtml", errorModel, this.ControllerContext));
            }
            else if (Request.Files.Count == 1)
            {
                HttpPostedFileBase file = Request.Files[0];
                string requestFileName = MKCtx.CMSvc.ReplaceBadCharacter(file.FileName);
                string fileName = AuthUser.M_ID + "_" + Path.GetFileNameWithoutExtension(requestFileName);

                // Mac Unicode 오류 수정
                if (fileName.IsNormalized(NormalizationForm.FormD))
                {
                    fileName = fileName.Normalize();
                }

                // DB 기록이 불가능한 파일명인 경우 파일명 변환
                Encoding eucKr = Encoding.GetEncoding(51949);
                if (eucKr.GetString(eucKr.GetBytes(fileName)).IndexOf("?") > -1)
                {
                    fileName = string.Concat(AuthUser.M_ID, "_", DateTime.Now.ToString("yyyyMMdd"), Path.GetRandomFileName().Substring(0, 3).ToUpper());
                }

                string fileExt = Path.GetExtension(file.FileName);
                string fileContentType = file.ContentType;
                int fileContentLength = file.ContentLength;
                string folderName = string.Concat(FilePathGenerate.GetUserFilePath(AuthUser.M_ID));
                string fileSaveFullPath = FilePathGenerate.GetUserFilePath(AuthUser.M_ID);
                int fileDupeCnt = 0;

                // 파일 사이즈 확인
                if (fileContentLength == 0)
                {
                    errorModel = new TextUserErrorModel()
                    {
                        msg = "파일을 선택해 주세요.",
                        rc = 2,
                        re_url = param.re_url,
                        pop_url = param.pop_url
                    };

                    return Content(ViewRendererHelper.RenderPartialView("~/Views/TextUser/Partial/Fail.cshtml", errorModel, this.ControllerContext));
                }

                // 확장자 확인
                if (Code.AcceptFileExt.IndexOf(fileExt.ToLower()) == -1)
                {
                    errorModel = new TextUserErrorModel()
                    {
                        msg = "파일문서, 이미지파일, 압축 파일이 아닙니다.\n다시 업로드하세요.",
                        rc = 3,
                        re_url = param.re_url,
                        pop_url = param.pop_url
                    };

                    return Content(ViewRendererHelper.RenderPartialView("~/Views/TextUser/Partial/Fail.cshtml", errorModel, this.ControllerContext));
                }

                // 형식 확인
                if (Code.AcceptMimeType.IndexOf(fileContentType) == -1)
                {
                    errorModel = new TextUserErrorModel()
                    {
                        msg = "파일 형식이 틀립니다.\n\n3MB 이내의 doc, docx, hwp, ppt, pptx, xls, xlsx, pdf, zip, jpg, gif파일형식만 가능합니다.",
                        rc = 3,
                        re_url = param.re_url,
                        pop_url = param.pop_url
                    };

                    return Content(ViewRendererHelper.RenderPartialView("~/Views/TextUser/Partial/Fail.cshtml", errorModel, this.ControllerContext));
                }

                // 크기 확인
                if (fileContentLength > FileMaxFileSize)
                {
                    errorModel = new TextUserErrorModel()
                    {
                        msg = "업로드된 모든 파일의 용량이 100MB를 초과하였습니다.\n\n첨부파일 용량은 최대 100MB까지 가능합니다.",
                        rc = 4,
                        re_url = param.re_url,
                        pop_url = param.pop_url
                    };

                    return Content(ViewRendererHelper.RenderPartialView("~/Views/TextUser/Partial/Fail.cshtml", errorModel, this.ControllerContext));
                }

                // 중복 파일명 조회
                string fullFileNamePath = string.Concat(fileSaveFullPath, fileName, fileExt);
                if (!System.IO.File.Exists(fullFileNamePath))
                {
                    file.SaveAs(fullFileNamePath);
                }
                else
                {
                    bool exist = true;

                    do
                    {
                        fileDupeCnt++;
                        fullFileNamePath = string.Concat(fileSaveFullPath, fileName, "_", fileDupeCnt, fileExt);
                        if (!System.IO.File.Exists(fullFileNamePath))
                        {
                            exist = false;
                        }
                    } while (exist);

                    file.SaveAs(fullFileNamePath);
                }

                string File_Name = Ctx.RpSvc.SelectUserFileDB(AuthUser.M_ID, param.FileNo);
                if (!string.IsNullOrEmpty(File_Name))
                {
                    string path = FilePathGenerate.GetUserFilePath(AuthUser.M_ID);
                    string delFileFullPath = string.Concat(path, @"\", File_Name);

                    System.IO.File.Delete(delFileFullPath);
                }

                int FileIdx = Ctx.RpSvc.UpdateOnPassUserFile(param.File_Type, Path.GetFileName(fullFileNamePath), fileContentLength.ToString(), AuthUser.M_ID, param.FileNo);

                List<TextUserResponseFileModel> fileModel = new List<TextUserResponseFileModel>();
                fileModel.Add(new TextUserResponseFileModel()
                {
                    re_url = param.re_url
                });

                // 파일 업로드 완료
                TextUserResponseModel successModel = new TextUserResponseModel()
                {
                    items = fileModel,
                    rc = 0
                };

                return Content(ViewRendererHelper.RenderPartialView("~/Views/TextUser/Partial/Success.cshtml", successModel, this.ControllerContext));
            }
            else
            {
                // 다중 업로드일 경우 처리
            }

            return Content("");
        }

        /// <summary>
        /// 첨부파일 등록
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult FileAttachAjax(TextUserRequestModel param)
        {
            TextUserErrorModel errorModel = null;

            try
            {
                Encoding eucKr = Encoding.GetEncoding(51949);

                if (Request.UrlReferrer != null)
                    Response.AddHeader("Access-Control-Allow-Origin", Request.UrlReferrer.GetLeftPart(UriPartial.Authority));
                else
                    Response.AddHeader("Access-Control-Allow-Origin", "*");

                Response.AddHeader("Access-Control-Allow-Credentials", "true");

                if (string.IsNullOrEmpty(AuthUser.M_ID))
                {
                    errorModel = new TextUserErrorModel()
                    {
                        msg = "로그인이 필요합니다.\n로그인 후 다시 이용해주세요.",
                        rc = 1
                    };

                    return Json(errorModel);
                }

                // 1. 파일 삭제 확인
                if (!string.IsNullOrEmpty(param.hiddFileName))
                {
                    string path = FilePathGenerate.GetUserFilePath(Utility.Decrypt_AES(AuthUser.M_ID).Split('@')[0]);
                    string delFileFullPath = string.Concat(path, @"\", param.hiddFileName);

                    System.IO.File.Delete(delFileFullPath);

                    TextUserResponseModel successModel = new TextUserResponseModel()
                    {
                        rc = 0
                    };

                    return Json(errorModel);
                }

                // 2. 업로드 확인
                if (Request.Files == null || Request.Files.Count == 0)
                {
                    errorModel = new TextUserErrorModel()
                    {
                        msg = "파일을 선택해 주세요.",
                        rc = 2
                    };

                    return Json(errorModel);
                }
                else if (Request.Files.Count == 1)
                {
                    HttpPostedFileBase file = Request.Files[0];
                    string requestFileName = MKCtx.CMSvc.ReplaceBadCharacter(file.FileName);
                    string fileName = Utility.Decrypt_AES(AuthUser.M_ID).Split('@')[0] + "_" + Path.GetFileNameWithoutExtension(requestFileName);
                    string originFileName = Path.GetFileName(requestFileName);
                    originFileName = originFileName.IsNormalized(NormalizationForm.FormD) ? originFileName.Normalize() : originFileName;

                    // 특수문자 % Replace - 모바일 파일 다운로드 오류 수정 
                    if (fileName.IndexOf("%") >= 0)
                    {
                        fileName = fileName.Replace("%", "_");
                    }

                    // Mac Unicode 오류 수정
                    if (fileName.IsNormalized(NormalizationForm.FormD))
                    {
                        fileName = fileName.Normalize();
                    }

                    // DB 기록이 불가능한 파일명인 경우 파일명 변환
                    if (eucKr.GetString(eucKr.GetBytes(fileName)).IndexOf("?") > -1)
                    {
                        fileName = string.Concat(Utility.Decrypt_AES(AuthUser.M_ID).Split('@')[0], "_", DateTime.Now.ToString("yyyyMMdd"), Path.GetRandomFileName().Substring(0, 3).ToUpper());
                    }

                    string fileExt = Path.GetExtension(file.FileName);
                    string fileContentType = file.ContentType;
                    int fileContentLength = file.ContentLength;
                    string folderName = string.Concat(FilePathGenerate.GetUserFilePath(Utility.Decrypt_AES(AuthUser.M_ID)).Split('@')[0]);
                    string fileSaveFullPath = FilePathGenerate.GetUserFilePath(Utility.Decrypt_AES(AuthUser.M_ID).Split('@')[0]);
                    int fileDupeCnt = 0;

                    // 파일 사이즈 확인
                    if (fileContentLength == 0)
                    {
                        errorModel = new TextUserErrorModel()
                        {
                            msg = "파일을 선택해 주세요.",
                            rc = 2
                        };

                        return Json(errorModel);
                    }

                    // 확장자 확인
                    if (Code.AcceptFileExt.IndexOf(fileExt.ToLower()) == -1)
                    {
                        errorModel = new TextUserErrorModel()
                        {
                            msg = "파일 형식이 올바르지 않습니다.\n\n문서파일, 이미지파일, 압축파일만 가능합니다.",
                            rc = 3
                        };

                        return Json(errorModel);
                    }

                    // 형식 확인
                    if (Code.AcceptTxt.IndexOf(fileContentType) == -1)
                    {
                        errorModel = new TextUserErrorModel()
                        {
                            msg = "파일 형식이 틀립니다.\n\n3MB 이내의 doc, docx, hwp, ppt, pptx, xls, xlsx, pdf, zip, jpg, gif 파일형식만 가능합니다.",
                            rc = 3
                        };

                        return Json(errorModel);
                    }

                    // 크기 확인
                    if (fileContentLength > FileMaxFileSize)
                    {
                        errorModel = new TextUserErrorModel()
                        {
                            msg = "업로드된 모든 파일의 용량이 100MB를 초과하였습니다.\n\n첨부파일 용량은 최대 100MB까지 가능합니다.",
                            rc = 4
                        };

                        return Json(errorModel);
                    }

                    // 이미 등록된 파일인지 확인 (파일명 + 사이즈로 체크)
                    string fullFileNamePath = string.Concat(fileSaveFullPath, fileName, fileExt);

                    /*
                    ### 파일은 덮어쓰기 되고, DB 데이터 raw 는 Insert 되는 버그로 인해 중복파일명조회 로직으로 변경 [2018.11.16 ytjung] ###
                        int isExists = srv.SelectExistsFile(AuthUser.M_ID, fileName + fileExt, fileContentLength.ToString());

                        if (isExists > 0)
                        {
                            errorModel = new TextUserErrorModel()
                            {
                                msg = "이미 포트폴리오에 추가된 파일 입니다.",
                                rc = 6
                            };

                            return Json(errorModel);
                        }
                        else
                        {
                            file.SaveAs(fullFileNamePath);
                        }
                    */

                    //개인정보 발견시 종료
                    if (param.isCheckFileContent)
                    {
                        var checkResult = MKCtx.CMSvc.CheckFileContents();
                        if (checkResult.InvalidCount > 0)
                        {
                            errorModel = new TextUserErrorModel()
                            {
                                msg = $"업로드 파일에서 주민번호로 의심되는 정보가 {checkResult.InvalidCount}건 검출되었습니다. 개인정보보호를 위해 개인정보가 포함된 파일은 등록할 수 없습니다. 개인정보 삭제 후 다시 업로드 해주세요.",
                                rc = 5
                            };
                            return Json(new { msg = errorModel.msg, rc = errorModel.rc }, JsonRequestBehavior.AllowGet);
                        }
                    }

                    // 중복 파일명 조회                    
                    if (!System.IO.File.Exists(fullFileNamePath))
                    {
                        file.SaveAs(fullFileNamePath);
                    }
                    else
                    {
                        bool exist = true;

                        do
                        {
                            fileDupeCnt++;
                            fullFileNamePath = string.Concat(fileSaveFullPath, fileName, "_", fileDupeCnt, fileExt);
                            if (!System.IO.File.Exists(fullFileNamePath))
                            {
                                exist = false;
                            }
                        } while (exist);

                        file.SaveAs(fullFileNamePath);
                    }

                    List<TextUserResponseFileModel> fileModel = new List<TextUserResponseFileModel>
                    {
                        new TextUserResponseFileModel()
                        {
                            File_Type = param.File_Type,
                            dFileName = Path.GetFileName(fullFileNamePath),
                            OldFileName = Path.GetFileName(fullFileNamePath).Replace(string.Concat(AuthUser.M_ID, "_"), ""),
                            Origin_FileName = originFileName,
                            File_Up_Stat = param.File_Up_Stat,
                            hidFile_Up_Stat = param.hidFile_Up_Stat,
                            FIle_Size = Convert.ToString(fileContentLength),
                            Ext = fileExt
                        }
                    };

                    // 파일 업로드 완료
                    TextUserResponseModel successModel = new TextUserResponseModel()
                    {
                        items = fileModel,
                        rc = 0
                    };

                    return Json(successModel);
                }
                else
                {
                    // 다중 업로드일 경우 처리
                }
            }
            catch (Exception ex)
            {
                return Json(errorModel = new TextUserErrorModel()
                {
                    msg = ex.ToString(),
                    rc = -1
                });
            }

            return Json(errorModel);
        }

        /// <summary>
        /// 첨부파일 삭제 => ajax 호출 전용
        /// </summary>
        /// <param name="fileNo">콤마(,)로 연결된 idx 문자열</param>
        /// <param name="re_url">되돌아갈 페이지</param>
        /// <returns></returns>
        [HttpPost]
        [ReturnUrlValid("re_url")]
        public JsonResult AttachedFile_Delete_Ajax(string fileNo, string re_url)
        {
            if (Request.UrlReferrer != null)
            {
                Response.AddHeader("Access-Control-Allow-Origin", Request.UrlReferrer.GetLeftPart(UriPartial.Authority));
            }
            else
            {
                Response.AddHeader("Access-Control-Allow-Origin", "*");
            }

            Response.AddHeader("Access-Control-Allow-Credentials", "true");

            bool isError = false;
            TextUserErrorModel errModel = new TextUserErrorModel();
            TextUserResponseModel resModel = new TextUserResponseModel();

            //파라미터 검증 => //비어있거나, (숫자+,) 조합이 아닐시 Error
            if (string.IsNullOrEmpty(AuthUser.M_ID) || string.IsNullOrEmpty(fileNo) || !(Regex.IsMatch(fileNo, @"^[0-9,]+$")))
            {
                isError = true;
                errModel.rc = 9;
                errModel.msg = "유효하지 않은 파일번호 입니다. 다시 시도해주세요.";
            }
            else
            {
                var arrFileNo = fileNo.Split(',');

                foreach (var item in arrFileNo)
                {
                    var fileInfo = Ctx.RpSvc.USP_JKM_UserFileDB_FileInfo_S(AuthUser.M_ID, Convert.ToInt32(item));
                    if (fileInfo != null)
                    {
                        try
                        {
                            //파일 삭제
                            string path = FilePathGenerate.GetUserFilePath(AuthUser.M_ID);
                            //string delFileFullPath = string.Concat(path, @"\", fileInfo.File_Name);

                            //System.IO.File.Delete(delFileFullPath);

                            //DB 업데이트
                            Ctx.RpSvc.USP_JKM_UserFileDB_SU(AuthUser.M_ID, Convert.ToInt32(item));

                            resModel.rc = 0;
                        }
                        catch (Exception ex)
                        {
                            isError = true;
                            errModel.rc = 9;
                            errModel.msg = ex.Message;
                        }
                    }
                    else
                    {
                        isError = true;
                        errModel.rc = 9;
                        errModel.msg = "유효하지 않은 파일번호 입니다. 다시 시도해주세요.";
                    }
                }
            }

            if (isError)
            {
                return Json(errModel);
            }
            else
            {
                return Json(resModel);
            }
        }

        /// <summary>
        ///  공통 이미지 파일 업로드
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [ReturnUrlValid("re_url")]
        public JsonResult ImageAttach(string Type, TextUserRequestModel param)
        {
            string Mid = param.M_ID;
            TextUserErrorModel errorModel = null;

            if (!string.IsNullOrWhiteSpace(param.Idata) && string.IsNullOrWhiteSpace(param.M_ID))
            {
                Mid = AuthUser.Decrypt_AES(param.Idata);
            }

            if (Request.UrlReferrer != null)
                Response.AddHeader("Access-Control-Allow-Origin", Request.UrlReferrer.GetLeftPart(UriPartial.Authority));
            else
                Response.AddHeader("Access-Control-Allow-Origin", "*");

            Response.AddHeader("Access-Control-Allow-Credentials", "true");


            //로그인 체크
            if (string.IsNullOrEmpty(Mid))
            {
                errorModel = new TextUserErrorModel()
                {
                    msg = "로그인 해주세요.",
                    rc = 1
                };

                return Json(errorModel);
            }

            // 파일존재
            if (Request.Files.Count == 1)
            {
                HttpPostedFileBase postfile = Request.Files[0];

                string fileName = string.Empty;
                string fileExt = Path.GetExtension(postfile.FileName);

                //파일 저장 위치 ( 넘어오느 Type 값이 저장경로 디렉토리 명)
                string fileSaveFullPath = FilePathGenerate.GetUserImageFilePath(Type, Mid);//AuthUser.M_ID

                //중복이 나지 않을때까지 파일명 루프         
                string fullFileNamePath = string.Empty;
                do
                {
                    fileName = string.Concat(FilePathGenerate.GetCreateRandomFileName() + fileExt);
                    fullFileNamePath = Path.Combine(fileSaveFullPath, fileName);
                }
                while (System.IO.File.Exists(fullFileNamePath));


                int fileContentLength = postfile.ContentLength;

                // 파일 사이즈 확인
                if (fileContentLength == 0)
                {
                    errorModel = new TextUserErrorModel()
                    {
                        msg = "파일 사이즈 에러",
                        rc = 3
                    };

                    return Json(errorModel);
                }

                // 확장자 확인
                if (!Code.CommonAcceptFileExt.Contains(fileExt, StringComparer.CurrentCultureIgnoreCase))
                {
                    errorModel = new TextUserErrorModel()
                    {
                        msg = "파일 형식이 올바르지 않습니다",
                        rc = 3
                    };

                    return Json(errorModel);
                }

                // 크기 확인
                if (fileContentLength > CommonFileMaxFileSize)
                {
                    errorModel = new TextUserErrorModel()
                    {
                        msg = "업로드된 모든 파일의 용량이 10MB를 초과하였습니다.\n\n첨부파일 용량은 최대 10MB까지 가능합니다.",
                        rc = 4
                    };

                    return Json(errorModel);
                }

                Bitmap bmp = new Bitmap(postfile.InputStream);
                //바이러스 여부
                bool isvirus = false;
                //성공 여부
                bool result = false;
                string Ext = string.Empty;

                ImageUploadHelper upimg = new ImageUploadHelper(fileSaveFullPath, string.Format("{0}", FilePathGenerate.GetCreateRandomFileName()));

                //파일업로드
                result = upimg.WriteImage(postfile, out Ext, true);


                if (result)
                {
                    List<TextUserResponseFileModel> fileModel = new List<TextUserResponseFileModel>();
                    fileModel.Add(new TextUserResponseFileModel()
                    {
                        File_Type = 1,
                        dFileName = string.Format("{0}.{1}", upimg.FileName, Ext),
                        dFilePath = string.Format("{0}{1}.{2}", FilePathGenerate.GetPath(Mid), upimg.FileName, Ext),
                        File_Up_Stat = 1,
                        hidFile_Up_Stat = 0,
                        isTempFile = 0,
                        FIle_Size = Convert.ToString(fileContentLength),
                        Ext = Ext
                    });

                    // 파일 업로드 완료
                    TextUserResponseModel successModel = new TextUserResponseModel()
                    {
                        items = fileModel,
                        rc = 1
                    };

                    return Json(successModel);


                }
            }

            // 파일이 없을경우
            errorModel = new TextUserErrorModel()
            {
                msg = "파일을 선택해 주세요.",
                rc = 2,
                M_ID = Mid,
                Idata = param.Idata,
                Pdata = param.Pdata,
                fileExist = Request.Files.Count

            };

            return Json(errorModel);
        }

        /// <summary>
        ///  공통 이미지 파일 업로드
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ImageTempAttach(string Type, TextUserRequestModel param)
        {
            string Mid = AuthUser.M_ID;
            TextUserErrorModel errorModel = null;

            if (!string.IsNullOrWhiteSpace(param.Idata) && string.IsNullOrWhiteSpace(param.M_ID))
            {
                Mid = AuthUser.Decrypt_AES(param.Idata);
            }

            if (Request.UrlReferrer != null)
                Response.AddHeader("Access-Control-Allow-Origin", Request.UrlReferrer.GetLeftPart(UriPartial.Authority));
            else
                Response.AddHeader("Access-Control-Allow-Origin", "*");

            Response.AddHeader("Access-Control-Allow-Credentials", "true");

            // 파일존재
            if (Request.Files.Count == 1)
            {
                HttpPostedFileBase postfile = Request.Files[0];

                string fileName = string.Empty;
                string fileExt = Path.GetExtension(postfile.FileName);

                string folderName = DateTime.Now.ToString("yyyyMMddHH");

                //파일 저장 위치 ( 넘어오느 Type 값이 저장경로 디렉토리 명)
                string fileSaveFullPath = FilePathGenerate.GetCommonTempFilePath(Type, folderName);//AuthUser.M_ID

                //중복이 나지 않을때까지 파일명 루프         
                string fullFileNamePath = string.Empty;
                do
                {
                    fileName = string.Concat(AuthUser.M_ID, "_", DateTime.Now.ToString("yyyyMMdd"), Path.GetRandomFileName().Substring(0, 5) + fileExt);
                    fullFileNamePath = Path.Combine(fileSaveFullPath, fileName);
                }
                while (System.IO.File.Exists(fullFileNamePath));


                int fileContentLength = postfile.ContentLength;

                // 파일 사이즈 확인
                if (fileContentLength == 0)
                {
                    errorModel = new TextUserErrorModel()
                    {
                        msg = "파일 사이즈 에러",
                        rc = 3
                    };

                    return Json(errorModel);
                }

                // 확장자 확인
                if (!Code.CommonAcceptFileExt.Contains(fileExt, StringComparer.CurrentCultureIgnoreCase))
                {
                    errorModel = new TextUserErrorModel()
                    {
                        msg = "파일 형식이 올바르지 않습니다",
                        rc = 3
                    };

                    return Json(errorModel);
                }

                // 크기 확인
                if (fileContentLength > CommonFileMaxFileSize)
                {
                    errorModel = new TextUserErrorModel()
                    {
                        msg = "업로드된 모든 파일의 용량이 10MB를 초과하였습니다.\n\n첨부파일 용량은 최대 10MB까지 가능합니다.",
                        rc = 4
                    };

                    return Json(errorModel);
                }

                Bitmap bmp = new Bitmap(postfile.InputStream);

                //성공 여부
                bool result = false;
                string Ext = string.Empty;

                ImageUploadHelper upimg = new ImageUploadHelper(fileSaveFullPath, string.Format("{0}", FilePathGenerate.GetCreateRandomFileName()));

                //파일업로드
                result = upimg.WriteImage(postfile, out Ext, true);

                if (result)
                {
                    List<TextUserResponseFileModel> fileModel = new List<TextUserResponseFileModel>();
                    fileModel.Add(new TextUserResponseFileModel()
                    {
                        File_Type = 1,
                        dFileName = string.Format("{0}.{1}", upimg.FileName, Ext),
                        dFilePath = string.Format("{0}/{1}.{2}", folderName, upimg.FileName, Ext),
                        //string.Format("{0}{1}.{2}", FilePathGenerate.GetPath(Mid), upimg.FileName, Ext),
                        File_Up_Stat = 1,
                        hidFile_Up_Stat = 0,
                        isTempFile = 1,
                        FIle_Size = Convert.ToString(fileContentLength),
                        Ext = Ext
                    });

                    // 파일 업로드 완료
                    TextUserResponseModel successModel = new TextUserResponseModel()
                    {
                        items = fileModel,
                        rc = 0
                    };

                    return Json(successModel);


                }
            }

            // 파일이 없을경우
            errorModel = new TextUserErrorModel()
            {
                msg = "파일을 선택해 주세요.",
                rc = 2,
                M_ID = Mid,
                Idata = param.Idata,
                Pdata = param.Pdata,
                fileExist = Request.Files.Count

            };

            return Json(errorModel);
        }

        /// <summary>
        ///  공통 이미지 파일 자르기
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult FileAttachUpdateAjax(string Type, TextUserRequestModel param)
        {
            TextUserErrorModel errorModel = null;
            string AuthId = AuthUser.M_ID;
            try
            {
                if (Request.UrlReferrer != null)
                    Response.AddHeader("Access-Control-Allow-Origin", Request.UrlReferrer.GetLeftPart(UriPartial.Authority));
                else
                    Response.AddHeader("Access-Control-Allow-Origin", "*");

                Response.AddHeader("Access-Control-Allow-Credentials", "true");

                var arrFileInfo = param.hiddFileName.Split('/');
                string affFileName = arrFileInfo[1];
                string affFilefolder = arrFileInfo[0];

                //Temp파일명 (Real 파일명 동일)
                string originFileName = affFileName;

                //Temp 파일경로
                string orginFilePath = FilePathGenerate.GetCommonTempFilePath(param.Type, affFilefolder) + @"\" + affFileName; //AuthUser.M_ID//param.Origin_Filepath;

                //Real 파일경로
                string RealFilePath = string.Concat(FilePathGenerate.GetCommonFilePath(Type, AuthId));

                if (!System.IO.File.Exists(orginFilePath))
                {
                    //에러
                    errorModel = new TextUserErrorModel()
                    {
                        msg = "파일이 존재하지 않습니다.",
                        rc = 1
                    };
                    return Json(errorModel);
                }

                bool iscrop = ((param.CropSize_nw > 0 && param.CropSize_ny > 0) ? true : false);

                //성공여부
                bool result;
                ImageCropHelper cropimg = new ImageCropHelper(orginFilePath, originFileName, RealFilePath);

                result = cropimg.CropImage(param.Origin_w, param.Origin_h, param.CropSize_nx, param.CropSize_ny, param.CropSize_nw, param.CropSize_nh, true, iscrop);

                if (result)
                {
                    List<TextUserResponseFileModel> fileModel = new List<TextUserResponseFileModel>();
                    fileModel.Add(new TextUserResponseFileModel()
                    {
                        File_Type = param.File_Type,
                        dFileName = cropimg.FileName,
                        OldFileName = cropimg.FileName,
                        dFilePath = string.Format("{0}{1}", FilePathGenerate.GetPath(AuthId), cropimg.FileName),
                        Origin_Filepath = param.hiddFileName,
                        File_Up_Stat = param.File_Up_Stat,
                        hidFile_Up_Stat = param.hidFile_Up_Stat,
                        FIle_Size = "0",
                        Ext = "jpeg"
                    });

                    //파일 Crop 완료
                    TextUserResponseModel successModel = new TextUserResponseModel()
                    {
                        items = fileModel,
                        rc = 0
                    };

                    return Json(successModel);
                }
                else
                {
                    //에러
                    errorModel = new TextUserErrorModel()
                    {
                        msg = "파일업로드중 오류가 발생하였습니다.",
                        rc = 4
                    };
                    return Json(errorModel);
                }


            }

            catch (Exception ex)
            {
                return Json(errorModel = new TextUserErrorModel()
                {
                    msg = ex.ToString(),
                    rc = -1
                });
            }

            return Json(errorModel);
        }



        /// <summary>
        /// 개인 정보 유효성 검사
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CheckFileContents()
        {
            Encoding eucKr = Encoding.GetEncoding(51949);
            JKFileContentsFilterResult result = new JKFileContentsFilterResult();

            if (Request.UrlReferrer != null)
                Response.AddHeader("Access-Control-Allow-Origin", Request.UrlReferrer.GetLeftPart(UriPartial.Authority));
            else
                Response.AddHeader("Access-Control-Allow-Origin", "*");

            Response.AddHeader("Access-Control-Allow-Credentials", "true");

            if (Request.Files != null &&
                Request.Files.Count > 0 &&
                Request.Files[0].ContentLength <= FileMaxFileSize)
            {
                result = MKCtx.CMSvc.CheckFileContents();
            }

            return Json(result);
        }

        /// <summary>
        /// 이미지 삭제 => 추천 서비스
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ImageDelete(string Type, string fileName)
        {

            if (Request.UrlReferrer != null)
                Response.AddHeader("Access-Control-Allow-Origin", Request.UrlReferrer.GetLeftPart(UriPartial.Authority));
            else
                Response.AddHeader("Access-Control-Allow-Origin", "*");

            Response.AddHeader("Access-Control-Allow-Credentials", "true");


            TextUserErrorModel errorModel = null;

            if (!string.IsNullOrEmpty(fileName))
            {
                string path = FilePathGenerate.GetRecommandshowFilePath(Type);
                string delFileFullPath = string.Concat(path, @"\", fileName);
                try
                {
                    System.IO.File.Delete(delFileFullPath);
                }
                catch (Exception ex)
                {
                    errorModel = new TextUserErrorModel()
                    {
                        msg = ex.Message,
                        rc = 9
                    };
                }
                // 파일 업로드 완료
                TextUserResponseModel successModel = new TextUserResponseModel()
                {
                    rc = 1
                };

                return Json(successModel);
            }

            errorModel = new TextUserErrorModel()
            {
                msg = "삭제할 파일이 존재하지 않습니다.",
                rc = 9
            };

            return Json(errorModel);
        }

        /// <summary>
        /// 이미지 path 가져오기 => 추천 서비스
        /// </summary>
        /// <param name="path"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public ActionResult Image(string path, string type, int isTempFile = 1)
        {
            string file_name = string.Empty;
            string dir = string.Empty;
            string affFilefolder = string.Empty;
            var arrFileInfo = path.Split('/');

            if (isTempFile == 1)
            {
                file_name = arrFileInfo[1];
                affFilefolder = arrFileInfo[0];
                dir = FilePathGenerate.GetCommonTempFilePath(type, affFilefolder);
            }
            else
            {
                file_name = path;
                dir = FilePathGenerate.GetRecommandshowFilePath(type);
            }

            return base.File(Path.Combine(dir + file_name), "image/jpeg");
        }

        /// <summary>
        /// 개인회원 탈퇴시 파일 삭제 처리 
        /// </summary>
        /// <param name="M_id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Prsn_leave(string M_id = "", string P_Type = "", string IsAdmin = "0")
        {
            if (Request.UrlReferrer != null)
                Response.AddHeader("Access-Control-Allow-Origin", Request.UrlReferrer.GetLeftPart(UriPartial.Authority));
            else
                Response.AddHeader("Access-Control-Allow-Origin", "*");

            Response.AddHeader("Access-Control-Allow-Credentials", "true");


            #region [개인회원 탈퇴- 기본변수 설정]
            /*
           * 유입경로 (PC, M, ADMIN)
           *  jobkorea_file_asp\Leave\Prsn_Proc.asp
           *  jobkorea_pc_net\JobKorea.WebUI\Views\Help\Leave.cshtml
           *  jobkorea_beta_mobile_asp\my\userleave_ok.asp
           *  jobkorea_admin_asp\JK35_Admin\delete\User_del.asp
           *  jobkorea_admin_asp\JK35_Admin\Customer_Center\Leave\Leave_Tongbo_Go_Ok.asp
           */
            //세션 아이디
            string AuthMid = AuthUser.M_ID;
            //Request 아이디
            string MemId = string.IsNullOrWhiteSpace(M_id) ? AuthMid : M_id;
            //Reqeust 구분
            string Ptype = P_Type;
            //탈퇴회원 여부체크
            int SignOutExit = 0;
            //파일 삭제 건수
            int Del_Cnt = 0;
            //반환 Model rc, msg
            TextUserResultModel ResultModel = new TextUserResultModel();

            #endregion [개인회원 탈퇴- 기본변수 설정]

            //#region [개인회원 탈퇴- 아이디 체크]
            //if (string.IsNullOrWhiteSpace(MemId))
            //{
            //    ResultModel = new TextUserResultModel()
            //    {
            //        msg = "로그인 해주세요.",
            //        rc = 1,
            //        pType = P_Type
            //    };

            //    return Content(ViewRendererHelper.RenderPartialView("~/Views/TextUser/Partial/AlertAndClose.cshtml", ResultModel, this.ControllerContext));
            //}

            //// 파라미터 아이디, 세션 아이디 비교
            //if (!string.IsNullOrWhiteSpace(M_id) && !string.IsNullOrWhiteSpace(AuthMid) && IsAdmin.Equals("0"))
            //{
            //    if (!M_id.Equals(AuthMid))
            //    {
            //        ResultModel = new TextUserResultModel()
            //        {
            //            msg = "잘못된 접근입니다.",
            //            rc = 1,
            //            pType = P_Type
            //        };

            //        return Content(ViewRendererHelper.RenderPartialView("~/Views/TextUser/Partial/AlertAndClose.cshtml", ResultModel, this.ControllerContext));
            //    }
            //}
            //#endregion [개인회원 탈퇴- 아이디 체크]

            //#region [개인회원 탈퇴- 탈퇴회원 여부체크]
            //SignOutExit = JKCtx.ETCRpSvc.USP_Leave_SignOut_S(MemId, 1);

            //if (SignOutExit == 0)
            //{
            //    ResultModel = new TextUserResultModel()
            //    {
            //        msg = "비정상적인 경로로 들어오셨습니다.",
            //        rc = 1,
            //        pType = P_Type
            //    };

            //    return Content(ViewRendererHelper.RenderPartialView("~/Views/TextUser/Partial/AlertAndClose.cshtml", ResultModel, this.ControllerContext));
            //}
            //#endregion [개인회원 탈퇴- 탈퇴회원 여부체크]

            //#region [개인회원 탈퇴- 탈퇴 처리 수정 1단계]

            //JKCtx.CCRpSvc.USP_Leave_MemberLeavedDB_U(MemId, 1, 33);

            //#endregion [개인회원 탈퇴- 탈퇴 처리 수정 1단계]

            //#region [개인회원 탈퇴- 입사지원 파일 삭제]
            //var CopassModel = JKCtx.GGRpSvc.USP_Leave_CoPass_S(MemId);

            //if (CopassModel != null && CopassModel.Any())
            //{
            //    foreach (var item in CopassModel)
            //    {
            //        if (!string.IsNullOrWhiteSpace(item.Add_File))
            //        {
            //            string OldFilePath = FilePathGenerate.GetUserCoPassFilePath("", (item.Email_Stat == 1 ? true : false));
            //            OldFilePath = Path.Combine(OldFilePath + item.Add_File);
            //            Del_Cnt = Del_Cnt + 1;
            //            DeleteFile(OldFilePath);
            //        }
            //    }
            //}
            //#endregion [개인회원 탈퇴- 입사지원 파일 삭제]

            //#region [개인회원 탈퇴- 첨부파일 삭제]
            //var UserFileModel = JKCtx.GGRpSvc.USP_Leave_UserFileDB_S(MemId);

            //if (UserFileModel != null && UserFileModel.Any())
            //{
            //    foreach (var item in UserFileModel)
            //    {
            //        if (!string.IsNullOrWhiteSpace(item.File_Name))
            //        {
            //            string OldFilePath = FilePathGenerate.GetUserFilePath(MemId);
            //            OldFilePath = Path.Combine(OldFilePath + item.File_Name);
            //            Del_Cnt = Del_Cnt + 1;
            //            DeleteFile(OldFilePath);
            //        }
            //    }
            //}
            //#endregion [개인회원 탈퇴- 첨부파일 삭제]

            //#region [개인회원 탈퇴- 탈퇴 처리 수정 2단계 ]

            //JKCtx.CCRpSvc.USP_Leave_MemberLeavedDB_U(MemId, 1, 34);

            //#endregion [개인회원 탈퇴- 탈퇴 처리 수정 2단계]

            //#region [개인회원 탈퇴- 비밀번호 분실신고 파일 삭제]
            //var LossReportModel = JKCtx.ETCRpSvc.USP_Leave_LossReport_S("M", MemId);

            //if (LossReportModel != null && LossReportModel.Any())
            //{
            //    foreach (var item in LossReportModel)
            //    {
            //        if (!string.IsNullOrWhiteSpace(item.Proof_File_Name) && item.Req_Date != null)
            //        {
            //            string OldFilePath = FilePathGenerate.GetLossReportPath(item.Req_Date);
            //            OldFilePath = Path.Combine(OldFilePath + item.Proof_File_Name);
            //            Del_Cnt = Del_Cnt + 1;
            //            DeleteFile(OldFilePath);
            //        }
            //    }
            //}
            //#endregion [개인회원 탈퇴- 비밀번호 분실신고 파일 삭제]

            //#region [개인회원 탈퇴- 개인 사진, 썸네일 파일 삭제]
            //var UserPhotoModel = JKCtx.GGRpSvc.USP_Leave_UserPhoto_S(MemId);

            //if (UserPhotoModel != null && UserPhotoModel.Any())
            //{
            //    foreach (var item in UserPhotoModel)
            //    {
            //        if (!string.IsNullOrWhiteSpace(item.M_Photo_Name))
            //        {
            //            string OldFilePath = FilePathGenerate.GetUserPhotoPath();
            //            OldFilePath = Path.Combine(OldFilePath + item.M_Photo_Name);
            //            Del_Cnt = Del_Cnt + 1;
            //            DeleteFile(OldFilePath);
            //        }

            //        if (!string.IsNullOrWhiteSpace(item.Photo_Thmnl_Name))
            //        {
            //            string OldFilePath = FilePathGenerate.GetUserPhotoThumbnailPath();
            //            OldFilePath = Path.Combine(OldFilePath + item.Photo_Thmnl_Name);
            //            Del_Cnt = Del_Cnt + 1;
            //            DeleteFile(OldFilePath);
            //        }
            //    }
            //}
            //#endregion [개인회원 탈퇴- 개인 사진, 썸네일 파일 삭제]

            //#region [개인회원 탈퇴- 취업지식 아바타 삭제]
            //var AvatorModel = JKCtx.KMRpSvc.USP_Leave_MemberTbl_S("M", MemId);

            //if (AvatorModel != null && AvatorModel.Any())
            //{
            //    foreach (var item in AvatorModel)
            //    {
            //        if (!string.IsNullOrWhiteSpace(item.Avata_Img_file) && item.Reg_Date != null)
            //        {
            //            string OldFilePath = FilePathGenerate.GetUserKnowledgePath(item.Reg_Date, "avata");
            //            OldFilePath = Path.Combine(OldFilePath + item.Avata_Img_file);
            //            Del_Cnt = Del_Cnt + 1;
            //            DeleteFile(OldFilePath);
            //        }
            //    }
            //}
            //#endregion [개인회원 탈퇴- 취업지식 아바타 삭제]

            //#region [개인회원 탈퇴- 취업지식 노하우 삭제]
            //var KnowhowModel = JKCtx.KMRpSvc.USP_Leave_KnowhowTbl_S(MemId, "M");

            //if (KnowhowModel != null && KnowhowModel.Any())
            //{
            //    foreach (var item in KnowhowModel)
            //    {
            //        if (!string.IsNullOrWhiteSpace(item.K_Img_Name) && item.K_W_Date != null)
            //        {
            //            string OldFilePath = FilePathGenerate.GetUserKnowledgePath(item.K_W_Date, "file");
            //            OldFilePath = Path.Combine(OldFilePath + item.K_Img_Name);
            //            Del_Cnt = Del_Cnt + 1;
            //            DeleteFile(OldFilePath);
            //        }
            //    }
            //}
            //#endregion [개인회원 탈퇴- 취업지식 노하우 삭제]

            //#region [개인회원 탈퇴- 취업지식 Q&A 삭제]
            //var QnAQModel = JKCtx.KMRpSvc.USP_Leave_QnAQTbl_S(MemId, "M");

            //if (QnAQModel != null && QnAQModel.Any())
            //{
            //    foreach (var item in QnAQModel)
            //    {
            //        if (!string.IsNullOrWhiteSpace(item.Q_Img_Name) && item.Q_W_Date != null)
            //        {
            //            string OldFilePath = FilePathGenerate.GetUserKnowledgePath(item.Q_W_Date, "file");
            //            OldFilePath = Path.Combine(OldFilePath + item.Q_Img_Name);
            //            Del_Cnt = Del_Cnt + 1;
            //            DeleteFile(OldFilePath);
            //        }
            //    }
            //}
            //#endregion [개인회원 탈퇴- 취업지식 Q&A 삭제]   

            //#region [개인회원 탈퇴- 취업지식 댓글 삭제]
            //var QnAAModel = JKCtx.KMRpSvc.USP_Leave_QnAATbl_S(MemId, "M");

            //if (QnAAModel != null && QnAAModel.Any())
            //{
            //    foreach (var item in QnAAModel)
            //    {
            //        if (!string.IsNullOrWhiteSpace(item.A_Img_Name) && item.A_W_Date != null)
            //        {
            //            string OldFilePath = FilePathGenerate.GetUserKnowledgePath(item.A_W_Date, "file");
            //            OldFilePath = Path.Combine(OldFilePath + item.A_Img_Name);
            //            Del_Cnt = Del_Cnt + 1;
            //            DeleteFile(OldFilePath);
            //        }
            //    }
            //}
            //#endregion [개인회원 탈퇴- 취업지식 댓글 삭제]   

            //#region [개인회원 탈퇴- 글로벌도전기 삭제]
            //var GlobalModel = JKCtx.JERpSvc.USP_Leave_GlobalDB_S(MemId);

            //if (GlobalModel != null && GlobalModel.Any())
            //{
            //    foreach (var item in GlobalModel)
            //    {
            //        if (!string.IsNullOrWhiteSpace(item.SC_Album1) && item.W_Date != null)
            //        {
            //            string OldFilePath = FilePathGenerate.GetUserGlobalPath(item.W_Date);
            //            OldFilePath = Path.Combine(OldFilePath + item.SC_Album1);
            //            Del_Cnt = Del_Cnt + 1;
            //            DeleteFile(OldFilePath);
            //        }

            //        if (!string.IsNullOrWhiteSpace(item.SC_Album2) && item.W_Date != null)
            //        {
            //            string OldFilePath = FilePathGenerate.GetUserGlobalPath(item.W_Date);
            //            OldFilePath = Path.Combine(OldFilePath + item.SC_Album2);
            //            Del_Cnt = Del_Cnt + 1;
            //            DeleteFile(OldFilePath);
            //        }

            //        if (!string.IsNullOrWhiteSpace(item.SC_Album3) && item.W_Date != null)
            //        {
            //            string OldFilePath = FilePathGenerate.GetUserGlobalPath(item.W_Date);
            //            OldFilePath = Path.Combine(OldFilePath + item.SC_Album3);
            //            Del_Cnt = Del_Cnt + 1;
            //            DeleteFile(OldFilePath);
            //        }

            //    }
            //}
            //#endregion [개인회원 탈퇴- 글로벌도전기 삭제]

            //#region [개인회원 탈퇴- 고객상담 삭제]
            //var FaqMailModel = JKCtx.CCRpSvc.USP_Leave_FaqMail_S(MemId, "M");

            //if (FaqMailModel != null && FaqMailModel.Any())
            //{
            //    foreach (var item in FaqMailModel)
            //    {
            //        if (!string.IsNullOrWhiteSpace(item.Add_File))
            //        {
            //            string OldFilePath = FilePathGenerate.GetHelpFilePath();
            //            OldFilePath = Path.Combine(OldFilePath + item.Add_File);
            //            Del_Cnt = Del_Cnt + 1;
            //            DeleteFile(OldFilePath);
            //        }
            //    }
            //}
            //#endregion [개인회원 탈퇴- 고객상담 삭제]   

            //#region [개인회원 탈퇴- 공모전 삭제]
            //var ContstModel = JKCtx.CTRpSvc.USP_Cntst_AddFile_S(MemId, "M");

            //if (ContstModel != null && ContstModel.Any())
            //{
            //    foreach (var item in ContstModel)
            //    {
            //        if (!string.IsNullOrWhiteSpace(item.Poster_File_Name))
            //        {
            //            string OldFilePath = FilePathGenerate.GetUserContestPath();
            //            OldFilePath = Path.Combine(OldFilePath + item.Poster_File_Name);
            //            Del_Cnt = Del_Cnt + 1;
            //            DeleteFile(OldFilePath);
            //        }

            //        if (!string.IsNullOrWhiteSpace(item.Rcmd_Cntst_Bnnr_File_Name))
            //        {
            //            string OldFilePath = FilePathGenerate.GetUserContestPath();
            //            OldFilePath = Path.Combine(OldFilePath + item.Rcmd_Cntst_Bnnr_File_Name);
            //            Del_Cnt = Del_Cnt + 1;
            //            DeleteFile(OldFilePath);
            //        }

            //        if (!string.IsNullOrWhiteSpace(item.Attach_File_Name))
            //        {
            //            string OldFilePath = FilePathGenerate.GetUserContestPath();
            //            OldFilePath = Path.Combine(OldFilePath + item.Attach_File_Name);
            //            Del_Cnt = Del_Cnt + 1;
            //            DeleteFile(OldFilePath);
            //        }

            //    }
            //}
            //#endregion [개인회원 탈퇴- 글로벌도전기 삭제]

            //#region [개인회원 탈퇴- 탈퇴 처리 수정 3단계 ]

            //JKCtx.CCRpSvc.USP_Leave_MemberLeavedDB_U(MemId, 1, 35);

            //#endregion [개인회원 탈퇴- 탈퇴 처리 수정 3단계]

            //#region [개인회원 탈퇴- DB 정보 삭제 ]

            //JKCtx.GGRpSvc.USP_Leave_File2_Prsn_D(MemId);

            //#endregion [개인회원 탈퇴- DB 정보 삭제]

            //#region [개인회원 탈퇴- 탈퇴 처리 수정 4단계 ]

            //JKCtx.CCRpSvc.USP_Leave_MemberLeavedDB_U(MemId, 1, 100);

            //#endregion [개인회원 탈퇴- 탈퇴 처리 수정 4단계]

            //#region [개인회원 탈퇴- 통계 업데이트 ]

            ////삭제된 파일이 있을경우
            //if (Del_Cnt > 0)
            //{
            //    JKCtx.ETCRpSvc.USP_Leave_SttJKAddFileDel_I("M", Del_Cnt);
            //}
            //#endregion [개인회원 탈퇴- 통계 업데이트]

            //정상처리 완료
            ResultModel = new TextUserResultModel()
            {
                msg = "개인회원" + MemId + "의 탈퇴처리가 완료되었습니다.",
                rc = 0,
                pType = P_Type.ToLower()
            };

            return Content(ViewRendererHelper.RenderPartialView("~/Views/TextUser/Partial/AlertAndClose.cshtml", ResultModel, this.ControllerContext));
        }

        public static void DeleteFile(string filePath)
        {
            try
            {
                //파일 존재여부 체크
                if (!string.IsNullOrEmpty(filePath) && System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }
            catch (Exception ex)
            {
            }
        }

        public static string FilesExist(string filePath)
        {
            string result = "";
            try
            {
                //파일 존재여부 체크
                if (!string.IsNullOrEmpty(filePath) && System.IO.File.Exists(filePath))
                {
                    result = filePath + "=true|";
                }
                else
                {
                    result = filePath + "=false|";
                }
            }
            catch (Exception ex)
            {
            }

            return result;
        }

        /// <summary>
        /// 개인회원 탈퇴시 파일 삭제 처리 
        /// </summary>
        /// <param name="M_id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult FileExistCheck(string M_id = "")
        {
            if (Request.UrlReferrer != null)
                Response.AddHeader("Access-Control-Allow-Origin", Request.UrlReferrer.GetLeftPart(UriPartial.Authority));
            else
                Response.AddHeader("Access-Control-Allow-Origin", "*");

            Response.AddHeader("Access-Control-Allow-Credentials", "true");

            int Del_Cnt = 0;
            string retContent = "";

            //var UserFileModel = JKCtx.GGRpSvc.USP_Leave_UserFileDB_S(M_id);

            //if (UserFileModel != null && UserFileModel.Any())
            //{
            //    foreach (var item in UserFileModel)
            //    {
            //        if (!string.IsNullOrWhiteSpace(item.File_Name))
            //        {
            //            string OldFilePath = FilePathGenerate.GetUserFilePath(M_id);
            //            OldFilePath = Path.Combine(OldFilePath + item.File_Name);
            //            Del_Cnt = Del_Cnt + 1;
            //            retContent += FilesExist(OldFilePath);
            //        }
            //    }
            //}


            TextUserResultModel ResultModel = new TextUserResultModel();
            ResultModel = new TextUserResultModel()
            {
                msg = retContent + "Del_Cnt=" + Del_Cnt,
                rc = 0
            };

            return Content(ViewRendererHelper.RenderPartialView("~/Views/TextUser/Partial/FileExistCheck.cshtml", ResultModel, this.ControllerContext));
        }

        [HttpPost]
        public JsonResult FileAppendAjax(TextUserRequestModel param)
        {
            TextUserErrorModel errorModel = null;

            try
            {
                Encoding eucKr = Encoding.GetEncoding(51949);

                if (string.IsNullOrEmpty(AuthUser.M_ID))
                {
                    errorModel = new TextUserErrorModel()
                    {
                        msg = "로그인이 필요합니다.\n로그인 후 다시 이용해주세요.",
                        rc = 1
                    };

                    return Json(errorModel);
                }

                if (Request.Files == null || Request.Files.Count == 0)
                {
                    errorModel = new TextUserErrorModel()
                    {
                        msg = "파일을 선택해 주세요.",
                        rc = 2
                    };

                    return Json(errorModel);
                }
                else if (Request.Files.Count == 1)
                {
                    HttpPostedFileBase file = Request.Files[0];
                    string requestFileName = MKCtx.CMSvc.ReplaceBadCharacter(file.FileName);
                    string fileName = Utility.Decrypt_AES(AuthUser.M_ID).Split('@')[0] + "_" + Path.GetFileNameWithoutExtension(requestFileName);
                    string originFileName = Path.GetFileName(requestFileName);
                    originFileName = originFileName.IsNormalized(NormalizationForm.FormD) ? originFileName.Normalize() : originFileName;

                    // 특수문자 % Replace - 모바일 파일 다운로드 오류 수정 
                    if (fileName.IndexOf("%") >= 0) fileName = fileName.Replace("%", "_");

                    // Mac Unicode 오류 수정
                    if (fileName.IsNormalized(NormalizationForm.FormD)) fileName = fileName.Normalize();

                    // DB 기록이 불가능한 파일명인 경우 파일명 변환
                    if (eucKr.GetString(eucKr.GetBytes(fileName)).IndexOf("?") > -1)
                    {
                        fileName = string.Concat(Utility.Decrypt_AES(AuthUser.M_ID).Split('@')[0], "_", DateTime.Now.ToString("yyyyMMdd"), Path.GetRandomFileName().Substring(0, 3).ToUpper());
                    }

                    string fileExt = Path.GetExtension(file.FileName);
                    string fileContentType = file.ContentType;
                    int fileContentLength = file.ContentLength;
                    string folderName = string.Concat(FilePathGenerate.GetUserFilePath(Utility.Decrypt_AES(AuthUser.M_ID)).Split('@')[0]);
                    string fileSaveFullPath = FilePathGenerate.GetUserFilePath(Utility.Decrypt_AES(AuthUser.M_ID).Split('@')[0]);
                    int fileDupeCnt = 0;

                    // 파일 사이즈 확인
                    if (fileContentLength == 0)
                    {
                        errorModel = new TextUserErrorModel()
                        {
                            msg = "파일을 선택해 주세요.",
                            rc = 2
                        };

                        return Json(errorModel);
                    }

                    // 확장자 확인
                    if (Code.AcceptFileExt.IndexOf(fileExt.ToLower()) == -1)
                    {
                        errorModel = new TextUserErrorModel()
                        {
                            msg = "파일 형식이 올바르지 않습니다.\n\n문서파일, 이미지파일, 압축파일만 가능합니다.",
                            rc = 3
                        };

                        return Json(errorModel);
                    }

                    // 형식 확인
                    if (Code.AcceptTxt.IndexOf(fileContentType) == -1)
                    {
                        errorModel = new TextUserErrorModel()
                        {
                            msg = "파일 형식이 틀립니다.\n\n3MB 이내의 doc, docx, hwp, ppt, pptx, xls, xlsx, pdf, zip, jpg, gif 파일형식만 가능합니다.",
                            rc = 3
                        };

                        return Json(errorModel);
                    }

                    // 크기 확인
                    var total = FilePathGenerate.FileUploadList.Sum(s => Convert.ToInt32(s.FIle_Size));
                    if (total + fileContentLength > FileMaxFileSize)
                    {
                        errorModel = new TextUserErrorModel()
                        {
                            msg = "업로드된 모든 파일의 용량이 10MB를 초과하였습니다.\n\n첨부파일 용량은 최대 10MB까지 가능합니다.",
                            rc = 4
                        };

                        return Json(errorModel);
                    }

                    // 이미 등록된 파일인지 확인 (파일명 + 사이즈로 체크)
                    string fullFileNamePath = string.Concat(fileSaveFullPath, fileName, fileExt);
                    if (System.IO.File.Exists(fullFileNamePath))
                    {
                        bool exist = true;
                        do
                        {
                            fileDupeCnt++;
                            fullFileNamePath = string.Concat(fileSaveFullPath, fileName, "_", fileDupeCnt, fileExt);
                            if (!System.IO.File.Exists(fullFileNamePath)) exist = false;
                        } while (exist);
                    }

                    if (FilePathGenerate.FileUploadList.Where(s => s.FullFileNamePath == fullFileNamePath).Count() > 0)
                    {
                        errorModel = new TextUserErrorModel()
                        {
                            msg = "이미 등록된 파일입니다.\n\n파일을 다시 선택해주세요.",
                            rc = 6
                        };

                        return Json(errorModel);
                    }

                    //개인정보 발견시 종료
                    if (param.isCheckFileContent)
                    {
                        var checkResult = MKCtx.CMSvc.CheckFileContents();
                        if (checkResult.InvalidCount > 0)
                        {
                            errorModel = new TextUserErrorModel()
                            {
                                msg = $"업로드 파일에서 주민번호로 의심되는 정보가 {checkResult.InvalidCount}건 검출되었습니다. 개인정보보호를 위해 개인정보가 포함된 파일은 등록할 수 없습니다. 개인정보 삭제 후 다시 업로드 해주세요.",
                                rc = 5
                            };
                            return Json(new { msg = errorModel.msg, rc = errorModel.rc }, JsonRequestBehavior.AllowGet);
                        }
                    }

                    FilePathGenerate.FileUploadList.Add(new TextUserResponseFileModel()
                    {
                        File_Type = param.File_Type,
                        dFileName = Path.GetFileName(fullFileNamePath),
                        OldFileName = Path.GetFileName(fullFileNamePath).Replace(string.Concat(AuthUser.M_ID, "_"), ""),
                        Origin_FileName = originFileName,
                        File_Up_Stat = param.File_Up_Stat,
                        hidFile_Up_Stat = param.hidFile_Up_Stat,
                        FIle_Size = Convert.ToString(fileContentLength),
                        Ext = fileExt,
                        FullFileNamePath = fullFileNamePath,
                        FileBases = file
                    });

                    TextUserResponseModel successModel = new TextUserResponseModel()
                    {
                        rc = 0,
                        msg = FilePathGenerate.FileUploadList.Sum(s => Convert.ToInt32(s.FIle_Size)).ToString()
                    };

                    return Json(successModel);
                }
            }
            catch (Exception ex)
            {
                return Json(errorModel = new TextUserErrorModel()
                {
                    msg = ex.ToString(),
                    rc = -1
                });
            }

            return Json(errorModel);
        }

        [HttpPost]
        public JsonResult RemoveFile(string fileName)
        {
            foreach (var item in FilePathGenerate.FileUploadList)
            {
                if (item.Origin_FileName == fileName)
                {
                    FilePathGenerate.FileUploadList.Remove(item);
                    break;
                }
            }

            TextUserResponseModel successModel = new TextUserResponseModel()
            {
                items = FilePathGenerate.FileUploadList,
                rc = 0,
                msg = FilePathGenerate.FileUploadList.Sum(s => Convert.ToInt32(s.FIle_Size)).ToString()
            };
            return Json(successModel);
        }

        [HttpPost]
        public JsonResult RegistData(RegDbModel regDb)
        {
            TextUserErrorModel errorModel = null;

            try
            {
                // 파일저장
                foreach (var item in FilePathGenerate.FileUploadList)
                {
                    var docType = regDb.FileList.Where(p => item.Origin_FileName.Contains(p.Split(',')[1])).First().Split(',')[0];
                    var idx = new GGRpSvc().USP_UserFileDB_I(Utility.Decrypt_AES(AuthUser.M_ID), docType, item.FullFileNamePath);
                    if (regDb.FileIdx == null) regDb.FileIdx = new List<int>();
                    regDb.FileIdx.Add(idx);
                    item.FileBases.SaveAs(item.FullFileNamePath);
                }

                // 의뢰서 DB저장
                var result = new OfferSvc().USP_RegistOffer_I(regDb);
                if (result.First().ResutCode == 0)
                {
                    FilePathGenerate.FileUploadList.Clear();
                    TextUserResponseModel successModel = new TextUserResponseModel()
                    {
                        items = null,
                        rc = 0,
                        msg = FilePathGenerate.FileUploadList.Sum(s => Convert.ToInt32(s.FIle_Size)).ToString()
                    };
                    return Json(successModel);
                }
                else
                {
                    return Json(errorModel = new TextUserErrorModel()
                    {
                        msg = result.First().ResultMsg,
                        rc = -1
                    });
                }
            }
            catch (Exception ex)
            {
                return Json(errorModel = new TextUserErrorModel()
                {
                    msg = ex.ToString(),
                    rc = -1
                });
            }

        }
    }
}