
function sendCode(phone, expkey) {
    $.ajax({
        url: '/Login/SendCode',
        type: 'post',
        dataType: 'json',
        data: { phone: phone, expkey: expkey },
        success: function (data) {

            if (data.status == 1) {
                $("#codemsg_id").val(data.msg);
                $('#expkey').val(data.expkey);
                $("#msgcontainer").hide();
                $('#msgtip1').html("");
            }
            else {
                $('#msgcontainer').show();
                $('#msgtip1').html(data.msg);
            }
        },
        error: function () { }

    });

}

function getCheckCode() {
    var length = parseInt($("#mobile").val()).toString().length;
    
    if (length !== 11) {
        return  $.alert({
            title: '温馨提示',
            content: '请输入11位合法的<span style="color: #3c9;">手机号码</span>',
            useBootstrap:false,
            boxWidth: "300px",
            type: "green", //orange,red/error,success/green,purple,dark
            buttons: {
                ok: {
                    text: "关闭",
                    action: function () {
                        $("#mobile").focus();
                    }
                }
            }
        });
    }

    if ($("#mobile").nextAll(".error").css("display") == "block") return $("#mobile").focus();

    sendCode($("#mobile").val(), $('#expkey').val());
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
            $("#getCheckCodeBtn").on("click", getCheckCode);
            getCheckCodeBtn.html("获取验证码").css("pointer-events", "all");
            clearInterval(timer);
        } else
            getCheckCodeBtn.html("<b style='color:#3c9;'>" + time + "</b>秒内有效");

    }, 1000);
}



function OnRegisterSuccess(data) {

    if (data.status == 0) {


        $('#msgtip1').html(data.msg);
        $('#msgcontainer').show();

    }
    else {
        var jc = $.alert({
            title: "",
            content: '<div class="register-success"><i class="fa fa-check-circle"></i><p>注册成功！</p></div>',
            autoClose: 'cancelAction|3000',
            columnClass: "w300",
            type: "green",
            buttons: {
                cancelAction: {
                    text: "立即登陆",
                    btnClass: 'btn-orange',
                    action: function () {
                        window.location.href = $('#loginid').attr("href");
                    }
                }

            }
        });

    }


}
function reinitIframe(obj) {
    var iframe = obj;
    try {
        var bHeight = iframe.contentWindow.document.body.scrollHeight;
        var dHeight = iframe.contentWindow.document.documentElement.scrollHeight;
        var height = Math.max(bHeight, dHeight);
        iframe.height = height + 20;
    } catch (ex) { }
}

$(function () {

    $("#getCheckCodeBtn").on("click", getCheckCode);


    jQuery.validator.addMethod("userPattern", function (value, element, param) {
        return this.optional(element) || (/^[a-zA-Z0-9_-]+$/.test(value));
    }, $.validator.format("请输入字母，数字，下划线，减号"));

    jQuery.validator.addMethod("isMobile", function (value, element, param) {
        return this.optional(element) || (/^((13[0-9])|(14[5|7])|(15([0-9]))|(17[0-9])|(18[0-9]))\d{8}$/.test(value));
    }, $.validator.format("请输入11位合法的电话号码"));


    $("#viewProtocolBtn").on("click", function () {
        var jc = $.dialog({
            title: '查看用户协议',
            //icon: 'fa fa-plus',
            content: '<iframe width="100%"  frameborder="0" onload="reinitIframe(this)"   src="/Login/Protocol"></iframe >',
            columnClass: "w500",
            type: "green", //orange,red/error,success/green,purple,dark
            onClose: function () {
                //表单关闭
                //alert("表单关闭")
            },
            onOpen: function () {

            },
            onContentReady: function () {
                var maxHeight = $(window).height() - (jc.$jconfirmBox.outerHeight() - jc.$contentPane.outerHeight()) - (jc.offsetTop + jc.offsetBottom);

                reinitIframe($('protoFrame')[0], maxHeight);
            }
        });
    });

    $("#registerform").validate({
        rules: {
            username: {
                maxlength: 20,
                minlength: 6,
                required: true,
                userPattern: true,
                remote: {
                    url: '/Login/IsHasUserName',
                    type: "post",
                    dataType: "json",
                    data: {
                        username: function () {
                            return $("#username").val();
                        }
                    },

                    dataFilter: function (data, type) {
                        data = JSON.parse(data);
                        if (data.status == 1)
                            return true;
                        else
                            return false;
                    }

                }
            },
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

            },
            mobile: {
                rangelength: [11, 11],
                required: true,
                isMobile: true,
                remote: {
                    url: '/Login/IsHasTel',
                    type: "post",
                    dataType: "json",
                    data: {
                        mobile: function () {
                            return $("#mobile").val();
                        }
                    },

                    dataFilter: function (data, type) {
                        data = JSON.parse(data);
                        if (data.status == 1)
                            return true;
                        else
                            return false;
                    }

                }
            },
            checkCode: {
                required: true,
                rangelength: [6, 6]
            }
        },
        messages: {
            username: {
                maxlength: "用户名最多20位",
                minlength: "用户名最少6字符",
                required: "用户名必填",
                userPattern: "请输入字母，数字，下划线，减号",
                remote: "该用户名已存在"

            },

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

            },
            mobile: {
                rangelength: "请输入11位手机号",
                required: "请输入手机号",
                isMobile: "请输入11位合法的电话号码",
                remote: "该手机号已被注册"

            },
            checkCode: {
                required: "请输入验证码",
                rangelength: "请输入6位验证码"
            }
        }

    });
    $("#leftImg").height($(".login-box").outerHeight());
    document.onkeydown = function (e) {
        var ev = document.all ? window.event : e;
        if (ev.keyCode == 13) {
            $('#registerform').submit();
        }
    }

    $("#protocol").on("click", function () {
        $(this).is(':checked') ? $(".login-btn").removeAttr("disabled") : $(".login-btn").attr("disabled", "disabled")
    })

    //获取焦点
    $("input[type='text'],input[type='password']").on("focus", function () {
        var prev = $(this).prev("span").eq(0);
        var img = prev.css("background-image").replace(".png", "-click.png");
        prev.css("background-image", img);
    });
    //失去焦点
    $("input[type='text'],input[type='password']").on("blur", function () {
        var prev = $(this).prev("span").eq(0);
        var img = prev.css("background-image").replace("-click.png", ".png");
        prev.css("background-image", img);
    });
    //输入时
    $("input[type='text'],input[type='password']").on("input", function () {
        var i = $(this).next("i").eq(0);
        if ($(this).val() !== "") {
            i.removeClass("hide");
        } else {
            i.addClass("hide");
        }
    });

    // 清除input
    $(".del-btn").on("click", function () {
        $(this).addClass("hide");
        $(this).prev("input").eq(0).val("");
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
        loginBox = $(".login-box"),
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