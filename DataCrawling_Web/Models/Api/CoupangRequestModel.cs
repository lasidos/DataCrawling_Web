using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Web.UI.WebControls;

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
    }

    // Item 클래스는 제품의 세부 정보를 정의합니다.
    public class Item
    {
        // 아이템 이름
        public string ItemName { get; set; }
        // 원래 가격
        public decimal OriginalPrice { get; set; }
        // 판매 가격
        public decimal SalePrice { get; set; }
        // 최대 구매 수량
        public int MaximumBuyCount { get; set; }
        // 개인당 최대 구매 수량
        public int MaximumBuyForPerson { get; set; }
        // 발송 소요 시간 (일)
        public int OutboundShippingTimeDay { get; set; }
        // 개인당 최대 구매 기간
        public int MaximumBuyForPersonPeriod { get; set; }
        // 단위 수량
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
        public bool PccNeeded { get; set; }
        // 외부 공급업체 SKU
        public string ExternalVendorSku { get; set; }
        // 바코드
        public string Barcode { get; set; }
        // 바코드 비어 있음 여부
        public bool EmptyBarcode { get; set; }
        // 바코드 비어 있음 이유
        public string EmptyBarcodeReason { get; set; }
        // 모델 번호
        public string ModelNo { get; set; }
        // 추가 속성
        public ExtraProperties ExtraProperties { get; set; }
        // 인증 목록
        public List<Certification> Certifications { get; set; }
        // 검색 태그 목록
        public List<string> SearchTags { get; set; }
        // 이미지 목록
        public List<Image> Images { get; set; }
        // 공지 목록
        public List<Notice> Notices { get; set; }
        // 속성 목록
        public List<Attribute> Attributes { get; set; }
        // 내용 목록
        public List<Content> Contents { get; set; }
        // 제안 조건
        public string OfferCondition { get; set; }
        // 제안 설명
        public string OfferDescription { get; set; }
    }

    // ExtraProperties 클래스는 추가 속성을 정의합니다.
    public class ExtraProperties
    {
        // 쿠팡 판매 가격
        public decimal CoupangSalePrice { get; set; }
        // 도서 온라인 판매 가격
        public decimal OnlineSalePriceForBooks { get; set; }
        // 거래 유형
        public string TransactionType { get; set; }
        // 사업 유형
        public string BusinessType { get; set; }
    }

    // Certification 클래스는 인증 정보를 정의합니다.
    public class Certification
    {
        // 인증 유형
        public string CertificationType { get; set; }
        // 인증 코드
        public string CertificationCode { get; set; }
    }

    // Image 클래스는 이미지 정보를 정의합니다.
    public class Image
    {
        // 이미지 순서
        public int ImageOrder { get; set; }
        // 이미지 유형
        public string ImageType { get; set; }
        // 공급업체 경로
        public string VendorPath { get; set; }
    }

    // Notice 클래스는 공지 정보를 정의합니다.
    public class Notice
    {
        // 공지 카테고리 이름
        public string NoticeCategoryName { get; set; }
        // 공지 세부 카테고리 이름
        public string NoticeCategoryDetailName { get; set; }
        // 내용
        public string Content { get; set; }
    }

    // Attribute 클래스는 속성 정보를 정의합니다.
    public class Attribute
    {
        // 속성 유형 이름
        public string AttributeTypeName { get; set; }
        // 속성 값 이름
        public string AttributeValueName { get; set; }
        // 노출 여부
        public string Exposed { get; set; }
    }

    // Content 클래스는 내용 정보를 정의합니다.
    public class Content
    {
        // 내용 유형
        public string ContentsType { get; set; }
        // 내용 세부 정보 목록
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
}