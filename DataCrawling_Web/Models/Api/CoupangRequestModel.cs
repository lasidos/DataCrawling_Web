using System;
using System.Collections.Generic;

namespace DataCrawling_Web.Models.Api
{
    public class CoupangRequestModel
    {
        // 노출카테고리코드
        // 카테고리 목록 조회 API 또는 카테고리 정보 excel을 다운받아 노출카테고리코드 확인 가능
        // ※ 미입력 시, 카테고리 자동매칭 서비스에 의해 자동으로 카테고리가 등록될 수 있습니다.
        public int DisplayCategoryCode { get; set; }

        // 등록상품명
        // 발주서에 사용되는 상품명
        // 최대 길이 : 100 자
        public string SellerProductName { get; set; }

        // 판매자ID (=업체코드) 
        public string VendorId { get; set; }

        // 판매시작일시
        // "yyyy-MM-dd'T'HH:mm:ss" 형식
        public DateTime SaleStartedAt { get; set; }

        // 판매종료일시
        // "yyyy-MM-dd'T'HH:mm:ss" 형식, *2099년 까지 길게 선택 가능
        public DateTime SaleEndedAt { get; set; }

        // 노출상품명
        // 실제 쿠팡 판매페이지에서 노출되는 상품명.
        // [brand]+[generalProductName] 과 동일하게 입력할 것을 권장, 미입력 상태로도 등록 가능
        // 미입력 시[brand]+[generalProductName] 이 노출되거나, [sellerProductName]이 노출될 수 있음
        // 최대 길이 : 100 자
        public string DisplayProductName { get; set; }

        // 브랜드
        // 브랜드명은 한글/영어 표준이름 입력
        // 띄어쓰기 및 특수문자 없이 입력
        public string Brand { get; set; }

        // 제품명
        // 구매옵션[Attribute exposed] 정보(사이즈, 색상 등)를 포함하지 않는 상품명.모델명 추가 기입 가능
        public string GeneralProductName { get; set; }

        // 상품군
        // 상품의 종류를 나타내는 명칭으로 노출카테고리의 최하위명을 참고하여 입력 가능.
        // 제품명[generalProductName] 과 중복될 경우, 입력 불필요
        public string ProductGroup { get; set; }

        // 배송방법
        public string DeliveryMethod { get; set; }

        // 택배사 코드
        public string DeliveryCompanyCode { get; set; }

        // 배송비종류
        public string DeliveryChargeType { get; set; }

        // 기본배송비
        // 유료배송 또는 조건부 무료배송 시, 편도 배송비 금액 입력
        public decimal DeliveryCharge { get; set; }

        // 무료배송을 위한 조건 금액
        // ● 예시 : 10,000원 이상 조건부 무료배송을 설정하기 원할 경우[deliveryChargeType] 을
        // 'CONDITIONAL_FREE'로 설정 후, [freeShipOverAmount] 에 10000을 입력
        // ※ 100원 이상 단위로 입력 가능
        // ※ 무료배송인 경우, 0 입력
        public decimal FreeShipOverAmount { get; set; }

        // 초도반품배송비
        // 무료배송인 경우 반품시 소비자가 지불하는 배송비
        public decimal DeliveryChargeOnReturn { get; set; }

        // 도서산간 배송여부
        // Y 도서산간 배송
        // N 도서산간 배송안함
        public string RemoteAreaDeliverable { get; set; }

        // 묶음 배송여부
        // UNION_DELIVERY	묶음 배송 가능
        // NOT_UNION_DELIVERY 묶음 배송 불가능
        public string UnionDeliveryType { get; set; }

        // 반품 센터 코드
        public string ReturnCenterCode { get; set; }

        // 반품지명
        public string ReturnChargeName { get; set; }

        // 반품지연락처
        public string CompanyContactNumber { get; set; }

        // 반품지우편번호
        public string ReturnZipCode { get; set; }

        // 반품지주소
        public string ReturnAddress { get; set; }

        // 반품지주소상세
        public string ReturnAddressDetail { get; set; }

        // 반품배송비
        public decimal ReturnCharge { get; set; }

        // 출고지주소코드
        public string OutboundShippingPlaceCode { get; set; }

        // 실사용자아이디(쿠팡 Wing ID)
        public string VendorUserId { get; set; }

        // 자동승인요청여부
        // 상품 등록 시, 자동으로 판매승인요청을 진행할지 여부 선택
        //  false : 작성 내용만 저장하고 판매요청 전 상태(판매를 원할 시에는 상품 승인요청 API 또는 wing에서 판매요청을 진행 해야 함)
        //  true : 저장 및 자동으로 판매 승인 요청
        public bool Requested { get; set; }

        // 업체상품옵션목록
        // 최대 200개 옵션 등록 가능
        public List<Item> Items { get; set; }

        // 구비 서류 필수인 경우 입력
        // 구비서류는 5MB이하의 파일 입력 가능(PDF, HWP, DOC, DOCX, TXT, PNG, JPG, JPEG)
        public List<RequiredDocument> RequiredDocuments { get; set; }

        public string ExtraInfoMessage { get; set; }

        public string Manufacture { get; set; }

        public List<Bundle> BundleInfo { get; set; }
    }

    // Item 클래스는 제품의 세부 정보를 정의합니다.
    public class Item
    {
        // 업체상품옵션명
        // 각각의 아이템에 중복되지 않도록 기입
        // 사이트에 노출되는 옵션명이 아니며, 구매옵션에 따라 변경될 수 있음
        // 최대 길이 : 150자 제한
        public string ItemName { get; set; }

        // 할인율기준가 (정가표시)
        // 할인율(%)표시를 위한 할인전 금액으로, 판매가격과 동일하게 입력시
        // '쿠팡가'로 노출.승인완료 이후 할인율기준가 수정은[옵션별 할인율기준가 변경] API를 통해 변경가능
        public decimal OriginalPrice { get; set; }

        // 판매가격
        // '최초' 업체상품 등록시 판매가격은 상품 승인 요청 전에만 가능하며,
        // 승인완료 이후 판매가격 수정은 [옵션별 가격 변경] API를 통해 변경가능
        public decimal SalePrice { get; set; }

        // 판매가능수량
        // 판매가능한 재고수량을 입력.
        // '최초' 업체상품 등록시 판매수량은 상품 승인 요청 전에만 가능하며,
        // 승인완료 이후 재고 수정은[옵션별 수량 변경] API를 통해 변경가능
        // 최대값 : 99999
        public int MaximumBuyCount { get; set; }

        // 인당 최대 구매 수량
        // 1인당 최대 구매 가능한 수량.
        // 제한이 없을 경우 ‘0’을 입력
        // (예: 인당 최대 구매 수량 100, 최대 구매 수량 기간 3 입력 시,
        // 1인당 3일 동안 최대 100개까지 구매 가능함을 의미)
        public int MaximumBuyForPerson { get; set; }

        // 최대 구매 수량 기간
        // 1인당 해당 상품을 구매할 수 있는 주기 설정.
        // 제한이 없을 경우 ‘1’을 입력
        // (예: 인당 최대 구매 수량 100, 최대 구매 수량 기간 3 입력 시, 1인당 3일 동안 최대 100개까지 구매 가능함을 의미)
        public int MaximumBuyForPersonPeriod { get; set; }

        // 기준출고일(일)
        // 주문일(D-Day) 이후 배송을 위한 출고예정일자를 '일' 단위로 입력.
        // (다음날(D+1) 출고일 경우 '1' 입력)
        public int OutboundShippingTimeDay { get; set; }

        // 단위수량
        // 상품에 포함된 수량을 입력하면(판매가격 ÷ 단위수량) 로 계산하여(1개당 가격 #,000원) 으로 노출.
        // 개당가격이 필요하지 않은 상품은 '0'을 입력
        public int UnitCount { get; set; }

        // 성인 전용 여부
        public string AdultOnly { get; set; }
        // 세금 유형
        public string TaxType { get; set; }
        // 병행 수입 여부
        public string ParallelImported { get; set; }
        // 해외 구매 여부
        public string OverseasPurchased { get; set; }

        // PCC 필요 여부
        // true	 고객이 PCC 입력 후 구매 가능 (PCC는 발주서에 포함 됨) 
        // false 고객이 PCC를 입력하지 않고 상품 구매 가능
        public bool PccNeeded { get; set; }

        // 판매자상품코드 (업체상품코드)
        // 업체고유의 item 코드값을 임의로 세팅할 수 있으며, 입력값은 발주서 조회 API response 에 포함됨.
        public string ExternalVendorSku { get; set; }

        // 바코드
        // 상품에 부착 된 유효한 표준상품 코드
        public string Barcode { get; set; }

        // 바코드 없음
        // 바코드가 없으면 true
        public bool EmptyBarcode { get; set; }

        // 바코드 없음에 대한 사유
        // 최대길이 : 100 자 제한
        public string EmptyBarcodeReason { get; set; }

        // 모델 번호
        public string ModelNo { get; set; }
        // 추가 속성
        public ExtraProperties ExtraProperties { get; set; }

        // 상품인증정보
        // 상품 인증 정보
        public List<Certification> Certifications { get; set; }

        // 검색어
        // 필요한 만큼 반복입력가능. ["검색어1", "검색어2"]
        // 1개의 검색어 당 20자 이내로, 최대 20개의 검색어를 입력가능, !@#$%^&*-+;:’. 외의 특수문자는 입력불가
        public List<string> SearchTags { get; set; }

        // 이미지목록
        // 필요한 만큼 반복 입력 가능
        public List<Image> Images { get; set; }

        // 상품고시정보 목록
        // 카테고리 메타 정보 조회 API 또는 전체카테고리 리스트 Excel file을 통해,
        // 필요한 고시정보 항목의 확인 및 선택 가능
        public List<Notice> Notices { get; set; }

        // 옵션목록(속성)
        // 카테고리 기준으로 정해진 옵션 목록을 입력하는 객체
        // 등록하기 원하는 구매옵션 개수 만큼 반복 입력 가능
        // ● 구매옵션(attribute exposed)의 모든 값이 중복될 경우 등록 불가
        // ● 한개 이상 필수 등록
        public List<Attribute> Attributes { get; set; }

        // 컨텐츠목록
        // 필요한 만큼 반복 입력 가능
        public List<Content> Contents { get; set; }

        // 상품상태
        // 상품 생성 후에는 offerCondition 변경 불가능
        // 노출카테고리 코드에 따라 아래 값을 선택가능, 없을 경우 NEW로 취급함
        public string OfferCondition { get; set; }

        // 중고상품 상세설명
        // 중고상품 상태를 설명, 700자 제한
        // offerCondition을 중고로 입력한 경우에만 작성
        public string OfferDescription { get; set; }
    }

    // ExtraProperties 클래스는 추가 속성을 정의합니다.
    public class ExtraProperties
    {
        public List<KeyValuePair<string, string>> Keys { get; set; }

        public ExtraProperties()
        {
            Keys = new List<KeyValuePair<string, string>>();
    }

        public void AddKeyValue(string key, string value)
        {
            Keys.Add(new KeyValuePair<string, string>(key, value));
        }
    }

    // Certification 클래스는 인증 정보를 정의합니다.
    public class Certification
    {
        public Certification()
        {
            CertificationAttachments = new List<Dictionary<string, string>>();
        }

        // 인증정보Type
        // 카테고리 메타정보 조회 API를 통해 등록가능한 Type을 구할 수 있다.
        // 인증대상이 아닌 카테고리일 경우 : NOT_REQUIRED
        public string CertificationType { get; set; }

        // 상품인증정보코드
        // 인증기관에서 발급받은 코드
        public string CertificationCode { get; set; }

        // 인증 첨부파일 목록
        public List<Dictionary<string, string>> CertificationAttachments { get; set; }
    }

    // Image 클래스는 이미지 정보를 정의합니다.
    public class Image
    {
        // 이미지 순서 0,1,2...
        public int ImageOrder { get; set; }

        // 대표이미지 타입
        // 3MB 이하의 정사각형 이미지를 JPG, PNG로 등록 가능(최소 500 x 500px, 최대 5000 x 5000px)
        // ● 필수
        // REPRESENTATION : 정사각형 대표이미지
        // ● 선택
        // DETAIL : 기타이미지(최대 9개까지 등록 가능)
        // USED_PRODUCT : 중고상태 이미지(최대 4개까지 등록 가능)
        public string ImageType { get; set; }

        // 쿠팡CDN경로
        // 쿠팡 CDN 에 올린 경우 직접 입력, vendorPath와 cdnPath 중 하나 이상 필수
        // 최대 길이 : 200 자
        public string CdnPath { get; set; }

        // 업체이미지경로
        // 업체에서 사용하는 이미지 경로, http://로 시작하는 경로일 경우
        // 자동 다운로드하여 쿠팡 CDN에 추가됨, vendorPath와 cdnPath 중 하나 이상 필수
        // 80, 443 Port 이미지 경로만 사용가능
        // 최대 길이 : 200 자
        public string VendorPath { get; set; }
    }

    // Notice 클래스는 공지 정보를 정의합니다.
    public class Notice
    {
        // 상품고시정보카테고리명
        // 카테고리별 입력 가능한 상품고시정보 카테고리 중 하나를 입력
        // 카테고리 메타 정보 조회 API 또는 전체 카테고리 정보 Excel file을 통해,
        // 필요한 고시정보 항목의 확인 및 선택 가능
        public string NoticeCategoryName { get; set; }

        // 상품고시정보카테고리상세명
        public string NoticeCategoryDetailName { get; set; }

        // 내용
        public string Content { get; set; }
    }

    // Attribute 클래스는 속성 정보를 정의합니다.
    public class Attribute
    {
        // 옵션타입명
        // 카테고리 메타 정보 조회 API 또는 전체카테고리 리스트 다운로드 엑셀 파일을 통해,
        // 카테고리에 맞는 옵션타입명 확인 및 선택 가능(구매옵션은 자유롭게 입력 가능)
        // 최대 길이 25자 제한
        public string AttributeTypeName { get; set; }

        // 옵션값
        // 옵션타입명[attributeTypeName] 에 해당하는 Value를 단위와 함께 입력(예시 : "200ml")
        // 최대 길이 30자 제한
        public string AttributeValueName { get; set; }

        // 옵션과 검색필터를 구분하는 항목
        // 해당 항목을 제외하고 등록할 경우, 옵션으로 등록되고
        // 해당 항목에 "NONE"이라는 값을 작성할 경우 필터로 등록 됨
        // 검색필터는 반드시 등록하는 카테고리와 일치하는 필터명만 사용 가능
        public string Exposed { get; set; }
    }

    // Content 클래스는 내용 정보를 정의합니다.
    public class Content
    {
        // 컨텐츠타입
        public string ContentsType { get; set; }

        // 상세컨텐츠목록
        public List<ContentDetail> ContentDetails { get; set; }
    }

    // ContentDetail 클래스는 내용 세부 정보를 정의합니다.
    public class ContentDetail
    {
        // 내용
        public string Content { get; set; }
        // 세부 유형
        public string DetailType { get; set; }
    }

    public class RequiredDocument
    {
        // 구비서류템플릿명
        // 카테고리 메타 정보 조회 API에서 확인 가능
        public string TemplateName { get; set; }

        // 구비서류 쿠팡CDN경로
        // documentPath 와 vendorDocumentPath 중 하나 필수
        // 최대 길이 : 150자
        public string DocumentPath { get; set; }

        // 구비서류벤더경로
        // 구비서류 경로, http://로 시작하는 경로일 경우 자동 다운로드하여 쿠팡 CDN에 추가됨. documentPath 와 vendorDocumentPath 중 하나 필수
        // 최대 길이 : 150자
        public string VendorDocumentPath { get; set; }
    }

    public class Bundle
    {
        public Bundle()
        {
            BundleList = new List<Dictionary<string, string>>();
        }

        // 번들상품 구분
        // ● SINGLE : 단일구성상품(기본값)
        // ● AB : 혼합구성상품
        // 혼합 구성 상품 등록 시에는 옵션을 구성할 수 없으며,
        // 번들 상품 정보를 추가한 뒤에는 번들 상품 값을 수정할 수 없습니다.
        // 이는 새로운 상품 등록과 동일한 것으로 간주되어 적용된 사항입니다.
        public string BundleType { get; set; }

        // 인증 첨부파일 목록
        public List<Dictionary<string, string>> BundleList { get; set; }
    }

    #region Enum

    // 배송비 종류
    public enum DeliveryChargeType
    {
        FREE,                // 무료배송
        NOT_FREE,            // 유료배송
        CHARGE_RECEIVED,     // 착불배송
        CONDITIONAL_FREE     // 조건부 무료배송
    }

    // 배송방법
    public enum DeliveryMethod
    {
        SEQUENCIAL,    // 일반배송 (순차배송)
        COLD_FRESH,    // 신선냉동
        MAKE_ORDER,    // 주문제작
        AGENT_BUY,     // 구매대행
        VENDOR_DIRECT   // 설치배송 또는 판매자 직접 전달
    }

    // 성인 전용 여부
    public enum AgeRestriction
    {
        ADULT_ONLY = 1,  // 19세이상 구입 가능 상품
        EVERYONE = 2     // 전연령 구입 가능 상품 (기본값)
    }

    // 세금 유형
    public enum TaxType
    {
        TAX,    // 과세 (기본값)
        FREE    // 비과세
    }

    // 병행 수입 여부
    public enum ParallelImported
    {
        PARALLEL_IMPORTED,        // 병행수입
        NOT_PARALLEL_IMPORTED     // 병행수입 아님 (기본값)
    }

    // 해외 구매 여부
    public enum OverseasPurchased
    {
        OVERSEAS_PURCHASED,        // 구매대행
        NOT_OVERSEAS_PURCHASED     // 구매대행 아님 (기본값)
    }

    public enum ContentType
    {
        IMAGE,            // 이미지
        IMAGE_NO_SPACE,   // 이미지(공백없음)
        TEXT,             // 텍스트
        IMAGE_TEXT,       // 이미지-텍스트
        TEXT_IMAGE,       // 텍스트-이미지
        IMAGE_IMAGE,      // 이미지-이미지
        TEXT_TEXT,        // 텍스트-텍스트
        TITLE,            // 제목
        HTML              // HTML
    }

    public enum ProductCondition
    {
        NEW = 1,            // 새 상품
        REFURBISHED = 2,   // 리퍼
        USED_BEST = 3,     // 중고(최상)
        USED_GOOD = 4,     // 중고(상)
        USED_NORMAL = 5     // 중고(중)
    }

    #endregion
}