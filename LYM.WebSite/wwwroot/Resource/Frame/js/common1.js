! function ($) {

    /*
    **  
    **  功能：时间选择器
    **  参数：option，参见daterangepicker的option设置。
    **  参数：callback，回调函数
    **  eg ： $('input').daterangepickers({},function(){});
    */

    $.fn.extend({
        daterangepickers: function (options, callback) {
            var locale = {
                format: "YYYY-MM-DD HH:mm:ss",  //控件中from和to 显示的日期格式  
                separator: " 到 ",
                applyLabel: "确定",
                cancelLabel: "取消",
                clearLabel: '清空',
                fromLabel: "开始",
                toLabel: "结束",
                customRangeLabel: "自定义",
                weekLabel: "W",
                daysOfWeek: ["日", "一", "二", "三", "四", "五", "六"],
                monthNames: ["一月", "二月", "三月", "四月", "五月", "六月", "七月", "八月", "九月", "十月", "十一月", "十二月 "],
                firstDay: 1
            }

            var ranges = {//自制快捷键
                '今天': [moment(), moment()],
                '昨天': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
                '最近7天': [moment().subtract(6, 'days'), moment()],
                '最近30天': [moment().subtract(29, 'days'), moment()],
                '这个月': [moment().startOf('month'), moment().endOf('month')],
                '上个月': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')]
            }

            options = options || {};

            options.ranges = !options.singleDatePicker ?
                    $.extend({}, options.ranges, ranges) : options.ranges;

            options.locale = options.locale ?
                function (_locale, locale) {
                    $.each(locale, function (i, n) {
                        _locale[i] = _locale[i] || n;
                    });
                    return _locale;
                }(options.locale, locale)
                : locale;

            return $(this).daterangepicker(options, callback);
        }
    })


    /*
    **  
    **  功能：自适应字体大小
    **  参数：min，最小的font-size，单位像素。
    **  参数：max，最大的font-size，单位像素。
    **  参数：mid，范围缓冲区，设置在60-70之间效果最佳。值越小产生的字体越大，反之字体越小，可以根据实际情况来调整这个参数。
    **  eg ： $('body').fontFlex(14, 20, 70);
    */

    $.fn.fontFlex = function (min, max, mid) {
        var $this = this;

        $(window).resize(function () {

            var size = window.innerWidth / mid;

            if (size < min) size = min;
            if (size > max) size = max;

            $this.css('font-size', size + 'px');

        }).trigger('resize');
    };

    if ($.fn.iCheck) {
        $('input').iCheck({
            checkboxClass: 'icheckbox_flat-green',
            radioClass: 'iradio_flat-green',
            //increaseArea: '20%' // optional
        });
    }



}(jQuery);


//将form中的值转换为键值对。
function getFormJson(frm) {
    var o = {};
    var a = $(frm).serializeArray();
    $.each(a, function () {
        if (o[this.name] !== undefined) {
            if (!o[this.name].push) {
                o[this.name] = [o[this.name]];
            }
            o[this.name].push(this.value || '');
        } else {
            o[this.name] = this.value || '';
        }
    });

    return o;
}




/* 
** 拖动滚动条
*/

(function (root, factory) {
    if (typeof define === 'function' && define.amd) {
        define(['exports'], factory);
    } else if (typeof exports !== 'undefined') {
        factory(exports);
    } else {
        factory((root.dragscroll = {}));
    }
}(this, function (exports) {
    var _window = window;
    var mousemove = 'mousemove';
    var mouseup = 'mouseup';
    var mousedown = 'mousedown';
    var addEventListener = 'addEventListener';
    var removeEventListener = 'removeEventListener';

    var dragged = [];
    var reset = function (i, el) {
        for (i = 0; i < dragged.length;) {
            el = dragged[i++];
            el[removeEventListener](mousedown, el.md, 0);
            _window[removeEventListener](mouseup, el.mu, 0);
            _window[removeEventListener](mousemove, el.mm, 0);
        }

        dragged = document.getElementsByClassName('dragscroll');
        for (i = 0; i < dragged.length;) {
            (function (el, lastClientX, lastClientY, pushed) {
                el[addEventListener](
                    mousedown,
                    el.md = function (e) {
                        pushed = 1;
                        lastClientX = e.clientX;
                        lastClientY = e.clientY;

                        e.preventDefault();
                        e.stopPropagation();
                    }, 0
                );

                _window[addEventListener](
                    mouseup, el.mu = function () { pushed = 0; }, 0
                );

                _window[addEventListener](
                    mousemove,
                    el.mm = function (e, scroller) {
                        scroller = el.scroller || el;
                        if (pushed) {
                            scroller.scrollLeft -=
                                (-lastClientX + (lastClientX = e.clientX));
                            scroller.scrollTop -=
                                (-lastClientY + (lastClientY = e.clientY));
                        }
                    }, 0
                );
            })(dragged[i++]);
        }
    }


    if (document.readyState == "complete") {
        reset();
    } else {
        _window[addEventListener]("load", reset, 0);
    }

    exports.reset = reset;
}));



/*
** 函数：checkAll(this)
** 功能：全选功能
*/

function checkAll(obj) {
    var parent = $(obj).parent();
    if (!parent.hasClass("select-muliple")) return;

    var siblings = parent.siblings();
    console.log(parent.hasClass("selected"))
    if (parent.hasClass("selected")) {
        siblings.removeClass("selected");
    } else {
        siblings.addClass("selected");
    }
    parent.toggleClass("selected");
}

/*
** 函数：checkSelf(obj)
** 功能：多选
** 参数：obj为this
** 例：onclick="checkSelf(this)"
*/

function checkSelf(obj) {
    var parent = $(obj).parent();
    parent.toggleClass("selected");

    if ($(".select-muliple").length !== 0) {//全选功能存在
        var size = $(".selected:not('.select-muliple')").length;
        var totle = parent.siblings("tr").length;
        var selectAll = $(".select-muliple");
        if (size === totle) {
            selectAll.addClass("selected");
        }
        else {
            selectAll.removeClass("selected");
        }
    } else {
        parent.siblings().removeClass("selected");
    }

}



/*
** 功能：表单验证
*/

if ($.validator) {
    $.validator.setDefaults({
        highlight: function (e) {
            $(e).closest(".display_flex").removeClass("has-success").addClass("has-error")
        },
        success: function (e, element) {
            console.log(element)
            $(element).is(':radio') || $(element).is(':checkbox') ?
            e.html("").addClass("has-secceed") :
            e.html("Ok!").addClass("has-secceed")

            e.nextAll(".tip-info").addClass("hide");
            // e.html("Ok!").addClass("has-secceed");

            //e.closest(".display_flex").removeClass("has-error").addClass("has-success")
        },
        errorElement: "span",
        errorPlacement: function (error, element) {
            error.prepend("<i class='fa fa-times-circle'></i>&nbsp;")
            if (element.is(':radio') || element.is(':checkbox')) { //如果是radio或checkbox
                var eid = element.attr('name'); //获取元素的name属性

                element.parent().hasClass("icheckbox_flat-green") ||
                element.parent().hasClass("iradio_flat-green") ?
                error.appendTo(element.parent().parent()) :
                error.appendTo(element.parent()); //将错误信息添加当前元素的父结点后面
            } else {
                console.log(error, element)

                error.insertAfter(element);
            }
            element.nextAll(".tip-info").addClass("hide")
        },
        errorClass: "validate-error",
        validClass: "has-success",

    });
}


/*
** 函数：reinitIframe(obj)
** 功能：重置iframe高度
** 参数：obj为this
** 例：onclick="reinitIframe(this)"
*/
function reinitIframe(obj) {
    var iframe = obj;
    try {
        var bHeight = iframe.contentWindow.document.body.scrollHeight;
        var dHeight = iframe.contentWindow.document.documentElement.scrollHeight;
        var height = Math.max(bHeight, dHeight);
        iframe.height = height + 20;
    } catch (ex) { }
}

if (typeof (toastr) === "object") {
    toastr.options = {
        closeButton: true,
        debug: false,
        progressBar: false,
        positionClass: "toast-top-right",
        onclick: null,
        showDuration: "300",
        hideDuration: "1000",
        timeOut: "5000",
        extendedTimeOut: "1000",
        showEasing: "swing",
        hideEasing: "linear",
        showMethod: "fadeIn",
        hideMethod: "fadeOut"
    };
}

/*
**  
**  功能：上传图片
**  参数：selector，
**  参数：filePicker，
**  参数：fileNum，最多上传图片二数
**  eg ： uploadPhoto("#uploader", "#filePicker", 1);//页面加载后使用
*/



function uploadPhoto(selector, filePicker, fileNum,lable) {
    fileNum = fileNum || 1; // 默认只能上传一张

    function e(e) {
        var a = o('<li id="' + e.id + '"><p class="title">' + e.name + '</p><p class="imgWrap"></p><p class="progress"><span></span></p></li>'),
            s = o('<div class="file-panel"><span class="cancel">删除</span><span class="rotateRight">向右旋转</span><span class="rotateLeft">向左旋转</span></div>').appendTo(a),
            i = a.find("p.progress span"),
            t = a.find("p.imgWrap"),
            r = o('<p class="error"></p>'),
            d = function (e) {
                switch (e) {
                    case "exceed_size":
                        text = "文件大小超出";
                        break;
                    case "interrupt":
                        text = "上传暂停";
                        break;
                    default:
                        text = "上传失败，请重试"
                }
                r.text(text).appendTo(a)
            };
        "invalid" === e.getStatus() ? d(e.statusText) : (t.text("预览中"), n.makeThumb(e, function (e, a) {
            if (e) return void t.text("不能预览");
            var s = o('<img src="' + a + '">');
            t.empty().append(s)
        }, v, b), w[e.id] = [e.size, 0], e.rotation = 0), e.on("statuschange", function (t, n) { "progress" === n ? i.hide().width(0) : "queued" === n && (a.off("mouseenter mouseleave"), s.remove()), "error" === t || "invalid" === t ? (console.log(e.statusText), d(e.statusText), w[e.id][1] = 1) : "interrupt" === t ? d("interrupt") : "queued" === t ? w[e.id][1] = 0 : "progress" === t ? (r.remove(), i.css("display", "block")) : "complete" === t && a.append('<span class="success"></span>'), a.removeClass("state-" + n).addClass("state-" + t) }), a.on("mouseenter", function () { s.stop().animate({ height: 30 }) }), a.on("mouseleave", function () { s.stop().animate({ height: 0 }) }), s.on("click", "span", function () {
            var a, s = o(this).index();
            switch (s) {
                case 0:
                    return void n.removeFile(e);
                case 1:
                    e.rotation += 90;
                    break;
                case 2:
                    e.rotation -= 90
            }
            x ? (a = "rotate(" + e.rotation + "deg)", t.css({ "-webkit-transform": a, "-mos-transform": a, "-o-transform": a, transform: a })) : t.css("filter", "progid:DXImageTransform.Microsoft.BasicImage(rotation=" + ~~(e.rotation / 90 % 4 + 4) % 4 + ")")
        }), a.appendTo(l)
    }

    function a(e) {
        var a = o("#" + e.id);
        delete w[e.id], s(), a.off().find(".file-panel").off().end().remove()
    }

    function s() {
        var e, a = 0,
            s = 0,
            t = f.children();
        o.each(w, function (e, i) { s += i[0], a += i[0] * i[1] }), e = s ? a / s : 0, t.eq(0).text(Math.round(100 * e) + "%"), t.eq(1).css("width", Math.round(100 * e) + "%"), i()
    }

    function i() {
        m == fileNum ? $(filePicker + "2").addClass("hide") : $(filePicker + "2").removeClass("hide");
        var e, a = "";
        "ready" === k ? a = "选中" + m + "张图片，共" + WebUploader.formatSize(h) + "。" :
            "confirm" === k ?
            (e = n.getStats(),
                e.uploadFailNum && (a = "已成功上传" + e.successNum + "张照片至XX相册，" + e.uploadFailNum + '张照片上传失败，<a class="retry" href="#">重新上传</a>失败图片或<a class="ignore" href="#">忽略</a>')) :
            (e = n.getStats(), a = "共" + m + "张（" + WebUploader.formatSize(h) + "），已上传" + e.successNum + "张", e.uploadFailNum && (a += "，失败" + e.uploadFailNum + "张")), p.html(a)
    }

    function t(e) {
        var a;
        if (e !== k) {
            switch (c.removeClass("state-" + k),
                c.addClass("state-" + e), k = e) {
                case "pedding":
                    u.removeClass("element-invisible"),
                        l.parent().removeClass("filled"),
                        l.hide(),
                        d.addClass("element-invisible"), n.refresh();
                    break;
                case "ready":
                    u.addClass("element-invisible"),
                    o(filePicker + "2").removeClass("element-invisible"), l.parent().addClass("filled"), l.show(), d.removeClass("element-invisible"), n.refresh();
                    break;
                case "uploading":
                    o(filePicker + "2").addClass("element-invisible"),
                            f.show(), c.text("暂停上传");
                    break;
                case "paused":
                    f.show(), c.text("继续上传");
                    break;
                case "confirm":
                    if (f.hide(), c.text("开始上传").addClass("disabled"),
                        a = n.getStats(),
                        a.successNum && !a.uploadFailNum) return void t("finish");
                    break;
                case "finish":
                    a = n.getStats(), a.successNum ? '' :
                        (k = "done", location.reload())
            }
            i()
        }
    }
    var n, o = jQuery,
        r = o(selector),
        l = o('<ul class="filelist"></ul>').appendTo(r.find(".queueList")),
        d = r.find(".statusBar"),
        p = d.find(".info"),
        c = r.find(".uploadBtn"),
        u = r.find(".placeholder"),
        f = d.find(".progress").hide(),
        m = 0,
        h = 0,
        g = window.devicePixelRatio || 1,
        v = 110 * g,
        b = 110 * g,
        k = "pedding",
        w = {},
        x = function () {
            var e = document.createElement("p").style,
                a = "transition" in e || "WebkitTransition" in e || "MozTransition" in e || "msTransition" in e || "OTransition" in e;
            return e = null, a
        }();
    if (!WebUploader.Uploader.support()) throw alert("Web Uploader 不支持您的浏览器！如果你使用的是IE浏览器，请尝试升级 flash 播放器"), new Error("WebUploader does not support the browser you are using.");
    n = WebUploader.create({
        // 选完文件后，是否自动上传。
        auto: true,
        pick: { id: filePicker, label: lable||"请选择图片" },
        dnd: selector + " .queueList",
        paste: document.body,
        accept: { title: "Images", extensions: "gif,jpg,jpeg,bmp,png", mimeTypes: "image/*" },
        swf: "~/Resource/UploadTool/Uploader.swf",
        //swf: "192.168.0.8080" + "/Uploader.swf",
        disableGlobalDnd: !0,
        chunked: !0,
        server: "/Physical/EmploymentRegister/UpLoadProcess",
        fileNumLimit: fileNum,
        fileSizeLimit: 5242880,
        fileSingleSizeLimit: 1048576
    }), n.addButton({ id: filePicker + "2", label: "继续添加" }), n.onUploadProgress = function (e, a) {
        var i = o("#" + e.id),
            t = i.find(".progress span");
        t.css("width", 100 * a + "%"), w[e.id][1] = a, s()
    }, n.onFileQueued = function (a) { m++, h += a.size, 1 === m && (u.addClass("element-invisible"), d.show()), e(a), t("ready"), s() }, n.onFileDequeued = function (e) { m--, h -= e.size, m || t("pedding"), a(e), s() }, n.on("all", function (e) {
        switch (e) {
            case "uploadFinished":
                t("confirm");
                break;
            case "startUpload":
                t("uploading");
                break;
            case "stopUpload":
                t("paused")
        }
    }), n.onError = function (e) { alert("Eroor: " + e) },
    //自定义参数
    n.option('formData', {
        key: filePicker,
    }),
        c.on("click", function () {
            
            return o(this).hasClass("disabled") ? !1 :
                void ("ready" === k ? n.upload() : "paused" === k ? n.upload() :
                    "uploading" === k && n.stop())
        }), p.on("click", ".retry", function () {
            n.retry()
        }), p.on("click", ".ignore", function () { alert("todo") }),
        c.addClass("state-" + k), s()

}



