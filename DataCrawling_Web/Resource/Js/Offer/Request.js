$(function () {
    /*var regFileCollection = new RegFileCollection(JSON.parse('[{\"IDX\":9634966,\"M_ID\":\"jmtest22\",\"File_Type\":7,\"File_Up_Stat\":1,\"File_Name\":\"speech-7228844_1280_1.jpg\",\"OpenStat\":null,\"RegDate\":\"\\/Date(-62135596800000)\\/\",\"File_Size\":null,\"Ext_Str\":\"jpg\",\"Display_File_Name\":\"speech-7228844_1280.jpg\"},{\"IDX\":9634957,\"M_ID\":\"jmtest22\",\"File_Type\":7,\"File_Up_Stat\":1,\"File_Name\":\"speech-7228844_1280.jpg\",\"OpenStat\":null,\"RegDate\":\"\\/Date(-62135596800000)\\/\",\"File_Size\":null,\"Ext_Str\":\"jpg\",\"Display_File_Name\":\"speech-7228844_1280.jpg\"}]'));*/

    $('.devText').focus(function (e) {
        $('.elWrap').removeClass('on');
        $(this).parents('.elWrap').addClass('on ok');
    });

    $('.btnSelType').click(function (e) {
        $('.elWrap').removeClass('on');
        $(this).parents('.elWrap').addClass('on');
    });

    $('.devText').blur(function (e) {
        if ($(this).hasClass('devText')) {
            if ($('.devText').val() == '') {
                $('.devText').parents('.elWrap').removeClass('ok');
            }
        }
        $(this).parents('.elWrap').removeClass('on');
    });

    $('.resist_content').click(function (e) {
        if (!$(e.target).hasClass('btnSelType') && !$(e.target).hasClass('txCate') && !$(e.target).hasClass('txResult')) {
            $('.btnSelType').parents('.elWrap').removeClass('on');
        }
    });

    $('.devCoTypeItem button').click(function (e) {
        e.preventDefault();
        $(this).parents('.selType').find('.txResult').text($(this).text());
        $(this).parents('.elWrap').addClass('ok');
        console.log($(this).text());
    });

    $('.spec_add .txArea').focus(function () {
        /*$('.spec_add .ph').show();*/
        $(this).parents('.spec_add').find('.ph').hide();
    });

    $('.ph').click(function () {
        $(this).parents('.spec_add').find('.txArea').focus();
    });

    $('.spec_add .txArea').blur(function (e) {
        if ($(this).val() == "") {
            $('.spec_add .ph').show();
        }
    });

    $('.buttonAddUrl').click(function () {
        $("#uploadPortfolioFile").click();
    });

    $('#uploadPortfolioFile').change(function (e) {
        var fileName = $(this).val();
        var fileSize = $(this)[0].files[0].size;
        if (fileSize > 1024 * 1024 * 100) {
            alert("첨부파일 용량은 최대 100MB까지 가능합니다.");
            $(this).val("");
            fileName = "";
        }

        var that = this,
            $source = $(e.currentTarget),
            $dropdown = $source.closest(".dropdown"),
            fileType = $source.data("value"),
            $container = $source.closest("[data-cid]"),
            id = $container.data("cid"),
            $fileEle = $container.find(":file");

        var $cloneFile = $fileEle.clone(true);
        $cloneFile.insertAfter($fileEle);

        var RegFileCollection = Backbone.Collection.extend({ File_Name: "asd" });
        var regFileCollection = new RegFileCollection({ File_Name: "speech-7228844_1280_1.jpg" });
        var item = regFileCollection;
        const formData = new FormData();
        formData.append("image", $(this)[0].files[0]);

        $.ajax({
            type: "POST",
            url: "/TextUser/FileAttachAjax",
            processData: false,
            contentType: false,
            data: formData,
            success: function (rtn) {
                const message = rtn.data.values[0];
                console.log("message: ", message)
                $("#resultUploadPath").text(message.uploadFilePath)
            },
            err: function (err) {
                console.log("err:", err)
            }
        })

        //$("#portfolioUploadFrm").html("").append('<input type="file" id="file_c1164" name="File_Co_Name" style="display:none;" val=' + fileName + '>');
        //$("#portfolioUploadFrm").ajaxSubmit({
        //    xhrFields: {
        //        withCredentials: true
        //    },
        //    beforeSubmit: function () {
        //        $(".modal-spinner").show();
        //    },
        //    success: function (data) {
        //        if (data.rc == "0") {
        //            $.getJSON("/User/Resume/AddUserFileDB",
        //                _.extend(item.toJSON(), { File_Name: data.items[0].dFileName, File_Type: fileType, File_Size: data.items[0].FIle_Size, Display_File_Name: data.items[0].Origin_FileName }),
        //                function (result) {
        //                    item.set({ IDX: result.idx, File_Type: fileType, File_Name: data.items[0].dFileName, Display_File_Name: data.items[0].Origin_FileName });
        //                    that.fileDbCollection.fetch({ reset: true, data: { _: new Date().getTime() } });
        //                    $(".modal-spinner").hide();
        //                });
        //        } else if (data.rc == "-1") {
        //            alert("처리중 에러가 발생하였습니다.");
        //            that.regFileCollection.remove(that.regFileCollection.get(id));
        //        } else {
        //            alert(data.msg);
        //            that.regFileCollection.remove(that.regFileCollection.get(id));
        //        }
        //    },
        //    error: function () {
        //        alert("알수 없는 문제가 발생했습니다.\업로드가 원활치 않으면 고객센터로 연락주세요.\n고객센터 : 1588-9350 / helpdesk@jobkorea.co.kr");
        //    },
        //    complete: function () {
        //        $(".modal-spinner").hide();
        //    }
        //});
        //var fn = fileName.split('\\');
        //$('.fileList .filename').text(fn[fn.length - 1]);
        //console.log(fileName);
    });
});