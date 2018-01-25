




$(function () {



   

    $("#yzmimg").bind('click', function () {

        $.ajax({
            url: '/Login/ReloadYZM?t=' + Math.random(),
            type: 'get',
            async: false,
            success: function (data) {
                $('#yzmimg').attr("src", "/Login/yzmimage?code=" + data.code);
                $("#code").val(data.code);


            }
        });


    });
    //$("#loginform").validate({
    //    rules: {
    //        username: {
    //            maxlength: 16,
    //            minlength: 4,
    //            required: true
    //        },
    //        password: {
    //            maxlength: 16,
    //            minlength: 6,
    //            required: true
    //        }
    //    },
    //    messages: {
    //        username: {
    //            maxlength: "用户名最多16位",
    //            minlength: "用户名最少4字符",
    //            required: "用户名必填"
    //        },
    //        password: {
    //            maxlength: "密码最多16个字符",
    //            minlength: "密码最少6个字符",
    //            required: "请输入密码"
    //        }
    //    }

    //});


    $("#leftImg").height($(".login-box").outerHeight());
    document.onkeydown = function (e) {
        var ev = document.all ? window.event : e;
        if (ev.keyCode == 13) {
            $('.login-btn').trigger("click");
        }
    }

    //获取焦点
    $("input[type='text'],input[type='password']").on("focus", function () {
        var prev = $(this).prevAll("span").eq(0);
        var img = prev.css("background-image").replace(".png", "-click.png");
        prev.css("background-image", img);
    });
    //失去焦点
    $("input[type='text'],input[type='password']").on("blur", function () {
        var prev = $(this).prevAll("span").eq(0);
        var img = prev.css("background-image").replace("-click.png", ".png");
        prev.css("background-image", img);
    });
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

        content.css({
            "padding-top": contentTop + "px",
            "min-height": formHeight + 10 + "px"
        })

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


