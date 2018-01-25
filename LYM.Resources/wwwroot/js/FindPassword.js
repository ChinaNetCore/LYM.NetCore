

function step1success(data) {

    $("div.alert-error").hide();
    if (data.status == 1) {

        $("#secretId").val(data.secretId);
        //$("#secretcode").val(data.secretcode);


        var listdata = data.listsecretcode;
        var opion = '';
        for (var i = 0; i < listdata.length; i++)
        {
            opion += "<option value='" + listdata[i].split('|')[0] + "'>" + listdata[i].split('|')[1] + "</option>";
        }
       
        $("#secretcode").html(opion);
        //next();
        $(".content-input-mobile").hide();
        $(".content-reset-password").show();
        $(".fix-1").eq(0).removeClass('current-state');
        $(".fix-1").eq(1).addClass('current-state');
    }
    else {
        $("div.alert-error").html(data.msg).show();
    }

}
function step2success(data) {
    $("div.alert-error").hide();
    if (data.status == 1) {
        next2();
    }
    else {
        $("div.alert-error").html(data.msg).show();
    }
}



function next() {
    $.alert({
        title: '标题',
        content: '' +
        '<form action="" class="formName">' +
        '<select class="name" style="height:30px;line-height:30px;padding-left: 5px;width:100%;"><option>1</option><option>2</option><option>3</option></select>' +
        '</form>',
        buttons: {
            formSubmit: {
                text: '确定',
                btnClass: 'btn-blue',
                action: function () {
                    var name = this.$content.find('.name').val();
                    if (!name) {
                   
                        return false;
                    }
                    //成功
                    $(".content-input-mobile").hide();
                    $(".content-reset-password").show();
                    $(".fix-1").eq(0).removeClass('current-state');
                    $(".fix-1").eq(1).addClass('current-state');


                }
            }
        },
        onContentReady: function () {
            var jc = this;
            this.$content.find('form').on('submit', function (e) {
                e.preventDefault();
                jc.$$formSubmit.trigger('click');
            });
        }
    });
}

function next2() {
    $(".content-reset-password").hide();
    $(".complete").show();
    $(".fix-1").eq(1).removeClass('current-state');
    $(".fix-1").eq(2).addClass('current-state');

    if (!!timer) clearInterval(timer);
    var time = 3;
    $("#hrefTime").html(time);
    var timer = setInterval(function () {
        time--;
        if (time == 0) {

            window.location.href = $('a.to-login').attr("href");
            clearInterval(timer);
        } else
            $("#hrefTime").html(time);

    }, 1000);


}
function sendCode(phone, expkey) {
    $.ajax({
        url: '/Login/SendCode',
        type: 'post',
        dataType: 'json',
        data: { phone: phone, expkey: expkey },
        success: function (data) {
           
            if (data.status == 1) {
                $("div.alert-error").html("").hide();
                $("#codemsg_id").val(data.msg);
                $('#expkey').val(data.expkey);
            }
            else {
                $("div.alert-error").html(data.msg).show();
                
            }
         
        },
        error: function () { }

    });

}

function getCheckCode() {
    if (parseInt($("#mobile").val()).toString().length !== 11) return;
   
    sendCode($("#mobile").val(),$('#expkey').val());
    var getCheckCodeBtn = $("#getCheckCodeBtn");
    getCheckCodeBtn.off("click");
    $("#checkCode").removeAttr("disabled");
    getCheckCodeBtn.css("pointer-events", "none");
    if (!!timer) clearInterval(timer);
    var time = 180;
    getCheckCodeBtn.html("<b style='color:#3c9;'>" + time + "</b>秒内有效");
    var timer = setInterval(function () {
        time--;
        if (time === 0) {
            getCheckCodeBtn.on("click", getCheckCode);
            getCheckCodeBtn.html("获取验证码").css("pointer-events", "all");
            clearInterval(timer);
        } else {
            getCheckCodeBtn.html("<b style='color:#3c9;'>" + time + "</b>秒内有效");
        }
         

    }, 1000);
}

$(function () {

    jQuery.validator.addMethod("userPattern", function (value, element, param) {
        return this.optional(element) || (/^[a-zA-Z0-9_-]+$/.test(value));
    }, $.validator.format("请输入字母，数字，下划线，减号"));

    jQuery.validator.addMethod("isMobile", function (value, element, param) {
        return this.optional(element) || (/^((13[0-9])|(14[5|7])|(15([0-9]))|(17[0-9])|(18[0-9]))\d{8}$/.test(value));
    }, $.validator.format("请输入11位合法的电话号码"));

    $("#getCheckCodeBtn").on("click", getCheckCode);

    $("#step1form").validate({
        rules: {
            //username: {
            //    maxlength: 20,
            //    minlength: 6,
            //    required: true,
            //    userPattern: true
            //    //remote: {
            //    //    url: '/Login/IsHasUserName',
            //    //    type: "post",
            //    //    dataType: "json",
            //    //    data: {
            //    //        username: function () {
            //    //            return $("#username").val();
            //    //        }
            //    //    },
            //    //    dataFilter: function (data, type) {
            //    //        data = JSON.parse(data);
            //    //        if (data.status == 1)
            //    //            return true;
            //    //        else
            //    //            return false;
            //    //    }
            //    //}
            //},
           
            mobile: {
                rangelength: [11, 11],
                required: true,
                isMobile: true
            },
            checkCode: {
                required: true,
                rangelength: [6, 6]
            }
        },
        messages: {
            //username: {
            //    maxlength: "用户名最多20位",
            //    minlength: "用户名最少6字符",
            //    required: "用户名必填",
            //    userPattern: "请输入字母，数字，下划线，减号"
            //    //remote: "该用户名已存在"
            //},

            mobile: {
                rangelength: "请输入11位手机号",
                required: "请输入手机号",
                isMobile: "请输入11位合法的电话号码"

            },
            checkCode: {
                required: "请输入验证码",
                rangelength: "请输入6位验证码"
            }
        },
        submitHandler: function (form) {
           form.ajaxSubmit();
        }
        

    });

    $("#step2form").validate({
        rules: {
           
            password: {
                maxlength: 16,
                minlength: 6,
                required: true
            },
            passwordRep: {
                maxlength: 16,
                minlength: 6,
                required: true,
                equalTo: "#password"

            }
        },
        messages: {
           

            password: {
                maxlength: "密码最多16个字符",
                minlength: "密码最少6个字符",
                required: "请输入密码"
            },
            passwordRep: {
                maxlength: "密码最多16个字符",
                minlength: "密码最少6个字符",
                required: "请输入密码",
                equalTo: "两次密码不一致"

            }
        },
        submitHandler: function (form) {
            form.ajaxSubmit();
        }

    });

  
    document.onkeydown = function (e) {
        var ev = document.all ? window.event : e;
        if (ev.keyCode == 13) {
            $('#registerform').submit();
        }
    }

    //输入时
    $("input[type='text'],input[type='password']").on("input", function () {
        var i = $(this).nextAll("i").eq(0);
        if ($(this).val() !== "") {
            i.removeClass("hide");
        } else {
            i.addClass("hide");
        }
    });

    // 清除input
    $(".del-btn").on("click", function () {
        $(this).addClass("hide");
        $(this).prevAll("input").eq(0).val("");
    });

    $(".del-btn").hover(function () {
        var img = $(this).css("background-image").replace(".png", "-click.png");
        $(this).css("background", img);
    }, function () {
        var img = $(this).css("background-image").replace("-click.png", ".png");
        $(this).css("background", img);
    });

    var resizeTop = function () {
        var content = $("#content");
        loginBox = $(".common-box"),
            container = $(".container");

        var formHeight = loginBox.outerHeight(),
            winWidth = $(window).width(),
            winHeight = $(window).height(),
            bodyWidth = $(document.body).width();

        /*垂直*/
        if (winHeight < (formHeight + 90 + 140)) {
            $(".footer").addClass("hide");
            $("#content").css({
                "bottom": "0"
            });
            if (winHeight < (formHeight + 90)) {
                $(".top").addClass("hide");
                $("#content").css("top", "0");
            } else {
                $("#content").css("top", "90px");
                $(".top").removeClass("hide");
            }
        } else {
            $(".footer").removeClass("hide");
            $(".top").removeClass("hide");
            $("#content").css({
                "bottom": "140px",
                "top": "90px"
            })
        }

        var contentHeight = content.outerHeight();
        var contentTop = (contentHeight - formHeight - 10 * 2) / 2;

        content.css("padding-top", contentTop + "px");



        if (bodyWidth <= 1010) {
            $("#content").css({
                "min-width": "1010",
                "width": "auto"
            });
            container.width("1010");
            window.scrollTo(780 - 50, (winHeight - formHeight) / 2);
            $(".title").css("padding-left", "40px")

        } else if (bodyWidth > 1010) {
            content.css("width", "100%");
            winWidth < 1100 ? container.outerWidth("1010") :
                container.width(1010 + (bodyWidth - 1010) / 2);
        }

        if (winWidth < (loginBox.outerWidth() + 6)) {
            content.css("padding-right", "10px");
        } else {
            content.css("padding-right", "50px");
        }

    }
    resizeTop();

    $(window).resize(function () {
        resizeTop();
    });
  

});